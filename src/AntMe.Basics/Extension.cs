using AntMe.EngineExtensions.Basics;
using AntMe.Factions.Ants;
using AntMe.Factions.Bugs;
using AntMe.ItemProperties.Basics;
using AntMe.Items.Basics;
using System;

namespace AntMe.Extension.Basics
{
    /// <summary>
    /// Extension for the Basic AntMe! Items.
    /// </summary>
    public sealed class Extension : IExtensionPack
    {
        public string Author { get { return "Tom Wendel @ AntMe! GmbH"; } }

        public string Description { get { return "Basic Extension with all the Core AntMe! Elements."; } }

        public string Name { get { return "AntMe! Basics"; } }

        public Version Version { get { return new Version(2, 0); } }

        public void Load(ITypeMapper typeMapper, Settings settings)
        {
            // ##########################
            // Standard Engine Extensions
            // ##########################
            typeMapper.RegisterEngineProperty<InteractionExtension>(this, "Interaction Extension (Core)", 70);
            typeMapper.RegisterEngineProperty<PhysicsExtension>(this, "Physics Extension (Core)", 100);
            typeMapper.RegisterEngineProperty<RecognitionExtension>(this, "Recognition Extension (Core)", 150);

            // ##########################
            // Standard Item Properties
            // ##########################
            typeMapper.RegisterItemPropertyS<AttackableProperty, AttackableState>(this, "Attackable");
            typeMapper.RegisterItemPropertyS<AttackerProperty, AttackerState>(this, "Attacker");
            typeMapper.RegisterItemPropertyS<CarrierProperty, CarrierState>(this, "Carrier");
            typeMapper.RegisterItemPropertyS<CollectorProperty, CollectorState>(this, "Collector");
            typeMapper.RegisterItemPropertyS<CollidableProperty, CollidableState>(this, "Collidable");
            typeMapper.RegisterItemPropertyS<PortableProperty, PortableState>(this, "Portable");
            typeMapper.RegisterItemPropertyS<SightingProperty, SightingState>(this, "Sighting");
            typeMapper.RegisterItemPropertyS<SmellableProperty, SmellableState>(this, "Smellable");
            typeMapper.RegisterItemPropertyS<SnifferProperty, SnifferState>(this, "Sniffer");
            typeMapper.RegisterItemPropertyS<VisibleProperty, VisibleState>(this, "Visible");
            typeMapper.RegisterItemPropertyS<WalkingProperty, WalkingState>(this, "Walking");
            typeMapper.RegisterItemPropertyS<CollectableProperty, CollectableState>(this, "Collectable");
            typeMapper.RegisterItemPropertyS<AppleCollectableProperty, AppleCollectableState>(this, "Apple Collectable");
            typeMapper.RegisterItemPropertyS<SugarCollectableProperty, SugarCollectableState>(this, "Sugar Collectable");

            // ##########################
            // Factions registrieren
            // ##########################

            // Basics
            typeMapper.RegisterFaction<AntFaction, AntFactionState, FactionInfo, AntFactory, AntFactoryInterop, AntUnit, AntUnitInterop>(this, "Ants");
            typeMapper.RegisterFaction<BugFaction, BugFactionState, FactionInfo, BugFactory, BugFactoryInterop, BugUnit, BugUnitInterop>(this, "Bugs");

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
            settings.Apply<AppleItem>("Mass", 200f, "Mass of an Apple");
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

            typeMapper.AttachItemProperty<AppleItem, PortableProperty>(this, "Apple Portable");
            typeMapper.AttachItemProperty<AppleItem, CollectableProperty>(this, "Apple Collectable");
            typeMapper.AttachItemProperty<AppleItem, AppleCollectableProperty>(this, "Apple Collectable"); // TODO: Amounts (amount))

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

            typeMapper.AttachItemProperty<SugarItem, CollectableProperty>(this, "Sugar Collectable");
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

            typeMapper.AttachItemProperty<AnthillItem, AttackableProperty>(this, "Anthill Attackable"); // TODO: Optional, wenn Settings angreifbar sind
            typeMapper.AttachItemProperty<AnthillItem, CollectableProperty>(this, "Anthill Collectable"); // TODO: Radius, Vermutlich entfernen
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
            settings.Apply<MarkerItem>("MinRadius", 20f, "Minimum Radius of a Marker");
            settings.Apply<MarkerItem>("MaxRadius", 200f, "Maximum Radius of a Marker");
            settings.Apply<MarkerItem>("Volume", 2000f, "Total Volume of a Marker");
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
            settings.Apply<BugItem>("MaxSpeed", 2f, "Maximum Speed of a Bug");
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
            settings.Apply<BugItem>("Mass", 10f, "Collision Mass of a Bug");
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
            settings.Apply<BugItem>("ViewRange", 20f, "View Range of a Bug");
            settings.Apply<BugItem>("ViewAngle", 360, "View Angle of a Bug");
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

            typeMapper.AttachItemProperty<BugItem, AttackableProperty>(this, "Bug Attackable"); //  BUG_RADIUS, BUG_HITPOINTS, BUG_HITPOINTS);
            typeMapper.AttachItemProperty<BugItem, AttackerProperty>(this, "Bug Attacker"); // BUG_RANGE, BUG_ATTACK_STRENGHT);
            typeMapper.AttachItemProperty<BugItem, CollectorProperty>(this, "Bug Collector"); // BUG_RANGE);
            typeMapper.AttachItemProperty<BugItem, SugarCollectableProperty>(this, "Bug Sugar Collectable"); // , BUG_SUGAR_CAPACITY, 0);
            typeMapper.AttachItemProperty<BugItem, AppleCollectableProperty>(this, "Bug Apple Collectable"); // BUG_APPLE_CAPACITY, 0);
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
            settings.Apply<ClassicBugItem>("MaxSpeed", 2f, "Maximum Speed of a Classic Bug");
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
            settings.Apply<ClassicBugItem>("Mass", 10f, "Collision Mass of a Classic Bug");
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
            settings.Apply<ClassicBugItem>("ViewRange", 20f, "View Range of a Classic Bug");
            settings.Apply<ClassicBugItem>("ViewAngle", 360, "View Angle of a Classic Bug");
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

            typeMapper.AttachItemProperty<ClassicBugItem, AttackableProperty>(this, "Classic Bug Attackable"); //  BUG_RADIUS, BUG_HITPOINTS, BUG_HITPOINTS);
            typeMapper.AttachItemProperty<ClassicBugItem, AttackerProperty>(this, "Classic Bug Attacker"); // BUG_RANGE, BUG_ATTACK_STRENGHT);
        }

