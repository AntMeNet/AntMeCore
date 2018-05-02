namespace AntMe
{
    /// <summary>
    /// Base Class for all Map Property States.
    /// </summary>
    public abstract class MapStateProperty : StateProperty
    {
        /// <summary>
        /// Reference to the Map.
        /// </summary>
        protected readonly Map Map;

        /// <summary>
        /// Reference to the Map Property.
        /// </summary>
        protected new readonly MapProperty Property;

        /// <summary>
        /// Default Constructor for the Deserializer.
        /// </summary>
        protected MapStateProperty() { }

        /// <summary>
        /// Default Constructor for the Type Mapper.
        /// </summary>
        /// <param name="map">Related Map</param>
        /// <param name="property">Related Engine Property</param>
        protected MapStateProperty(Map map, MapProperty property) : base(property)
        {
            Map = map;
            Property = property;
        }
    }
}
