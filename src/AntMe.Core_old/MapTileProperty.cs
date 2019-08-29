
using System;
using System.IO;

namespace AntMe
{
    /// <summary>
    /// Base Class for all Map Tile Properties.
    /// </summary>
    public abstract class MapTileProperty : Property, ISerializableState
    {
        /// <summary>
        /// Reference to the Simulation Context.
        /// </summary>
        protected readonly SimulationContext Context;

        /// <summary>
        /// Reference to the related Map Tile.
        /// </summary>
        protected readonly MapTile MapTile;

        /// <summary>
        /// Default Constructor for the Type Mapper.
        /// </summary>
        /// <param name="context">Simulation Context</param>
        /// <param name="mapTile">Related Map Tile</param>
        public MapTileProperty(SimulationContext context, MapTile mapTile) : base()
        {
            Context = context;
            MapTile = mapTile;
        }

        /// <summary>
        /// Serializes the first Frame of this Property.
        /// </summary>
        /// <param name="stream">Output Stream</param>
        /// <param name="version">Protocol Version</param>
        public abstract void SerializeFirst(BinaryWriter stream, byte version);

        /// <summary>
        /// Serializes following Frames. (Not supported in Map Tile Properties)
        /// </summary>
        /// <param name="stream">Output Stream</param>
        /// <param name="version">Protocol Version</param>
        public void SerializeUpdate(BinaryWriter stream, byte version)
        {
            throw new NotSupportedException("Update is not supported for Map Tile Properties");
        }

        /// <summary>
        /// Deserializes the first Frame of this Map Tile Property.
        /// </summary>
        /// <param name="stream">Input Stream</param>
        /// <param name="version">Protocol Version</param>
        public abstract void DeserializeFirst(BinaryReader stream, byte version);

        /// <summary>
        /// Deserializes all following Frames. (Not supported in Map Tile Property)
        /// </summary>
        /// <param name="stream">Input Stream</param>
        /// <param name="version">Protocol Version</param>
        public void DeserializeUpdate(BinaryReader stream, byte version)
        {
            throw new NotSupportedException("Update is not supported for Map Tile Properties");
        }


    }
}
