using AntMe.Basics.EngineProperties;
using AntMe.Basics.FactionProperties;
using AntMe.Basics.Factions;
using AntMe.Basics.Factions.Ants;
using AntMe.Basics.Factions.Ants.Interop;
using AntMe.Basics.Factions.Bugs;
using AntMe.Basics.ItemProperties;
using AntMe.Basics.Items;
using AntMe.Basics.LevelProperties;
using AntMe.Basics.MapProperties;
using AntMe.Basics.MapTileProperties;
using AntMe.Basics.MapTiles;
using System;

namespace AntMe.Basics
{
    /// <summary>
    /// Extension for the Basic AntMe! Items.
    /// </summary>
    public sealed class Extension : IExtensionPack
    {
        /// <summary>
        /// Name of the Author.
        /// </summary>
        public string Author { get { return "Tom Wendel @ AntMe! GmbH"; } }

        /// <summary>
        /// Short description of this Extension.
        /// </summary>
        public string Description { get { return "Basic Extension with all the Core AntMe! Elements."; } }

        /// <summary>
        /// Name of this Extension.
        /// </summary>
        public string Name { get { return "AntMe! Basics"; } }

        /// <summary>
        /// Extension Version.
        /// </summary>
        public Version Version { get { return new Version(2, 0); } }

