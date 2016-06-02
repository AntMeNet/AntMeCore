using System;

namespace AntMe
{
    /// <summary>
    ///     Struktur zur Sammlung von zellenspezifischen Informationen der Karte.
    /// </summary>
    [Serializable]
    public struct MapTile
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

        /// <summary>
        ///     Der Shape dieser Zelle.
        /// </summary>
        public TileShape Shape { get; set; }

        /// <summary>
        ///     Geschwindigkeitsmodifikator dieser Zelle. Hat nur bei flachen
        ///     Bereichen und auf Rampen Einfluss.
        /// </summary>
        public TileSpeed Speed { get; set; }

        /// <summary>
        ///     Höhenangaben dieser Zelle. Bei Rampen und Canyons gibt dieser Wert
        ///     die Höhe der tiefsten Stelle an.
        /// </summary>
        public TileHeight Height { get; set; }

        #region static Methods

        /// <summary>
        ///     Ermittelt den Geschwindigkeitsmultiplikator zum angegebenen Parameter.
        /// </summary>
        /// <param name="speed">Geschwindigkeit</param>
        /// <returns>Speedmultiplikator</returns>
        public static float GetSpeedMultiplicator(TileSpeed speed)
        {
            switch (speed)
            {
                case TileSpeed.Stop:
                    return SPEED_STOP;
                case TileSpeed.Slowest:
                    return SPEED_SLOWEST;
                case TileSpeed.Slower:
                    return SPEED_SLOWER;
                case TileSpeed.Normal:
                    return SPEED_NORMAL;
                case TileSpeed.Faster:
                    return SPEED_FASTER;
                default:
                    throw new NotSupportedException("Unknown TileSpeed Type");
            }
        }

        #endregion

        /// <summary>
        ///     Gibt den resultierenden Geschwindigkeitsmultiplikator zurück.
        /// </summary>
        /// <returns>Geschwindigkeitsmultiplikator</returns>
        public float GetSpeedMultiplicator()
        {
            return GetSpeedMultiplicator(Speed);
        }

        /// <summary>
        ///     Gibt zurück, ob diese Zelle begehbar ist.
        /// </summary>
        /// <returns>Zelle kann betreten werden</returns>
        public bool CanEnter()
        {
            return (Shape == TileShape.Flat ||
                    Shape == TileShape.RampTop ||
                    Shape == TileShape.RampBottom ||
                    Shape == TileShape.RampLeft ||
                    Shape == TileShape.RampRight);
        }
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

    /// <summary>
    ///     Liste der möglichen Höhenlayern.
    /// </summary>
    public enum TileHeight
    {
        /// <summary>
        ///     Niedrig.
        /// </summary>
        Low = 1,

        /// <summary>
        ///     Mittel.
        /// </summary>
        Medium = 2,

        /// <summary>
        ///     Hoch.
        /// </summary>
        High = 3
    }

    /// <summary>
    ///     Liste möglicher Zellen-Geschwindigkeiten.
    /// </summary>
    public enum TileSpeed
    {
        /// <summary>
        ///     Nahezu voller Stop (10%).
        /// </summary>
        Stop = 1,

        /// <summary>
        ///     Sehr langsam (50%).
        /// </summary>
        Slowest = 2,

        /// <summary>
        ///     Langsamer (80%).
        /// </summary>
        Slower = 3,

        /// <summary>
        ///     Normale Geschwindigkeit (100%).
        /// </summary>
        Normal = 4,

        /// <summary>
        ///     Schneller (120%).
        /// </summary>
        Faster = 5,
    }
}