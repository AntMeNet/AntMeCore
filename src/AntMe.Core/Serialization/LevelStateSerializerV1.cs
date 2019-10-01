using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AntMe.Serialization
{
    [Obsolete]
    internal sealed class LevelStateSerializerV1 : ILevelStateSerializer
    {
        private readonly Dictionary<byte, bool> _factions = new Dictionary<byte, bool>();
        private readonly Dictionary<int, bool> _items = new Dictionary<int, bool>();

        private readonly Dictionary<Type, byte> _knownFactionTypes = new Dictionary<Type, byte>();
        private readonly Dictionary<Type, byte> _knownItemTypes = new Dictionary<Type, byte>();
        private readonly MemoryStream _stream;
        private readonly BinaryWriter _writer;
        private readonly bool _disposed = false;
        private byte _factionTypeIndexPointer;
        private int _frameCounter;

        private bool _initialized;
        private byte _itemTypeIndexPointer;
        private bool _lastFlag;
        private readonly object _lockItem = new object();
        private readonly byte _version = 1;

        /// <summary>
        ///     neue Instanz eines State Serializers.
        /// </summary>
        public LevelStateSerializerV1()
        {
            _stream = new MemoryStream();
            _writer = new BinaryWriter(_stream, Encoding.UTF8);
        }

        /// <summary>
        ///     Disposed den Serializer.
        /// </summary>
        public void Dispose()
        {
            lock (_lockItem)
            {
                // Serializer bereits disposed.
                if (_disposed) return;

                _writer.Dispose();
                _stream.Dispose();
                _knownFactionTypes.Clear();
                _knownItemTypes.Clear();
                _items.Clear();
                _factions.Clear();
            }
        }

        public void Serialize(BinaryWriter writer, LevelState state)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Serialisiert einen einzelnen State auf Basis der vorherigen States.
        /// </summary>
        /// <param name="output">Output Stream</param>
        /// <param name="state">Zu serialisierender State</param>
        /// <returns>Resultierender Byte Array</returns>
        public void Serialize(Stream output, LevelState state)
        {
            lock (_lockItem)
            {
                // Serializer bereits Disposed
                if (_disposed)
                    throw new NotSupportedException("Serializer already disposed");

                // Reset stream
                _stream.Position = 0;

                InternalSerialize(state);

                // Copy to buffer
                var length = (int) _stream.Position;
                var buffer = new byte[length];
                _stream.Position = 0;
                _stream.Read(buffer, 0, length);
                output.Write(buffer, 0, length);
            }
        }

        private void ValidateSerialization(Action<BinaryWriter, byte> serializer)
        {
            _writer.Flush();
            var sizePos = _stream.Position;
            _writer.Write((short) 0);
            _writer.Flush();
            var packagePos = _stream.Position;
            serializer(_writer, _version);
            _writer.Flush();
            var endPos = _stream.Position;
            var size = endPos - packagePos;

            if (size > short.MaxValue)
                throw new InvalidOperationException("Package is too big");
            if (size > 0)
            {
                _stream.Position = sizePos;
                _writer.Write((short) size);
                _writer.Flush();
                _stream.Position = endPos;
            }
        }

        private void InternalSerialize(LevelState state)
        {
            if (state == null)
                throw new ArgumentNullException("state Parameter is null");

            // Stream öffnen
            if (!_initialized)
            {
                _writer.Write((byte) SerializerPackage.StreamVersion);
                _writer.Write(_version);
            }

            // Start Frame
            _writer.Write((byte) SerializerPackage.FrameStart);
            _writer.Write(_frameCounter++);

            #region Main- & Map-Handling

            if (state.Map == null)
                throw new NotSupportedException("Map is null");

            if (!_initialized)
            {
                // Serialize First Main
                _writer.Write((byte) SerializerPackage.MainFirst);
                ValidateSerialization(state.SerializeFirst);

                // Serialize First Map
                _writer.Write((byte) SerializerPackage.MapFirst);
                ValidateSerialization(state.Map.SerializeFirst);

                _initialized = true;
            }
            else
            {
                // Serialize Main Udate
                _writer.Write((byte) SerializerPackage.MainUpdate);
                ValidateSerialization(state.SerializeUpdate);

                // Serialize Map Update
                _writer.Write((byte) SerializerPackage.MapUpdate);
                ValidateSerialization(state.Map.SerializeUpdate);
            }

            #endregion

            #region Faction Handling

            if (state.Factions == null)
                throw new ArgumentNullException("Factions List is null");

            // Factions durchlaufen
            foreach (var faction in state.Factions)
                if (_factions.ContainsKey(faction.SlotIndex))
                {
                    // Alte Faction
                    _writer.Write((byte) SerializerPackage.FactionUpdate);
                    _writer.Write(faction.SlotIndex);
                    ValidateSerialization(faction.SerializeUpdate);
                    _factions[faction.SlotIndex] = !_lastFlag;
                }
                else
                {
                    byte factionId = 0;

                    // Neue Faction
                    if (!_knownFactionTypes.TryGetValue(faction.GetType(), out factionId))
                    {
                        factionId = _factionTypeIndexPointer++;

                        // Neuer Type
                        _writer.Write((byte) SerializerPackage.FactionTypeRegistration);
                        _writer.Write(factionId);
                        _writer.Write(faction.GetType().FullName);
                        _knownFactionTypes.Add(faction.GetType(), factionId);
                    }

                    _writer.Write((byte) SerializerPackage.FactionFirst);
                    _writer.Write(faction.SlotIndex);
                    _writer.Write(factionId);
                    ValidateSerialization(faction.SerializeFirst);

                    _factions.Add(faction.SlotIndex, !_lastFlag);
                }

            // Dropped Factions
            foreach (var factionKey in _factions.Keys.ToArray())
                if (_factions[factionKey] == _lastFlag)
                {
                    _writer.Write((byte) SerializerPackage.FactionLost);
                    _writer.Write(factionKey);
                    _factions.Remove(factionKey);
                }

            #endregion

            #region Item Handling

            if (state.Items == null)
                throw new ArgumentNullException("Items List is null");

            // Items durchlaufen
            foreach (var item in state.Items)
                if (_items.ContainsKey(item.Id))
                {
                    // Altes Item
                    _writer.Write((byte) SerializerPackage.ItemUpdate);
                    _writer.Write(item.Id);
                    ValidateSerialization(item.SerializeUpdate);
                    _items[item.Id] = !_lastFlag;
                }
                else
                {
                    byte itemType = 0;

                    // Neues Item
                    if (!_knownItemTypes.TryGetValue(item.GetType(), out itemType))
                    {
                        itemType = _itemTypeIndexPointer++;

                        // Neuer Type
                        _writer.Write((byte) SerializerPackage.ItemTypeRegistration);
                        _writer.Write(itemType);
                        _writer.Write(item.GetType().FullName);
                        _knownItemTypes.Add(item.GetType(), itemType);
                    }

                    if (item is FactionItemState)
                    {
                        var factionItemState = item as FactionItemState;
                        _writer.Write((byte) SerializerPackage.ItemFactionFirst);
                        _writer.Write(item.Id);
                        _writer.Write(factionItemState.SlotIndex);
                    }
                    else
                    {
                        _writer.Write((byte) SerializerPackage.ItemFirst);
                        _writer.Write(item.Id);
                    }

                    _writer.Write(itemType);
                    ValidateSerialization(item.SerializeFirst);
                    _items.Add(item.Id, !_lastFlag);
                }

            // Dropped Items
            foreach (var itemKey in _items.Keys.ToArray())
                if (_items[itemKey] == _lastFlag)
                {
                    _writer.Write((byte) SerializerPackage.ItemLost);
                    _writer.Write(itemKey);
                    _items.Remove(itemKey);
                }

            #endregion

            _writer.Write((byte) SerializerPackage.FrameEnd);

            _lastFlag = !_lastFlag;
        }

        /// <summary>
        ///     Liste der möglichen Serializer Packages.
        /// </summary>
        internal enum SerializerPackage
        {
            /// <summary>
            ///     Intro [Versionsummer (byte)]
            /// </summary>
            StreamVersion = 1,

            /// <summary>
            ///     Start eines Simulationsframes [Framenummer (int)]
            /// </summary>
            FrameStart = 2,

            /// <summary>
            ///     Erster Main State [Größe (short), Ladung (byte[])]
            /// </summary>
            MainFirst = 11,

            /// <summary>
            ///     Update Main State [Größe (short), Ladung (byte[])]
            /// </summary>
            MainUpdate = 12,

            /// <summary>
            ///     Erster Map State [Größe (short), Ladung (byte[])]
            /// </summary>
            MapFirst = 21,

            /// <summary>
            ///     Update Map State [Größe (short), Ladung (byte[])]
            /// </summary>
            MapUpdate = 22,

            /// <summary>
            ///     Typ registrierung [Index (byte), Assembly (string), Name (string)]
            /// </summary>
            ItemTypeRegistration = 31,

            /// <summary>
            ///     Erster Item State [ID (int), Typnummer (byte), Größe (short), Ladung (byte[])]
            /// </summary>
            ItemFirst = 41,

            /// <summary>
            ///     Erster FactionItem State [ID (int), Faction (byte), Typnummer (byte), Größe (short), Ladung (byte[])]
            /// </summary>
            ItemFactionFirst = 42,

            /// <summary>
            ///     Update Item State [ID (int), Größe (short), Ladung (byte[])]
            /// </summary>
            ItemUpdate = 43,

            /// <summary>
            ///     Lost Item, [ID (int)]
            /// </summary>
            ItemLost = 44,

            /// <summary>
            ///     Typ registrierung [Index (byte), Assembly (string), Name (string)]
            /// </summary>
            FactionTypeRegistration = 51,

            /// <summary>
            ///     Erster Faction State [Index (byte), Typnummer (byte), Größe (short), Ladung (byte[])]
            /// </summary>
            FactionFirst = 61,

            /// <summary>
            ///     Update Faction State [Index (byte), Größe (short), Ladung (byte[])]
            /// </summary>
            FactionUpdate = 62,

            /// <summary>
            ///     Lost Item, [ID (int)]
            /// </summary>
            FactionLost = 63,

            /// <summary>
            ///     Letzter Block im Frame
            /// </summary>
            FrameEnd = 101
        }
    }
}