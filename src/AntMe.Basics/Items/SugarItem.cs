using AntMe.ItemProperties.Basics;

namespace AntMe.Items.Basics
{
    /// <summary>
    /// Der Zucker-Hügel.
    /// </summary>
    public class SugarItem : Item
    {
        /// <summary>
        /// Radius des Zucker-Berges, egal wie viel Ladung er hat.
        /// </summary>
        public const float SugarRadius = 10f;

        /// <summary>
        /// Maximale Ladung eines Zuckerberges.
        /// </summary>
        public const int SugarMaxCapacity = 1000;

        private SugarCollectableProperty _sugar;

        public SugarItem(ITypeResolver resolver, Vector2 position, int amount)
            : base(resolver, position, Angle.Right)
        {
            Radius = SugarRadius;

            // Todesbedingung
            _sugar = GetProperty<SugarCollectableProperty>();
            _sugar.OnAmountChanged += (good, newValue) =>
            {
                if (newValue <= 0)
                {
                    // Entfernen von der Landkarte
                    Engine.RemoveItem(this);
                }
            };
        }
    }
}