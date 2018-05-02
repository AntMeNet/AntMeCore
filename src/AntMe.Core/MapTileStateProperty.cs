using System.IO;

namespace AntMe
{
    /// <summary>
    /// Base Class for all Map Tile Properties States.
    /// </summary>
    public class MapTileStateProperty : StateProperty
    {
        /// <summary>
        /// Reference to the Map Tile.
        /// </summary>
        protected readonly MapTile MapTile;

        /// <summary>
        /// Reference to the Map Tile Property.
        /// </summary>
        protected new readonly MapTileProperty Property;

        /// <summary>
        /// Default Constructor for the Deserializer.
        /// </summary>
        public MapTileStateProperty() { }

        /// <summary>
        /// Default Constructor for the Type Mapper.
        /// </summary>
        /// <param name="mapTile">Map Tile</param>
        /// <param name="property">Map Tile Property</param>
        public MapTileStateProperty(MapTile mapTile, MapTileProperty property) : base(property)
        {
            MapTile = mapTile;
            Property = property;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="version"></param>
        public override void DeserializeFirst(BinaryReader stream, byte version)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="version"></param>
        public override void DeserializeUpdate(BinaryReader stream, byte version)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="version"></param>
        public override void SerializeFirst(BinaryWriter stream, byte version)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="version"></param>
        public override void SerializeUpdate(BinaryWriter stream, byte version)
        {
        }
    }
}
