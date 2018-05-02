using System;
using System.Collections.Generic;

namespace AntMe
{
    /// <summary>
    /// Type Repository for all dynamic Elements within the Game Simulation.
    /// </summary>
    public interface ITypeMapper
    {
        #region Management

        /// <summary>
        /// Removes all Extension Elements with the given Extension Pack Source.
        /// </summary>
        /// <param name="extensionPack">Extension Pack to remove</param>
        void RemoveExtensionPack(IExtensionPack extensionPack);

        #endregion

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

        #region Map Properties

        /// <summary>
        /// Registers additional Map Properties.
        /// </summary>
        /// <typeparam name="T">Type of Map Property</typeparam>
        /// <param name="extensionPack"></param>
        /// <param name="name"></param>
        /// <param name="createPropertyDelegate"></param>
        void RegisterMapProperty<T>(IExtensionPack extensionPack, string name, 
            Func<Map, T> createPropertyDelegate = null)
            where T : MapProperty;

        /// <summary>
        /// Registers additional Map Properties.
        /// </summary>
        /// <typeparam name="T">Type of Map Property</typeparam>
        /// <typeparam name="TS">Type of State for the Map Property</typeparam>
        /// <param name="extensionPack"></param>
        /// <param name="name"></param>
        /// <param name="createPropertyDelegate"></param>
        /// <param name="createStateDelegate"></param>
        void RegisterMapProperty<T, TS>(IExtensionPack extensionPack, string name, 
            Func<Map, T> createPropertyDelegate = null, 
            Func<Map, MapProperty, TS> createStateDelegate = null)
            where T : MapProperty
            where TS : MapStateProperty;

        /// <summary>
        /// List all Map Properties.
        /// </summary>
        IEnumerable<IStateInfoTypeMapperEntry> MapProperties { get; }

        #endregion

        #region Map Extender

        /// <summary>
        /// Registeres a Delegate to extend a Map.
        /// </summary>
        /// <param name="extensionPack">Extension Pack</param>
        /// <param name="name">Name</param>
        /// <param name="priority">Priority</param>
        /// <param name="extenderDelegate">Extender Delegate</param>
        void RegisterMapExtender(IExtensionPack extensionPack, string name, Action<Map> extenderDelegate, int priority);

        /// <summary>
        /// List of all Map Extender.
        /// </summary>
        IEnumerable<IRankedTypeMapperEntry> MapExtender { get; }

        #endregion

        #region Map Material

        /// <summary>
        /// Registers a new Material.
        /// </summary>
        /// <typeparam name="T">Material Type</typeparam>
        /// <param name="extensionPack">Extension Pack</param>
        /// <param name="name">Material Name</param>
        void RegisterMapMaterial<T>(IExtensionPack extensionPack, string name) 
            where T : MapMaterial;

        /// <summary>
        /// List of all available Materials.
        /// </summary>
        IEnumerable<ITypeMapperEntry> MapMaterials { get; }

        #endregion

        #region Map Tiles

        /// <summary>
        /// Registeres a Map Tile.
        /// </summary>
        /// <typeparam name="T">Map Tile Type</typeparam>
        /// <typeparam name="TS">Map Tile State Type</typeparam>
        /// <typeparam name="TI">Map Tile Info Type</typeparam>
        /// <param name="extensionPack">Extension Pack</param>
        /// <param name="name">Name</param>
        /// <param name="createStateDelegate">Optional Create State Delegate</param>
        /// <param name="createInfoDelegate">Optional Create Info Delegate</param>
        void RegisterMapTile<T, TS, TI>(IExtensionPack extensionPack, string name,
            Func<MapTile, TS> createStateDelegate = null,
            Func<MapTile, Item, TI> createInfoDelegate = null)
            where T : MapTile
            where TS : MapTileState
            where TI : MapTileInfo;

        /// <summary>
        /// List of all Map Tiles
        /// </summary>
        IEnumerable<IStateInfoTypeMapperEntry> MapTiles { get; }

        #endregion

        #region Map Tile Properties

