﻿using System.Collections.Generic;

namespace AntMe
{
    /// <summary>
    /// Custom Implementation of a read only Dictionary.
    /// </summary>
    /// <typeparam name="TKey">Dictionary Key</typeparam>
    /// <typeparam name="TValue">Dictionary Value</typeparam>
    public sealed class ReadOnlyDictionary<TKey, TValue>
    {
        private IDictionary<TKey, TValue> dictionary;

        /// <summary>
        /// Default Constructor for the Dictionary Wrapper.
        /// </summary>
        /// <param name="dictionary">Base Dictionary</param>
        public ReadOnlyDictionary(IDictionary<TKey, TValue> dictionary)
        {
            this.dictionary = dictionary;
        }

        /// <summary>
        /// Checks if the Dictionary contains the given Key.
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>Contains Key</returns>
        public bool ContainsKey(TKey key)
        {
            return dictionary.ContainsKey(key);
        }

        /// <summary>
        /// List of available Keys.
        /// </summary>
        public IEnumerable<TKey> Keys
        {
            get { return dictionary.Keys; }
        }

        /// <summary>
        /// Tries to get the Value of the given Key.
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">Value out Parameter</param>
        /// <returns>Return if the Key was available</returns>
        public bool TryGetValue(TKey key, out TValue value)
        {
            return dictionary.TryGetValue(key, out value);
        }

        /// <summary>
        /// Direct Access to Values.
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns></returns>
        public TValue this[TKey key]
        {
            get { return dictionary[key]; }
        }

        /// <summary>
        /// Returns the Number of containing Items.
        /// </summary>
        public int Count
        {
            get { return dictionary.Count; }
        }
    }
}
