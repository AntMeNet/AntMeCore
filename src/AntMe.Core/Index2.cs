using System;

namespace AntMe
{
    /// <summary>
    /// Data type to store cell coordinates with X- and Y-Part.
    /// </summary>
    [Serializable]
    public struct Index2
    {
        /// <summary>
        /// Index X-Direction.
        /// </summary>
        public int X;

        /// <summary>
        /// Index Y-Direction.
        /// </summary>
        public int Y;

        /// <summary>
        /// Creates a new Index2-Instance.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Index2(int x, int y)
        {
            X = x;
            Y = y;
        }

        #region Constants

        /// <summary>
        ///     Null Index
        /// </summary>
        public static Index2 Zero
        {
            get { return new Index2(0, 0); }
        }

        /// <summary>
        ///     Index Up
        /// </summary>
        public static Index2 Up
        {
            get { return new Index2(0, -1); }
        }


        /// <summary>
        ///     Index Down
        /// </summary>
        public static Index2 Down
        {
            get { return new Index2(0, 1); }
        }

        /// <summary>
        ///     Index Left
        /// </summary>
        public static Index2 Left
        {
            get { return new Index2(-1, 0); }
        }

        /// <summary>
        ///     Index Right
        /// </summary>
        public static Index2 Right
        {
            get { return new Index2(1, 0); }
        }

        /// <summary>
        ///     Index Upper Left
        /// </summary>
        public static Index2 UpperLeft
        {
            get { return new Index2(-1, -1); }
        }

        /// <summary>
        ///     Index Upper Right
        /// </summary>
        public static Index2 UpperRight
        {
            get { return new Index2(1, -1); }
        }

        /// <summary>
        ///     Index Lower Left
        /// </summary>
        public static Index2 LowerLeft
        {
            get { return new Index2(-1, 1); }
        }

        /// <summary>
        ///     Index Lower Right
        /// </summary>
        public static Index2 LowerRight
        {
            get { return new Index2(1, 1); }
        }

        #endregion

        #region Operators

        /// <summary>
        /// Adds two Index2.
        /// </summary>
        /// <param name="a">Index a</param>
        /// <param name="b">Index b</param>
        /// <returns>Added Value</returns>
        public static Index2 operator +(Index2 a, Index2 b)
        {
            return new Index2(a.X + b.X, a.Y + b.Y);
        }

        /// <summary>
        /// Subtract two Index2.
        /// </summary>
        /// <param name="a">Index a</param>
        /// <param name="b">Index b</param>
        /// <returns>Substracted Value </returns>
        public static Index2 operator -(Index2 a, Index2 b)
        {
            return new Index2(a.X - b.X, a.Y - b.Y);
        }

        /// <summary>
        /// Multiplies an Index2 with a scaling factor.
        /// </summary>
        /// <param name="a">Index</param>
        /// <param name="scale">Scaling Factor</param>
        /// <returns>Skalierter Index2</returns>
        public static Index2 operator *(Index2 a, int scale)
        {
            return new Index2(a.X * scale, a.Y * scale);
        }

        /// <summary>
        /// Divides an Index2 with a scaling divisor.
        /// </summary>
        /// <param name="a">Index</param>
        /// <param name="scale">Divisor</param>
        /// <returns></returns>
        public static Index2 operator /(Index2 a, int scale)
        {
            return new Index2(a.X / scale, a.Y / scale);
        }

        /// <summary>
        /// Compares two Index2 of equality.
        /// </summary>
        /// <param name="a">Index a</param>
        /// <param name="b">Index b</param>
        /// <returns>a same like b?</returns>
        public static bool operator ==(Index2 a, Index2 b)
        {
            return (a.X == b.X && a.Y == b.Y);
        }

        /// <summary>
        /// Compares two Index2 of disparity
        /// </summary>
        /// <param name="a">Index a</param>
        /// <param name="b">Index b</param>
        /// <returns>a disparate b?</returns>
        public static bool operator !=(Index2 a, Index2 b)
        {
            return !(a == b);
        }

        #endregion

        /// <summary>
        /// Returns the value of Index2 like {x}/{y}.
        /// </summary>
        /// <returns>Value as String.</returns>
        public override string ToString()
        {
            return string.Format("{0}/{1}", X, Y);
        }

        /// <summary>
        /// Returns the hash value of Index2.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return X.GetHashCode() + Y.GetHashCode();
        }

        /// <summary>
        /// Compares the value of two Index2 of equality.
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