using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

namespace AntMe
{
    /// <summary>
    /// Kern Komponente zur Simulation aller Spielelemente.
    /// </summary>
    public sealed class Engine : PropertyList<EngineProperty>
    {
        private readonly ITypeResolver typeResolver;

        private readonly HashSet<Item> items = new HashSet<Item>();
        private readonly Dictionary<int, Item> itemsById = new Dictionary<int, Item>();

        private readonly Queue<Item> insertQueue = new Queue<Item>();
        private readonly Queue<Item> removeQueue = new Queue<Item>();

        private int nextId = 1;

        // Last ID: 2
        private readonly Tracer tracer = new Tracer("AntMe.Engine");

        /// <summary>
        /// Standard Konstruktor für Engines.
        /// </summary>
        public Engine(ITypeResolver resolver)
        {
            tracer.Trace(TraceEventType.Information, 1, "Engine wird instanziiert");

            typeResolver = resolver;
            State = EngineState.Uninitialized;
            Round = -1;

            resolver.ResolveEngine(this);

            tracer.Trace(TraceEventType.Information, 2, "Engine ist instanziiert");
        }

        /// <summary>
        /// Gibt die Referenz auf den aktuell verwendeten Type Resolver zurück.
        /// </summary>
        public ITypeResolver TypeResolver { get { return typeResolver; } }

        /// <summary>
        /// Liefert die aktuelle Rundenzahl. Vor der ersten Runde liefert
        /// diese Eigenschaft -1.
        /// </summary>
        [DisplayName("Round")]
        [Description("Liefert die aktuelle Rundenzahl. Vor der ersten Runde liefert diese Eigenschaft -1.")]
        public int Round { get; private set; }

        /// <summary>
        /// Gibt den Status der Engine an.
        /// </summary>
        [DisplayName("State")]
        [Description("Gibt den Status der Engine an.")]
        public EngineState State { get; private set; }

        /// <summary>
        /// Gibt die Referenz auf die aktuell verwendete Map
        /// </summary>
        [Browsable(false)]
        public Map Map { get; private set; }

        /// <summary>
        /// Initialisiert die Engine mit passender Konfiguration und
        /// startet die Extensions.
        /// </summary>
        /// <param name="map"></param>
        public void Init(Map map)
        {
            // State check
            if (State != EngineState.Uninitialized)
                throw new NotSupportedException("Engine is already initialized");

            // Check Parameter
            if (map == null)
                throw new ArgumentNullException("map");

            // Prüft Karte
            map.CheckMap();
            Map = map;

            // Extensions initialisieren
            foreach (var property in Properties)
                property.Init();

            State = EngineState.Simulating;
        }

        /// <summary>
        /// Validiert, ob das Einfügen neuer Engine Properties zu diesem Zeitpunkt zulässig ist.
        /// </summary>
        /// <param name="property"></param>
        protected override void ValidateAddProperty(EngineProperty property)
        {
            if (State != EngineState.Uninitialized)
                throw new NotSupportedException("Engine is already initialized");
        }

        /// <summary>
        /// Versetzt die Engine in den finalisierten Modus.
        /// </summary>
        public void Finish()
        {
            State = EngineState.Finished;
        }

        /// <summary>
        /// Versetzt die Engine in den fehlgeschlagenen Modus.
        /// </summary>
        public void Fail()
        {
            State = EngineState.Failed;
        }

        /// <summary>
        /// Führt ein Update für die kommende Runde aus und startet dadruch die Berechnungen.
        /// </summary>
        public void Update()
        {
            // Darf nur gestartet werden, wenn das System initialisiert ist und sich nicht schon im Update-Modus befindet.
            if (State != EngineState.Simulating)
                throw new NotSupportedException("Engine is not ready");

            Round++;

            // Pre Update Call für alle Items
            foreach (Item item in items)
                item.BeforeUpdate();

            // Update Calls an alle Extensions
            foreach (var property in Properties)
                property.Update();

            // Post Update Call an alle Items
            foreach (Item item in items)
                item.AfterUpdate();

            // Add Items
            while (insertQueue.Count > 0)
                PrivateInsertItem(insertQueue.Dequeue());

            // Remove Items
            while (removeQueue.Count > 0)
                PrivateRemoveItem(removeQueue.Dequeue());

            // Rundenevent werfen
            if (OnNextRound != null)
                OnNextRound(Round);
        }

