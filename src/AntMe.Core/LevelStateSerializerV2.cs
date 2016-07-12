using System;
using System.IO;

namespace AntMe
{
    internal sealed class LevelStateSerializerV2 : ILevelStateSerializer
    {
        private const byte VERSION = 2;

        private SimulationContext context;

        private bool keyframeSent = false;

        public LevelStateSerializerV2(SimulationContext context)
        {
            this.context = context;
        }

        public void Serialize(BinaryWriter writer, LevelState state)
        {
            // keyframe flag
            writer.Write(!keyframeSent);

            if (!keyframeSent)
            {
                // First Frame
                keyframeSent = true;

                // Base Level
                SerializeFirst(writer, state);

                // Base Map
                SerializeFirst(writer, state.Map);
            }
            else
            {
                // Following Frames

                // Base Level
                SerializeUpdate(writer, state);

                // Base Map
                SerializeUpdate(writer, state.Map);
            }
        }

        private void SerializeFirst(BinaryWriter writer, ISerializableState state)
        {
            using (MemoryStream mem = new MemoryStream())
            {
                using (BinaryWriter memWriter = new BinaryWriter(mem))
                {
                    // Serialize
                    state.SerializeFirst(memWriter, VERSION);

                    // Copy to Buffer
                    byte[] buffer = mem.GetBuffer();
                    ushort length = (ushort)mem.Position;

                    // Write to main Stream
                    writer.Write(length);
                    writer.Write(buffer, 0, length);
                }
            }
        }

        private void SerializeUpdate(BinaryWriter writer, ISerializableState state)
        {
            using (MemoryStream mem = new MemoryStream())
            {
                using (BinaryWriter memWriter = new BinaryWriter(mem))
                {
                    // Serialize
                    state.SerializeFirst(memWriter, VERSION);

                    // Copy to Buffer
                    byte[] buffer = mem.GetBuffer();
                    ushort length = (ushort)mem.Position;

                    // Write to main Stream
                    writer.Write(length);
                    writer.Write(buffer, 0, length);
                }
            }
        }

        public void Dispose()
        {
        }
    }
}
