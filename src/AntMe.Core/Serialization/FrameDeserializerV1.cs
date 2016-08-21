using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using static AntMe.Serialization.FrameSerializerV1;

namespace AntMe.Serialization
{
    [Obsolete]
    internal sealed class FrameDeserializerV1 : IFrameDeserializer
    {
        private readonly MemoryStream _stream;
        private readonly BinaryReader _reader;
        private object _lockObject = new object();

        private bool _streamOpen = false;
        private bool _frameOpen = false;
        private int _frameNumber = 0;
        private byte _version = 0;
        private bool _disposed = false;

        private Frame _currentState;

        private readonly Dictionary<byte, FactionState> _factions = new Dictionary<byte, FactionState>();
        private readonly Dictionary<int, ItemState> _items = new Dictionary<int, ItemState>();
        private readonly Dictionary<byte, Type> _knownFactionTypes = new Dictionary<byte, Type>();
        private readonly Dictionary<byte, Type> _knownItemTypes = new Dictionary<byte, Type>();

        /// <summary>
        /// Neue Instanz eines State Deserializers.
        /// </summary>
        public FrameDeserializerV1()
        {
            _stream = new MemoryStream();
            _reader = new BinaryReader(_stream, Encoding.UTF8);
        }

        /// <summary>
        /// Deserialisiert den gegebenen Byte Array.
        /// </summary>
        /// <param name="input">Input Array</param>
        /// <returns>Aktuelle Instanz des Main States</returns>
        public Frame Deserialize(Stream input)
        {
            throw new NotSupportedException("Version 1 is not supported anymore");

            lock (_lockObject)
            {
                // Deserializer ist bereits disposed.
                if (_disposed)
                    throw new NotSupportedException("Deserializer already disposed");

                _stream.Position = 0;
                // _stream.Write(rawData, offset, rawData.Length - offset);
                _stream.Position = 0;

                return InternalDeserialize();
            }
        }

        private Frame InternalDeserialize()
        {
            while (ReadNext()) { }
            return _currentState;
        }

        private bool ReadNext()
        {
            SerializerPackage package = (SerializerPackage)_reader.ReadByte();
            switch (package)
            {
                #region Infrastruktur

                case SerializerPackage.StreamVersion:
                    return ReadStreamVersion();
                case SerializerPackage.FrameStart:
                    return ReadFrameStart();
                case SerializerPackage.FrameEnd:
                    return ReadFrameEnd();

                #endregion

                #region Main & Map

                case SerializerPackage.MainFirst:
                    return ReadMainFirst();
                case SerializerPackage.MainUpdate:
                    return ReadMainUpdate();
                case SerializerPackage.MapFirst:
                    return ReadMapFirst();
                case SerializerPackage.MapUpdate:
                    return ReadMapUpdate();

                #endregion

                #region Factions

                case SerializerPackage.FactionTypeRegistration:
                    return ReadFactionTypeRegistration();
                case SerializerPackage.FactionFirst:
                    return ReadFactionFirst();
                case SerializerPackage.FactionUpdate:
                    return ReadFactionUpdate();
                case SerializerPackage.FactionLost:
                    return ReadFactionLost();

                #endregion

                #region Items

                case SerializerPackage.ItemTypeRegistration:
                    return ReadItemTypeRegistration();
                case SerializerPackage.ItemFirst:
                    return ReadItemFirst();
                case SerializerPackage.ItemFactionFirst:
                    return ReadItemFactionFirst();
                case SerializerPackage.ItemUpdate:
                    return ReadItemUpdate();
                case SerializerPackage.ItemLost:
                    return ReadItemLost();

                #endregion

                default:
                    throw new NotSupportedException("Unknown Token Type");
            }
        }

        #region Infrastruktur

        private bool ReadStreamVersion()
        {
            _version = _reader.ReadByte();
            if (_version != 1)
                throw new NotSupportedException("Stream Version not supported");
            if (_streamOpen)
                throw new NotSupportedException("Stream is already open");
            _streamOpen = true;
            return true;
        }