        /// <summary>
        /// Startup Delegate to register all Types and Settings.
        /// </summary>
        /// <param name="typeMapper">Reference to the Type Mapper</param>
        /// <param name="settings">Reference to the Extension Settings</param>
        public void Load(ITypeMapper typeMapper, KeyValueStore settings)
        {
            // ##########################
            // Standard Engine Extensions
            // ##########################
            typeMapper.RegisterEngineProperty<InteractionProperty>(this, "Interaction Extension (Core)", 70);
            typeMapper.RegisterEngineProperty<PhysicsProperty>(this, "Physics Extension (Core)", 100);

            settings.Set<RecognitionProperty>("SmellsAliance", false, "Can a Unit smell Smellable Stuff from Aliance Units");
            settings.Set<RecognitionProperty>("SmellsForeign", false, "Can a Unit smell Smellable Stuff from Enemy Units");
            typeMapper.RegisterEngineProperty<RecognitionProperty>(this, "Recognition Extension (Core)", 150);

            // ##########################
            // Map Materials
            // ##########################
            typeMapper.RegisterMapMaterial<LavaMaterial>(this, "Lava Material");
            typeMapper.RegisterMapMaterial<MudMaterial>(this, "Mud Material");
            typeMapper.RegisterMapMaterial<SandMaterial>(this, "Sand Material");
            typeMapper.RegisterMapMaterial<GrasMaterial>(this, "Gras Material");
            typeMapper.RegisterMapMaterial<StoneMaterial>(this, "Stone Material");

            // ##########################
            // Map Properties
            // ##########################
            typeMapper.RegisterMapProperty<TileUpdaterProperty>(this, "Updateable Tiles");

            // ##########################
            // Map Tiles
            // ##########################
            typeMapper.RegisterMapTile<ConcaveCliffMapTile, ConcaveCliffMapTileState, ConcaveCliffMapTileInfo>(this, "Concave Cliff Map Tile");
            typeMapper.RegisterMapTile<ConvexCliffMapTile, ConvexCliffMapTileState, ConvexCliffMapTileInfo>(this, "Convex Cliff Map Tile");
            typeMapper.RegisterMapTile<WallCliffMapTile, WallCliffMapTileState, WallCliffMapTileInfo>(this, "Cliff Wall Map Tile");
            typeMapper.RegisterMapTile<LeftRampToCliffMapTile, LeftRampToCliffMapTileState, WallCliffMapTileInfo>(this, "Left Ramp To Cliff Wall Map Tile");
            typeMapper.RegisterMapTile<RightRampToCliffMapTile, RightRampToCliffMapTileState, WallCliffMapTileInfo>(this, "Right Ramp To Cliff Wall Map Tile");
            typeMapper.RegisterMapTile<FlatMapTile, FlatMapTileState, FlatMapTileInfo>(this, "Flat Map Tile");
            typeMapper.RegisterMapTile<RampMapTile, RampMapTileState, RampMapTileInfo>(this, "Ramp Map Tile");

            // ##########################
            // Map Tile Properties
            // ##########################
            typeMapper.RegisterMapTilePropertySI<WalkableTileProperty, WalkableTileStateProperty, WalkableTileInfoProperty>(this, "Walkable Map Tiles");

            // ##########################
            // Attach Tile Properties
            // ##########################
            typeMapper.AttachMapTileProperty<FlatMapTile, WalkableTileProperty>(this, "Walkable Flat Tiles");
            typeMapper.AttachMapTileProperty<RampMapTile, WalkableTileProperty>(this, "Walkable Ramps");

            // ##########################
            // Standard Item Properties
            // ##########################
            typeMapper.RegisterItemPropertySI<AttackableProperty, AttackableState, AttackableInfo>(this, "Attackable");
            typeMapper.RegisterItemPropertySI<AttackerProperty, AttackerState, AttackerInfo>(this, "Attacker");
            typeMapper.RegisterItemPropertyS<CarrierProperty, CarrierState>(this, "Carrier");
            typeMapper.RegisterItemPropertySI<CollidableProperty, CollidableState, CollidableInfo>(this, "Collidable");
            typeMapper.RegisterItemPropertyS<PortableProperty, PortableState>(this, "Portable");
            typeMapper.RegisterItemPropertyS<SightingProperty, SightingState>(this, "Sighting"); // keine Info
            typeMapper.RegisterItemPropertyS<SmellableProperty, SmellableState>(this, "Smellable"); // keine Info
            typeMapper.RegisterItemPropertyS<SnifferProperty, SnifferState>(this, "Sniffer"); // keine Info
            typeMapper.RegisterItemPropertyS<VisibleProperty, VisibleState>(this, "Visible"); // keine Info
            typeMapper.RegisterItemPropertyS<WalkingProperty, WalkingState>(this, "Walking");
            typeMapper.RegisterItemPropertyS<AppleCollectorProperty, AppleCollectorState>(this, "Apple Collector"); // keine Info
            typeMapper.RegisterItemPropertyS<SugarCollectorProperty, SugarCollectorState>(this, "Sugar Collector"); // keine Info
            typeMapper.RegisterItemPropertyS<AppleCollectableProperty, AppleCollectableState>(this, "Apple Collectable"); // keine Info
            typeMapper.RegisterItemPropertyS<SugarCollectableProperty, SugarCollectableState>(this, "Sugar Collectable"); // keine Info

            // ##########################
            // Factions registrieren
            // ##########################

            // Faction Properties
            typeMapper.RegisterFactionPropertyS<PointsProperty, PointsState>(this, "Faction Points Property");
            typeMapper.RegisterFactionProperty<AntDeathCounterProperty>(this, "Death Ant Counter");
            typeMapper.RegisterFactionProperty<AnthillDeathCounterProperty>(this, "Death Anthill Counter");

            // Ant Faction
            settings.Set<AntFaction>("InitialAnthillCount", 1, "Number of initial Anthills");
            settings.Set<AntFaction>("InitialAntCount", 0, "Number of initial Ants");
            settings.Set<AntFaction>("ConcurrentAntCount", 100, "Number of concurrent Ants");
            settings.Set<AntFaction>("ConcurrentAnthillCount", 1, "Number of concurrent Anthills");
            settings.Set<AntFaction>("TotalAntCount", int.MaxValue, "Total Number of Ants per Simulation");
            settings.Set<AntFaction>("TotalAnthillCount", 1, "Number of total Anthills per Simulation");
            settings.Set<AntFaction>("AntRespawnDelay", 1, "Number of Rounds until another Respawn");
            typeMapper.RegisterFaction<AntFaction, AntFactionState, FactionInfo, AntFactory, AntFactoryInterop, AntUnit, AntUnitInterop, AntItem>(this, "Ants");

            // Ant Factory Interops
            typeMapper.AttachFactoryInteropProperty<AntFactoryInterop, TotalStatisticsInterop>(this, "Ant Total Statistics", (f, i) =>
            {
                return new TotalStatisticsInterop(f, i, typeof(AntItem));
            });
            typeMapper.AttachFactoryInteropProperty<AntFactoryInterop, ByTypeStatisticsInterop>(this, "Ant By Type Statistics", (f, i) =>
            {
                return new ByTypeStatisticsInterop(f, i, typeof(AntItem));
            });
            typeMapper.AttachFactoryInteropProperty<AntFactoryInterop, ByCasteStatisticsInterop>(this, "Ant By Caste Statistics", (f, i) =>
            {
                return new ByCasteStatisticsInterop(f, i, typeof(AntItem));
            });

            // Ant Unit Interops
            typeMapper.AttachUnitInteropProperty<AntUnitInterop, AntMovementInterop>(this, "Ant Movement Interop");
            typeMapper.AttachUnitInteropProperty<AntUnitInterop, RecognitionInterop>(this, "Ant Recognition Interop");
            typeMapper.AttachUnitInteropProperty<AntUnitInterop, InteractionInterop>(this, "Ant Interaction Interop");

            // Bug Faction
            typeMapper.RegisterFaction<BugFaction, BugFactionState, FactionInfo, BugFactory, BugFactoryInterop, BugUnit, BugUnitInterop, BugItem>(this, "Bugs");

            // Bug Factory Interops
            typeMapper.AttachFactoryInteropProperty<BugFactoryInterop, TotalStatisticsInterop>(this, "Bug Total Statistics", (f, i) =>
            {
                return new TotalStatisticsInterop(f, i, typeof(BugItem));
            });
            typeMapper.AttachFactoryInteropProperty<BugFactoryInterop, ByTypeStatisticsInterop>(this, "Bug By Type Statistics", (f, i) =>
            {
                return new ByTypeStatisticsInterop(f, i, typeof(BugItem));
            });

            // Faction Extensions
            typeMapper.AttachFactionProperty<Faction, PointsProperty>(this, "Faction Points");
            typeMapper.AttachFactionProperty<AntFaction, AntDeathCounterProperty>(this, "Counter for dead Ants");
            typeMapper.AttachFactionProperty<AntFaction, AnthillDeathCounterProperty>(this, "Counter for destroyed Anthills");

            // Faction Attatchments


            // ##########################
            // Game Items registrieren
            // ##########################
            RegisterApple(typeMapper, settings);
            RegisterSugar(typeMapper, settings);
            RegisterAnthill(typeMapper, settings);
            RegisterMarker(typeMapper, settings);
            RegisterAnt(typeMapper, settings);
            RegisterBug(typeMapper, settings);
            RegisterClassicBug(typeMapper, settings);

            // ##############################
            // Level Properties registrieren
            // ##############################

            typeMapper.RegisterLevelProperty<TriggerLevelProperty>(this, "Level Trigger");
            typeMapper.RegisterLevelProperty<DialogHighlightsLevelProperty, DialogHighlightsStateProperty>(this, "Dialog Highlights");
            typeMapper.RegisterLevelProperty<MapMarkerHighlightsLevelProperty, MapMarkerHighlightsStateProperty>(this, "Map Marker Highlights");
            typeMapper.RegisterLevelProperty<NotificationHighlightsLevelProperty, NotificationHighlightsStateProperty>(this, "Notification Highlights");
        }

