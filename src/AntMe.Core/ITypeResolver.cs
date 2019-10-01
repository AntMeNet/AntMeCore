using System;

namespace AntMe
{
    /// <summary>
    ///     Interface zur dynamischen Typ-Auflösung während der Simulation.
    /// </summary>
    public interface ITypeResolver
    {
        #region Engine Resolver

        /// <summary>
        ///     Ermittelt alle registrierten Extensions für die übergebene Engine.
        /// </summary>
        /// <param name="engine">Referenz auf die neue Engine</param>
        void ResolveEngine(Engine engine);

        #endregion

        #region Map Resolver

        /// <summary>
        ///     Resolves the given Map.
        /// </summary>
        /// <param name="map">Map</param>
        void ResolveMap(SimulationContext context, Map map);

        void ResolveMapTile(SimulationContext context, MapTile tile);

        /// <summary>
        ///     Generates a Map State for the given Map.
        /// </summary>
        /// <param name="map">Map</param>
        /// <returns>Map State</returns>
        MapState CreateMapState(Map map);

        MapTileState CreateMapTileState(MapTile tile);

        MapTileInfo CreateMapTileInfo(MapTile tile, Item observer);

        #endregion

        #region Item Resolver

        /// <summary>
        ///     Interne Methode zum Auffüllen von neuen Items auf Basis der registrierten Properties.
        /// </summary>
        /// <param name="item">Neues Item</param>
        void ResolveItem(Item item);

        /// <summary>
        ///     Erstellt ein neues State-Objekt für das übergebene Item.
        /// </summary>
        /// <param name="Item">Zugehöriges Item</param>
        /// <returns>State Objekt</returns>
        ItemState CreateItemState(Item Item);

        /// <summary>
        ///     Erstellt ein neues Info-Objekt auf Basis der beiden übergebenen Items.
        /// </summary>
        /// <param name="Item">Item das im Info Objekt repräsentiert wird</param>
        /// <returns>Info</returns>
        ItemInfo CreateItemInfo(Item Item);

        #endregion

        #region Faction Resolver

        /// <summary>
        ///     Erstellt eine neue Faction auf Basis des übergebenen Factory Types oder null,
        ///     falls keine passende Faction gefunden werden konnte.
        /// </summary>
        /// <param name="context">Faction Context</param>
        /// <param name="factoryType">Typ der Spieler Factory</param>
        /// <param name="level">Level</param>
        /// <returns>Neue Faction-Instanz</returns>
        Faction CreateFaction(SimulationContext context, Type factoryType, Level level);

        /// <summary>
        ///     Füllt eine neue Faction mit den registrierten Properties.
        /// </summary>
        /// <param name="faction"></param>
        void ResolveFaction(Faction faction);

        /// <summary>
        ///     Erstellt den State für diese Faction.
        /// </summary>
        /// <param name="faction">Faction</param>
        /// <returns>State</returns>
        FactionState CreateFactionState(Faction faction);

        /// <summary>
        ///     Erstellt ein Info Objekt für diese Faction auf Basis des Beobachters
        /// </summary>
        /// <param name="faction">Faction</param>
        /// <param name="observer">Beobachter</param>
        /// <returns>Info Objekt</returns>
        FactionInfo CreateFactionInfo(Faction faction, Item observer);

        #endregion

        #region Interop Resolver

        /// <summary>
        ///     Erzeugt eine neue Instanz eines passendes Factory Interop.
        /// </summary>
        /// <param name="faction"></param>
        /// <returns></returns>
        FactoryInterop CreateFactoryInterop(Faction faction);

        /// <summary>
        ///     Erzeugt eine neue Instanz eines passenden Unit Interop.
        /// </summary>
        /// <param name="faction"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        UnitInterop CreateUnitInterop(Faction faction, FactionItem item);

        #endregion

        #region Level Resolver

        /// <summary>
        ///     Resolves all Extensions and Stuff for a new Level.
        /// </summary>
        /// <param name="level">Level</param>
        void ResolveLevel(Level level);

        /// <summary>
        ///     Generates a State for the given Level.
        /// </summary>
        /// <param name="level">Level</param>
        /// <returns>New State</returns>
        LevelState CreateLevelState(Level level);

        #endregion
    }
}