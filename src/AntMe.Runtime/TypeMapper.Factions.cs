using System;
using System.Collections.Generic;
using System.Linq;

namespace AntMe.Runtime
{
    public partial class TypeMapper
    {
        #region Factions

        private class FactionTypeMap : StateInfoTypeMap<Func<Faction, FactionState>, Func<Faction, Item, FactionInfo>>, ITypeMapperFactionEntry
        {
            public Type FactoryType { get; set; }

            public Type UnitType { get; set; }

            public Type FactoryInteropType { get; set; }

            public Type UnitInteropType { get; set; }
        }

        private List<FactionTypeMap> factions = new List<FactionTypeMap>();

        public IEnumerable<ITypeMapperFactionEntry> Factions { get { return factions; } }

        public void RegisterFaction<T, S, I, F, FI, U, UI, IT>(IExtensionPack extensionPack, string name,
            Func<Faction, S> createStateDelegate = null,
            Func<Faction, Item, I> createInfoDelegate = null)
            where T : Faction
            where S : FactionState
            where I : FactionInfo
            where F : FactionFactory
            where FI : FactoryInterop
            where U : FactionUnit
            where UI : UnitInterop
            where IT : FactionItem
        {
            ValidateDefaults(extensionPack, name);

            // Handle Faction Type
            Type type = typeof(T);
            ValidateType<Faction>(type, new Type[] { typeof(SimulationContext), typeof(Type), typeof(Level) }, false);

            // TODO: Check Collisions

            // Handle Faction State
            Type stateType = typeof(S);
            ValidateType<FactionState>(stateType, new Type[] { typeof(T) }, true);

            // Handle Faction Info
            Type infoType = typeof(I);
            ValidateType<FactionInfo>(infoType, new Type[] { typeof(T), typeof(Item) }, false);

            // Handle Factory
            Type factoryType = typeof(F);
            // TODO: Anything to check here?

            // Handle Factory Interop
            Type factoryInteropType = typeof(FI);
            ValidateType<FactoryInterop>(factoryInteropType, new Type[] { typeof(T) }, false);

            // Handle Unit
            Type unitType = typeof(U);
            // TODO: Anything to check here?

            // Handle Unit Interop
            Type unitInteropType = typeof(UI);
            ValidateType<UnitInterop>(unitInteropType, new Type[] { typeof(T), typeof(IT) }, false);

            factions.Add(new FactionTypeMap()
            {
                ExtensionPack = extensionPack,
                Name = name,
                Type = type,
                StateType = stateType,
                InfoType = infoType,
                FactoryType = factoryType,
                FactoryInteropType = factoryInteropType,
                UnitType = unitType,
                UnitInteropType = unitInteropType,
                CreateStateDelegate = createStateDelegate,
                CreateInfoDelegate = createInfoDelegate
            });
        }



        /// <summary>
        /// Identifiziert die zugehörige Faction zum übergebenen Player Type.
        /// </summary>
        /// <param name="playerType">Typ des Spielers</param>
        /// <returns>Full Type Name</returns>
        public string LookupFactionName(Type playerType)
        {
            var faction = factions.FirstOrDefault(f => playerType.IsSubclassOf(f.FactoryType));
            return faction?.Type.FullName;
        }

        #endregion

        #region Faction Property

        private class FactionPropertyTypeMap : StateInfoTypeMap<Func<Faction, FactionProperty, FactionStateProperty>, Func<Faction, FactionProperty, Item, FactionInfoProperty>> { }

        private List<FactionPropertyTypeMap> factionProperties = new List<FactionPropertyTypeMap>();

        public IEnumerable<IStateInfoTypeMapperEntry> FactionProperties
        {
            get { return factionProperties; }
        }

        public void RegisterFactionProperty<T>(IExtensionPack extensionPack, string name)
            where T : FactionProperty
        {
            RegisterFactionProperty<T, FactionStateProperty, FactionInfoProperty>(extensionPack, name, false, false, null, null);
        }