        /// <summary>
        /// Registers Apples
        /// </summary>
        /// <param name="typeMapper">Type Mapper</param>
        /// <param name="settings">Settings</param>
        private void RegisterApple(ITypeMapper typeMapper, KeyValueStore settings)
        {
            // Apple
            typeMapper.RegisterItem<AppleItem, AppleState, AppleInfo>(this, "Apple");

            // Collidable
            settings.Set<AppleItem>("Mass", 200f, "Mass of an Apple");
            typeMapper.AttachItemProperty<AppleItem, CollidableProperty>(this, "Apple Collidable", (i) =>
            {
                CollidableProperty property = new CollidableProperty(i);

                // Define Radius
                property.CollisionRadius = AppleItem.AppleInnerRadius;

                // Define Mass
                property.CollisionFixed = false;
                property.CollisionMass = i.Settings.GetFloat<AppleItem>("Mass").Value;

                return property;
            });

            // Visibility
            typeMapper.AttachItemProperty<AppleItem, VisibleProperty>(this, "Apple Visible", (i) =>
            {
                VisibleProperty property = new VisibleProperty(i);

                // Bind Visibility Radius to the Item Radius
                property.VisibilityRadius = i.Radius;
                i.RadiusChanged += (item, v) => { property.VisibilityRadius = v; };

                return property;
            });

            // Portable
            settings.Set<AppleItem>("Weight", 200f, "Weight of an Apple");
            typeMapper.AttachItemProperty<AppleItem, PortableProperty>(this, "Apple Portable", (i) =>
            {
                PortableProperty property = new PortableProperty(i);

                // Set Weight
                property.PortableWeight = settings.GetFloat<AppleItem>("Weight").Value;

                // Bind Portable Radius to the Item Radius
                property.PortableRadius = i.Radius;
                i.RadiusChanged += (item, v) => { property.PortableRadius = v; };

                return property;
            });

            settings.Set<AppleItem>("Collectable", false, "Will an Apple be collectable");
            settings.Set<AppleItem>("Amount", 250, "Amount of Apple Units");
            typeMapper.AttachItemProperty<AppleItem, AppleCollectableProperty>(this, "Apple Collectable", (i) =>
            {
                if (!i.Settings.GetBool<AppleItem>("Collectable").Value)
                    return null;

                AppleCollectableProperty property = new AppleCollectableProperty(i);
                property.Capacity = i.Settings.GetInt<AppleItem>("Amount").Value;
                property.Amount = i.Settings.GetInt<AppleItem>("Amount").Value;
                return property;
            });
        }

        /// <summary>
        /// Registers Sugar
        /// </summary>
        /// <param name="typeMapper">Type Mapper</param>
        /// <param name="settings">Settings</param>
        private void RegisterSugar(ITypeMapper typeMapper, KeyValueStore settings)
        {
            // Sugar
            typeMapper.RegisterItem<SugarItem, SugarState, SugarInfo>(this, "Sugar");

            // Collidable
            typeMapper.AttachItemProperty<SugarItem, CollidableProperty>(this, "Sugar Collidable", (i) =>
            {
                CollidableProperty property = new CollidableProperty(i);

                // Define mass (Fixed)
                property.CollisionFixed = true;
                property.CollisionMass = 0f;

                // Bind Collision Radius to the Item Radius
                property.CollisionRadius = i.Radius;
                i.RadiusChanged += (item, v) => { property.CollisionRadius = v; };

                return property;
            });

            // Visibility
            typeMapper.AttachItemProperty<SugarItem, VisibleProperty>(this, "Sugar Visible", (i) =>
            {
                VisibleProperty property = new VisibleProperty(i);

                // Bind Visibility Radius to the Item Radius
                property.VisibilityRadius = i.Radius;
                i.RadiusChanged += (item, v) => { property.VisibilityRadius = v; };

                return property;
            });

            // Collectable
            typeMapper.AttachItemProperty<SugarItem, SugarCollectableProperty>(this, "Sugar Collectable"); // TODO: Amounts (SugarMaxCapacity, Math.Min(SugarMaxCapacity, amount))
        }

        /// <summary>
        /// Registers Anthills
        /// </summary>
        /// <param name="typeMapper">Type Mapper</param>
        /// <param name="settings">Settings</param>
        private void RegisterAnthill(ITypeMapper typeMapper, KeyValueStore settings)
        {
            // Anthill
            typeMapper.RegisterItem<AnthillItem, AnthillState, AnthillInfo>(this, "Anthill");

            // Collision
            typeMapper.AttachItemProperty<AnthillItem, CollidableProperty>(this, "Anthill Collidable", (i) =>
            {
                CollidableProperty property = new CollidableProperty(i);

                // Set Collision Mass
                property.CollisionFixed = true;
                property.CollisionMass = 0f;

                // Bind Radius to the Item Radius
                property.CollisionRadius = i.Radius;
                i.RadiusChanged += (item, v) => { property.CollisionRadius = v; };

                return property;
            });

            // Visibility
            typeMapper.AttachItemProperty<AnthillItem, VisibleProperty>(this, "Anthill Visible", (i) =>
            {
                VisibleProperty property = new VisibleProperty(i);

                // Bind Visibility Radius to the Item Radius
                property.VisibilityRadius = i.Radius;
                i.RadiusChanged += (item, v) => { property.VisibilityRadius = v; };

                return property;
            });

            // Attackable
            settings.Set<AnthillItem>("Attackable", false, "Enables the possibility to destroy Anthills");
            settings.Set<AnthillItem>("MaxHealth", 1000, "Maximum Health of an Anthill");
            settings.Set<AnthillItem>("Buildable", false, "Can an Anthill build by ants");
            typeMapper.AttachItemProperty<AnthillItem, AttackableProperty>(this, "Anthill Attackable", (i) =>
            {
                // Check Attackable Switch
                if (!i.Settings.GetBool<AnthillItem>("Attackable").Value)
                    return null;

                AttackableProperty property = new AttackableProperty(i);

                // Bind Attackable Radius to Item Radius
                property.AttackableRadius = i.Radius;
                i.RadiusChanged += (item, v) => { property.AttackableRadius = v; };

                // Health
                property.AttackableMaximumHealth = settings.GetInt<AnthillItem>("MaxHealth").Value;
                property.AttackableHealth = settings.GetInt<AnthillItem>("MaxHealth").Value;

                return property;
            });

            // Collectable
            typeMapper.AttachItemProperty<AnthillItem, SugarCollectableProperty>(this, "Anthill Sugarsafe"); // TODO: Radius
            typeMapper.AttachItemProperty<AnthillItem, AppleCollectableProperty>(this, "Anthill Applesafe"); // TODO: Radius
        }

