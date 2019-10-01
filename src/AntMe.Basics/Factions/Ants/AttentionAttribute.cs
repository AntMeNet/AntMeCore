namespace AntMe.Basics.Factions.Ants
{
    /// <summary>
    ///     Attribute to set the Attention Value for this Ant.
    ///     Has impact to ViewRange, ViewAngle
    /// </summary>
    public class AttentionAttribute : UnitAttribute
    {
        /// <summary>
        ///     Default Constructor.
        /// </summary>
        /// <param name="value">Attention Value</param>
        public AttentionAttribute(sbyte value) : base("attention", value)
        {
        }
    }
}