        private bool ReadFrameStart()
        {
            if (_frameOpen)
                throw new NotSupportedException("Frame is already open");
            _frameOpen = true;
            _frameNumber = _reader.ReadInt32();
            return true;
        }

        private bool ReadFrameEnd()
        {
            if (!_streamOpen)
                throw new NotSupportedException("There is no open Stream");
            if (!_frameOpen)
                throw new NotSupportedException("There is no open Frame");
            _frameOpen = false;
            return false;
        }

        #endregion

        #region Main und Map


        private bool ReadMainFirst()
        {
            if (!_streamOpen)
                throw new NotSupportedException("No open Stream available");
            if (!_frameOpen)
                throw new NotSupportedException("No open Frame avaible");

            _currentState = new Frame();
            ValidatedDeserialisation(_currentState.DeserializeFirst);
            return true;
        }

        private bool ReadMainUpdate()
        {
            CheckBasics();

            if (_currentState == null)
                throw new NotSupportedException("There is no MainState");

            ValidatedDeserialisation(_currentState.DeserializeUpdate);
            return true;
        }

        private bool ReadMapFirst()
        {
            CheckBasics();

            if (_currentState.Map != null)
                throw new NotSupportedException("There is a MapState already");

            _currentState.Map = new MapState();
            ValidatedDeserialisation(_currentState.Map.DeserializeFirst);
            return true;
        }

        private bool ReadMapUpdate()
        {
            CheckBasics();

            if (_currentState.Map == null)
                throw new NotSupportedException("There is no MapState");

            ValidatedDeserialisation(_currentState.Map.DeserializeUpdate);
            return true;
        }

        #endregion

        #region Factions

        private bool ReadFactionTypeRegistration()
        {
            CheckBasics();

            var index = _reader.ReadByte();
            var name = _reader.ReadString();

            if (_knownFactionTypes.ContainsKey(index))
                throw new NotSupportedException("Type is already known");

            object state = null;
            try
            {
                state = Activator.CreateInstance(Type.GetType(name));
            }
            catch (Exception)
            {
                // TODO: Check right Exceptions (unknonw Types und so)
                _knownFactionTypes.Add(index, null);
                return true;
            }

            if (!(state is FactionState))
                throw new NotSupportedException("Type is no FactionState Type");
            _knownFactionTypes.Add(index, state.GetType());

            return true;
        }

        private bool ReadFactionFirst()
        {
            CheckBasics();

            byte index = _reader.ReadByte();
            byte typeIndex = _reader.ReadByte();

            if (!_knownFactionTypes.ContainsKey(typeIndex))
                throw new NotSupportedException("Type is not registered");

            // Neue Instanz erzeugen
            Type factionType = _knownFactionTypes[typeIndex];
            FactionState state = null;
            if (factionType != null)
            {
                state = Activator.CreateInstance(factionType) as FactionState;
                state.SlotIndex = index;
                ValidatedDeserialisation(state.DeserializeFirst);
            }
            else
            {
                state = new FactionState();
                SkipBlock();
            }

            _factions.Add(index, state);
            _currentState.Factions.Add(state);

            return true;
        }

        private bool ReadFactionUpdate()
        {
            CheckBasics();

            byte index = _reader.ReadByte();

            FactionState state;
            if (_factions.TryGetValue(index, out state))
            {
                if (state.GetType() == typeof(FactionState))
                    SkipBlock();
                else
                    ValidatedDeserialisation(state.DeserializeUpdate);
            }
            else
                throw new NotSupportedException("No FactionState available");

            return true;
        }

        private bool ReadFactionLost()
        {
            CheckBasics();

            byte index = _reader.ReadByte();
            FactionState state;
            if (_factions.TryGetValue(index, out state))
            {
                _currentState.Factions.Remove(state);
                _factions.Remove(index);
            }

            return true;
        }

        #endregion

        #region Items

        private bool ReadItemTypeRegistration()
        {
            CheckBasics();

            byte index = _reader.ReadByte();
            string name = _reader.ReadString();

            if (_knownItemTypes.ContainsKey(index))
                throw new NotSupportedException("Type is already known");

            object state = null;
            try
            {
                state = Activator.CreateInstance(Type.GetType(name));
            }
            catch (Exception)
            {
                // TODO: Check right Exceptions
                _knownItemTypes.Add(index, null);
                return true;
            }

            if (!(state is ItemState))
                throw new NotSupportedException("Type is no ItemState Type");
            _knownItemTypes.Add(index, state.GetType());

            return true;
        }

