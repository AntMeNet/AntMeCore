using AntMe.ItemProperties.Basics;
using System;

namespace AntMe.Items.Basics
{
    public class MarkerItem : FactionItem
    {
        public const float MARKER_MINIMUM_RADIUS = 20f;
        public const float MARKER_MAXIMUM_RADIUS = 200f;
        public const float MARKER_VOLUME = 2000f;

        private SmellableProperty _smellable;

        private readonly float _step;

        public MarkerItem(SimulationContext context, Faction faction, Vector2 position, float radius, int information)
            : base(context, faction, position, radius, Angle.Right)
        {
            Information = information;

            _smellable = GetProperty<SmellableProperty>();

            float finalRadius = Math.Max(MARKER_MINIMUM_RADIUS, Math.Min(MARKER_MAXIMUM_RADIUS, radius));
            TotalAge = (int)(MARKER_VOLUME / finalRadius);
            _step = (finalRadius - MARKER_MINIMUM_RADIUS) / TotalAge;
            CurrentAge = 0;
        }

        /// <summary>
        /// Gibt das maximale alter dieser Markierung in Runden zurück.
        /// </summary>
        public int TotalAge { get; private set; }

        /// <summary>
        /// Gibt das aktuelle Alter der Markierung in Runden zurück.
        /// </summary>
        public int CurrentAge { get; private set; }

        /// <summary>
        ///     Gibt den aktuellen Radius dieser Markierung zurück.
        /// </summary>
        public new float Radius
        {
            get { return _smellable.SmellableRadius; }
            private set { _smellable.SmellableRadius = value; }
        }

        /// <summary>
        ///     Gibt die enthaltene Information dieser Markierung zurück.
        /// </summary>
        public int Information { get; private set; }

        protected override void OnUpdate()
        {
            // Marker vergrößern
            CurrentAge++;
            Radius = MARKER_MINIMUM_RADIUS + (CurrentAge * _step);

            // Marker entfernen
            if (CurrentAge >= TotalAge)
                Engine.RemoveItem(this);
        }
    }
}