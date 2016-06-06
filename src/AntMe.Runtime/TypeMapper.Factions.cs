using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        /// <summary>
        /// Liefert eine Liste der registrierten Factions.
        /// </summary>
        public IEnumerable<ITypeMapperFactionEntry> Factions
        {
            get
            {
                return factions;
            }
        }

        /// <summary>
        /// Registriert eine neue Faction am System.
        /// </summary>
        /// <param name="extensionPack">Referenz auf die Extension</param>
        /// <param name="name">Name der Faction</param>
        /// <typeparam name="T">Typ der spezialisierten Faction</typeparam>
        /// <typeparam name="F">Typ der Fabric</typeparam>
        /// <typeparam name="U">Typ der Unit</typeparam>
        /// <typeparam name="S">Typ der State Klasse</typeparam>
        /// <typeparam name="I">Typ der Info Klasse</typeparam>
        /// <typeparam name="FI"></typeparam>
        /// <typeparam name="UI"></typeparam>
        /// <param name="createInfoDelegate"></param>
        /// <param name="createStateDelegate"></param>
        public void RegisterFaction<T, S, I, F, FI, U, UI>(IExtensionPack extensionPack, string name,
            Func<Faction, S> createStateDelegate = null,
            Func<Faction, Item, I> createInfoDelegate = null)
            where T : Faction
            where S : FactionState
            where I : FactionInfo
            where F : FactionFactory
            where FI : FactoryInterop
            where U : FactionUnit
            where UI : UnitInterop
        {
            // TODO: Null-Check und Vererbungscheck
            factions.Add(new FactionTypeMap()
            {
                ExtensionPack = extensionPack,
                Name = name,
                Type = typeof(T),
                StateType = typeof(S),
                InfoType = typeof(I),
                FactoryType = typeof(F),
                FactoryInteropType = typeof(FI),
                UnitType = typeof(U),
                UnitInteropType = typeof(UI),
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
            if (faction != null)
                return faction.Type.FullName;
            return null;
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
            // Extension prüfen
            if (extensionPack == null)
            {
                // tracer.Trace(TraceEventType.Critical, 19, "Extension Pack not set");
                throw new ArgumentNullException("extensionPack");
            }

            // Namen prüfen
            if (string.IsNullOrEmpty(name))
            {
                // tracer.Trace(TraceEventType.Critical, 18, "Name of Property not set");
                throw new ArgumentNullException("name");
            }

            // Type
            Type type = typeof(T);
            if (type.IsAbstract)
            {
                // tracer.Trace(TraceEventType.Critical, 20, "Property is abstract '{0}' ({1})", name, type.FullName);
                throw new ArgumentException("Type is abstract");
            }

            // Kollisionen prüfen
            if (factionProperties.Any(p => p.Type == type))
            {
                // tracer.Trace(TraceEventType.Critical, 21, "Property is already registered. '{0}' ({1})", name, type.FullName);
                throw new NotSupportedException("Property is already registered");
            }

            // TODO: Konstruktoren prüfen

            Type stateType = null;

            // Prüfen, ob State Type angegeben wurde.
            if (stateSet)
            {
                stateType = typeof(S);
                // tracer.Trace(TraceEventType.Information, 22, "Property contains State Property. '{0}' ({1})", name, stateType.FullName);

                // Abstract
                if (stateType.IsAbstract)
                {
                    // TODO: Tracer
                    throw new ArgumentException("State Type is abstract");
                }

                // Braucht einen parameterlosen Konstruktor
                if (stateType.GetConstructor(new Type[] { }) == null)
                {
                    // tracer.Trace(TraceEventType.Critical, 23, "State Property contains no parameterless Constructor. '{0}' ({1})", name, stateType.FullName);
                    throw new NotSupportedException("State Type has no parameterless Constructor");
                }

                // Braucht einen Konstruktor der das Item und das Item Property entgegen nimmt.
                if (stateType.GetConstructor(new Type[] { typeof(Faction), typeof(T) }) == null)
                {
                    // tracer.Trace(TraceEventType.Critical, 24, "State Property contains no Constructor with Property Type. '{0}' ({1})", name, stateType.FullName);
                    throw new NotSupportedException("State Type has no Constructor with Item Property Type");
                }
            }
            else
            {
                // tracer.Trace(TraceEventType.Information, 26, "Property contains no State Property. '{0}'", name);
            }

            // Check Info Type
            Type infoType = null;

            // Prüfen, ob Info Type angegeben wurde.
            if (infoSet)
            {
                infoType = typeof(I);
                // tracer.Trace(TraceEventType.Information, 25, "Property contains Info Property. '{0}' ({1})", name, infoType.FullName);

                // Auf Abstract prüfen
                if (infoType.IsAbstract)
                {
                    // TODO: Tracer
                    throw new ArgumentException("Info Type is abstract");
                }

                // Braucht einen Konstruktor mit Item Property 
                if (infoType.GetConstructor(new Type[] { typeof(Faction), typeof(T), typeof(Item) }) == null)
                {
                    // tracer.Trace(TraceEventType.Critical, 28, "Info Property does not contain a Constructor with Property Type and Observer Item. '{0}' ({1})", name, infoType.FullName);
                    throw new NotSupportedException("Info Type has no Constructor with Item Property and a Game Item");
                }
            }
            else
            {
                // tracer.Trace(TraceEventType.Information, 27, "Property contains no Info Property. '{0}'", name);
            }

            factionProperties.Add(new FactionPropertyTypeMap()
            {
                ExtensionPack = extensionPack,
                Name = name,
                Type = typeof(T),
                StateType = stateType,
                InfoType = infoType,
                CreateStateDelegate = createStateDelegate,
                CreateInfoDelegate = createInfoDelegate
            });

            // tracer.Trace(TraceEventType.Information, 29, "Property registered successful. '{0}'", name);
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
            // Extension prüfen
            if (extensionPack == null)
            {
                // TODO: Tracer
                throw new ArgumentNullException("extensionPack");
            }

            // Name prüfen
            if (string.IsNullOrEmpty(name))
            {
                // TODO: Tracer
                throw new ArgumentNullException("name");
            }

            if (!factionProperties.Any(c => c.Type == typeof(P)))
            {
                // TODO: Tracer
                throw new ArgumentException("Property is not registered");
            }

            if (factionAttachments.Any(c => c.Type == typeof(F) && c.AttachmentType == typeof(P)))
            {
                // TODO: Tracer
                throw new NotSupportedException("Item Property Combination is already reagistered");
            }

            if (createPropertyDelegate == null)
            {
                // TODO: Construktor prüfen
                if (typeof(P).GetConstructor(new Type[] { typeof(Faction) }) == null)
                {
                    // TODO: Tracer
                    throw new NotSupportedException("Property has no fitting Constructor.");
                }
            }

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
