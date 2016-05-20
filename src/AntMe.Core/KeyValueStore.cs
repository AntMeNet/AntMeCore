using System;
using System.Collections.Generic;
using System.IO;

namespace AntMe
{
    /// <summary>
    /// Settings-Container für Item/Faction/Property-Settings.
    /// </summary>
    public sealed class KeyValueStore
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
        public KeyValueStore()
        {
            global = new Dictionary<string, string>();
            descriptions = new Dictionary<string, string>();
        }

        /// <summary>
        /// Initialize the Settings with the given Stream.
        /// </summary>
        /// <param name="stream">Stream</param>
        public KeyValueStore(Stream stream) : this()
        {
            Apply(stream);
        }

        /// <summary>
        /// Initialize the Settings with the given Settings File.
        /// </summary>
        /// <param name="filename">Dateinamen</param>
        public KeyValueStore(string filename) : this()
        {
            Apply(filename);
        }

        /// <summary>
        /// Initialize the Settings with another Settings-Instance as source.
        /// </summary>
        /// <param name="settings">Source Settings</param>
        public KeyValueStore(KeyValueStore settings) : this()
        {
            Apply(settings);
        }

        /// <summary>
        /// Generates the Full Key out of Type and Key.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">Key</param>
        /// <returns>Full Key</returns>
        private string FullKey<T>(string key)
        {
            return string.Format("{0}:{1}", typeof(T).FullName, key);
        }

        /// <summary>
        /// Overwrites Settings with the given Settings in a Stream.
        /// </summary>
        /// <param name="stream">Source Stream</param>
        public void Apply(Stream stream)
        {
            // TODO: Implement File Load.
            throw new NotImplementedException();
        }

        /// <summary>
        /// Overwrites Settings with the given Settings in a File.
        /// </summary>
        /// <param name="filename">Source File</param>
        public void Apply(string filename)
        {
            using (Stream stream = File.Open(filename, FileMode.Open))
            {
                Apply(stream);
            }
        }

        /// <summary>
        /// Overwrites Settings with the given Settings Instance.
        /// </summary>
        /// <param name="settings">Settings</param>
        public void Apply(KeyValueStore settings)
        {
            foreach (var key in settings.global.Keys)
            {
                string description = null;
                settings.descriptions.TryGetValue(key, out description);
                Set(key, settings.global[key], description);
            }
        }

        /// <summary>
        /// Sets the value for the given Key.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">Settings Key</param>
        /// <param name="value">Settings Value</param>
        /// <param name="description">Optional Description for this key</param>
        public void Set<T>(string key, string value, string description = null)
        {
            Set(FullKey<T>(key), value, description);
        }

        /// <summary>
        /// Sets the value for the given Key.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">Settings Key</param>
        /// <param name="value">Settings Value</param>
        /// <param name="description">Optional Description for this key</param>
        public void Set<T>(string key, int value, string description = null)
        {
            Set(FullKey<T>(key), value.ToString(), description);
        }

        /// <summary>
        /// Sets the value for the given Key.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">Settings Key</param>
        /// <param name="value">Settings Value</param>
        /// <param name="description">Optional Description for this key</param>
        public void Set<T>(string key, float value, string description = null)
        {
            Set(FullKey<T>(key), value.ToString(), description);
        }

        /// <summary>
        /// Sets the value for the given Key.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">Settings Key</param>
        /// <param name="value">Settings Value</param>
        /// <param name="description">Optional Description for this key</param>
        public void Set<T>(string key, bool value, string description = null)
        {
            Set(FullKey<T>(key), value.ToString(), description);
        }

        /// <summary>
        /// Sets the value for the given Key.
        /// </summary>
        /// <param name="key">Full Settings Key (incl. Type and Key)</param>
        /// <param name="value">Settings Value</param>
        /// <param name="description">Optional Description for this key</param>
        public void Set(string key, string value, string description = null)
        {
            // TODO: Check right syntax (full.type.name:key)

            global[key] = value;

            // Set Description (if available)
            if (!string.IsNullOrEmpty(description))
                descriptions[key] = description;
        }

        /// <summary>
        /// Sets the value for the given Key.
        /// </summary>
        /// <param name="key">Full Settings Key (incl. Type and Key)</param>
        /// <param name="value">Settings Value</param>
        /// <param name="description">Optional Description for this key</param>
        public void Set(string key, int value, string description = null)
        {
            Set(key, value.ToString(), description);
        }

        /// <summary>
        /// Sets the value for the given Key.
        /// </summary>
        /// <param name="key">Full Settings Key (incl. Type and Key)</param>
        /// <param name="value">Settings Value</param>
        /// <param name="description">Optional Description for this key</param>
        public void Set(string key, float value, string description = null)
        {
            Set(key, value.ToString(), description);
        }

