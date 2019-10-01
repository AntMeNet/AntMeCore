using System;
using System.IO;

namespace AntMe
{
    /// <summary>
    ///     Base Class for all Map Properties.
    /// </summary>
    public abstract class MapProperty : Property, ISerializableState
    {
        protected readonly SimulationContext Context;

        /// <summary>
        ///     Reference to the Map.
        /// </summary>
        protected readonly Map Map;

        /// <summary>
        ///     Default Constructor for the Type Mapper.
        /// </summary>
        /// <param name="context">Simulation Context</param>
        /// <param name="map">Reference to the related Map</param>
        public MapProperty(SimulationContext context, Map map)
        {
            Context = context;
            Map = map;
        }

        /// <summary>
        ///     Serializes the Map Property.
        /// </summary>
        /// <param name="stream">Output Stream</param>
        /// <param name="version">Protocol Version</param>
        public abstract void DeserializeFirst(BinaryReader stream, byte version);

        /// <summary>
        ///     Serializes following Frames. (Not supported in Map Property)
        /// </summary>
        /// <param name="stream">Output Stream</param>
        /// <param name="version">Protocol Version</param>
        public void DeserializeUpdate(BinaryReader stream, byte version)
        {
            throw new NotSupportedException("Update is not supported for Map Property");
        }

        /// <summary>
        ///     Deserializes the first Frame of this Property.
        /// </summary>
        /// <param name="stream">Input Stream</param>
        /// <param name="version">Protocol Version</param>
        public abstract void SerializeFirst(BinaryWriter stream, byte version);

        /// <summary>
        ///     Deserializes all following Frames. (Not supported in Map Property)
        /// </summary>
        /// <param name="stream">Input Stream</param>
        /// <param name="version">Protocol Version</param>
        public void SerializeUpdate(BinaryWriter stream, byte version)
        {
            throw new NotSupportedException("Update is not supported for Map Property");
        }

        /// <summary>
        ///     Updates the current Map Property.
        /// </summary>
        /// <param name="round">Current Round</param>
        public abstract void Update(int round);
    }
}