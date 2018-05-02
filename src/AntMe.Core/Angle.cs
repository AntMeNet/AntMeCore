using System;
using System.Globalization;

namespace AntMe
{
    /// <summary>
    /// Represents an Angle.
    /// </summary>
    public struct Angle
    {
        /// <summary>
        /// Minimum value between two values before it's handled as equal
        /// </summary>
        public const float EpsMin = 0.001f;


        #region Static Values

        /// <summary>
        /// Value for Pi.
        /// </summary>
        public static float Pi = (float)Math.PI;

        /// <summary>
        /// Value for Pi over two.
        /// </summary>
        public static float PiHalf = Pi / 2;

        /// <summary>
        /// Value for Pi over four.
        /// </summary>
        public static float PiQuarter = PiHalf / 2;

        /// <summary>
        /// Value for two Pi.
        /// </summary>
        public static float TwoPi = Pi * 2;

        /// <summary>
        /// Angle to Right (East).
        /// </summary>
        public static Angle Right = new Angle(0);

        /// <summary>
        /// Angle to lower Right (Southeast).
        /// </summary>
        public static Angle LowerRight = new Angle(PiQuarter);

        /// <summary>
        /// Angle to Down (South).
        /// </summary>
        public static Angle Down = new Angle(PiHalf);

        /// <summary>
        /// Angle to lower Left (Southwest).
        /// </summary>
        public static Angle LowerLeft = new Angle(PiQuarter * 3);

        /// <summary>
        /// Angle to Left (West).
        /// </summary>
        public static Angle Left = new Angle(Pi);

        /// <summary>
        /// Angle to upper Left (Northwest).
        /// </summary>
        public static Angle UpperLeft = new Angle(PiQuarter * 5);

        /// <summary>
        /// Angle to Up (North).
        /// </summary>
        public static Angle Up = new Angle(PiHalf * 3);

        /// <summary>
        /// Angle to upper Right (Northeast).
        /// </summary>
        public static Angle UpperRight = new Angle(PiQuarter * 7);

        #endregion

        /// <summary>
        /// Creates a new Angle based on the given Radian.
        /// </summary>
        /// <param name="radian">Radian</param>
        public Angle(float radian)
        {
            Radian = NormalizeRadian(radian);
        }

        /// <summary>
        /// Gets or sets the Radian Angle [0;2Pi]
        /// </summary>
        public float Radian { get; }

        /// <summary>
        /// Gets or sets the Angle in Degrees [0;359]
        /// </summary>
        public int Degree => NormalizeDegree(ConvertToDegree(Radian));

        /// <summary>
        /// Gets or sets the Angle as an <see cref="AntMe.Compass"/>
        /// </summary>
        public Compass Compass => ConvertToCompass(Degree);

        /// <summary>
        /// Inverts the X-Component of this Angle.
        /// </summary>
        /// <returns>Inverted Angle</returns>
        public Angle InvertX()
        {
            return Vector2.FromAngle(this).InvertX().ToAngle();
        }

        /// <summary>
        /// Inverts the Y-Component of this Angle.
        /// </summary>
        /// <returns>Inverted Angle</returns>
        public Angle InvertY()
        {
            return Vector2.FromAngle(this).InvertY().ToAngle();
        }

        /// <summary>
        /// Adds the given Radian to this Angle.
        /// </summary>
        /// <param name="radian">Additinal Radian</param>
        /// <returns>New Angle</returns>
        public Angle AddRadian(float radian)
        {
            return new Angle(Radian + radian);
        }

        /// <summary>
        /// Adds the given Degrees to this Angle.
        /// </summary>
        /// <param name="degree">Additional Degrees</param>
        /// <returns>New Angle</returns>
        public Angle AddDegree(int degree)
        {
            return FromDegree(Degree + degree);
        }

        /// <summary>
        /// Substract the given Radian from this Angle.
        /// </summary>
        /// <param name="radian">Radian to substract</param>
        /// <returns>new Angle</returns>
        public Angle SubstractRadian(float radian)
        {
            return new Angle(Radian - radian);
        }

