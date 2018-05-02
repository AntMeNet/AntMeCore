using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

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
        private readonly Dictionary<string, ValueDescriptionEntry> _storage;

        /// <summary>
        /// Dictionary for informations and comments about the KeyValueStore (will be saved).
        /// </summary>
        public Dictionary<string, string> Common { get; }


        private int _nextId = 1;
        private readonly Tracer _tracer = new Tracer("AntMe.Core.KeyValueStore");

        /// <summary>
        /// Default Constructor
        /// </summary>
        public KeyValueStore()
        {
            _storage = new Dictionary<string, ValueDescriptionEntry>();
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
        /// Generates the Full Key out of Type and Key.
        /// </summary>
        /// <param name="type">Type</param>
        /// <param name="key">Key</param>
        /// <returns>Full Key</returns>
        private static string FullKey(Type type, string key)
        {
            return FullKey(type.FullName, key);
        }

        /// <summary>
        /// Generates the Full Key out of TypeKey and Key.
        /// </summary>
        /// <param name="typeKey">TypeKey</param>
        /// <param name="key">Key</param>
        /// <returns>Full Key</returns>
        private static string FullKey(string typeKey, string key)
        {
            if (!Regex.IsMatch(typeKey, @"\w+|\d+|\.|\+|(\<\w+|\d+|\.|\+|\>)"))
                throw new ArgumentException("The TypeKey matches not the definition: " + '"' + typeKey + '"');
            return $"{typeKey}:{key}";
        }

        /// <summary>
        /// Sets the Value and the Discription for the given Key.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        /// <param name="description">Optional Description for this key</param>
        public void Set<T>(string key, string value, string description = null)
        {
            Set(FullKey<T>(key), value, description);
        }

        /// <summary>
        /// Sets the Value and the Discription for the given key.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        /// <param name="description">Optional Description for this key</param>
        public void Set<T>(string key, int value, string description = null)
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
        public void Set<T>(string key, float value, string description = null)
        {
            Set(FullKey<T>(key), value.ToString(CultureInfo.InvariantCulture), description);
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
        /// <param name="key">Full key (incl. Type and Key)</param>
        /// <param name="valueDescriptionEntry">ValueDescriptionEntry</param>
        private void Set(string key, ValueDescriptionEntry valueDescriptionEntry)
        {
            Set(key, valueDescriptionEntry.Value, valueDescriptionEntry.Description);
        }

        /// <summary>
        /// Sets the Value and the Discription for the given Key.
        /// </summary>
        /// <param name="key">Full key (incl. Type and Key)</param>
        /// <param name="value"> Value</param>
        /// <param name="description">Optional Description for this key</param>
        public void Set(string key, string value, string description = null)
        {
            // TODO: Check right syntax (full.type.name:key)
            if (!_storage.TryGetValue(key, out var entry))
            {
                entry = new ValueDescriptionEntry() { Value = value, Description = description };
                _storage[key] = entry;
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(value))
                    entry.Value = value;
                if (!string.IsNullOrWhiteSpace(description))
                    entry.Description = description;
            }
        }

        /// <summary>
        /// Sets the Value and the Discription for the given Key.
        /// </summary>
        /// <param name="key">Full key (incl. Type and Key)</param>
        /// <param name="value">Value</param>
        /// <param name="description">Optional Description for this key</param>
        public void Set(string key, int value, string description = null)
        {
            Set(key, value.ToString(CultureInfo.InvariantCulture), description);
        }

        /// <summary>
        /// Sets the value for the given Key.
        /// </summary>
        /// <param name="key">Full Settings Key (incl. Type and Key)</param>
        /// <param name="value">Settings Value</param>
        /// <param name="description">Optional Description for this key</param>
        public void Set(string key, float value, string description = null)
        {
            Set(key, value.ToString(CultureInfo.InvariantCulture), description);
        }

        /// <summary>
        /// Sets the Value and the Discription for the given Key.
        /// </summary>
        /// <param name="key">Full key (incl. Type and Key)</param>
        /// <param name="value"> Value</param>
        /// <param name="description">Optional Description for this key</param>
        public void Set(string key, bool value, string description = null)
        {
            Set(key, value.ToString(), description);
        }

        /// <summary>
        /// Sets the Value and the Discription for the given Key.
        /// </summary>
        /// <param name="type">Type</param>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        /// <param name="description">Optional Description for this key</param>
        public void Set(Type type, string key, string value, string description = null)
        {
            Set(FullKey(type, key), value, description);
        }

        /// <summary>
        /// Sets the Value and the Discription for the given Key.
        /// </summary>
        /// <param name="type">Type</param>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        /// <param name="description">Optional Description for this key</param>
        public void Set(Type type, string key, int value, string description = null)
        {
            Set(FullKey(type, key), value, description);
        }

        /// <summary>
        /// Sets the Value and the Discription for the given Key.
        /// </summary>
        /// <param name="type">Type</param>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        /// <param name="description">Optional Description for this key</param>
        public void Set(Type type, string key, bool value, string description = null)
        {
            Set(FullKey(type, key), value, description);
        }

        /// <summary>
        /// Sets the Value and the Discription for the given Key.
        /// </summary>
        /// <param name="type">Type</param>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        /// <param name="description">Optional Description for this key</param>
        public void Set(Type type, string key, float value, string description = null)
        {
            Set(FullKey(type, key), value, description);
        }

        /// <summary>
        /// Enumerates all Settings Keys.
        /// </summary>
        public IEnumerable<string> Keys => _storage.Keys;

        /// <summary>
        /// Clones the instance of the KeyValueStore
        /// </summary>
        /// <returns>Full Copy of the KeyValueStore</returns>
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
            // merge storage dictonary 
            foreach (var key in keyValueStore._storage.Keys)
            {
                keyValueStore._storage.TryGetValue(key, out var newVde);
                if (_storage.Keys.Contains(key))
                {
                    _storage.TryGetValue(key, out var orgVde);
                    if (orgVde == null)
                        orgVde = new ValueDescriptionEntry();
                    if (newVde != null && string.IsNullOrEmpty(newVde.Value))
                        newVde.Value = orgVde.Value;
                    if (newVde != null && string.IsNullOrEmpty(newVde.Description))
                        newVde.Description = orgVde.Description;

                    _storage[key] = newVde;
                }
                else
                {
                    _storage.Add(key, newVde ?? new ValueDescriptionEntry());
                }
            }

            // merge common dictonary 
            foreach (var key in keyValueStore.Common.Keys)
            {
                keyValueStore.Common.TryGetValue(key, out var newValue);

                if (Common.Keys.Contains(key))
                {
                    Common.TryGetValue(key, out var orgValue);

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
        /// <param name="key">Key</param>
        /// <returns>Description</returns>
        public string GetDescription<T>(string key)
        {
            return GetDescription(FullKey<T>(key));
        }

        /// <summary>
        /// Gets the Description with the given Object-Key interpreted as String.
        /// </summary>
        /// <param name="type">Type</param>
        /// <param name="key">Key</param>
        /// <returns>Description</returns>
        public string GetDescription(Type type, string key)
        {
            return GetDescription(FullKey(type, key));
        }

        /// <summary>
        /// Gets the Description with the given Object-Key interpreted as String.
        /// </summary>
        /// <param name="key">Full Key (incl. Type and Key)</param>
        /// <returns>Description</returns>
        public string GetDescription(string key)
        {
            if (_storage.TryGetValue(key, out var result))
                return result.Description;
            return string.Empty;
        }

        /// <summary>
        /// Gets the Value with the given Object-Key interpreted as String.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">Key</param>
        /// <returns>Value</returns>
        public string GetString<T>(string key)
        {
            return GetString(FullKey<T>(key));
        }

        /// <summary>
        /// Gets the Value with the given Object-Key interpreted as String.
        /// </summary>
        /// <param name="type">Type</param>
        /// <param name="key">Key</param>
        /// <returns>Value</returns>
        public string GetString(Type type, string key)
        {
            return GetString(FullKey(type, key));
        }

        /// <summary>
        /// Gets the Value with the given Key interpreted as String.
        /// </summary>
        /// <param name="key">Full Key (incl. Type and Key)</param>
        /// <returns>Value</returns>
        public string GetString(string key)
        {
            if (_storage.TryGetValue(key, out var result))
                return result.Value;
            return string.Empty;
        }

        /// <summary>
        /// Gets the Value with the given Key interpreted as Integer.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">Key</param>
        /// <returns>Value</returns>
        public int? GetInt<T>(string key)
        {
            return GetInt(FullKey<T>(key));
        }

        /// <summary>
        /// Gets the Value with the given Key interpreted as Integer.
        /// </summary>
        /// <param name="type">Type</param>
        /// <param name="key">Key</param>
        /// <returns>Value</returns>
        public int? GetInt(Type type, string key)
        {
            return GetInt(FullKey(type, key));
        }

        /// <summary>
        /// Sets the Value with the given Key interpreted as Integer.
        /// </summary>
        /// <param name="key">Full Key (incl. Type and Key)</param>
        /// <returns>Value</returns>
        public int? GetInt(string key)
        {
            string value = GetString(key);
            int result;
            if (int.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out result))
                return result;
            return null;
        }

        /// <summary>
        /// Sets the Value with the given Key interpreted as Bool.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">Key</param>
        /// <returns>Value</returns>
        public bool? GetBool<T>(string key)
        {
            return GetBool(FullKey<T>(key));
        }

        /// <summary>
        /// Gets the Value with the given Key interpreted as Bool.
        /// </summary>
        /// <param name="type">Type</param>
        /// <param name="key">Key</param>
        /// <returns>Value</returns>
        public bool? GetBool(Type type, string key)
        {
            return GetBool(FullKey(type, key));
        }

        /// <summary>
        /// Sets the Value with the given Key interpreted as Bool.
        /// </summary>
        /// <param name="key">Full Key (incl. Type and Key)</param>
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
        /// <param name="key">Key</param>
        /// <returns>Value</returns>
        public float? GetFloat<T>(string key)
        {
            return GetFloat(FullKey<T>(key));
        }

        /// <summary>
        /// Gets the Value with the given Key interpreted as Float.
        /// </summary>
        /// <param name="type">Type</param>
        /// <param name="key">Key</param>
        /// <returns>Value</returns>
        public float? GetFloat(Type type, string key)
        {
            return GetFloat(FullKey(type, key));
        }

        /// <summary>
        /// Sets the Value with the given Key interpreted as Float.
        /// </summary>
        /// <param name="key">Full Key (incl. Type and Key)</param>
        /// <returns>Value</returns>
        public float? GetFloat(string key)
        {
            string value = GetString(key);
            if (float.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var result))
                return result;
            return null;
        }

        /// <summary>
        /// Gets the Description with the given Key interpreted as String.
        /// </summary>
        /// <param name="key">Full Key (incl. Type and Key)</param>
        /// <returns>Description</returns>
        private ValueDescriptionEntry GetValueDescriptionEntry(string key)
        {
            if (_storage.TryGetValue(key, out var result))
                return result;
            return null;
        }

        private void Load(Stream stream)
        {

            List<string> errors = new List<string>();

            using (StreamReader sr = new StreamReader(stream))
            {
                int currentLine = 0;
                string currentTypeKey = "";

                while (sr.Peek() >= 0)
                {
                    currentLine++;
                    string rawData = sr.ReadLine();
                    if (rawData != null)
                    {
                        string editedData = rawData.TrimStart(' ').TrimEnd(' ');

                        if (editedData.StartsWith("["))
                        {
                            if (editedData.IndexOf(']') == -1)
                            {
                                errors.Add(
                                    $"ERROR Line {currentLine}: missing ']' (end of TypeKey deklaration)");
                                _tracer.Trace(System.Diagnostics.TraceEventType.Error, _nextId++,
                                    $"ERROR Line {currentLine}: missing ']' (end of TypeKey deklaration)");
                                continue;
                            }

                            currentTypeKey = editedData.Substring(1, editedData.IndexOf(']') - 1).TrimStart(' ').TrimEnd(' ');
                        }
                        else if (editedData.Contains("="))
                        {

                            string key = editedData.Substring(0, editedData.IndexOf('=')).TrimEnd(' ');
                            editedData = editedData.Remove(0, editedData.IndexOf('=') + 1);

                            if (currentTypeKey == "Common")
                            {
                                Common.Add(key, editedData.TrimStart(' ').TrimEnd(' '));
                                continue;
                            }
                            var vde = new ValueDescriptionEntry();

                            if (editedData.IndexOf("//", StringComparison.Ordinal) >= 0)
                            {
                                vde.Value = editedData.Substring(0, editedData.IndexOf("//", StringComparison.Ordinal)).TrimStart(' ').TrimEnd(' ');
                                vde.Description = editedData.Remove(0, editedData.IndexOf("//", StringComparison.Ordinal) + 2).TrimStart(' ').TrimEnd(' ');
                            }
                            else
                            {
                                vde.Value = editedData.TrimStart(' ').TrimEnd(' ');
                            }

                            if (string.IsNullOrWhiteSpace(vde.Value))
                            {
                                errors.Add($"ERROR Line {currentLine}: missing Value");
                                _tracer.Trace(System.Diagnostics.TraceEventType.Error, _nextId++,
                                    $"ERROR Line {currentLine}: missing Value");
                            }
                            Set(FullKey(currentTypeKey, key), vde);

                        }
                        else if (string.IsNullOrWhiteSpace(editedData))
                        {
                        }
                        else
                        {
                            errors.Add(
                                $"ERROR Line {currentLine}: could not be deserialized, make sure it matches the Format([TYPEKEY] or KEY=VALUE//DESCRIPTION)");
                            _tracer.Trace(System.Diagnostics.TraceEventType.Error, _nextId++,
                                $"ERROR Line {currentLine}: could not be deserialized, make sure it matches the Format([TYPEKEY] or KEY=VALUE//DESCRIPTION)");
                        }
                    }
                }
                sr.Close();
            }

            if (errors.Count > 0)
                throw new AggregateException(string.Join(Environment.NewLine, errors.ToArray()));
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

            var sw = new StreamWriter(stream);

            if (Common.Count > 0)
            {
                sw.WriteLine("[Common]");
                foreach (var item in Common)
                {
                    sw.WriteLine("{0}={1}", item.Key, item.Value);
                }
                sw.WriteLine("");
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
                    foreach (var key in _storage.Where(k => k.Key.StartsWith(string.Format("{0}:", typekey))).Select(k => k.Key.Substring(k.Key.IndexOf(":", StringComparison.Ordinal) + 1)).ToArray())
                    {
                        string fullkey = $"{typekey}:{key}";
                        ValueDescriptionEntry vde = GetValueDescriptionEntry(fullkey);
                        keyLength = Math.Max(keyLength, key.Length);
                        valueLength = Math.Max(valueLength, vde.Value.Length);
                    }
                }

                foreach (var key in _storage.Where(k => k.Key.StartsWith(string.Format("{0}:", typekey))).Select(k => k.Key.Substring(k.Key.IndexOf(":", StringComparison.Ordinal) + 1)).ToArray())
                {
                    string fullkey = $"{typekey}:{key}";
                    ValueDescriptionEntry vde = GetValueDescriptionEntry(fullkey);
                    string description = " // ";
                    if (string.IsNullOrWhiteSpace(vde.Description))
                        description = string.Empty;
                    else
                        description += vde.Description;
                    sw.WriteLine("{0}={1}{2}", key.PadRight(keyLength), (vde.Value?.ToString(CultureInfo.InvariantCulture) ?? "").PadRight(valueLength), description);
                }
                sw.WriteLine();



            }
            sw.Flush();
            //sw.Dispose(); //TODO: Dispose was not used, because it closes the baseStream (.Net 4.5 has an option in the constructor to avoid this)

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