namespace AntMe
{
    /// <summary>
    /// Statische Liste der 8 Haupt-Himmelsrichtungen. Direkter Cast nach
    /// <see cref="System.Int32" /> liefert eine Winkelangabe.
    /// <code>
    /// int degree = (int)Compass.West;
    /// </code>
    /// </summary>
    public enum Compass
    {
        /// <summary>
        /// Osten mit dem <seealso cref="Angle" /> von 0.
        /// </summary>
        East = 0,

        /// <summary>
        /// Südosten mit dem <seealso cref="Angle" /> von 45.
        /// </summary>
        SouthEast = 45,

        /// <summary>
        /// Süden mit dem <seealso cref="Angle" /> von 90.
        /// </summary>
        South = 90,

        /// <summary>
        /// Südwesten mit dem <seealso cref="Angle" /> von 135.
        /// </summary>
        SouthWest = 135,

        /// <summary>
        /// West mit dem <seealso cref="Angle" /> von 180.
        /// </summary>
        West = 180,

        /// <summary>
        /// Nordwesten mit dem <seealso cref="Angle" /> von 225.
        /// </summary>
        NorthWest = 225,

        /// <summary>
        /// Norden mit dem <seealso cref="Angle" /> von 270.
        /// </summary>
        North = 270,

        /// <summary>
        /// Nordosten mit dem <seealso cref="Angle" /> von 315.
        /// </summary>
        NorthEast = 315
    }
}