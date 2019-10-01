using System;
using AntMe.Basics.Factions.Ants;
using AntMe.Basics.ItemProperties;

namespace AntMe.Basics.Items
{
    /// <summary>
    ///     Represents an Ant.
    /// </summary>
    public class AntItem : FactionItem
    {
        /// <summary>
        ///     Default Radius for Ants.
        /// </summary>
        public const float AntRadius = 2f;

        /// <summary>
        ///     Default Mass of Ants.
        /// </summary>
        public const float AntMass = 1f;

        /// <summary>
        ///     Reference to the Walking Property.
        /// </summary>
        private readonly WalkingProperty walking;

        /// <summary>
        ///     Creates a new Instance of an Ant.
        /// </summary>
        /// <param name="context">Simulation Context</param>
        /// <param name="attributes">List of Attributes</param>
        /// <param name="faction">Related Faction</param>
        /// <param name="position">Startposition</param>
        /// <param name="orientation">Startorientation</param>
        /// <param name="name">Name of the Ant</param>
        public AntItem(SimulationContext context, UnitAttributeCollection attributes, AntFaction faction,
            Vector2 position, Angle orientation, string name)
            : base(context, attributes, faction, position, AntRadius, orientation)
        {
            Name = name;

            // Gets the Reference to the walking Property
            walking = GetProperty<WalkingProperty>();
            if (walking == null)
                throw new NotSupportedException("There is no Walking Property");
        }

        /// <summary>
        ///     Returns the Name of this Ant.
        /// </summary>
        public string Name { get; }

        /// <summary>
        ///     Adds some more Information to the Item State.
        /// </summary>
        /// <param name="state">Item State</param>
        protected override void OnBeforeState(ItemState state)
        {
            var antState = (AntState) state;
            antState.Mode = AntStateMode.Idle;
            if (walking.Speed > 0f)
                antState.Mode = AntStateMode.Walk;
        }
    }
}