        public void RegisterFactionPropertyS<T, S>(IExtensionPack extensionPack, string name, Func<Faction, FactionProperty, S> createStateDelegate = null)
            where T : FactionProperty
            where S : FactionStateProperty
        {
            RegisterFactionProperty<T, S, FactionInfoProperty>(extensionPack, name, true, false, createStateDelegate, null);
        }

        public void RegisterFactionPropertyI<T, I>(IExtensionPack extensionPack, string name, Func<Faction, FactionProperty, Item, I> createInfoDelegate = null)
            where T : FactionProperty
            where I : FactionInfoProperty
        {
            RegisterFactionProperty<T, FactionStateProperty, I>(extensionPack, name, false, true, null, createInfoDelegate);
        }

        public void RegisterFactionPropertySI<T, S, I>(IExtensionPack extensionPack, string name, Func<Faction, FactionProperty, S> createStateDelegate = null, Func<Faction, FactionProperty, Item, I> createInfoDelegate = null)
            where T : FactionProperty
            where S : FactionStateProperty
            where I : FactionInfoProperty
        {
            RegisterFactionProperty<T, S, I>(extensionPack, name, true, true, createStateDelegate, createInfoDelegate);
        }

        private void RegisterFactionProperty<T, S, I>(IExtensionPack extensionPack, string name,
            bool stateSet, bool infoSet,
            Func<Faction, FactionProperty, S> createStateDelegate = null,
            Func<Faction, FactionProperty, Item, I> createInfoDelegate = null)
            where T : FactionProperty
            where S : FactionStateProperty
            where I : FactionInfoProperty
        {
            ValidateDefaults(extensionPack, name);

            // Handle Type
            Type type = typeof(T);
            ValidateType<FactionProperty>(type, new Type[] { typeof(Faction) }, false);

            // Check Registration Collision
            if (factionProperties.Any(p => p.Type == type))
                throw new NotSupportedException("Property is already registered");

            // Handle State Type
            Type stateType = null;
            if (stateSet)
            {
                stateType = typeof(S);
                ValidateType<FactionStateProperty>(stateType, new Type[] { typeof(Faction), typeof(T) }, true);
            }

            // Handle Info Type
            Type infoType = null;
            if (infoSet)
            {
                infoType = typeof(I);
                ValidateType<FactionInfoProperty>(infoType, new Type[] { typeof(Faction), typeof(T), typeof(Item) }, false);
            }

            factionProperties.Add(new FactionPropertyTypeMap()
            {
                ExtensionPack = extensionPack,
                Name = name,
                Type = type,
                StateType = stateType,
                InfoType = infoType,
                CreateStateDelegate = createStateDelegate,
                CreateInfoDelegate = createInfoDelegate
            });
        }

        #endregion

        #region Faction Attachment Properties

        private class FactionAttachmentTypeMap : AttachmentTypeMap<Func<Faction, FactionProperty>> { }

        private List<FactionAttachmentTypeMap> factionAttachments = new List<FactionAttachmentTypeMap>();

        public IEnumerable<IAttachmentTypeMapperEntry> FactionAttachments { get { return factionAttachments; } }

        public void AttachFactionProperty<F, P>(IExtensionPack extensionPack, string name, Func<Faction, P> createPropertyDelegate = null)
            where F : Faction
            where P : FactionProperty
        {
            ValidateDefaults(extensionPack, name);

            if (!factionProperties.Any(c => c.Type == typeof(P)))
                throw new ArgumentException("Property is not registered");

            if (factionAttachments.Any(c => c.Type == typeof(F) && c.AttachmentType == typeof(P)))
                throw new NotSupportedException("Item Property Combination is already reagistered");

            factionAttachments.Add(new FactionAttachmentTypeMap()
            {
                ExtensionPack = extensionPack,
                Name = name,
                Type = typeof(F),
                AttachmentType = typeof(P),
                CreateDelegate = createPropertyDelegate
            });
        }

