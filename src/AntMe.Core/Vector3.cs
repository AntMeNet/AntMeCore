using System;

namespace AntMe
{
    /// <summary>
    /// Represents a 3D Vector
    /// </summary>
    [Serializable]
    public struct Vector3
    {
        /// <summary>
        /// Minimum value between two values before it's handled as equal
        /// </summary>
        public const float EpsMin = 0.001f;

        /// <summary>
        /// X-Axis
        /// </summary>
        public float X { get; }

        /// <summary>
        /// Y-Axis
        /// </summary>
        public float Y { get; }

        /// <summary>
        /// Z-Axis
        /// </summary>
        public float Z { get; }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="x">X-Axis</param>
        /// <param name="y">Y-Axis</param>
        /// <param name="z">Z-Axis</param>
        public Vector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        #region Constrants

        /// <summary>
        /// Null Vektor.
        /// </summary>
        public static Vector3 Zero => new Vector3(0, 0, 0);

        #endregion

        #region Static factories

        /// <summary>
        /// Generates a normalized vector based on the given angle
        /// </summary>
        /// <param name="angle">Angle</param>
        /// <returns>Normalized vector</returns>
        public static Vector3 FromAngleXy(Angle angle)
        {
            return FromAngleXy(angle.Radian);
        }

        /// <summary>
        /// Generates a normalized vector based on the given angle
        /// </summary>
        /// <param name="radian">Angle</param>
        /// <returns>Normalized vector</returns>
        public static Vector3 FromAngleXy(float radian)
        {
            return new Vector3(
                (float)Math.Cos(radian),
                (float)Math.Sin(radian), 0f);
        }

        #endregion

        /// <summary>
        /// Inverts vector on x-axis
        /// </summary>
        /// <returns>inverted vector</returns>
        public Vector3 InvertX()
        {
            return new Vector3(X * -1, Y, Z);
        }

        /// <summary>
        /// Inverts vector on y-axis
        /// </summary>
        /// <returns>inverted vector</returns>
        public Vector3 InvertY()
        {
            return new Vector3(X, Y * -1, Z);
        }

        /// <summary>
        /// Inverts vector on z-axis
        /// </summary>
        /// <returns>inverted vector</returns>
        public Vector3 InvertZ()
        {
            return new Vector3(X, Y, Z * -1);
        }

        /// <summary>
        /// Inverts vector on x- and y-axis
        /// </summary>
        /// <returns>inverted vector</returns>
        public Vector3 InvertXy()
        {
            return new Vector3(X * -1, Y * -1, Z);
        }

        /// <summary>
        /// Inverts vector on x- and z-axis
        /// </summary>
        /// <returns>inverted vector</returns>
        public Vector3 InvertXz()
        {
            return new Vector3(X * -1, Y, Z * -1);
        }

        /// <summary>
        /// Inverts vector on y- and z-axis
        /// </summary>
        /// <returns>inverted vector</returns>
        public Vector3 InvertYz()
        {
            return new Vector3(X, Y * -1, Z * -1);
        }

        /// <summary>
        /// Inverts vector on x-, y- and z-axis
        /// </summary>
        /// <returns>inverted vector</returns>
        public Vector3 InvertXyz()
        {
            return new Vector3(X * -1, Y * -1, Z * -1);
        }

