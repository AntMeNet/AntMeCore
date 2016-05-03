using System;
using System.Globalization;

namespace AntMe
{
    /// <summary>
    /// Datentyp zur Speicherung, Prüfung und Umrechnung von Winkelangaben.
    /// </summary>
    public struct Angle
    {
        #region Statische Werte

        /// <summary>
        /// Wert für PI.
        /// </summary>
        public static float Pi = (float) Math.PI;

        /// <summary>
        /// Wert für PI/2.
        /// </summary>
        public static float PiHalf = Pi/2;

        /// <summary>
        /// Wert für PI/4.
        /// </summary>
        public static float PiQuarter = PiHalf/2;

        /// <summary>
        /// Wert für 2PI.
        /// </summary>
        public static float TwoPi = Pi*2;

        /// <summary>
        /// Winkelangabe für Rechts (Osten).
        /// </summary>
        public static Angle Right = new Angle(0);

        /// <summary>
        /// Winkelangabe für Rechts Unten (Südosten).
        /// </summary>
        public static Angle LowerRight = new Angle(PiQuarter);

        /// <summary>
        /// Winkelangabe für Unten (Süden).
        /// </summary>
        public static Angle Down = new Angle(PiHalf);

        /// <summary>
        /// Winkelangabe für Links Unten (Südwesten).
        /// </summary>
        public static Angle LowerLeft = new Angle(PiQuarter*3);

        /// <summary>
        /// Winkelangabe für Links (Westen).
        /// </summary>
        public static Angle Left = new Angle(Pi);

        /// <summary>
        /// Winkelangabe für Oben Links (Nordwesten).
        /// </summary>
        public static Angle UpperLeft = new Angle(PiQuarter*5);

        /// <summary>
        /// Winkelangabe für Oben (Norden).
        /// </summary>
        public static Angle Up = new Angle(PiHalf*3);

        /// <summary>
        /// Winkelangabe für Oben Rechts (Nordosten).
        /// </summary>
        public static Angle UpperRight = new Angle(PiQuarter*7);

        #endregion

        private float value;

        /// <summary>
        ///     Erzeugt einen neuen Winkel mit der initialen Angabe des gegebenen Bogenmaßes [0;2Pi].
        /// </summary>
        /// <param name="value">Winkel im Bogenmaß</param>
        public Angle(float value)
        {
            this.value = 0;
            Radian = value;
        }

        /// <summary>
        /// Wert des Winkels im Bogenmaß [0;2Pi]
        /// </summary>
        public float Radian
        {
            get { return value; }
            set { value = NormalizeRadian(value); }
        }

        /// <summary>
        /// Wert des Winkels in Grad [0;359]
        /// </summary>
        public int Degree
        {
            get { return ConvertToDegree(value); }
            set { Radian = ConvertToRadian(NormalizeDegree(value)); }
        }

        /// <summary>
        /// Liefert den Wert als <see cref="AntMe.Compass" /> (angenähert) oder legt diese fest.
        /// </summary>
        public Compass Compass
        {
            get
            {
                int index = ((Degree + 22)/45)%8;
                return (Compass) (index*45);
            }
            set { Degree = (int) value; }
        }

        /// <summary>
        /// Invertiert die X-Komponente des Winkels
        /// </summary>
        /// <returns>Der neue Angle-Wert</returns>
        public Angle InvertX()
        {
            return Vector2.FromAngle(this).InvertX().ToAngle();
        }

        /// <summary>
        /// Invertiert die Y-Komponente des Winkels
        /// </summary>
        /// <returns>Den neuen Angle-Wert</returns>
        public Angle InvertY()
        {
            return Vector2.FromAngle(this).InvertY().ToAngle();
        }

        /// <summary>
        /// Addiert den angegeben Wert in Bogenmaß zum aktuellen Angle hinzu.
        /// </summary>
        /// <param name="radian">Zu addierenden Wert in Bogenmaß</param>
        /// <returns>Neuer Angle</returns>
        public Angle AddRadian(float radian)
        {
            return new Angle(Radian + radian);
        }

        /// <summary>
        /// Addiert den angegeben Wert im Gradmaß zum aktuellen Angle hinzu.
        /// </summary>
        /// <param name="degree">Zu addierenden Wert im Gradmaß</param>
        /// <returns>Neuer Angle</returns>
        public Angle AddDegree(int degree)
        {
            return new Angle
            {
                Degree = Degree + degree
            };
        }

