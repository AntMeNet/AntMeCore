using System;
using System.Collections.Generic;

namespace AntMe
{
    /// <summary>
    /// Zentrales Type Repository zur Auflistung von erweiterbaren Engine Fragmenten.
    /// </summary>
    public interface ITypeMapper
    {
        #region Engine Properties

        /// <summary>
        /// Registriert eine Engine Extension.
        /// </summary>
        /// <param name="extensionPack">Referenz Extension</param>
        /// <param name="name">Name der Extension</param>
        /// <param name="rank">Rang der Extension</param>
        /// <param name="createExtensionDelegate">Delegat zum Erstellen einer neuen Instanz</param>
        /// <typeparam name="T">Extension Type</typeparam>
        void RegisterEngineProperty<T>(IExtensionPack extensionPack, string name, int rank, Func<Engine, T> createExtensionDelegate = null) where T : EngineProperty;

        /// <summary>
        /// Listet alle registrierten Extensions auf.
        /// </summary>
        IEnumerable<IRankedTypeMapperEntry> EngineProperties { get; }

        #endregion

        #region Items

        /// <summary>
        /// Registriert einen Game Type beim Type Mapper.
        /// </summary>
        /// <param name="extensionPack">Referenz auf den Extension Pack.</param>
        /// <param name="name">Name des Game Items</param>
        /// <param name="createStateDelegate">Delegat zur manuellen Erstellung eines States</param>
        /// <param name="createInfoDelegate">Delegat zur manuellen Erstellung eines Info Objektes</param>
        /// <typeparam name="T">Type des Types</typeparam>
        /// <typeparam name="S">Dazugehöriger State Type</typeparam>
        /// <typeparam name="I">Dazugehöriger Info Type</typeparam>
        void RegisterItem<T, S, I>(IExtensionPack extensionPack, string name,
            Func<Item, S> createStateDelegate = null,
            Func<Item, Item, I> createInfoDelegate = null)
            where T : Item
            where S : ItemState
            where I : ItemInfo;

        /// <summary>
        /// Liefert eine Liste registrierter Game Items.
        /// </summary>
        IEnumerable<IStateInfoTypeMapperEntry> Items { get; }

        #endregion

        #region Item Properties

        /// <summary>
        /// Registriert ein Item Property das keine State- oder Info-Properties besitzt.
        /// </summary>
        /// <typeparam name="T">Type des Property</typeparam>
        /// <param name="extensionPack">Referenz auf den Extension Pack</param>
        /// <param name="name">Name des Properties</param>
        void RegisterItemProperty<T>(IExtensionPack extensionPack, string name) where T : ItemProperty;

        /// <summary>
        /// Registriert ein Item Property das nur ein State Property hat.
        /// </summary>
        /// <typeparam name="T">Type des Property</typeparam>
        /// <typeparam name="S">Type des State Properties</typeparam>
        /// <param name="extensionPack">Referenz auf den Extension Pack</param>
        /// <param name="name">Name des Properties</param>
        /// <param name="createStateDelegate">Optionaler Delegat zur Erzeugung des States.</param>
        void RegisterItemPropertyS<T, S>(IExtensionPack extensionPack, string name,
            Func<Item, ItemProperty, S> createStateDelegate = null)
            where T : ItemProperty
            where S : ItemStateProperty;

        /// <summary>
        /// Registriert ein Item Property das nur ein Info Property hat.
        /// </summary>
        /// <typeparam name="T">Type des Property</typeparam>
        /// <typeparam name="I">Type des Info Property</typeparam>
        /// <param name="extensionPack">Referenz auf den Extension Pack</param>
        /// <param name="name">Name des Properties</param>
        /// <param name="createInfoDelegate">Optionaler Delegat zur Erzeugung des Info Properties.</param>
        void RegisterItemPropertyI<T, I>(IExtensionPack extensionPack, string name,
            Func<Item, ItemProperty, Item, I> createInfoDelegate = null)
            where T : ItemProperty
            where I : ItemInfoProperty;

        /// <summary>
        /// Registriert ein Item Property mit samt den State- und Info-Properties
        /// </summary>
        /// <param name="extensionPack">Referenz auf den Extension Pack</param>
        /// <param name="name">Name des Properties</param>
        /// <param name="createStateDelegate">Delegate zum Erstellen eines neuen State Properties</param>
        /// <param name="createInfoDelegate">Delegate zum Erstellen eines neuen Info Properties</param>
        /// <typeparam name="T">Type des Property</typeparam>
        /// <typeparam name="S">Type des State Property</typeparam>
        /// <typeparam name="I">Type des Info Property</typeparam>
        void RegisterItemPropertySI<T, S, I>(IExtensionPack extensionPack, string name,
            Func<Item, ItemProperty, S> createStateDelegate = null,
            Func<Item, ItemProperty, Item, I> createInfoDelegate = null)
            where T : ItemProperty
            where S : ItemStateProperty
            where I : ItemInfoProperty;

