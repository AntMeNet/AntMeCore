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
        private readonly List<Type> factionPropertyTypes = new List<Type>();
        private readonly List<Type> factionTypes = new List<Type>();
        private readonly List<Type> itemPropertyTypes = new List<Type>();
        private readonly List<Type> itemTypes = new List<Type>();

        private bool keyframeSent;

        private readonly List<byte> knownFactions = new List<byte>();
        private readonly List<ItemState> knownItems = new List<ItemState>();

        private readonly List<Type> levelPropertyTypes = new List<Type>();
        private readonly List<Type> mapPropertyTypes = new List<Type>();
        private readonly List<Type> mapTilePropertyTypes = new List<Type>();
        private readonly List<Type> mapTileTypes = new List<Type>();
        private readonly List<Type> materialTypes = new List<Type>();

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
                DoKeyframe();

                // First Frame
                keyframeSent = true;

                // Insert Level
                DoLevelInsert(writer, state);

                // Insert Level Properties (Includes Registration)
                foreach (var property in state.Properties)
                    DoLevelPropertyInsert(writer, property);

                // Insert Map
                DoMapInsert(writer, state.Map);

                // Insert Map Properties (Includes Registration)
                foreach (var property in state.Map.Properties)
                    DoMapPropertyInsert(writer, property);

                // Map Tiles & Materials
                var size = state.Map.GetCellCount();
                for (byte y = 0; y < size.Y; y++)
                for (byte x = 0; x < size.X; x++)
                {
                    var mapTile = state.Map.Tiles[x, y];

                    // Insert Tiles
                    DoMapTileInsert(writer, mapTile, x, y);

                    // Insert Tiles Properties
                    foreach (var property in mapTile.Properties)
                        DoMapTilePropertyInsert(writer, property, x, y);

                    // Insert Materials
                    DoMaterialInsert(writer, mapTile.Material, x, y);
                }
            }
            else
            {
                // Following Frames

                // Update Level
                DoLevelUpdate(writer, state);

                // Update Level Properties
                foreach (var property in state.Properties)
                    DoLevelPropertyUpdate(writer, property);

                // Update Map
                DoMapUpdate(writer, state.Map);

                // Update Map Properties
                foreach (var property in state.Map.Properties)
                    DoMapPropertyUpdate(writer, property);

                // Map Tiles & Materials
                // TODO: Recognize Material- and Tile-Switches
                var size = state.Map.GetCellCount();
                for (byte y = 0; y < size.Y; y++)
                for (byte x = 0; x < size.X; x++)
                {
                    var mapTile = state.Map.Tiles[x, y];

                    // Update Map Tile
                    DoMapTileUpdate(writer, mapTile, x, y);

                    // Update Properties
                    foreach (var property in mapTile.Properties)
                        DoMapTilePropertyUpdate(writer, property, x, y);

                    // Update Material
                    DoMaterialUpdate(writer, mapTile.Material, x, y);
                }
            }

            // Enumerate Factions
            foreach (var faction in state.Factions)
                if (!knownFactions.Contains(faction.SlotIndex))
                {
                    // Insert Faction
                    DoFactionInsert(writer, faction);

                    // Insert faction Properties
                    foreach (var property in faction.Properties)
                        DoFactionPropertyInsert(writer, faction.SlotIndex, property);

                    knownFactions.Add(faction.SlotIndex);
                }
                else
                {
                    // Update Faction
                    DoFactionUpdate(writer, faction);

                    // Update Faction Properties
                    foreach (var property in faction.Properties)
                        DoFactionPropertyUpdate(writer, faction.SlotIndex, property);
                }

            // Enumerate Items
            foreach (var item in state.Items)
                if (!knownItems.Contains(item))
                {
                    // Insert Item
                    if (item is FactionItemState)
                        DoFactionItemInsert(writer, item as FactionItemState);
                    else DoItemInsert(writer, item);

                    // Insert Item Properties
                    foreach (var property in item.Properties)
                        DoItemPropertyInsert(writer, item.Id, property);

                    knownItems.Add(item);
                }
                else
                {
                    // Update Item
                    DoItemUpdate(writer, item);

                    // Update Item Properties
                    foreach (var property in item.Properties)
                        DoItemPropertyUpdate(writer, item.Id, property);
                }

            var deleted = knownItems.Except(state.Items).ToArray();
            foreach (var item in deleted)
            {
                // Delete Item
                DoItemDelete(writer, item.Id);

                knownItems.Remove(item);
            }

            // Finalize Stream
            writer.Write((byte) LevelStateSerializerPackageV2.FrameEnd);
        }

        public void Dispose()
        {
        }

        private void DoKeyframe()
        {
            levelPropertyTypes.Clear();
            mapPropertyTypes.Clear();
            mapTileTypes.Clear();
            mapTilePropertyTypes.Clear();
            materialTypes.Clear();
            factionTypes.Clear();
            factionPropertyTypes.Clear();
            itemTypes.Clear();
            itemPropertyTypes.Clear();

            knownFactions.Clear();
            knownItems.Clear();
        }

        /// <summary>
        ///     Sends Faction Item Insert into Stream.
        ///     * Package Key [0x97] (byte)
        ///     * Id (int)
        ///     * Type Index (ushort)
        ///     * ByteCount (ushort)
        ///     * Payload (byte[])
        /// </summary>
        /// <param name="writer">Output Stream</param>
        /// <param name="item">Current Item</param>
        private void DoFactionItemInsert(BinaryWriter writer, FactionItemState item)
        {
            var typeIndex = GetIndex(
                itemTypes,
                item.GetType(),
                writer,
                LevelStateSerializerPackageV2.ItemTypeInsert);

            writer.Write((byte) LevelStateSerializerPackageV2.FactionItemInsert);
            writer.Write(item.Id);
            writer.Write(typeIndex);
            SerializeFirst(writer, item);
        }

        /// <summary>
        ///     Sends Item Property Update into Stream.
        ///     * Package Key [0xE6] (byte)
        ///     * Id (int)
        ///     * Type Index (ushort)
        ///     * ByteCount (ushort)
        ///     * Payload (byte[])
        /// </summary>
        /// <param name="writer">Output Stream</param>
        /// <param name="id">Item Id</param>
        /// <param name="property">Property</param>
        private void DoItemPropertyUpdate(BinaryWriter writer, int id, ItemStateProperty property)
        {
            var typeIndex = GetIndex(
                itemPropertyTypes,
                property.GetType(),
                writer,
                LevelStateSerializerPackageV2.ItemPropertyTypeInsert);

            writer.Write((byte) LevelStateSerializerPackageV2.ItemPropertyUpdate);
            writer.Write(id);
            writer.Write(typeIndex);
            SerializeUpdate(writer, property);
        }

        /// <summary>
        ///     Sends Item Property Insert into Stream.
        ///     * Package Key [0xD6] (byte)
        ///     * Id (int)
        ///     * Type Index (ushort)
        ///     * ByteCount (ushort)
        ///     * Payload (byte[])
        /// </summary>
        /// <param name="writer">Output Stream</param>
        /// <param name="id">Item Id</param>
        /// <param name="property">Property</param>
        private void DoItemPropertyInsert(BinaryWriter writer, int id, ItemStateProperty property)
        {
            var typeIndex = GetIndex(
                itemPropertyTypes,
                property.GetType(),
                writer,
                LevelStateSerializerPackageV2.ItemPropertyTypeInsert);

            writer.Write((byte) LevelStateSerializerPackageV2.ItemPropertyInsert);
            writer.Write(id);
            writer.Write(typeIndex);
            SerializeFirst(writer, property);
        }

        /// <summary>
        ///     Sends Item Delete into Stream.
        ///     * Package Key [0xB6] (byte)
        ///     * Id (int)
        /// </summary>
        /// <param name="writer">Output Stream</param>
        /// <param name="id">Id</param>
        private void DoItemDelete(BinaryWriter writer, int id)
        {
            writer.Write((byte) LevelStateSerializerPackageV2.ItemDelete);
            writer.Write(id);
        }

        /// <summary>
        ///     Sends Item Update into Stream.
        ///     * Package Key [0xA6] (byte)
        ///     * Id (int)
        ///     * ByteCount (ushort)
        ///     * Payload (byte[])
        /// </summary>
        /// <param name="writer">Output Stream</param>
        /// <param name="item">Current Item</param>
        private void DoItemUpdate(BinaryWriter writer, ItemState item)
        {
            writer.Write((byte) LevelStateSerializerPackageV2.ItemUpdate);
            writer.Write(item.Id);
            SerializeUpdate(writer, item);
        }

        /// <summary>
        ///     Sends Item Insert into Stream.
        ///     * Package Key [0x96] (byte)
        ///     * Id (int)
        ///     * ItemType Index (ushort)
        ///     * ByteCount (ushort)
        ///     * Payload (byte[])
        /// </summary>
        /// <param name="writer">Output Stream</param>
        /// <param name="item">Current Item</param>
        private void DoItemInsert(BinaryWriter writer, ItemState item)
        {
            var typeIndex = GetIndex(
                itemTypes,
                item.GetType(),
                writer,
                LevelStateSerializerPackageV2.ItemTypeInsert);

            writer.Write((byte) LevelStateSerializerPackageV2.ItemInsert);
            writer.Write(item.Id);
            writer.Write(typeIndex);
            SerializeFirst(writer, item);
        }

        private void DoFactionPropertyUpdate(BinaryWriter writer, byte slotIndex, FactionStateProperty property)
        {
            var typeIndex = GetIndex(
                factionPropertyTypes,
                property.GetType(),
                writer,
                LevelStateSerializerPackageV2.FactionPropertyTypeInsert);

            writer.Write((byte) LevelStateSerializerPackageV2.FactionPropertyUpdate);
            writer.Write(slotIndex);
            writer.Write(typeIndex);
            SerializeUpdate(writer, property);
        }

        private void DoFactionPropertyInsert(BinaryWriter writer, byte slotIndex, FactionStateProperty property)
        {
            var typeIndex = GetIndex(
                factionPropertyTypes,
                property.GetType(),
                writer,
                LevelStateSerializerPackageV2.FactionPropertyTypeInsert);

            writer.Write((byte) LevelStateSerializerPackageV2.FactionPropertyInsert);
            writer.Write(slotIndex);
            writer.Write(typeIndex);
            SerializeFirst(writer, property);
        }

        /// <summary>
        ///     Sends Material Update into Stream.
        ///     * Package Key [0xA5] (byte)
        ///     * Slot Index (byte)
        ///     * ByteCount (ushort)
        ///     * Payload (byte[])
        /// </summary>
        /// <param name="writer">Output Stream</param>
        /// <param name="faction">Current Faction</param>
        private void DoFactionUpdate(BinaryWriter writer, FactionState faction)
        {
            writer.Write((byte) LevelStateSerializerPackageV2.FactionUpdate);
            writer.Write(faction.SlotIndex);
            SerializeUpdate(writer, faction);
        }

        /// <summary>
        ///     Sends Material Update into Stream.
        ///     * Package Key [0x95] (byte)
        ///     * Slot Index (byte)
        ///     * FactionType Index (ushort)
        ///     * ByteCount (ushort)
        ///     * Payload (byte[])
        /// </summary>
        /// <param name="writer">Output Stream</param>
        /// <param name="faction">Current Faction</param>
        private void DoFactionInsert(BinaryWriter writer, FactionState faction)
        {
            var typeIndex = GetIndex(
                factionTypes,
                faction.GetType(),
                writer,
                LevelStateSerializerPackageV2.FactionTypeInsert);

            writer.Write((byte) LevelStateSerializerPackageV2.FactionInsert);
            writer.Write(faction.SlotIndex);
            writer.Write(typeIndex);
            SerializeFirst(writer, faction);
        }

        /// <summary>
        ///     Sends Material Update into Stream.
        ///     * Package Key [0xA4] (byte)
        ///     * Position X (byte)
        ///     * Position Y (byte)
        ///     * ByteCount (ushort)
        ///     * Payload (byte[])
        /// </summary>
        /// <param name="writer">Output Stream</param>
        /// <param name="material">Current Material</param>
        /// <param name="x">Position X</param>
        /// <param name="y">Position Y</param>
        private void DoMaterialUpdate(BinaryWriter writer, MapMaterial material, byte x, byte y)
        {
            writer.Write((byte) LevelStateSerializerPackageV2.MaterialUpdate);
            writer.Write(x);
            writer.Write(y);
            SerializeUpdate(writer, material);
        }

        /// <summary>
        ///     Sends Material Insert into Stream.
        ///     * Package Key [0x94] (byte)
        ///     * Position X (byte)
        ///     * Position Y (byte)
        ///     * MaterialType Index (ushort)
        ///     * ByteCount (ushort)
        ///     * Payload (byte[])
        /// </summary>
        /// <param name="writer">Output Stream</param>
        /// <param name="material">Current Material</param>
        /// <param name="x">Position X</param>
        /// <param name="y">Position Y</param>
        private void DoMaterialInsert(BinaryWriter writer, MapMaterial material, byte x, byte y)
        {
            var typeIndex = GetIndex(
                materialTypes,
                material.GetType(),
                writer,
                LevelStateSerializerPackageV2.MaterialTypeInsert);

            writer.Write((byte) LevelStateSerializerPackageV2.MaterialInsert);
            writer.Write(x);
            writer.Write(y);
            writer.Write(typeIndex);
            SerializeFirst(writer, material);
        }

        /// <summary>
        ///     Sends Map Tile Property Insert to the Stream.
        ///     * Package Key [0xE3] (byte)
        ///     * Position X (byte)
        ///     * Position Y (byte)
        ///     * Type Index (ushort)
        ///     * ByteCount (ushort)
        ///     * Payload (byte[])
        /// </summary>
        /// <param name="writer">Output Stream</param>
        /// <param name="property">Property</param>
        /// <param name="x">Position X</param>
        /// <param name="y">Position Y</param>
        private void DoMapTilePropertyUpdate(BinaryWriter writer, MapTileStateProperty property, byte x, byte y)
        {
            var typeIndex = GetIndex(
                mapTilePropertyTypes,
                property.GetType(),
                writer,
                LevelStateSerializerPackageV2.MapTilePropertyTypeInsert);

            writer.Write((byte) LevelStateSerializerPackageV2.MapTilePropertyUpdate);
            writer.Write(x);
            writer.Write(y);
            writer.Write(typeIndex);
            SerializeUpdate(writer, property);
        }

        /// <summary>
        ///     Sends Map Tile Property Insert to the Stream.
        ///     * Package Key [0xD3] (byte)
        ///     * Position X (byte)
        ///     * Position Y (byte)
        ///     * Type Index (ushort)
        ///     * ByteCount (ushort)
        ///     * Payload (byte[])
        /// </summary>
        /// <param name="writer">Output Stream</param>
        /// <param name="property">Property</param>
        /// <param name="x">Position X</param>
        /// <param name="y">Position Y</param>
        private void DoMapTilePropertyInsert(BinaryWriter writer, MapTileStateProperty property, byte x, byte y)
        {
            var typeIndex = GetIndex(
                mapTilePropertyTypes,
                property.GetType(),
                writer,
                LevelStateSerializerPackageV2.MapTilePropertyTypeInsert);

            writer.Write((byte) LevelStateSerializerPackageV2.MapTilePropertyInsert);
            writer.Write(x);
            writer.Write(y);
            writer.Write(typeIndex);
            SerializeFirst(writer, property);
        }

        /// <summary>
        ///     Sends Map Update into Stream.
        ///     * Package Key [0xA3] (byte)
        ///     * Position X (byte)
        ///     * Position Y (byte)
        ///     * ByteCount (ushort)
        ///     * Payload (byte[])
        /// </summary>
        /// <param name="writer">Output Stream</param>
        /// <param name="mapTile">Current MapTile</param>
        /// <param name="x">Position X</param>
        /// <param name="y">Position Y</param>
        private void DoMapTileUpdate(BinaryWriter writer, MapTileState mapTile, byte x, byte y)
        {
            var typeIndex = GetIndex(
                mapTileTypes,
                mapTile.GetType(),
                writer,
                LevelStateSerializerPackageV2.MapTileTypeInsert);

            writer.Write((byte) LevelStateSerializerPackageV2.MapTileUpdate);
            writer.Write(x);
            writer.Write(y);
            SerializeUpdate(writer, mapTile);
        }

        /// <summary>
        ///     Sends Map Update into Stream.
        ///     * Package Key [0x93] (byte)
        ///     * Position X (byte)
        ///     * Position Y (byte)
        ///     * MapTileType Index (ushort)
        ///     * ByteCount (ushort)
        ///     * Payload (byte[])
        /// </summary>
        /// <param name="writer">Output Stream</param>
        /// <param name="mapTile">Current MapTile</param>
        /// <param name="x">Position X</param>
        /// <param name="y">Position Y</param>
        private void DoMapTileInsert(BinaryWriter writer, MapTileState mapTile, byte x, byte y)
        {
            var typeIndex = GetIndex(
                mapTileTypes,
                mapTile.GetType(),
                writer,
                LevelStateSerializerPackageV2.MapTileTypeInsert);

            writer.Write((byte) LevelStateSerializerPackageV2.MapTileInsert);
            writer.Write(x);
            writer.Write(y);
            writer.Write(typeIndex);
            SerializeFirst(writer, mapTile);
        }

        /// <summary>
        ///     Sends Map Property Update to the Stream.
        ///     * Package Key [0xE2] (byte)
        ///     * Type Index (ushort)
        ///     * ByteCount (ushort)
        ///     * Payload (byte[])
        /// </summary>
        /// <param name="writer">Output Stream</param>
        /// <param name="property">Property</param>
        private void DoMapPropertyUpdate(BinaryWriter writer, MapStateProperty property)
        {
            var typeIndex = GetIndex(
                mapPropertyTypes,
                property.GetType(),
                writer,
                LevelStateSerializerPackageV2.MapPropertyTypeInsert);

            writer.Write((byte) LevelStateSerializerPackageV2.MapPropertyUpdate);
            writer.Write(typeIndex);
            SerializeUpdate(writer, property);
        }

        /// <summary>
        ///     Sends Map Property Insert to the Stream.
        ///     * Package Key [0xD2] (byte)
        ///     * Type Index (ushort)
        ///     * ByteCount (ushort)
        ///     * Payload (byte[])
        /// </summary>
        /// <param name="writer">Output Stream</param>
        /// <param name="property">Property</param>
        private void DoMapPropertyInsert(BinaryWriter writer, MapStateProperty property)
        {
            var typeIndex = GetIndex(
                mapPropertyTypes,
                property.GetType(),
                writer,
                LevelStateSerializerPackageV2.MapPropertyTypeInsert);

            writer.Write((byte) LevelStateSerializerPackageV2.MapPropertyInsert);
            writer.Write(typeIndex);
            SerializeFirst(writer, property);
        }

        /// <summary>
        ///     Sends Map Update into Stream.
        ///     * Package Key [0xA2] (byte)
        ///     * ByteCount (ushort)
        ///     * Payload (byte[])
        /// </summary>
        /// <param name="writer">Output Stream</param>
        /// <param name="map">Current Map</param>
        private void DoMapUpdate(BinaryWriter writer, MapState map)
        {
            writer.Write((byte) LevelStateSerializerPackageV2.MapUpdate);
            SerializeUpdate(writer, map);
        }

        /// <summary>
        ///     Sends Map Insert into Stream.
        ///     * Package Key [0x92] (byte)
        ///     * ByteCount (ushort)
        ///     * Payload (byte[])
        /// </summary>
        /// <param name="writer">Output Stream</param>
        /// <param name="map">Current Map</param>
        private void DoMapInsert(BinaryWriter writer, MapState map)
        {
            writer.Write((byte) LevelStateSerializerPackageV2.MapInsert);
            SerializeFirst(writer, map);
        }

        /// <summary>
        ///     Sends a Level Property Update into the Stream
        ///     * Package Key [0xE1] (byte)
        ///     * Type Index (ushort)
        ///     * ByteCount (ushort)
        ///     * Payload (byte[])
        /// </summary>
        /// <param name="writer">Output Stream</param>
        /// <param name="property">Property</param>
        private void DoLevelPropertyUpdate(BinaryWriter writer, LevelStateProperty property)
        {
            var typeIndex = GetIndex(
                levelPropertyTypes,
                property.GetType(),
                writer,
                LevelStateSerializerPackageV2.LevelPropertyTypeInsert);

            writer.Write((byte) LevelStateSerializerPackageV2.LevelPropertyUpdate);
            writer.Write(typeIndex);
            SerializeUpdate(writer, property);
        }

        /// <summary>
        ///     Sends a Level Property Insert into the Stream
        ///     * Package Key [0xD1] (byte)
        ///     * Type Index (ushort)
        ///     * ByteCount (ushort)
        ///     * Payload (byte[])
        /// </summary>
        /// <param name="writer">Output Stream</param>
        /// <param name="property">Property</param>
        private void DoLevelPropertyInsert(BinaryWriter writer, LevelStateProperty property)
        {
            var typeIndex = GetIndex(
                levelPropertyTypes,
                property.GetType(),
                writer,
                LevelStateSerializerPackageV2.LevelPropertyTypeInsert);

            writer.Write((byte) LevelStateSerializerPackageV2.LevelPropertyInsert);
            writer.Write(typeIndex);
            SerializeFirst(writer, property);
        }

        /// <summary>
        ///     Sends Level Update into Stream.
        ///     * Package Key [0xA1] (byte)
        ///     * ByteCount (ushort)
        ///     * Payload (byte[])
        /// </summary>
        /// <param name="writer">Output Stream</param>
        /// <param name="state">Current State</param>
        private void DoLevelUpdate(BinaryWriter writer, LevelState state)
        {
            writer.Write((byte) LevelStateSerializerPackageV2.LevelUpdate);
            SerializeUpdate(writer, state);
        }

        /// <summary>
        ///     Sends Level Insert into Stream.
        ///     * Package Key [0x91] (byte)
        ///     * ByteCount (ushort)
        ///     * Payload (byte[])
        /// </summary>
        /// <param name="writer">Output Stream</param>
        /// <param name="state">Current State</param>
        private void DoLevelInsert(BinaryWriter writer, LevelState state)
        {
            writer.Write((byte) LevelStateSerializerPackageV2.LevelInsert);
            SerializeFirst(writer, state);
        }

        /// <summary>
        ///     Gets the Index of the given Type and registers if not available.
        ///     * Package Key (insertCommand) (byte)
        ///     * Type Index (ushort)
        ///     * Type FullName (string)
        /// </summary>
        /// <param name="list">Type List</param>
        /// <param name="type">Type</param>
        /// <param name="writer">Output Stream</param>
        /// <param name="insertCommand">Insert Command</param>
        /// <returns>Index of Type in List</returns>
        private ushort GetIndex(List<Type> list, Type type, BinaryWriter writer,
            LevelStateSerializerPackageV2 insertCommand)
        {
            if (!list.Contains(type))
            {
                list.Add(type);
                writer.Write((byte) insertCommand);
                writer.Write((ushort) list.IndexOf(type));
                writer.Write(type.FullName);
            }

            return (ushort) list.IndexOf(type);
        }

        /// <summary>
        ///     Serializes the Content of the First Frame into the Stream.
        ///     * ByteCount (ushort)
        ///     * Payload (byte[])
        /// </summary>
        /// <param name="writer">Output Stream</param>
        /// <param name="state">State</param>
        private void SerializeFirst(BinaryWriter writer, ISerializableState state)
        {
            using (var mem = new MemoryStream())
            {
                using (var memWriter = new BinaryWriter(mem))
                {
                    // Serialize
                    state.SerializeFirst(memWriter, VERSION);

                    // Copy to Buffer
                    var buffer = mem.GetBuffer();
                    var length = (ushort) mem.Position;

                    // Write to main Stream
                    writer.Write(length);
                    writer.Write(buffer, 0, length);
                }
            }
        }

        /// <summary>
        ///     Serializes the Content of an Update Frame into the Stream.
        ///     * ByteCount (ushort)
        ///     * Payload (byte[])
        /// </summary>
        /// <param name="writer">Output Stream</param>
        /// <param name="state">State</param>
        private void SerializeUpdate(BinaryWriter writer, ISerializableState state)
        {
            using (var mem = new MemoryStream())
            {
                using (var memWriter = new BinaryWriter(mem))
                {
                    // Serialize
                    state.SerializeFirst(memWriter, VERSION);

                    // Copy to Buffer
                    var buffer = mem.GetBuffer();
                    var length = (ushort) mem.Position;

                    // Write to main Stream
                    writer.Write(length);
                    writer.Write(buffer, 0, length);
                }
            }
        }
    }
}