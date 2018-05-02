using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AntMe.Runtime
{
    public partial class TypeMapper
    {
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
        public void RegisterItemPropertySi<T, S, I>(IExtensionPack extensionPack, string name,
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

                ItemStateProperty prop = null;
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
                    prop = Activator.CreateInstance(map.StateType, item, property) as ItemStateProperty;
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
    }
}
