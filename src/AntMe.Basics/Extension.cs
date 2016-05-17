using AntMe.Basics.EngineExtensions;
using AntMe.Basics.Factions;
using AntMe.Basics.Factions.Ants;
using AntMe.Basics.Factions.Ants.Interop;
using AntMe.Basics.Factions.Bugs;
using AntMe.Basics.ItemProperties;
using AntMe.Basics.Items;

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
        public void Load(ITypeMapper typeMapper, Settings settings)
        {
            // ##########################
            // Standard Engine Extensions
            // ##########################
            typeMapper.RegisterEngineProperty<InteractionExtension>(this, "Interaction Extension (Core)", 70);
            typeMapper.RegisterEngineProperty<PhysicsExtension>(this, "Physics Extension (Core)", 100);

            settings.Set<RecognitionExtension>("SmellsAliance", false, "Can a Unit smell Smellable Stuff from Aliance Units");
            settings.Set<RecognitionExtension>("SmellsForeign", false, "Can a Unit smell Smellable Stuff from Enemy Units");
            typeMapper.RegisterEngineProperty<RecognitionExtension>(this, "Recognition Extension (Core)", 150);

            // ##########################
            // Standard Item Properties
            // ##########################
            typeMapper.RegisterItemPropertyS<AttackableProperty, AttackableState>(this, "Attackable");
            typeMapper.RegisterItemPropertyS<AttackerProperty, AttackerState>(this, "Attacker");
            typeMapper.RegisterItemPropertyS<CarrierProperty, CarrierState>(this, "Carrier");
            typeMapper.RegisterItemPropertyS<CollidableProperty, CollidableState>(this, "Collidable");
            typeMapper.RegisterItemPropertyS<PortableProperty, PortableState>(this, "Portable");
            typeMapper.RegisterItemPropertyS<SightingProperty, SightingState>(this, "Sighting");
            typeMapper.RegisterItemPropertyS<SmellableProperty, SmellableState>(this, "Smellable");
            typeMapper.RegisterItemPropertyS<SnifferProperty, SnifferState>(this, "Sniffer");
            typeMapper.RegisterItemPropertyS<VisibleProperty, VisibleState>(this, "Visible");
            typeMapper.RegisterItemPropertyS<WalkingProperty, WalkingState>(this, "Walking");
            typeMapper.RegisterItemPropertyS<AppleCollectorProperty, AppleCollectorState>(this, "Apple Collector");
            typeMapper.RegisterItemPropertyS<SugarCollectorProperty, SugarCollectorState>(this, "Sugar Collector");
            typeMapper.RegisterItemPropertyS<AppleCollectableProperty, AppleCollectableState>(this, "Apple Collectable");
            typeMapper.RegisterItemPropertyS<SugarCollectableProperty, SugarCollectableState>(this, "Sugar Collectable");

            // ##########################
            // Factions registrieren
            // ##########################

            // Ant Faction
            settings.Set<AntFaction>("InitialAnthillCount", 1, "Number of initial Anthills");
            settings.Set<AntFaction>("InitialAntCount", 0, "Number of initial Ants");
            settings.Set<AntFaction>("ConcurrentAntCount", 100, "Number of concurrent Ants");
            settings.Set<AntFaction>("ConcurrentAnthillCount", 1, "Number of concurrent Anthills");
            settings.Set<AntFaction>("TotalAntCount", int.MaxValue, "Total Number of Ants per Simulation");
            settings.Set<AntFaction>("TotalAnthillCount", 1, "Number of total Anthills per Simulation");
            settings.Set<AntFaction>("AntRespawnDelay", 1, "Number of Rounds until another Respawn");
            typeMapper.RegisterFaction<AntFaction, AntFactionState, FactionInfo, AntFactory, AntFactoryInterop, AntUnit, AntUnitInterop>(this, "Ants");

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

            // Bug Faction
            typeMapper.RegisterFaction<BugFaction, BugFactionState, FactionInfo, BugFactory, BugFactoryInterop, BugUnit, BugUnitInterop>(this, "Bugs");

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
        }

        /// <summary>
        /// Registers Apples
        /// </summary>
        /// <param name="typeMapper">Type Mapper</param>
        /// <param name="settings">Settings</param>
        private void RegisterApple(ITypeMapper typeMapper, Settings settings)
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
        private void RegisterSugar(ITypeMapper typeMapper, Settings settings)
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
        private void RegisterAnthill(ITypeMapper typeMapper, Settings settings)
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
        private void RegisterMarker(ITypeMapper typeMapper, Settings settings)
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
        private void RegisterBug(ITypeMapper typeMapper, Settings settings)
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
        private void RegisterClassicBug(ITypeMapper typeMapper, Settings settings)
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
        private void RegisterAnt(ITypeMapper typeMapper, Settings settings)
        {
            // Ant Item
            settings.Set<AntItem>("ZickZackAngle", 10, "Correction Angle after Sprint");
            settings.Set<AntItem>("ZickZackRange", 30f, "Distance to go every Sprint");
            settings.Set<AntItem>("RotationSpeed", 20, "Maximum Rotation Angle per Round");
            settings.Set<AntItem>("DropSugar", false, "Will an Ant leave a small Sugar on Drop");
            settings.Set<AntItem>("MarkerDelay", 10, "Time in Rounds between Marker-Drops");
            typeMapper.RegisterItem<AntItem, AntState, AntInfo>(this, "Ant");

            // Walking
            settings.Set<AntItem>("MaxSpeed", 1f, "Maximum Speed of an Ant");
            typeMapper.AttachItemProperty<AntItem, WalkingProperty>(this, "Ant Walking", (i) =>
            {
                WalkingProperty property = new WalkingProperty(i);

                // Set Maximum Speed based on the current Settings
                // TODO: Check for Castes
                property.MaximumSpeed = i.Settings.GetFloat<AntItem>("MaxSpeed").Value;

                // Bind Item Orientation to Walk-Direction
                property.Direction = i.Orientation;
                i.OrientationChanged += (item, v) => { property.Direction = v; };

                return property;
            });

            // Collision
            settings.Set<AntItem>("Mass", 1f, "Collision Mass of an Ant");
            typeMapper.AttachItemProperty<AntItem, CollidableProperty>(this, "Ant Collidable", (i) =>
            {
                CollidableProperty property = new CollidableProperty(i);

                // Set Mass to Settings
                property.CollisionFixed = false;
                property.CollisionMass = i.Settings.GetFloat<AntItem>("Mass").Value;

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
            settings.Set<AntItem>("ViewRange", 20f, "View Range of an Ant");
            settings.Set<AntItem>("ViewAngle", 360, "View Angle of an Ant");
            typeMapper.AttachItemProperty<AntItem, SightingProperty>(this, "Ant Sighting", (i) =>
            {
                SightingProperty property = new SightingProperty(i);

                // Set View Range and Angle
                property.ViewRange = i.Settings.GetFloat<AntItem>("ViewRange").Value;
                property.ViewAngle = i.Settings.GetFloat<AntItem>("ViewAngle").Value;

                // Bind View Direction to the Item Orientation
                property.ViewDirection = i.Orientation;
                i.OrientationChanged += (item, v) => { property.ViewDirection = v; };

                return property;
            });

            // Sniffer
            typeMapper.AttachItemProperty<AntItem, SnifferProperty>(this, "Ant Sniffer");

            // Carrier
            settings.Set<AntItem>("CarrierStrength", 10f, "Carrier Strength of an Ant");
            typeMapper.AttachItemProperty<AntItem, CarrierProperty>(this, "Ant Carrier", (i) =>
            {
                CarrierProperty property = new CarrierProperty(i);
                property.CarrierStrength = i.Settings.GetFloat<AntItem>("CarrierStrength").Value;
                return property;
            });

            // Attackable
            settings.Set<AntItem>("MaxHealth", 100f, "Maximum Health for an Ant");
            typeMapper.AttachItemProperty<AntItem, AttackableProperty>(this, "Ant Attackable", (i) =>
            {
                AttackableProperty property = new AttackableProperty(i);

                // Bind Attackable Radius to Item Radius
                property.AttackableRadius = i.Radius;
                i.RadiusChanged += (item, v) => { property.AttackableRadius = v; };

                // Health
                property.AttackableMaximumHealth = settings.GetInt<AntItem>("MaxHealth").Value;
                property.AttackableHealth = settings.GetInt<AntItem>("MaxHealth").Value;

                return property;
            });

            // Attacker
            settings.Set<AntItem>("AttackRange", 3f, "Attack Range for a Bug");
            settings.Set<AntItem>("RecoveryTime", 2, "Recovery Time in Rounds for a Bug");
            settings.Set<AntItem>("AttackStrength", 5, "Attach Strength for a Bug");
            typeMapper.AttachItemProperty<AntItem, AttackerProperty>(this, "Ant Attacker", (i) =>
            {
                AttackerProperty property = new AttackerProperty(i);
                property.AttackRange = i.Settings.GetFloat<AntItem>("AttackRange").Value;
                property.AttackRecoveryTime = i.Settings.GetInt<AntItem>("RecoveryTime").Value;
                property.AttackStrength = i.Settings.GetInt<AntItem>("AttackStrength").Value;
                return property;
            });

            // Collector
            settings.Set<AntItem>("SugarCapacity", 5, "Maximum Capacity for Sugar");
            settings.Set<AntItem>("AppleCapacity", 2, "Maximum Capacity for Apple");
            typeMapper.AttachItemProperty<AntItem, SugarCollectorProperty>(this, "Ant Sugar Collectable", (i) =>
            {
                SugarCollectorProperty property = new SugarCollectorProperty(i);
                property.Capacity = i.Settings.GetInt<AntItem>("SugarCapacity").Value;
                return property;
            });
            typeMapper.AttachItemProperty<AntItem, AppleCollectorProperty>(this, "Ant Apple Collectable", (i) =>
            {
                AppleCollectorProperty property = new AppleCollectorProperty(i);
                property.Capacity = i.Settings.GetInt<AntItem>("AppleCapacity").Value;
                return property;
            }); // TODO: Optional, wenn _settings.ANT_APPLECOLLECT | _settings.ANT_APPLE_CAPACITY, 0);
        }
    }
}
