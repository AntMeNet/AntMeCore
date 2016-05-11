using AntMe.Factions.Ants;
using AntMe.ItemProperties.Basics;
using System;

namespace AntMe.Items.Basics
{
    /// <summary>
    /// Represents an AntHill.
    /// </summary>
    public class AnthillItem : FactionItem
    {
        /// <summary>
        /// Default Radius for an AntHill.
        /// </summary>
        public const float HillRadius = 20f;

        public AnthillItem(SimulationContext context, AntFaction faction, Vector2 position)
            : base(context, faction, position, HillRadius, Angle.Right)
        {
            AntFactionSettings settings = new AntFactionSettings();

            var attackable = new AttackableProperty(this);
            if (attackable != null)
            {
                attackable.OnAttackableHealthChanged += (item, value) =>
                {
                    // Sollten die Hitpoints unter 0 kommen, ist der Ameisenhügel zerstört
                    if (value <= 0)
                        Engine.RemoveItem(this);
                };
            }
        }
    }
}