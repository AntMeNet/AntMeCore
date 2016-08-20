namespace AntMe
{
    /// <summary>
    /// Static List of the eight main compass points.
    /// Direct cast to <see cref="System.Int32" /> returns an angle value.
    /// <code>
    /// int degree = (int)Compass.West;
    /// </code>
    /// </summary>
    public enum Compass
    {
        /// <summary>
        /// East with <seealso cref="Angle" /> 0 degreees.
        /// </summary>
        East = 0,

        /// <summary>
        /// Southeast with <seealso cref="Angle" /> 45 degrees.
        /// </summary>
        SouthEast = 45,

        /// <summary>
        /// South with <seealso cref="Angle" /> 90 degrees.
        /// </summary>
        South = 90,

        /// <summary>
        /// Southwest with <seealso cref="Angle" /> 135 degrees.
        /// </summary>
        SouthWest = 135,

        /// <summary>
        /// West with <seealso cref="Angle" /> 180 degrees.
        /// </summary>
        West = 180,

        /// <summary>
        /// Northwest with <seealso cref="Angle" /> 225 degrees.
        /// </summary>
        NorthWest = 225,

        /// <summary>
        /// North with <seealso cref="Angle" /> 270 degrees.
        /// </summary>
        North = 270,

        /// <summary>
        /// Northeast with <seealso cref="Angle" /> 315 degrees.
        /// </summary>
        NorthEast = 315
    }
}