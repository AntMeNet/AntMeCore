using AntMe.ItemProperties.Basics;
using System;

namespace AntMe.Items.Basics
{
    /// <summary>
    /// Represents an Sugar Hill.
    /// </summary>
    public class SugarItem : Item
    {
        /// <summary>
        /// Default Radius for a Sugar Hill.
        /// </summary>
        public const float SugarRadius = 10f;

        /// <summary>
        /// Maximale Ladung eines Zuckerberges.
        /// </summary>
        public const int SugarMaxCapacity = 1000;

        private SugarCollectableProperty _sugar;

        public SugarItem(SimulationContext context, Vector2 position, int amount)
            : base(context, position, SugarRadius, Angle.Right)
        {
            // Todesbedingung
            //_sugar = GetProperty<SugarCollectableProperty>();
            //_sugar.OnAmountChanged += (good, newValue) =>
            //{
            //    if (newValue <= 0)
            //    {
            //        // Entfernen von der Landkarte
            //        Engine.RemoveItem(this);
            //    }
            //};
        }
    }
}