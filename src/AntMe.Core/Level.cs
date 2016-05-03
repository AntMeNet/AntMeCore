using System;
using System.Collections.Generic;
using System.Linq;


namespace AntMe
{
    /// <summary>
    /// Basisklasse für alle AntMe! Levels.
    /// </summary>
    public abstract class Level : PropertyList<LevelProperty>
    {
        /// <summary>
        /// Referenz auf den Type Resolver.
        /// </summary>
        protected readonly ITypeResolver Resolver;

        /// <summary>
        /// Anzahl Simulationsframes pro Sekunde
        /// </summary>
        public const int FRAMES_PER_SECOND = 20;

        private Engine engine = null;
        private MapState mapState = null;
        private Settings settings = null;

        // Last ID: 0
        private readonly Tracer tracer = new Tracer("AntMe.Level");

        /// <summary>
        /// Standard-Konstruktor.
        /// </summary>
        /// <param name="resolver">Referenz auf den Type Resolver.</param>
        protected Level(ITypeResolver resolver)
        {
            Resolver = resolver;
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

            // TODO: Level-Extensions
        }

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
        protected Random Random { get; private set; }

        /// <summary>
        /// Gibt den aktuellen Mode des Levels zurück. Hieran lässt sich
        /// ablesen, ob das Spiel noch iniziailsiert werden muss oder welcher
        /// Spieler gewonnen hat.
        /// </summary>
        public LevelMode Mode { get; private set; }

