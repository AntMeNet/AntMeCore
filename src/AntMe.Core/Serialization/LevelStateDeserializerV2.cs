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

        private List<Type> levelPropertyTypes = new List<Type>();
        private List<Type> mapPropertyTypes = new List<Type>();
        private List<Type> mapTileTypes = new List<Type>();
        private List<Type> mapTilePropertyTypes = new List<Type>();
        private List<Type> materialTypes = new List<Type>();
        private List<Type> factionTypes = new List<Type>();
        private List<Type> factionPropertyTypes = new List<Type>();
        private List<Type> itemTypes = new List<Type>();
        private List<Type> itemPropertyTypes = new List<Type>();

        public LevelStateDeserializerV2(SimulationContext context)
        {
            this.context = context;
        }

        public LevelState Deserialize(BinaryReader reader)
        {
            bool keyframe = reader.ReadBoolean();

            // In case of a Keyframe Delete known Stuff
            if (keyframe) DoKeyframe();

            LevelStateSerializerPackageV2 next = (LevelStateSerializerPackageV2)reader.ReadByte();
            while (next != LevelStateSerializerPackageV2.FrameEnd)
            {
                switch (next)
                {
                    case LevelStateSerializerPackageV2.LevelInsert: DoLevelInsert(reader); break;
                    case LevelStateSerializerPackageV2.LevelUpdate: DoLevelUpdate(reader); break;
                    case LevelStateSerializerPackageV2.LevelPropertyInsert: DoLevelPropertyInsert(reader); break;
                    case LevelStateSerializerPackageV2.LevelPropertyUpdate: DoLevelPropertyUpdate(reader); break;
                    case LevelStateSerializerPackageV2.LevelPropertyTypeInsert: DoLevelPropertyTypeInsert(reader); break;
                    case LevelStateSerializerPackageV2.MapInsert: DoMapInsert(reader); break;
                    case LevelStateSerializerPackageV2.MapUpdate: DoMapUpdate(reader); break;
                    case LevelStateSerializerPackageV2.MapPropertyInsert: DoMapPropertyInsert(reader); break;
                    case LevelStateSerializerPackageV2.MapPropertyUpdate: DoMapPropertyUpdate(reader); break;
                    case LevelStateSerializerPackageV2.MapPropertyTypeInsert: DoMapPropertyTypeInsert(reader); break;
                    case LevelStateSerializerPackageV2.MapTileInsert: DoMapTileInsert(reader); break;
                    case LevelStateSerializerPackageV2.MapTileUpdate: DoMapTileUpdate(reader); break;
                    case LevelStateSerializerPackageV2.MapTileTypeInsert: DoMapTileTypeInsert(reader); break;
                    case LevelStateSerializerPackageV2.MapTilePropertyInsert: DoMapTilePropertyInsert(reader); break;
                    case LevelStateSerializerPackageV2.MapTilePropertyUpdate: DoMapTilePropertyUpdate(reader); break;
                    case LevelStateSerializerPackageV2.MapTilePropertyTypeInsert: DoMapTilePropertyTypeInsert(reader); break;
                    case LevelStateSerializerPackageV2.MaterialInsert: DoMaterialInsert(reader); break;
                    case LevelStateSerializerPackageV2.MaterialUpdate: DoMaterialUpdate(reader); break;
                    case LevelStateSerializerPackageV2.MaterialTypeInsert: DoMaterialTypeInsert(reader); break;
                    case LevelStateSerializerPackageV2.FactionInsert: DoFactionInsert(reader); break;
                    case LevelStateSerializerPackageV2.FactionUpdate: DoFactionUpdate(reader); break;
                    case LevelStateSerializerPackageV2.FactionTypeInsert: DoFactionTypeInsert(reader); break;
                    case LevelStateSerializerPackageV2.FactionPropertyInsert: DoFactionPropertyInsert(reader); break;
                    case LevelStateSerializerPackageV2.FactionPropertyUpdate: DoFactionPropertyUpdate(reader); break;
                    case LevelStateSerializerPackageV2.FactionPropertyTypeInsert: DoFactionPropertyTypeInsert(reader); break;
                    case LevelStateSerializerPackageV2.ItemInsert: DoItemInsert(reader); break;
                    case LevelStateSerializerPackageV2.ItemUpdate: DoItemUpdate(reader); break;
                    case LevelStateSerializerPackageV2.ItemDelete: DoItemDelete(reader); break;
                    case LevelStateSerializerPackageV2.ItemTypeInsert: DoItemTypeInsert(reader); break;
                    case LevelStateSerializerPackageV2.ItemPropertyInsert: DoItemPropertyInsert(reader); break;
                    case LevelStateSerializerPackageV2.ItemPropertyUpdate: DoItemPropertyUpdate(reader); break;
                    case LevelStateSerializerPackageV2.ItemPropertyTypeInsert: DoItemPropertyTypeInsert(reader); break;
                    case LevelStateSerializerPackageV2.FactionItemInsert: DoFactionItemInsert(reader); break;
                    default: throw new NotSupportedException("Invalid Package Type");
                }

                next = (LevelStateSerializerPackageV2)reader.ReadByte();
            }

            return latest;
        }

        private void DoKeyframe()
        {
            // Cleanup Caches
            latest = null;

            levelPropertyTypes.Clear();
            mapPropertyTypes.Clear();
            mapTileTypes.Clear();
            mapTilePropertyTypes.Clear();
            materialTypes.Clear();
            factionTypes.Clear();
            factionPropertyTypes.Clear();
            itemTypes.Clear();
            itemPropertyTypes.Clear();
        }

        private void DoFactionItemInsert(BinaryReader reader)
        {
            throw new NotImplementedException();
        }

        private void DoItemPropertyTypeInsert(BinaryReader reader)
        {
            ushort index = reader.ReadUInt16();
            string name = reader.ReadString();

            // Make sure its the next index in line
            if (index != itemPropertyTypes.Count)
                throw new NotSupportedException("New Type Index does not match the current List");

            // Identify Type
            var map = context.Mapper.ItemProperties.
                Where(m => m.StateType != null).
                FirstOrDefault(m => m.StateType.FullName.Equals(name));

            // Insert into List
            if (map != null) itemPropertyTypes.Add(map.StateType);
            else itemPropertyTypes.Add(null);
        }

        private void DoItemPropertyUpdate(BinaryReader reader)
        {
            throw new NotImplementedException();
        }

        private void DoItemPropertyInsert(BinaryReader reader)
        {
            throw new NotImplementedException();
        }

        private void DoItemTypeInsert(BinaryReader reader)
        {
            ushort index = reader.ReadUInt16();
            string name = reader.ReadString();

            // Make sure its the next index in line
            if (index != itemTypes.Count)
                throw new NotSupportedException("New Type Index does not match the current List");

            // Identify Type
            var map = context.Mapper.Items.
                Where(m => m.StateType != null).
                FirstOrDefault(m => m.StateType.FullName.Equals(name));

            // Insert into List
            if (map != null) itemTypes.Add(map.StateType);
            else itemTypes.Add(null);
        }

        private void DoItemDelete(BinaryReader reader)
        {
            throw new NotImplementedException();
        }

        private void DoItemUpdate(BinaryReader reader)
        {
            throw new NotImplementedException();
        }

        private void DoItemInsert(BinaryReader reader)
        {
            throw new NotImplementedException();
        }

        private void DoFactionPropertyTypeInsert(BinaryReader reader)
        {
            ushort index = reader.ReadUInt16();
            string name = reader.ReadString();

            // Make sure its the next index in line
            if (index != factionPropertyTypes.Count)
                throw new NotSupportedException("New Type Index does not match the current List");

            // Identify Type
            var map = context.Mapper.FactionProperties.
                Where(m => m.StateType != null).
                FirstOrDefault(m => m.StateType.FullName.Equals(name));

            // Insert into List
            if (map != null) factionPropertyTypes.Add(map.StateType);
            else factionPropertyTypes.Add(null);
        }

        private void DoFactionPropertyUpdate(BinaryReader reader)
        {
            throw new NotImplementedException();
        }

        private void DoFactionPropertyInsert(BinaryReader reader)
        {
            throw new NotImplementedException();
        }

        private void DoFactionTypeInsert(BinaryReader reader)
        {
            ushort index = reader.ReadUInt16();
            string name = reader.ReadString();

            // Make sure its the next index in line
            if (index != factionTypes.Count)
                throw new NotSupportedException("New Type Index does not match the current List");

            // Identify Type
            var map = context.Mapper.Factions.
                Where(m => m.StateType != null).
                FirstOrDefault(m => m.StateType.FullName.Equals(name));

            // Insert into List
            if (map != null) factionTypes.Add(map.StateType);
            else factionTypes.Add(null);
        }

        private void DoFactionUpdate(BinaryReader reader)
        {
            throw new NotImplementedException();
        }

        private void DoFactionInsert(BinaryReader reader)
        {
            throw new NotImplementedException();
        }

        private void DoMaterialTypeInsert(BinaryReader reader)
        {
            ushort index = reader.ReadUInt16();
            string name = reader.ReadString();

            // Identify Type
            var map = context.Mapper.MapMaterials.
                Where(m => m.Type != null).
                FirstOrDefault(m => m.Type.FullName.Equals(name));

            // Insert into List
            if (map != null) materialTypes.Add(map.Type);
            else materialTypes.Add(null);
        }

        private void DoMaterialUpdate(BinaryReader reader)
        {
            byte x = reader.ReadByte();
            byte y = reader.ReadByte();

            MapMaterial material = latest.Map.Tiles[x, y].Material;
            DeserializeUpdate(reader, material);
        }

        private void DoMaterialInsert(BinaryReader reader)
        {
            byte x = reader.ReadByte();
            byte y = reader.ReadByte();
            ushort typeIndex = reader.ReadUInt16();

            Type type = materialTypes[typeIndex];
            MapMaterial material;
            if (type != null) material = Activator.CreateInstance(type, context) as MapMaterial;
            else material = new UnknownMaterial(context);

            DeserializeFirst(reader, material);
            latest.Map.Tiles[x, y].Material = material;
        }

        private void DoMapTilePropertyTypeInsert(BinaryReader reader)
        {
            ushort index = reader.ReadUInt16();
            string name = reader.ReadString();

            // Make sure its the next index in line
            if (index != mapTilePropertyTypes.Count)
                throw new NotSupportedException("New Type Index does not match the current List");

            // Identify Type
            var map = context.Mapper.MapTileProperties.
                Where(m => m.StateType != null).
                FirstOrDefault(m => m.StateType.FullName.Equals(name));

            // Insert into List
            if (map != null) mapTilePropertyTypes.Add(map.StateType);
            else mapTilePropertyTypes.Add(null);
        }

        private void DoMapTilePropertyUpdate(BinaryReader reader)
        {
            byte x = reader.ReadByte();
            byte y = reader.ReadByte();
            ushort typeIndex = reader.ReadUInt16();

            Type type = mapTilePropertyTypes[typeIndex];
            MapTileStateProperty property = null;
            if (type != null)
                property = latest.Map.Tiles[x, y].GetProperty(type);

            if (property != null) DeserializeUpdate(reader, property);
            else DeserializeUnknown(reader);
        }

        private void DoMapTilePropertyInsert(BinaryReader reader)
        {
            byte x = reader.ReadByte();
            byte y = reader.ReadByte();
            ushort typeIndex = reader.ReadUInt16();

            Type type = mapTilePropertyTypes[typeIndex];
            MapTileStateProperty property = null;
            if (type != null)
            {
                property = Activator.CreateInstance(type) as MapTileStateProperty;
                latest.Map.Tiles[x, y].AddProperty(property);
            }

            if (property != null) DeserializeFirst(reader, property);
            else DeserializeUnknown(reader);
        }

        private void DoMapTileTypeInsert(BinaryReader reader)
        {
            ushort index = reader.ReadUInt16();
            string name = reader.ReadString();

            // Make sure its the next index in line
            if (index != mapTileTypes.Count)
                throw new NotSupportedException("New Type Index does not match the current List");

            // Identify Type
            var map = context.Mapper.MapTiles.
                Where(m => m.StateType != null).
                FirstOrDefault(m => m.StateType.FullName.Equals(name));

            // Insert into List
            if (map != null) mapTileTypes.Add(map.StateType);
            else mapTileTypes.Add(null);
        }

        private void DoMapTileUpdate(BinaryReader reader)
        {
            byte x = reader.ReadByte();
            byte y = reader.ReadByte();

            DeserializeUpdate(reader, latest.Map.Tiles[x, y]);
        }

        private void DoMapTileInsert(BinaryReader reader)
        {
            byte x = reader.ReadByte();
            byte y = reader.ReadByte();
            ushort typeIndex = reader.ReadUInt16();

            Type type = mapTileTypes[typeIndex];
            MapTileState tile;
            if (type != null) tile = Activator.CreateInstance(type) as MapTileState;
            else tile = new UnknownMapTileState();
            DeserializeFirst(reader, tile);

            latest.Map.Tiles[x, y] = tile;
        }

        private void DoMapPropertyTypeInsert(BinaryReader reader)
        {
            ushort index = reader.ReadUInt16();
            string name = reader.ReadString();

            // Identify Type
            var map = context.Mapper.MapProperties.
                Where(m => m.StateType != null).
                FirstOrDefault(m => m.StateType.FullName.Equals(name));

            // Insert into List
            if (map != null) mapPropertyTypes.Add(map.StateType);
            else mapPropertyTypes.Add(null);
        }

        private void DoMapPropertyUpdate(BinaryReader reader)
        {
            ushort index = reader.ReadUInt16();

            // Identify Property
            Type propertyType = mapPropertyTypes[index];
            MapStateProperty property = null;
            if (propertyType != null)
                property = latest.Map.GetProperty(propertyType);

            // Deserialize
            if (property != null) DeserializeUpdate(reader, property);
            else DeserializeUnknown(reader);
        }

        private void DoMapPropertyInsert(BinaryReader reader)
        {
            ushort index = reader.ReadUInt16();

            // Identify Type
            Type propertyType = mapPropertyTypes[index];
            MapStateProperty property = null;
            if (propertyType != null)
            {
                property = Activator.CreateInstance(propertyType) as MapStateProperty;
                latest.Map.AddProperty(property);
            }

            // Deserialization
            if (property != null) DeserializeFirst(reader, property);
            else DeserializeUnknown(reader);
        }

        private void DoMapUpdate(BinaryReader reader)
        {
            DeserializeUpdate(reader, latest.Map);
        }

        private void DoMapInsert(BinaryReader reader)
        {
            latest.Map = new MapState();
            DeserializeFirst(reader, latest.Map);
        }

        private void DoLevelPropertyTypeInsert(BinaryReader reader)
        {
            ushort index = reader.ReadUInt16();
            string name = reader.ReadString();

            // Identify Type
            var map = context.Mapper.LevelProperties.
                Where(m => m.StateType != null).
                FirstOrDefault(m => m.StateType.FullName.Equals(name));

            // Insert into List
            if (map != null) levelPropertyTypes.Add(map.StateType);
            else levelPropertyTypes.Add(null);
        }

        private void DoLevelPropertyUpdate(BinaryReader reader)
        {
            ushort index = reader.ReadUInt16();

            // Identify Property
            Type propertyType = levelPropertyTypes[index];
            LevelStateProperty property = null;
            if (propertyType != null)
                property = latest.GetProperty(propertyType);

            // Deserialize
            if (property != null) DeserializeUpdate(reader, property);
            else DeserializeUnknown(reader);
        }

        private void DoLevelPropertyInsert(BinaryReader reader)
        {
            ushort index = reader.ReadUInt16();

            // Identify Type
            Type propertyType = levelPropertyTypes[index];
            LevelStateProperty property = null;
            if (propertyType != null)
            {
                property = Activator.CreateInstance(propertyType) as LevelStateProperty;
                latest.AddProperty(property);
            }

            // Deserialization
            if (property != null) DeserializeFirst(reader, property);
            else DeserializeUnknown(reader);
        }

        private void DoLevelUpdate(BinaryReader reader)
        {
            DeserializeUpdate(reader, latest);
        }

        private void DoLevelInsert(BinaryReader reader)
        {
            latest = new LevelState();
            DeserializeFirst(reader, latest);
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
