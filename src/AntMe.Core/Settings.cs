using System;
using System.Collections.Generic;
using System.IO;

namespace AntMe
{
    /// <summary>
    /// Settings-Container für Item/Faction/Property-Settings.
    /// </summary>
    public sealed class Settings
    {
        // Item-Settings (global)
        // Faction-Settings (global, Slot)
        // FactionItem-Settings (global, Slot, Faction)
        // Property-Settings (global, Slot, Faction, Item)

        // Default, wird von Application überschrieben
        // Level (Clone und Default überschreiben)
        // Faction
        // Item
        // Property

        /*
            - Standard-Settings für Items, Factions, Properties (Wie?)
            - Überschreiben durch Applikationssettings
            - [Optional] Überschreiben durch Level-Settings
            - Clone für jeden Slot [0..7] und überschreiben durch Slot-Settings
            - Überschreiben der Slot-Settings durch entsprechende Faction-Settings
            
            Offene Fragen:
            - wie soll das Sammeln der Default-Settings aussehen? -> ISettingsConsumer?
            - Wie kann ein Item andere Defaults für Properties setzen?
            - Wie kann eine Faction andere Defaults für Properties/Items setzen?
            - Bedingte Settings (Properties in Items/Factions, Abhängigkeiten zu anderen Settings) -> calculatedValues?
        */

        /// <summary>
        /// globales Settings-Dictionary
        /// key = Typename:Keyname (bsp.: "AntMe.Factions.AntFaction:MaxAntCount")
        /// </summary>
        private Dictionary<string, string> global;

        /// <summary>
        /// Leerer Konstruktor.
        /// </summary>
        public Settings()
        {
            global = new Dictionary<string, string>();
        }

        /// <summary>
        /// Konstruktor mit einem initialen Stream.
        /// </summary>
        /// <param name="stream">Stream</param>
        public Settings(Stream stream) : this()
        {
            Apply(stream);
        }

        /// <summary>
        /// Konstruktor mit einer initialen Settings-Datei.
        /// </summary>
        /// <param name="filename">Dateinamen</param>
        public Settings(string filename) : this()
        {
            Apply(filename);
        }

        /// <summary>
        /// Konstruktor auf Basis einer anderen Settings-Liste.
        /// </summary>
        /// <param name="settings">Basis Settings</param>
        public Settings(Settings settings) : this()
        {
            Apply(settings);
        }

        private string FullKey<T>(string key)
        {
            return string.Format("{0}:{1}", typeof(T).FullName, key);
        }

        /// <summary>
        /// Fügt Settings aus einem Stream zu den aktuellen Settings hinzu.
        /// </summary>
        /// <param name="stream">Stream</param>
        public void Apply(Stream stream) { }

        /// <summary>
        /// Fügt Settings aus einer Datei zu den aktuellen Settings hinzu.
        /// </summary>
        /// <param name="filename">Datei</param>
        public void Apply(string filename)
        {
            using (Stream stream = File.Open(filename, FileMode.Open))
            {
                Apply(stream);
            }
        }

        /// <summary>
        /// Fügt Settings aus einem Stream zu den aktuellen Settings hinzu.
        /// </summary>
        /// <param name="settings">Quellsettings</param>
        public void Apply(Settings settings)
        {
            foreach (var key in settings.global.Keys)
            {
                Apply(key, settings.global[key]);
            }
        }

        /// <summary>
        /// Fügt den gegebenen Schlüssel in die Settings ein oder überschreibt ihn.
        /// </summary>
        /// <typeparam name="T">Datentyp für den diese Settings gelten</typeparam>
        /// <param name="key">Settings Name</param>
        /// <param name="value">Settings Wert</param>
        public void Apply<T>(string key, string value)
        {
            Apply(FullKey<T>(key), value);
        }

        /// <summary>
        /// Fügt den angegebenen Key ein.
        /// </summary>
        /// <param name="key">Dictionary Key (inkl. Type-Prefix)</param>
        /// <param name="value">Settings Value</param>
        private void Apply(string key, string value)
        {
            global[key] = value;
        }

        /// <summary>
        /// Fertigt eine Kopie der aktuellen Settings an.
        /// </summary>
        /// <returns>Kopie der Settings</returns>
        public Settings Clone()
        {
            return new Settings(this);
        }

        /// <summary>
        /// Erstellt einen Merge aus den aktuellen und den gegebenen Settings her.
        /// </summary>
        /// <param name="settings">Zusatzsettings</param>
        /// <returns>Kopie der aktuellen Settings</returns>
        public Settings Merge(Settings settings)
        {
            Settings result = new Settings(this);
            result.Apply(settings);
            return result;
        }

        /// <summary>
        /// Gibt den Wert des Setting-Keys zurück oder null, falls nicht vorhanden.
        /// </summary>
        /// <typeparam name="T">Aufrufender Type</typeparam>
        /// <param name="key">Setting Key</param>
        /// <returns>Setting Value</returns>
        public string GetString<T>(string key)
        {
            string result;
            if (global.TryGetValue(FullKey<T>(key), out result))
                return result;
            return string.Empty;
        }

        /// <summary>
        /// Gibt den Settings Wert als Integer zurück oder null, falls nicht vorhanden.
        /// </summary>
        /// <typeparam name="T">Aufrufender Type</typeparam>
        /// <param name="key">Settings key</param>
        /// <returns>Settings Wert</returns>
        public int? GetInt<T>(string key)
        {
            string value = GetString<T>(key);
            int result;
            if (int.TryParse(value, out result))
                return result;
            return null;
        }

        /// <summary>
        /// Gibt den Settings-Wert als bool zurück oder null, falls nicht vorhanden.
        /// </summary>
        /// <typeparam name="T">Aufrufender Type</typeparam>
        /// <param name="key">Settings Key</param>
        /// <returns>Settings Wert</returns>
        public bool? GetBool<T>(string key)
        {
            string value = GetString<T>(key);
            bool result;
            if (bool.TryParse(value, out result))
                return result;
            return null;
        }

        /// <summary>
        /// Gibt den Settings-Wert als float zurück oder null, falls nicht vorhanden.
        /// </summary>
        /// <typeparam name="T">Aufrufender Type</typeparam>
        /// <param name="key">Settings Key</param>
        /// <returns>Settings Value</returns>
        public float? GetFloat<T>(string key)
        {
            string value = GetString<T>(key);
            float result;
            if (float.TryParse(value, out result))
                return result;
            return null;
        }

        /// <summary>
        /// Speichert den aktuellen Stand der Settings in der gegebenen Datei.
        /// </summary>
        /// <param name="filename">Dateinamen</param>
        public void Save(string filename)
        {
            using (Stream stream = File.Open(filename, FileMode.Create))
            {
                Save(stream);
            }
        }

        /// <summary>
        /// Speichert den aktuellen Stand der Settings im gegebenen Stream.
        /// </summary>
        /// <param name="stream">Output Stream</param>
        public void Save(Stream stream)
        {
            throw new NotImplementedException();
        }
    }
}
