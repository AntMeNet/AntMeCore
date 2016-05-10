using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace AntMe
{
    /// <summary>
    ///     Root Klasse für die Erstellung eines AntMe! Level States. Durch Vererbung
    ///     können Map Packs weitere Informationen hier unterbringen.
    /// </summary>
    public sealed class LevelState : PropertyList<LevelStateProperty>, ISerializableState
    {
        /// <summary>
        ///     Gibt den Zeitpunkt der Erstellung an oder legt dieses fest.
        /// </summary>
        [DisplayName("Create Date")]
        [Description("")]
        [ReadOnly(true)]
        [Category("Dynamic")]
        public DateTimeOffset Date { get; set; }

        /// <summary>
        ///     Liefert eine Liste der beteiligten Fraktionen oder legt diese fest.
        /// </summary>
        public IList<FactionState> Factions;

        /// <summary>
        ///     Liefert eine Liste der beteiligten Spiel-Elemente oder legt diese fest.
        /// </summary>
        public IList<ItemState> Items;

        /// <summary>
        ///     Gibt Map Informationen oder legt diese fest.
        /// </summary>
        public MapState Map;

        /// <summary>
        ///     Gibt den Modus des Spiels an.
        /// </summary>
        [DisplayName("State")]
        [Description("")]
        [ReadOnly(true)]
        [Category("Dynamic")]
        public LevelMode Mode { get; set; }

        /// <summary>
        ///     Gibt die aktuelle Runde an oder legt diese fest.
        /// </summary>
        [DisplayName("Round")]
        [Description("")]
        [ReadOnly(true)]
        [Category("Dynamic")]
        public int Round { get; set; }

        /// <summary>
        /// Parameterloser Konstruktor für den Deserializer.
        /// </summary>
        public LevelState()
        {
            Round = 0;
            Map = null;
            Mode = LevelMode.Uninit;

            Date = DateTimeOffset.Now;
            Factions = new List<FactionState>();
            Items = new List<ItemState>();
        }

        /// <summary>
        /// Methode, die beim ersten Serialisieren dieser Instanz aufgerufen wird. 
        /// Dieser Call kann auch lange nach der Erstellung passieren, wenn 
        /// beispielsweise neue Zuschauer die States beobachten wollen.
        /// </summary>
        /// <param name="stream">Output Stream</param>
        /// <param name="version">Protocol Version</param>
        public void SerializeFirst(BinaryWriter stream, byte version)
        {
            stream.Write(Date.ToString("u"));
            stream.Write((byte)Mode);
            stream.Write(Round);
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
        public void SerializeUpdate(BinaryWriter stream, byte version)
        {
            stream.Write(Date.ToString("u"));
            stream.Write((byte)Mode);
            stream.Write(Round);
        }

        /// <summary>
        /// Methode, die beim ersten Erstellen aus einem Stream heraus aufgerufen 
        /// wird. dieser Call sollte alle Grundinformationen des States herstellen 
        /// und muss nicht dem State des ersten Frames entsprechen.
        /// </summary>
        /// <param name="stream">Input Stream</param>
        /// <param name="version">Protocol Version</param>
        public void DeserializeFirst(BinaryReader stream, byte version)
        {
            Date = DateTimeOffset.Parse(stream.ReadString()).ToLocalTime();
            Mode = (LevelMode)stream.ReadByte();
            Round = stream.ReadInt32();
        }

        /// <summary>
        /// Methode, die bei einem Folgeframe aufgerufen wird, nachdem der 
        /// State bereits initialisiert wurde.
        /// </summary>
        /// <param name="stream">Input Stream</param>
        /// <param name="version">Protocol Version</param>
        public void DeserializeUpdate(BinaryReader stream, byte version)
        {
            Date = DateTimeOffset.Parse(stream.ReadString()).ToLocalTime();
            Mode = (LevelMode)stream.ReadByte();
            Round = stream.ReadInt32();
        }
    }
}