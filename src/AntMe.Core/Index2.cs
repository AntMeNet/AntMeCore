using System;

namespace AntMe
{
    /// <summary>
    /// Datentyp zur Speicherung von Zell-Koordinaten mit X- und Y-Anteil.
    /// </summary>
    [Serializable]
    public struct Index2
    {
        /// <summary>
        /// Index in X-Richtung.
        /// </summary>
        public int X;

        /// <summary>
        /// Index in Y-Richtung.
        /// </summary>
        public int Y;

        /// <summary>
        /// Erstellt eine neue Index2-Instanz.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Index2(int x, int y)
        {
            X = x;
            Y = y;
        }

        #region Konstanten

        /// <summary>
        ///     Null Index.
        /// </summary>
        public static Index2 Zero
        {
            get { return new Index2(0, 0); }
        }

        /// <summary>
        ///     Index nach oben.
        /// </summary>
        public static Index2 Up
        {
            get { return new Index2(0, -1); }
        }


        /// <summary>
        ///     Index nach unten.
        /// </summary>
        public static Index2 Down
        {
            get { return new Index2(0, 1); }
        }

        /// <summary>
        ///     Index nach left.
        /// </summary>
        public static Index2 Left
        {
            get { return new Index2(-1, 0); }
        }

        /// <summary>
        ///     Index nach rechts.
        /// </summary>
        public static Index2 Right
        {
            get { return new Index2(1, 0); }
        }

        /// <summary>
        ///     Index nach oben links.
        /// </summary>
        public static Index2 UpperLeft
        {
            get { return new Index2(-1, -1); }
        }

        /// <summary>
        ///     Index nach oben rechts.
        /// </summary>
        public static Index2 UpperRight
        {
            get { return new Index2(1, -1); }
        }

        /// <summary>
        ///     Index nach unten links.
        /// </summary>
        public static Index2 LowerLeft
        {
            get { return new Index2(-1, 1); }
        }

        /// <summary>
        ///     Index nach unten rechts.
        /// </summary>
        public static Index2 LowerRight
        {
            get { return new Index2(1, 1); }
        }

        #endregion

        #region Operator

        /// <summary>
        /// Addiert zwei Index2.
        /// </summary>
        /// <param name="a">Index a</param>
        /// <param name="b">Index b</param>
        /// <returns>Addierter Wert</returns>
        public static Index2 operator +(Index2 a, Index2 b)
        {
            return new Index2(a.X + b.X, a.Y + b.Y);
        }

        /// <summary>
        /// Subtrahiert zwei Index2.
        /// </summary>
        /// <param name="a">Index a</param>
        /// <param name="b">Index b</param>
        /// <returns>Subtrahierter Wert</returns>
        public static Index2 operator -(Index2 a, Index2 b)
        {
            return new Index2(a.X - b.X, a.Y - b.Y);
        }

        /// <summary>
        /// Multipliziert einen Index2 mit einem Skalierungsfaktor.
        /// </summary>
        /// <param name="a">Index</param>
        /// <param name="scale">Skalierungsfaktor</param>
        /// <returns>Skalierter Index2</returns>
        public static Index2 operator *(Index2 a, int scale)
        {
            return new Index2(a.X * scale, a.Y * scale);
        }

        /// <summary>
        /// Dividiert einen Index2 mit einem Skalierungsdivisor.
        /// </summary>
        /// <param name="a">Index</param>
        /// <param name="scale">Divisor</param>
        /// <returns></returns>
        public static Index2 operator /(Index2 a, int scale)
        {
            return new Index2(a.X / scale, a.Y / scale);
        }

        /// <summary>
        /// Vergleicht zwei Index2 auf Gleichheit.
        /// </summary>
        /// <param name="a">Index a</param>
        /// <param name="b">Index b</param>
        /// <returns>a gleich b?</returns>
        public static bool operator ==(Index2 a, Index2 b)
        {
            return (a.X == b.X && a.Y == b.Y);
        }

        /// <summary>
        /// Vergleicht zwei Index auf Ungleichheit.
        /// </summary>
        /// <param name="a">Index a</param>
        /// <param name="b">Index b</param>
        /// <returns>a ungleich b?</returns>
        public static bool operator !=(Index2 a, Index2 b)
        {
            return !(a == b);
        }

        #endregion

        /// <summary>
        /// Gibt den Wert des Index2 in Form von {x}/{y} zurück.
        /// </summary>
        /// <returns>Wert als Zeichenhette</returns>
        public override string ToString()
        {
            return string.Format("{0}/{1}", X, Y);
        }

        /// <summary>
        /// Ermittelt den Hashwert dieses Index2.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return X.GetHashCode() + Y.GetHashCode();
        }

        /// <summary>
        /// Vergleicht die Werte zweiter Index2 auf Gleichheit.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (!(obj is Index2))
                return false;

            var other = (Index2)obj;
            return (X == other.X && Y == other.Y);
        }
    }
}