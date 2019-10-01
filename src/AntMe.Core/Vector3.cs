using System;

namespace AntMe
{
    /// <summary>
    ///     Datentyp zur Speicherung von 3D-Koordinaten mit X-, Y- und Z-Komponente.
    /// </summary>
    [Serializable]
    public struct Vector3
    {
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
        ///     Z-Anteil der Koordinate.
        /// </summary>
        public float Z;

        /// <summary>
        ///     Parameterisierter Konstruktor der Vector3-Klasse.
        /// </summary>
        /// <param name="x">X-Anteil</param>
        /// <param name="y">Y-Anteil</param>
        /// <param name="z">Z-Anteil</param>
        public Vector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        #region Konstanten

        /// <summary>
        ///     Null Vektor.
        /// </summary>
        public static Vector3 Zero => new Vector3(0, 0, 0);

        #endregion

        #region Static Helper

        /// <summary>
        ///     Erstellt einen Vector3 mit Einheitsmaß aus einem gegebenen Winkel.
        /// </summary>
        /// <param name="angle">Winkel als Angle</param>
        /// <returns>Neuer Einheitsvektor auf XY-Achse</returns>
        public static Vector3 FromAngleXY(Angle angle)
        {
            return FromAngleXY(angle.Radian);
        }

        /// <summary>
        ///     Erstellt einen Vector3 mit Einheitsmaß aus einem gegebenen Winkel.
        /// </summary>
        /// <param name="radian">Winkel als Bogenmaß</param>
        /// <returns>Neuer Einheitsvektor auf XY-Achse</returns>
        public static Vector3 FromAngleXY(float radian)
        {
            return new Vector3(
                (float) Math.Cos(radian),
                (float) Math.Sin(radian), 0f);
        }

        #endregion

        /// <summary>
        ///     Invertiert die X-Achse des Vektors.
        /// </summary>
        /// <returns>Neuer Vektor</returns>
        public Vector3 InvertX()
        {
            return new Vector3(X * -1, Y, Z);
        }

        /// <summary>
        ///     Invertiert die Y-Achse des Vektors.
        /// </summary>
        /// <returns>Neuer Vektor</returns>
        public Vector3 InvertY()
        {
            return new Vector3(X, Y * -1, Z);
        }

        /// <summary>
        ///     Invertiert die Z-Achse des Vektors.
        /// </summary>
        /// <returns>Neuer Vektor</returns>
        public Vector3 InvertZ()
        {
            return new Vector3(X, Y, Z * -1);
        }

        /// <summary>
        ///     Invertiert die X- und Y-Achse des Vektors.
        /// </summary>
        /// <returns>Neuer Vektor</returns>
        public Vector3 InvertXY()
        {
            return new Vector3(X * -1, Y * -1, Z);
        }

        /// <summary>
        ///     Invertiert die X- und Z-Achse des Vektors.
        /// </summary>
        /// <returns>Neuer Vektor</returns>
        public Vector3 InvertXZ()
        {
            return new Vector3(X * -1, Y, Z * -1);
        }

        /// <summary>
        ///     Invertiert die Y- und Z-Achse des Vektors.
        /// </summary>
        /// <returns>Neuer Vektor</returns>
        public Vector3 InvertYZ()
        {
            return new Vector3(X, Y * -1, Z * -1);
        }

        /// <summary>
        ///     Invertiert die X-, Y- und Z-Achse des Vektors.
        /// </summary>
        /// <returns>Neuer Vektor</returns>
        public Vector3 InvertXYZ()
        {
            return new Vector3(X * -1, Y * -1, Z * -1);
        }