        /// <summary>
        /// Sets the value for the given Key.
        /// </summary>
        /// <param name="key">Full Settings Key (incl. Type and Key)</param>
        /// <param name="value">Settings Value</param>
        /// <param name="description">Optional Description for this key</param>
        public void Set(string key, bool value, string description = null)
        {
            Set(key, value.ToString(), description);
        }

        /// <summary>
        /// Enumerates all Settings Keys.
        /// </summary>
        public IEnumerable<string> Keys
        {
            get { return global.Keys; }
        }

        /// <summary>
        /// Clones the full set of Settings.
        /// </summary>
        /// <returns>Full Copy of Settings</returns>
        public KeyValueStore Clone()
        {
            return new KeyValueStore(this);
        }

        /// <summary>
        /// Merge Settings with the given Settings Instance.
        /// </summary>
        /// <param name="settings">Additional Settings</param>
        /// <returns>New Instance with merged Settings</returns>
        public KeyValueStore Merge(KeyValueStore settings)
        {
            KeyValueStore result = new KeyValueStore(this);
            result.Apply(settings);
            return result;
        }

        /// <summary>
        /// Sets the Value with the given Key interpreted as String.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">Settings Key</param>
        /// <returns>Value</returns>
        public string GetString<T>(string key)
        {
            string result;
            if (global.TryGetValue(FullKey<T>(key), out result))
                return result;
            return string.Empty;
        }

        /// <summary>
        /// Sets the Value with the given Key interpreted as String.
        /// </summary>
        /// <param name="key">Full Settings Key (incl. Type and Key)</param>
        /// <returns>Value</returns>
        public string GetString(string key)
        {
            string result;
            if (global.TryGetValue(key, out result))
                return result;
            return string.Empty;
        }

        /// <summary>
        /// Sets the Value with the given Key interpreted as Integer.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">Settings Key</param>
        /// <returns>Value</returns>
        public int? GetInt<T>(string key)
        {
            string value = GetString<T>(key);
            int result;
            if (int.TryParse(value, out result))
                return result;
            return null;
        }

        /// <summary>
        /// Sets the Value with the given Key interpreted as Integer.
        /// </summary>
        /// <param name="key">Full Settings Key (incl. Type and Key)</param>
        /// <returns>Value</returns>
        public int? GetInt(string key)
        {
            string value = GetString(key);
            int result;
            if (int.TryParse(value, out result))
                return result;
            return null;
        }

        /// <summary>
        /// Sets the Value with the given Key interpreted as Bool.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">Settings Key</param>
        /// <returns>Value</returns>
        public bool? GetBool<T>(string key)
        {
            string value = GetString<T>(key);
            bool result;
            if (bool.TryParse(value, out result))
                return result;
            return null;
        }

        /// <summary>
        /// Sets the Value with the given Key interpreted as Bool.
        /// </summary>
        /// <param name="key">Full Settings Key (incl. Type and Key)</param>
        /// <returns>Value</returns>
        public bool? GetBool(string key)
        {
            string value = GetString(key);
            bool result;
            if (bool.TryParse(value, out result))
                return result;
            return null;
        }

        /// <summary>
        /// Sets the Value with the given Key interpreted as Float.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">Settings Key</param>
        /// <returns>Value</returns>
        public float? GetFloat<T>(string key)
        {
            string value = GetString<T>(key);
            float result;
            if (float.TryParse(value, out result))
                return result;
            return null;
        }

        /// <summary>
        /// Sets the Value with the given Key interpreted as Float.
        /// </summary>
        /// <param name="key">Full Settings Key (incl. Type and Key)</param>
        /// <returns>Value</returns>
        public float? GetFloat(string key)
        {
            string value = GetString(key);
            float result;
            if (float.TryParse(value, out result))
                return result;
            return null;
        }

        /// <summary>
        /// Returns the Description for the Settings with the given Key.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">Settings Key</param>
        /// <returns>Description</returns>
        public string GetDescription<T>(string key)
        {
            return GetDescription(FullKey<T>(key));
        }

        /// <summary>
        /// Returns the Description for the Settings with the given Key.
        /// </summary>
        /// <param name="key">Full Settings Key (incl. Type and Key)</param>
        /// <returns>Description</returns>
        public string GetDescription(string key)
        {
            string result;
            if (descriptions.TryGetValue(key, out result))
                return result;
            return string.Empty;
        }

        /// <summary>
        /// Saves all Settings to a File.
        /// </summary>
        /// <param name="filename">Filename</param>
        public void Save(string filename)
        {
            using (Stream stream = File.Open(filename, FileMode.Create))
            {
                Save(stream);
            }
        }

        /// <summary>
        /// Saves all Settings to a File.
        /// </summary>
        /// <param name="stream">Output Stream</param>
        public void Save(Stream stream)
        {
            // TODO: Implement Settings Save
            throw new NotImplementedException();
        }
    }
}
