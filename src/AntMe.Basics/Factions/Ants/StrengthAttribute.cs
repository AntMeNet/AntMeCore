namespace AntMe.Basics.Factions.Ants
{
    /// <summary>
    /// Attribute to set the Strength Value of this Ant.
    /// </summary>
    public class StrengthAttribute : UnitAttribute
    {
        /// <summary>
        /// Default Constructor.
        /// </summary>
        /// <param name="value">Strength Value</param>
        public StrengthAttribute(sbyte value) : base("strength", value)
        {
        }
    }
}