        /// <summary>
        /// 
        /// </summary>
        /// <param name="extensionPack"></param>
        /// <param name="name"></param>
        /// <typeparam name="T"></typeparam>
        void RegisterMapTileProperty<T>(IExtensionPack extensionPack, string name)
            where T : MapTileProperty;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="extensionPack"></param>
        /// <param name="name"></param>
        /// <param name="createStateDelegate"></param>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TS"></typeparam>
        void RegisterMapTilePropertyS<T, TS>(IExtensionPack extensionPack, string name,
            Func<MapTile, MapTileProperty, TS> createStateDelegate = null)
            where T : MapTileProperty
            where TS : MapTileStateProperty;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="extensionPack"></param>
        /// <param name="name"></param>
        /// <param name="createInfoDelegate"></param>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TI"></typeparam>
        void RegisterMapTilePropertyI<T, TI>(IExtensionPack extensionPack, string name,
            Func<MapTile, MapTileProperty, Item, TI> createInfoDelegate = null)
            where T : MapTileProperty
            where TI : MapTileInfoProperty;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="extensionPack"></param>
        /// <param name="name"></param>
        /// <param name="createStateDelegate"></param>
        /// <param name="createInfoDelegate"></param>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TS"></typeparam>
        /// <typeparam name="TI"></typeparam>
        void RegisterMapTilePropertySi<T, TS, TI>(IExtensionPack extensionPack, string name,
            Func<MapTile, MapTileProperty, TS> createStateDelegate = null,
            Func<MapTile, MapTileProperty, Item, TI> createInfoDelegate = null)
            where T : MapTileProperty
            where TS : MapTileStateProperty
            where TI : MapTileInfoProperty;

        /// <summary>
        /// List of all Map Tile Properties.
        /// </summary>
        IEnumerable<IStateInfoTypeMapperEntry> MapTileProperties { get; }

        #endregion

        #region Map Tile Attachments

        /// <summary>
        /// 
        /// </summary>
        /// <param name="extensionPack"></param>
        /// <param name="name"></param>
        /// <param name="createPropertyDelegate"></param>
        /// <typeparam name="TI"></typeparam>
        /// <typeparam name="TP"></typeparam>
        void AttachMapTileProperty<TI, TP>(IExtensionPack extensionPack, string name, Func<MapTile, TP> createPropertyDelegate = null)
            where TI : MapTile
            where TP : MapTileProperty;

        /// <summary>
        /// 
        /// </summary>
        IEnumerable<IAttachmentTypeMapperEntry> MapTileAttachments { get; }

        #endregion

        #region Map Tile Extender

        /// <summary>
        /// 
        /// </summary>
        /// <param name="extensionPack"></param>
        /// <param name="name"></param>
        /// <param name="extenderDelegate"></param>
        /// <param name="priority"></param>
        /// <typeparam name="T"></typeparam>
        void RegisterMapTileExtender<T>(IExtensionPack extensionPack, string name, Action<MapTile> extenderDelegate, int priority)
            where T : MapTile;

        /// <summary>
        /// 
        /// </summary>
        IEnumerable<IRankedTypeMapperEntry> MapTileExtender { get; }

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
        /// <typeparam name="TS">Dazugehöriger State Type</typeparam>
        /// <typeparam name="TI">Dazugehöriger Info Type</typeparam>
        void RegisterItem<T, TS, TI>(IExtensionPack extensionPack, string name,
            Func<Item, TS> createStateDelegate = null,
            Func<Item, Item, TI> createInfoDelegate = null)
            where T : Item
            where TS : ItemState
            where TI : ItemInfo;

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
        /// <typeparam name="TS">Type des State Properties</typeparam>
        /// <param name="extensionPack">Referenz auf den Extension Pack</param>
        /// <param name="name">Name des Properties</param>
        /// <param name="createStateDelegate">Optionaler Delegat zur Erzeugung des States.</param>
        void RegisterItemPropertyS<T, TS>(IExtensionPack extensionPack, string name,
            Func<Item, ItemProperty, TS> createStateDelegate = null)
            where T : ItemProperty
            where TS : ItemStateProperty;

        /// <summary>
        /// Registriert ein Item Property das nur ein Info Property hat.
        /// </summary>
        /// <typeparam name="T">Type des Property</typeparam>
        /// <typeparam name="TI">Type des Info Property</typeparam>
        /// <param name="extensionPack">Referenz auf den Extension Pack</param>
        /// <param name="name">Name des Properties</param>
        /// <param name="createInfoDelegate">Optionaler Delegat zur Erzeugung des Info Properties.</param>
        void RegisterItemPropertyI<T, TI>(IExtensionPack extensionPack, string name,
            Func<Item, ItemProperty, Item, TI> createInfoDelegate = null)
            where T : ItemProperty
            where TI : ItemInfoProperty;