        /// <summary>
        /// Liefert eine Liste aller registrierten Properties zurück.
        /// </summary>
        IEnumerable<IStateInfoTypeMapperEntry> ItemProperties { get; }

        #endregion

        #region Item Attachment Properties

        /// <summary>
        /// Hängt ein definiertes Property an ein Item an.
        /// </summary>
        /// <typeparam name="I">Item</typeparam>
        /// <typeparam name="P">Property</typeparam>
        /// <param name="extensionPack">Referenz auf den Extension Pack</param>
        /// <param name="name">Name des Attachments</param>
        /// <param name="createPropertyDelegate">Erstellungsdelegat</param>
        void AttachItemProperty<I, P>(IExtensionPack extensionPack, string name, Func<Item, P> createPropertyDelegate = null)
            where I : Item
            where P : ItemProperty;

        /// <summary>
        /// Auflistung aller registrierten Item Attachments.
        /// </summary>
        IEnumerable<IAttachmentTypeMapperEntry> ItemAttachments { get; }

        #endregion

        #region Item Extender

        /// <summary>
        /// Registriert einen Delegaten zur Erweiterung des angegebenen Item Types.
        /// </summary>
        /// <param name="extensionPack">Extension Pack</param>
        /// <param name="name">Name der Erweiterung</param>
        /// <param name="priority">Priorität</param>
        /// <typeparam name="T">Item Type für den der Extender gilt</typeparam>
        /// <param name="extenderDelegate">Delegat</param>
        void RegisterItemExtender<T>(IExtensionPack extensionPack, string name, Action<Item> extenderDelegate, int priority)
            where T : Item;

        /// <summary>
        /// Listet alle Item Extender auf.
        /// </summary>
        IEnumerable<IRankedTypeMapperEntry> ItemExtender { get; }

        #endregion

        #region Factions

        /// <summary>
        /// Registriert eine neue Faciton.
        /// </summary>
        /// <typeparam name="T">Typ der Faction</typeparam>
        /// <typeparam name="S">Typ des Faction States</typeparam>
        /// <typeparam name="I">Typ der Faction Info</typeparam>
        /// <typeparam name="F">Typ der Factory</typeparam>
        /// <typeparam name="FI">Typ der Factory Interop</typeparam>
        /// <typeparam name="U">Typ der Unit</typeparam>
        /// <typeparam name="UI">Typ der Unit Interop</typeparam>
        /// <param name="extensionPack">Referenz auf den Extension Pack.</param>
        /// <param name="name">Name der Faction.</param>
        /// <param name="createStateDelegate">Optionaler Delegat zur Erstellung des Faction States.</param>
        /// <param name="createInfoDelegate">Optionaler Delegat zur Erstellung der Faction Info.</param>
        void RegisterFaction<T, S, I, F, FI, U, UI>(IExtensionPack extensionPack, string name,
            Func<Faction, S> createStateDelegate = null,
            Func<Faction, Item, I> createInfoDelegate = null)
            where T : Faction
            where S : FactionState
            where I : FactionInfo
            where F : FactionFactory
            where FI : FactoryInterop
            where U : FactionUnit
            where UI : UnitInterop;

        /// <summary>
        /// Liefert eine Liste der registrierten Factions.
        /// </summary>
        IEnumerable<ITypeMapperFactionEntry> Factions { get; }

        #endregion

        #region Faction Properties

        /// <summary>
        /// Registriert ein Faction Property.
        /// </summary>
        /// <typeparam name="T">Typ des Properties.</typeparam>
        /// <param name="extensionPack">Referenz auf den Extension Pack</param>
        /// <param name="name">Name des Properties.</param>
        void RegisterFactionProperty<T>(IExtensionPack extensionPack, string name)
            where T : FactionProperty;

        /// <summary>
        /// Registriert ein Faction Property mit zugehörigem State Property.
        /// </summary>
        /// <typeparam name="T">Typ des Properties.</typeparam>
        /// <typeparam name="S">Typ des State Properties.</typeparam>
        /// <param name="extensionPack">Referenz auf den Extension Pack</param>
        /// <param name="name">Name des Properties.</param>
        /// <param name="createStateDelegate">Optionaler Delegat zur Erstellung des State Properties.</param>
        void RegisterFactionProperty<T, S>(IExtensionPack extensionPack, string name,
            Func<Faction, FactionProperty, S> createStateDelegate = null)
            where T : FactionProperty
            where S : FactionStateProperty;

