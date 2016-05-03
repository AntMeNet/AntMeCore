namespace AntMe
{
    /// <summary>
    /// Basis-Klasse für alle Item State Properties.
    /// </summary>
    public abstract class ItemStateProperty : StateProperty
    {
        /// <summary>
        /// Parameterloser Konstruktor für den Deserializer.
        /// </summary>
        public ItemStateProperty() : base() { }

        /// <summary>
        /// Standard-Konstruktor.
        /// </summary>
        /// <param name="property">Referenz auf das zugehörige Item Property.</param>
        public ItemStateProperty(ItemProperty property) : base(property) { }
    }
}
