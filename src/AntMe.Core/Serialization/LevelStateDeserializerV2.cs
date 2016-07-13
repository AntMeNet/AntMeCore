using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AntMe.Serialization
{
    internal sealed class LevelStateDeserializerV2 : ILevelStateDeserializer
    {
        private const byte VERSION = 2;

        private SimulationContext context;

        private LevelState latest;

        private List<Type> levelProperties = new List<Type>();
        private List<Type> mapProperties = new List<Type>();

        public LevelStateDeserializerV2(SimulationContext context)
        {
            this.context = context;
        }

        public LevelState Deserialize(BinaryReader reader)
        {
            bool keyframe = reader.ReadBoolean();

            if (keyframe)
                DeserializeKeyFrame(reader);
            else
                DeserializeUpdateFrame(reader);


            return latest;
        }

        private void DeserializeKeyFrame(BinaryReader reader)
        {
            // Cleanup Caches
            latest = null;
            levelProperties.Clear();
            mapProperties.Clear();

            // Base Level
            latest = new LevelState();
            DeserializeFirst(reader, latest);

            // Level State Properties
            byte levelPropertyCount = reader.ReadByte();
            for (byte i = 0; i < levelPropertyCount; i++)
            {
                string name = reader.ReadString();

                // Identify Type
                var map = context.Mapper.LevelProperties.
                    Where(m => m.StateType != null).
                    FirstOrDefault(m => m.StateType.FullName.Equals(name));

                if (map != null)
                {
                    // Known property
                    LevelStateProperty property = Activator.CreateInstance(map.StateType) as LevelStateProperty;
                    latest.AddProperty(property);
                    levelProperties.Add(map.StateType);
                    DeserializeFirst(reader, property);
                }
                else
                {
                    // Unknown Property
                    levelProperties.Add(null);
                    DeserializeUnknown(reader);
                }
            }

            // Base Map
            latest.Map = new MapState();
            DeserializeFirst(reader, latest.Map);

            // Map Properties
            byte mapPropertyCount = reader.ReadByte();
            for (byte i = 0; i < mapPropertyCount; i++)
            {
                string name = reader.ReadString();

                // Identify Type
                var map = context.Mapper.MapProperties.
                    Where(m => m.StateType != null).
                    FirstOrDefault(m => m.StateType.FullName.Equals(name));

                if (map != null)
                {
                    // Known property
                    MapStateProperty property = Activator.CreateInstance(map.StateType) as MapStateProperty;
                    latest.Map.AddProperty(property);
                    mapProperties.Add(map.StateType);
                    DeserializeFirst(reader, property);
                }
                else
                {
                    // Unknown Property
                    levelProperties.Add(null);
                    DeserializeUnknown(reader);
                }
            }
        }

        private void DeserializeUpdateFrame(BinaryReader reader)
        {
            // Base Level
            DeserializeUpdate(reader, latest);

            // Base Map
            DeserializeUpdate(reader, latest.Map);
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

        private void DeserializeUnknown(BinaryReader reader)
        {
            int dumpCount = reader.ReadUInt16();
            byte[] dump = reader.ReadBytes(dumpCount);
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
