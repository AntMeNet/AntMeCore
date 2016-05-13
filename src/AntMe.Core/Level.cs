using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AntMe
{
    /// <summary>
    /// Basisklasse für alle AntMe! Levels.
    /// </summary>
    public abstract class Level : PropertyList<LevelProperty>
    {
        /// <summary>
        /// Gives the maximum Number of Players per Level.
        /// </summary>
        public const byte MAX_SLOTS = 8;

        /// <summary>
        /// Anzahl Simulationsframes pro Sekunde
        /// </summary>
        public const int FRAMES_PER_SECOND = 20;

        /// <summary>
        /// Simulation Engine.
        /// </summary>
        private Engine engine = null;

        /// <summary>
        /// Cached Map State.
        /// </summary>
        private MapState mapState = null;

        /// <summary>
        /// Specialized Settings for different slots.
        /// </summary>
        private Settings[] slotSettings;

        // Last ID: 0
        private readonly Tracer tracer = new Tracer("AntMe.Level");

        /// <summary>
        /// Standard-Konstruktor.
        /// </summary>
        protected Level(SimulationContext context)
        {
            Context = new SimulationContext(context.Resolver, context.Settings);

            // Clone Settings for Slots
            slotSettings = new Settings[MAX_SLOTS];
            for (int i = 0; i < MAX_SLOTS; i++)
                slotSettings[i] = Settings.Clone();

            // Give the Level Designer the chance to change Faction/Slot Settings.
            DoSettings(Settings, slotSettings);

            Mode = LevelMode.Uninit;

            // Ermitteln des LevelDescriptionAttribute
            Type level = GetType();
            object[] levelDescriptions = level.GetCustomAttributes(typeof(LevelDescriptionAttribute), false);
            if (levelDescriptions.Length != 1)
            {
                Mode = LevelMode.InitFailed;
                throw new NotSupportedException("No Level Description found");
            }
            LevelDescription = (LevelDescriptionAttribute)levelDescriptions[0];

            // Level-Extensions
            context.Resolver.ResolveLevel(this);
        }

        /// <summary>
        /// Holds the current Simulation Context for this Level.
        /// </summary>
        public SimulationContext Context { get; private set; }

        /// <summary>
        /// Gibt die Level-Beschreibung (Instanz des Level-Attributes) zurück.
        /// </summary>
        public LevelDescriptionAttribute LevelDescription { get; private set; }

        /// <summary>
        /// Gibt die letzte Exception zurück, die aufgetreten ist.
        /// </summary>
        public Exception LastException { get; private set; }

        /// <summary>
        /// Gibt den aktuellen State zurück.
        /// </summary>
        public LevelState State { get; private set; }

        /// <summary>
        /// Gibt den zugrundeliegenden Seed für den Zufallsgenerator zurück.
        /// </summary>
        public int RandomSeed { get; private set; }

        /// <summary>
        /// Global Settings for the whole Level.
        /// </summary>
        public Settings Settings { get { return Context.Settings; } }

        /// <summary>
        /// Referenz auf die grundlegende Engine
        /// </summary>
        public Engine Engine
        {
            get { return engine; }
        }

        /// <summary>
        /// Leveleigener Zufallsgenerator. Bitte unbedingt bei der Erstellung
        /// zufällig platzierter Elemente oder ähnlichem verwenden, damit eine
        /// deterministrische Simulation sichergestellt werden kann.
        /// </summary>
        public Random Random { get { return Context.Random; } }

        /// <summary>
        /// Gibt den aktuellen Mode des Levels zurück. Hieran lässt sich
        /// ablesen, ob das Spiel noch iniziailsiert werden muss oder welcher
        /// Spieler gewonnen hat.
        /// </summary>
        public LevelMode Mode { get; private set; }

        /// <summary>
        /// Liefert eine Auflistung der Factions in einem 8-Elemente Array -
        /// zugeordnet nach SlotIndex.
        /// </summary>
        public Faction[] Factions { get; private set; }

        #region Init

        /// <summary>
        ///     Level Initialisierung. Hier wird eine Engine erstellt, die
        ///     Fraktionen gebildet und das Spiel begonnen.
        /// </summary>
        /// <exception cref="System.ArgumentOutOfRangeException">
        ///     Sollten mehr
        ///     als 8 Factions übergeben worden sein.
        /// </exception>
        /// <exception cref="System.NotSupportedException">
        ///     Tritt auf, wenn
        ///     LevelDescription und übergebene Faction-List nicht übereinstimmen
        ///     oder das Level nicht im richtigen Modus ist.
        /// </exception>
        /// <param name="slot">Liste der Spieler</param>
        public void Init(params LevelSlot[] slot)
        {
            Init(0, slot);
        }

        /// <summary>
        ///     Level Initialisierung. Hier wird eine Engine erstellt, die
        ///     Fraktionen gebildet und das Spiel begonnen.
        /// </summary>
        /// <exception cref="System.ArgumentOutOfRangeException">
        ///     Sollten mehr
        ///     als 8 Factions übergeben worden sein.
        /// </exception>
        /// <exception cref="System.NotSupportedException">
        ///     Tritt auf, wenn
        ///     LevelDescription und übergebene Faction-List nicht übereinstimmen
        ///     oder das Level nicht im richtigen Modus ist.
        /// </exception>
        /// <param name="randomSeed">Initialwert des Zufallsgenerators. 0 = Random</param>
        /// <param name="slots">Liste der involvierten Factions</param>
        public void Init(int randomSeed, params LevelSlot[] slots)
        {
            // Level muss Uninit sein
            if (Mode != LevelMode.Uninit)
                throw new NotSupportedException("Level is not ready for init");

            // Zufallsgenerator initialisieren
            RandomSeed = (int)DateTime.Now.Ticks;
            if (randomSeed != 0)
                RandomSeed = randomSeed;

            // Recreates the Simulation Context including Randomizer.
            Context = new SimulationContext(
                Context.Resolver, 
                Context.Settings,
                new Random(RandomSeed));

            // Parameter checken
            if (slots.Length > MAX_SLOTS)
            {
                Mode = LevelMode.InitFailed;
                throw new ArgumentOutOfRangeException();
            }

            engine = new Engine(Context.Resolver);

            // Erstelle Karte und prüfe Karte auf Korrektheit
            Map map = LevelDescription.Map;
            if (map == null)
            {
                Mode = LevelMode.InitFailed;
                throw new NotSupportedException("No Map was created");
            }
            map.CheckMap();

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

            // Doppelte Farben prüfen
            var colors = new List<PlayerColor>();
            foreach (var slot in slots)
            {
                if (slot != null)
                {
                    if (colors.Contains(slot.Color))
                    {
                        Mode = LevelMode.InitFailed;
                        throw new NotSupportedException("There are two Players with the same color");
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
                Mode = LevelMode.InitFailed;
                throw new NotSupportedException(string.Format("Too less player. Requires {0} Player", minPlayer));
            }

            if (playerCount > maxPlayer)
            {
                Mode = LevelMode.InitFailed;
                throw new NotSupportedException(string.Format("Too many player. Requires a Maximum of {0} Player",
                    maxPlayer));
            }

            if (highestSlot > map.GetPlayerCount())
            {
                Mode = LevelMode.InitFailed;
                throw new NotSupportedException(string.Format("Too many Slots used. Map has only {0} Slots",
                    map.GetPlayerCount()));
            }

            // Factions erzeugen
            Factions = new Faction[MAX_SLOTS];
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i] == null)
                    continue;

                SimulationContext factionContext = new SimulationContext(Context.Resolver, slotSettings[i]);

                // Identify Faction
                Factions[i] = Context.Resolver.CreateFaction(factionContext, slots[i].FactoryType, this);

                // Falls Faction nicht gefunden werden konnte
                if (Factions[i] == null)
                    throw new Exception(string.Format("Cound not identify Faction for player {0}.",
                        slots[i].Name));

            }

            // State erzeugen
            mapState = new MapState();
            mapState.BlockBorder = map.BlockBorder;
            mapState.Tiles = (MapTile[,])map.Tiles.Clone();

            engine.Init(map);
            engine.OnInsertItem += engine_OnInsertItem;
            engine.OnRemoveItem += engine_OnRemoveItem;

            // Fraktionen ins Spiel einbetten
            for (byte i = 0; i < Factions.Length; i++)
            {
                try
                {
                    InitFaction(i, slots[i], Factions[i]);
                }
                catch (Exception)
                {
                    switch (i)
                    {
                        case 0:
                            Mode = LevelMode.FailedPlayer1;
                            break;
                        case 1:
                            Mode = LevelMode.FailedPlayer2;
                            break;
                        case 2:
                            Mode = LevelMode.FailedPlayer3;
                            break;
                        case 3:
                            Mode = LevelMode.FailedPlayer4;
                            break;
                        case 4:
                            Mode = LevelMode.FailedPlayer5;
                            break;
                        case 5:
                            Mode = LevelMode.FailedPlayer6;
                            break;
                        case 6:
                            Mode = LevelMode.FailedPlayer7;
                            break;
                        case 7:
                            Mode = LevelMode.FailedPlayer8;
                            break;
                    }
                    Factions = null;
                    throw;
                }
            }

            // Initialisierung des Levels
            try
            {
                OnInit();
            }
            catch (Exception)
            {
                Mode = LevelMode.InitFailed;
                Factions = null;
                throw;
            }

            // Mode wechseln, damit OnInit() bereits berechnungen durchführen kann.
            Mode = LevelMode.Running;
        }

        private void engine_OnInsertItem(Item item)
        {
            // Sicher stellen, dass nur Items verarbeitet werden
            if (!(item is Item))
                throw new NotSupportedException("Item of a wrong type was inserted");

            OnInsertItem(item);
        }

        private void engine_OnRemoveItem(Item item)
        {
            // Sicher stellen, dass nur Items verarbeitet werden
            if (!(item is Item))
                throw new NotSupportedException("Item of a wrong type was deleted");

            var Item = item as Item;

            OnRemoveItem(Item);
        }

        /// <summary>
        ///     Initialisiert die systemeigenen Fraktionstypen (Ameisen, Wanzen, Viren)
        /// </summary>
        /// <param name="slotIndex"></param>
        /// <param name="slot"></param>
        /// <param name="faction"></param>
        private void InitFaction(byte slotIndex, LevelSlot slot, Faction faction)
        {
            if (faction == null)
                return;

            faction.Init(
                slotIndex,
                slot.Team,
                slot.Name,
                slot.Color,
                new Random(RandomSeed + slotIndex + 1),
                new Vector2(
                (engine.Map.StartPoints[slotIndex].X + 0.5f) * Map.CELLSIZE,
                (engine.Map.StartPoints[slotIndex].Y + 0.5f) * Map.CELLSIZE));
        }

        #endregion

        /// <summary>
        /// Gives the Level Designer the chance to change Settings.
        /// </summary>
        protected virtual void DoSettings(Settings levelSettings, Settings[] slotSettings)
        {
        }

        /// <summary>
        ///     Wird vom System aufgerufen, um das Level zu initialisieren. Das
        ///     hier kann verwendet werden, um Listen, Trigger und Caches zu
        ///     initialisieren.
        ///     - Trigger registrieren
        ///     - Start Einheiten erzeugen
        /// </summary>
        protected virtual void OnInit()
        {
        }

        /// <summary>
        ///     Wird vom System vor dem Engine-Update in jeder Simulationsrunde aufgerufen. An dieser
        ///     Stelle kann die Level Logik reagieren.
        /// </summary>
        protected virtual void OnUpdate()
        {
        }

        /// <summary>
        /// Wird aufgerufen, wenn ein neues Item in das Level eingefügt wird.
        /// </summary>
        /// <param name="item"></param>
        protected virtual void OnInsertItem(Item item)
        {
        }

        /// <summary>
        ///     Wird vom System aufgerufen, wenn Elemente aus der Simulation
        ///     entfernt wurden.
        /// </summary>
        /// <param name="item">Entferntes Element</param>
        protected virtual void OnRemoveItem(Item item)
        {
        }

        /// <summary>
        ///     Führt die nächste Simulationsrunde durch und liefert anschließend
        ///     den neusten State der Simulation. Sollte vom Controller des Levels
        ///     aufgerufen werden.
        /// </summary>
        /// <returns>Aktueller State</returns>
        public LevelState NextState()
        {
            // Alle Pre-Running States
            if ((int)Mode < 10)
                throw new NotSupportedException("Level is not running yet");

            if (Mode == LevelMode.Running)
            {
                // TODO: try/catch für Programmierfehler
                // TODO: Watchdog für Laufzeit-Überschreitung
                engine.Update();

                // Updates der Factions
                for (int i = 0; i < MAX_SLOTS; i++)
                {
                    if (Factions[i] != null)
                    {
                        Factions[i].Update(engine.Round);
                    }
                }

                // Level- und Trigger Update
                try
                {
                    OnUpdate();

                    foreach (var property in Properties)
                        property.OnUpdate();
                }
                catch (Exception ex)
                {
                    Mode = LevelMode.FailedSystem;
                    LastException = ex;
                }
            }

            // Create a new Instance of State
            if (State == null)
            {
                State = Context.Resolver.CreateLevelState(this);
                State.Map = mapState;

                // Collect all Faction States
                for (int i = 0; i < MAX_SLOTS; i++)
                {
                    if (Factions[i] != null)
                    {
                        State.Factions.Add(Factions[i].GetFactionState());
                    }
                }
            }

            State.Round = engine.Round;
            State.Mode = Mode;

            // TODO: Property States sammeln
            // Fügt die Screenhighlights ein
            //while (screenHighlights.Count > 0)
            //    state.ScreenHighlights.Add(screenHighlights.Dequeue());

            // Remove old Items
            foreach (var item in State.Items.ToArray())
            {
                if (!engine.Items.Any(i => i.Id == item.Id))
                    State.Items.Remove(item);
            }

            // Insert new Items
            foreach (Item item in engine.Items)
            {
                ItemState itemState = item.GetState();
                if (!State.Items.Contains(itemState))
                    State.Items.Add(itemState);
            }

            return State;
        }

        #region Story Telling

        /// <summary>
        ///     Beendet das Spiel mit dem angegebenen Sieger.
        /// </summary>
        /// <param name="slotIndex">Sieger</param>
        protected void Finish(int slotIndex)
        {
            // Only allowed in running mode
            if (Mode != LevelMode.Running)
                throw new NotSupportedException("Level is not running");

            // Prüfen, ob Slot gültig ist
            if (slotIndex < 0 && slotIndex >= MAX_SLOTS)
                throw new ArgumentOutOfRangeException("slotIndex", "SlotIndex must be between 0 and 7");

            // Prüfen, ob der entsprechende Player auch existiert
            if (Factions[slotIndex] == null)
                throw new ArgumentException("SlotIndex does not exist");

            // State setzen
            switch (slotIndex)
            {
                case 0:
                    Mode = LevelMode.FinishedPlayer1;
                    break;
                case 1:
                    Mode = LevelMode.FinishedPlayer2;
                    break;
                case 2:
                    Mode = LevelMode.FinishedPlayer3;
                    break;
                case 3:
                    Mode = LevelMode.FinishedPlayer4;
                    break;
                case 4:
                    Mode = LevelMode.FinishedPlayer5;
                    break;
                case 5:
                    Mode = LevelMode.FinishedPlayer6;
                    break;
                case 6:
                    Mode = LevelMode.FinishedPlayer7;
                    break;
                case 7:
                    Mode = LevelMode.FinishedPlayer8;
                    break;
            }
        }

        /// <summary>
        ///     Beendet das Spiel mit dem angegebenen Verlierer.
        /// </summary>
        /// <param name="slotIndex">Verlierer</param>
        protected void Fail(int slotIndex)
        {
            // Only allowed in running mode
            if (Mode != LevelMode.Running)
                throw new NotSupportedException("Level is not running");

            // Prüfen, ob Slot gültig ist
            if (slotIndex < 0 && slotIndex >= MAX_SLOTS)
                throw new ArgumentOutOfRangeException("slotIndex", "SlotIndex must be between 0 and 7");

            // Prüfen, ob der entsprechende Player auch existiert
            if (Factions[slotIndex] == null)
                throw new ArgumentException("SlotIndex does not exist");

            switch (slotIndex)
            {
                case 0:
                    Mode = LevelMode.FailedPlayer1;
                    break;
                case 1:
                    Mode = LevelMode.FailedPlayer2;
                    break;
                case 2:
                    Mode = LevelMode.FailedPlayer3;
                    break;
                case 3:
                    Mode = LevelMode.FailedPlayer4;
                    break;
                case 4:
                    Mode = LevelMode.FailedPlayer5;
                    break;
                case 5:
                    Mode = LevelMode.FailedPlayer6;
                    break;
                case 6:
                    Mode = LevelMode.FailedPlayer7;
                    break;
                case 7:
                    Mode = LevelMode.FailedPlayer8;
                    break;
            }
        }

        /// <summary>
        ///     Beendet das Spiel mit einem untentschieden.
        /// </summary>
        protected void Draw()
        {
            // Only allowed in running mode
            if (Mode != LevelMode.Running)
                throw new NotSupportedException("Level is not running");

            Mode = LevelMode.Draw;
        }

        #endregion
    }
}