        /// <summary>
        /// Liefert eine Auflistung der Factions in einem 8-Elemente Array -
        /// zugeordnet nach PlayerIndex.
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
        /// <param name="resolver">Instanz des aktuellen Type Resolvers</param>
        /// <param name="settings">Die aktuellen Settings</param>
        /// <param name="factions">Liste der involvierten Factions</param>
        public void Init(ITypeResolver resolver, Settings settings, params Faction[] factions)
        {
            Init(resolver, settings, 0, factions);
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
        /// <param name="resolver">Instanz des aktuellen Type Resolvers</param>
        /// <param name="settings">Aktuelle Settings</param>
        /// <param name="randomSeed">Initialwert des Zufallsgenerators. 0 = Random</param>
        /// <param name="factions">Liste der involvierten Factions</param>
        public void Init(ITypeResolver resolver, Settings settings, int randomSeed, params Faction[] factions)
        {
            // Level muss Uninit sein
            if (Mode != LevelMode.Uninit)
                throw new NotSupportedException("Level is not ready for init");

            // Zufallsgenerator initialisieren
            RandomSeed = (int)DateTime.Now.Ticks;
            if (randomSeed != 0)
                RandomSeed = randomSeed;
            Random = new Random(RandomSeed);

            // Parameter checken
            if (factions.Length > 8)
            {
                Mode = LevelMode.InitFailed;
                throw new ArgumentOutOfRangeException();
            }

            engine = new Engine(resolver);

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

            // Ermitteln der Filter Attribute
            object[] levelFilters = GetType().GetCustomAttributes(typeof(FactionFilterAttribute), false);
            var filters = new Dictionary<int, List<FactionFilterAttribute>>();
            foreach (FactionFilterAttribute item in levelFilters)
            {
                if (!filters.ContainsKey(item.PlayerIndex))
                    filters.Add(item.PlayerIndex, new List<FactionFilterAttribute>());
                filters[item.PlayerIndex].Add(item);
            }

            // Doppelte Farben prüfen
            var colors = new List<PlayerColor>();
            foreach (Faction faction in factions)
            {
                if (faction != null)
                {
                    if (colors.Contains(faction.PlayerColor))
                    {
                        Mode = LevelMode.InitFailed;
                        throw new NotSupportedException("There are two Players with the same color");
                    }
                    colors.Add(faction.PlayerColor);
                }
            }

            // TODO: Factions auf vollständigkeit Prüfen

            // Gegencheck mit Level-Attributen
            Factions = new Faction[8];
            int playerCount = 0;
            int highestSlot = 0;
            for (int i = 0; i < factions.Length; i++)
            {
                Factions[i] = factions[i];
                if (factions[i] != null)
                {
                    // Counter
                    playerCount++;
                    highestSlot = i;

                    // Filter
                    if (filters.ContainsKey(i) && filters[i].Count > 0)
                    {
                        if (filters[i].Any(item => item.FactionType == factions[i].GetType()))
                            continue;

                        Mode = LevelMode.InitFailed;
                        Factions = null;
                        throw new NotSupportedException(string.Format(
                            "Faction '{0}' is not allowed in Slot {1}",
                            factions[i].GetType(), i));
                    }
                }
            }

            // Faction Counts mit Map- und Level-Requirements gegenchecken
            if (playerCount < minPlayer)
            {
                Mode = LevelMode.InitFailed;
                Factions = null;
                throw new NotSupportedException(string.Format("Too less player. Requires {0} Player", minPlayer));
            }

            if (playerCount > maxPlayer)
            {
                Mode = LevelMode.InitFailed;
                Factions = null;
                throw new NotSupportedException(string.Format("Too many player. Requires a Maximum of {0} Player",
                    maxPlayer));
            }

            if (highestSlot > map.GetPlayerCount())
            {
                Mode = LevelMode.InitFailed;
                Factions = null;
                throw new NotSupportedException(string.Format("Too many Slots used. Map has only {0} Slots",
                    map.GetPlayerCount()));
            }

            try
            {
                DoSettings();
            }
            catch (Exception)
            {
                Factions = null;
                Mode = LevelMode.InitFailed;
                throw;
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
                    InitFaction(i, Factions[i]);
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
        /// <param name="index"></param>
        /// <param name="faction"></param>
        private void InitFaction(byte index, Faction faction)
        {
            if (faction == null)
                return;

            faction.PlayerIndex = index;
            faction.Level = this;
            faction.Home = new Vector2(
                (engine.Map.StartPoints[index].X + 0.5f) * Map.CELLSIZE,
                (engine.Map.StartPoints[index].Y + 0.5f) * Map.CELLSIZE);
            faction.Random = new Random(RandomSeed + index + 1);
            faction.Init();
        }

        #endregion

        /// <summary>
        ///     Wird vom System aufgerufen, bevor die Settings an die Engine und an
        ///     die Fraktionen weitergegeben werden. Map, Engine und Factions
        ///     existieren, sind aber uninitialisiert.
        ///     - Level Settings anpassen
        ///     - Faction Settings anpassen
        ///     - Engine Extensions registrieren
        /// </summary>
        protected virtual void DoSettings()
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
                for (int i = 0; i < 8; i++)
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

            var state = new LevelState(mapState, engine.Round, Mode);

            // TODO: Property States sammeln
            // Fügt die Screenhighlights ein
            //while (screenHighlights.Count > 0)
            //    state.ScreenHighlights.Add(screenHighlights.Dequeue());

            // Durchläuft alle Fraktionen zur Ermittlung des States
            for (int i = 0; i < 8; i++)
            {
                if (Factions[i] != null)
                {
                    state.Factions.Add(Factions[i].GetFactionState());
                }
            }

            // Durchläuft alle Items zur Ermittlung der States
            foreach (Item item in engine.Items)
                state.Items.Add(item.GetState());

            return State = state;
        }

        #region Story Telling

        /// <summary>
        ///     Beendet das Spiel mit dem angegebenen Sieger.
        /// </summary>
        /// <param name="playerIndex">Sieger</param>
        protected void Finish(int playerIndex)
        {
            // Only allowed in running mode
            if (Mode != LevelMode.Running)
                throw new NotSupportedException("Level is not running");

            // Prüfen, ob Playerindex gültig ist
            if (playerIndex < 0 && playerIndex >= 8)
                throw new ArgumentOutOfRangeException("playerIndex", "PlayerIndex must be between 0 and 7");

            // Prüfen, ob der entsprechende Player auch existiert
            if (Factions[playerIndex] == null)
                throw new ArgumentException("PlayerIndex does not exist");

            // State setzen
            switch (playerIndex)
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
        /// <param name="playerIndex">Verlierer</param>
        protected void Fail(int playerIndex)
        {
            // Only allowed in running mode
            if (Mode != LevelMode.Running)
                throw new NotSupportedException("Level is not running");

            // Prüfen, ob Playerindex gültig ist
            if (playerIndex < 0 && playerIndex >= 8)
                throw new ArgumentOutOfRangeException("playerIndex", "PlayerIndex must be between 0 and 7");

            // Prüfen, ob der entsprechende Player auch existiert
            if (Factions[playerIndex] == null)
                throw new ArgumentException("PlayerIndex does not exist");

            switch (playerIndex)
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