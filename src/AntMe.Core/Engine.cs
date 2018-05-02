using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace AntMe
{
    /// <summary>
    /// Simulation Core Engine.
    /// </summary>
    public sealed class Engine : PropertyList<EngineProperty>
    {
        private readonly HashSet<Item> _items;
        private readonly Dictionary<int, Item> _itemsById;

        private readonly Queue<Item> _insertQueue;
        private readonly Queue<Item> _removeQueue;

        private int _nextId = 1;

        // Last ID: 2
        private readonly Tracer _tracer = new Tracer("AntMe.Engine");

        /// <summary>
        /// Default Constructor for Type Mapper
        /// </summary>
        /// <param name="resolver">Reference to the Type Resolver</param>
        public Engine(ITypeResolver resolver)
        {
            _tracer.Trace(TraceEventType.Information, 1, "Engine wird instanziiert");

            TypeResolver = resolver;
            State = EngineState.Uninitialized;
            Round = -1;

            _items = new HashSet<Item>();
            _itemsById = new Dictionary<int, Item>();
            _insertQueue = new Queue<Item>();
            _removeQueue = new Queue<Item>();

            resolver.ResolveEngine(this);

            _tracer.Trace(TraceEventType.Information, 2, "Engine ist instanziiert");
        }

        /// <summary>
        /// Reference to the Type Resolver.
        /// </summary>
        public ITypeResolver TypeResolver { get; }

        /// <summary>
        /// Gets the current Simulation Round or -1, of not started.
        /// </summary>
        public int Round { get; private set; }

        /// <summary>
        /// Gets the current State of the Engine.
        /// </summary>
        public EngineState State { get; private set; }

        /// <summary>
        /// Reference to the current Map.
        /// </summary>
        public Map Map { get; private set; }

        /// <summary>
        /// Initializes the Engine Instance.
        /// </summary>
        /// <param name="map">Map to use</param>
        public void Init(Map map)
        {
            // State check
            if (State != EngineState.Uninitialized)
                throw new NotSupportedException("Engine is already initialized");

            // Check Parameter
            if (map == null)
                throw new ArgumentNullException(nameof(map));

            // Check Map
            map.ValidateMap();
            Map = map;

            // Initialize Extensions
            foreach (var property in Properties)
                property.Init();

            State = EngineState.Simulating;
        }

        /// <summary>
        /// Validates the Properties to attach.
        /// </summary>
        /// <param name="property">Property</param>
        protected override void ValidateAddProperty(EngineProperty property)
        {
            if (State != EngineState.Uninitialized)
                throw new NotSupportedException("Engine is already initialized");
        }

        /// <summary>
        /// Finishs the current Simulation.
        /// </summary>
        public void Finish()
        {
            State = EngineState.Finished;
        }

        /// <summary>
        /// Fails the current Simulation.
        /// </summary>
        public void Fail()
        {
            State = EngineState.Failed;
        }

        /// <summary>
        /// Updates the current Simulation and simulates another Round.
        /// </summary>
        public void Update()
        {
            // Engine must be running.
            if (State != EngineState.Simulating)
                throw new NotSupportedException("Engine is not ready");

            Round++;

            // Pre Update Call for every Item
            foreach (Item item in _items)
                item.BeforeUpdate();

            // Update Calls for the Properties
            foreach (var property in Properties)
                property.Update();

            // Post Update Call for all Items
            foreach (Item item in _items)
                item.AfterUpdate();

            // Add new Items
            while (_insertQueue.Count > 0)
                PrivateInsertItem(_insertQueue.Dequeue());

            // Remove new Items
            while (_removeQueue.Count > 0)
                PrivateRemoveItem(_removeQueue.Dequeue());

            // Inform about another Round
            OnNextRound?.Invoke(Round);
        }

        /// <summary>
        /// Signal for removed Items.
        /// </summary>
        public event ChangeItem OnRemoveItem;

        /// <summary>
        /// Signal for added Items.
        /// </summary>
        public event ChangeItem OnInsertItem;

        /// <summary>
        /// Signal for another Round.
        /// </summary>
        public event ValueUpdate<int> OnNextRound;

        #region Private Helper

        /// <summary>
        /// Internal Method to add an Item to the Simulation.
        /// </summary>
        /// <param name="item">New Items</param>
        private void PrivateInsertItem(Item item)
        {
            int id = _nextId++;

            // Add Item to the internal Item List
            _items.Add(item);
            _itemsById.Add(id, item);

            item.InternalInsertEngine(this, id);

            // Generate Distance Information for the Item
            NormalizeItemPosition(item);

            // Inform about a new Item
            OnInsertItem?.Invoke(item);
        }

        /// <summary>
        /// Internal Method for removing an Item.
        /// </summary>
        /// <param name="item">Item to remove</param>
        private void PrivateRemoveItem(Item item)
        {
            if (_items.Contains(item))
            {
                // Inform about removed Item
                OnRemoveItem?.Invoke(item);

                // Remove Item from internal Lists
                _items.Remove(item);
                _itemsById.Remove(item.Id);
                item.InternalRemoveEngine();
            }
        }

        /// <summary>
        /// Normalizes the Position Information of this Item.
        /// Bring it back to Map Boundaries.
        /// </summary>
        /// <param name="item">Item</param>
        private void NormalizeItemPosition(Item item)
        {
            Vector2 limit = Map.GetSize();

            // X Axis
            if (item.Position.X < 0)
                item.Position = new Vector3(0, item.Position.Y, item.Position.Z);
            if (item.Position.X > limit.X)
                item.Position = new Vector3(limit.Y - Vector3.EpsMin, item.Position.Y, item.Position.Z);

            // Y Axis
            if (item.Position.Y < 0)
                item.Position = new Vector3(item.Position.X, 0, item.Position.Z);
            if (item.Position.Y > limit.Y)
                item.Position = new Vector3(item.Position.X, limit.Y - Vector3.EpsMin, item.Position.Z);

            // Z Axis
            float height = Map.GetHeight(new Vector2(item.Position.X, item.Position.Y));
            if (item.Position.Z < Map.MinZ || item.Position.Z < height)
                item.Position = new Vector3(item.Position.X, item.Position.Y, Math.Max(Map.MinZ, height));
            if (item.Position.Z > Map.MaxZ)
                item.Position = new Vector3(item.Position.X, item.Position.Y, Map.MaxZ - Vector3.EpsMin);

            item.Cell = Map.GetCellIndex(item.Position);
        }

        #endregion

        #region Item Management

        /// <summary>
        /// List of all Items.
        /// </summary>
        public IEnumerable<Item> Items
        {
            get { return _items; }
        }

        /// <summary>
        /// Adds the given Item to the Simulation.
        /// </summary>
        /// <param name="item">New Item</param>
        public void InsertItem(Item item)
        {
            if (item == null)
                throw new ArgumentNullException();

            // Engine must be running
            if (State != EngineState.Simulating)
                throw new NotSupportedException("Engine must be in ready- or update-mode");

            // Item can't be already Part of an Engine
            if (_items.Contains(item) || _insertQueue.Contains(item))
                throw new InvalidOperationException("Item is already part of the Simulation");

            // Queue to insert
            _insertQueue.Enqueue(item);                
        }

        /// <summary>
        /// Removes the given Item from the Simulation.
        /// </summary>
        /// <param name="item">Item to remove</param>
        public void RemoveItem(Item item)
        {
            if (item == null)
                throw new ArgumentNullException();

            // Engine must be running
            if (State != EngineState.Simulating)
                throw new NotSupportedException("Engine is not in ready- or update-Mode");

            // Item must be Part of the Simulation
            if (!_items.Contains(item) && !_removeQueue.Contains(item))
                return;

            // Queue to remove
            _removeQueue.Enqueue(item);
        }

        #endregion
    }
}