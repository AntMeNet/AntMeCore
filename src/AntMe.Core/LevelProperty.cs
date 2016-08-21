namespace AntMe
{
    /// <summary>
    /// Base Class for all Level Properties.
    /// </summary>
    public abstract class LevelProperty : Property
    {
        /// <summary>
        /// Reference to the Level.
        /// </summary>
        public Level Level { get; private set; }

        /// <summary>
        /// Default Constructor for Type Mapper.
        /// </summary>
        /// <param name="level">Level</param>
        public LevelProperty(Level level)
        {
            Level = level;
        }

        /// <summary>
        /// Gets called before Level Initialization. Allows to set up final Stuff like
        /// - modify Level Settings
        /// - modify Faction/Slot Settings
        /// - modify Engine Extensions
        /// </summary>
        public virtual void DoSettings() { }

        /// <summary>
        /// Gets called during Level Initialization. Allows to set up Level Design.
        /// - adding Trigger
        /// - Generate Initial Items
        /// </summary>
        public virtual void OnInit() { }

        /// <summary>
        /// Gets a call before the Level updates everything.
        /// </summary>
        public virtual void OnBeforeUpdate() { }

        /// <summary>
        /// Gets called every Round to regulate the Situation.
        /// </summary>
        public virtual void OnUpdate() { }

        /// <summary>
        /// Gets a call after everything was updated.
        /// </summary>
        public virtual void OnAfterUpdate() { }

        /// <summary>
        /// Gets called when new Items came to the Level.
        /// </summary>
        /// <param name="item">New Item</param>
        public virtual void OnInsertItem(Item item) { }

        /// <summary>
        /// Gets called before Items will be deleted.
        /// </summary>
        /// <param name="item">Removed Item</param>
        public virtual void OnRemoveItem(Item item) { }
    }
}