        /// <summary>
        /// Registriert ein Faction Property mit zugehörigem Info Property.
        /// </summary>
        /// <typeparam name="T">Typ des Properties.</typeparam>
        /// <typeparam name="I">Typ des Info Properties.</typeparam>
        /// <param name="extensionPack">Referenz auf den Extension Pack</param>
        /// <param name="name">Name des Properties.</param>
        /// <param name="createInfoDelegate">Optionaler Delegat zur Erstellung des Info Properties.</param>
        void RegisterFactionProperty<T, I>(IExtensionPack extensionPack, string name,
            Func<Faction, FactionProperty, Item, I> createInfoDelegate = null)
            where T : FactionProperty
            where I : FactionInfoProperty;

        /// <summary>
        /// Registriert ein Faction Property mit zugehörigem State- und Info-Property.
        /// </summary>
        /// <typeparam name="T">Typ des Properties.</typeparam>
        /// <typeparam name="S">Typ des State Properties.</typeparam>
        /// <typeparam name="I">Typ des Info Properties.</typeparam>
        /// <param name="extensionPack">Referenz auf den Extension Pack</param>
        /// <param name="name">Name des Properties.</param>
        /// <param name="createStateDelegate">Optionaler Delegat zur Erstellung des State Properties.</param>
        /// <param name="createInfoDelegate">Optionaler Delegat zur Erstellung des Info Properties.</param>
        void RegisterFactionProperty<T, S, I>(IExtensionPack extensionPack, string name,
            Func<Faction, FactionProperty, S> createStateDelegate = null,
            Func<Faction, FactionProperty, Item, I> createInfoDelegate = null)
            where T : FactionProperty
            where S : FactionStateProperty
            where I : FactionInfoProperty;

        /// <summary>
        /// Auflistung aller registrierten Faction Properties.
        /// </summary>
        IEnumerable<IStateInfoTypeMapperEntry> FactionProperties { get; }

        #endregion

        #region Faction Attachment Properties

        void AttachFactionProperty<I, P>(IExtensionPack extensionPack, string name, Func<Faction, P> createPropertyDelegate = null)
            where I : Faction
            where P : FactionProperty;

        IEnumerable<IAttachmentTypeMapperEntry> FactionAttachments { get; }

        #endregion

        #region Faction Extender

        /// <summary>
        /// Registriert einen neuen Faction Extender.
        /// </summary>
        /// <typeparam name="T">Typ der zu erweiternden Faction</typeparam>
        /// <param name="extensionPack">Referenz auf den verantwortlichen Extension Pack.</param>
        /// <param name="name">Name der Erweiterung</param>
        /// <param name="rank">Priorität</param>
        /// <param name="extenderDelegate">Erweiterungsdelegat der beim Erstellen aufgerufen wird.</param>
        void RegisterFactionExtender<T>(IExtensionPack extensionPack, string name, int rank, Action<Faction> extenderDelegate);

        /// <summary>
        /// Auflistung aller registrierten Faction Extender.
        /// </summary>
        IEnumerable<IRankedTypeMapperEntry> FactionExtender { get; }

        #endregion

        #region Interop Properties

        /// <summary>
        /// Liste aller registrierten Factory Interop Properties.
        /// </summary>
        IEnumerable<ITypeMapperEntry> FactoryInteropProperties { get; }

        /// <summary>
        /// Liste aller registrierten Unit Interop Properties.
        /// </summary>
        IEnumerable<ITypeMapperEntry> UnitInteropProperties { get; }

        /// <summary>
        /// Registriert ein Property für den Factory Interop
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="extensionPack">Referenz auf die Extension</param>
        /// <param name="name">Name des Properties</param>
        void RegisterFactoryInteropProperty<T>(IExtensionPack extensionPack, string name)
            where T : FactoryInteropProperty;

        /// <summary>
        /// Registriert ein Property für den Unit Interop
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="extensionPack">Extension Pack</param>
        /// <param name="name">name des Properties</param>
        void RegisterUnitInteropProperty<T>(IExtensionPack extensionPack, string name)
            where T : UnitInteropProperty;

        #endregion

        #region Interop Attachment Properties

        IEnumerable<IAttachmentTypeMapperEntry> FactoryInteropAttachments { get; }

        IEnumerable<IAttachmentTypeMapperEntry> UnitInteropAttachments { get; }

