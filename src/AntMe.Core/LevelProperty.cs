namespace AntMe
{
    /// <summary>
    /// Basis-Klasse für alle Level Properties.
    /// </summary>
    public abstract class LevelProperty : Property
    {
        /// <summary>
        /// Referenz auf das zugehörige Level.
        /// </summary>
        public Level Level { get; private set; }

        /// <summary>
        /// Standard-Konstruktor.
        /// </summary>
        /// <param name="level">Referenz auf das Level.</param>
        public LevelProperty(Level level)
        {
            Level = level;
        }

        /// <summary>
        ///     Wird vom System aufgerufen, bevor die Settings an die Engine und an
        ///     die Fraktionen weitergegeben werden. Map, Engine und Factions
        ///     existieren, sind aber uninitialisiert.
        ///     - Level Settings anpassen
        ///     - Faction Settings anpassen
        ///     - Engine Extensions registrieren
        /// </summary>
        public virtual void DoSettings() { }

        /// <summary>
        ///     Wird vom System aufgerufen, um das Level zu initialisieren. Das
        ///     hier kann verwendet werden, um Listen, Trigger und Caches zu
        ///     initialisieren.
        ///     - Trigger registrieren
        ///     - Start Einheiten erzeugen
        /// </summary>
        public virtual void OnInit() { }

        /// <summary>
        ///     Wird vom System vor dem Engine-Update in jeder Simulationsrunde aufgerufen. An dieser
        ///     Stelle kann die Level Logik reagieren.
        /// </summary>
        public virtual void OnUpdate() { }

        /// <summary>
        /// Wird aufgerufen wenn ein neues Item in das Level eingefügt wird.
        /// </summary>
        /// <param name="item">Referenz auf das neue item.</param>
        public virtual void OnInsertItem(Item item) { }

        /// <summary>
        ///     Wird vom System aufgerufen, wenn Elemente aus der Simulation
        ///     entfernt wurden.
        /// </summary>
        /// <param name="item">Entferntes Element</param>
        public virtual void OnRemoveItem(Item item) { }
    }
}
