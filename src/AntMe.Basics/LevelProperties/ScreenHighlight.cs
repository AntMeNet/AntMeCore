namespace AntMe
{
    /// <summary>
    ///     Klasse zur Repräsentanz von UI Highlights zur Weitergabe im State.
    /// </summary>
    public abstract class ScreenHighlight
    {
        /// <summary>
        ///     Zu übertragende Nachricht.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        ///     Soll die Nachricht den Client blockieren?
        /// </summary>
        public bool Blocking { get; set; }

        /// <summary>
        ///     Summe an Sekunden, die die Nachricht zu sehen sein soll.
        /// </summary>
        public int Delay { get; set; }

        /// <summary>
        ///     Bitfeld zur Angabe des Players, für den die Nachricht gilt.
        /// </summary>
        public byte TargetPlayer { get; set; }
    }
}