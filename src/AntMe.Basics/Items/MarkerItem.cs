using System;
using AntMe.Basics.ItemProperties;

namespace AntMe.Basics.Items
{
    /// <summary>
    ///     Represents a Ant Marker.
    /// </summary>
    public class MarkerItem : FactionItem
    {
        private readonly float minRadius;
        private readonly SmellableProperty smellable;

        private readonly float step;

        public MarkerItem(SimulationContext context, Faction faction, Vector2 position, float radius, int information)
            : base(context, faction, position, radius, Angle.Right)
        {
            Information = information;

            smellable = GetProperty<SmellableProperty>();
            if (smellable == null)
                throw new NotSupportedException("Marker has no Smellable Property");

            minRadius = Settings.GetFloat<MarkerItem>("MinRadius").Value;
            var maxRadius = Settings.GetFloat<MarkerItem>("MaxRadius").Value;
            var volume = Settings.GetFloat<MarkerItem>("Volume").Value;

            var finalRadius = Math.Max(minRadius, Math.Min(maxRadius, radius));
            TotalAge = (int) (volume / finalRadius);
            step = (finalRadius - minRadius) / TotalAge;
            CurrentAge = 0;
        }

        /// <summary>
        ///     Returns the total Age of this Marker.
        /// </summary>
        public int TotalAge { get; }

        /// <summary>
        ///     Returns the current Age of this Marker.
        /// </summary>
        public int CurrentAge { get; private set; }

        /// <summary>
        ///     Gets the Marker Information.
        /// </summary>
        public int Information { get; }

        protected override void OnUpdate()
        {
            // Marker vergrößern
            CurrentAge++;
            Radius = minRadius + CurrentAge * step;

            // Marker entfernen
            if (CurrentAge >= TotalAge)
                Engine.RemoveItem(this);
        }
    }
}