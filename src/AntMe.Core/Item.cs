using System;
using System.Collections.Generic;

namespace AntMe
{
    /// <summary>
    /// Abstract Base Class for all kind of Game Items.
    /// </summary>
    public abstract class Item : PropertyList<ItemProperty>
    {
        private Engine engine;
        private int id;
        private readonly Dictionary<Item, ItemInfo> _itemInfos = new Dictionary<Item, ItemInfo>();
        private ItemState state;

        private Index2 cell = Index2.Zero;
        private Angle orientation;
        private Vector3 position;
        private float radius;

        /// <summary>
        /// Default Constructor.
        /// </summary>
        /// <param name="context">Current Simulation Context</param>
        /// <param name="position">First Position of this Item</param>
        /// <param name="radius">Radius of this Item</param>
        /// <param name="orientation">First Orientation of this Item</param>
        public Item(SimulationContext context, Vector2 position, float radius, Angle orientation)
        {
            Context = context;
            Orientation = orientation;
            Radius = radius;
            Position = new Vector3(position.X, position.Y, 0);

            // Resolve Item Properties and Extender
            Context.Resolver.ResolveItem(this);
        }

        /// <summary>
        /// Reference to the attached Engine.
        /// </summary>
        public Engine Engine { get { return engine; } }

        /// <summary>
        /// Current Simulation Context
        /// </summary>
        public SimulationContext Context { get; private set; }

        /// <summary>
        /// Settings for this Item
        /// </summary>
        public Settings Settings { get { return Context.Settings; } }

        /// <summary>
        /// Randomizer
        /// </summary>
        public Random Random { get { return Context.Random; } }

        /// <summary>
        /// Id of this Game Item.
        /// </summary>
        public int Id
        {
            get { return id; }
        }

        /// <summary>
        /// Current Map Cell.
        /// </summary>
        public Index2 Cell
        {
            get { return cell; }
            internal set
            {
                if (cell != value)
                {
                    cell = value;
                    if (CellChanged != null)
                        CellChanged(this, value);
                }
            }
        }

        /// <summary>
        /// Item Orientation.
        /// </summary>
        public Angle Orientation
        {
            get { return orientation; }
            set
            {
                orientation = value;
                if (OrientationChanged != null)
                    OrientationChanged(this, value);
            }
        }

        /// <summary>
        /// Item Radius.
        /// </summary>
        public float Radius
        {
            get
            {
                return radius;
            }
            set
            {
                radius = value;
                if (RadiusChanged != null)
                    RadiusChanged(this, value);
            }
        }

        /// <summary>
        /// Item Position.
        /// </summary>
        public Vector3 Position
        {
            get { return position; }
            set
            {
                if (position != value)
                {
                    position = value;

                    // Zelle updaten
                    if (Engine != null)
                        Cell = Engine.Map.GetCellIndex(value);

                    //InvalidateDistances();
                    if (PositionChanged != null)
                        PositionChanged(this, value);
                }
            }
        }

        /// <summary>
        /// Wird vom Level zur Erstellung eines einfachen Info Objektes
        /// aufgerufen. Dieses Objekt wird als gekapselter Informationsaufruf
        /// (keine direkte Referenz) zwischen Spielelementen und deren
        /// Implementierung verwendet.
        /// </summary>
        /// <param name="observer">betrachtendes Objekt</param>
        /// <returns>Info zum Item</returns>
        public ItemInfo GetItemInfo(Item observer)
        {
            if (observer == null)
                throw new ArgumentNullException("observer");

            // Puffer prüfen
            if (_itemInfos.ContainsKey(observer))
                return _itemInfos[observer];

            // Neue Instanz erzeugen
            ItemInfo info = Context.Resolver.CreateItemInfo(this, observer);
            if (info == null)
                throw new NotSupportedException("Could not create new Game Item Info");

            _itemInfos.Add(observer, info);
            return info;
        }

        /// <summary>
        ///     Ermittelt aus dem übergebenen Info-Objekt wieder das enthaltene Item.
        /// </summary>
        /// <param name="info">Info Objekt</param>
        /// <returns>Das zugrunde liegende Item</returns>
        public Item GetItemFromInfo(ItemInfo info)
        {
            return info.GetItem();
        }

        /// <summary>
        /// Returns an Instance of 
        /// </summary>
        /// <returns>Neue Instanz des Item State</returns>
        public ItemState GetState()
        {
            // Beim ersten Aufruf muss der State neu gefüllt werden
            if (state == null)
                state = Context.Resolver.CreateItemState(this);
            OnBeforeState(state);
            return state;
        }

