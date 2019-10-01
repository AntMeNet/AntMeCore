namespace AntMe
{
    /// <summary>
    ///     Base Class for all Engine Properties.
    /// </summary>
    public abstract class EngineProperty : Property
    {
        /// <summary>
        ///     Reference to the Engine.
        /// </summary>
        protected readonly Engine Engine;

        // Last ID: 0
        private readonly Tracer tracer = new Tracer("AntMe.EngineExtension");

        /// <summary>
        ///     Default Constructor for the Type Mapper.
        /// </summary>
        /// <param name="engine">Reference to the Engine</param>
        public EngineProperty(Engine engine)
        {
            Engine = engine;

            Engine.OnInsertItem += Insert;
            Engine.OnRemoveItem += Remove;
        }

        /// <summary>
        ///     Gets a call after Engine Initialization.
        /// </summary>
        public abstract void Init();

        /// <summary>
        ///     Gets a call after every Engine Update.
        /// </summary>
        public abstract void Update();

        /// <summary>
        ///     Gets a call after adding a new Item to the Engine.
        /// </summary>
        /// <param name="item">New Item</param>
        protected abstract void Insert(Item item);

        /// <summary>
        ///     Gets a call before removing an item from Engine.
        /// </summary>
        /// <param name="item">Removed Item</param>
        protected abstract void Remove(Item item);
    }
}