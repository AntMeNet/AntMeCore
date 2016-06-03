namespace AntMe
{
    /// <summary>
    /// Base Class for all Materials used in Maps.
    /// </summary>
    public abstract class MapMaterial
    {
        public MapMaterial(float speed)
        {
            Speed = speed;
        }

        /// <summary>
        /// Speed Multiplier for walking Units.
        /// </summary>
        public float Speed { get; protected set; }
    }
}
