namespace AntMe
{
    /// <summary>
    /// Base Class for all Faction Properties.
    /// </summary>
    public abstract class FactionProperty : Property
    {
        private readonly Faction faction;

        /// <summary>
        /// Default Constructor.
        /// </summary>
        /// <param name="faction">Reference to the related Faction.</param>
        public FactionProperty(Faction faction) : base()
        {
            this.faction = faction;
        }

        /// <summary>
        /// Reference to the related Faction.
        /// </summary>
        public Faction Faction { get { return faction; } }

        /// <summary>
        /// Gets called by the Engine during Faction Initialization.
        /// </summary>
        public virtual void Init() { }
    }
}
