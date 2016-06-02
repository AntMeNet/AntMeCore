using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AntMe
{
    /// <summary>
    /// KeyValueDesciption-Container for e.g.: Settings or Localizations
    /// </summary>
    public class KeyValueStore
    {

        /// <summary>
        /// Dictionary for all Values and Discriptions
        /// key = Typename:Keyname (e.g.: "AntMe.Factions.AntFaction:InitialAnthillCount","AntMe.Basics.Items.AntItem:AntRadius")
        /// </summary>
        private Dictionary<string, ValueDescriptionEntry> Storage;

        /// <summary>
        /// Dictionary for informations and comments about the KeyValueStore (will be saved).
        /// </summary>
        public Dictionary<string, string> Common { get; private set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public KeyValueStore()
        {
            Storage = new Dictionary<string, ValueDescriptionEntry>();
            Common = new Dictionary<string, string>();
        }

        /// <summary>
        /// Initialize a new KeyValueStore with the given Stream.
        /// </summary>
        /// <param name="stream">Stream</param>
        public KeyValueStore(Stream stream) : this()
        {
            Load(stream);
        }

        /// <summary>
        /// Initialize a new KeyValueStore with the given KeyValueStore File.
        /// </summary>
        /// <param name="filename">Dateinamen</param>
        public KeyValueStore(string filename) : this()
        {
            using (Stream stream = File.Open(filename, FileMode.Open))
            {
                Load(stream);
            }
        }

        /// <summary>
        /// Initialize a new KeyValueStore with another KeyValueStore-Instance as source.
        /// </summary>
        /// <param name="keyValueStore">Source KeyValueStore</param>
        public KeyValueStore(KeyValueStore keyValueStore) : this()
        {
            Merge(keyValueStore);
        }

        /// <summary>
        /// Generates the Full Key out of Type and Key.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">Key</param>
        /// <returns>Full Key</returns>
        private static string FullKey<T>(string key)
        {
            return FullKey(typeof(T).FullName, key);
        }
        /// <summary>
        /// Generates the Full Key out of TypeKey and Key.
        /// </summary>
        /// <param name="typeKey">TypeKey</param>
        /// <param name="key">Key</param>
        /// <returns>Full Key</returns>
        private static string FullKey(string typeKey, string key)
        {
           
            return string.Format("{0}:{1}", typeKey, key);
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
            KeyValueStore newStore = new KeyValueStore();
            newStore.Merge(this);
            return newStore;
        }


        /// <summary>
        /// Merge information with the given KeyValueStore-Instance.
        /// </summary>
        /// <param name="stream">Source KeyValueStore-Stream</param>
        public void Merge(Stream stream)
        {
            Merge(new KeyValueStore(stream));
        }

        /// <summary>
        /// Merge information with the given KeyValueStore-Instance.
        /// </summary>
        /// <param name="filename">Source KeyValueStore-File</param>
        public void Merge(string filename)
        {
            Merge(new KeyValueStore(filename));
        }

        /// <summary>
        /// Merge information with the given KeyValueStore-Instance.
        /// </summary>
        /// <param name="keyValueStore">Source KeyValueStore</param>
        public void Merge(KeyValueStore keyValueStore)
        {
            //merge storage dictonary 
            foreach (var key in keyValueStore.Storage.Keys)
            {
                ValueDescriptionEntry newVDE;
                keyValueStore.Storage.TryGetValue(key, out newVDE);
                if (Storage.Keys.Contains(key))
                {
                    ValueDescriptionEntry orgVDE;
                    Storage.TryGetValue(key, out orgVDE);
                    if (orgVDE == null)
                        orgVDE = new ValueDescriptionEntry();
                    if (string.IsNullOrEmpty(newVDE.Value))
                        newVDE.Value = orgVDE.Value;
                    if (string.IsNullOrEmpty(newVDE.Description))
                        newVDE.Value = orgVDE.Description;

                    Storage[key] = newVDE;
                }
                else
                {
                    Storage.Add(key, newVDE != null ? newVDE : new ValueDescriptionEntry());
                }
            }

            //merge common dictonary 
            foreach (var key in keyValueStore.Common.Keys)
            {
                string newValue;
                keyValueStore.Common.TryGetValue(key, out newValue);

                if (Common.Keys.Contains(key))
                {
                    string orgValue;
                    Common.TryGetValue(key, out orgValue);

                    if (string.IsNullOrEmpty(newValue))
                        newValue = orgValue;

                    Common[key] = newValue;
                }
                else
                {
                    Common.Add(key, newValue);
                }

            }
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

        private void Load(Stream stream)
        {

            List<string> Errors = new List<string>();

            using (StreamReader sr = new StreamReader(stream))
            {
                int currentLine = 0;
                string currentTypeKey = "";

                while (sr.Peek() >= 0)
                {
                    currentLine++;
                    bool waitforTypeKey = false;
                    string rawData = sr.ReadLine();
                    string editedData = rawData.TrimStart(' ').TrimEnd(' ');

                    if (editedData.StartsWith("["))
                    {
                        if (editedData.IndexOf(']') == -1)
                        {
                            waitforTypeKey = true;
                            Errors.Add("ERROR Line " + currentLine.ToString() + ": missing ']' (end of TypeKey deklaration)");
                            continue;
                        }

                        currentTypeKey = editedData.Substring(1, editedData.IndexOf(']') - 1).TrimStart(' ').TrimEnd(' ');
                        continue;
                    }
                    else if (waitforTypeKey)
                        continue;
                    else if (editedData.Contains("="))
                    {

                        string key = editedData.Substring(0, editedData.IndexOf('=')).TrimEnd(' ');
                        editedData = editedData.Remove(0, editedData.IndexOf('=') + 1);

                        if (currentTypeKey == "Common")
                        {
                            Common.Add(key, editedData.TrimStart(' ').TrimEnd(' '));
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

                        if (string.IsNullOrWhiteSpace(VDE.Value))
                            Errors.Add("WARNING Line " + currentLine.ToString() + ": missing Value");
                        if (string.IsNullOrWhiteSpace(VDE.Description))
                            Errors.Add("WARNING Line " + currentLine.ToString() + ": missing Description");

                        Set(FullKey(currentTypeKey, key), VDE);

                    }
                    else if (string.IsNullOrWhiteSpace(editedData))
                    {
                        continue;
                    }
                    else
                    {
                        Errors.Add("ERROR Line " + currentLine.ToString() + ": could not be deserialized, make sure it matches the Format([TYPEKEY] or KEY=VALUE//DESCRIPTION)");
                    }
                }
                sr.Close();
            }

            if (Errors.Count > 0)
                throw new AggregateException(string.Join(Environment.NewLine, Errors.ToArray()));
        }

        /// <summary>
        /// Saves all Settings to a File.
        /// </summary>
        /// <param name="filename">Filename</param>
        /// <param name="alignedValues">Optional easy to read output</param>
        public void Save(string filename, bool alignedValues = false)
        {
            using (Stream stream = File.Open(filename, FileMode.Create))
            {
                Save(stream, alignedValues);
            }
        }

        /// <summary>
        /// Saves all Settings to a File.
        /// </summary>
        /// <param name="stream">Output Stream</param>
        /// <param name="alignedValues">Optional easy to read output</param>
        public void Save(Stream stream, bool alignedValues = false)
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

                    int keyLength = 1;
                    int valueLength = 1;

                    if (alignedValues)
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
                        string description = " // ";
                        if (string.IsNullOrWhiteSpace(VDE.Description))
                            description = string.Empty;
                        else
                            description += VDE.Description;
                        sw.WriteLine("{0}={1}{2}", key.ToString().PadRight(keyLength), (VDE.Value != null ? VDE.Value : "").PadRight(valueLength), description);
                    }
                    sw.WriteLine();

                }

                sw.Close();
            }
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