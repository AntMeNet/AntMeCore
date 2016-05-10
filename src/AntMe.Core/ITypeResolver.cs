using System;

namespace AntMe
{
    /// <summary>
    /// Interface zur dynamischen Typ-Auflösung während der Simulation.
    /// </summary>
    public interface ITypeResolver
    {
        #region Engine Resolver

        /// <summary>
        /// Ermittelt alle registrierten Extensions für die übergebene Engine.
        /// </summary>
        /// <param name="engine">Referenz auf die neue Engine</param>
        void ResolveEngine(Engine engine);

        #endregion

        #region Item Resolver

        /// <summary>
        /// Interne Methode zum Auffüllen von neuen Items auf Basis der registrierten Properties.
        /// </summary>
        /// <param name="item">Neues Item</param>
        void ResolveItem(Item item);

        /// <summary>
        /// Erstellt ein neues State-Objekt für das übergebene Item.
        /// </summary>
        /// <param name="Item">Zugehöriges Item</param>
        /// <returns>State Objekt</returns>
        ItemState CreateItemState(Item Item);

        /// <summary>
        /// Erstellt ein neues Info-Objekt auf Basis der beiden übergebenen Items.
        /// </summary>
        /// <param name="Item">Item das im Info Objekt repräsentiert wird</param>
        /// <param name="observer">Item das auf das Basis-Item schaut.</param>
        /// <returns>Info</returns>
        ItemInfo CreateItemInfo(Item Item, Item observer);

        #endregion

        #region Faction Resolver

        /// <summary>
        /// Erstellt eine neue Faction auf Basis des übergebenen Factory Types oder null, 
        /// falls keine passende Faction gefunden werden konnte.
        /// </summary>
        /// <param name="factoryType">Typ der Spieler Factory</param>
        /// <param name="settings">Settings</param>
        /// <param name="level">Level</param>
        /// <returns>Neue Faction-Instanz</returns>
        Faction CreateFaction(Type factoryType, Settings settings, Level level);

        /// <summary>
        /// Füllt eine neue Faction mit den registrierten Properties.
        /// </summary>
        /// <param name="faction"></param>
        void ResolveFaction(Faction faction);

        /// <summary>
        /// Erstellt den State für diese Faction.
        /// </summary>
        /// <param name="faction">Faction</param>
        /// <returns>State</returns>
        FactionState CreateFactionState(Faction faction);

        /// <summary>
        /// Erstellt ein Info Objekt für diese Faction auf Basis des Beobachters
        /// </summary>
        /// <param name="faction">Faction</param>
        /// <param name="observer">Beobachter</param>
        /// <returns>Info Objekt</returns>
        FactionInfo CreateFactionInfo(Faction faction, Item observer);

        #endregion

        #region Interop Resolver

        /// <summary>
        /// Erzeugt eine neue Instanz eines passendes Factory Interop.
        /// </summary>
        /// <param name="faction"></param>
        /// <returns></returns>
        FactoryInterop CreateFactoryInterop(Faction faction);

        /// <summary>
        /// Erzeugt eine neue Instanz eines passenden Unit Interop.
        /// </summary>
        /// <param name="faction"></param>
        /// <returns></returns>
        UnitInterop CreateUnitInterop(Faction faction);

        #endregion

        #region Level Resolver

        /// <summary>
        /// Methode zum Anwenden von Level Properties und Extender
        /// </summary>
        /// <param name="level"></param>
        void ResolveLevel(Level level);

        #endregion

        #region Settings

        /// <summary>
        /// Liefert die Standard-Einstellungen für alle registrierten Elemente
        /// </summary>
        /// <returns></returns>
        Settings GetGlobalSettings();

        #endregion
    }
}
