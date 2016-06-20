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
        private readonly ITypeResolver typeResolver;

        private readonly HashSet<Item> items;
        private readonly Dictionary<int, Item> itemsById;

        private readonly Queue<Item> insertQueue;
        private readonly Queue<Item> removeQueue;

        private int nextId = 1;

        // Last ID: 2
        private readonly Tracer tracer = new Tracer("AntMe.Engine");

        /// <summary>
        /// Default Constructor for Type Mapper
        /// </summary>
        /// <param name="resolver">Reference to the Type Resolver</param>
        public Engine(ITypeResolver resolver)
        {
            tracer.Trace(TraceEventType.Information, 1, "Engine wird instanziiert");

            typeResolver = resolver;
            State = EngineState.Uninitialized;
            Round = -1;

            items = new HashSet<Item>();
            itemsById = new Dictionary<int, Item>();
            insertQueue = new Queue<Item>();
            removeQueue = new Queue<Item>();

            resolver.ResolveEngine(this);

            tracer.Trace(TraceEventType.Information, 2, "Engine ist instanziiert");
        }

        /// <summary>
        /// Reference to the Type Resolver.
        /// </summary>
        public ITypeResolver TypeResolver { get { return typeResolver; } }

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
                throw new ArgumentNullException("map");

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
            foreach (Item item in items)
                item.BeforeUpdate();

            // Update Calls for the Properties
            foreach (var property in Properties)
                property.Update();

            // Post Update Call for all Items
            foreach (Item item in items)
                item.AfterUpdate();

            // Add new Items
            while (insertQueue.Count > 0)
                PrivateInsertItem(insertQueue.Dequeue());

            // Remove new Items
            while (removeQueue.Count > 0)
                PrivateRemoveItem(removeQueue.Dequeue());

            // Inform about another Round
            if (OnNextRound != null)
                OnNextRound(Round);
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
            int id = nextId++;

            // Add Item to the internal Item List
            items.Add(item);
            itemsById.Add(id, item);

            item.InternalInsertEngine(this, id);

            // Generate Distance Information for the Item
            NormalizeItemPosition(item);

            // Inform about a new Item
            if (OnInsertItem != null)
                OnInsertItem(item);
        }

        /// <summary>
        /// Internal Method for removing an Item.
        /// </summary>
        /// <param name="item">Item to remove</param>
        private void PrivateRemoveItem(Item item)
        {
            if (items.Contains(item))
            {
                // Inform about removed Item
                if (OnRemoveItem != null)
                    OnRemoveItem(item);

                // Remove Item from internal Lists
                items.Remove(item);
                itemsById.Remove(item.Id);
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
                item.Position = new Vector3(limit.Y - Vector3.EPS_MIN, item.Position.Y, item.Position.Z);

            // Y Axis
            if (item.Position.Y < 0)
                item.Position = new Vector3(item.Position.X, 0, item.Position.Z);
            if (item.Position.Y > limit.Y)
                item.Position = new Vector3(item.Position.X, limit.Y - Vector3.EPS_MIN, item.Position.Z);

            // Z Axis
            float height = Map.GetHeight(new Vector2(item.Position.X, item.Position.Y));
            if (item.Position.Z < Map.MIN_Z || item.Position.Z < height)
                item.Position = new Vector3(item.Position.X, item.Position.Y, Math.Max(Map.MIN_Z, height));
            if (item.Position.Z > Map.MAX_Z)
                item.Position = new Vector3(item.Position.X, item.Position.Y, Map.MAX_Z - Vector3.EPS_MIN);

            item.Cell = Map.GetCellIndex(item.Position);
        }

        #endregion

        #region Item Management

        /// <summary>
        /// List of all Items.
        /// </summary>
        public IEnumerable<Item> Items
        {
            get { return items; }
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
            if (items.Contains(item) || insertQueue.Contains(item))
                throw new InvalidOperationException("Item is already part of the Simulation");

            // Queue to insert
            insertQueue.Enqueue(item);                
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
            if (!items.Contains(item) && !removeQueue.Contains(item))
                return;

            // Queue to remove
            removeQueue.Enqueue(item);
        }

        #endregion
    }
}