        /// <summary>
        /// Registers Marker
        /// </summary>
        /// <param name="typeMapper">Type Mapper</param>
        /// <param name="settings">Settings</param>
        private void RegisterMarker(ITypeMapper typeMapper, KeyValueStore settings)
        {
            // Marker
            settings.Set<MarkerItem>("MinRadius", 20f, "Minimum Radius of a Marker");
            settings.Set<MarkerItem>("MaxRadius", 200f, "Maximum Radius of a Marker");
            settings.Set<MarkerItem>("Volume", 2000f, "Total Volume of a Marker");
            typeMapper.RegisterItem<MarkerItem, MarkerState, MarkerInfo>(this, "Marker");

            // Smellable
            typeMapper.AttachItemProperty<MarkerItem, SmellableProperty>(this, "Marker Smellable", (i) =>
            {
                SmellableProperty property = new SmellableProperty(i);

                // Bind Smellable Radius to the Item Radius
                property.SmellableRadius = i.Radius;
                i.RadiusChanged += (item, v) => { property.SmellableRadius = v; };

                return property;
            });
        }

        /// <summary>
        /// Registers Bugs
        /// </summary>
        /// <param name="typeMapper">Type Mapper</param>
        /// <param name="settings">Settings</param>
        private void RegisterBug(ITypeMapper typeMapper, KeyValueStore settings)
        {
            // Bug
            typeMapper.RegisterItem<BugItem, BugState, BugInfo>(this, "Bug");

            // Walking
            settings.Set<BugItem>("MaxSpeed", 2f, "Maximum Speed of a Bug");
            typeMapper.AttachItemProperty<BugItem, WalkingProperty>(this, "Bug Walking", (i) =>
            {
                WalkingProperty property = new WalkingProperty(i);

                // Set Maximum Speed
                property.MaximumSpeed = i.Settings.GetFloat<BugItem>("MaxSpeed").Value;

                // Bind Direction to the Items Orientation
                property.Direction = i.Orientation;
                i.OrientationChanged += (item, v) => { property.Direction = v; };

                return property;
            });

            // Collision
            settings.Set<BugItem>("Mass", 10f, "Collision Mass of a Bug");
            typeMapper.AttachItemProperty<BugItem, CollidableProperty>(this, "Bug Collidable", (i) =>
            {
                CollidableProperty property = new CollidableProperty(i);

                // Set Collision Mass
                property.CollisionFixed = false;
                property.CollisionMass = i.Settings.GetFloat<BugItem>("Mass").Value;

                // Bind Collision Radius to the Item Radius
                property.CollisionRadius = i.Radius;
                i.RadiusChanged += (item, v) => { property.CollisionRadius = v; };

                return property;
            });

            // Visibility
            typeMapper.AttachItemProperty<BugItem, VisibleProperty>(this, "Bug Visible", (i) =>
            {
                VisibleProperty property = new VisibleProperty(i);

                // Bind Visibility Radius to the Item Radius
                property.VisibilityRadius = i.Radius;
                i.RadiusChanged += (item, v) => { property.VisibilityRadius = v; };

                return property;
            });

            // Sighting
            settings.Set<BugItem>("ViewRange", 20f, "View Range of a Bug");
            settings.Set<BugItem>("ViewAngle", 360, "View Angle of a Bug");
            typeMapper.AttachItemProperty<BugItem, SightingProperty>(this, "Bug Sighting", (i) =>
            {
                SightingProperty property = new SightingProperty(i);

                // Set View Range and Angle
                property.ViewRange = i.Settings.GetFloat<BugItem>("ViewRange").Value;
                property.ViewAngle = i.Settings.GetFloat<BugItem>("ViewAngle").Value;

                // Bind View Direction to the Item Orientation
                property.ViewDirection = i.Orientation;
                i.OrientationChanged += (item, v) => { property.ViewDirection = v; };

                return property;
            });

            // Sniffer
            typeMapper.AttachItemProperty<BugItem, SnifferProperty>(this, "Bug Sniffer");

            // Attackable
            settings.Set<BugItem>("MaxHealth", 1000, "Maximum Health of a Bug");
            typeMapper.AttachItemProperty<BugItem, AttackableProperty>(this, "Bug Attackable", (i) =>
            {
                AttackableProperty property = new AttackableProperty(i);

                // Bind Attackable Radius to Item Radius
                property.AttackableRadius = i.Radius;
                i.RadiusChanged += (item, v) => { property.AttackableRadius = v; };

                // Health
                property.AttackableMaximumHealth = settings.GetInt<BugItem>("MaxHealth").Value;
                property.AttackableHealth = settings.GetInt<BugItem>("MaxHealth").Value;

                return property;
            });

            // Attacker
            settings.Set<BugItem>("AttackRange", 5f, "Attack Range for a Bug");
            settings.Set<BugItem>("RecoveryTime", 5, "Recovery Time in Rounds for a Bug");
            settings.Set<BugItem>("AttackStrength", 10, "Attach Strength for a Bug");
            typeMapper.AttachItemProperty<BugItem, AttackerProperty>(this, "Bug Attacker", (i) =>
            {
                AttackerProperty property = new AttackerProperty(i);
                property.AttackRange = i.Settings.GetFloat<BugItem>("AttackRange").Value;
                property.AttackRecoveryTime = i.Settings.GetInt<BugItem>("RecoveryTime").Value;
                property.AttackStrength = i.Settings.GetInt<BugItem>("AttackStrength").Value;
                return property;
            });

            //typeMapper.AttachItemProperty<BugItem, CollectorProperty>(this, "Bug Collector"); // BUG_RANGE);
            //typeMapper.AttachItemProperty<BugItem, SugarCollectableProperty>(this, "Bug Sugar Collectable"); // , BUG_SUGAR_CAPACITY, 0);
            //typeMapper.AttachItemProperty<BugItem, AppleCollectableProperty>(this, "Bug Apple Collectable"); // BUG_APPLE_CAPACITY, 0);
        }