        private bool ReadItemFirst()
        {
            CheckBasics();

            int id = _reader.ReadInt32();
            byte type = _reader.ReadByte();

            ItemState state;
            if (_knownItemTypes.ContainsKey(type))
            {
                state = Activator.CreateInstance(_knownItemTypes[type]) as ItemState;
                state.Id = id;
                ValidatedDeserialisation(state.DeserializeFirst);
            }
            else
            {
                state = new ItemState();
                state.Id = id;
                SkipBlock();
            }

            _items.Add(id, state);
            _currentState.Items.Add(state);

            return true;
        }

        private bool ReadItemFactionFirst()
        {
            CheckBasics();

            int id = _reader.ReadInt32();
            byte faction = _reader.ReadByte();
            byte type = _reader.ReadByte();

            FactionItemState state;
            if (_knownItemTypes.ContainsKey(type))
            {
                state = Activator.CreateInstance(_knownItemTypes[type]) as FactionItemState;
                state.Id = id;
                state.SlotIndex = faction;
                ValidatedDeserialisation(state.DeserializeFirst);
            }
            else
            {
                // TODO: Unknown Faction State
                // state = new FactionItemState();
                state = null;
                SkipBlock();
            }

            _items.Add(id, state);
            _currentState.Items.Add(state);

            return true;
        }

        private bool ReadItemUpdate()
        {
            CheckBasics();

            int id = _reader.ReadInt32();

            ItemState state;
            if (_items.TryGetValue(id, out state))
            {
                if (state.GetType() == typeof(ItemState) || state.GetType() == typeof(FactionItemState))
                    SkipBlock();
                else
                    ValidatedDeserialisation(state.DeserializeUpdate);
            }
            else
                throw new NotSupportedException("Item with the given ID not found");

            return true;
        }

        private bool ReadItemLost()
        {
            CheckBasics();

            int id = _reader.ReadInt32();

            ItemState state;
            if (_items.TryGetValue(id, out state))
            {
                _currentState.Items.Remove(state);
                _items.Remove(id);
            }
            else
                throw new NotSupportedException("Item with the given ID not found");

            return true;
        }

        #endregion

        /// <summary>
        /// Deserialisiert mit Hilfe der angegebenen Methode, checkt aber mit der Checksumme gegen.
        /// </summary>
        /// <param name="deserializer">Methode zur Deserialisierung</param>
        private void ValidatedDeserialisation(Action<BinaryReader, byte> deserializer)
        {
            short size = _reader.ReadInt16();
            long pos = _stream.Position;
            deserializer(_reader, _version);
            if (_stream.Position - pos != size)
                throw new InvalidOperationException("Size and Position don't match");
        }

        /// <summary>
        /// Liest die ersten beiden Bytes aus, um die Länge des Restblocks zu ermitteln und liest 
        /// den Block vollständig aus.
        /// </summary>
        private void SkipBlock()
        {
            short size = _reader.ReadInt16();
            _reader.ReadBytes(size);
        }

        private void CheckBasics()
        {
            if (!_streamOpen)
                throw new NotSupportedException("No open Stream available");
            if (!_frameOpen)
                throw new NotSupportedException("No open Frame avaible");
            if (_currentState == null)
                throw new NotSupportedException("No MainState available");
        }

        /// <summary>
        /// Disposed den Deserializer.
        /// </summary>
        public void Dispose()
        {
            lock (_lockObject)
            {
                // Deserializer ist bereits disposed.
                if (_disposed) return;

                _reader.Dispose();
                _stream.Dispose();
                _factions.Clear();
                _items.Clear();
                _knownFactionTypes.Clear();
                _knownItemTypes.Clear();
            }
        }

        public Frame Deserialize(BinaryReader reader)
        {
            throw new NotImplementedException();
        }
    }
}