        /// <summary>
        /// Registriert ein Item Property mit samt den State- und Info-Properties
        /// </summary>
        /// <param name="extensionPack">Referenz auf den Extension Pack</param>
        /// <param name="name">Name des Properties</param>
        /// <param name="createStateDelegate">Delegate zum Erstellen eines neuen State Properties</param>
        /// <param name="createInfoDelegate">Delegate zum Erstellen eines neuen Info Properties</param>
        /// <typeparam name="T">Type des Property</typeparam>
        /// <typeparam name="TS">Type des State Property</typeparam>
        /// <typeparam name="TI">Type des Info Property</typeparam>
        void RegisterItemPropertySi<T, TS, TI>(IExtensionPack extensionPack, string name,
            Func<Item, ItemProperty, TS> createStateDelegate = null,
            Func<Item, ItemProperty, Item, TI> createInfoDelegate = null)
            where T : ItemProperty
            where TS : ItemStateProperty
            where TI : ItemInfoProperty;

        /// <summary>
        /// Liefert eine Liste aller registrierten Properties zurück.
        /// </summary>
        IEnumerable<IStateInfoTypeMapperEntry> ItemProperties { get; }

        #endregion

        #region Item Attachment Properties

        /// <summary>
        /// Hängt ein definiertes Property an ein Item an.
        /// </summary>
        /// <typeparam name="TI">Item</typeparam>
        /// <typeparam name="TP">Property</typeparam>
        /// <param name="extensionPack">Referenz auf den Extension Pack</param>
        /// <param name="name">Name des Attachments</param>
        /// <param name="createPropertyDelegate">Erstellungsdelegat</param>
        void AttachItemProperty<TI, TP>(IExtensionPack extensionPack, string name, Func<Item, TP> createPropertyDelegate = null)
            where TI : Item
            where TP : ItemProperty;

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
        /// <typeparam name="TS">Typ des Faction States</typeparam>
        /// <typeparam name="TI">Typ der Faction Info</typeparam>
        /// <typeparam name="TF">Typ der Factory</typeparam>
        /// <typeparam name="TFi">Typ der Factory Interop</typeparam>
        /// <typeparam name="TU">Typ der Unit</typeparam>
        /// <typeparam name="TUi">Typ der Unit Interop</typeparam>
        /// <typeparam name="TIt">Item Type</typeparam>
        /// <param name="extensionPack">Referenz auf den Extension Pack.</param>
        /// <param name="name">Name der Faction.</param>
        /// <param name="createStateDelegate">Optionaler Delegat zur Erstellung des Faction States.</param>
        /// <param name="createInfoDelegate">Optionaler Delegat zur Erstellung der Faction Info.</param>
        void RegisterFaction<T, TS, TI, TF, TFi, TU, TUi, TIt>(IExtensionPack extensionPack, string name,
            Func<Faction, TS> createStateDelegate = null,
            Func<Faction, Item, TI> createInfoDelegate = null)
            where T : Faction
            where TS : FactionState
            where TI : FactionInfo
            where TF : FactionFactory
            where TFi : FactoryInterop
            where TU : FactionUnit
            where TUi : UnitInterop
            where TIt : FactionItem;

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
        /// <typeparam name="TS">Typ des State Properties.</typeparam>
        /// <param name="extensionPack">Referenz auf den Extension Pack</param>
        /// <param name="name">Name des Properties.</param>
        /// <param name="createStateDelegate">Optionaler Delegat zur Erstellung des State Properties.</param>
        void RegisterFactionPropertyS<T, TS>(IExtensionPack extensionPack, string name,
            Func<Faction, FactionProperty, TS> createStateDelegate = null)
            where T : FactionProperty
            where TS : FactionStateProperty;

        /// <summary>
        /// Registriert ein Faction Property mit zugehörigem Info Property.
        /// </summary>
        /// <typeparam name="T">Typ des Properties.</typeparam>
        /// <typeparam name="TI">Typ des Info Properties.</typeparam>
        /// <param name="extensionPack">Referenz auf den Extension Pack</param>
        /// <param name="name">Name des Properties.</param>
        /// <param name="createInfoDelegate">Optionaler Delegat zur Erstellung des Info Properties.</param>
        void RegisterFactionPropertyI<T, TI>(IExtensionPack extensionPack, string name,
            Func<Faction, FactionProperty, Item, TI> createInfoDelegate = null)
            where T : FactionProperty
            where TI : FactionInfoProperty;

        /// <summary>
        /// Registriert ein Faction Property mit zugehörigem State- und Info-Property.
        /// </summary>
        /// <typeparam name="T">Typ des Properties.</typeparam>
        /// <typeparam name="TS">Typ des State Properties.</typeparam>
        /// <typeparam name="TI">Typ des Info Properties.</typeparam>
        /// <param name="extensionPack">Referenz auf den Extension Pack</param>
        /// <param name="name">Name des Properties.</param>
        /// <param name="createStateDelegate">Optionaler Delegat zur Erstellung des State Properties.</param>
        /// <param name="createInfoDelegate">Optionaler Delegat zur Erstellung des Info Properties.</param>
        void RegisterFactionPropertySi<T, TS, TI>(IExtensionPack extensionPack, string name,
            Func<Faction, FactionProperty, TS> createStateDelegate = null,
            Func<Faction, FactionProperty, Item, TI> createInfoDelegate = null)
            where T : FactionProperty
            where TS : FactionStateProperty
            where TI : FactionInfoProperty;