        /// <summary>
        /// Subtrahiert den angegeben Wert in Bogenmaß zum aktuellen Angle hinzu.
        /// </summary>
        /// <param name="radian">Zu subtrahierenden Wert in Bogenmaß</param>
        /// <returns>Neuer Angle</returns>
        public Angle SubstractRadian(float radian)
        {
            return new Angle(Radian - radian);
        }

        /// <summary>
        /// Subtrahiert den angegeben Wert in Bogenmaß zum aktuellen Angle hinzu.
        /// </summary>
        /// <param name="degree">Zu subtrahierenden Wert im Gradmaß</param>
        /// <returns>Neuer Angle</returns>
        public Angle SubstractDegree(int degree)
        {
            return new Angle
            {
                Degree = Degree + degree
            };
        }

        /// <summary>
        /// Ermittelt den Hashcode dieses Winkels.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return value.GetHashCode();
        }

        /// <summary>
        /// Vergleicht zwei
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            Angle other;
            if (obj is Angle)
                other = (Angle) obj;
            else if (obj is float)
                other = new Angle((float) obj);
            else
                return false;

            return this == other;
        }

        /// <summary>
        /// Gibt den Wert des Winkels (Bogenmaß) als Zeichenkette zurück.
        /// </summary>
        /// <returns>Bogenmaß als Zeichenkette</returns>
        public override string ToString()
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        #region Casting

        /// <summary>
        /// Wandelt einen Angle implizit in einen Float (Bogenmaß) um.
        /// </summary>
        /// <param name="angle">Eingabewinkel</param>
        public static implicit operator float(Angle angle)
        {
            return angle.Radian;
        }

        /// <summary>
        /// Wandelt ein Bogenmaß implizit in einen Angle um.
        /// </summary>
        /// <param name="angle">Bogenmaß</param>
        public static implicit operator Angle(float angle)
        {
            return new Angle(angle);
        }

        #endregion

        #region Operator

        /// <summary>
        /// Addiert einen Winkel (Bogenmaß) auf den Winkel auf.
        /// </summary>
        /// <param name="angle">Basiswinkel</param>
        /// <param name="diff">Zusätzlicher Winkel</param>
        /// <returns>Addierte Winkel</returns>
        public static Angle operator +(Angle angle, float diff)
        {
            return angle.AddRadian(diff);
        }

        /// <summary>
        /// Subtrahiert einen Winkel (Bogenmaß) vom Winkel.
        /// </summary>
        /// <param name="angle">Basiswinkel</param>
        /// <param name="diff">Abzuziehender Winkel</param>
        /// <returns>Subtrahierter Winkel.</returns>
        public static Angle operator -(Angle angle, float diff)
        {
            return angle.SubstractRadian(diff);
        }

        /// <summary>
        /// Vergleicht zwei Winkel.
        /// </summary>
        /// <param name="a">Winkel a</param>
        /// <param name="b">Winkel b</param>
        /// <returns>Winkel gleich</returns>
        public static bool operator ==(Angle a, Angle b)
        {
            return (a.value == b.value);
        }

        /// <summary>
        /// Vergleicht zwei Winkel auf Ungleichheit.
        /// </summary>
        /// <param name="a">Winkel a</param>
        /// <param name="b">Winkel b</param>
        /// <returns>Winkel ungleich</returns>
        public static bool operator !=(Angle a, Angle b)
        {
            return !(a == b);
        }

        /// <summary>
        /// Prüft, ob ein Winkel größer als ein anderer ist.
        /// </summary>
        /// <param name="a">Winkel a</param>
        /// <param name="b">Winkel b</param>
        /// <returns>a größer b?</returns>
        public static bool operator >(Angle a, Angle b)
        {
            return Diff(a, b) > 0;
        }

        /// <summary>
        /// Prüft, ob ein Winkel größer oder gleich einem anderen ist.
        /// </summary>
        /// <param name="a">Winkel a</param>
        /// <param name="b">Winkel b</param>
        /// <returns>a größer/gleich b?</returns>
        public static bool operator >=(Angle a, Angle b)
        {
            return Diff(a, b) >= 0;
        }

        /// <summary>
        /// Prüft, ob ein Winkel kleiner als ein anderer ist.
        /// </summary>
        /// <param name="a">Winkel a</param>
        /// <param name="b">Winkel b</param>
        /// <returns>a kleiner b?</returns>
        public static bool operator <(Angle a, Angle b)
        {
            return Diff(a, b) < 0;
        }

        /// <summary>
        /// Prüft, ob ein Winkel kleiner oder gleich einem anderen ist.
        /// </summary>
        /// <param name="a">Winkel a</param>
        /// <param name="b">Winkel b</param>
        /// <returns>a kleiner/gleich b?</returns>
        public static bool operator <=(Angle a, Angle b)
        {
            return Diff(a, b) <= 0;
        }

        #endregion

        #region Static Helper

        /// <summary>
        ///     Rechnet eine Grad-Angabe in Bogenmaß um.
        /// </summary>
        /// <param name="degree">Winkelangabe in Grad</param>
        /// <returns>Winkelangabe in Bogenmaß</returns>
        public static float ConvertToRadian(int degree)
        {
            return (float) degree/360*TwoPi;
        }

        /// <summary>
        ///     Rechnet ein Bogenmaß in Grad um.
        /// </summary>
        /// <param name="radian">Winkelangabe in Bogenmaß</param>
        /// <returns>Winkelangabe in Grad</returns>
        public static int ConvertToDegree(float radian)
        {
            // Math.Round is imperative since e.g. ConvertToDegree(ConvertToRadian(125)) would yield 124 instead.
            return (int) Math.Round(radian*360/TwoPi);
        }

        /// <summary>
        ///     Normalisiert eine Bogenmaß Angabe auf den Wertebereich [0;2Pi].
        /// </summary>
        /// <param name="radian">Unnormalisiertes Bogenmaß</param>
        /// <returns>Normalisiertes Bogenmaß</returns>
        public static float NormalizeRadian(float radian)
        {
            if (radian < 0)
            {
                int multiplier = (int) (-radian/TwoPi) + 1;
                radian += (TwoPi*multiplier);
            }
            return radian%TwoPi;
        }

        /// <summary>
        ///     Normalisiert eine Grad Angabe auf den Wertebereich [0;359].
        /// </summary>
        /// <param name="degree">Unnormalisierte Grad Angabe</param>
        /// <returns>Normalisierte Grad Angabe</returns>
        public static int NormalizeDegree(int degree)
        {
            if (degree < 0)
            {
                int multiplier = (-degree/360) + 1;
                degree += (multiplier*360);
            }
            return degree%360;
        }

        /// <summary>
        ///     Erzeugt eine Instanz von Angle auf Basis einer Winkelangabe in Grad.
        /// </summary>
        /// <param name="degree">Winkelangabe in Grad</param>
        /// <returns>Neue Instanz von Angle</returns>
        public static Angle FromDegree(int degree)
        {
            return new Angle {Degree = degree};
        }

        /// <summary>
        ///     Erzeugt eine Instanz von Angle auf Basis einer Winkelangabe im Bogenmaß.
        /// </summary>
        /// <param name="radian">Winkelangabe im Bogenmaß</param>
        /// <returns>Neue Instanz von Angle</returns>
        public static Angle FromRadian(float radian)
        {
            return new Angle(radian);
        }

        /// <summary>
        ///     Erzeugt eine Instanz von Angle auf Basis einer Kompass-Angabe.
        /// </summary>
        /// <param name="compass"></param>
        /// <returns></returns>
        public static Angle FromCompass(Compass compass)
        {
            return FromDegree((int) compass);
        }

        /// <summary>
        ///     Ermittelt die Differenz zwischen zwei Winkelangaben.
        /// </summary>
        /// <param name="a">Winkel a</param>
        /// <param name="b">Winkel b</param>
        /// <returns></returns>
        public static float Diff(Angle a, Angle b)
        {
            float diff = b.Radian - a.Radian;
            if (diff > Pi)
                return diff - TwoPi;
            if (diff < -Pi)
                return diff + TwoPi;
            return diff;
        }

        /// <summary>
        ///     Ermittelt die Differenz zwischen zwei Winkelangaben.
        /// </summary>
        /// <param name="a">Winkel a in Grad</param>
        /// <param name="b">Winkel b in Grad</param>
        /// <returns>Differenz in Grad</returns>
        public static int Diff(int a, int b)
        {
            Angle alpha = FromDegree(a);
            Angle beta = FromDegree(b);
            float diff = Diff(alpha, beta);
            return ConvertToDegree(diff);
        }

        #endregion
    }
}