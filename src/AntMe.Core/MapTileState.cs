using System;
using System.IO;

namespace AntMe
{
    /// <summary>
    /// Base Class for all Map Tile States.
    /// </summary>
    public abstract class MapTileState : PropertyList<MapTileStateProperty>, ISerializableState
    {
        /// <summary>
        /// Reference to the Map Tile.
        /// </summary>
        protected readonly MapTile MapTile;

        /// <summary>
        /// Height Level for this Tile.
        /// </summary>
        public int HeightLevel { get; set; }

        /// <summary>
        /// Material for this Tile.
        /// </summary>
        public MapMaterial Material { get; set; }

        /// <summary>
        /// Gets or sets the Orientation of this Tile.
        /// </summary>
        public Compass Orientation { get; set; }

        /// <summary>
        /// Default Constructor for the Deserializer.
        /// </summary>
        public MapTileState() : base() { }

        /// <summary>
        /// Default Constructor for the Type Mapper.
        /// </summary>
        /// <param name="mapTile">Related Tile</param>
        public MapTileState(MapTile mapTile) : base()
        {
            MapTile = mapTile;

            MapTile.OnHeightLevelChanged += (v) => { HeightLevel = v; };
            MapTile.OnMaterialChanged += (v) => { Material = v; };
            MapTile.OnOrientationChanged += (v) => { Orientation = v; };
        }

        public void DeserializeFirst(BinaryReader stream, byte version)
        {
            throw new NotImplementedException();
        }

        public void DeserializeUpdate(BinaryReader stream, byte version)
        {
            throw new NotImplementedException();
        }

        public void SerializeFirst(BinaryWriter stream, byte version)
        {
            throw new NotImplementedException();
        }

        public void SerializeUpdate(BinaryWriter stream, byte version)
        {
            throw new NotImplementedException();
        }
    }
}
