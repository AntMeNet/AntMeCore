using AntMe.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AntMe
{
    /// <summary>
    /// Base Class for all AntMe! Levels.
    /// </summary>
    public abstract class Level : PropertyList<LevelProperty>
    {
        /// <summary>
        /// Gives the maximum Number of Players per Level.
        /// </summary>
        public const byte MAX_SLOTS = 8;

        /// <summary>
        /// Frames per Second for a realtime Simulation.
        /// </summary>
        public const int FRAMES_PER_SECOND = 20;

        /// <summary>
        /// The complete List of all active Game Items.
        /// </summary>
        private readonly HashSet<Item> items;

        /// <summary>
        /// ID-Ordnered Dictionary of all active Game Items.
        /// </summary>
        private readonly Dictionary<int, Item> itemsById;

        /// <summary>
        /// Queue of all new Items.
        /// </summary>
        private readonly Queue<Item> insertQueue;

        /// <summary>
        /// Queue of all Items to remove.
        /// </summary>
        private readonly Queue<Item> removeQueue;

        private int nextId = 1;

        /// <summary>
        /// Slot specific Settings.
        /// </summary>
        private KeyValueStore[] slotSettings;

        // Last ID: 0
        private readonly Tracer tracer = new Tracer("AntMe.Level");

        /// <summary>
        /// Holds the current Simulation Context for this Level.
        /// </summary>
        public SimulationContext Context { get; private set; }

        /// <summary>
        /// Holds the Level Description from the Attribute.
        /// </summary>
        public LevelDescriptionAttribute LevelDescription { get; private set; }

        /// <summary>
        /// Returns the last occured Exception (on Failure)
        /// </summary>
        public Exception LastException { get; private set; }

        /// <summary>
        /// Returns the latest State.
        /// </summary>
        public Frame LatestFrame { get; private set; }

        /// <summary>
        /// Returns the Random Seed for the current instance.
        /// </summary>
        public int RandomSeed { get; private set; }

        /// <summary>
        /// Reference to the current Map.
        /// </summary>
        public Map Map { get; private set; }

        /// <summary>
        /// Global Settings for the whole Level.
        /// </summary>
        public KeyValueStore Settings { get { return Context.Settings; } }

        /// <summary>
        /// Level related Randomizer. Should be used for random generated Items 
        /// for the common level. All Factions have own Randomizer to use for 
        /// Faction-relatved stuff.
        /// </summary>
        public Random Random { get { return Context.Random; } }

        /// <summary>
        /// Returns the current Status of the Level.
        /// </summary>
        public SimulationState State { get; private set; }

        /// <summary>
        /// Gets the current Simulation Round or -1, of not started.
        /// </summary>
        public int Round { get; private set; }

        /// <summary>
        /// Returns a list of Slots related to the current Mode. e.g. On Mode 
        /// Finish the LevelModeSlots contains the winning Slots.
        /// </summary>
        public byte[] LevelModeSlots { get; private set; }

        /// <summary>
        /// List of Factions, ordered by Slot-Number.
        /// </summary>
        public Faction[] Factions { get; private set; }

        /// <summary>
        /// Default Constructor.
        /// </summary>
        protected Level(SimulationContext context)
        {
            Context = new SimulationContext(context.Resolver, context.Mapper, context.Settings);

            // Clone Settings for Slots
            slotSettings = new KeyValueStore[MAX_SLOTS];
            for (int i = 0; i < MAX_SLOTS; i++)
                slotSettings[i] = Settings.Clone();

            // Give the Level Designer the chance to change Faction/Slot Settings.
            DoSettings(Settings, slotSettings);

            State = SimulationState.Uninit;

            // Ermitteln des LevelDescriptionAttribute
            Type level = GetType();
            object[] levelDescriptions = level.GetCustomAttributes(typeof(LevelDescriptionAttribute), false);
            if (levelDescriptions.Length != 1)
            {
                State = SimulationState.InitFailed;
                throw new NotSupportedException("No Level Description found");
            }
            LevelDescription = (LevelDescriptionAttribute)levelDescriptions[0];

            // Level-Extensions
            context.Resolver.ResolveLevel(this);

            items = new HashSet<Item>();
            itemsById = new Dictionary<int, Item>();
            insertQueue = new Queue<Item>();
            removeQueue = new Queue<Item>();
            Round = -1;
        }

        #region Init

        /// <summary>
        /// Initializes the Level.
        /// </summary>
        /// <param name="slots">List of Slots</param>
        public void Init(params LevelSlot[] slots)
        {
            Init(0, slots);
        }

        /// <summary>
        /// Initializes the Level.
        /// </summary>
        /// <param name="randomSeed">Randomizer Seed</param>
        /// <param name="slots">List of Slots</param>
        public void Init(int randomSeed, params LevelSlot[] slots)
        {
            // Init only allowed in Uninit-Mode.
            if (State != SimulationState.Uninit)
                throw new NotSupportedException("Level is not ready for init");

            // Initialize the Randomizer.
            RandomSeed = (int)DateTime.Now.Ticks;
            if (randomSeed != 0)
                RandomSeed = randomSeed;

            // Generate the Simulation Context for this Level.
            Context = new SimulationContext(
                Context.Resolver,
                Context.Mapper,
                Context.Settings,
                new Random(RandomSeed));

            // Check for the right number of Slots.
            if (slots.Length > MAX_SLOTS)
            {
                var exception = new ArgumentOutOfRangeException("There are too many Slots");
                SetState(SimulationState.InitFailed, exception);
                throw exception;
            }

            // Generate Map and validate.
            Map map = MapSerializer.Deserialize(Context, GetMap());
            if (map == null)
            {
                var exception = new NotSupportedException("No Map was created");
                SetState(SimulationState.InitFailed, exception);
                throw exception;
            }
            map.ValidateMap();

            int minPlayer = LevelDescription.MinPlayerCount;
            int maxPlayer = LevelDescription.MaxPlayerCount;

            // TODO: Ermitteln der Filter Attribute
            //object[] levelFilters = GetType().GetCustomAttributes(typeof(FactionFilterAttribute), false);
            //var filters = new Dictionary<int, List<FactionFilterAttribute>>();
            //foreach (FactionFilterAttribute item in levelFilters)
            //{
            //    if (!filters.ContainsKey(item.SlotIndex))
            //        filters.Add(item.SlotIndex, new List<FactionFilterAttribute>());
            //    filters[item.SlotIndex].Add(item);
            //}

            // Check for Color Collisions.
            var colors = new HashSet<PlayerColor>();
            foreach (var slot in slots)
            {
                if (slot != null)
                {
                    if (colors.Contains(slot.Color))
                    {
                        var exception = new NotSupportedException("There are two Players with the same color");
                        SetState(SimulationState.InitFailed, exception);
                        throw exception;
                    }
                    colors.Add(slot.Color);
                }
            }

            // Gegencheck mit Level-Attributen
            int playerCount = 0;
            int highestSlot = 0;
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i] != null)
                {
                    // Counter
                    playerCount++;
                    highestSlot = i;

                    // TODO: Filter
                    //if (filters.ContainsKey(i) && filters[i].Count > 0)
                    //{
                    //    if (filters[i].Any(item => item.FactionType == slots[i].GetType()))
                    //        continue;

                    //    Mode = LevelMode.InitFailed;
                    //    Factions = null;
                    //    throw new NotSupportedException(string.Format(
                    //        "Faction '{0}' is not allowed in Slot {1}",
                    //        slots[i].GetType(), i));
                    //}
                }
            }

            // Faction Counts mit Map- und Level-Requirements gegenchecken
            if (playerCount < minPlayer)
            {
                var exception = new NotSupportedException(string.Format("Not enought player. Requires {0} Player", minPlayer));
                SetState(SimulationState.InitFailed, exception);
                throw exception;
            }

            if (playerCount > maxPlayer)
            {
                var exception = new NotSupportedException(string.Format("Too many player. Requires a Maximum of {0} Player", maxPlayer));
                SetState(SimulationState.InitFailed, exception);
                throw exception;
            }

            if (highestSlot > map.GetPlayerCount())
            {
                var exception = new NotSupportedException(string.Format("Too many Slots used. Map has only {0} Slots", map.GetPlayerCount()));
                SetState(SimulationState.InitFailed, exception);
                throw exception;
            }

            // Factions erzeugen
            Factions = new Faction[MAX_SLOTS];
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i] == null)
                    continue;

                SimulationContext factionContext = new SimulationContext(Context.Resolver, Context.Mapper, slotSettings[i]);

                // Identify and generate Faction
                try
                {
                    Factions[i] = Context.Resolver.CreateFaction(factionContext, slots[i].FactoryType, this);
                }
                catch (Exception ex)
                {
                    SetState(SimulationState.InitFailed, ex);
                    throw;
                }

                // In Case the Faction could not be found...
                if (Factions[i] == null)
                {
                    var exception = new Exception(string.Format("Cound not identify Faction for player {0}.", slots[i].Name));
                    SetState(SimulationState.InitFailed, exception);
                    throw exception;
                }
            }

            // Check Parameter
            if (map == null)
                throw new ArgumentNullException("map");

            // Check Map
            map.ValidateMap();
            Map = map;

            // Fraktionen ins Spiel einbetten
            for (byte i = 0; i < Factions.Length; i++)
            {
                if (Factions[i] == null)
                    continue;

                try
                {
                    Factions[i].Init(
                        i,
                        slots[i].Team,
                        slots[i].Name,
                        slots[i].Color,
                        new Random(RandomSeed + i + 1),
                        new Vector2(
                        (Map.StartPoints[i].X + 0.5f) * Map.CELLSIZE,
                        (Map.StartPoints[i].Y + 0.5f) * Map.CELLSIZE));
                }
                catch (Exception ex)
                {
                    SetState(SimulationState.PlayerException, ex, i);
                    Factions = null;
                    throw;
                }
            }

            // Initialisierung des Levels
            try
            {
                OnInit();
                foreach (var property in Properties)
                    property.OnInit();
            }
            catch (Exception ex)
            {
                SetState(SimulationState.InitFailed, ex);
                Factions = null;
                throw;
            }

            SetState(SimulationState.Running);
        }

        #endregion

        /// <summary>
        /// Generates the Map for this Level.
        /// </summary>
        /// <returns>Map Instance</returns>
        public abstract byte[] GetMap();

        /// <summary>
        /// Gives the Level Designer the chance to change Settings.
        /// </summary>
        protected virtual void DoSettings(KeyValueStore levelSettings, KeyValueStore[] slotSettings) { }

        /// <summary>
        /// This Method will be called after the basic Initialization to gives the 
        /// Level Designer the chance to add additional stuff like Triggers.
        /// </summary>
        protected virtual void OnInit() { }

        /// <summary>
        /// This Method will be called before the Simulation updates everything.
        /// </summary>
        protected virtual void OnBeforeUpdate() { }

        /// <summary>
        /// This Method will be called after every Simulation Update.
        /// </summary>
        protected virtual void OnUpdate() { }

        /// <summary>
        /// This Method will be called after the Simulation updated everything.
        /// </summary>
        protected virtual void OnAfterUpdate() { }

        /// <summary>
        /// This Method will be called for every added Item in the Simulation.
        /// </summary>
        /// <param name="item">New Item</param>
        protected virtual void OnInsertItem(Item item) { }

        /// <summary>
        /// This Method will be called for every removed Item.
        /// </summary>
        /// <param name="item">Removed Item</param>
        protected virtual void OnRemoveItem(Item item) { }

        /// <summary>
        ///     Führt die nächste Simulationsrunde durch und liefert anschließend
        ///     den neusten State der Simulation. Sollte vom Controller des Levels
        ///     aufgerufen werden.
        /// </summary>
        /// <returns>Aktueller State</returns>
        public Frame NextFrame()
        {
            // Alle Pre-Running States
            if (State != SimulationState.Running)
                throw new NotSupportedException("Level is not running");

            // Count up Rounds
            Round++;

            try
            {
                #region System Update (Before)

                // Level & Properties
                OnBeforeUpdate();
                foreach (var property in Properties)
                    property.OnBeforeUpdate();

                // Map & Properties + MapTiles
                Map.BeforeUpdate(Round);

                // Factions & Properties
                for (int i = 0; i < MAX_SLOTS; i++)
                    Factions[i]?.InternalBeforeUpdate();

                // Items
                foreach (var item in Items)
                    item.InternalBeforeUpdate();

                #endregion

                #region System Update

                // Level & Properties
                OnUpdate();
                foreach (var property in Properties)
                    property.OnUpdate();

                // Map & MapTiles & Properties
                Map.Update(Round);

                // Fations & Properties
                for (int i = 0; i < MAX_SLOTS; i++)
                    Factions[i]?.InternalUpdate();

                // Items & Properties
                foreach (var item in Items)
                    item.InternalUpdate();

                #endregion

                #region Add & Remove

                // Add new Items
                while (insertQueue.Count > 0)
                    PrivateInsertItem(insertQueue.Dequeue());

                // Remove new Items
                while (removeQueue.Count > 0)
                    PrivateRemoveItem(removeQueue.Dequeue());

                #endregion
            }
            catch (Exception ex)
            {
                // Handle System Error.
                SetState(SimulationState.SystemException, ex);
                throw;
            }

            CancellationTokenSource ts = new CancellationTokenSource();
            Task<bool> t = Task.Factory.StartNew<bool>(() =>
            {
                using (ts.Token.Register(Thread.CurrentThread.Abort))
                {
                    for (byte i = 0; i < MAX_SLOTS; i++)
                    {
                        Faction faction = Factions[i];
                        if (faction == null) continue;

                        try
                        {
                            // TODO: Track Time and Stuff
                            faction.InternalInterop();
                        }
                        catch (Exception ex)
                        {
                            // TODO: Player fault
                            SetState(SimulationState.PlayerException, ex, i);
                            return false;
                        }
                    }

                    return true;
                }
            }, ts.Token);
            if (!t.Wait(1000))
            {
                ts.Cancel();

                // -> Round Timeout
                // Cleanup
                SetState(SimulationState.PlayerException, new TimeoutException());
                return null;
            }

            // Cleanup if there was an Exception during Interop.
            if (!t.Result)
            {
                return null;
            }

            try
            {
                #region System Update (After)

                // Level & Properties
                OnAfterUpdate();
                foreach (var property in Properties)
                    property.OnAfterUpdate();

                // Map & Properties + MapTiles
                Map.AfterUpdate(Round);

                // Factions & Properties
                for (int i = 0; i < MAX_SLOTS; i++)
                    Factions[i]?.InternalAfterUpdate();

                // Items
                foreach (var item in Items)
                    item.InternalAfterUpdate();

                #endregion

                #region Create State

                // Create a new Instance of State
                if (LatestFrame == null)
                {
                    // Base State
                    LatestFrame = Context.Resolver.CreateLevelState(this);

                    // Map State
                    LatestFrame.Map = Map.GetState();

                    // Collect Faction States
                    for (int i = 0; i < MAX_SLOTS; i++)
                        if (Factions[i] != null)
                            LatestFrame.Factions.Add(Factions[i].GetFactionState());
                }

                LatestFrame.Round = Round;
                LatestFrame.Mode = State;

                // Remove old Items
                foreach (var item in LatestFrame.Items.ToArray())
                {
                    if (!items.Any(i => i.Id == item.Id))
                        LatestFrame.Items.Remove(item);
                }

                // Insert new Items
                foreach (Item item in items)
                {
                    ItemState itemState = item.GetState();
                    if (!LatestFrame.Items.Contains(itemState))
                        LatestFrame.Items.Add(itemState);
                }

                #endregion

                // Inform about another Round
                RoundChanged?.Invoke(Round);
            }
            catch (Exception ex)
            {
                // Handle System Error
                SetState(SimulationState.SystemException, ex);
                throw;
            }

            return LatestFrame;
        }

        /// <summary>
        /// Switches Level State and attaches the additional Information.
        /// </summary>
        /// <param name="state">New Level Mode</param>
        private void SetState(SimulationState state) { SetState(state, null, null); }

        /// <summary>
        /// Switches Level State and attaches the additional Information.
        /// </summary>
        /// <param name="state">New Level Mode</param>
        /// <param name="exception">Optional Exception for <see cref="LastException"/></param>
        private void SetState(SimulationState state, Exception exception) { SetState(state, exception, null); }

        /// <summary>
        /// Switches Level State and attaches the additional Information.
        /// </summary>
        /// <param name="state">New Level Mode</param>
        /// <param name="slots">Optional Slot Marker for <see cref="LevelModeSlots"/></param>
        private void SetState(SimulationState state, params byte[] slots) { SetState(state, null, slots); }

        /// <summary>
        /// Switches Level State and attaches the additional Information.
        /// </summary>
        /// <param name="state">New Level Mode</param>
        /// <param name="exception">Optional Exception for <see cref="LastException"/></param>
        /// <param name="slots">Optional Slot Marker for <see cref="LevelModeSlots"/></param>
        private void SetState(SimulationState state, Exception exception, params byte[] slots)
        {
            // Replace null with empty Array.
            if (slots == null)
                slots = new byte[0];

            State = state;
            LastException = exception;
            LevelModeSlots = slots;
        }

        #region Story Telling

        /// <summary>
        /// Finishes the Level with the given Slots as Winner.
        /// </summary>
        /// <param name="slots">List of Winner</param>
        protected void FinishPlayer(params byte[] slots)
        {
            // Only allowed in running mode
            if (State != SimulationState.Running)
                throw new NotSupportedException("Level is not running");

            // Make shure there is a winner
            if (slots.Length < 1)
                throw new ArgumentException("No Winner selected", "slots");

            for (int i = 0; i < slots.Length; i++)
            {
                // Check for valid Slot Index
                if (slots[i] < 0 && slots[i] >= MAX_SLOTS)
                    throw new ArgumentException(string.Format("Slot must be between 0 and {0}", MAX_SLOTS - 1), "slots");

                // Check for existing Faction
                if (Factions[slots[i]] == null)
                    throw new ArgumentException("Slot has no valid Faction");
            }

            SetState(SimulationState.Finished, slots);
        }

        /// <summary>
        /// Finishes the Level with the given Teams as Winner.
        /// </summary>
        /// <param name="teams">List of Winner Teams</param>
        protected void FinishTeam(params byte[] teams)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Finishes the Level with the given slots as Woser.
        /// </summary>
        /// <param name="slots">List of Loser</param>
        protected void FailPlayer(params byte[] slots)
        {
            // Only allowed in running mode
            if (State != SimulationState.Running)
                throw new NotSupportedException("Level is not running");

            // Make shure there is a winner
            if (slots.Length < 1)
                throw new ArgumentException("No Winner selected", "slots");

            for (int i = 0; i < slots.Length; i++)
            {
                // Check for valid Slot Index
                if (slots[i] < 0 && slots[i] >= MAX_SLOTS)
                    throw new ArgumentException(string.Format("Slot must be between 0 and {0}", MAX_SLOTS - 1), "slots");

                // Check for existing Faction
                if (Factions[slots[i]] == null)
                    throw new ArgumentException("Slot has no valid Faction");
            }

            SetState(SimulationState.Failed, slots);
        }

        /// <summary>
        /// Finishes the Level with the given slots as Woser.
        /// </summary>
        /// <param name="teams">List of Loser Teams</param>
        protected void FailTeam(params byte[] teams)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Finishes the Level mithout Winner nor Loser.
        /// </summary>
        protected void Draw()
        {
            // Only allowed in running mode
            if (State != SimulationState.Running)
                throw new NotSupportedException("Level is not running");

            SetState(SimulationState.Draw);
        }

        #endregion

        #region Item Management

        /// <summary>
        /// List of all Items.
        /// </summary>
        public IEnumerable<Item> Items
        {
            get { return items.AsEnumerable(); }
        }

        /// <summary>
        /// Adds the given Item to the Simulation.
        /// </summary>
        /// <param name="item">New Item</param>
        public void Insert(Item item)
        {
            if (item == null)
                throw new ArgumentNullException();

            if (State != SimulationState.Running && State != SimulationState.Uninit)
                throw new NotSupportedException("Level is not ready to add Items");

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
        public void Remove(Item item)
        {
            if (item == null)
                throw new ArgumentNullException();

            if (State != SimulationState.Running && State != SimulationState.Uninit)
                throw new NotSupportedException("Level is not ready to remove Items");

            // Item must be Part of the Simulation
            if (!items.Contains(item) && !removeQueue.Contains(item))
                return;

            // Queue to remove
            removeQueue.Enqueue(item);
        }

        public event ChangeItem InsertItem;

        public event ChangeItem RemoveItem;

        /// <summary>
        /// Signal for another Round.
        /// </summary>
        public event ValueUpdate<int> RoundChanged;

        #endregion

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
            OnInsertItem(item);
            InsertItem?.Invoke(item);
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
                RemoveItem?.Invoke(item);
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
    }
}