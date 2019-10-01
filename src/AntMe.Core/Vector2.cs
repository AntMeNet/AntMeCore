using System;

namespace AntMe
{
    /// <summary>
    ///     Datentyp zur Speicherung von 2D-Koordinaten mit X- und Y-Komponente.
    /// </summary>
    [Serializable]
    public struct Vector2
    {
        private const float SIN45 = 0.70710678118654752440084436210485f;

        /// <summary>
        ///     Minimalste Abweichung bei Vergleichsprüfungen. Soll Rundungsfehlern entgegen wirken.
        /// </summary>
        public const float EPS_MIN = 0.001f;

        /// <summary>
        ///     X-Anteil der Koordinate.
        /// </summary>
        public float X;

        /// <summary>
        ///     Y-Anteil der Koordinate.
        /// </summary>
        public float Y;

        /// <summary>
        ///     Parameterisierter Konstruktor der Vector2-Klasse.
        /// </summary>
        /// <param name="x">X-Anteil</param>
        /// <param name="y">Y-Anteil</param>
        public Vector2(float x, float y)
        {
            X = x;
            Y = y;
        }

        #region Konstanten

        /// <summary>
        ///     Null Vektor.
        /// </summary>
        public static Vector2 Zero => new Vector2(0, 0);

        /// <summary>
        ///     Vektor nach links mit der Länge 1.
        /// </summary>
        public static Vector2 Left => new Vector2(-1, 0);

        /// <summary>
        ///     Vector nach links oben mit der Länge 1.
        /// </summary>
        public static Vector2 UpperLeft => new Vector2(-SIN45, -SIN45);

        /// <summary>
        ///     Vector nach links unten mit der Länge 1.
        /// </summary>
        public static Vector2 LowerLeft => new Vector2(-SIN45, SIN45);

        /// <summary>
        ///     Vektor nach rechts mit der Länge 1.
        /// </summary>
        public static Vector2 Right => new Vector2(1, 0);

        /// <summary>
        ///     Vector nach rechts oben mit der Länge 1.
        /// </summary>
        public static Vector2 UpperRight => new Vector2(SIN45, -SIN45);

        /// <summary>
        ///     Vector nach rechts unten mit der Länge 1.
        /// </summary>
        public static Vector2 LowerRight => new Vector2(SIN45, SIN45);

        /// <summary>
        ///     Vektor nach oben mit der Länge 1.
        /// </summary>
        public static Vector2 Up => new Vector2(0, -1);

        /// <summary>
        ///     Vektor nach unten mit der Länge 1.
        /// </summary>
        public static Vector2 Down => new Vector2(0, 1);

        #endregion

        #region Static Helper

        /// <summary>
        ///     Erstellt einen Vector2 mit Einheitsmaß aus einem gegebenen Winkel.
        /// </summary>
        /// <param name="angle">Winkel als <see cref="AntMe.Core.Angle" /></param>
        /// <returns>Neuer Einheitsvektor</returns>
        public static Vector2 FromAngle(Angle angle)
        {
            return FromAngle(angle.Radian);
        }

        /// <summary>
        ///     Erstellt einen Vector2 mit Einheitsmaß aus einem gegebenen Winkel.
        /// </summary>
        /// <param name="radian">Winkel als Bogenmaß</param>
        /// <returns>Neuer Einheitsvektor</returns>
        public static Vector2 FromAngle(float radian)
        {
            return new Vector2(
                (float) Math.Cos(radian),
                (float) Math.Sin(radian));
        }

        #endregion

        /// <summary>
        ///     Invertiert die X-Achse des Vektors.
        /// </summary>
        /// <returns>Neuer Vektor</returns>
        public Vector2 InvertX()
        {
            return new Vector2(X * -1, Y);
        }

        /// <summary>
        ///     Invertiert die Y-Achse des Vektors.
        /// </summary>
        /// <returns>Neuer Vektor</returns>
        public Vector2 InvertY()
        {
            return new Vector2(X, Y * -1);
        }

        /// <summary>
        ///     Invertiert die X- und Y-Achse des Vektors.
        /// </summary>
        /// <returns>Neuer Vektor</returns>
        public Vector2 InvertXY()
        {
            return new Vector2(X * -1, Y * -1);
        }

        /// <summary>
        ///     Berechnet das Quadrat der Länge des Vektors.
        /// </summary>
        /// <returns>Quadrat der Länge des Vektors</returns>
        public float LengthSquared()
        {
            return X * X + Y * Y;
        }

        /// <summary>
        ///     Berechnet die Länge des Vektors.
        /// </summary>
        /// <returns>Länge des Vektors</returns>
        public float Length()
        {
            return (float) Math.Sqrt(LengthSquared());
        }

        /// <summary>
        ///     Ermittelt die Richtung des Vektors.
        /// </summary>
        /// <returns>Winkel des Vektors als <see cref="Angle" /></returns>
        public Angle ToAngle()
        {
            return new Angle((float) Math.Atan2(Y, X));
        }

        /// <summary>
        ///     Normalisiert den Vektor auf die Länge 1.
        /// </summary>
        /// <returns>Normalisierter Vektor</returns>
        public Vector2 Normalize()
        {
            // Speziallfall Null-Vektor
            if (this == Zero)
                return Zero;

            var scale = 1 / Length();
            return this * scale;
        }

        /// <summary>
        ///     Vergleicht, ob zwei Vektoren identische Werte haben.
        /// </summary>
        /// <param name="obj">Vergleichsvektor</param>
        /// <returns>true, falls beide gleich sind</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is Vector2))
                return false;

            var b = (Vector2) obj;
            return X == b.X && Y == b.Y;
        }

        /// <summary>
        ///     Ermittelt einen Hashcode für den aktuellen Vektor.
        /// </summary>
        /// <returns>Hashcode</returns>
        public override int GetHashCode()
        {
            return X.GetHashCode() + Y.GetHashCode();
        }

        /// <summary>
        ///     Gibt den Vektor in lesbarem Format "x/y" zurück.
        /// </summary>
        /// <returns>Vektor als <see cref="string" /></returns>
        public override string ToString()
        {
            return string.Format("{0:0.0000}/{1:0.0000}", X, Y);
        }

        #region Operator

        public static Vector2 operator +(Vector2 a, Vector2 b)
        {
            return new Vector2(a.X + b.X, a.Y + b.Y);
        }

        public static Vector2 operator -(Vector2 a, Vector2 b)
        {
            return new Vector2(a.X - b.X, a.Y - b.Y);
        }

        public static Vector2 operator *(Vector2 a, float scale)
        {
            return new Vector2(a.X * scale, a.Y * scale);
        }

        public static Vector2 operator *(Vector2 a, double scale)
        {
            return a * (float) scale;
        }

        public static Vector2 operator /(Vector2 a, float scale)
        {
            return new Vector2(a.X / scale, a.Y / scale);
        }

        public static Vector2 operator /(Vector2 a, double scale)
        {
            return a / (float) scale;
        }

        public static bool operator ==(Vector2 a, Vector2 b)
        {
            return a.X == b.X && a.Y == b.Y;
        }

        public static bool operator !=(Vector2 a, Vector2 b)
        {
            return !(a == b);
        }

        #endregion
    }
}