        /// <summary>
        ///     Das Event wird geworfen, wenn sich das Element in eine andere Zelle
        ///     bewegt hat.
        /// </summary>
        public event ValueChanged<Index2> CellChanged;

        /// <summary>
        ///     Das Event wird geworfen, wenn sich die aktuelle Position des Items
        ///     ändern sollte. Dies kann durch Extensions, das Spielelement oder
        ///     das Level passieren.
        /// </summary>
        public event ValueChanged<Vector3> PositionChanged;

        /// <summary>
        ///     Das Event wird geworfen, wenn sich die aktuelle Orientierung des Items
        ///     ändern sollte. Dies kann durch Extensions, das Spielelement oder das
        ///     Level passieren.
        /// </summary>
        public event ValueChanged<Angle> OrientationChanged;

        /// <summary>
        /// Das event wird geworfen wenn sich der aktuelle Radius ändert.
        /// </summary>
        public event ValueChanged<float> RadiusChanged;

        /// <summary>
        /// Gibt den Namen samt ID als Zeichenkette zurück.
        /// </summary>
        /// <returns>Name und ID</returns>
        public override string ToString()
        {
            return string.Format("{0} ({1})", GetType().Name, Id);
        }

        #region Engine Calls

        internal void InternalInsertEngine(Engine engine, int id)
        {
            this.engine = engine;
            this.id = id;

            OnInsert();
            if (Inserted != null)
                Inserted(this);
        }

        /// <summary>
        ///     Wird von der Engine aufgerufen, wenn das Item aus der Welt entfernt wurde
        /// </summary>
        internal void InternalRemoveEngine()
        {
            OnRemoved();
            if (Removed != null)
                Removed(this);

            engine = null;
            id = 0;

            // TODO: Cleanup Infos
        }

        /// <summary>
        ///     Wird von der Engine bei jedem Rundendurchlauf aufgerufen
        /// </summary>
        internal virtual void BeforeUpdate()
        {
            OnUpdate();
        }

        /// <summary>
        ///     Wird von der Engine nach jedem Rundendurchlauf aufgerufen
        /// </summary>
        internal virtual void AfterUpdate()
        {
            OnUpdated();
        }

        #endregion

        #region Property Management

        /// <summary>
        /// Überschriebener Validator für neue Properties.
        /// </summary>
        /// <param name="property">Neues Property</param>
        protected override void ValidateAddProperty(ItemProperty property)
        {
            if (Engine != null)
                throw new InvalidOperationException("Item is already in use");

            if (property.Item != this)
                throw new InvalidOperationException("Property uses the wrong Item");
        }

        #endregion

        #region Virtuelle Methoden

        /// <summary>
        ///     Wird beim Einfügen in die Engine aufgerufen. Zu diesem Zeitpunkt
        ///     ist die ID und die Engine bereits gesetzt.
        /// </summary>
        protected virtual void OnInsert() { }

        /// <summary>
        /// Kann überschrieben werden, um noch die letzten Infos in den State zu füllen
        /// </summary>
        protected virtual void OnBeforeState(ItemState state) { }

        /// <summary>
        ///     Wird vor jedem Engine Update aufgerufen.
        /// </summary>
        protected virtual void OnUpdate() { }

        /// <summary>
        ///     Wird nach jedem Engine Update aufgerufen.
        /// </summary>
        protected virtual void OnUpdated() { }

        /// <summary>
        ///     Wird beim Entfernen aus der Engine aufgerufen.
        /// </summary>
        protected virtual void OnRemoved() { }

        /// <summary>
        /// Signalisiert das Einfügen in eine Engine.
        /// </summary>
        public event ChangeItem Inserted;

        /// <summary>
        /// Event signalisiert das Entfernen dieses Items aus der Engine.
        /// </summary>
        public event ChangeItem Removed;

        #endregion

        #region Static Helper

        /// <summary>
        ///     Liefert die Entfernung zwischen zwei Elementen zurück.
        /// </summary>
        /// <param name="item1">Element 1</param>
        /// <param name="item2">Element 2</param>
        /// <returns>Entfernung zueinander</returns>
        public static float GetDistance(Item item1, Item item2)
        {
            // TODO: Optimize with cache maybe?
            return (item1.Position - item2.Position).Length();
        }

        /// <summary>
        ///     Liefert die Himmelsrichtung von Element 1 zu Element 2 zurück.
        /// </summary>
        /// <param name="item1">Element 1</param>
        /// <param name="item2">Element 2</param>
        /// <returns>Richtung</returns>
        public static Angle GetDirection(Item item1, Item item2)
        {
            return (item2.Position - item1.Position).ToAngleXY();
        }

        #endregion
    }
}