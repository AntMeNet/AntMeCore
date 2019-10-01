using System;

namespace AntMe
{
    /// <summary>
    ///     Abstract Base Class for all kind of Game Items.
    /// </summary>
    public abstract class Item : PropertyList<ItemProperty>
    {
        /// <summary>
        ///     Current Cell.
        /// </summary>
        private Index2 _cell = Index2.Zero;

        /// <summary>
        ///     Reference to the Engine
        /// </summary>
        private Engine _engine;

        /// <summary>
        ///     Current ID or 0 if not added.
        /// </summary>
        private int _id;

        /// <summary>
        ///     Current Orientation
        /// </summary>
        private Angle _orientation;

        /// <summary>
        ///     Current Position
        /// </summary>
        private Vector3 _position;

        /// <summary>
        ///     Current Radius
        /// </summary>
        private float _radius;

        /// <summary>
        ///     Instance of the Item State.
        /// </summary>
        private ItemState _state;

        /// <summary>
        ///     Instance of the item info.
        /// </summary>
        private ItemInfo _info;

        /// <summary>
        ///     Default Constructor.
        /// </summary>
        /// <param name="context">Current Simulation Context</param>
        /// <param name="position">First Position of this Item</param>
        /// <param name="radius">Radius of this Item</param>
        /// <param name="orientation">First Orientation of this Item</param>
        protected Item(SimulationContext context, Vector2 position, float radius, Angle orientation)
            : this(context, null, position, radius, orientation)
        {
        }

        /// <summary>
        ///     Default Constructor.
        /// </summary>
        /// <param name="context">Current Simulation Context</param>
        /// <param name="attributes">List of Unit Attributes</param>
        /// <param name="position">First Position of this Item</param>
        /// <param name="radius">Radius of this Item</param>
        /// <param name="orientation">First Orientation of this Item</param>
        protected Item(SimulationContext context, UnitAttributeCollection attributes, Vector2 position, float radius,
            Angle orientation)
        {
            Context = context;
            Orientation = orientation;
            Radius = radius;
            Position = new Vector3(position.X, position.Y, 0);
            Attributes = attributes;

            // Resolve Item Properties and Extender
            Context.Resolver.ResolveItem(this);
        }

        /// <summary>
        ///     Reference to the attached Engine.
        /// </summary>
        public Engine Engine => _engine;

        /// <summary>
        ///     Current Simulation Context
        /// </summary>
        public SimulationContext Context { get; }

        /// <summary>
        ///     Settings for this Item
        /// </summary>
        public KeyValueStore Settings => Context.Settings;

        /// <summary>
        ///     Randomizer
        /// </summary>
        public Random Random => Context.Random;

        /// <summary>
        ///     Collection of Attributes for this Item.
        /// </summary>
        public UnitAttributeCollection Attributes { get; }

        /// <summary>
        ///     Id of this Game Item.
        /// </summary>
        public int Id => _id;

        /// <summary>
        ///     Current Map Cell.
        /// </summary>
        public Index2 Cell
        {
            get => _cell;
            internal set
            {
                if (_cell != value)
                {
                    _cell = value;
                    CellChanged?.Invoke(this, value);
                }
            }
        }

        /// <summary>
        ///     Item Orientation.
        /// </summary>
        public Angle Orientation
        {
            get => _orientation;
            set
            {
                _orientation = value;
                OrientationChanged?.Invoke(this, value);
            }
        }

        /// <summary>
        ///     Item Radius.
        /// </summary>
        public float Radius
        {
            get => _radius;
            set
            {
                _radius = value;
                RadiusChanged?.Invoke(this, value);
            }
        }

        /// <summary>
        ///     Item Position.
        /// </summary>
        public Vector3 Position
        {
            get => _position;
            set
            {
                if (_position != value)
                {
                    _position = value;

                    // Zelle updaten
                    if (Engine != null)
                        Cell = Engine.Map.GetCellIndex(value);

                    //InvalidateDistances();
                    PositionChanged?.Invoke(this, value);
                }
            }
        }

        /// <summary>
        ///     Generates an Info Object for the given Observer.
        /// </summary>
        /// <returns>Info Object</returns>
        public ItemInfo GetItemInfo()
        {
            if (_info == null)
                _info =Context.Resolver.CreateItemInfo(this);

            if (_info == null)
                throw new NotSupportedException("Could not create new Game Item Info");

            return _info;
        }

        /// <summary>
        ///     Returns the real Item from an Info Object.
        /// </summary>
        /// <param name="info">Info Objekt</param>
        /// <returns>Related Item</returns>
        public Item GetItemFromInfo(ItemInfo info)
        {
            // Check for Cheaters
            if (Engine != info.GetItem().Engine)
                throw new NotSupportedException("Invalid GetItemFromInfo call");

            return info.GetItem();
        }