        /// <summary>
        /// Registers classic Bugs
        /// </summary>
        /// <param name="typeMapper">Type Mapper</param>
        /// <param name="settings">Settings</param>
        private void RegisterClassicBug(ITypeMapper typeMapper, KeyValueStore settings)
        {
            // Classic Bug
            typeMapper.RegisterItem<ClassicBugItem, BugState, BugInfo>(this, "Classic Bug");

            // Walking
            settings.Set<ClassicBugItem>("MaxSpeed", 2f, "Maximum Speed of a Classic Bug");
            typeMapper.AttachItemProperty<ClassicBugItem, WalkingProperty>(this, "Classic Bug Walking", (i) =>
            {
                WalkingProperty property = new WalkingProperty(i);

                // Set Walking Speed
                property.MaximumSpeed = i.Settings.GetFloat<ClassicBugItem>("MaxSpeed").Value;

                // Bind Direction to the Items Orientation
                property.Direction = i.Orientation;
                i.OrientationChanged += (item, v) => { property.Direction = v; };

                return property;
            });

            // Collision
            settings.Set<ClassicBugItem>("Mass", 10f, "Collision Mass of a Classic Bug");
            typeMapper.AttachItemProperty<ClassicBugItem, CollidableProperty>(this, "Classic Bug Collidable", (i) =>
            {
                CollidableProperty property = new CollidableProperty(i);

                // Set Collision Mass
                property.CollisionFixed = false;
                property.CollisionMass = i.Settings.GetFloat<ClassicBugItem>("Mass").Value;

                // Bind Collision Radius to the Item Radius
                property.CollisionRadius = i.Radius;
                i.RadiusChanged += (item, v) => { property.CollisionRadius = v; };

                return property;
            });

            // Visibility
            typeMapper.AttachItemProperty<ClassicBugItem, VisibleProperty>(this, "Classic Bug Visible", (i) =>
            {
                VisibleProperty property = new VisibleProperty(i);

                // Bind Visibility Radius to the Item Radius
                property.VisibilityRadius = i.Radius;
                i.RadiusChanged += (item, v) => { property.VisibilityRadius = v; };

                return property;
            });

            // Sighting
            settings.Set<ClassicBugItem>("ViewRange", 20f, "View Range of a Classic Bug");
            settings.Set<ClassicBugItem>("ViewAngle", 360, "View Angle of a Classic Bug");
            typeMapper.AttachItemProperty<ClassicBugItem, SightingProperty>(this, "Classic Bug Sighting", (i) =>
            {
                SightingProperty property = new SightingProperty(i);

                // Set View Range and Angle
                property.ViewRange = i.Settings.GetFloat<ClassicBugItem>("ViewRange").Value;
                property.ViewAngle = i.Settings.GetFloat<ClassicBugItem>("ViewAngle").Value;

                // Bind View Direction to the Item Orientation
                property.ViewDirection = i.Orientation;
                i.OrientationChanged += (item, v) => { property.ViewDirection = v; };

                return property;
            });

            // Sniffer
            typeMapper.AttachItemProperty<ClassicBugItem, SnifferProperty>(this, "Classic Bug Sniffer");

            // Attackable
            settings.Set<ClassicBugItem>("MaxHealth", 1000, "Maximum Health of a Classic Bug");
            typeMapper.AttachItemProperty<ClassicBugItem, AttackableProperty>(this, "Classic Bug Attackable", (i) =>
            {
                AttackableProperty property = new AttackableProperty(i);

                // Bind Attackable Radius to Item Radius
                property.AttackableRadius = i.Radius;
                i.RadiusChanged += (item, v) => { property.AttackableRadius = v; };

                // Health
                property.AttackableMaximumHealth = settings.GetInt<ClassicBugItem>("MaxHealth").Value;
                property.AttackableHealth = settings.GetInt<ClassicBugItem>("MaxHealth").Value;

                return property;
            });

            // Attacker
            settings.Set<ClassicBugItem>("AttackRange", 5f, "Attack Range for a Classic Bug");
            settings.Set<ClassicBugItem>("RecoveryTime", 5, "Recovery Time in Rounds for a Classic Bug");
            settings.Set<ClassicBugItem>("AttackStrength", 10, "Attach Strength for a Classic Bug");
            typeMapper.AttachItemProperty<ClassicBugItem, AttackerProperty>(this, "Classic Bug Attacker", (i) =>
            {
                AttackerProperty property = new AttackerProperty(i);
                property.AttackRange = i.Settings.GetFloat<ClassicBugItem>("AttackRange").Value;
                property.AttackRecoveryTime = i.Settings.GetInt<ClassicBugItem>("RecoveryTime").Value;
                property.AttackStrength = i.Settings.GetInt<ClassicBugItem>("AttackStrength").Value;
                return property;
            });
        }

