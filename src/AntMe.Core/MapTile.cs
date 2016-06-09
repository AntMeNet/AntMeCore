using System;
using System.ComponentModel;
using System.IO;

namespace AntMe
{
    /// <summary>
    /// Base Class for Map Tiles
    /// </summary>
    public abstract class MapTile : PropertyList<MapTileProperty>, ISerializableState
    {
        /// <summary>
        /// Default Constructor.
        /// </summary>
        /// <param name="canEnter">Can Item Enter?</param>
        public MapTile(bool canEnter)
        {
            CanEnter = canEnter;
        }

        /// <summary>
        /// Generates the Map Tile State.
        /// </summary>
        /// <returns></returns>
        public MapTileState GetState()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets or sets the Material.
        /// </summary>
        [DisplayName("Material")]
        [Description("Gets or sets the Material.")]
        public MapMaterial Material { get; set; }

        /// <summary>
        /// Gets or sets the Orientation of this Tile.
        /// </summary>
        [DisplayName("Orientation")]
        [Description("Gets or sets the Orientation of this Tile.")]
        public Compass Orientation { get; set; }

        /// <summary>
        /// Sets or gets the base Height Level.
        /// </summary>
        [DisplayName("Height Level")]
        [Description("Sets or gets the base Height Level.")]
        public byte HeightLevel { get; set; }

        /// <summary>
        /// Gets or sets the possibility to enter the Tile.
        /// </summary>
        [DisplayName("Orientation")]
        [Description("Gets or sets the Orientation of this Tile.")]
        public bool CanEnter { get; private set; }

        /// <summary>
        /// Returns the Level to enter on the East Side.
        /// </summary>
        [DisplayName("Enter Level East")]
        [Description("Returns the Level to enter on the East Side.")]
        public abstract byte? EnterLevelEast { get; }

        /// <summary>
        /// Returns the Level to enter on the South Side.
        /// </summary>
        [DisplayName("Enter Level South")]
        [Description("Returns the Level to enter on the South Side.")]
        public abstract byte? EnterLevelSouth { get; }

        /// <summary>
        /// Returns the Level to enter on the West Side.
        /// </summary>
        [DisplayName("Enter Level West")]
        [Description("Returns the Level to enter on the West Side.")]
        public abstract byte? EnterLevelWest { get; }

        /// <summary>
        /// Returns the Level to enter on the North Side.
        /// </summary>
        [DisplayName("Enter Level North")]
        [Description("Returns the Level to enter on the North Side.")]
        public abstract byte? EnterLevelNorth { get; }

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
            stream.Write(CanEnter);
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
            CanEnter = stream.ReadBoolean();
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
    }
}