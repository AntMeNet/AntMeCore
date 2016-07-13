using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AntMe.Serialization
{
    internal sealed class LevelStateSerializerV2 : ILevelStateSerializer
    {
        private const byte VERSION = 2;

        private SimulationContext context;

        private bool keyframeSent = false;

        private List<Type> levelProperties = new List<Type>();
        private List<Type> mapProperties = new List<Type>();

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
                // Cleanup Caches
                levelProperties.Clear();
                mapProperties.Clear();

                // First Frame
                keyframeSent = true;

                // Base Level
                SerializeFirst(writer, state);

                // Level Properties
                writer.Write((byte)state.Properties.Count());
                foreach (var property in state.Properties)
                {
                    Type propertyType = property.GetType();
                    levelProperties.Add(propertyType);
                    writer.Write(propertyType.FullName);
                    SerializeFirst(writer, property);
                }

                // Base Map
                SerializeFirst(writer, state.Map);

                // Map Properties
                writer.Write((byte)state.Map.Properties.Count());
                foreach (var property in state.Map.Properties)
                {
                    Type propertyType = property.GetType();
                    mapProperties.Add(propertyType);
                    writer.Write(propertyType.FullName);
                    SerializeFirst(writer, property);
                }
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