        /// <summary>
        /// Registers Ants
        /// </summary>
        /// <param name="typeMapper">Type Mapper</param>
        /// <param name="settings">Settings</param>
        private void RegisterAnt(ITypeMapper typeMapper, KeyValueStore settings)
        {
            // Ant Item
            settings.Set<AntItem>("ZickZackAngle", 10, "Correction Angle after Sprint");
            settings.Set<AntItem>("ZickZackRange", 30f, "Distance to go every Sprint");
            settings.Set<AntItem>("DropSugar", false, "Will an Ant leave a small Sugar on Drop");
            settings.Set<AntItem>("MarkerDelay", 10, "Time in Rounds between Marker-Drops");
            settings.Set<AntItem>("ClassicBorderBehavior", true, "Should an ant be reflected by Walls (like in AntMe! 1)");
            settings.Set<AntItem>("RotationSpeed[-1]", 10, "Maximum Rotation Angle per Round (with Speed Attribute -1)");
            settings.Set<AntItem>("RotationSpeed[0]", 20, "Maximum Rotation Angle per Round (with Speed Attribute 0)");
            settings.Set<AntItem>("RotationSpeed[1]", 30, "Maximum Rotation Angle per Round (with Speed Attribute 1)");
            settings.Set<AntItem>("RotationSpeed[2]", 40, "Maximum Rotation Angle per Round (with Speed Attribute 2)");
            typeMapper.RegisterItem<AntItem, AntState, AntInfo>(this, "Ant");

            // Walking
            settings.Set<AntItem>("MaxSpeed[-1]", 0.8f, "Maximum Speed of an Ant (with Speed Attribute -1)");
            settings.Set<AntItem>("MaxSpeed[0]", 1f, "Maximum Speed of an Ant (with Speed Attribute 0)");
            settings.Set<AntItem>("MaxSpeed[1]", 1.2f, "Maximum Speed of an Ant (with Speed Attribute 1)");
            settings.Set<AntItem>("MaxSpeed[2]", 1.4f, "Maximum Speed of an Ant (with Speed Attribute 2)");
            typeMapper.AttachItemProperty<AntItem, WalkingProperty>(this, "Ant Walking", (i) =>
            {
                WalkingProperty property = new WalkingProperty(i);

                // Try to get Attribute Setting
                sbyte speedAttribute = 0;
                if (i.Attributes != null)
                    speedAttribute = i.Attributes.GetValue("speed");

                // Set Maximum Speed based on the current Settings
                float maxSpeed;
                switch (speedAttribute)
                {
                    case -1: maxSpeed = i.Settings.GetFloat<AntItem>("MaxSpeed[-1]").Value; break;
                    case 1: maxSpeed = i.Settings.GetFloat<AntItem>("MaxSpeed[1]").Value; break;
                    case 2: maxSpeed = i.Settings.GetFloat<AntItem>("MaxSpeed[2]").Value; break;
                    default: maxSpeed = i.Settings.GetFloat<AntItem>("MaxSpeed[0]").Value; break;
                }
                // TODO: Check for Castes
                property.MaximumSpeed = maxSpeed;

                // Bind Item Orientation to Walk-Direction
                property.Direction = i.Orientation;
                i.OrientationChanged += (item, v) => { property.Direction = v; };

                return property;
            });

            // Collision
            settings.Set<AntItem>("Mass[-1]", 0.5f, "Collision Mass of an Ant (with Defense Attribute -1)");
            settings.Set<AntItem>("Mass[0]", 1f, "Collision Mass of an Ant (with Defense Attribute 0)");
            settings.Set<AntItem>("Mass[1]", 1.5f, "Collision Mass of an Ant (with Defense Attribute 1)");
            settings.Set<AntItem>("Mass[2]", 2f, "Collision Mass of an Ant (with Defense Attribute 2)");
            typeMapper.AttachItemProperty<AntItem, CollidableProperty>(this, "Ant Collidable", (i) =>
            {
                CollidableProperty property = new CollidableProperty(i);

                // Try to get Attribute Setting
                sbyte defenseAttribute = 0;
                if (i.Attributes != null)
                    defenseAttribute = i.Attributes.GetValue("defense");

                float mass;
                switch (defenseAttribute)
                {
                    case -1: mass = i.Settings.GetFloat<AntItem>("Mass[-1]").Value; break;
                    case 1: mass = i.Settings.GetFloat<AntItem>("Mass[1]").Value; break;
                    case 2: mass = i.Settings.GetFloat<AntItem>("Mass[2]").Value; break;
                    default: mass = i.Settings.GetFloat<AntItem>("Mass[0]").Value; break;
                }

                // Set Mass to Settings
                property.CollisionFixed = false;
                property.CollisionMass = mass;

                // Bind Collision Radius to Item Radius
                property.CollisionRadius = i.Radius;
                i.RadiusChanged += (item, v) => { property.CollisionRadius = v; };

                return property;
            });

            // Visibility
            typeMapper.AttachItemProperty<AntItem, VisibleProperty>(this, "Ant Visible", (i) =>
            {
                VisibleProperty property = new VisibleProperty(i);

                // Bind Visibility Radius to the Item Radius
                property.VisibilityRadius = i.Radius;
                i.RadiusChanged += (item, v) => { property.VisibilityRadius = v; };

                return property;
            });

            // Sighting
            settings.Set<AntItem>("ViewRange[-1]", 10f, "View Range of an Ant (with Attention Attribute -1)");
            settings.Set<AntItem>("ViewRange[0]", 20f, "View Range of an Ant (with Attention Attribute 0)");
            settings.Set<AntItem>("ViewRange[1]", 25f, "View Range of an Ant (with Attention Attribute 1)");
            settings.Set<AntItem>("ViewRange[2]", 30f, "View Range of an Ant (with Attention Attribute 2)");
            settings.Set<AntItem>("ViewAngle[-1]", 45, "View Angle of an Ant (with Attention Attribute -1)");
            settings.Set<AntItem>("ViewAngle[0]", 90, "View Angle of an Ant (with Attention Attribute 0)");
            settings.Set<AntItem>("ViewAngle[1]", 180, "View Angle of an Ant (with Attention Attribute 1)");
            settings.Set<AntItem>("ViewAngle[2]", 270, "View Angle of an Ant (with Attention Attribute 2)");
            typeMapper.AttachItemProperty<AntItem, SightingProperty>(this, "Ant Sighting", (i) =>
            {
                SightingProperty property = new SightingProperty(i);

                // Try to get Attribute Setting
                sbyte attentionAttribute = 0;
                if (i.Attributes != null)
                {
                    attentionAttribute = i.Attributes.GetValue("attention");
                }

                // Set View Range and Angle
                float viewRange;
                switch (attentionAttribute)
                {
                    case -1: viewRange = i.Settings.GetFloat<AntItem>("ViewRange[-1]").Value; break;
                    case 1: viewRange = i.Settings.GetFloat<AntItem>("ViewRange[1]").Value; break;
                    case 2: viewRange = i.Settings.GetFloat<AntItem>("ViewRange[2]").Value; break;
                    default: viewRange = i.Settings.GetFloat<AntItem>("ViewRange[0]").Value; break;
                }
                property.ViewRange = viewRange;

                float viewAngle;
                switch (attentionAttribute)
                {
                    case -1: viewAngle = i.Settings.GetFloat<AntItem>("ViewAngle[-1]").Value; break;
                    case 1: viewAngle = i.Settings.GetFloat<AntItem>("ViewAngle[1]").Value; break;
                    case 2: viewAngle = i.Settings.GetFloat<AntItem>("ViewAngle[2]").Value; break;
                    default: viewAngle = i.Settings.GetFloat<AntItem>("ViewAngle[0]").Value; break;
                }
                property.ViewAngle = viewAngle;

                // Bind View Direction to the Item Orientation
                property.ViewDirection = i.Orientation;
                i.OrientationChanged += (item, v) => { property.ViewDirection = v; };

                return property;
            });

            // Sniffer
            typeMapper.AttachItemProperty<AntItem, SnifferProperty>(this, "Ant Sniffer");

            // Carrier
            settings.Set<AntItem>("CarrierStrength[-1]", 5f, "Carrier Strength of an Ant (with Strength Attribute -1)");
            settings.Set<AntItem>("CarrierStrength[0]", 10f, "Carrier Strength of an Ant (with Strength Attribute 0)");
            settings.Set<AntItem>("CarrierStrength[1]", 15f, "Carrier Strength of an Ant (with Strength Attribute 1)");
            settings.Set<AntItem>("CarrierStrength[2]", 20f, "Carrier Strength of an Ant (with Strength Attribute 2)");
            typeMapper.AttachItemProperty<AntItem, CarrierProperty>(this, "Ant Carrier", (i) =>
            {
                CarrierProperty property = new CarrierProperty(i);

                // Try to get Attribute Setting
                sbyte strengthAttribute = 0;
                if (i.Attributes != null)
                    strengthAttribute = i.Attributes.GetValue("strength");

                float strength;
                switch (strengthAttribute)
                {
                    case -1: strength = i.Settings.GetFloat<AntItem>("CarrierStrength[-1]").Value; break;
                    case 1: strength = i.Settings.GetFloat<AntItem>("CarrierStrength[1]").Value; break;
                    case 2: strength = i.Settings.GetFloat<AntItem>("CarrierStrength[2]").Value; break;
                    default: strength = i.Settings.GetFloat<AntItem>("CarrierStrength[0]").Value; break;
                }

                property.CarrierStrength = strength;
                return property;
            });

            // Attackable
            settings.Set<AntItem>("MaxHealth[-1]", 100f, "Maximum Health for an Ant (with Defense Attribute -1)");
            settings.Set<AntItem>("MaxHealth[0]", 100f, "Maximum Health for an Ant (with Defense Attribute 0)");
            settings.Set<AntItem>("MaxHealth[1]", 100f, "Maximum Health for an Ant (with Defense Attribute 1)");
            settings.Set<AntItem>("MaxHealth[2]", 100f, "Maximum Health for an Ant (with Defense Attribute 2)");
            typeMapper.AttachItemProperty<AntItem, AttackableProperty>(this, "Ant Attackable", (i) =>
            {
                AttackableProperty property = new AttackableProperty(i);

                // Try to get Attribute Setting
                sbyte defenseAttribute = 0;
                if (i.Attributes != null)
                {
                    defenseAttribute = i.Attributes.GetValue("defense");
                }

                // Health
                int health;
                switch (defenseAttribute)
                {
                    case -1: health = i.Settings.GetInt<AntItem>("MaxHealth[-1]").Value; break;
                    case 1: health = i.Settings.GetInt<AntItem>("MaxHealth[1]").Value; break;
                    case 2: health = i.Settings.GetInt<AntItem>("MaxHealth[2]").Value; break;
                    default: health = i.Settings.GetInt<AntItem>("MaxHealth[0]").Value; break;
                }
                property.AttackableMaximumHealth = health;
                property.AttackableHealth = health;

                // Bind Attackable Radius to Item Radius
                property.AttackableRadius = i.Radius;
                i.RadiusChanged += (item, v) => { property.AttackableRadius = v; };

                return property;
            });

            // Attacker
            settings.Set<AntItem>("AttackRange[-1]", 2f, "Attack Range for an Ant (with Attack Attribute -1)");
            settings.Set<AntItem>("AttackRange[0]", 3f, "Attack Range for an Ant (with Attack Attribute 0)");
            settings.Set<AntItem>("AttackRange[1]", 5f, "Attack Range for an Ant (with Attack Attribute 1)");
            settings.Set<AntItem>("AttackRange[2]", 7f, "Attack Range for an Ant (with Attack Attribute 2)");
            settings.Set<AntItem>("RecoveryTime[-1]", 4, "Recovery Time in Rounds for an Ant (with Speed Attribute -1)");
            settings.Set<AntItem>("RecoveryTime[0]", 2, "Recovery Time in Rounds for an Ant (with Speed Attribute 0)");
            settings.Set<AntItem>("RecoveryTime[1]", 1, "Recovery Time in Rounds for an Ant (with Speed Attribute 1)");
            settings.Set<AntItem>("RecoveryTime[2]", 0, "Recovery Time in Rounds for an Ant (with Speed Attribute 2)");
            settings.Set<AntItem>("AttackStrength[-1]", 4, "Attack Strength for an Ant (with Strength Attribute -1)");
            settings.Set<AntItem>("AttackStrength[0]", 5, "Attack Strength for an Ant (with Strength Attribute 0)");
            settings.Set<AntItem>("AttackStrength[1]", 7, "Attack Strength for an Ant (with Strength Attribute 1)");
            settings.Set<AntItem>("AttackStrength[2]", 10, "Attack Strength for an Ant (with Strength Attribute 2)");
            settings.Set<AntItem>("AttackMultiplier[-1]", 0.7f, "Multiplier for the Attack Strength for an Ant (with Attack Attribute -1)");
            settings.Set<AntItem>("AttackMultiplier[0]", 1f, "Multiplier for the Attack Strength for an Ant (with Attack Attribute 0)");
            settings.Set<AntItem>("AttackMultiplier[1]", 1.4f, "Multiplier for the Attack Strength for an Ant (with Attack Attribute 1)");
            settings.Set<AntItem>("AttackMultiplier[2]", 1.8f, "Multiplier for the Attack Strength for an Ant (with Attack Attribute 2)");
            typeMapper.AttachItemProperty<AntItem, AttackerProperty>(this, "Ant Attacker", (i) =>
            {
                // Try to get Attribute Setting
                sbyte attackAttribute = 0;
                sbyte speedAttribute = 0;
                sbyte strengthAttribute = 0;
                if (i.Attributes != null)
                {
                    attackAttribute = i.Attributes.GetValue("attack");
                    speedAttribute = i.Attributes.GetValue("speed");
                    strengthAttribute = i.Attributes.GetValue("strength");
                }

                AttackerProperty property = new AttackerProperty(i);

                float attackRange;
                switch (attackAttribute)
                {
                    case -1: attackRange = i.Settings.GetFloat<AntItem>("AttackRange[-1]").Value; break;
                    case 1: attackRange = i.Settings.GetFloat<AntItem>("AttackRange[1]").Value; break;
                    case 2: attackRange = i.Settings.GetFloat<AntItem>("AttackRange[2]").Value; break;
                    default: attackRange = i.Settings.GetFloat<AntItem>("AttackRange[0]").Value; break;
                }
                property.AttackRange = attackRange;

                int recoveryTime;
                switch (speedAttribute)
                {
                    case -1: recoveryTime = i.Settings.GetInt<AntItem>("RecoveryTime[-1]").Value; break;
                    case 1: recoveryTime = i.Settings.GetInt<AntItem>("RecoveryTime[1]").Value; break;
                    case 2: recoveryTime = i.Settings.GetInt<AntItem>("RecoveryTime[2]").Value; break;
                    default: recoveryTime = i.Settings.GetInt<AntItem>("RecoveryTime[0]").Value; break;
                }
                property.AttackRecoveryTime = recoveryTime;

                int attackStrength;
                switch (strengthAttribute)
                {
                    case -1: attackStrength = i.Settings.GetInt<AntItem>("AttackStrength[-1]").Value; break;
                    case 1: attackStrength = i.Settings.GetInt<AntItem>("AttackStrength[1]").Value; break;
                    case 2: attackStrength = i.Settings.GetInt<AntItem>("AttackStrength[2]").Value; break;
                    default: attackStrength = i.Settings.GetInt<AntItem>("AttackStrength[0]").Value; break;
                }
                switch (attackAttribute)
                {
                    case -1: attackStrength = (int)(attackStrength * i.Settings.GetFloat<AntItem>("AttackMultiplier[-1]").Value); break;
                    case 1: attackStrength = (int)(attackStrength * i.Settings.GetFloat<AntItem>("AttackMultiplier[1]").Value); break;
                    case 2: attackStrength = (int)(attackStrength * i.Settings.GetFloat<AntItem>("AttackMultiplier[2]").Value); break;
                    default: attackStrength = (int)(attackStrength * i.Settings.GetFloat<AntItem>("AttackMultiplier[0]").Value); break;
                }
                property.AttackStrength = attackStrength;

                return property;
            });

            // Collector
            settings.Set<AntItem>("SugarCapacity[-1]", 3, "Maximum Capacity for Sugar (with Strength Attribute -1)");
            settings.Set<AntItem>("SugarCapacity[0]", 5, "Maximum Capacity for Sugar (with Strength Attribute 0)");
            settings.Set<AntItem>("SugarCapacity[1]", 7, "Maximum Capacity for Sugar (with Strength Attribute 1)");
            settings.Set<AntItem>("SugarCapacity[2]", 10, "Maximum Capacity for Sugar (with Strength Attribute 2)");
            settings.Set<AntItem>("AppleCapacity[-1]", 1, "Maximum Capacity for Apple (with Strength Attribute -1)");
            settings.Set<AntItem>("AppleCapacity[0]", 2, "Maximum Capacity for Apple (with Strength Attribute 0)");
            settings.Set<AntItem>("AppleCapacity[1]", 4, "Maximum Capacity for Apple (with Strength Attribute 1)");
            settings.Set<AntItem>("AppleCapacity[2]", 6, "Maximum Capacity for Apple (with Strength Attribute 2)");
            typeMapper.AttachItemProperty<AntItem, SugarCollectorProperty>(this, "Ant Sugar Collectable", (i) =>
            {
                // Try to get Attribute Setting
                sbyte strengthAttribute = 0;
                if (i.Attributes != null)
                    strengthAttribute = i.Attributes.GetValue("strength");

                int capacity;
                switch (strengthAttribute)
                {
                    case -1: capacity = i.Settings.GetInt<AntItem>("SugarCapacity[-1]").Value; break;
                    case 1: capacity = i.Settings.GetInt<AntItem>("SugarCapacity[1]").Value; break;
                    case 2: capacity = i.Settings.GetInt<AntItem>("SugarCapacity[2]").Value; break;
                    default: capacity = i.Settings.GetInt<AntItem>("SugarCapacity[0]").Value; break;
                }

                SugarCollectorProperty property = new SugarCollectorProperty(i);
                property.Capacity = capacity;
                return property;
            });
            typeMapper.AttachItemProperty<AntItem, AppleCollectorProperty>(this, "Ant Apple Collectable", (i) =>
            {
                // Try to get Attribute Setting
                sbyte strengthAttribute = 0;
                if (i.Attributes != null)
                    strengthAttribute = i.Attributes.GetValue("strength");

                int capacity;
                switch (strengthAttribute)
                {
                    case -1: capacity = i.Settings.GetInt<AntItem>("AppleCapacity[-1]").Value; break;
                    case 1: capacity = i.Settings.GetInt<AntItem>("AppleCapacity[1]").Value; break;
                    case 2: capacity = i.Settings.GetInt<AntItem>("AppleCapacity[2]").Value; break;
                    default: capacity = i.Settings.GetInt<AntItem>("AppleCapacity[0]").Value; break;
                }

                AppleCollectorProperty property = new AppleCollectorProperty(i);
                property.Capacity = capacity;
                return property;
            }); // TODO: Optional, wenn _settings.ANT_APPLECOLLECT | _settings.ANT_APPLE_CAPACITY, 0);
        }
    }
}
