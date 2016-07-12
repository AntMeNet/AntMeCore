using System;

namespace AntMe
{
    /// <summary>
    /// Basisinterface für alle Extension Packs.
    /// </summary>
    public interface IExtensionPack
    {
        /// <summary>
        /// Gibt den Namen der Extension zurück.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gibt die Beschreibung dieser Extension zurück.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Gibt die Versionsnummer dieser Extension zurück.
        /// </summary>
        Version Version { get; }

        /// <summary>
        /// Gibt den Namen des Autors zurück.
        /// </summary>
        string Author { get; }

        /// <summary>
        /// Wird beim Laden der Extension aufgerufen. 
        /// Hier sollten alle Fragmente im typeMapper registriert werden.
        /// </summary>
        /// <param name="typeMapper">Aktiver Type Mapper</param>
        /// <param name="settings">Basis Settings</param>
        /// <param name="dictionary">Basis Dictionary</param>
        void Load(ITypeMapper typeMapper, KeyValueStore settings, KeyValueStore dictionary);
    }
}
