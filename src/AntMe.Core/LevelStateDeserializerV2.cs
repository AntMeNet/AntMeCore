using System;
using System.IO;

namespace AntMe
{
    internal sealed class LevelStateDeserializerV2 : ILevelStateDeserializer
    {
        private const byte VERSION = 2;

        private SimulationContext context;

        private LevelState latest;

        public LevelStateDeserializerV2(SimulationContext context)
        {
            this.context = context;
        }

        public LevelState Deserialize(BinaryReader reader)
        {
            bool keyframe = reader.ReadBoolean();

            if (keyframe)
            {
                // Base Level
                latest = new LevelState();
                DeserializeFirst(reader, latest);

                // Level State Properties

                // Base Map
                latest.Map = new MapState();
                DeserializeFirst(reader, latest.Map);

                // Map Properties
            }
            else
            {
                // Base Level
                DeserializeUpdate(reader, latest);

                // Base Map
                DeserializeUpdate(reader, latest.Map);
            }

            return latest;
        }

        private void DeserializeFirst(BinaryReader reader, ISerializableState state)
        {
            int dumpCount = reader.ReadUInt16();
            byte[] dump = reader.ReadBytes(dumpCount);

            using (MemoryStream mem = new MemoryStream(dump))
            {
                using (BinaryReader memReader = new BinaryReader(mem))
                {
                    state.DeserializeFirst(memReader, VERSION);
                }
            }
        }

        private void DeserializeUpdate(BinaryReader reader, ISerializableState state)
        {
            int dumpCount = reader.ReadUInt16();
            byte[] dump = reader.ReadBytes(dumpCount);

            using (MemoryStream mem = new MemoryStream(dump))
            {
                using (BinaryReader memReader = new BinaryReader(mem))
                {
                    state.DeserializeUpdate(memReader, VERSION);
                }
            }
        }

        public void Dispose()
        {
        }
    }
}
