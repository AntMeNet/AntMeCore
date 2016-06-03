﻿using System;
using System.Collections.Generic;

namespace AntMe
{
    /// <summary>
    /// Collection of Attributes of an Unit.
    /// </summary>
    public sealed class UnitAttributeCollection
    {
        private Dictionary<string, sbyte> values;

        /// <summary>
        /// Default Constructor.
        /// </summary>
        /// <param name="name">Name of this Attribute Category</param>
        /// <param name="attributes">List of Attributes</param>
        public UnitAttributeCollection(string name, IDictionary<string, sbyte> attributes)
        {
            Name = name;

            values = new Dictionary<string, sbyte>();
            int sum = 0;
            foreach (var item in attributes)
            {
                values.Add(item.Key, item.Value);
                sum += item.Value;
            }

            // Check if valid
            if (sum > 0)
                throw new ArgumentException("The Sum of all Attributes is greater than zero");
        }

        /// <summary>
        /// Gets or sets Name of the Unit Category.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// List of all available Keys.
        /// </summary>
        public IEnumerable<string> Keys { get { return values.Keys; } }

        /// <summary>
        /// Returns the Value of the given Key or 0 if not available.
        /// </summary>
        /// <param name="key">Attribute Key</param>
        /// <returns>Attribute Value or 0</returns>
        public sbyte GetValue(string key)
        {
            sbyte result = 0;
            values.TryGetValue(key, out result);
            return result;
        }
    }
}