        /// <summary>
        /// Auflistung aller registrierten Faction Properties.
        /// </summary>
        IEnumerable<IStateInfoTypeMapperEntry> FactionProperties { get; }

        #endregion

        #region Faction Attachment Properties

        /// <summary>
        /// 
        /// </summary>
        /// <param name="extensionPack"></param>
        /// <param name="name"></param>
        /// <param name="createPropertyDelegate"></param>
        /// <typeparam name="TF"></typeparam>
        /// <typeparam name="TP"></typeparam>
        void AttachFactionProperty<TF, TP>(IExtensionPack extensionPack, string name, Func<Faction, TP> createPropertyDelegate = null)
            where TF : Faction
            where TP : FactionProperty;

        /// <summary>
        /// 
        /// </summary>
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

        #region Interop Attachment Properties

        /// <summary>
        /// 
        /// </summary>
        IEnumerable<IAttachmentTypeMapperEntry> FactoryInteropAttachments { get; }

        /// <summary>
        /// 
        /// </summary>
        IEnumerable<IAttachmentTypeMapperEntry> UnitInteropAttachments { get; }

        /// <summary>
        /// Hängt ein Property an eine gegebene Factory Interop an.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TP"></typeparam>
        /// <param name="extensionPack"></param>
        /// <param name="name"></param>
        /// <param name="createPropertyDelegate"></param>
        void AttachFactoryInteropProperty<T, TP>(IExtensionPack extensionPack, string name,
            Func<Faction, FactoryInterop, TP> createPropertyDelegate = null)
            where T : FactoryInterop
            where TP : FactoryInteropProperty;

        /// <summary>
        /// Hängt ein Property an eine gegebene Unit Interop an.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TP"></typeparam>
        /// <param name="extensionPack"></param>
        /// <param name="name"></param>
        /// <param name="createPropertyDelegate"></param>
        void AttachUnitInteropProperty<T, TP>(IExtensionPack extensionPack, string name,
            Func<UnitInterop, TP> createPropertyDelegate = null)
            where T : UnitInterop
            where TP : UnitInteropProperty;

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
        /// <typeparam name="TP">Typ des Properties</typeparam>
        /// <param name="extensionPack">Referenz auf den Extension Pack</param>
        /// <param name="name">Name des Properties</param>
        /// <param name="createPropertyDelegate">Optionaler Erstellungs-Delegat</param>
        void RegisterLevelProperty<TP>(IExtensionPack extensionPack, string name, Func<Level, LevelProperty> createPropertyDelegate = null)
            where TP : LevelProperty;

        /// <summary>
        /// Registriert ein Level Property mit State Property.
        /// </summary>
        /// <typeparam name="TP">Typ des Properties</typeparam>
        /// <typeparam name="TS">Typ des State Properties</typeparam>
        /// <param name="extensionPack">Referenz auf den Extension Pack</param>
        /// <param name="name">Name des Properties</param>
        /// <param name="createPropertyDelegate">Optionaler Erstellungs-Delegat</param>
        /// <param name="createStatePropertyDelegate">Optionaler Delegat zur Erstellung des States</param>
        void RegisterLevelProperty<TP, TS>(IExtensionPack extensionPack, string name, Func<Level, LevelProperty> createPropertyDelegate = null, Func<Level, LevelProperty, LevelStateProperty> createStatePropertyDelegate = null)
            where TP : LevelProperty
            where TS : LevelStateProperty;

        #endregion

        #region Level Extender

        /// <summary>
        /// Auflistung aller Level Extender.
        /// </summary>
        IEnumerable<IRankedTypeMapperEntry> LevelExtender { get; }

        /// <summary>
        /// Registriert eine Extender-Methode für einen bestimmten Level Type
        /// </summary>
        /// <typeparam name="TL">Typ des Levels</typeparam>
        /// <param name="extensionPack">Referenz auf den Extension Pack</param>
        /// <param name="name">Name</param>
        /// <param name="extenderDelegate">Methode</param>
        /// <param name="rank">Aufruf-Ranking</param>
        void RegisterLevelExtender<TL>(IExtensionPack extensionPack, string name, int rank, Action<Level> extenderDelegate)
            where TL : Level;

        #endregion
    }
}
