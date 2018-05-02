using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace AntMe
{
    /// <summary>
    /// Base Type for Types with additional Properties.
    /// </summary>
    /// <typeparam name="T">Property Type</typeparam>
    public abstract class PropertyList<T> : IEnumerable<T> where T : Property
    {
        private readonly Dictionary<Type, T> _properties = new Dictionary<Type, T>();

        /// <summary>
        /// Gets the Property of the given Type.
        /// </summary>
        /// <param name="type">Requested Property Type</param>
        /// <returns>Property Instance or null</returns>
        public T this[Type type] => _properties.TryGetValue(type, out var result) ? result : null;

        /// <summary>
        /// List of all Properties.
        /// </summary>
        [Browsable(false)]
        public IEnumerable<T> Properties => _properties.Values;

        /// <summary>
        /// Generates an Enumerator for all Properties.
        /// </summary>
        /// <returns>Enumerator</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return _properties.Values.GetEnumerator();
        }

        /// <summary>
        /// Generates an Enumerator for all Properties.
        /// </summary>
        /// <returns>Enumerator</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _properties.Values.GetEnumerator();
        }

        /// <summary>
        /// Adds the given Property to the List.
        /// </summary>
        /// <param name="property">New Property</param>
        public void AddProperty(T property)
        {
            if (property == null)
                throw new ArgumentNullException(nameof(property));

            // Check for Type Collisions
            var type = property.GetType();
            if (_properties.ContainsKey(type))
                throw new InvalidOperationException("Property Type already added");

            ValidateAddProperty(property);

            _properties.Add(type, property);
        }

        /// <summary>
        /// Overwritable Validator for new Properties.
        /// </summary>
        /// <param name="property">New Property</param>
        protected virtual void ValidateAddProperty(T property) { }

        /// <summary>
        /// Checks if the Property is part of the List.
        /// </summary>
        /// <typeparam name="TV">Property Type to check</typeparam>
        /// <returns>Is in List</returns>
        public bool ContainsProperty<TV>() where TV : T
        {
            return ContainsProperty(typeof(TV));
        }

        /// <summary>
        /// Checks if the Property is part of the List.
        /// </summary>
        /// <param name="type">Property Type to check</param>
        /// <returns>Is in List</returns>
        public bool ContainsProperty(Type type)
        {
            if (type == null) return false;
            return _properties.ContainsKey(type);
        }

        /// <summary>
        /// Gets the Property with the requested Type.
        /// </summary>
        /// <typeparam name="TV">Property Type</typeparam>
        /// <returns>Instance of this Type or null</returns>
        public TV GetProperty<TV>() where TV : T
        {
            return GetProperty(typeof(TV)) as TV;
        }

        /// <summary>
        /// Gets the Property with the requested Type.
        /// </summary>
        /// <param name="type">Property Type</param>
        /// <returns>Instance of this Type or null</returns>
        public T GetProperty(Type type)
        {
            if (type != null && _properties.ContainsKey(type))
                return _properties[type];
            return null;
        }
    }
}