        /// <summary>
        /// Substract the given Degrees from this Angle.
        /// </summary>
        /// <param name="degree">Degrees to substract</param>
        /// <returns>New Angle</returns>
        public Angle SubstractDegree(int degree)
        {
            return FromDegree(Degree - degree);
        }

        /// <summary>
        /// Generates the Hash Code for this Angle.
        /// </summary>
        /// <returns>Hashcode</returns>
        public override int GetHashCode()
        {
            return Radian.GetHashCode();
        }

        /// <summary>
        /// Compares to Angles
        /// </summary>
        /// <param name="obj">Other Angle</param>
        /// <returns>Angles are equal</returns>
        public override bool Equals(object obj)
        {
            Angle other;
            if (obj is Angle angle)
                other = angle;
            else if (obj is float)
                other = new Angle((float)obj);
            else
                return false;

            return this == other;
        }

        /// <summary>
        /// Returns Angle as String as Radian.
        /// </summary>
        /// <returns>Radian</returns>
        public override string ToString()
        {
            return Radian.ToString(CultureInfo.InvariantCulture);
        }

        #region Casting

        /// <summary>
        /// Converts Angle to Radian (float).
        /// </summary>
        /// <param name="angle">Angle</param>
        public static implicit operator float(Angle angle)
        {
            return angle.Radian;
        }

        /// <summary>
        /// Converts Radian (float) to Angle.
        /// </summary>
        /// <param name="angle">Radian Measure</param>
        public static implicit operator Angle(float angle)
        {
            return new Angle(angle);
        }

        #endregion

        #region Operator

        /// <summary>
        /// Adds the given Radian to this Angle.
        /// </summary>
        /// <param name="angle">Angle</param>
        /// <param name="diff">Additinal Radian</param>
        /// <returns>New Angle</returns>
        public static Angle operator +(Angle angle, float diff)
        {
            return angle.AddRadian(diff);
        }

        /// <summary>
        /// Subtracts an Angle (radian measure) from this Angle.
        /// </summary>
        /// <param name="angle">Base Angle</param>
        /// <param name="diff">Subtracting Angle</param>
        /// <returns>Substracted Angle</returns>
        public static Angle operator -(Angle angle, float diff)
        {
            return angle.SubstractRadian(diff);
        }

        /// <summary>
        /// Compares two Angles.
        /// </summary>
        /// <param name="a">Angle a</param>
        /// <param name="b">Angle b</param>
        /// <returns>Angle similar</returns>
        public static bool operator ==(Angle a, Angle b)
        {
            return Math.Abs(a.Radian - b.Radian) < EpsMin;
        }

        /// <summary>
        /// Compares two Angles of disparity.
        /// </summary>
        /// <param name="a">Angle a</param>
        /// <param name="b">Angle b</param>
        /// <returns>Angle dissimilar</returns>
        public static bool operator !=(Angle a, Angle b)
        {
            return !(a == b);
        }

        /// <summary>
        /// Check if an Angle is bigger than another one.
        /// </summary>
        /// <param name="a">Angle a</param>
        /// <param name="b">Angle b</param>
        /// <returns>a bigger than b?</returns>
        public static bool operator >(Angle a, Angle b)
        {
            return Diff(a, b) > 0;
        }

        /// <summary>
        /// Check if an Angle is bigger or similar to another one.
        /// </summary>
        /// <param name="a">Angle a</param>
        /// <param name="b">Angle b</param>
        /// <returns>a bigger than/similar to b?</returns>
        public static bool operator >=(Angle a, Angle b)
        {
            return Diff(a, b) >= 0;
        }

        /// <summary>
        /// Check if an Angle is smaller than another one.
        /// </summary>
        /// <param name="a">Angle a</param>
        /// <param name="b">Angle b</param>
        /// <returns>a smaller than b?</returns>
        public static bool operator <(Angle a, Angle b)
        {
            return Diff(a, b) < 0;
        }

        /// <summary>
        /// Check if an Angle is smaller or similar to another one.
        /// </summary>
        /// <param name="a">Angle a</param>
        /// <param name="b">Angle b</param>
        /// <returns>a smaller than/similar to b?</returns>
        public static bool operator <=(Angle a, Angle b)
        {
            return Diff(a, b) <= 0;
        }

