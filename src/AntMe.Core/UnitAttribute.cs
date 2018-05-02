using System;

namespace AntMe
{
    /// <summary>
    /// Base Attribute for all Unit Attributes.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public abstract class UnitAttribute : Attribute
    {
        /// <summary>
        /// Default Constructor.
        /// </summary>
        /// <param name="key">Attribute Key</param>
        /// <param name="value">Value</param>
        protected UnitAttribute(string key, sbyte value) : this(key, value, -1, 2)
        {
        }

        /// <summary>
        /// Sepcial Constructor to define also the minimum and maximum Value.
        /// </summary>
        /// <param name="key">Attribute Key</param>
        /// <param name="value">Value</param>
        /// <param name="minValue">Minimum Value</param>
        /// <param name="maxValue">Maximum Value</param>
        protected UnitAttribute(string key, sbyte value, sbyte minValue, sbyte maxValue)
        {
            Key = key;
            Value = value;
            MinValue = minValue;
            MaxValue = maxValue;
        }

        /// <summary>
        /// Attribute Key.
        /// </summary>
        public string Key { get; }

        /// <summary>
        /// Value of this Attribute.
        /// </summary>
        public sbyte Value { get; }

        /// <summary>
        /// Gets the minimum Value allowed for Value.
        /// </summary>
        public sbyte MinValue { get; }

        /// <summary>
        /// Gets the maximum Value allowed for Value.
        /// </summary>
        public sbyte MaxValue { get; }
    }
}
