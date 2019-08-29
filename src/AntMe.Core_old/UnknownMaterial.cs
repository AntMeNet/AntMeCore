namespace AntMe
{
    /// <summary>
    /// Placeholder Class for unknown Material Types.
    /// </summary>
    public sealed class UnknownMaterial : MapMaterial
    {
        /// <summary>
        /// Name of the origianl Material Type.
        /// </summary>
        public string MaterialType { get; private set; }

        /// <summary>
        /// Returns the Payload.
        /// </summary>
        public byte[] Payload { get; private set; }

        /// <summary>
        /// Default Constructor for the Deserializer.
        /// </summary>
        /// <param name="context">Simulation Context</param>
        internal UnknownMaterial(SimulationContext context) : this(context, string.Empty, null) { }

        /// <summary>
        /// Default Constructor for the Deserializer.
        /// </summary>
        /// <param name="context">Simulation Context</param>
        /// <param name="materialType">Material Type</param>
        /// <param name="payload">Payload</param>
        internal UnknownMaterial(SimulationContext context, string materialType, byte[] payload) : base(context)
        {
            MaterialType = materialType;
            Payload = payload;
        }
    }
}
