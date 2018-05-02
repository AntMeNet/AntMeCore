namespace AntMe
{
    /// <summary>
    /// Base Class for all Info Properties.
    /// </summary>
    public abstract class InfoProperty : Property
    {
        /// <summary>
        /// Reference to the related Property.
        /// </summary>
        protected readonly Property Property;

        /// <summary>
        /// References to the Observer.
        /// </summary>
        protected readonly Item Observer;

        /// <summary>
        /// Default Constructor for the Type Mapper.
        /// </summary>
        /// <param name="property">Property</param>
        /// <param name="observer">Observer</param>
        protected InfoProperty(Property property, Item observer)
        {
            Property = property;
            Observer = observer;
        }
    }
}
