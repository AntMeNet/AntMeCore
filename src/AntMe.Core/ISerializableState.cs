using System.IO;

namespace AntMe
{
    /// <summary>
    /// Basis-Interface für alle serialisierbaren Properties.
    /// </summary>
    public interface ISerializableState
    {
        /// <summary>
        /// Methode, die beim ersten Serialisieren dieser Instanz aufgerufen wird. 
        /// Dieser Call kann auch lange nach der Erstellung passieren, wenn 
        /// beispielsweise neue Zuschauer die States beobachten wollen.
        /// </summary>
        /// <param name="stream">Output Stream</param>
        /// <param name="version">Protocol Version</param>
        void SerializeFirst(BinaryWriter stream, byte version);

        /// <summary>
        /// Methode, die beim erneuten Serialisieren dieser Instanz aufgerufen 
        /// wird. Hier müssen nur noch veränderbare Daten geschickt werden. Dieser 
        /// Call muss nicht zwangsläufig in jedem Frame stattfinden, wird aber 
        /// ganz sicher immer vom selben Client abgerufen - es können also diffs 
        /// gesendet werden.
        /// </summary>
        /// <param name="stream">Output Stream</param>
        /// <param name="version">Protocol Version</param>
        void SerializeUpdate(BinaryWriter stream, byte version);

        /// <summary>
        /// Methode, die beim ersten Erstellen aus einem Stream heraus aufgerufen 
        /// wird. dieser Call sollte alle Grundinformationen des States herstellen 
        /// und muss nicht dem State des ersten Frames entsprechen.
        /// </summary>
        /// <param name="stream">Input Stream</param>
        /// <param name="version">Protocol Version</param>
        void DeserializeFirst(BinaryReader stream, byte version);

        /// <summary>
        /// Methode, die bei einem Folgeframe aufgerufen wird, nachdem der 
        /// State bereits initialisiert wurde.
        /// </summary>
        /// <param name="stream">Input Stream</param>
        /// <param name="version">Protocol Version</param>
        void DeserializeUpdate(BinaryReader stream, byte version);
    }
}
