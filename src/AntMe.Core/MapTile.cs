using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace AntMe
{
    /// <summary>
    /// Base Class for Map Tiles
    /// </summary>
    public abstract class MapTile : PropertyList<MapTileProperty>, ISerializableState
    {
        private MapTileState state;

        private MapMaterial material;

        private MapTileOrientation orientation;

        private byte heightLevel;

        private Dictionary<Item, MapTileInfo> infos;

        /// <summary>
        /// Reference to the Simulation Context.
        /// </summary>
        protected readonly SimulationContext Context;

        /// <summary>
        /// List and Data of all unknown Property from Deserialization.
        /// </summary>
        [Browsable(false)]
        public Dictionary<string, byte[]> UnknownProperties { get; private set; }

        /// <summary>
        /// Default Constructor.
        /// </summary>
        /// <param name="context">Simulation Context</param>
        public MapTile(SimulationContext context)
        {
            Context = context;
            infos = new Dictionary<Item, MapTileInfo>();
            UnknownProperties = new Dictionary<string, byte[]>();

            context.Resolver.ResolveMapTile(context, this);
        }

        /// <summary>
        /// Generates the Map Tile State.
        /// </summary>
        /// <returns></returns>
        public MapTileState GetState()
        {
            if (state == null)
                state = Context.Resolver.CreateMapTileState(this);
            return state;
        }

        /// <summary>
        /// Returns an Info Object for the current Tile for the giben Observer.
        /// </summary>
        /// <param name="observer">Observer Item</param>
        /// <returns>Info Object</returns>
        public MapTileInfo GetInfo(Item observer)
        {
            if (observer == null)
                throw new ArgumentNullException("observer");

            // Check Info Cache
            if (infos.ContainsKey(observer))
                return infos[observer];

            // Generate new Instance
            var info = Context.Resolver.CreateMapTileInfo(this, observer);
            if (info == null)
                throw new NotSupportedException("Could not create new Map Tile Info");

            infos.Add(observer, info);
            return info;
        }

        /// <summary>
        /// Gets or sets the Material.
        /// </summary>
        [Browsable(false)]
        public MapMaterial Material
        {
            get { return material; }
            set
            {
                material = value;
                if (OnMaterialChanged != null)
                    OnMaterialChanged(value);
            }
        }

        /// <summary>
        /// Gets or sets the Orientation of this Tile.
        /// </summary>
        [DisplayName("Orientation")]
        [Description("Gets or sets the Orientation of this Tile.")]
        public MapTileOrientation Orientation
        {
            get { return orientation; }
            set
            {
                orientation = value;
                if (OnOrientationChanged != null)
                    OnOrientationChanged(value);
            }
        }

        /// <summary>
        /// Sets or gets the base Height Level.
        /// </summary>
        [DisplayName("Height Level")]
        [Description("Sets or gets the base Height Level.")]
        public byte HeightLevel
        {
            get { return heightLevel; }
            set
            {
                heightLevel = value;
                if (OnHeightLevelChanged != null)
                    OnHeightLevelChanged(value);
            }
        }

        /// <summary>
        /// Returns the Level to enter on the East Side.
        /// </summary>
        [DisplayName("Connection Level East")]
        [Description("Returns the Level on the East Side.")]
        public byte? ConnectionLevelEast
        {
            get
            {
                switch (Orientation)
                {
                    case MapTileOrientation.NotRotated: return GetConnectionLevelEast();
                    case MapTileOrientation.RotBy90Degrees: return GetConnectionLevelNorth();
                    case MapTileOrientation.RotBy180Degrees: return GetConnectionLevelWest();
                    case MapTileOrientation.RotBy270Degrees: return GetConnectionLevelSouth();
                    default: throw new NotSupportedException("Wrong Orientation Value");
                }
            }
        }

        /// <summary>
        /// Gets a call from ConnectionLevel[Orientation] depends on Map Tile Orientation.
        /// </summary>
        /// <returns>Height Level on an unrotated East Side</returns>
        protected virtual byte? GetConnectionLevelEast() { return null; }

        /// <summary>
        /// Returns the Level to enter on the South Side.
        /// </summary>
        [DisplayName("Connection Level South")]
        [Description("Returns the Level on the South Side.")]
        public byte? ConnectionLevelSouth
        {
            get
            {
                switch (Orientation)
                {
                    case MapTileOrientation.NotRotated: return GetConnectionLevelSouth();
                    case MapTileOrientation.RotBy90Degrees: return GetConnectionLevelEast();
                    case MapTileOrientation.RotBy180Degrees: return GetConnectionLevelNorth();
                    case MapTileOrientation.RotBy270Degrees: return GetConnectionLevelWest();
                    default: throw new NotSupportedException("Wrong Orientation Value");
                }
            }
        }

        /// <summary>
        /// Gets a call from ConnectionLevel[Orientation] depends on Map Tile Orientation.
        /// </summary>
        /// <returns>Height Level on an unrotated South Side</returns>
        protected virtual byte? GetConnectionLevelSouth() { return null; }

        /// <summary>
        /// Returns the Level to enter on the West Side.
        /// </summary>
        [DisplayName("Connection Level West")]
        [Description("Returns the Level on the West Side.")]
        public byte? ConnectionLevelWest
        {
            get
            {
                switch (Orientation)
                {
                    case MapTileOrientation.NotRotated: return GetConnectionLevelWest();
                    case MapTileOrientation.RotBy90Degrees: return GetConnectionLevelSouth();
                    case MapTileOrientation.RotBy180Degrees: return GetConnectionLevelEast();
                    case MapTileOrientation.RotBy270Degrees: return GetConnectionLevelNorth();
                    default: throw new NotSupportedException("Wrong Orientation Value");
                }
            }
        }

        /// <summary>
        /// Gets a call from ConnectionLevel[Orientation] depends on Map Tile Orientation.
        /// </summary>
        /// <returns>Height Level on an unrotated West Side</returns>
        protected virtual byte? GetConnectionLevelWest() { return null; }

        /// <summary>
        /// Returns the Level to enter on the North Side.
        /// </summary>
        [DisplayName("Connection Level North")]
        [Description("Returns the Level on the North Side.")]
        public byte? ConnectionLevelNorth
        {
            get
            {
                switch (Orientation)
                {
                    case MapTileOrientation.NotRotated: return GetConnectionLevelNorth();
                    case MapTileOrientation.RotBy90Degrees: return GetConnectionLevelWest();
                    case MapTileOrientation.RotBy180Degrees: return GetConnectionLevelSouth();
                    case MapTileOrientation.RotBy270Degrees: return GetConnectionLevelEast();
                    default: throw new NotSupportedException("Wrong Orientation Value");
                }
            }
        }

        /// <summary>
        /// Gets a call from ConnectionLevel[Orientation] depends on Map Tile Orientation.
        /// </summary>
        /// <returns>Height Level on an unrotated North Side</returns>
        protected virtual byte? GetConnectionLevelNorth() { return null; }

        /// <summary>
        /// Validates the current Map Tile against the given Tile.
        /// </summary>
        /// <param name="tile">Tile</param>
        /// <param name="exceptions">Result-List of occured Exceptions</param>
        public bool ValidateTileToTheEast(MapTile tile, IList<Exception> exceptions)
        {
            switch (Orientation)
            {
                case MapTileOrientation.NotRotated: return OnValidateEastSide(tile, exceptions);
                case MapTileOrientation.RotBy90Degrees: return OnValidateNorthSide(tile, exceptions);
                case MapTileOrientation.RotBy180Degrees: return OnValidateWestSide(tile, exceptions);
                case MapTileOrientation.RotBy270Degrees: return OnValidateSouthSide(tile, exceptions);
                default: throw new NotSupportedException("Wrong Orientation Value");
            }
        }

        /// <summary>
        /// Gets called to validate the Map Tile close to this one.
        /// </summary>
        /// <param name="tile">Neighbor Tile</param>
        /// <param name="exceptions">Result-List of occured Exceptions</param>
        protected virtual bool OnValidateEastSide(MapTile tile, IList<Exception> exceptions) { return true; }

        /// <summary>
        /// Validates the current Map Tile against the given Tile.
        /// </summary>
        /// <param name="tile">Tile</param>
        /// <param name="exceptions">Result-List of occured Exceptions</param>
        public virtual bool ValidateTileToTheSouth(MapTile tile, IList<Exception> exceptions)
        {
            switch (Orientation)
            {
                case MapTileOrientation.NotRotated: return OnValidateSouthSide(tile, exceptions);
                case MapTileOrientation.RotBy90Degrees: return OnValidateEastSide(tile, exceptions);
                case MapTileOrientation.RotBy180Degrees: return OnValidateNorthSide(tile, exceptions);
                case MapTileOrientation.RotBy270Degrees: return OnValidateWestSide(tile, exceptions);
                default: throw new NotSupportedException("Wrong Orientation Value");
            }
        }

        /// <summary>
        /// Gets called to validate the Map Tile close to this one.
        /// </summary>
        /// <param name="tile">Neighbor Tile</param>
        /// <param name="exceptions">Result-List of occured Exceptions</param>
        protected virtual bool OnValidateSouthSide(MapTile tile, IList<Exception> exceptions) { return true; }

        /// <summary>
        /// Validates the current Map Tile against the given Tile.
        /// </summary>
        /// <param name="tile">Tile</param>
        /// <param name="exceptions">Result-List of occured Exceptions</param>
        public virtual bool ValidateTileToTheWest(MapTile tile, IList<Exception> exceptions)
        {
            switch (Orientation)
            {
                case MapTileOrientation.NotRotated: return OnValidateWestSide(tile, exceptions);
                case MapTileOrientation.RotBy90Degrees: return OnValidateSouthSide(tile, exceptions);
                case MapTileOrientation.RotBy180Degrees: return OnValidateEastSide(tile, exceptions);
                case MapTileOrientation.RotBy270Degrees: return OnValidateNorthSide(tile, exceptions);
                default: throw new NotSupportedException("Wrong Orientation Value");
            }
        }

        /// <summary>
        /// Gets called to validate the Map Tile close to this one.
        /// </summary>
        /// <param name="tile">Neighbor Tile</param>
        /// <param name="exceptions">Result-List of occured Exceptions</param>
        protected virtual bool OnValidateWestSide(MapTile tile, IList<Exception> exceptions) { return true; }

        /// <summary>
        /// Validates the current Map Tile against the given Tile.
        /// </summary>
        /// <param name="tile">Tile</param>
        /// <param name="exceptions">Result-List of occured Exceptions</param>
        public virtual bool ValidateTileToTheNorth(MapTile tile, IList<Exception> exceptions)
        {
            switch (Orientation)
            {
                case MapTileOrientation.NotRotated: return OnValidateNorthSide(tile, exceptions);
                case MapTileOrientation.RotBy90Degrees: return OnValidateWestSide(tile, exceptions);
                case MapTileOrientation.RotBy180Degrees: return OnValidateSouthSide(tile, exceptions);
                case MapTileOrientation.RotBy270Degrees: return OnValidateEastSide(tile, exceptions);
                default: throw new NotSupportedException("Wrong Orientation Value");
            }
        }

        /// <summary>
        /// Gets called to validate the Map Tile close to this one.
        /// </summary>
        /// <param name="tile">Neighbor Tile</param>
        /// <param name="exceptions">Result-List of occured Exceptions</param>
        protected virtual bool OnValidateNorthSide(MapTile tile, IList<Exception> exceptions) { return true; }

        /// <summary>
        /// Returns the Height at the given Position.
        /// </summary>
        /// <param name="position">relative Position</param>
        /// <returns>Map Height</returns>
        public abstract float GetHeight(Vector2 position);

        /// <summary>
        /// Serializes the first Frame of this Map Tile.
        /// </summary>
        /// <param name="stream">Output Stream</param>
        /// <param name="version">Protocol Version</param>
        public virtual void SerializeFirst(BinaryWriter stream, byte version)
        {
            stream.Write((ushort)Orientation);
            stream.Write(HeightLevel);
        }

        /// <summary>
        /// Serializes following Frames. (Not supported in Map Tile)
        /// </summary>
        /// <param name="stream">Output Stream</param>
        /// <param name="version">Protocol Version</param>
        public void SerializeUpdate(BinaryWriter stream, byte version)
        {
            throw new NotSupportedException("Update is not supported for Map Tiles");
        }

        /// <summary>
        /// Deserializes the first Frame of this Map Tile.
        /// </summary>
        /// <param name="stream">Input Stream</param>
        /// <param name="version">Protocol Version</param>
        public virtual void DeserializeFirst(BinaryReader stream, byte version)
        {
            Orientation = (MapTileOrientation)stream.ReadUInt16();
            HeightLevel = stream.ReadByte();
        }

        /// <summary>
        /// Deserializes all following Frames. (Not supported in Map Tile)
        /// </summary>
        /// <param name="stream">Input Stream</param>
        /// <param name="version">Protocol Version</param>
        public void DeserializeUpdate(BinaryReader stream, byte version)
        {
            throw new NotSupportedException("Update is not supported for Map Tiles");
        }

        /// <summary>
        /// Signal for a changed Material.
        /// </summary>
        public event ValueUpdate<MapMaterial> OnMaterialChanged;

        /// <summary>
        /// Signal for a changed Orientation.
        /// </summary>
        public event ValueUpdate<MapTileOrientation> OnOrientationChanged;

        /// <summary>
        /// Signal for a changed HeightLevel.
        /// </summary>
        public event ValueUpdate<byte> OnHeightLevelChanged;
    }
}