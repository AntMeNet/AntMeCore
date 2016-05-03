using System;

namespace AntMe
{
    /// <summary>
    /// Basis-Klasse für alle Factory Interop Klassen.
    /// </summary>
    public abstract class FactoryInterop : Interop { }

    /// <summary>
    /// Konkretisierte Basis-Klasse für Factory Interops.
    /// </summary>
    /// <typeparam name="F">Typ der dazugehörigen Faction</typeparam>
    public abstract class FactoryInterop<F> : FactoryInterop 
        where F : Faction
    {
        /// <summary>
        /// Referenz auf die zugrundeliegende Faction.
        /// </summary>
        protected readonly F Faction;

        /// <summary>
        /// Konstruktur der Interop-Klasse.
        /// </summary>
        /// <param name="faction">Instanz der zugehörigen Faction.</param>
        public FactoryInterop(F faction)
        {
            if (faction == null)
                throw new ArgumentNullException("faction");

            // Faction soll bereits teil eines Levels sein.
            if (faction.Level == null)
                throw new ArgumentException("Faction is not Part of a Level");

            Faction = faction;
        }
    }
}
