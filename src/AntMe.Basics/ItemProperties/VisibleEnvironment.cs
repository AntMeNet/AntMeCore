using System;
using System.ComponentModel;

namespace AntMe.ItemProperties.Basics
{
    /// <summary>
    ///     Umgebungsinformationen.
    /// </summary>
    public sealed class VisibleEnvironment
    {
        /// <summary>
        ///     Liefert Zelleninfos zur aktuellen Zelle
        /// </summary>
        [DisplayName("Center")]
        [Description("")]
        public VisibleCell? Center { get; internal set; }

        /// <summary>
        ///     Liefert Zelleninfos zur Zelle Richtung Norden.
        /// </summary>
        [DisplayName("North")]
        [Description("")]
        public VisibleCell? North { get; internal set; }

        /// <summary>
        ///     Liefert Zelleninfos zur Zelle Richtung Süden.
        /// </summary>
        [DisplayName("South")]
        [Description("")]
        public VisibleCell? South { get; internal set; }

        /// <summary>
        ///     Liefert Zelleninfos zur Zelle Richtung Westen.
        /// </summary>
        [DisplayName("West")]
        [Description("")]
        public VisibleCell? West { get; internal set; }

        /// <summary>
        ///     Liefert Zelleninfos zur Zelle Richtung Osten.
        /// </summary>
        [DisplayName("East")]
        [Description("")]
        public VisibleCell? East { get; internal set; }

        /// <summary>
        ///     Liefert Zelleninfos zur Zelle Richtung Nordwesten.
        /// </summary>
        [DisplayName("North West")]
        [Description("")]
        public VisibleCell? NorthWest { get; internal set; }

        /// <summary>
        ///     Liefert Zelleninfos zur Zelle Richtung Nordosten.
        /// </summary>
        [DisplayName("North East")]
        [Description("")]
        public VisibleCell? NorthEast { get; internal set; }

        /// <summary>
        ///     Liefert Zelleninfos zur Zelle Richtung Südwesten.
        /// </summary>
        [DisplayName("South West")]
        [Description("")]
        public VisibleCell? SouthWest { get; internal set; }

        /// <summary>
        ///     Liefert Zelleninfos zur Zelle Richtung Südosten.
        /// </summary>
        [DisplayName("South East")]
        [Description("")]
        public VisibleCell? SouthEast { get; internal set; }

        /// <summary>
        ///     Liefert Zelleninfos in angegebener Richtung.
        /// </summary>
        /// <param name="compass">Kompass Information</param>
        /// <returns>Zellinfos</returns>
        public VisibleCell? this[Compass compass]
        {
            get
            {
                switch (compass)
                {
                    case Compass.North:
                        return North;
                    case Compass.South:
                        return South;
                    case Compass.West:
                        return West;
                    case Compass.East:
                        return East;
                    case Compass.NorthWest:
                        return NorthWest;
                    case Compass.NorthEast:
                        return NorthEast;
                    case Compass.SouthWest:
                        return SouthWest;
                    case Compass.SouthEast:
                        return SouthEast;
                    default:
                        throw new Exception("Unknown Compass Value");
                }
            }
        }

        public VisibleCell? this[Index2 coordinate]
        {
            get { return this[coordinate.X, coordinate.Y]; }
            internal set { this[coordinate.X, coordinate.Y] = value; }
        }

        /// <summary>
        ///     Liefert Zellen Informationen auf Basis relativer Koordinaten.
        /// </summary>
        /// <param name="x">X Koordinate im Bereich [-1...1]</param>
        /// <param name="y">Y Koordinate im Bereich [-1...1]</param>
        /// <returns>Zelleninfos</returns>
        public VisibleCell? this[int x, int y]
        {
            get
            {
                x = Math.Min(1, Math.Max(-1, x));
                y = Math.Min(1, Math.Max(-1, y));

                switch (y)
                {
                    case -1:
                        switch (x)
                        {
                            case -1:
                                return NorthWest;
                            case 0:
                                return North;
                            case 1:
                                return NorthEast;
                            default:
                                throw new Exception("Unknown Y Parameter");
                        }
                    case 0:
                        switch (x)
                        {
                            case -1:
                                return West;
                            case 0:
                                return Center;
                            case 1:
                                return East;
                            default:
                                throw new Exception("Unknown Y Parameter");
                        }
                    case 1:
                        switch (x)
                        {
                            case -1:
                                return SouthWest;
                            case 0:
                                return South;
                            case 1:
                                return SouthEast;
                            default:
                                throw new Exception("Unknown Y Parameter");
                        }
                    default:
                        throw new Exception("Unknown X Parameter");
                }
            }
            internal set
            {
                x = Math.Min(1, Math.Max(-1, x));
                y = Math.Min(1, Math.Max(-1, y));

                switch (y)
                {
                    case -1:
                        switch (x)
                        {
                            case -1:
                                NorthWest = value;
                                break;
                            case 0:
                                North = value;
                                break;
                            case 1:
                                NorthEast = value;
                                break;
                            default:
                                throw new Exception("Unknown Y Parameter");
                        }
                        break;
                    case 0:
                        switch (x)
                        {
                            case -1:
                                West = value;
                                break;
                            case 0:
                                Center = value;
                                break;
                            case 1:
                                East = value;
                                break;
                            default:
                                throw new Exception("Unknown Y Parameter");
                        }
                        break;
                    case 1:
                        switch (x)
                        {
                            case -1:
                                SouthWest = value;
                                break;
                            case 0:
                                South = value;
                                break;
                            case 1:
                                SouthEast = value;
                                break;
                            default:
                                throw new Exception("Unknown Y Parameter");
                        }
                        break;
                    default:
                        throw new Exception("Unknown X Parameter");
                }
            }
        }
    }
}