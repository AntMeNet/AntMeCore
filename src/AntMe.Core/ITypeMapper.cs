﻿using System;
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
        /// <typeparam name="S">Type of State for the Map Property</typeparam>
        /// <param name="extensionPack"></param>
        /// <param name="name"></param>
        /// <param name="createPropertyDelegate"></param>
        /// <param name="createStateDelegate"></param>
        void RegisterMapProperty<T, S>(IExtensionPack extensionPack, string name, 
            Func<Map, T> createPropertyDelegate = null, 
            Func<Map, MapProperty, S> createStateDelegate = null)
            where T : MapProperty
            where S : MapStateProperty;

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
        /// <typeparam name="S">Map Tile State Type</typeparam>
        /// <typeparam name="I">Map Tile Info Type</typeparam>
        /// <param name="extensionPack">Extension Pack</param>
        /// <param name="name">Name</param>
        /// <param name="createStateDelegate">Optional Create State Delegate</param>
        /// <param name="createInfoDelegate">Optional Create Info Delegate</param>
        void RegisterMapTile<T, S, I>(IExtensionPack extensionPack, string name,
            Func<MapTile, S> createStateDelegate = null,
            Func<MapTile, Item, I> createInfoDelegate = null)
            where T : MapTile
            where S : MapTileState
            where I : MapTileInfo;

        /// <summary>
        /// List of all Map Tiles
        /// </summary>
        IEnumerable<IStateInfoTypeMapperEntry> MapTiles { get; }

        #endregion

        #region Map Tile Properties

        void RegisterMapTileProperty<T>(IExtensionPack extensionPack, string name)
            where T : MapTileProperty;

        void RegisterMapTilePropertyS<T, S>(IExtensionPack extensionPack, string name,
            Func<MapTile, MapTileProperty, S> createStateDelegate = null)
            where T : MapTileProperty
            where S : MapTileStateProperty;

        void RegisterMapTilePropertyI<T, I>(IExtensionPack extensionPack, string name,
            Func<MapTile, MapTileProperty, Item, I> createInfoDelegate = null)
            where T : MapTileProperty
            where I : MapTileInfoProperty;

        void RegisterMapTilePropertySI<T, S, I>(IExtensionPack extensionPack, string name,
            Func<MapTile, MapTileProperty, S> createStateDelegate = null,
            Func<MapTile, MapTileProperty, Item, I> createInfoDelegate = null)
            where T : MapTileProperty
            where S : MapTileStateProperty
            where I : MapTileInfoProperty;

        /// <summary>
        /// List of all Map Tile Properties.
        /// </summary>
        IEnumerable<IStateInfoTypeMapperEntry> MapTileProperties { get; }

        #endregion

        #region Map Tile Attachments

        void AttachMapTileProperty<I, P>(IExtensionPack extensionPack, string name, Func<MapTile, P> createPropertyDelegate = null)
            where I : MapTile
            where P : MapTileProperty;

        IEnumerable<IAttachmentTypeMapperEntry> MapTileAttachments { get; }

        #endregion

        #region Map Tile Extender

        void RegisterMapTileExtender<T>(IExtensionPack extensionPack, string name, Action<MapTile> extenderDelegate, int priority)
            where T : MapTile;

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
        /// <typeparam name="IT">Item Type</typeparam>
        /// <param name="extensionPack">Referenz auf den Extension Pack.</param>
        /// <param name="name">Name der Faction.</param>
        /// <param name="createStateDelegate">Optionaler Delegat zur Erstellung des Faction States.</param>
        /// <param name="createInfoDelegate">Optionaler Delegat zur Erstellung der Faction Info.</param>
        void RegisterFaction<T, S, I, F, FI, U, UI, IT>(IExtensionPack extensionPack, string name,
            Func<Faction, S> createStateDelegate = null,
            Func<Faction, Item, I> createInfoDelegate = null)
            where T : Faction
            where S : FactionState
            where I : FactionInfo
            where F : FactionFactory
            where FI : FactoryInterop
            where U : FactionUnit
            where UI : UnitInterop
            where IT : FactionItem;

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
        void RegisterFactionPropertyS<T, S>(IExtensionPack extensionPack, string name,
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
        void RegisterFactionPropertyI<T, I>(IExtensionPack extensionPack, string name,
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
        void RegisterFactionPropertySI<T, S, I>(IExtensionPack extensionPack, string name,
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

        void AttachFactionProperty<F, P>(IExtensionPack extensionPack, string name, Func<Faction, P> createPropertyDelegate = null)
            where F : Faction
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
            Func<Faction, FactoryInterop, P> createPropertyDelegate = null)
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
