using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace AntMe.Runtime
{
    /// <summary>
    /// Repository for all extending Types within AntMe!
    /// </summary>
    public class TypeMapper : ITypeMapper, ITypeResolver
    {
        // Last ID: 31
        private Tracer tracer = new Tracer("AntMe.Simulation.TypeMapper");

        /// <summary>
        /// Neue Instanz des Type Mappers.
        /// </summary>
        public TypeMapper()
        {
            tracer.Trace(TraceEventType.Information, 1, "TypeMapper started");
        }

        #region Management

        /// <summary>
        /// Removes all Extension Elements with the given Extension Pack Source.
        /// </summary>
        /// <param name="extensionPack">Extension Pack to remove</param>
        public void RemoveExtensionPack(IExtensionPack extensionPack)
        {
            // Engine Properties
            foreach (var item in enginePropertyContainer.Where(c => c.ExtensionPack == extensionPack).ToArray())
                enginePropertyContainer.Remove(item);

            // Items
            foreach (var item in itemsContainer.Where(c => c.ExtensionPack == extensionPack).ToArray())
                itemsContainer.Remove(item);

            // Item Properties
            foreach (var item in itemProperties.Where(c => c.ExtensionPack == extensionPack).ToArray())
                itemProperties.Remove(item);

            // Item Attachement Properties
            foreach (var item in itemAttachments.Where(c => c.ExtensionPack == extensionPack).ToArray())
                itemAttachments.Remove(item);

            // Item Extender
            foreach (var item in itemExtender.Where(c => c.ExtensionPack == extensionPack).ToArray())
                itemExtender.Remove(item);

            // Factions
            foreach (var item in factions.Where(c => c.ExtensionPack == extensionPack).ToArray())
                factions.Remove(item);


            // Faction Properties
            foreach (var item in factionProperties.Where(c => c.ExtensionPack == extensionPack).ToArray())
                factionProperties.Remove(item);


            // Faction Attachement Properites
            foreach (var item in factionAttachments.Where(c => c.ExtensionPack == extensionPack).ToArray())
                factionAttachments.Remove(item);

            // Faction Extender
            foreach (var item in factionExtender.Where(c => c.ExtensionPack == extensionPack).ToArray())
                factionExtender.Remove(item);

            // Interop Attachment Properties
            foreach (var item in factoryInteropAttachments.Where(c => c.ExtensionPack == extensionPack).ToArray())
                factoryInteropAttachments.Remove(item);
            foreach (var item in unitInteropAttachments.Where(c => c.ExtensionPack == extensionPack).ToArray())
                unitInteropAttachments.Remove(item);

            // Interop Extender
            foreach (var item in factoryInteropExtender.Where(c => c.ExtensionPack == extensionPack).ToArray())
                factoryInteropExtender.Remove(item);
            foreach (var item in unitInteropExtender.Where(c => c.ExtensionPack == extensionPack).ToArray())
                unitInteropExtender.Remove(item);

            // Level Properties
            foreach (var item in levelProperties.Where(c => c.ExtensionPack == extensionPack).ToArray())
                levelProperties.Remove(item);

            // Level Extender
            foreach (var item in levelExtender.Where(c => c.ExtensionPack == extensionPack).ToArray())
                levelExtender.Remove(item);
        }

        #endregion

        #region Type Maps

        /// <summary>
        /// Default Container for all Mapping Elements.
        /// </summary>
        private class TypeMap : ITypeMapperEntry
        {
            /// <summary>
            /// Reference to the source Extension Pack.
            /// </summary>
            public IExtensionPack ExtensionPack { get; set; }

            /// <summary>
            /// Name of the Mapping Element.
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Type of the Mapping Element.
            /// </summary>
            public Type Type { get; set; }
        }

        /// <summary>
        /// Container for all Extender Mapping Elements.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        private class ExtenderTypeMap<T> : TypeMap, IRankedTypeMapperEntry
        {
            /// <summary>
            /// Delegate to extend the Type.
            /// </summary>
            public T ExtenderDelegate { get; set; }

            /// <summary>
            /// Extender Rank.
            /// </summary>
            public int Rank { get; set; }
        }

        private class StateInfoTypeMap<CS, CI> : TypeMap, IStateInfoTypeMapperEntry
        {
            public Type StateType { get; set; }

            public Type InfoType { get; set; }

            public CS CreateStateDelegate { get; set; }

            public CI CreateInfoDelegate { get; set; }
        }

        private class AttachmentTypeMap<T> : TypeMap, IAttachmentTypeMapperEntry
        {
            public Type AttachmentType { get; set; }

            public T CreateDelegate { get; set; }
        }

        #endregion

        #region Engine Properties

        private class EnginePropertyContainer : ExtenderTypeMap<Func<Engine, EngineProperty>> { }

        private readonly List<EnginePropertyContainer> enginePropertyContainer =
            new List<EnginePropertyContainer>();

        /// <summary>
        /// Listet alle registrierten Extensions auf.
        /// </summary>
        public IEnumerable<IRankedTypeMapperEntry> EngineProperties
        {
            get
            {
                tracer.Trace(TraceEventType.Information, 17, "Requested Engine Property List");
                return enginePropertyContainer.OrderBy(e => e.Rank);
            }
        }

        /// <summary>
        /// Registriert eine Engine Extension. Die Extension braucht entweder einen Konstruktor mit 
        /// den Parametern (Engine) oder einen Create Delegaten.
        /// </summary>
        /// <param name="extensionPack">Referenz auf den verantwortlichen Extension Pack</param>
        /// <param name="name">Name der Extension</param>
        /// <param name="rank">Rang der Extension</param>
        /// <param name="createExtensionDelegate">Delegat zum Erstellen einer neuen Instanz</param>
        /// <typeparam name="T">Extension Type</typeparam>
        public void RegisterEngineProperty<T>(IExtensionPack extensionPack, string name, int rank, Func<Engine, T> createExtensionDelegate = null) where T : EngineProperty
        {
            Type t = typeof(T);

            tracer.Trace(TraceEventType.Information, 3, "Try to register new Extension '{0}' ({1})", name, t.FullName);

            // Extension Pack darf nicht null sein.
            if (extensionPack == null)
            {
                tracer.Trace(TraceEventType.Critical, 4, "Extension Pack is null ({0})", t.FullName);
                throw new ArgumentNullException("extensionPack");
            }

            // Existenz eines Namens sicher stellen
            if (string.IsNullOrEmpty(name))
            {
                tracer.Trace(TraceEventType.Critical, 4, "Extension Name is not set ({0})", t.FullName);
                throw new ArgumentNullException("name");
            }

            // Instaniierbarkeit prüfen
            if (t.IsAbstract)
            {
                tracer.Trace(TraceEventType.Critical, 5, "Extension is abstract '{0}' ({1})", name, t.FullName);
                throw new NotSupportedException("Extension is abstract");
            }

            // Collision checken
            if (enginePropertyContainer.Any(e => e.Type == t))
            {
                tracer.Trace(TraceEventType.Critical, 6, "Extension is already registered '{0}' ({1})", name, t.FullName);
                throw new ArgumentException("This Extension is already registered");
            }

            if (createExtensionDelegate == null)
            {
                tracer.Trace(TraceEventType.Information, 7, "Extension contains no CreateDelegate");

                // Der Standard-Prozess erwartet einen mit der Struktur ctor(Engine)
                if (t.GetConstructor(new[] { typeof(Engine) }) == null)
                {
                    tracer.Trace(TraceEventType.Critical, 9, "Extension has no empty Constructor '{0}' ({1})", name, t.FullName);
                    throw new NotSupportedException(string.Format("Extension has no empty Constructor '{0}' ({1})", name, t.FullName));
                }
            }
            else
            {
                tracer.Trace(TraceEventType.Information, 8, "Extension contains CreateDelegate");
            }

            EnginePropertyContainer container = new EnginePropertyContainer()
            {
                ExtensionPack = extensionPack,
                Name = name,
                Rank = rank,
                Type = t,
                ExtenderDelegate = createExtensionDelegate
            };

            tracer.Trace(TraceEventType.Information, 2, "Register Engine Extension succeeded '{0}' ({1})", name, t.FullName);

            enginePropertyContainer.Add(container);
        }

        #endregion

        #region Items

        private class ItemTypeMap : StateInfoTypeMap<Func<Item, ItemState>, Func<Item, Item, ItemInfo>> { }

        private List<ItemTypeMap> itemsContainer =
            new List<ItemTypeMap>();

        /// <summary>
        /// Liefert eine Liste registrierter Game Items.
        /// </summary>
        public IEnumerable<IStateInfoTypeMapperEntry> Items { get { return itemsContainer; } }

        /// <summary>
        /// Registriert einen Game Type beim Type Mapper.
        /// </summary>
        /// <param name="extensionPack">Extension Pack</param>
        /// <param name="name">Name des Game Items</param>
        /// <param name="createStateDelegate">Delegat zur manuellen Erstellung eines States</param>
        /// <param name="createInfoDelegate">Delegat zur manuellen Erstellung eines Info Objektes</param>
        /// <typeparam name="T">Type des Types</typeparam>
        /// <typeparam name="S">Dazugehöriger State Type</typeparam>
        /// <typeparam name="I">Dazugehöriger Info Type</typeparam>
        public void RegisterItem<T, S, I>(IExtensionPack extensionPack, string name,
            Func<Item, S> createStateDelegate = null,
            Func<Item, Item, I> createInfoDelegate = null)
            where T : Item
            where S : ItemState
            where I : ItemInfo
        {
            Type itemType = typeof(T);
            Type stateType = typeof(S);
            Type infoType = typeof(I);

            tracer.Trace(TraceEventType.Information, 30, string.Format("Register new Item Type '{0}'", itemType.FullName));

            // Extension Pack darf nicht null sein
            if (extensionPack == null)
            {
                tracer.Trace(TraceEventType.Critical, 31, "Extension Pack is null '{0}'", itemType.FullName);
                throw new ArgumentNullException("extensionPack");
            }

            // Text darf nicht leer sein
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            // Prüft auf abstract
            if (itemType.IsAbstract)
            {
                string msg = string.Format("Item Type '{0}' from Extension Pack '{1}' is abstract", itemType.FullName, extensionPack.Name);
                throw new NotSupportedException(msg);
            }

            // Prüfen ob dieser Type bereits registriert ist
            if (itemsContainer.Any(c => c.Type == itemType))
            {
                string msg = string.Format("Item '{0}' from Extension Pack '{1}' is already registered", itemType.FullName, extensionPack.Name);
                throw new NotSupportedException(msg);
            }

            if (stateType.IsAbstract)
            {
                string msg = string.Format("State Type '{0}' from Extension Pack '{1}' is abstract", stateType.FullName, extensionPack.Name);
                throw new NotSupportedException(msg);
            }

            // Check for Parameterless Constructor
            if (stateType.GetConstructor(new Type[] { }) == null)
            {
                string msg = string.Format("The State '{0}' from Extension Pack '{1}' has no parameterless Constructor", stateType.FullName, extensionPack.Name);
                throw new NotSupportedException(msg);
            }

            // Check for Constructor with T
            if (stateType.GetConstructor(new Type[] { typeof(T) }) == null)
            {
                string msg = string.Format("The State '{0}' from Extension Pack '{1}' has no Constructor with Item", stateType.FullName, extensionPack.Name);
                throw new NotSupportedException(msg);
            }

            if (infoType.IsAbstract)
            {
                string msg = string.Format("Info Type '{0}' from Extension Pack '{1}' is abstract", infoType.FullName, extensionPack.Name);
                throw new NotSupportedException(msg);
            }

            // Check for Item/Item Constructor
            if (infoType.GetConstructor(new Type[] { typeof(T), typeof(Item) }) == null)
            {
                string msg = string.Format("The Info Type '{0}' from Extension Pack '{1}' has no Constructor with T and Item", infoType.FullName, extensionPack.Name);
                throw new NotSupportedException(msg);
            }

            itemsContainer.Add(new ItemTypeMap()
            {
                ExtensionPack = extensionPack,
                Name = name,
                Type = itemType,
                StateType = stateType,
                InfoType = infoType,
                CreateStateDelegate = createStateDelegate,
                CreateInfoDelegate = createInfoDelegate
            });
        }

        #endregion

        #region Item Properties

        private class ItemPropertyTypeMap : StateInfoTypeMap<Func<Item, ItemProperty, ItemStateProperty>, Func<Item, ItemProperty, Item, ItemInfoProperty>> { }

        private List<ItemPropertyTypeMap> itemProperties = new List<ItemPropertyTypeMap>();

        /// <summary>
        /// Liefert eine Liste aller registrierten Properties zurück.
        /// </summary>
        public IEnumerable<IStateInfoTypeMapperEntry> ItemProperties
        {
            get
            {
                tracer.Trace(TraceEventType.Information, 29, "Requested List of Properties");
                return itemProperties;
            }
        }

        /// <summary>
        /// Registriert ein Item Property das keine State- oder Info-Properties besitzt.
        /// </summary>
        /// <typeparam name="T">Type des Property</typeparam>
        /// <param name="extensionPack">Referenz auf den Extension Pack</param>
        /// <param name="name">Name des Properties</param>
        public void RegisterItemProperty<T>(IExtensionPack extensionPack, string name)
            where T : ItemProperty
        {
            RegisterItemProperty<T, ItemStateProperty, ItemInfoProperty>(extensionPack, name, false, false, null, null);
        }

        /// <summary>
        /// Registriert ein Item Property das nur ein State Property hat.
        /// </summary>
        /// <typeparam name="T">Type des Property</typeparam>
        /// <typeparam name="S">Type des State Properties</typeparam>
        /// <param name="extensionPack">Referenz auf den Extension Pack</param>
        /// <param name="name">Name des Properties</param>
        /// <param name="createStateDelegate">Optionaler Delegat zur Erzeugung des States.</param>
        public void RegisterItemPropertyS<T, S>(IExtensionPack extensionPack, string name,
            Func<Item, ItemProperty, S> createStateDelegate = null)
            where T : ItemProperty
            where S : ItemStateProperty
        {
            RegisterItemProperty<T, S, ItemInfoProperty>(extensionPack, name, true, false, createStateDelegate, null);
        }

        /// <summary>
        /// Registriert ein Item Property das nur ein Info Property hat.
        /// </summary>
        /// <typeparam name="T">Type des Property</typeparam>
        /// <typeparam name="I">Type des Info Property</typeparam>
        /// <param name="extensionPack">Referenz auf den Extension Pack</param>
        /// <param name="name">Name des Properties</param>
        /// <param name="createInfoDelegate">Optionaler Delegat zur Erzeugung des Info Properties.</param>
        public void RegisterItemPropertyI<T, I>(IExtensionPack extensionPack, string name,
            Func<Item, ItemProperty, Item, I> createInfoDelegate = null)
            where T : ItemProperty
            where I : ItemInfoProperty
        {
            RegisterItemProperty<T, ItemStateProperty, I>(extensionPack, name, false, true, null, createInfoDelegate);
        }

        /// <summary>
        /// Registriert ein Item Property mit samt den State- und Info-Properties
        /// </summary>
        /// <param name="extensionPack">Referenz auf den Extension Pack</param>
        /// <param name="name">Name des Properties</param>
        /// <param name="createStateDelegate">Delegate zum Erstellen eines neuen State Properties</param>
        /// <param name="createInfoDelegate">Delegate zum Erstellen eines neuen Info Properties. Parameter sind das eigentliche Game Item, das Property und der Observer</param>
        /// <typeparam name="T">Type des Property</typeparam>
        /// <typeparam name="S">Type des State Property</typeparam>
        /// <typeparam name="I">Type des Info Property</typeparam>
        public void RegisterItemPropertySI<T, S, I>(IExtensionPack extensionPack, string name,
            Func<Item, ItemProperty, S> createStateDelegate = null,
            Func<Item, ItemProperty, Item, I> createInfoDelegate = null)
            where T : ItemProperty
            where S : ItemStateProperty
            where I : ItemInfoProperty
        {
            RegisterItemProperty<T, S, I>(extensionPack, name, true, true, createStateDelegate, createInfoDelegate);
        }

        private void RegisterItemProperty<T, S, I>(IExtensionPack extensionPack, string name,
            bool stateSet, bool infoSet,
            Func<Item, ItemProperty, S> createStateDelegate = null,
            Func<Item, ItemProperty, Item, I> createInfoDelegate = null)
            where T : ItemProperty
            where S : ItemStateProperty
            where I : ItemInfoProperty
        {
            // Extension prüfen
            if (extensionPack == null)
            {
                tracer.Trace(TraceEventType.Critical, 19, "Extension Pack not set");
                throw new ArgumentNullException("extensionPack");
            }

            // Namen prüfen
            if (string.IsNullOrEmpty(name))
            {
                tracer.Trace(TraceEventType.Critical, 18, "Name of Property not set");
                throw new ArgumentNullException("name");
            }

            // Type
            Type type = typeof(T);
            if (type.IsAbstract)
            {
                tracer.Trace(TraceEventType.Critical, 20, "Property is abstract '{0}' ({1})", name, type.FullName);
                throw new ArgumentException("Type is abstract");
            }

            // Kollisionen prüfen
            if (itemProperties.Any(p => p.Type == type))
            {
                tracer.Trace(TraceEventType.Critical, 21, "Property is already registered. '{0}' ({1})", name, type.FullName);
                throw new NotSupportedException("Property is already registered");
            }

            // Konstruktoren prüfen
            if (type.GetConstructor(new Type[] { typeof(Item) }) == null)
            {
                string msg = string.Format("Property contains no Constructor with Item. '{0}' ({1})", name, type.FullName);
                tracer.Trace(TraceEventType.Critical, 23, msg);
                throw new NotSupportedException(msg);
            }

            Type stateType = null;

            // Prüfen, ob State Type angegeben wurde.
            if (stateSet)
            {
                stateType = typeof(S);
                tracer.Trace(TraceEventType.Information, 22, "Property contains State Property. '{0}' ({1})", name, stateType.FullName);

                // Abstract
                if (stateType.IsAbstract)
                {
                    // TODO: Tracer
                    string msg = string.Format("State Type '{0}' from Extension Pack '{1}' is abstract", stateType.FullName, extensionPack.Name);
                    throw new ArgumentException(msg);
                }

                // Braucht einen parameterlosen Konstruktor
                if (stateType.GetConstructor(new Type[] { }) == null)
                {
                    string msg = string.Format("State Property contains no parameterless Constructor. '{0}' ({1})", name, stateType.FullName);
                    tracer.Trace(TraceEventType.Critical, 23, msg);
                    throw new NotSupportedException(msg);
                }

                // Braucht einen Konstruktor der das Item und das Item Property entgegen nimmt.
                if (stateType.GetConstructor(new Type[] { typeof(Item), typeof(T) }) == null)
                {
                    string msg = string.Format("State Property contains no Constructor with Property Type. '{0}' ({1})", name, stateType.FullName);
                    tracer.Trace(TraceEventType.Critical, 24, msg);
                    throw new NotSupportedException(msg);
                }
            }
            else
            {
                tracer.Trace(TraceEventType.Information, 26, "Property contains no State Property. '{0}'", name);
            }

            // Check Info Type
            Type infoType = null;

            // Prüfen, ob Info Type angegeben wurde.
            if (infoSet)
            {
                infoType = typeof(I);
                tracer.Trace(TraceEventType.Information, 25, "Property contains Info Property. '{0}' ({1})", name, infoType.FullName);

                // Auf Abstract prüfen
                if (infoType.IsAbstract)
                {
                    // TODO: Tracer
                    string msg = string.Format("Info Type '{0}' from Extension Pack '{1}' is abstract", infoType.FullName, extensionPack.Name);
                    throw new ArgumentException(msg);
                }

                // Braucht einen Konstruktor mit Item Property 
                if (infoType.GetConstructor(new Type[] { typeof(Item), typeof(T), typeof(Item) }) == null)
                {
                    string msg = string.Format("Info Property does not contain a Constructor with Property Type and Observer Item. '{0}' ({1})", name, infoType.FullName);
                    tracer.Trace(TraceEventType.Critical, 28, msg);
                    throw new NotSupportedException(msg);
                }
            }
            else
            {
                tracer.Trace(TraceEventType.Information, 27, "Property contains no Info Property. '{0}'", name);
            }

            itemProperties.Add(new ItemPropertyTypeMap()
            {
                ExtensionPack = extensionPack,
                Name = name,
                Type = typeof(T),
                StateType = stateType,
                InfoType = infoType,
                CreateStateDelegate = createStateDelegate,
                CreateInfoDelegate = createInfoDelegate
            });

            tracer.Trace(TraceEventType.Information, 29, "Property registered successful. '{0}'", name);
        }

        #endregion

        #region Item Attachment Properties

        private class ItemAttchmentTypeMap : AttachmentTypeMap<Func<Item, ItemProperty>> { }

        private List<ItemAttchmentTypeMap> itemAttachments = new List<ItemAttchmentTypeMap>();

        /// <summary>
        /// Auflistung aller registrierten Item Attachments.
        /// </summary>
        public IEnumerable<IAttachmentTypeMapperEntry> ItemAttachments
        {
            get { return itemAttachments; }
        }

        /// <summary>
        /// Hängt ein definiertes Property an ein Item an.
        /// </summary>
        /// <typeparam name="I">Item</typeparam>
        /// <typeparam name="P">Property</typeparam>
        /// <param name="extensionPack"></param>
        /// <param name="name"></param>
        /// <param name="createPropertyDelegate"></param>
        public void AttachItemProperty<I, P>(IExtensionPack extensionPack, string name, Func<Item, P> createPropertyDelegate = null)
            where I : Item
            where P : ItemProperty
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

            if (!itemProperties.Any(c => c.Type == typeof(P)))
            {
                // TODO: Tracer
                throw new ArgumentException("Property is not registered");
            }

            if (itemAttachments.Any(c => c.Type == typeof(I) && c.AttachmentType == typeof(P)))
            {
                // TODO: Tracer
                string msg = string.Format("Item Property Combination '{0}'/'{1}' is already reagistered", typeof(I).FullName, typeof(P).FullName);
                throw new NotSupportedException(msg);
            }

            if (createPropertyDelegate == null)
            {
                // TODO: Construktor prüfen
                if (typeof(P).GetConstructor(new Type[] { typeof(Item) }) == null)
                {
                    // TODO: Tracer
                    string msg = string.Format("Property '{0}' has no fitting Constructor.", typeof(P).FullName);
                    throw new NotSupportedException(msg);
                }
            }

            itemAttachments.Add(new ItemAttchmentTypeMap()
            {
                ExtensionPack = extensionPack,
                Name = name,
                Type = typeof(I),
                AttachmentType = typeof(P),
                CreateDelegate = createPropertyDelegate
            });
        }

        #endregion

        #region Item Extender

        private class ItemExtenderTypeMap : ExtenderTypeMap<Action<Item>> { }

        private List<ItemExtenderTypeMap> itemExtender = new List<ItemExtenderTypeMap>();

        /// <summary>
        /// Liefert eine priorisierte Liste der Extender zum angegebenen Game Item Type zurück.
        /// </summary>
        /// <typeparam name="T">Game Item Type</typeparam>
        /// <returns>Liste der Extender</returns>
        public IEnumerable<IRankedTypeMapperEntry> ItemExtender
        {
            get
            {
                return itemExtender.OrderBy(g => g.Rank);
            }
        }

        /// <summary>
        /// Registriert einen Delegaten zur Erweiterung des angegebenen Item Types.
        /// </summary>
        /// <param name="extensionPack"></param>
        /// <param name="name">Name der Erweiterung</param>
        /// <param name="rank">Priorität</param>
        /// <typeparam name="T">Item Type für den der Extender gilt</typeparam>
        /// <param name="extenderDelegate">Delegat</param>
        public void RegisterItemExtender<T>(IExtensionPack extensionPack, string name, Action<Item> extenderDelegate, int rank)
            where T : Item
        {
            // Kein Name angegeben
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            // Kein Delegat angegeben
            if (extenderDelegate == null)
                throw new ArgumentNullException("extenderDelegate");

            itemExtender.Add(new ItemExtenderTypeMap()
            {
                ExtensionPack = extensionPack,
                Name = name,
                Type = typeof(T),
                Rank = rank,
                ExtenderDelegate = extenderDelegate
            });
        }

        #endregion

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

        #region Interop Attachment Properties

        private class FactoryAttchmentTypeMap : AttachmentTypeMap<Func<Faction, FactoryInterop, FactoryInteropProperty>> { }

        private class UnitAttchmentTypeMap : AttachmentTypeMap<Func<UnitInterop, UnitInteropProperty>> { }

        private List<FactoryAttchmentTypeMap> factoryInteropAttachments = new List<FactoryAttchmentTypeMap>();

        private List<UnitAttchmentTypeMap> unitInteropAttachments = new List<UnitAttchmentTypeMap>();

        public IEnumerable<IAttachmentTypeMapperEntry> FactoryInteropAttachments { get { return factoryInteropAttachments; } }

        public IEnumerable<IAttachmentTypeMapperEntry> UnitInteropAttachments { get { return unitInteropAttachments; } }

        /// <summary>
        /// Hängt ein Property an eine gegebene Factory Interop an.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="P"></typeparam>
        /// <param name="extensionPack"></param>
        /// <param name="name"></param>
        /// <param name="createPropertyDelegate"></param>
        public void AttachFactoryInteropProperty<T, P>(IExtensionPack extensionPack, string name,
            Func<Faction, FactoryInterop, P> createPropertyDelegate = null)
            where T : FactoryInterop
            where P : FactoryInteropProperty
        {
            // TODO: Prüfungen
            factoryInteropAttachments.Add(new FactoryAttchmentTypeMap()
            {
                ExtensionPack = extensionPack,
                Name = name,
                Type = typeof(T),
                AttachmentType = typeof(P),
                CreateDelegate = createPropertyDelegate
            });
        }

        /// <summary>
        /// Hängt ein Property an eine gegebene Unit Interop an.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="P"></typeparam>
        /// <param name="extensionPack"></param>
        /// <param name="name"></param>
        /// <param name="createPropertyDelegate"></param>
        public void AttachUnitInteropProperty<T, P>(IExtensionPack extensionPack, string name,
            Func<UnitInterop, P> createPropertyDelegate = null)
            where T : UnitInterop
            where P : UnitInteropProperty
        {
            // TODO: Prüfungen
            unitInteropAttachments.Add(new UnitAttchmentTypeMap()
            {
                ExtensionPack = extensionPack,
                Name = name,
                Type = typeof(T),
                AttachmentType = typeof(P),
                CreateDelegate = createPropertyDelegate
            });
        }

        #endregion

        #region Interop Extender

        private class FactoryExtenderTypeMap : ExtenderTypeMap<Action<FactoryInterop>> { }

        private class UnitExtenderTypeMap : ExtenderTypeMap<Action<UnitInterop>> { }

        private List<FactoryExtenderTypeMap> factoryInteropExtender = new List<FactoryExtenderTypeMap>();

        private List<UnitExtenderTypeMap> unitInteropExtender = new List<UnitExtenderTypeMap>();

        /// <summary>
        /// List of all available Factory Interop Extender.
        /// </summary>
        public IEnumerable<IRankedTypeMapperEntry> FactoryInteropExtender { get { return factoryInteropExtender; } }

        /// <summary>
        /// List of all available Unit Interop Extender.
        /// </summary>
        public IEnumerable<IRankedTypeMapperEntry> UnitInteropExtender { get { return unitInteropExtender; } }

        /// <summary>
        /// Registriert einen Extender für das gegebene Factory Interop
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="extensionPack"></param>
        /// <param name="name"></param>
        /// <param name="rank"></param>
        /// <param name="extenderDelegate"></param>
        public void RegisterFactoryInteropExtender<T>(IExtensionPack extensionPack, string name, int rank, Action<FactoryInterop> extenderDelegate)
            where T : FactoryInterop
        {
            // TODO: Prüfungen
            factoryInteropExtender.Add(new FactoryExtenderTypeMap()
            {
                ExtensionPack = extensionPack,
                Name = name,
                Type = typeof(T),
                Rank = rank,
                ExtenderDelegate = extenderDelegate
            });
        }

        /// <summary>
        /// Registriert einen Extender für das gegebene Unit Interop
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="extensionPack"></param>
        /// <param name="name"></param>
        /// <param name="rank"></param>
        /// <param name="extenderDelegate"></param>
        public void RegisterUnitInteropExtender<T>(IExtensionPack extensionPack, string name, int rank, Action<UnitInterop> extenderDelegate)
            where T : UnitInterop
        {
            // TODO: prüfungen
            unitInteropExtender.Add(new UnitExtenderTypeMap()
            {
                ExtensionPack = extensionPack,
                Name = name,
                Type = typeof(T),
                Rank = rank,
                ExtenderDelegate = extenderDelegate
            });
        }

        #endregion

        #region Level Properties

        private class LevelPropertiesTypeMap : StateInfoTypeMap<Func<Level, LevelProperty, LevelStateProperty>, Action>
        {
            public Func<Level, LevelProperty> CreatePropertyDelegate { get; set; }
        }

        private List<LevelPropertiesTypeMap> levelProperties = new List<LevelPropertiesTypeMap>();

        public IEnumerable<IStateInfoTypeMapperEntry> LevelProperties
        {
            get
            {
                return levelProperties;
            }
        }

        public void RegisterLevelProperty<P>(IExtensionPack extensionPack, string name, Func<Level, LevelProperty> createPropertyDelegate = null) 
            where P : LevelProperty
        {
            RegisterLevelProperty<P, LevelStateProperty>(extensionPack, name, false, createPropertyDelegate, null);
        }

        public void RegisterLevelProperty<P, S>(IExtensionPack extensionPack, string name, Func<Level, LevelProperty> createPropertyDelegate = null, Func<Level, LevelProperty, LevelStateProperty> createStatePropertyDelegate = null)
            where P : LevelProperty
            where S : LevelStateProperty
        {
            RegisterLevelProperty<P, S>(extensionPack, name, true, createPropertyDelegate, createStatePropertyDelegate);
        }

        private void RegisterLevelProperty<P, S>(IExtensionPack extensionPack, string name, bool stateSet, Func<Level, LevelProperty> createPropertyDelegate = null, Func<Level, LevelProperty, LevelStateProperty> createStatePropertyDelegate = null)
        {
            Type stateType = null;
            if (stateSet)
                stateType = typeof(S);

            levelProperties.Add(new LevelPropertiesTypeMap()
            {
                ExtensionPack = extensionPack,
                Name = name,
                Type = typeof(P),
                StateType = stateType,
                InfoType = null,
                CreatePropertyDelegate = createPropertyDelegate,
                CreateStateDelegate = createStatePropertyDelegate,
                CreateInfoDelegate = null,
            });
        }

        #endregion

        #region Level Extender

        private class LevelExtenderTypeMap : ExtenderTypeMap<Action<Level>> { }

        private List<LevelExtenderTypeMap> levelExtender = new List<LevelExtenderTypeMap>();

        public IEnumerable<IRankedTypeMapperEntry> LevelExtender
        {
            get
            {
                return levelExtender;
            }
        }

        public void RegisterLevelExtender<L>(IExtensionPack extensionPack, string name, int rank, Action<Level> extenderDelegate)
            where L : Level
        {
            // TODO: Check Stuff
            levelExtender.Add(new LevelExtenderTypeMap()
            {
                ExtensionPack = extensionPack,
                Name = name,
                Type = typeof(L),
                Rank = rank,
                ExtenderDelegate = extenderDelegate
            });
        }

        #endregion

        #region Engine Resolver

        /// <summary>
        /// Ermittelt alle registrierten Extensions für die übergebene Engine.
        /// </summary>
        /// <param name="engine">Referenz auf die neue Engine</param>
        public void ResolveEngine(Engine engine)
        {
            tracer.Trace(TraceEventType.Information, 10, "Apply Extensions to a new Engine");

            // Engine muss gesetzt sein
            if (engine == null)
            {
                tracer.Trace(TraceEventType.Critical, 11, "No Engine was set");
                throw new ArgumentNullException("engine");
            }

            var extensions = enginePropertyContainer.OrderBy(e => e.Rank).ToArray();
            for (int i = 0; i < extensions.Length; i++)
            {
                EnginePropertyContainer item = extensions[i];

                tracer.Trace(TraceEventType.Information, 12, "Try to apply Extension {0}", item.Name);

                EngineProperty property = null;
                if (item.ExtenderDelegate != null)
                {
                    // Benutze CreateDelegate
                    tracer.Trace(TraceEventType.Information, 13, "Use Delegate");
                    property = item.ExtenderDelegate(engine);
                }
                else
                {
                    // Automatische Instanz
                    tracer.Trace(TraceEventType.Information, 14, "Use Activator");
                    property = Activator.CreateInstance(item.Type, engine) as EngineProperty;
                }

                // Existenz prüfen
                if (property == null)
                {
                    tracer.Trace(TraceEventType.Critical, 15, "Could not create Extension '{0}'", item.Name);
                    throw new NullReferenceException(
                        string.Format("Es konnte keine Instanz der Engine Extension {0} erstellt werden.", item.Name));
                }

                engine.AddProperty(property);

                tracer.Trace(TraceEventType.Information, 13, "Apply Extension Successful {0}", item.Name);
            }
        }

        #endregion

        #region Item Resolver

        /// <summary>
        /// Hängt alle Properties an und startet Extender.
        /// </summary>
        /// <param name="item">Neues Game Item</param>
        public void ResolveItem(Item item)
        {
            if (item == null)
                throw new ArgumentNullException("Item");

            // Sicherstellen, dass das Item registriert ist.
            if (!itemsContainer.Any(c => c.Type == item.GetType()))
            {
                // TODO: Trace
                throw new NotSupportedException("Item is not registered.");
            }

            // Vererbungskette auflösen
            List<Type> types = new List<Type>();
            Type current = item.GetType();
            types.Add(current);
            while (current != typeof(Item))
            {
                current = current.BaseType;
                types.Add(current);
            }
            Type[] itemTypes = types.ToArray();
            Array.Reverse(itemTypes);

            // Attachements
            foreach (var type in itemTypes)
            {
                foreach (var attachment in itemAttachments.Where(c => c.Type == type))
                {
                    if (attachment.CreateDelegate != null)
                    {
                        ItemProperty property = attachment.CreateDelegate(item);
                        if (property != null)
                        {
                            if (property.GetType() != attachment.AttachmentType)
                            {
                                // TODO: Trace
                                throw new NotSupportedException("Delegate returned wrong Property Type");
                            }
                            item.AddProperty(property);
                        }
                    }
                    else
                    {
                        item.AddProperty(Activator.CreateInstance(attachment.AttachmentType, item) as ItemProperty);
                    }
                }
            }

            // Extender
            foreach (var extender in itemExtender.Where(c => itemTypes.Contains(c.Type)).OrderBy(c => c.Rank))
            {
                extender.ExtenderDelegate(item);
            }
        }

        /// <summary>
        /// Instanziert einen State inkl. Properties.
        /// </summary>
        /// <param name="item">Item Instanz</param>
        /// <returns>Neue Instanz des passenden States</returns>
        public ItemState CreateItemState(Item item)
        {
            if (item == null)
                throw new ArgumentNullException("Item");

            var container = itemsContainer.FirstOrDefault(g => g.Type == item.GetType());
            if (container == null)
            {
                // TODO: Trace
                throw new ArgumentException("Item is not registered");
            }

            ItemState state;
            if (container.CreateStateDelegate != null)
            {
                // Erstellung über Delegat
                state = container.CreateStateDelegate(item);
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
                state = Activator.CreateInstance(container.StateType, item) as ItemState;
                if (state == null)
                {
                    // TODO: Trace
                    throw new Exception("State could not be created.");
                }
            }


            // State Properties auffüllen
            foreach (var property in item.Properties)
            {
                var map = itemProperties.FirstOrDefault(p => p.Type == property.GetType());
                if (map == null)
                    throw new NotSupportedException("Property is not registered.");

                StateProperty prop = null;
                if (map.CreateStateDelegate != null)
                {
                    // Option 1: Create Delegate
                    prop = map.CreateStateDelegate(item, property);

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
                    prop = Activator.CreateInstance(map.StateType, item, property) as StateProperty;
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

        /// <summary>
        /// Instanziert eine Info zum dazugehörigen Item.
        /// </summary>
        /// <param name="item">Item Instanz</param>
        /// <param name="observer">Beobachtendes Game Item</param>
        /// <returns>Neue Instanz des passenden Infos</returns>
        public ItemInfo CreateItemInfo(Item item, Item observer)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            if (observer == null)
                throw new ArgumentNullException("observer");

            var container = itemsContainer.FirstOrDefault(g => g.Type == item.GetType());
            if (container == null)
            {
                // TODO: Trace
                throw new ArgumentException("Item is not registered");
            }

            // Keine Info
            if (container.InfoType == null)
                return null;

            if (container != null)
            {
                var info = Activator.CreateInstance(container.InfoType, item, observer) as ItemInfo;

                // Info-Properties in Abhängigkeit der Item-Props
                foreach (var property in item.Properties)
                {
                    var map = itemProperties.FirstOrDefault(p => p.Type == property.GetType());
                    if (map == null)
                        throw new NotSupportedException("Property is not registered.");

                    ItemInfoProperty prop = null;
                    if (map.CreateInfoDelegate != null)
                    {
                        // Option 1: Create Delegate
                        prop = map.CreateInfoDelegate(item, property, observer);

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
                        prop = Activator.CreateInstance(map.InfoType, item, property, observer) as ItemInfoProperty;
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

            return null;
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

        #region Interop Resolver

        /// <summary>
        /// Creates a filled Factory Interop (all registered Properties and applied Extender) 
        /// for the given Faction.
        /// </summary>
        /// <param name="faction">Reference to the Faction</param>
        /// <returns>New Instance of a full filled Factory Interop</returns>
        public FactoryInterop CreateFactoryInterop(Faction faction)
        {
            if (faction == null)
                throw new ArgumentNullException("faction");

            // Search for the related Faction Definition
            var factionDefintion = factions.FirstOrDefault(f => f.Type == faction.GetType());
            if (factionDefintion == null)
                throw new ArgumentException("Faction is not registered");

            // Creates a new Instance of the registered Interop Type
            FactoryInterop result = Activator.CreateInstance(factionDefintion.FactoryInteropType, faction) as FactoryInterop;
            if (result == null)
                throw new Exception("Error during Factory Interop Creation.");

            // Creates all Attachment Properties for the selected Interop Type
            foreach (var item in factoryInteropAttachments.Where(a => a.Type == result.GetType()))
            {
                FactoryInteropProperty property = null;
                if (item.CreateDelegate != null)
                {
                    // Create by Delegate
                    property = item.CreateDelegate(faction, result);
                }
                else
                {
                    // Create by Default
                    property = Activator.CreateInstance(item.AttachmentType, faction, result) as FactoryInteropProperty;
                    if (property == null)
                        throw new Exception("Error during Factory Interop Property Creation.");
                }

                // Attach created Property
                if (property != null)
                    result.AddProperty(property);
            }

            // Execute all Extender for the selected Interop Type.
            foreach (var extender in factoryInteropExtender.Where(e => e.Type == result.GetType()).OrderBy(e => e.Rank))
            {
                extender.ExtenderDelegate(result);
            }

            return result;
        }

        /// <summary>
        /// Erzeugt eine neue Instanz eines passenden Unit Interop.
        /// </summary>
        /// <param name="faction"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public UnitInterop CreateUnitInterop(Faction faction, FactionItem item)
        {
            if (faction == null)
                throw new ArgumentNullException("faction");

            // Search for the related Faction Definition
            var factionDefintion = factions.FirstOrDefault(f => f.Type == faction.GetType());
            if (factionDefintion == null)
                throw new ArgumentException("Faction is not registered");

            // Creates a new Instance of the registered Interop Type
            UnitInterop result = Activator.CreateInstance(factionDefintion.UnitInteropType, faction, item) as UnitInterop;
            if (result == null)
                throw new Exception("Error during Factory Interop Creation.");

            // Creates all Attachment Properties for the selected Interop Type
            foreach (var attachment in unitInteropAttachments.Where(a => a.Type == result.GetType()))
            {
                UnitInteropProperty property = null;
                if (attachment.CreateDelegate != null)
                {
                    // Create by Delegate
                    property = attachment.CreateDelegate(result);
                }
                else
                {
                    // Create by Default
                    property = Activator.CreateInstance(attachment.AttachmentType, faction, item, result) as UnitInteropProperty;
                    if (property == null)
                        throw new Exception("Error during Factory Interop Property Creation.");
                }

                // Attach
                if (property != null)
                    result.AddProperty(property);
            }

            // Execute all Extender for the selected Interop Type.
            foreach (var extender in unitInteropExtender.Where(e => e.Type == result.GetType()).OrderBy(e => e.Rank))
            {
                extender.ExtenderDelegate(result);
            }

            return result;
        }

        #endregion

        #region Level Resolver

        public void ResolveLevel(Level level)
        {
            // Level Properties
            foreach (var item in levelProperties)
            {
                LevelProperty property = null;
                if (item.CreatePropertyDelegate != null)
                    property = item.CreatePropertyDelegate(level);
                else
                {
                    property = Activator.CreateInstance(item.Type, level) as LevelProperty;
                    if (property == null)
                        throw new Exception("Not able to create a Level Property");
                }

                if (property != null)
                    level.AddProperty(property);
            }

            // TODO: Level-Vererbung auflösen
            // Level Extender
            foreach (var extender in levelExtender.Where(e => e.Type == level.GetType()).OrderBy(e => e.Rank))
            {
                extender.ExtenderDelegate(level);
            }
        }

        /// <summary>
        /// Generates a State for the given Level.
        /// </summary>
        /// <param name="level">Level</param>
        /// <returns>New State</returns>
        public LevelState CreateLevelState(Level level)
        {
            if (level == null)
                throw new ArgumentNullException("level");

            LevelState state = new LevelState();

            // State Properties auffüllen
            foreach (var property in level.Properties)
            {
                var map = levelProperties.FirstOrDefault(p => p.Type == property.GetType());
                if (map == null)
                    throw new NotSupportedException("Property is not registered.");

                LevelStateProperty prop = null;
                if (map.CreateStateDelegate != null)
                {
                    // Option 1: Create Delegate
                    prop = map.CreateStateDelegate(level, property);

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
                    prop = Activator.CreateInstance(map.StateType, level, property) as LevelStateProperty;
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

        #endregion
    }
}
