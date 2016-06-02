namespace AntMe.Basics.Factions.Ants
{
    /// <summary>
    /// Attribute to set the Defense Value of this Ant.
    /// </summary>
    public class DefenseAttribute : UnitAttribute
    {
        /// <summary>
        /// Default Constructor.
        /// </summary>
        /// <param name="value">Defense Value</param>
        public DefenseAttribute(sbyte value) : base("defense", value)
        {
        }
    }
}