        #endregion

        #region Faction Extender

        private class FactionExtenderTypeMap : ExtenderTypeMap<Action<Faction>> { }

        private List<FactionExtenderTypeMap> factionExtender = new List<FactionExtenderTypeMap>();

        public IEnumerable<IRankedTypeMapperEntry> FactionExtender
        {
            get { return factionExtender; }
        }

        public void RegisterFactionExtender<T>(IExtensionPack extensionPack, string name, int rank, Action<Faction> extenderDelegate)
        {
            ValidateDefaults(extensionPack, name);

            // TODO: Check Stuff
            factionExtender.Add(new FactionExtenderTypeMap()
            {
                ExtensionPack = extensionPack,
                Name = name,
                Type = typeof(T),
                Rank = rank,
                ExtenderDelegate = extenderDelegate
            });
        }

        #endregion

        #region Faction Resolver

        /// <summary>
        /// Erstellt eine neue Faction auf Basis des übergebenen Factory Types oder null, 
        /// falls keine passende Faction gefunden werden konnte.
        /// </summary>
        /// <param name="context">Simulation Context</param>
        /// <param name="factoryType">Typ der Spieler Factory</param>
        /// <param name="level">Level</param>
        /// <returns>Neue Faction-Instanz</returns>
        public Faction CreateFaction(SimulationContext context, Type factoryType, Level level)
        {
            var faction = factions.FirstOrDefault(f => factoryType.IsSubclassOf(f.FactoryType));
            if (faction != null)
                return Activator.CreateInstance(faction.Type, context, factoryType, level) as Faction;
            return null;
        }

        public void ResolveFaction(Faction faction)
        {
            if (faction == null)
                throw new ArgumentNullException("Faction");

            // Sicherstellen, dass das Item registriert ist.
            if (!factions.Any(c => c.Type == faction.GetType()))
            {
                // TODO: Trace
                throw new NotSupportedException("Faction is not registered.");
            }

            // Vererbungskette auflösen
            List<Type> types = new List<Type>();
            Type current = faction.GetType();
            types.Add(current);
            while (current != typeof(Faction))
            {
                current = current.BaseType;
                types.Add(current);
            }
            Type[] factionTypes = types.ToArray();
            Array.Reverse(factionTypes);

            // Attachements
            foreach (var type in factionTypes)
            {
                foreach (var attachment in factionAttachments.Where(c => c.Type == type))
                {
                    if (attachment.CreateDelegate != null)
                    {
                        FactionProperty property = attachment.CreateDelegate(faction);
                        if (property != null)
                        {
                            if (property.GetType() != attachment.AttachmentType)
                            {
                                // TODO: Trace
                                throw new NotSupportedException("Delegate returned wrong Property Type");
                            }
                            faction.AddProperty(property);
                        }
                    }
                    else
                    {
                        faction.AddProperty(Activator.CreateInstance(attachment.AttachmentType, faction) as FactionProperty);
                    }
                }
            }

            // Extender
            foreach (var extender in factionExtender.Where(c => factionTypes.Contains(c.Type)).OrderBy(c => c.Rank))
            {
                extender.ExtenderDelegate(faction);
            }
        }

