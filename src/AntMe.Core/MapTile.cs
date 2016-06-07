using System;
using System.ComponentModel;

namespace AntMe
{
    /// <summary>
    /// Base Class for Map Tiles
    /// </summary>
    public abstract class MapTile
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
    }
}