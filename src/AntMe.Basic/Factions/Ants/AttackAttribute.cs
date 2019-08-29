﻿namespace AntMe.Basics.Factions.Ants
{
    /// <summary>
    /// Attribute to set the Attack Value of the Ant.
    /// Has impact to AttackStrength (as Multiplier), AttackRange
    /// </summary>
    public class AttackAttribute : UnitAttribute
    {
        /// <summary>
        /// Default Constructor.
        /// </summary>
        /// <param name="value">Attack Value</param>
        public AttackAttribute(sbyte value) : base("attack", value)
        {
        }
    }
}
