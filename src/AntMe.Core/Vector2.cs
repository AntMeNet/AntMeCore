using System;

namespace AntMe
{
    /// <summary>
    /// Represents a 2D Vector
    /// </summary>
    [Serializable]
    public struct Vector2
    {
        /// <summary>
        /// Gets the hard wired value of a Sin45
        /// </summary>
        private const float Sin45 = 0.70710678118654752440084436210485f;

        /// <summary>
        /// Minimum value between two values before it's handled as equal
        /// </summary>
        public const float EpsMin = 0.001f;

        /// <summary>
        /// X-Axis
        /// </summary>
        public float X { get; }

        /// <summary>
        /// Y-Axsis
        /// </summary>
        public float Y { get; }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="x">X-Axis</param>
        /// <param name="y">Y-Axis</param>
        public Vector2(float x, float y)
        {
            X = x;
            Y = y;
        }

        #region Konstanten

        /// <summary>
        /// Zero vector
        /// </summary>
        public static Vector2 Zero => new Vector2(0, 0);

        /// <summary>
        /// Normalized vector to the left
        /// </summary>
        public static Vector2 Left => new Vector2(-1, 0);

        /// <summary>
        /// Normalized vector to the upper left
        /// </summary>
        public static Vector2 UpperLeft => new Vector2(-Sin45, -Sin45);

        /// <summary>
        /// Normalized vector to the lower left
        /// </summary>
        public static Vector2 LowerLeft => new Vector2(-Sin45, Sin45);

        /// <summary>
        /// Normalized vector to the right
        /// </summary>
        public static Vector2 Right => new Vector2(1, 0);

        /// <summary>
        /// Normalized vector to the upper right
        /// </summary>
        public static Vector2 UpperRight => new Vector2(Sin45, -Sin45);

        /// <summary>
        /// Normalized vector to the lower right
        /// </summary>
        public static Vector2 LowerRight => new Vector2(Sin45, Sin45);

        /// <summary>
        /// Normalized vector to up
        /// </summary>
        public static Vector2 Up => new Vector2(0, -1);

        /// <summary>
        /// Normalized vector to down
        /// </summary>
        public static Vector2 Down => new Vector2(0, 1);

        #endregion

        #region Static Helper

        /// <summary>
        /// Generates a normalized vector based on the given angle
        /// </summary>
        /// <param name="angle">angle</param>
        /// <returns>normalized vector</returns>
        public static Vector2 FromAngle(Angle angle)
        {
            return FromAngle(angle.Radian);
        }

        /// <summary>
        /// Generates a normalized vector based on the given angle
        /// </summary>
        /// <param name="radian">angle</param>
        /// <returns>normalized vector</returns>
        public static Vector2 FromAngle(float radian)
        {
            return new Vector2(
                (float)Math.Cos(radian),
                (float)Math.Sin(radian));
        }

        #endregion

        /// <summary>
        /// inverts the value of the x-axis
        /// </summary>
        /// <returns>inverted vector</returns>
        public Vector2 InvertX()
        {
            return new Vector2(X * -1, Y);
        }

        /// <summary>
        /// Inverts the value of the y-xis
        /// </summary>
        /// <returns>inverted vector</returns>
        public Vector2 InvertY()
        {
            return new Vector2(X, Y * -1);
        }

        /// <summary>
        /// Inverts vector on x- and y-axis. It turns by 180 degrees
        /// </summary>
        /// <returns>Neuer Vektor</returns>
        public Vector2 InvertXy()
        {
            return new Vector2(X * -1, Y * -1);
        }

        /// <summary>
        /// Calculates the sqare length of the vector
        /// </summary>
        /// <returns>Square length</returns>
        public float LengthSquared()
        {
            return X * X + Y * Y;
        }

        /// <summary>
        /// Calculates the length of the vector
        /// </summary>
        /// <returns>Length</returns>
        public float Length()
        {
            return (float)Math.Sqrt(LengthSquared());
        }