        /// <summary>
        /// Calculates the square length of the vector
        /// </summary>
        /// <returns>Square length</returns>
        public float LengthSquared()
        {
            return X * X + Y * Y + Z * Z;
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
        /// Returns the angle between x- and y-axis
        /// </summary>
        /// <returns>angle</returns>
        public Angle ToAngleXy()
        {
            return new Angle((float)Math.Atan2(Y, X));
        }

        /// <summary>
        /// Returns the x- and y-axis as a 2D vector
        /// </summary>
        /// <returns>vector</returns>
        public Vector2 ToVector2Xy()
        {
            return new Vector2(X, Y);
        }

        /// <summary>
        /// Returns the y- and z-axis as a 2D vector
        /// </summary>
        /// <returns>vector</returns>
        public Vector2 ToVector2Yz()
        {
            return new Vector2(Y, Z);
        }

        /// <summary>
        /// Returns the x- and z-axis as a 2D vector
        /// </summary>
        /// <returns>vector</returns>
        public Vector2 ToVector2Xz()
        {
            return new Vector2(X, Z);
        }

        /// <summary>
        /// Returns the angle between x- and z-axis
        /// </summary>
        /// <returns>angle</returns>
        public Angle ToAngleXz()
        {
            return new Angle((float)Math.Atan2(Z, X));
        }

        /// <summary>
        /// Returns the angle between y- and z-axis
        /// </summary>
        /// <returns>angle</returns>
        public Angle ToAngleYz()
        {
            return new Angle((float)Math.Atan2(Z, Y));
        }

        /// <summary>
        /// Normalizes the vector
        /// </summary>
        /// <returns></returns>
        public Vector3 Normalize()
        {
            // Special case Null vector (returns again null)
            if (this == Zero)
                return Zero;

            float scale = 1 / Length();
            return this * scale;
        }

        /// <summary>
        /// Compares for equality.
        /// </summary>
        /// <param name="other">other vector</param>
        /// <returns>true in case of equalitry</returns>
        public override bool Equals(object other)
        {
            if (!(other is Vector3))
                return false;

            var b = (Vector3)other;
            return Math.Abs(X - b.X) < EpsMin 
                    && Math.Abs(Y - b.Y) < EpsMin 
                    && Math.Abs(Z - b.Z) < EpsMin;
        }

        /// <summary>
        /// Generates hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            return X.GetHashCode() + Y.GetHashCode() + Z.GetHashCode();
        }

        /// <summary>
        /// returns the value of this vector as string in the X/Y/Z format.
        /// </summary>
        /// <returns>Value as string</returns>
        public override string ToString()
        {
            return $"{X:0.000}/{Y:0.000}/{Z:0.000}";
        }

        #region Operators

        /// <summary>
        /// Adds two vectors.
        /// </summary>
        /// <param name="a">Vector a</param>
        /// <param name="b">Vector b</param>
        /// <returns>new vector</returns>
        public static Vector3 operator +(Vector3 a, Vector3 b)
        {
            return new Vector3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        /// <summary>
        /// Substracts vector b from vector a.
        /// </summary>
        /// <param name="a">Vector a</param>
        /// <param name="b">Vector b</param>
        /// <returns>new vector</returns>
        public static Vector3 operator -(Vector3 a, Vector3 b)
        {
            return new Vector3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        /// <summary>
        /// Scales the vector.
        /// </summary>
        /// <param name="a">Vector</param>
        /// <param name="scale">Scalar</param>
        /// <returns>new vector</returns>
        public static Vector3 operator *(Vector3 a, float scale)
        {
            return new Vector3(a.X * scale, a.Y * scale, a.Z * scale);
        }

        /// <summary>
        /// Scales the vector.
        /// </summary>
        /// <param name="a">Vector</param>
        /// <param name="scale">Scalar</param>
        /// <returns>new vector</returns>
        public static Vector3 operator *(Vector3 a, double scale)
        {
            return a * (float)scale;
        }


        /// <summary>
        /// Divides a vector by a scalar
        /// </summary>
        /// <param name="a">Vector</param>
        /// <param name="scale">Scalar</param>
        /// <returns>new vector</returns>
        public static Vector3 operator /(Vector3 a, float scale)
        {
            return new Vector3(a.X / scale, a.Y / scale, a.Z / scale);
        }

        /// <summary>
        /// Divides a vector by a scalar
        /// </summary>
        /// <param name="a">Vector</param>
        /// <param name="scale">Scalar</param>
        /// <returns>new vector</returns>
        public static Vector3 operator /(Vector3 a, double scale)
        {
            return a / (float)scale;
        }

        /// <summary>
        /// Checks two vectors for equality
        /// </summary>
        /// <param name="a">Vector a</param>
        /// <param name="b">Vector b</param>
        /// <returns>true if equal</returns>
        public static bool operator ==(Vector3 a, Vector3 b)
        {
            return Math.Abs(a.X - b.X) < EpsMin 
                    && Math.Abs(a.Y - b.Y) < EpsMin 
                    && Math.Abs(a.Z - b.Z) < EpsMin;
        }

        /// <summary>
        /// Checks two vectors for inequality
        /// </summary>
        /// <param name="a">Vector a</param>
        /// <param name="b">Vector b</param>
        /// <returns>true if not equal</returns>

        public static bool operator !=(Vector3 a, Vector3 b)
        {
            return !(a == b);
        }

        #endregion
    }
}