        public FactionState CreateFactionState(Faction faction)
        {
            if (faction == null)
                throw new ArgumentNullException("Faction");

            var container = factions.FirstOrDefault(g => g.Type == faction.GetType());
            if (container == null)
            {
                // TODO: Trace
                throw new ArgumentException("Faction is not registered");
            }

            FactionState state;
            if (container.CreateStateDelegate != null)
            {
                // Erstellung über Delegat
                state = container.CreateStateDelegate(faction);
                if (state == null)
                {
                    // TODO: Trace
                    throw new NotSupportedException("No state was returned by delegate.");
                }

                if (state.GetType() != container.StateType)
                {
                    // TODO: Trace
                    throw new NotSupportedException("delegate returned a wrong State Type");
                }
            }
            else
            {
                // Automatische Erstellung
                state = Activator.CreateInstance(container.StateType) as FactionState;
                if (state == null)
                {
                    // TODO: Trace
                    throw new Exception("State could not be created.");
                }
            }

            // Insert static Values
            state.FactionName = container.Name;
            state.Name = faction.Name;
            state.PlayerColor = faction.PlayerColor;
            state.SlotIndex = faction.SlotIndex;
            state.StartPoint = faction.Home;

            // State Properties auffüllen
            foreach (var property in faction.Properties)
            {
                var map = factionProperties.FirstOrDefault(p => p.Type == property.GetType());
                if (map == null)
                    throw new NotSupportedException("Property is not registered.");

                FactionStateProperty prop = null;
                if (map.CreateStateDelegate != null)
                {
                    // Option 1: Create Delegate
                    prop = map.CreateStateDelegate(faction, property);

                    if (prop != null)
                    {
                        if (prop.GetType() != map.StateType)
                        {
                            // TODO: Trace
                            throw new NotSupportedException("Delegate returned a wrong Property Type");
                        }

                        state.AddProperty(prop);
                    }
                }
                else if (map.StateType != null)
                {
                    // Option 2: Dynamische Erzeugung
                    prop = Activator.CreateInstance(map.StateType, faction, property) as FactionStateProperty;
                    if (prop == null)
                    {
                        // TODO: Trace
                        throw new Exception("Could not create State Property");
                    }
                    state.AddProperty(prop);
                }
            }

            return state;
        }

        public FactionInfo CreateFactionInfo(Faction faction, Item observer)
        {
            if (faction == null)
                throw new ArgumentNullException("Faction");

            if (observer == null)
                throw new ArgumentNullException("observer");

            var container = factions.FirstOrDefault(g => g.Type == faction.GetType());
            if (container == null)
            {
                // TODO: Trace
                throw new ArgumentException("Faction is not registered");
            }

            FactionInfo info = null;
            if (container.CreateInfoDelegate != null)
            {
                info = container.CreateInfoDelegate(faction, observer);
                if (info == null)
                {
                    // TODO: Trace
                    throw new NotSupportedException("No info was returned by delegate.");
                }

                if (info.GetType() != container.StateType)
                {
                    // TODO: Trace
                    throw new NotSupportedException("delegate returned a wrong Info Type");
                }
            }
            else
            {
                info = Activator.CreateInstance(container.InfoType, faction, observer) as FactionInfo;
                if (info == null)
                {
                    // TODO: Trace
                    throw new Exception("Info could not be created.");
                }
            }

            // Info-Properties in Abhängigkeit der Item-Props
            foreach (var property in faction.Properties)
            {
                var map = factionProperties.FirstOrDefault(p => p.Type == property.GetType());
                if (map == null)
                    throw new NotSupportedException("Property is not registered.");

                FactionInfoProperty prop = null;
                if (map.CreateInfoDelegate != null)
                {
                    // Option 1: Create Delegate
                    prop = map.CreateInfoDelegate(faction, property, observer);

                    if (prop != null)
                    {
                        if (prop.GetType() != map.InfoType)
                        {
                            // TODO: Tracing
                            throw new NotSupportedException("Create Delegate returned a wrong type");
                        }

                        info.AddProperty(prop);
                    }
                }
                else if (map.InfoType != null)
                {
                    // Option 2: Automatische Erstellung
                    prop = Activator.CreateInstance(map.InfoType, faction, property, observer) as FactionInfoProperty;
                    if (prop == null)
                    {
                        // TODO: Trace
                        throw new Exception("Could not create Info Property");
                    }
                    info.AddProperty(prop);
                }

                // TODO: Logging falls Properties nicht gefunden werden
            }


            return info;
        }

        #endregion
    }
}
