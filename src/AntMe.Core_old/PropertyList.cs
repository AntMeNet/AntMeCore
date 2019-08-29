﻿using System;
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
        private readonly Dictionary<Type, T> properties = new Dictionary<Type, T>();

        /// <summary>
        /// Gets the Property of the given Type.
        /// </summary>
        /// <param name="type">Requested Property Type</param>
        /// <returns>Property Instance or null</returns>
        public T this[Type type]
        {
            get
            {
                T result;
                if (properties.TryGetValue(type, out result))
                    return result;
                return null;
            }
        }

        /// <summary>
        /// List of all Properties.
        /// </summary>
        [Browsable(false)]
        public IEnumerable<T> Properties
        {
            get { return properties.Values; }
        }

        /// <summary>
        /// Generates an Enumerator for all Properties.
        /// </summary>
        /// <returns>Enumerator</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return properties.Values.GetEnumerator();
        }

        /// <summary>
        /// Generates an Enumerator for all Properties.
        /// </summary>
        /// <returns>Enumerator</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return properties.Values.GetEnumerator();
        }

        /// <summary>
        /// Adds the given Property to the List.
        /// </summary>
        /// <param name="property">New Property</param>
        public void AddProperty(T property)
        {
            if (property == null)
                throw new ArgumentNullException("property");

            // Check for Type Collisions
            Type type = property.GetType();
            if (properties.ContainsKey(type))
                throw new InvalidOperationException("Property Type already added");

            ValidateAddProperty(property);

            properties.Add(type, property);
        }

        /// <summary>
        /// Overwritable Validator for new Properties.
        /// </summary>
        /// <param name="property">New Property</param>
        protected virtual void ValidateAddProperty(T property) { }

        /// <summary>
        /// Checks if the Property is part of the List.
        /// </summary>
        /// <typeparam name="V">Property Type to check</typeparam>
        /// <returns>Is in List</returns>
        public bool ContainsProperty<V>() where V : T
        {
            return ContainsProperty(typeof(V));
        }

        /// <summary>
        /// Checks if the Property is part of the List.
        /// </summary>
        /// <param name="type">Property Type to check</param>
        /// <returns>Is in List</returns>
        public bool ContainsProperty(Type type)
        {
            if (type == null) return false;
            return properties.ContainsKey(type);
        }

        /// <summary>
        /// Gets the Property with the requested Type.
        /// </summary>
        /// <typeparam name="V">Property Type</typeparam>
        /// <returns>Instance of this Type or null</returns>
        public V GetProperty<V>() where V : T
        {
            return GetProperty(typeof(V)) as V;
        }

        /// <summary>
        /// Gets the Property with the requested Type.
        /// </summary>
        /// <param name="type">Property Type</param>
        /// <returns>Instance of this Type or null</returns>
        public T GetProperty(Type type)
        {
            if (type != null && properties.ContainsKey(type))
                return properties[type];
            return null;
        }
    }
}