        /// <summary>
        ///     Returns the Item State.
        /// </summary>
        /// <returns>Item State</returns>
        public ItemState GetState()
        {
            // Create new Instance one first Call.
            if (_state == null)
                _state = Context.Resolver.CreateItemState(this);

            OnBeforeState(_state);
            return _state;
        }

        #region Property Management

        /// <summary>
        ///     Vaidator for new Properties. The default implementation prevents
        ///     adding new Properties to the Item if it's already part of an Engine.
        /// </summary>
        /// <param name="property">New Property.</param>
        protected override void ValidateAddProperty(ItemProperty property)
        {
            if (Engine != null)
                throw new InvalidOperationException("Item is already in use");

            if (property.Item != this)
                throw new InvalidOperationException("Property uses the wrong Item");
        }

        #endregion

        /// <summary>
        ///     Returns a readable String representation for this Item.
        /// </summary>
        /// <returns>Name and ID</returns>
        public override string ToString()
        {
            return string.Format("{0} ({1})", GetType().Name, Id);
        }

        #region Events

        /// <summary>
        ///     Signal for a changed Cell.
        /// </summary>
        public event ValueChanged<Index2> CellChanged;

        /// <summary>
        ///     Signal for a changed Position.
        /// </summary>
        public event ValueChanged<Vector3> PositionChanged;

        /// <summary>
        ///     Signal for a changed Orientation.
        /// </summary>
        public event ValueChanged<Angle> OrientationChanged;

        /// <summary>
        ///     Signal for a changed Radius.
        /// </summary>
        public event ValueChanged<float> RadiusChanged;

        /// <summary>
        ///     Signal for adding Item to the Engine.
        /// </summary>
        public event ChangeItem Inserted;

        /// <summary>
        ///     Signal for removing Item from Engine.
        /// </summary>
        public event ChangeItem Removed;

        #endregion

        #region Engine Calls

        /// <summary>
        ///     Internal Call for adding this Item to an Engine. Get called by the Engine.
        /// </summary>
        /// <param name="engine">Engine</param>
        /// <param name="id">New Id</param>
        internal void InternalInsertEngine(Engine engine, int id)
        {
            this._engine = engine;
            this._id = id;

            OnInsert();
            Inserted?.Invoke(this);
        }

        /// <summary>
        ///     Internal Call for removing this Item from its current Engine.
        /// </summary>
        internal void InternalRemoveEngine()
        {
            OnRemoved();
            Removed?.Invoke(this);

            _engine = null;
            _id = 0;

            // TODO: Cleanup Infos
        }

        /// <summary>
        ///     Internal Call before the Item gets Updated.
        /// </summary>
        internal virtual void BeforeUpdate()
        {
            OnUpdate();
        }

        /// <summary>
        ///     Internal Call after the Item gets updated.
        /// </summary>
        internal virtual void AfterUpdate()
        {
            OnUpdated();
        }

        #endregion

        #region Virtual Methods

        /// <summary>
        ///     Gets called after the Item was added to an Engine.
        /// </summary>
        protected virtual void OnInsert()
        {
        }

        /// <summary>
        ///     Gets called before the Engine get the Item State. This gives the
        ///     change to add additional Information into the State.
        /// </summary>
        protected virtual void OnBeforeState(ItemState state)
        {
        }

        /// <summary>
        ///     Gets Called before the Item gets updated.
        /// </summary>
        protected virtual void OnUpdate()
        {
        }

        /// <summary>
        ///     Gets called after the Item gets updated.
        /// </summary>
        protected virtual void OnUpdated()
        {
        }

        /// <summary>
        ///     Gets called before the Item will be removed from the Engine.
        /// </summary>
        protected virtual void OnRemoved()
        {
        }

        #endregion

        #region Static Helper

        /// <summary>
        ///     Calculates the Distance between two Items. (Center to Center)
        /// </summary>
        /// <param name="item1">Item 1</param>
        /// <param name="item2">Item 2</param>
        /// <returns>Distance</returns>
        public static float GetDistance(Item item1, Item item2)
        {
            return (item1.Position - item2.Position).Length();
        }

        /// <summary>
        ///     Calculates the Direction from one Item to another.
        /// </summary>
        /// <param name="item1">Item 1</param>
        /// <param name="item2">Item 2</param>
        /// <returns>Direction from Item 1 to Item 2</returns>
        public static Angle GetDirection(Item item1, Item item2)
        {
            return (item2.Position - item1.Position).ToAngleXY();
        }

        #endregion
    }
}