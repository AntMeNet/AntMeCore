using AntMe.EngineExtensions.Basics;
using AntMe.Factions.Ants;
using AntMe.Factions.Bugs;
using AntMe.ItemProperties.Basics;
using AntMe.Items.Basics;
using System;

namespace AntMe.Extension.Basics
{
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

            // Apple
            typeMapper.RegisterItem<AppleItem, AppleState, AppleInfo>(this, "Apple");
            typeMapper.AttachItemProperty<AppleItem, CollidableProperty>(this, "Apple Collidable");
            typeMapper.AttachItemProperty<AppleItem, PortableProperty>(this, "Apple Portable");
            typeMapper.AttachItemProperty<AppleItem, VisibleProperty>(this, "Apple Visible");
            typeMapper.AttachItemProperty<AppleItem, CollectableProperty>(this, "Apple Collectable");
            typeMapper.AttachItemProperty<AppleItem, AppleCollectableProperty>(this, "Apple Collectable"); // TODO: Amounts (amount))

            // Sugar
            typeMapper.RegisterItem<SugarItem, SugarState, SugarInfo>(this, "Sugar");
            typeMapper.AttachItemProperty<SugarItem, CollidableProperty>(this, "Sugar Collidable");
            typeMapper.AttachItemProperty<SugarItem, VisibleProperty>(this, "Sugar Visible");
            typeMapper.AttachItemProperty<SugarItem, CollectableProperty>(this, "Sugar Collectable");
            typeMapper.AttachItemProperty<SugarItem, SugarCollectableProperty>(this, "Sugar Collectable"); // TODO: Amounts (SugarMaxCapacity, Math.Min(SugarMaxCapacity, amount))

            // Anthill
            typeMapper.RegisterItem<AnthillItem, AnthillState, AnthillInfo>(this, "Anthill");
            typeMapper.AttachItemProperty<AnthillItem, CollidableProperty>(this, "Anthill Collidable");
            typeMapper.AttachItemProperty<AnthillItem, AttackableProperty>(this, "Anthill Attackable"); // TODO: Optional, wenn Settings angreifbar sind
            typeMapper.AttachItemProperty<AnthillItem, CollectableProperty>(this, "Anthill Collectable"); // TODO: Radius, Vermutlich entfernen
            typeMapper.AttachItemProperty<AnthillItem, SugarCollectableProperty>(this, "Anthill Sugarsafe"); // TODO: Radius
            typeMapper.AttachItemProperty<AnthillItem, AppleCollectableProperty>(this, "Anthill Applesafe"); // TODO: Radius
            typeMapper.AttachItemProperty<AnthillItem, VisibleProperty>(this, "Anthill Visible");

            // Marker
            typeMapper.RegisterItem<MarkerItem, MarkerState, MarkerInfo>(this, "Marker");
            typeMapper.AttachItemProperty<MarkerItem, SmellableProperty>(this, "Marker Smellable"); // TODO: Radius MARKER_MINIMUM_RADIUS

            // Ant
            typeMapper.RegisterItem<AntItem, AntState, AntInfo>(this, "Ant");
            typeMapper.AttachItemProperty<AntItem, VisibleProperty>(this, "Ant Visible");
            typeMapper.AttachItemProperty<AntItem, CollidableProperty>(this, "Ant Collidable", (i) =>
            {
                return new CollidableProperty(i, (i as AntItem).Faction.Settings.GetFloat<AntItem>("Mass") ?? 0f);
            });
            typeMapper.AttachItemProperty<AntItem, SnifferProperty>(this, "Ant Sniffer");
            typeMapper.AttachItemProperty<AntItem, WalkingProperty>(this, "Ant Walking"); // _settings.ANT_MAX_SPEED) {Direction = orientation};
            typeMapper.AttachItemProperty<AntItem, SightingProperty>(this, "Ant Sighting"); // _settings.ANT_VIEW_RANGE, _settings.ANT_VIEW_ANGLE, orientation);
            typeMapper.AttachItemProperty<AntItem, AttackableProperty>(this, "Ant Attackable"); // _settings.ANT_RADIUS, _settings.ANT_HITPOINTS, _settings.ANT_HITPOINTS);
            typeMapper.AttachItemProperty<AntItem, AttackerProperty>(this, "Ant Attacker"); // _settings.ANT_RANGE, _settings.ANT_ATTACK_STRENGHT, _settings.ANT_ATTACK_RECOVERY);
            typeMapper.AttachItemProperty<AntItem, CarrierProperty>(this, "Ant Carrier"); //  _settings.ANT_STRENGHT);
            typeMapper.AttachItemProperty<AntItem, CollectorProperty>(this, "Ant Collector"); // _settings.ANT_RANGE);
            typeMapper.AttachItemProperty<AntItem, SugarCollectableProperty>(this, "Ant Sugar Collectable"); // _settings.ANT_SUGAR_CAPACITY, 0);
            typeMapper.AttachItemProperty<AntItem, AppleCollectableProperty>(this, "Ant Apple Collectable"); // TODO: Optional, wenn _settings.ANT_APPLECOLLECT | _settings.ANT_APPLE_CAPACITY, 0);

            // Bug
            typeMapper.RegisterItem<BugItem, BugState, BugInfo>(this, "Bug");
            typeMapper.AttachItemProperty<BugItem, WalkingProperty>(this, "Bug Walking"); // BUG_MAX_SPEED);
            typeMapper.AttachItemProperty<BugItem, CollidableProperty>(this, "Bug Collidable"); //  BUG_RADIUS);
            typeMapper.AttachItemProperty<BugItem, SightingProperty>(this, "Bug Sighting"); // BUG_VIEWRANGE);
            typeMapper.AttachItemProperty<BugItem, SnifferProperty>(this, "Bug Sniffer");
            typeMapper.AttachItemProperty<BugItem, VisibleProperty>(this, "Bug Visible"); //  BUG_RADIUS);
            typeMapper.AttachItemProperty<BugItem, AttackableProperty>(this, "Bug Attackable"); //  BUG_RADIUS, BUG_HITPOINTS, BUG_HITPOINTS);
            typeMapper.AttachItemProperty<BugItem, AttackerProperty>(this, "Bug Attacker"); // BUG_RANGE, BUG_ATTACK_STRENGHT);
            typeMapper.AttachItemProperty<BugItem, CollectorProperty>(this, "Bug Collector"); // BUG_RANGE);
            typeMapper.AttachItemProperty<BugItem, SugarCollectableProperty>(this, "Bug Sugar Collectable"); // , BUG_SUGAR_CAPACITY, 0);
            typeMapper.AttachItemProperty<BugItem, AppleCollectableProperty>(this, "Bug Apple Collectable"); // BUG_APPLE_CAPACITY, 0);

            // ##########################
            // Settings
            // ##########################

            
        }
    }
}
