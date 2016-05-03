using System;
using System.Resources;

namespace AntMe
{
    /// <summary>
    /// Attribut zur Beschreibung einer Level-Klasse.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    [Serializable]
    public sealed class LevelDescriptionAttribute : Attribute
    {
        /// <summary>
        /// Parameterloser Konstuktor für den Serializer.
        /// </summary>
        public LevelDescriptionAttribute()
        {
            MinPlayerCount = 0;
            MaxPlayerCount = 8;
            Hidden = false;
        }

        /// <summary>
        /// Standard Konstruktor.
        /// </summary>
        /// <param name="guid">Level Guid als String</param>
        /// <param name="mapType">Typ der zu verwendenden Map</param>
        /// <param name="name">Name des Levels</param>
        /// <param name="description">Level-Beschreibung</param>
        public LevelDescriptionAttribute(string guid, Type mapType, string name, string description)
        {
            Init(guid, mapType, name, description);
        }

        /// <summary>
        /// Standard Konstruktor.
        /// </summary>
        /// <param name="guid">Level Guid als String</param>
        /// <param name="mapType">Typ der zu verwendenden Map</param>
        /// <param name="resourceType">Typ der Ressource für Name und Beschreibung</param>
        /// <param name="nameKey">Ressource Key für den Namen</param>
        /// <param name="descriptionKey">Ressource Key für die Beschreibung</param>
        public LevelDescriptionAttribute(string guid, Type mapType, Type resourceType, string nameKey, string descriptionKey)
        {
            // Ressourcen auflösen und Strings auslesen
            var resourceManager = new ResourceManager(resourceType);
            string name = resourceManager.GetString(nameKey);
            string description = resourceManager.GetString(descriptionKey);

            Init(guid, mapType, name, description);
        }

        private void Init(string guid, Type mapType, string name, string description)
        {
            // Check Id
            Guid id;
            if (!Guid.TryParse(guid, out id))
                throw new ArgumentException("Guid hat kein gültiges Format");

            // Check Name
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            // Check Description
            if (string.IsNullOrEmpty(description))
                throw new ArgumentNullException("description");

            // Check Map
            Map map = (Map)Activator.CreateInstance(mapType);
            map.CheckMap();

            Id = id;
            Name = name;
            Description = description;
            Map = map;

            MinPlayerCount = 0;
            MaxPlayerCount = 8;
            Hidden = false;
        }

        /// <summary>
        /// Gibt die eindeutige ID des Levels zurück.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gibt den Namen des Levels zurück.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gibt die Beschreibung des Levels zurück.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///     Gibt die maximale Anzahl Spieler an.
        /// </summary>
        public int MaxPlayerCount { get; set; }

        /// <summary>
        ///     Gibt die minimale Anzahl Spieler an.
        /// </summary>
        public int MinPlayerCount { get; set; }

        /// <summary>
        /// Gibt an, ob das Level beim Durchsuchen der Datei gefunden werden soll.
        /// </summary>
        public bool Hidden { get; set; }

        /// <summary>
        /// Gibt die Map zurück.
        /// </summary>
        public Map Map { get; set; }

        /// <summary>
        /// Validiert die Angaben des Attributs.
        /// </summary>
        public void Validate()
        {
            // ID prüfen
            if (Id == Guid.Empty)
                throw new ArgumentNullException("Id kann nicht empty sein");

            // Name prüfen
            if (string.IsNullOrEmpty(Name))
                throw new ArgumentNullException("Name kann nicht leer sein");

            // Description prüfen
            if (string.IsNullOrEmpty(Description))
                throw new ArgumentNullException("Description kann nicht leer sein");

            // Map prüfen
            if (Map == null)
                throw new ArgumentNullException("Map darf nicht null sein");
            Map.CheckMap();

            // Min Player
            if (MinPlayerCount < 0 || MinPlayerCount > 8)
                throw new ArgumentOutOfRangeException("MinPlayerCount muss zwischen 0 und 8 liegen.");

            // Max Player
            if (MaxPlayerCount < 0 || MaxPlayerCount > 8)
                throw new ArgumentOutOfRangeException("MaxPlayerCount muss zwischen 0 und 8 liegen.");
        }
    }
}