        /// <summary>
        /// Hängt ein Property an eine gegebene Factory Interop an.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="P"></typeparam>
        /// <param name="extensionPack"></param>
        /// <param name="name"></param>
        /// <param name="createPropertyDelegate"></param>
        void AttachFactoryInteropProperty<T, P>(IExtensionPack extensionPack, string name,
            Func<FactoryInterop, P> createPropertyDelegate = null)
            where T : FactoryInterop
            where P : FactoryInteropProperty;

        /// <summary>
        /// Hängt ein Property an eine gegebene Unit Interop an.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="P"></typeparam>
        /// <param name="extensionPack"></param>
        /// <param name="name"></param>
        /// <param name="createPropertyDelegate"></param>
        void AttachUnitInteropProperty<T, P>(IExtensionPack extensionPack, string name,
            Func<UnitInterop, P> createPropertyDelegate = null)
            where T : UnitInterop
            where P : UnitInteropProperty;

        #endregion

        #region Interop Extender

        /// <summary>
        /// Auflistung aller Factory Interop Extender.
        /// </summary>
        IEnumerable<IRankedTypeMapperEntry> FactoryInteropExtender { get; }

        /// <summary>
        /// Auflistung aller Unit Interop Extender.
        /// </summary>
        IEnumerable<IRankedTypeMapperEntry> UnitInteropExtender { get; }


        /// <summary>
        /// Registriert einen Extender für das gegebene Factory Interop
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="extensionPack"></param>
        /// <param name="name"></param>
        /// <param name="rank"></param>
        /// <param name="extenderDelegate"></param>
        void RegisterFactoryInteropExtender<T>(IExtensionPack extensionPack, string name, int rank, Action<FactoryInterop> extenderDelegate)
            where T : FactoryInterop;

        /// <summary>
        /// Registriert einen Extender für das gegebene Unit Interop
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="extensionPack"></param>
        /// <param name="name"></param>
        /// <param name="rank"></param>
        /// <param name="extenderDelegate"></param>
        void RegisterUnitInteropExtender<T>(IExtensionPack extensionPack, string name, int rank, Action<UnitInterop> extenderDelegate)
            where T : UnitInterop;

        #endregion

        #region Level Properties

        /// <summary>
        /// Auflistung aller verfügbaren Level Properties.
        /// </summary>
        IEnumerable<IStateInfoTypeMapperEntry> LevelProperties { get; }

        /// <summary>
        /// Registriert ein Level Property
        /// </summary>
        /// <typeparam name="P">Typ des Properties</typeparam>
        /// <param name="extensionPack">Referenz auf den Extension Pack</param>
        /// <param name="name">Name des Properties</param>
        /// <param name="createPropertyDelegate">Optionaler Erstellungs-Delegat</param>
        void RegisterLevelProperty<P>(IExtensionPack extensionPack, string name, Func<Level, LevelProperty> createPropertyDelegate = null)
            where P : LevelProperty;

        /// <summary>
        /// Registriert ein Level Property mit State Property.
        /// </summary>
        /// <typeparam name="P">Typ des Properties</typeparam>
        /// <typeparam name="S">Typ des State Properties</typeparam>
        /// <param name="extensionPack">Referenz auf den Extension Pack</param>
        /// <param name="name">Name des Properties</param>
        /// <param name="createPropertyDelegate">Optionaler Erstellungs-Delegat</param>
        /// <param name="createStatePropertyDelegate">Optionaler Delegat zur Erstellung des States</param>
        void RegisterLevelProperty<P, S>(IExtensionPack extensionPack, string name, Func<Level, LevelProperty> createPropertyDelegate = null, Func<Level, LevelProperty, LevelStateProperty> createStatePropertyDelegate = null)
            where P : LevelProperty
            where S : LevelStateProperty;

        #endregion

        #region Level Extender

        /// <summary>
        /// Auflistung aller Level Extender.
        /// </summary>
        IEnumerable<IRankedTypeMapperEntry> LevelExtender { get; }

        /// <summary>
        /// Registriert eine Extender-Methode für einen bestimmten Level Type
        /// </summary>
        /// <typeparam name="L">Typ des Levels</typeparam>
        /// <param name="extensionPack">Referenz auf den Extension Pack</param>
        /// <param name="name">Name</param>
        /// <param name="extenderDelegate">Methode</param>
        /// <param name="rank">Aufruf-Ranking</param>
        void RegisterLevelExtender<L>(IExtensionPack extensionPack, string name, int rank, Action<Level> extenderDelegate)
            where L : Level;

        #endregion
    }
}
