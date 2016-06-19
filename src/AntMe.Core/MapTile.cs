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

        private Compass orientation;

        private byte heightLevel;

        private Dictionary<Item, MapTileInfo> infos;

        /// <summary>
        /// Reference to the Simulation Context.
        /// </summary>
        protected readonly SimulationContext Context;

        /// <summary>
        /// List and Data of all unknown Property from Deserialization.
        /// </summary>
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
        [DisplayName("Material")]
        [Description("Gets or sets the Material.")]
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
        public Compass Orientation
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
        [DisplayName("Enter Level East")]
        [Description("Returns the Level to enter on the East Side.")]
        public abstract byte? ConnectionLevelEast { get; }

        /// <summary>
        /// Returns the Level to enter on the South Side.
        /// </summary>
        [DisplayName("Enter Level South")]
        [Description("Returns the Level to enter on the South Side.")]
        public abstract byte? ConnectionLevelSouth { get; }

        /// <summary>
        /// Returns the Level to enter on the West Side.
        /// </summary>
        [DisplayName("Enter Level West")]
        [Description("Returns the Level to enter on the West Side.")]
        public abstract byte? ConnectionLevelWest { get; }

        /// <summary>
        /// Returns the Level to enter on the North Side.
        /// </summary>
        [DisplayName("Enter Level North")]
        [Description("Returns the Level to enter on the North Side.")]
        public abstract byte? ConnectionLevelNorth { get; }

        /// <summary>
        /// Validates the current Map Tile against the given Tile.
        /// </summary>
        /// <param name="compass">Direction</param>
        /// <param name="tile">Tile</param>
        public virtual void ValidateAgainst(Compass compass, MapTile tile) { }

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
            Orientation = (Compass)stream.ReadUInt16();
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
        public event ValueUpdate<Compass> OnOrientationChanged;

        /// <summary>
        /// Signal for a changed HeightLevel.
        /// </summary>
        public event ValueUpdate<byte> OnHeightLevelChanged;
    }
}