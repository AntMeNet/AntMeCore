using System;

namespace AntMe
{
    /// <summary>
    /// Base Class for Map Tiles
    /// </summary>
    public abstract class MapTile
    {
        #region Constants

        /// <summary>
        ///     Geschwindigkeitsmultiplikator für Flächen mit dem Stop-Flag.
        /// </summary>
        public const float SPEED_STOP = 0.1f;

        /// <summary>
        ///     Geschwindigkeitsmultiplikator für Flächen mit dem Slowest-Flag.
        /// </summary>
        public const float SPEED_SLOWEST = 0.5f;

        /// <summary>
        ///     Geschwindigkeitsmultiplikator für Flächen mit dem Stlower-Flag.
        /// </summary>
        public const float SPEED_SLOWER = 0.8f;

        /// <summary>
        ///     Geschwindigkeitsmultiplikator für Flächen mit dem Normal-Flag.
        /// </summary>
        public const float SPEED_NORMAL = 1f;

        /// <summary>
        ///     Geschwindigkeitsmultiplikator für Flächen mit dem Faster-Flag.
        /// </summary>
        public const float SPEED_FASTER = 1.2f;

        /// <summary>
        ///     Höhen in Simulationseinheiten einer Fläche mit dem Low-Flag.
        /// </summary>
        public const float HEIGHT_LOW = 0f;

        /// <summary>
        ///     Höhen in Simulationseinheiten einer Fläche mit dem Medium-Flag.
        /// </summary>
        public const float HEIGHT_MEDIUM = 10f;

        /// <summary>
        ///     Höhen in Simulationseinheiten einer Fläche mit dem High-Flag.
        /// </summary>
        public const float HEIGHT_HIGH = 20f;

        #endregion

        public MapTile(byte heightLevel, bool canEnter)
        {
            CanEnter = canEnter;
        }

        /// <summary>
        /// Gets or sets the Material.
        /// </summary>
        public MapMaterial Material { get; set; }

        /// <summary>
        /// Sets or gets the base Height Level.
        /// </summary>
        public byte HeightLevel { get; private set; }

        /// <summary>
        /// Gets or sets the possibility to enter the Tile.
        /// </summary>
        public bool CanEnter { get; private set; }

        /// <summary>
        /// Returns the Height at the given Position.
        /// </summary>
        /// <param name="position">relative Position</param>
        /// <returns>Map Height</returns>
        public abstract float GetHeight(Vector2 position);
    }

    /// <summary>
    ///     Liste der möglichen Zellen-Typen.
    /// </summary>
    public enum TileShape
    {
        /// <summary>
        ///     Ebene Fläche.
        /// </summary>
        Flat = 0x00,

        /// <summary>
        ///     Rampe nach Süden.
        /// </summary>
        RampBottom = 0x10,
        /// <summary>
        ///     Rampe nach Osten.
        /// </summary>
        RampRight = 0x11,
        /// <summary>
        ///     Rampe nach Norden.
        /// </summary>
        RampTop = 0x12,
        /// <summary>
        ///     Rampe nach Westen.
        /// </summary>
        RampLeft = 0x13,

        /// <summary>
        ///     Gerade Klippe nach Süden.
        /// </summary>
        CanyonBottom = 0x20,
        /// <summary>
        ///     Gerade Klippe nach Osten.
        /// </summary>
        CanyonRight = 0x21,
        /// <summary>
        ///     Gerade Klippe nach Nordern.
        /// </summary>
        CanyonTop = 0x22,
        /// <summary>
        ///     Gerade Klippe nach Westen.
        /// </summary>
        CanyonLeft = 0x23,

        /// <summary>
        ///     Innere Klippe nach Südwesten.
        /// </summary>
        CanyonUpperRightConcave = 0x30,
        /// <summary>
        ///     Innere Klippe nach Südosten.
        /// </summary>
        CanyonUpperLeftConcave = 0x31,
        /// <summary>
        ///     Innere Klippe nach Nordosten.
        /// </summary>
        CanyonLowerLeftConcave = 0x32,
        /// <summary>
        ///     Innere Klippe nach Nordwesten.
        /// </summary>
        CanyonLowerRightConcave = 0x33,

        /// <summary>
        ///     Äußere Klippe nach Südwesten.
        /// </summary>
        CanyonLowerLeftConvex = 0x40,
        /// <summary>
        ///     Äußere Klippe nach Südosten.
        /// </summary>
        CanyonLowerRightConvex = 0x41,
        /// <summary>
        ///     Äußere Klippe nach Nordosten.
        /// </summary>
        CanyonUpperRightConvex = 0x42,
        /// <summary>
        ///     Äußere Klippe nach Nordwesten.
        /// </summary>
        CanyonUpperLeftConvex = 0x43,

    }
}