        /// <summary>
        /// Dieses Event informiert über entfernte Elemente.
        /// </summary>
        public event ChangeItem OnRemoveItem;

        /// <summary>
        /// Dieses Event informiert über eingefügte Elemente.
        /// </summary>
        public event ChangeItem OnInsertItem;

        /// <summary>
        /// Wird nach dem Update aufgerufen und informiert über das Ende einer
        /// neuen Runde.
        /// </summary>
        public event ValueUpdate<int> OnNextRound;

        #region Private Helfer

        /// <summary>
        /// Interne Methode zum Hinzufügen neuer Elemente am Ende von Update
        /// </summary>
        /// <param name="item"></param>
        private void PrivateInsertItem(Item item)
        {
            int id = nextId++;

            // Item zu den Listen der Engine hinzufügen
            items.Add(item);
            itemsById.Add(id, item);

            item.InternalInsertEngine(this, id);

            // Generiere Distanzinfos zu allen anderen Items
            RelocateItem(item);

            // Insert Call an alle Extensions
            foreach (var property in Properties)
                property.Insert(item);

            // Events
            if (OnInsertItem != null)
                OnInsertItem(item);
        }

        /// <summary>
        /// Interne Methode zum Entfernen der markierten Elemente. Wird von Update am Ende des Updates aufgerufen.
        /// </summary>
        /// <param name="item">Zu entfernendes Item</param>
        private void PrivateRemoveItem(Item item)
        {
            if (items.Contains(item))
            {
                // Event werfen
                if (OnRemoveItem != null)
                    OnRemoveItem(item);

                // Remove Call für alle Extensions
                foreach (var property in Properties)
                    property.Remove(item);

                // Entferne Item aus allen Engine Listen
                items.Remove(item);
                itemsById.Remove(item.Id);
                item.InternalRemoveEngine();
            }
        }

        /// <summary>
        /// Berechnet die neuen Daten der Distance Values.
        /// </summary>
        /// <param name="item"></param>
        private void RelocateItem(Item item)
        {
            Vector2 limit = Map.GetSize();

            // X-Achse
            if (item.Position.X < 0)
                item.Position = new Vector3(0, item.Position.Y, item.Position.Z);
            if (item.Position.X > limit.X)
                item.Position = new Vector3(limit.Y - Vector3.EPS_MIN, item.Position.Y, item.Position.Z);

            // Y-Achse
            if (item.Position.Y < 0)
                item.Position = new Vector3(item.Position.X, 0, item.Position.Z);
            if (item.Position.Y > limit.Y)
                item.Position = new Vector3(item.Position.X, limit.Y - Vector3.EPS_MIN, item.Position.Z);

            // Z-Achse
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
        /// Liefert eine lesbare Liste der enthaltenen Items.
        /// </summary>
        [Browsable(false)]
        public IEnumerable<Item> Items
        {
            get { return items; }
        }

        /// <summary>
        ///     Fügt das angegebene Element zur Simulation hinzu.
        /// </summary>
        /// <param name="item">Das einzufügende Element</param>
        public void InsertItem(Item item)
        {
            if (item == null)
                throw new ArgumentNullException();

            // Darf nur zur initialisierten Engine hinzugefügt werden
            if (State != EngineState.Simulating)
                throw new NotSupportedException("Engine must be in ready- or update-mode");

            // Prüfen, ob Item vielleicht schon in der Simulation läuft.
            if (items.Contains(item) || insertQueue.Contains(item))
                throw new InvalidOperationException("Item is already part of the Simulation");

            // Engine ist bereit - Item kann direkt eingefügt werden.
            if (State == EngineState.Simulating)
                insertQueue.Enqueue(item);                
        }

        /// <summary>
        ///     Entfernt das angegebene Element aus der Simulation.
        /// </summary>
        /// <param name="item"></param>
        public void RemoveItem(Item item)
        {
            if (item == null)
                throw new ArgumentNullException();

            // Darf nur zur initialisierten Engine hinzugefügt werden
            if (State != EngineState.Simulating)
                throw new NotSupportedException("Engine is not in ready- or update-Mode");

            // Prüfen, ob Element Teil der Engine ist
            if (!items.Contains(item) && !removeQueue.Contains(item))
                return;

            // Direkter Löschvorgang
            if (State == EngineState.Simulating)
                removeQueue.Enqueue(item);
        }

        #endregion
    }
}