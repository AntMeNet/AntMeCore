using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace AntMe
{
    /// <summary>
    /// Auflistung von Properties für Game Items, States und Infos.
    /// </summary>
    /// <typeparam name="T">Property-Basistyp</typeparam>
    public abstract class PropertyList<T> : IEnumerable<T> where T : Property
    {
        private readonly Dictionary<Type, T> _properties = new Dictionary<Type, T>();

        /// <summary>
        /// Direkter Zugriff auf bestimmte Property-Types.
        /// </summary>
        /// <param name="type">Gesuchter Property-Type.</param>
        /// <returns>Property-Instanz oder null, falls nicht vorhanden.</returns>
        public T this[Type type]
        {
            get
            {
                if (_properties.ContainsKey(type))
                    return _properties[type];
                return null;
            }
        }

        /// <summary>
        ///     Listet die enthaltenen Properties auf.
        /// </summary>
        [Browsable(false)]
        public IEnumerable<T> Properties
        {
            get { return _properties.Values; }
        }

        /// <summary>
        /// Liefert einen Enumerator für die Auflistung von Properties zurück.
        /// </summary>
        /// <returns>Enumerator</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return _properties.Values.GetEnumerator();
        }

        /// <summary>
        /// Liefert einen Enumerator für die Auflistung von Properties zurück.
        /// </summary>
        /// <returns>Enumerator</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _properties.Values.GetEnumerator();
        }

        /// <summary>
        ///     Fügt dem Item eine neue Eigenschaft hinzu.
        /// </summary>
        /// <param name="property">Hinzuzufügende Eigenschaft</param>
        public void AddProperty(T property)
        {
            if (property == null)
                throw new ArgumentNullException("property");

            Type type = property.GetType();
            if (_properties.ContainsKey(type))
                throw new InvalidOperationException("Property Type already added");

            ValidateAddProperty(property);

            _properties.Add(type, property);
        }

        /// <summary>
        /// Überschreibbarer Validator für neu hinzugefügte Properties.
        /// </summary>
        /// <param name="property">Neue hinzugefügtes Property</param>
        protected virtual void ValidateAddProperty(T property) { }

        /// <summary>
        ///     Prüft, ob das Item über die angefragte Eigenschaft verfügt.
        /// </summary>
        /// <typeparam name="V">Type der zu überprüfende Eigenschaft</typeparam>
        /// <returns>Ist die Eigenschaft enthalten</returns>
        public bool ContainsProperty<V>() where V : T
        {
            return _properties.ContainsKey(typeof(V));
        }

        /// <summary>
        ///     Gibt die angefragte Eigenschaft des Items zurück oder null, falls nicht vorhanden.
        /// </summary>
        /// <typeparam name="V">Datentyp der Eigenschaft</typeparam>
        /// <returns>Referenz auf die Eigenschaft oder null</returns>
        public V GetProperty<V>() where V : T
        {
            if (_properties.ContainsKey(typeof(V)))
                return _properties[typeof(V)] as V;
            return null;
        }
    }
}