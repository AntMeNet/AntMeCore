using System;

namespace AntMe.Basics.ItemProperties
{
    /// <summary>
    /// Set of visible Environment Information.
    /// </summary>
    public sealed class VisibleEnvironment
    {
        /// <summary>
        /// Information about the current Cell.
        /// </summary>
        public VisibleCell? Center { get; internal set; }

        /// <summary>
        /// Cell Information in North Direction.
        /// </summary>
        public VisibleCell? North { get; internal set; }

        /// <summary>
        /// Cell Information in South Direction.
        /// </summary>
        public VisibleCell? South { get; internal set; }

        /// <summary>
        /// Cell Information in West Direction.
        /// </summary>
        public VisibleCell? West { get; internal set; }

        /// <summary>
        /// Cell Information in East Direction.
        /// </summary>
        public VisibleCell? East { get; internal set; }

        /// <summary>
        /// Cell Information in Northwest Direction.
        /// </summary>
        public VisibleCell? NorthWest { get; internal set; }

        /// <summary>
        /// Cell Information in Northeast Direction.
        /// </summary>
        public VisibleCell? NorthEast { get; internal set; }

        /// <summary>
        /// Cell Information in Southwest Direction.
        /// </summary>
        public VisibleCell? SouthWest { get; internal set; }

        /// <summary>
        /// Cell Information in Southeast Direction.
        /// </summary>
        public VisibleCell? SouthEast { get; internal set; }

        /// <summary>
        ///     Liefert Zelleninfos in angegebener Richtung.
        /// </summary>
        /// <param name="compass">Kompass Information</param>
        /// <returns>Zellinfos</returns>
        //public VisibleCell? this[Compass compass]
        //{
        //    get
        //    {
        //        switch (compass)
        //        {
        //            case Compass.North:
        //                return North;
        //            case Compass.South:
        //                return South;
        //            case Compass.West:
        //                return West;
        //            case Compass.East:
        //                return East;
        //            case Compass.NorthWest:
        //                return NorthWest;
        //            case Compass.NorthEast:
        //                return NorthEast;
        //            case Compass.SouthWest:
        //                return SouthWest;
        //            case Compass.SouthEast:
        //                return SouthEast;
        //            default:
        //                throw new Exception("Unknown Compass Value");
        //        }
        //    }
        //}

        internal VisibleCell? this[Index2 coordinate]
        {
            get { return this[coordinate.X, coordinate.Y]; }
            set { this[coordinate.X, coordinate.Y] = value; }
        }

        /// <summary>
        ///     Liefert Zellen Informationen auf Basis relativer Koordinaten.
        /// </summary>
        /// <param name="x">X Koordinate im Bereich [-1...1]</param>
        /// <param name="y">Y Koordinate im Bereich [-1...1]</param>
        /// <returns>Zelleninfos</returns>
        internal VisibleCell? this[int x, int y]
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
            set
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