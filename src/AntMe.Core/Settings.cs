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
        /// <summary>
        /// Dictionary for Setting Values
        /// key = Typename:Keyname (e.g.: "AntMe.Factions.AntFaction:MaxAntCount")
        /// </summary>
        private Dictionary<string, string> global;

        /// <summary>
        /// Dictionary for Descriptions.
        /// </summary>
        private Dictionary<string, string> descriptions;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public Settings()
        {
            global = new Dictionary<string, string>();
            descriptions = new Dictionary<string, string>();
        }

        /// <summary>
        /// Initialize the Settings with the given Stream.
        /// </summary>
        /// <param name="stream">Stream</param>
        public Settings(Stream stream) : this()
        {
            Apply(stream);
        }

        /// <summary>
        /// Initialize the Settings with the given Settings File.
        /// </summary>
        /// <param name="filename">Dateinamen</param>
        public Settings(string filename) : this()
        {
            Apply(filename);
        }

        /// <summary>
        /// Initialize the Settings with another Settings as source.
        /// </summary>
        /// <param name="settings">Source Settings</param>
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
        public void Apply(Stream stream)
        {
            throw new NotImplementedException();
        }

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
                string description = null;
                settings.descriptions.TryGetValue(key, out description);
                Apply(key, settings.global[key], description);
            }
        }

        /// <summary>
        /// Fügt den gegebenen Schlüssel in die Settings ein oder überschreibt ihn.
        /// </summary>
        /// <typeparam name="T">Datentyp für den diese Settings gelten</typeparam>
        /// <param name="key">Settings Name</param>
        /// <param name="value">Settings Wert</param>
        /// <param name="description">Optional Description for this key</param>
        public void Apply<T>(string key, string value, string description = null)
        {
            Apply(FullKey<T>(key), value, description);
        }

        /// <summary>
        /// Fügt den gegebenen Schlüssel in die Settings ein oder überschreibt ihn.
        /// </summary>
        /// <typeparam name="T">Datentyp für den diese Settings gelten</typeparam>
        /// <param name="key">Settings Name</param>
        /// <param name="value">Settings Wert</param>
        /// <param name="description">Optional Description for this key</param>
        public void Apply<T>(string key, int value, string description = null)
        {
            Apply(FullKey<T>(key), value.ToString(), description);
        }

        /// <summary>
        /// Fügt den angegebenen Key ein.
        /// </summary>
        /// <param name="key">Dictionary Key (inkl. Type-Prefix)</param>
        /// <param name="value">Settings Value</param>
        /// <param name="description">Optional Description for this key</param>
        public void Apply(string key, string value, string description = null)
        {
            // TODO: Check right syntax (full.type.name:key)

            global[key] = value;

            // Set Description (if available)
            if (!string.IsNullOrEmpty(description))
                descriptions[key] = description;
        }

        public void Apply(string key, int value, string description = null)
        {
            Apply(key, value.ToString(), description);
        }

        public void Apply(string key, float value, string description = null)
        {
            Apply(key, value.ToString(), description);
        }

        public void Apply(string key, bool value, string description = null)
        {
            Apply(key, value.ToString(), description);
        }

        /// <summary>
        /// Enumerates all Settings Keys.
        /// </summary>
        public IEnumerable<string> Keys
        {
            get { return global.Keys; }
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

        public string GetString(string key)
        {
            string result;
            if (global.TryGetValue(key, out result))
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

        public int? GetInt(string key)
        {
            string value = GetString(key);
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

        public bool? GetBool(string key)
        {
            string value = GetString(key);
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

        public float? GetFloat(string key)
        {
            string value = GetString(key);
            float result;
            if (float.TryParse(value, out result))
                return result;
            return null;
        }

        public string GetDescription<T>(string key)
        {
            return GetDescription(FullKey<T>(key));
        }

        public string GetDescription(string key)
        {
            string result;
            if (descriptions.TryGetValue(key, out result))
                return result;
            return string.Empty;
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
