using System;
using System.Collections.Generic;

namespace AntMe
{
    /// <summary>
    /// Base Class for Map Tiles
    /// </summary>
    public abstract class MapTile : PropertyList<MapTileProperty>
    {
        public MapTile(byte heightLevel, bool canEnter)
        {
            CanEnter = canEnter;
            HeightLevel = heightLevel;
        }

        public MapTileState GetState()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets or sets the Material.
        /// </summary>
        public MapMaterial Material { get; set; }

        /// <summary>
        /// Gets or sets the Orientation of this Tile.
        /// </summary>
        public Compass Orientation { get; set; }

        /// <summary>
        /// Sets or gets the base Height Level.
        /// </summary>
        public byte HeightLevel { get; private set; }

        /// <summary>
        /// Gets or sets the possibility to enter the Tile.
        /// </summary>
        public bool CanEnter { get; private set; }

        /// <summary>
        /// Returns the Height at the given Position.
        /// </summary>
        /// <param name="position">relative Position</param>
        /// <returns>Map Height</returns>
        public abstract float GetHeight(Vector2 position);
    }
}