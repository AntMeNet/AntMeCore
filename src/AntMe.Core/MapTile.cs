using System;
using System.Collections.Generic;

namespace AntMe
{
    /// <summary>
    /// Base Class for Map Tiles
    /// </summary>
    public abstract class MapTile : PropertyList<MapTileProperty>
    {
        /// <summary>
        /// Contains the List of Items in this Cell (maintained by Engine).
        /// </summary>
        internal HashSet<Item> Items { get; private set; }

        public MapTile(byte heightLevel, bool canEnter)
        {
            CanEnter = canEnter;
            Items = new HashSet<Item>();
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