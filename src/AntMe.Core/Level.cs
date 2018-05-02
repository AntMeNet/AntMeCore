using AntMe.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

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
        public const byte MaxSlots = 8;

        /// <summary>
        /// Frames per Second for a realtime Simulation.
        /// </summary>
        public const int FramesPerSecond = 20;

        /// <summary>
        /// Simulation Engine.
        /// </summary>
        private Engine _engine;

        /// <summary>
        /// Slot specific Settings.
        /// </summary>
        private readonly KeyValueStore[] _slotSettings;

        /// <summary>
        /// Holds the current Simulation Context for this Level.
        /// </summary>
        public SimulationContext Context { get; private set; }

        /// <summary>
        /// Holds the Level Description from the Attribute.
        /// </summary>
        public LevelDescriptionAttribute LevelDescription { get; }

        /// <summary>
        /// Returns the last occured Exception (on Failure)
        /// </summary>
        public Exception LastException { get; private set; }

        /// <summary>
        /// Returns the latest State.
        /// </summary>
        public LevelState State { get; private set; }

        /// <summary>
        /// Returns the Random Seed for the current instance.
        /// </summary>
        public int RandomSeed { get; private set; }

        /// <summary>
        /// Global Settings for the whole Level.
        /// </summary>
        public KeyValueStore Settings => Context.Settings;

        /// <summary>
        /// Reference to the Simulation Engine.
        /// </summary>
        public Engine Engine => _engine;

        /// <summary>
        /// Level related Randomizer. Should be used for random generated Items 
        /// for the common level. All Factions have own Randomizer to use for 
        /// Faction-relatved stuff.
        /// </summary>
        public Random Random => Context.Random;

        /// <summary>
        /// Returns the current Status of the Level.
        /// </summary>
        public LevelMode Mode { get; private set; }

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
            _slotSettings = new KeyValueStore[MaxSlots];
            for (int i = 0; i < MaxSlots; i++)
                _slotSettings[i] = Settings.Clone();

            // Give the Level Designer the chance to change Faction/Slot Settings.
            DoSettings(Settings, _slotSettings);

            Mode = LevelMode.Uninit;

            // Ermitteln des LevelDescriptionAttribute
            var level = GetType();
            var levelDescriptions = level.GetCustomAttributes(typeof(LevelDescriptionAttribute), false);
            if (levelDescriptions.Length != 1)
            {
                Mode = LevelMode.InitFailed;
                throw new NotSupportedException("No Level Description found");
            }
            LevelDescription = (LevelDescriptionAttribute)levelDescriptions[0];

            // Level-Extensions
            context.Resolver.ResolveLevel(this);
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
            if (Mode != LevelMode.Uninit)
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
            if (slots.Length > MaxSlots)
            {
                var exception = new ArgumentOutOfRangeException(nameof(slots), "There are too many Slots");
                SetMode(LevelMode.InitFailed, exception);
                throw exception;
            }

            // Generate Engine
            _engine = new Engine(Context.Resolver);

            // Generate Map and validate.
            var map = MapSerializer.Deserialize(Context, GetMap());
            if (map == null)
            {
                var exception = new NotSupportedException("No Map was created");
                SetMode(LevelMode.InitFailed, exception);
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
                        SetMode(LevelMode.InitFailed, exception);
                        throw exception;
                    }
                    colors.Add(slot.Color);
                }
            }

            // Gegencheck mit Level-Attributen
            var playerCount = 0;
            var highestSlot = 0;
            for (var i = 0; i < slots.Length; i++)
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
                var exception = new NotSupportedException($"Not enought player. Requires {minPlayer} Player");
                SetMode(LevelMode.InitFailed, exception);
                throw exception;
            }

            if (playerCount > maxPlayer)
            {
                var exception = new NotSupportedException($"Too many player. Requires a Maximum of {maxPlayer} Player");
                SetMode(LevelMode.InitFailed, exception);
                throw exception;
            }

            if (highestSlot > map.GetPlayerCount())
            {
                var exception = new NotSupportedException(
                    $"Too many Slots used. Map has only {map.GetPlayerCount()} Slots");
                SetMode(LevelMode.InitFailed, exception);
                throw exception;
            }

            // Factions erzeugen
            Factions = new Faction[MaxSlots];
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i] == null)
                    continue;

                SimulationContext factionContext = new SimulationContext(Context.Resolver, Context.Mapper, _slotSettings[i]);

                // Identify and generate Faction
                try
                {
                    Factions[i] = Context.Resolver.CreateFaction(factionContext, slots[i].FactoryType, this);
                }
                catch (Exception ex)
                {
                    SetMode(LevelMode.InitFailed, ex);
                    throw;
                }

                // In Case the Faction could not be found...
                if (Factions[i] == null)
                {
                    var exception = new Exception($"Cound not identify Faction for player {slots[i].Name}.");
                    SetMode(LevelMode.InitFailed, exception);
                    throw exception;
                }
            }

            _engine.Init(map);
            _engine.OnInsertItem += engine_OnInsertItem;
            _engine.OnRemoveItem += engine_OnRemoveItem;

            // Fraktionen ins Spiel einbetten
            for (byte i = 0; i < Factions.Length; i++)
            {
                try
                {
                    InitFaction(i, slots[i], Factions[i]);
                }
                catch (Exception ex)
                {
                    SetMode(LevelMode.PlayerException, ex, i);
                    Factions = null;
                    throw;
                }
            }

            // Initialisierung des Levels
            try
            {
                OnInit();
            }
            catch (Exception ex)
            {
                SetMode(LevelMode.InitFailed, ex);
                Factions = null;
                throw;
            }

            SetMode(LevelMode.Running);
        }

        private void engine_OnInsertItem(Item item)
        {
            OnInsertItem(item);
        }

        private void engine_OnRemoveItem(Item item)
        {
            OnRemoveItem(item);
        }

        /// <summary>
        ///     Initialisiert die systemeigenen Fraktionstypen (Ameisen, Wanzen, Viren)
        /// </summary>
        /// <param name="slotIndex"></param>
        /// <param name="slot"></param>
        /// <param name="faction"></param>
        private void InitFaction(byte slotIndex, LevelSlot slot, Faction faction)
        {
            faction?.Init(
                slotIndex,
                slot.Team,
                slot.Name,
                slot.Color,
                new Random(RandomSeed + slotIndex + 1),
                new Vector2(
                    (_engine.Map.StartPoints[slotIndex].X + 0.5f) * Map.Cellsize,
                    (_engine.Map.StartPoints[slotIndex].Y + 0.5f) * Map.Cellsize));
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
        /// This Method will be called after every Simulation Update.
        /// </summary>
        protected virtual void OnUpdate() { }

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
        public LevelState NextState()
        {
            // Alle Pre-Running States
            if (Mode != LevelMode.Running)
                throw new NotSupportedException("Level is not running");

            // TODO: try/catch für Programmierfehler
            // TODO: Watchdog für Laufzeit-Überschreitung

            _engine.Update();

            // Updates der Factions
            for (int i = 0; i < MaxSlots; i++)
            {
                Factions[i]?.Update(_engine.Round);
            }

            try
            {
                // Update Level Properties
                foreach (var property in Properties)
                    property.OnUpdate();

                OnUpdate();

            }
            catch (Exception ex)
            {
                SetMode(LevelMode.SystemException, ex);
            }


            // Create a new Instance of State
            if (State == null)
            {
                // Base State
                State = Context.Resolver.CreateLevelState(this);

                // Map State
                State.Map = Engine.Map.GetState();

                // Collect Faction States
                for (int i = 0; i < MaxSlots; i++)
                    if (Factions[i] != null)
                        State.Factions.Add(Factions[i].GetFactionState());
            }

            State.Round = _engine.Round;
            State.Mode = Mode;

            // Remove old Items
            foreach (var item in State.Items.ToArray())
            {
                if (_engine.Items.All(i => i.Id != item.Id))
                    State.Items.Remove(item);
            }

            // Insert new Items
            foreach (Item item in _engine.Items)
            {
                ItemState itemState = item.GetState();
                if (!State.Items.Contains(itemState))
                    State.Items.Add(itemState);
            }

            return State;
        }

        /// <summary>
        /// Switches Game Mode and attaches the additional Information.
        /// </summary>
        /// <param name="mode">New Level Mode</param>
        private void SetMode(LevelMode mode) { SetMode(mode, null, null); }

        /// <summary>
        /// Switches Game Mode and attaches the additional Information.
        /// </summary>
        /// <param name="mode">New Level Mode</param>
        /// <param name="exception">Optional Exception for <see cref="LastException"/></param>
        private void SetMode(LevelMode mode, Exception exception) { SetMode(mode, exception, null); }

        /// <summary>
        /// Switches Game Mode and attaches the additional Information.
        /// </summary>
        /// <param name="mode">New Level Mode</param>
        /// <param name="slots">Optional Slot Marker for <see cref="LevelModeSlots"/></param>
        private void SetMode(LevelMode mode, params byte[] slots) { SetMode(mode, null, slots); }

        /// <summary>
        /// Switches Game Mode and attaches the additional Information.
        /// </summary>
        /// <param name="mode">New Level Mode</param>
        /// <param name="exception">Optional Exception for <see cref="LastException"/></param>
        /// <param name="slots">Optional Slot Marker for <see cref="LevelModeSlots"/></param>
        private void SetMode(LevelMode mode, Exception exception, params byte[] slots)
        {
            // Replace null with empty Array.
            if (slots == null)
                slots = new byte[0];

            Mode = mode;
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
            if (Mode != LevelMode.Running)
                throw new NotSupportedException("Level is not running");

            // Make shure there is a winner
            if (slots.Length < 1)
                throw new ArgumentException("No Winner selected", nameof(slots));

            for (int i = 0; i < slots.Length; i++)
            {
                // Check for valid Slot Index
                if (slots[i] >= MaxSlots)
                    throw new ArgumentException($"Slot must be between 0 and {MaxSlots - 1}", nameof(slots));

                // Check for existing Faction
                if (Factions[slots[i]] == null)
                    throw new ArgumentException("Slot has no valid Faction");
            }

            SetMode(LevelMode.Finished, slots);
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
            if (Mode != LevelMode.Running)
                throw new NotSupportedException("Level is not running");

            // Make shure there is a winner
            if (slots.Length < 1)
                throw new ArgumentException("No Winner selected", nameof(slots));

            for (int i = 0; i < slots.Length; i++)
            {
                // Check for valid Slot Index
                if (slots[i] >= MaxSlots)
                    throw new ArgumentException($"Slot must be between 0 and {MaxSlots - 1}", nameof(slots));

                // Check for existing Faction
                if (Factions[slots[i]] == null)
                    throw new ArgumentException("Slot has no valid Faction");
            }

            SetMode(LevelMode.Failed, slots);
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
            if (Mode != LevelMode.Running)
                throw new NotSupportedException("Level is not running");

            SetMode(LevelMode.Draw);
        }

        #endregion
    }
}