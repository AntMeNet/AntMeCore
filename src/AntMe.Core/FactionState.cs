using System.ComponentModel;
using System.IO;

namespace AntMe
{
    /// <summary>
    ///     State Klasse zur Übertragung von Fraktionsstatus.
    /// </summary>
    public class FactionState : PropertyList<FactionStateProperty>, ISerializableState
    {
        /// <summary>
        ///     Gibt den Name der Fraktion zurück oder legt diesen fest.
        /// </summary>
        [DisplayName("Name")]
        [Description("Name of the Player")]
        [ReadOnly(true)]
        [Category("Static")]
        public string Name { get; set; }

        /// <summary>
        ///     Gibt den Spieler Index zurück oder legt diesen fest.
        /// </summary>
        [DisplayName("Slot")]
        [Description("Slot Index")]
        [ReadOnly(true)]
        [Category("Static")]
        public byte SlotIndex { get; set; }

        /// <summary>
        /// Gibt den Namen der zugrunde liegenden Faction zurück oder legt diesen fest.
        /// </summary>
        [DisplayName("Faction Name")]
        [Description("Name of the Faction")]
        [ReadOnly(true)]
        [Category("Static")]
        public string FactionName { get; set; }

        /// <summary>
        /// Gibt die Farbe des Spielers an oder legt diese fest.
        /// </summary>
        [DisplayName("Color")]
        [Description("Player Color")]
        [ReadOnly(true)]
        [Category("Static")]
        public PlayerColor PlayerColor { get; set; }

        /// <summary>
        /// Gibt die Startposition des Spielers zurück oder legt diesen fest.
        /// </summary>
        [DisplayName("Start Point")]
        [Description("Start Point")]
        [ReadOnly(true)]
        [Category("Static")]
        public Vector2 StartPoint { get; set; }

        /// <summary>
        ///     Gibt den aktuellen Punktestand dieser Fraktion zurück oder legt
        ///     diese fest.
        /// </summary>
        [DisplayName("Points")]
        [Description("Total Points")]
        [ReadOnly(true)]
        [Category("Dynamic")]
        public int Points { get; set; }

        /// <summary>
        /// Methode, die beim ersten Serialisieren dieser Instanz aufgerufen wird. 
        /// Dieser Call kann auch lange nach der Erstellung passieren, wenn 
        /// beispielsweise neue Zuschauer die States beobachten wollen.
        /// </summary>
        /// <param name="stream">Output Stream</param>
        /// <param name="version">Protocol Version</param>
        public virtual void SerializeFirst(BinaryWriter stream, byte version)
        {
            stream.Write(Name ?? string.Empty);
            stream.Write(FactionName ?? string.Empty);
            stream.Write((byte)PlayerColor);
            stream.Write(StartPoint.X);
            stream.Write(StartPoint.Y);
            stream.Write(Points);
        }

        /// <summary>
        /// Methode, die beim erneuten Serialisieren dieser Instanz aufgerufen 
        /// wird. Hier müssen nur noch veränderbare Daten geschickt werden. Dieser 
        /// Call muss nicht zwangsläufig in jedem Frame stattfinden, wird aber 
        /// ganz sicher immer vom selben Client abgerufen - es können also diffs 
        /// gesendet werden.
        /// </summary>
        /// <param name="stream">Output Stream</param>
        /// <param name="version">Protocol Version</param>
        public virtual void SerializeUpdate(BinaryWriter stream, byte version)
        {
            stream.Write(Points);
        }

        /// <summary>
        /// Methode, die beim ersten Erstellen aus einem Stream heraus aufgerufen 
        /// wird. dieser Call sollte alle Grundinformationen des States herstellen 
        /// und muss nicht dem State des ersten Frames entsprechen.
        /// </summary>
        /// <param name="stream">Input Stream</param>
        /// <param name="version">Protocol Version</param>
        public virtual void DeserializeFirst(BinaryReader stream, byte version)
        {
            Name = stream.ReadString();
            FactionName = stream.ReadString();
            PlayerColor = (PlayerColor)stream.ReadByte();
            StartPoint = new Vector2(
                stream.ReadSingle(),
                stream.ReadSingle());
            Points = stream.ReadInt32();
        }

        /// <summary>
        /// Methode, die bei einem Folgeframe aufgerufen wird, nachdem der 
        /// State bereits initialisiert wurde.
        /// </summary>
        /// <param name="stream">Input Stream</param>
        /// <param name="version">Protocol Version</param>
        public virtual void DeserializeUpdate(BinaryReader stream, byte version)
        {
            Points = stream.ReadInt32();
        }

        public override string ToString()
        {
            return string.Format("{0} ({1})", Name, FactionName);
        }
    }

    /// <summary>
    /// Liste der möglichen Spielerfarben.
    /// </summary>
    public enum PlayerColor
    {
        /// <summary>
        /// Schwarzer Spieler.
        /// </summary>
        Black = 0,

        /// <summary>
        /// Roter Spieler.
        /// </summary>
        Red = 1,

        /// <summary>
        /// Blauer Spieler.
        /// </summary>
        Blue = 2,

        /// <summary>
        /// Hellblauer Spieler.
        /// </summary>
        Cyan = 3,

        /// <summary>
        /// Lila Spieler.
        /// </summary>
        Purple = 4,

        /// <summary>
        /// Orangener Spieler.
        /// </summary>
        Orange = 5,

        /// <summary>
        /// Grüner Spieler.
        /// </summary>
        Green = 6,

        /// <summary>
        /// Weisser Spieler.
        /// </summary>
        White = 7,

        /// <summary>
        /// Undefinierte Spielerfarbe.
        /// </summary>
        Undefined = 8
    }
}