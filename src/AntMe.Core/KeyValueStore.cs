using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AntMe
{
    /// <summary>
    /// Settings-Container für Item/Faction/Property-Settings.
    /// </summary>
    public sealed class KeyValueStore
    {

        /// <summary>
        /// Dictionary for all Values and Discriptions
        /// key = Typename:Keyname (e.g.: "AntMe.Factions.AntFaction:InitialAnthillCount","AntMe.Basics.Items.AntItem:AntRadius")
        /// </summary>
        private Dictionary<string, ValueDescriptionEntry> Storage;

        /// <summary>
        /// Dictionary for informations and comments about the KeyValueStore (will be saved).
        /// </summary>
        public Dictionary<string, string> Common { get; set; }

        /// <summary>
        /// Dictionary of errors that occurred while the Loading form file or stream.
        /// If empty no error occurred.
        /// key = fileName/"stream"
        /// </summary>
        public Dictionary<string, List<string>> LoadingErrors;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public KeyValueStore()
        {
            Storage = new Dictionary<string, ValueDescriptionEntry>();
            Common = new Dictionary<string, string>();
            LoadingErrors = new Dictionary<string, List<string>>();
        }

        /// <summary>
        /// Initialize the KeyValueStore with the given Stream.
        /// </summary>
        /// <param name="stream">Stream</param>
        public KeyValueStore(Stream stream) : this()
        {
            Apply(stream);
        }

        /// <summary>
        /// Initialize the KeyValueStore with the given KeyValueStore File.
        /// </summary>
        /// <param name="filename">Dateinamen</param>
        public KeyValueStore(string filename) : this()
        {
            Apply(filename);
        }

        /// <summary>
        /// Initialize the KeyValueStore with another KeyValueStore-Instance as source.
        /// </summary>
        /// <param name="keyValueStore">Source KeyValueStore</param>
        public KeyValueStore(KeyValueStore keyValueStore) : this()
        {
            Apply(keyValueStore);
        }

        /// <summary>
        /// Generates the Full Key out of Type and Key.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">Key</param>
        /// <returns>Full Key</returns>
        private static string FullKey<T>(string key)
        {
            return string.Format("{0}:{1}", typeof(T).FullName, key);
        }
        /// <summary>
        /// Generates the Full Key out of Type and Key.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">Key</param>
        /// <returns>Full Key</returns>
        private static string FullKey(string typeKey, string key)
        {
            return string.Format("{0}:{1}", typeKey, key);
        }

        /// <summary>
        /// Applies all given KeyValues in a Stream to the KeyValueStore.
        /// Overwrites existing KeyValues and adds new ones.
        /// </summary>
        /// <param name="stream">Source Stream</param>
        public void Apply(Stream stream)
        {
            // TODO: Implement File Load.
            throw new NotImplementedException();
        }

        /// <summary>
        /// Applies all given KeyValues in a File to the KeyValueStore.
        /// Overwrites existing KeyValues and adds new ones.
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
        /// Applies all given KeyValues in a keyValueStore to the KeyValueStore.
        /// Overwrites existing KeyValues and adds new ones.
        /// </summary>
        /// <param name="keyValueStore">Settings</param>
        public void Apply(KeyValueStore keyValueStore)
        {
            foreach (var key in keyValueStore.Storage.Keys)
            {
                ValueDescriptionEntry VDE = null;
                keyValueStore.Storage.TryGetValue(key, out VDE);
                Set(key, VDE);
            }
        }

        /// <summary>
        /// Sets the Value and the Discription for the given Key.
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
        /// Sets the Value and the Discription for the given Key.
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
        /// Sets the Value and the Discription for the given Key.
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
        /// Sets the Value and the Discription for the given Key.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        /// <param name="description">Optional Description for this key</param>
        public void Set<T>(string key, bool value, string description = null)
        {
            Set(FullKey<T>(key), value.ToString(), description);
        }


        /// <summary>
        /// Sets the Value and the Discription for the given Key.
        /// </summary>
        /// <param name="key">Full Key (incl. Type and Key)</param>
        /// <param name="valueDescriptionEntry">ValueDescriptionEntry</param>
        private void Set(string key, ValueDescriptionEntry valueDescriptionEntry)
        {
            Set(key, valueDescriptionEntry.Value, valueDescriptionEntry.Description);
        }

        /// <summary>
        /// Sets the Value and the Discription for the given Key.
        /// </summary>
        /// <param name="key">Full Key (incl. Type and Key)</param>
        /// <param name="value"> Value</param>
        /// <param name="description">Optional Description for this key</param>
        public void Set(string key, string value, string description = null)
        {
            // TODO: Check right syntax (full.type.name:key)
            ValueDescriptionEntry VDE;
            if (!Storage.TryGetValue(key, out VDE))
                VDE = new ValueDescriptionEntry() { Value = value, Description = description };
            else
            {
                if (value != null)
                    VDE.Value = value;
                if (description != null)
                    VDE.Description = description;
            }
            Storage[key] = VDE;

        }

        /// <summary>
        /// Sets the Value and the Discription for the given Key.
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
        /// Sets the Value and the Discription for the given Key.
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
            get { return Storage.Keys; }
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
        /// Gets the Description with the given Object-Key interpreted as String.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">Object-Key</param>
        /// <returns>Description</returns>
        public string GetDescription<T>(string key)
        {
            return GetDescription(FullKey<T>(key));
        }

        /// <summary>
        /// Gets the Description with the given Object-Key interpreted as String.
        /// </summary>
        /// <param name="key">Full Key (incl. Type and Key)</param>
        /// <returns>Description</returns>
        public string GetDescription(string key)
        {
            ValueDescriptionEntry result;
            if (Storage.TryGetValue(key, out result))
                return result.Description;
            return string.Empty;
        }

        /// <summary>
        /// Gets the Value with the given Object-Key interpreted as String.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">Object-Key</param>
        /// <returns>Value</returns>
        public string GetString<T>(string key)
        {
            ValueDescriptionEntry result;
            if (Storage.TryGetValue(FullKey<T>(key), out result))
                return result.Value;
            return string.Empty;
        }

        /// <summary>
        /// Gets the Value with the given Key interpreted as String.
        /// </summary>
        /// <param name="key">Full Key (incl. Type and Key)</param>
        /// <returns>Value</returns>
        public string GetString(string key)
        {
            ValueDescriptionEntry result;
            if (Storage.TryGetValue(key, out result))
                return result.Value;
            return string.Empty;
        }

        /// <summary>
        /// Gets the Value with the given Key interpreted as Integer.
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
        /// Gets the Description with the given Object-Key interpreted as String.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">Object-Key</param>
        /// <returns>Description</returns>
        private ValueDescriptionEntry GetValueDescriptionEntry<T>(string key)
        {
            return GetValueDescriptionEntry(FullKey<T>(key));
        }

        /// <summary>
        /// Gets the Description with the given Object-Key interpreted as String.
        /// </summary>
        /// <param name="key">Full Key (incl. Type and Key)</param>
        /// <returns>Description</returns>
        private ValueDescriptionEntry GetValueDescriptionEntry(string key)
        {
            ValueDescriptionEntry result;
            if (Storage.TryGetValue(key, out result))
                return result;
            return null;
        }

        /// <summary>
        /// Loads a KeyValueStore from a given stream
        /// </summary>
        /// <param name="stream">Stream</param>
        /// <returns>KeyValueStore</returns>
        public static KeyValueStore Load(Stream stream)
        {
            return Load(stream, "Stream");
        }

        /// <summary>
        /// Loads a KeyValueStore from a given file
        /// </summary>
        /// <param name="filename">Filename</param>
        /// <returns>KeyValueStore</returns>
        public static KeyValueStore Load(string filename)
        {
            using (Stream stream = File.Open(filename, FileMode.Open))
            {
                return Load(stream, filename);
            }
        }

        private static KeyValueStore Load(Stream stream, string source)
        {

            KeyValueStore locaKeyValueStore = new KeyValueStore();
            List<string> locaErrors = new List<string>();

            using (StreamReader sr = new StreamReader(stream))
            {
                int currentLine = 0;
                string currentTypeKey = "";

                while (sr.Peek() >= 0)
                {
                    currentLine++;
                    string rawData = sr.ReadLine();
                    string editedData = rawData.TrimStart(' ').TrimEnd(' ');

                    if (editedData.StartsWith("["))
                    {
                        currentTypeKey = editedData.Substring(1, editedData.IndexOf(']') - 1).TrimStart(' ').TrimEnd(' ');
                        continue;
                    }
                    else if (editedData.Contains("="))
                    {

                        string key = editedData.Substring(0, editedData.IndexOf('=')).TrimEnd(' ');
                        editedData = editedData.Remove(0, editedData.IndexOf('=') + 1);

                        if (currentTypeKey == "Common")
                        {
                            locaKeyValueStore.Common.Add(key, editedData.TrimStart(' ').TrimEnd(' '));
                            continue;
                        }
                        ValueDescriptionEntry VDE = new ValueDescriptionEntry();

                        if (editedData.IndexOf("//") >= 0)
                        {
                            VDE.Value = editedData.Substring(0, editedData.IndexOf("//")).TrimStart(' ').TrimEnd(' ');
                            VDE.Description = editedData.Remove(0, editedData.IndexOf("//") + 2).TrimStart(' ').TrimEnd(' ');
                        }
                        else
                        {
                            VDE.Value = editedData.TrimStart(' ').TrimEnd(' ');
                        }

                        locaKeyValueStore.Set(FullKey(currentTypeKey, key), VDE);

                    }
                    else if (string.IsNullOrWhiteSpace(editedData))
                    {
                        continue;
                    }
                    else
                    {
                        locaErrors.Add("ERROR Line " + currentLine.ToString() + ": could not be deserialized, make sure it matches the Format([TYPEKEY] or KEY=VALUE(//DESCRIPTION))");
                    }
                }
            }

            if (locaErrors.Count > 0)
                locaKeyValueStore.LoadingErrors.Add(source, locaErrors);


            return locaKeyValueStore;
        }

        /// <summary>
        /// Saves all Settings to a File.
        /// </summary>
        /// <param name="filename">Filename</param>
        /// <param name="easyReading">Optional easy to read output</param>
        public void Save(string filename, bool easyReading = false)
        {
            using (Stream stream = File.Open(filename, FileMode.Create))
            {
                Save(stream, easyReading);
            }
        }

        /// <summary>
        /// Saves all Settings to a File.
        /// </summary>
        /// <param name="stream">Output Stream</param>
        /// <param name="easyReading">Optional easy to read output</param>
        public void Save(Stream stream, bool easyReading = false)
        {

            using (StreamWriter sw = new StreamWriter(stream))
            {

                if (Common.Count > 0)
                {
                    sw.WriteLine("[Common]");
                    foreach (var item in Common)
                    {
                        sw.WriteLine("{0}={1}", item.Key, item.Value);
                    }
                }

                string[] keytemp = Keys.ToArray();

                var typeKeys = keytemp.Select(k => k.Substring(0, k.IndexOf(':'))).Distinct();

                foreach (var typekey in typeKeys.OrderBy(k => k))
                {
                    sw.WriteLine("[{0}]", typekey);

                    int keyLength = 0;
                    int valueLength = 0;

                    if (easyReading)
                    {
                        foreach (var key in Storage.Where(k => k.Key.StartsWith(typekey)).Select(k => k.Key.Substring(k.Key.IndexOf(":") + 1)).ToArray())
                        {
                            string fullkey = string.Format("{0}:{1}", typekey, key);
                            ValueDescriptionEntry VDE = GetValueDescriptionEntry(fullkey);
                            keyLength = Math.Max(keyLength, key.Length);
                            valueLength = Math.Max(valueLength, VDE.Value.Length);
                        }
                    }

                    foreach (var key in Storage.Where(k => k.Key.StartsWith(typekey)).Select(k => k.Key.Substring(k.Key.IndexOf(":") + 1)).ToArray())
                    {
                        string fullkey = string.Format("{0}:{1}", typekey, key);
                        ValueDescriptionEntry VDE = GetValueDescriptionEntry(fullkey);
                        sw.WriteLine("{0}={1}//{2}", key.ToString().PadRight(keyLength), (VDE.Value != null ? VDE.Value : "no Value").PadRight(valueLength), VDE.Description != null ? VDE.Description : "no Description");
                    }
                    sw.WriteLine();

                }

                sw.Close();
            }

            //// TODO: Implement Settings Save
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Nested class for data storing.
        /// </summary>
        private class ValueDescriptionEntry
        {

            public string Value { get; set; }

            public string Description { get; set; }

        }

    }
}