        /// <summary>
        /// Registers Ants
        /// </summary>
        /// <param name="typeMapper">Type Mapper</param>
        /// <param name="settings">Settings</param>
        private void RegisterAnt(ITypeMapper typeMapper, Settings settings)
        {
            // Ant Item
            typeMapper.RegisterItem<AntItem, AntState, AntInfo>(this, "Ant");

            // Walking
            settings.Apply<AntItem>("MaxSpeed", 1f, "Maximum Speed of an Ant");
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
            settings.Apply<AntItem>("Mass", 1f, "Collision Mass of an Ant");
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
            settings.Apply<AntItem>("ViewRange", 20f, "View Range of an Ant");
            settings.Apply<AntItem>("ViewAngle", 360, "View Angle of an Ant");
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

            typeMapper.AttachItemProperty<AntItem, AttackableProperty>(this, "Ant Attackable"); // _settings.ANT_RADIUS, _settings.ANT_HITPOINTS, _settings.ANT_HITPOINTS);
            typeMapper.AttachItemProperty<AntItem, AttackerProperty>(this, "Ant Attacker"); // _settings.ANT_RANGE, _settings.ANT_ATTACK_STRENGHT, _settings.ANT_ATTACK_RECOVERY);
            typeMapper.AttachItemProperty<AntItem, CarrierProperty>(this, "Ant Carrier"); //  _settings.ANT_STRENGHT);
            typeMapper.AttachItemProperty<AntItem, CollectorProperty>(this, "Ant Collector"); // _settings.ANT_RANGE);
            typeMapper.AttachItemProperty<AntItem, SugarCollectableProperty>(this, "Ant Sugar Collectable"); // _settings.ANT_SUGAR_CAPACITY, 0);
            typeMapper.AttachItemProperty<AntItem, AppleCollectableProperty>(this, "Ant Apple Collectable"); // TODO: Optional, wenn _settings.ANT_APPLECOLLECT | _settings.ANT_APPLE_CAPACITY, 0);
        }
    }
}