        /// <summary>
        /// Calculates the direction of the vector
        /// </summary>
        /// <returns>Angle or direction<see cref="Angle" /></returns>
        public Angle ToAngle()
        {
            return new Angle((float)Math.Atan2(Y, X));
        }

        /// <summary>
        /// Normalizes the vector
        /// </summary>
        /// <returns>Normalisierter Vektor</returns>
        public Vector2 Normalize()
        {
            // Special case of Zero (returns again zero)
            if (this == Zero)
                return Zero;

            float scale = 1 / Length();
            return this * scale;
        }

        /// <summary>
        /// Compares two vectors for equality
        /// </summary>
        /// <param name="other">compared vector</param>
        /// <returns>true if equal</returns>
        public override bool Equals(object other)
        {
            if (!(other is Vector2))
                return false;

            var b = (Vector2)other;
            return (Math.Abs(X - b.X) < EpsMin && Math.Abs(Y - b.Y) < EpsMin);
        }

        /// <summary>
        /// Generates the hash code of the vector
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            return X.GetHashCode() + Y.GetHashCode();
        }

        /// <summary>
        /// Returns the value of the vector in x/y format
        /// </summary>
        /// <returns>value as string</returns>
        public override string ToString()
        {
            return $"{X:0.000}/{Y:0.000}";
        }

        #region Operator

        /// <summary>
        /// Addition between two vectors
        /// </summary>
        /// <param name="a">Vector a</param>
        /// <param name="b">Vector b</param>
        /// <returns></returns>
        public static Vector2 operator +(Vector2 a, Vector2 b)
        {
            return new Vector2(a.X + b.X, a.Y + b.Y);
        }

        /// <summary>
        /// Substraction of two vectors
        /// </summary>
        /// <param name="a">Vector a</param>
        /// <param name="b">Vector b</param>
        /// <returns></returns>
        public static Vector2 operator -(Vector2 a, Vector2 b)
        {
            return new Vector2(a.X - b.X, a.Y - b.Y);
        }

        /// <summary>
        /// Multiplication of a vector and a scalar
        /// </summary>
        /// <param name="a">vector</param>
        /// <param name="scale">multiplier</param>
        /// <returns>new vector</returns>
        public static Vector2 operator *(Vector2 a, float scale)
        {
            return new Vector2(a.X * scale, a.Y * scale);
        }

        /// <summary>
        /// Multiplication of a vector and a scalar
        /// </summary>
        /// <param name="a">vector</param>
        /// <param name="scale">multiplier</param>
        /// <returns>new vector</returns>
        public static Vector2 operator *(Vector2 a, double scale)
        {
            return a * (float)scale;
        }

        /// <summary>
        /// Division of a vector by a scalar
        /// </summary>
        /// <param name="a">vector</param>
        /// <param name="scale">scalar</param>
        /// <returns>new vector</returns>
        public static Vector2 operator /(Vector2 a, float scale)
        {
            return new Vector2(a.X / scale, a.Y / scale);
        }

        /// <summary>
        /// Division of a vector by a scalar
        /// </summary>
        /// <param name="a">vector</param>
        /// <param name="scale">scalar</param>
        /// <returns>new vector</returns>
        public static Vector2 operator /(Vector2 a, double scale)
        {
            return a / (float)scale;
        }

        /// <summary>
        /// Compares two vectors for equality by respecting the epsilon
        /// </summary>
        /// <param name="a">Vector a</param>
        /// <param name="b">Vector b</param>
        /// <returns>true if equal</returns>
        public static bool operator ==(Vector2 a, Vector2 b)
        {
            return (Math.Abs(a.X - b.X) < EpsMin && Math.Abs(a.Y - b.Y) < EpsMin);
        }

        /// <summary>
        /// Compares two vectors for inequality by respecting the epsilon
        /// </summary>
        /// <param name="a">Vector a</param>
        /// <param name="b">Vector b</param>
        /// <returns>true if not equal</returns>
        public static bool operator !=(Vector2 a, Vector2 b)
        {
            return !(a == b);
        }

        #endregion
    }
}