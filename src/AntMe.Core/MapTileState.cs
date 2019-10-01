using System.IO;

namespace AntMe
{
    /// <summary>
    ///     Base Class for all Map Tile States.
    /// </summary>
    public abstract class MapTileState : PropertyList<MapTileStateProperty>, ISerializableState
    {
        /// <summary>
        ///     Reference to the Map Tile.
        /// </summary>
        protected readonly MapTile MapTile;

        /// <summary>
        ///     Default Constructor for the Deserializer.
        /// </summary>
        public MapTileState()
        {
        }

        /// <summary>
        ///     Default Constructor for the Type Mapper.
        /// </summary>
        /// <param name="mapTile">Related Tile</param>
        public MapTileState(MapTile mapTile)
        {
            MapTile = mapTile;

            HeightLevel = MapTile.HeightLevel;
            MapTile.OnHeightLevelChanged += v => { HeightLevel = v; };

            Material = MapTile.Material;
            MapTile.OnMaterialChanged += v => { Material = v; };

            Orientation = MapTile.Orientation;
            MapTile.OnOrientationChanged += v => { Orientation = v; };
        }

        /// <summary>
        ///     Height Level for this Tile.
        /// </summary>
        public int HeightLevel { get; set; }

        /// <summary>
        ///     Material for this Tile.
        /// </summary>
        public MapMaterial Material { get; set; }

        /// <summary>
        ///     Gets or sets the Orientation of this Tile.
        /// </summary>
        public MapTileOrientation Orientation { get; set; }

        public void DeserializeFirst(BinaryReader stream, byte version)
        {
            HeightLevel = stream.ReadInt32();
            Orientation = (MapTileOrientation) stream.ReadUInt16();
        }

        public void DeserializeUpdate(BinaryReader stream, byte version)
        {
        }

        public void SerializeFirst(BinaryWriter stream, byte version)
        {
            stream.Write(HeightLevel);
            stream.Write((ushort) Orientation);
        }

        public void SerializeUpdate(BinaryWriter stream, byte version)
        {
        }
    }
}