        #endregion

        #region Static Helper

        /// <summary>
        /// Converts Degrees to Radian without any Normalization.
        /// </summary>
        /// <param name="degree">Degree</param>
        /// <returns>Radian</returns>
        public static float ConvertToRadian(int degree)
        {
            return (float)degree / 360 * TwoPi;
        }

        /// <summary>
        /// Converts Radian to Degree without any Normalization.
        /// </summary>
        /// <param name="radian">Radian</param>
        /// <returns>Degree</returns>
        public static int ConvertToDegree(float radian)
        {
            return (int)Math.Round(radian * 360 / TwoPi);
        }

        /// <summary>
        /// Convertes the given Radian to a Compass Value.
        /// </summary>
        /// <param name="radian">Radian</param>
        /// <returns>Compass Direction</returns>
        public static Compass ConvertToCompass(float radian)
        {
            int degree = ConvertToDegree(radian);
            return ConvertToCompass(degree);
        }

        /// <summary>
        /// Converts the given degree to a Compass Value.
        /// </summary>
        /// <param name="degree">Degree</param>
        /// <returns>Compass Direction</returns>
        public static Compass ConvertToCompass(int degree)
        {
            int normalized = NormalizeDegree(degree);
            int index = ((normalized + 22) / 45) % 8;
            return (Compass)(index * 45);
        }

        /// <summary>
        /// Normalize the given Radian to the value range of [0;2Pi).
        /// </summary>
        /// <param name="radian">Radian</param>
        /// <returns>Normaized Radian</returns>
        public static float NormalizeRadian(float radian)
        {
            if (radian < 0)
            {
                int multiplier = (int)(-radian / TwoPi) + 1;
                radian += (TwoPi * multiplier);
            }
            return radian % TwoPi;
        }

        /// <summary>
        /// Normalizes Degrees to the value range of [0;359].
        /// </summary>
        /// <param name="degree">Degrees</param>
        /// <returns>Normalized Degrees</returns>
        public static int NormalizeDegree(int degree)
        {
            if (degree < 0)
            {
                int multiplier = (-degree / 360) + 1;
                degree += (multiplier * 360);
            }
            return degree % 360;
        }

        /// <summary>
        /// Gets an Angle from the given Degree.
        /// </summary>
        /// <param name="degree">degree</param>
        /// <returns>New Angle</returns>
        public static Angle FromDegree(int degree)
        {
            return new Angle(ConvertToRadian(NormalizeDegree(degree)));
        }

        /// <summary>
        ///     Creates an Instance of Angle based on an angle declaration in radian measure.
        /// </summary>
        /// <param name="radian">Angle declaration in radian measure</param>
        /// <returns>New Instance of Angle</returns>
        public static Angle FromRadian(float radian)
        {
            return new Angle(radian);
        }

        /// <summary>
        ///     Creates an Instance of Angle based on a compass declaration.
        /// </summary>
        /// <param name="compass"></param>
        /// <returns></returns>
        public static Angle FromCompass(Compass compass)
        {
            return FromDegree((int)compass);
        }

        /// <summary>
        /// Calculates the difference between to angle declarations in radian measure.
        /// </summary>
        /// <param name="a">Angle a</param>
        /// <param name="b">Angle b</param>
        /// <returns></returns>
        public static float Diff(Angle a, Angle b)
        {
            var diff = b.Radian - a.Radian;
            if (diff > Pi)
                return diff - TwoPi;
            if (diff < -Pi)
                return diff + TwoPi;
            return diff;
        }

        /// <summary>
        ///     Calculates the difference between two angle declarations.
        /// </summary>
        /// <param name="a">Angle a in degrees</param>
        /// <param name="b">Angle b in degrees</param>
        /// <returns>Difference in degrees</returns>
        public static int Diff(int a, int b)
        {
            var alpha = FromDegree(a);
            var beta = FromDegree(b);
            var diff = Diff(alpha, beta);
            return ConvertToDegree(diff);
        }

        #endregion
    }
}