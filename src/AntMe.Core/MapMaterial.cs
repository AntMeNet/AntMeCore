﻿using System.ComponentModel;
using System.IO;

namespace AntMe
{
    /// <summary>
    /// Base Class for all Materials used in Maps.
    /// </summary>
    public abstract class MapMaterial : ISerializableState
    {
        /// <summary>
        /// Reference to the Simulation Context.
        /// </summary>
        protected readonly SimulationContext Context;

        private float speed;

        /// <summary>
        /// Default Constructor without Parameters.
        /// </summary>
        /// <param name="context">Simulation Context</param>
        public MapMaterial(SimulationContext context) : this(context, 1f) { }

        /// <summary>
        /// Default Constructor.
        /// </summary>
        /// <param name="context">Simulation Context</param>
        /// <param name="speed">Map Tile Speed</param>
        public MapMaterial(SimulationContext context, float speed)
        {
            Context = context;
            Speed = speed;
        }

        /// <summary>
        /// Speed Multiplier for walking Units.
        /// </summary>
        [DisplayName("Speed")]
        [Description("Speed Multiplier for walking Units.")]
        public float Speed
        {
            get { return speed; }
            protected set
            {
                speed = value;
                OnSpeedChanged?.Invoke(value);
            }
        }

        /// <summary>
        /// Serializes the first Frame of this State.
        /// </summary>
        /// <param name="stream">Output Stream</param>
        /// <param name="version">Protocol Version</param>
        public virtual void SerializeFirst(BinaryWriter stream, byte version)
        {
            stream.Write(Speed);
        }

        /// <summary>
        /// Serializes following Frames of this State.
        /// </summary>
        /// <param name="stream">Output Stream</param>
        /// <param name="version">Protocol Version</param>
        public virtual void SerializeUpdate(BinaryWriter stream, byte version)
        {
        }

        /// <summary>
        /// Deserializes the first Frame of this State.
        /// </summary>
        /// <param name="stream">Input Stream</param>
        /// <param name="version">Protocol Version</param>
        public virtual void DeserializeFirst(BinaryReader stream, byte version)
        {
            Speed = stream.ReadSingle();
        }

        /// <summary>
        /// Deserializes all following Frames of this State.
        /// </summary>
        /// <param name="stream">Input Stream</param>
        /// <param name="version">Protocol Version</param>
        public virtual void DeserializeUpdate(BinaryReader stream, byte version)
        {
        }

        /// <summary>
        /// Signal for a changed Speed.
        /// </summary>
        public ValueUpdate<float> OnSpeedChanged;
    }
}
