namespace AntMe
{
    /// <summary>
    /// Basis-Klasse für Properties von Factory Interops.
    /// </summary>
    public abstract class FactoryInteropProperty : InteropProperty
    {
        /// <summary>
        /// Referenz auf die zugehörige Faction.
        /// </summary>
        protected readonly Faction Faction;

        /// <summary>
        /// Referenz auf das zugehörige Interop Objekt.
        /// </summary>
        protected readonly FactoryInterop Interop;

        /// <summary>
        /// Konstruktor des Factory Interop Properties.
        /// </summary>
        /// <param name="faction">Referenz auf die Faction.</param>
        /// <param name="interop">Referenz auf das Interop.</param>
        public FactoryInteropProperty(Faction faction, FactoryInterop interop)
        {
            Faction = faction;
            Interop = interop;
        }
    }
}
