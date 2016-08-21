namespace AntMe
{
    /// <summary>
    /// Base Class for all Faction Properties.
    /// </summary>
    public abstract class FactionProperty : Property
    {
        /// <summary>
        /// Reference to the Faction.
        /// </summary>
        protected readonly Faction Faction;

        /// <summary>
        /// Default Constructor.
        /// </summary>
        /// <param name="faction">Reference to the related Faction.</param>
        public FactionProperty(Faction faction) : base()
        {
            Faction = faction;
        }

        /// <summary>
        /// Gets called by the Engine during Faction Initialization.
        /// </summary>
        public virtual void Init() { }

        /// <summary>
        /// Gets a call before the Update Process.
        /// </summary>
        public virtual void OnBeforeUpdate() { }

        /// <summary>
        /// Gets a call during the Update Process.
        /// </summary>
        public virtual void OnUpdate() { }

        /// <summary>
        /// Gets a call after the Update Process.
        /// </summary>
        public virtual void OnAfterUpdate() { }
    }
}
