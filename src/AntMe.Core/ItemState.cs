using System;
using System.ComponentModel;
using System.IO;

namespace AntMe
{
    /// <summary>
    ///     Basisklasse für die Übertragung eines beliebigen Item States. Für die
    ///     Übertragung von fraktionsbezogenen Items bitte FactionItemState verwenden.
    /// </summary>
    public class ItemState : PropertyList<StateProperty>, ISerializableState
    {
        /// <summary>
        ///     Gibt die Id des Spielelements an oder legt diese fest.
        /// </summary>
        [DisplayName("ID")]
        [Description("")]
        [ReadOnly(true)]
        [Category("Static")]
        public int Id { get; set; }

        /// <summary>
        ///     Gibt die absolute Position des Spielelements an oder legt diese fest.
        /// </summary>
        [DisplayName("Position")]
        [Description("")]
        [ReadOnly(true)]
        [Category("Dynamic")]
        public Vector3 Position { get; set; }

        /// <summary>
        /// Gibt den Radius eines Spielelements an oder liegt diesen fest.
        /// </summary>
        [DisplayName("Radius")]
        [Description("")]
        [ReadOnly(true)]
        [Category("Dynamic")]
        public float Radius { get; set; }

        /// <summary>
        /// Gibt die Blickrichtung des Spielelementes an.
        /// </summary>
        [DisplayName("Orientation")]
        [Description("")]
        [ReadOnly(true)]
        [Category("Dynamic")]
        public short Orientation { get; set; }

        /// <summary>
        /// Itemlose Instanz (Client)
        /// </summary>
        public ItemState()
        {

        }

        /// <summary>
        /// Itembezogene Instanz (Server)
        /// </summary>
        /// <param name="item"></param>
        public ItemState(Item item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            Id = item.Id;
            Position = item.Position;
            Orientation = (short)item.Orientation.Degree;
            Radius = item.Radius;

            item.PositionChanged += (i, v) => { Position = v; };
            item.OrientationChanged += (i, v) => { Orientation = (short)v.Degree; };
            item.RadiusChanged += (i, v) => { Radius = v; };
        }

        /// <summary>
        /// Methode, die beim ersten Serialisieren dieser Instanz aufgerufen wird. 
        /// Dieser Call kann auch lange nach der Erstellung passieren, wenn 
        /// beispielsweise neue Zuschauer die States beobachten wollen.
        /// </summary>
        /// <param name="stream">Output Stream</param>
        /// <param name="version">Protocol Version</param>
        public virtual void SerializeFirst(BinaryWriter stream, byte version)
        {
            stream.Write(Position.X);
            stream.Write(Position.Y);
            stream.Write(Position.Z);
            stream.Write(Radius);
            stream.Write(Orientation);

            // Properties
            foreach (var property in Properties)
                property.SerializeFirst(stream, version);
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
            stream.Write(Position.X);
            stream.Write(Position.Y);
            stream.Write(Position.Z);
            stream.Write(Radius);
            stream.Write(Orientation);

            // Properties
            foreach (var property in Properties)
                property.SerializeUpdate(stream, version);
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
            Position = new Vector3(
                stream.ReadSingle(),
                stream.ReadSingle(),
                stream.ReadSingle());
            Radius = stream.ReadSingle();
            Orientation = stream.ReadInt16();

            // Properties
            foreach (var property in Properties)
                property.DeserializeFirst(stream, version);
        }

        /// <summary>
        /// Methode, die bei einem Folgeframe aufgerufen wird, nachdem der 
        /// State bereits initialisiert wurde.
        /// </summary>
        /// <param name="stream">Input Stream</param>
        /// <param name="version">Protocol Version</param>
        public virtual void DeserializeUpdate(BinaryReader stream, byte version)
        {
            Position = new Vector3(
                stream.ReadSingle(),
                stream.ReadSingle(),
                stream.ReadSingle());
            Radius = stream.ReadSingle();
            Orientation = stream.ReadInt16();

            // Properties
            foreach (var property in Properties)
                property.DeserializeUpdate(stream, version);
        }

        /// <summary>
        /// Liefert einen spechenden Namen für den ItemState (Name des Types) zurück.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return GetType().Name;
        }
    }
}