namespace AntMe.Basics.Factions.Ants
{
    /// <summary>
    /// Attribute to define the Caste Name of an Ant Unit.
    /// </summary>
    public class CasteAttribute : UnitGroupAttribute
    {
        /// <summary>
        /// Default Constructor.
        /// </summary>
        /// <param name="name">Name of the Caste</param>
        public CasteAttribute(string name) : base(name)
        {
        }
    }
}