        /// <summary>
        ///     Berechnet das Quadrat der Länge des Vektors.
        /// </summary>
        /// <returns>Quadrat der Länge des Vektors</returns>
        public float LengthSquared()
        {
            return X * X + Y * Y + Z * Z;
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
        ///     Ermittelt die Richtung des Vektors zwischen X und Y.
        /// </summary>
        /// <returns></returns>
        public Angle ToAngleXY()
        {
            return new Angle((float) Math.Atan2(Y, X));
        }

        /// <summary>
        ///     Ermittelt einen Vector2 aus der X- und Y-Koordinate.
        /// </summary>
        /// <returns></returns>
        public Vector2 ToVector2XY()
        {
            return new Vector2(X, Y);
        }

        /// <summary>
        ///     Ermittelt einen Vector2 aus der Y- und Z-Koordinate.
        /// </summary>
        /// <returns></returns>
        public Vector2 ToVector2YZ()
        {
            return new Vector2(Y, Z);
        }

        /// <summary>
        ///     Ermittelt einen Vector2 aus der X- und Z-Koordinate.
        /// </summary>
        /// <returns></returns>
        public Vector2 ToVector2XZ()
        {
            return new Vector2(X, Z);
        }

        /// <summary>
        ///     Ermittelt die Richtung des Vektors zwischen X und Z.
        /// </summary>
        /// <returns></returns>
        public Angle ToAngleXZ()
        {
            return new Angle((float) Math.Atan2(Z, X));
        }

        /// <summary>
        ///     Ermittelt die Richtung des Vektors zwischen Y und Z.
        /// </summary>
        /// <returns></returns>
        public Angle ToAngleYZ()
        {
            return new Angle((float) Math.Atan2(Z, Y));
        }

        /// <summary>
        ///     Normalisiert den Vektor auf die Länge 1.
        /// </summary>
        /// <returns></returns>
        public Vector3 Normalize()
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
            if (!(obj is Vector3))
                return false;

            var b = (Vector3) obj;
            return X == b.X && Y == b.Y && Z == b.Z;
        }

        /// <summary>
        ///     Ermittelt einen Hashcode für den aktuellen Vektor.
        /// </summary>
        /// <returns>Hashcode</returns>
        public override int GetHashCode()
        {
            return X.GetHashCode() + Y.GetHashCode() + Z.GetHashCode();
        }

        /// <summary>
        ///     Gibt den Vektor in lesbarem Format "x/y/z" zurück.
        /// </summary>
        /// <returns>Vektor als <see cref="string" /></returns>
        public override string ToString()
        {
            return string.Format("{0:0.0000}/{1:0.0000}/{2:0.0000}", X, Y, Z);
        }

        #region Operatoren

        /// <summary>
        ///     Addition zweier Vector3.
        /// </summary>
        /// <param name="a">Vektor 1</param>
        /// <param name="b">Vektor 2</param>
        /// <returns>Addierte Vektoren</returns>
        public static Vector3 operator +(Vector3 a, Vector3 b)
        {
            return new Vector3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        /// <summary>
        ///     Subjtraktion zweier Vektor3.
        /// </summary>
        /// <param name="a">Vektor 1</param>
        /// <param name="b">Vektor 2</param>
        /// <returns>Subtrahierter Vektor</returns>
        public static Vector3 operator -(Vector3 a, Vector3 b)
        {
            return new Vector3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        /// <summary>
        ///     Skalierung eines Vektor3.
        /// </summary>
        /// <param name="a">Vektor</param>
        /// <param name="scale">Skalierungsfaktor</param>
        /// <returns>Skalierter Vektor.</returns>
        public static Vector3 operator *(Vector3 a, float scale)
        {
            return new Vector3(a.X * scale, a.Y * scale, a.Z * scale);
        }

        /// <summary>
        ///     Skalierung eines Vektor3.
        /// </summary>
        /// <param name="a">Vektor</param>
        /// <param name="scale">Skalierungsfaktor</param>
        /// <returns>Skalierter Vektor.</returns>
        public static Vector3 operator *(Vector3 a, double scale)
        {
            return a * (float) scale;
        }


        public static Vector3 operator /(Vector3 a, float scale)
        {
            return new Vector3(a.X / scale, a.Y / scale, a.Z / scale);
        }

        public static Vector3 operator /(Vector3 a, double scale)
        {
            return a / (float) scale;
        }

        /// <summary>
        ///     Wertevergleich zweier Vektoren.
        /// </summary>
        /// <param name="a">Vektor 1</param>
        /// <param name="b">Vektor 2</param>
        /// <returns>Sind die Vektoren identisch?</returns>
        public static bool operator ==(Vector3 a, Vector3 b)
        {
            // TODO: evtl. Epsilon berücksichtigen.
            return a.X == b.X && a.Y == b.Y && a.Z == b.Z;
        }

        /// <summary>
        ///     Wertevergleich zweier Vektoren.
        /// </summary>
        /// <param name="a">Vektor 1</param>
        /// <param name="b">Vektor 2</param>
        /// <returns>Sind die Vektoren nicht identisch?</returns>
        public static bool operator !=(Vector3 a, Vector3 b)
        {
            return !(a == b);
        }

        #endregion
    }
}