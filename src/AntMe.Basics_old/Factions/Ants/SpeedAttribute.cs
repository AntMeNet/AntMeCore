namespace AntMe.Basics.Factions.Ants
{
    /// <summary>
    /// Attribute to set the Speed Value of the Ant.
    /// Has Impact to WalkingSpeed, RotationSpeed, RecoveryTime
    /// </summary>
    public class SpeedAttribute : UnitAttribute
    {
        /// <summary>
        /// Default Constructor.
        /// </summary>
        /// <param name="value">Speed Value</param>
        public SpeedAttribute(sbyte value) : base("speed", value)
        {
        }
    }
}
