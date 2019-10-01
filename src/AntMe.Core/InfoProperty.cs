namespace AntMe
{
    /// <summary>
    ///     Base Class for all Info Properties.
    /// </summary>
    public abstract class InfoProperty : Property
    {
        /// <summary>
        ///     Reference to the related Property.
        /// </summary>
        protected readonly Property Property;

        /// <summary>
        ///     Default Constructor for the Type Mapper.
        /// </summary>
        /// <param name="property">Property</param>
        protected InfoProperty(Property property)
        {
            Property = property;
        }
    }
}