using AntMe.ItemProperties.Basics;

namespace AntMe.Items.Basics
{
    public class AppleItem : Item
    {
        /// <summary>
        /// Maximale Ladung des Apfels
        /// </summary>
        public const int AppleMaxAmount = 250;

        /// <summary>
        /// Kollisionsradius des Apfels
        /// </summary>
        public const float AppleInnerRadius = 10;

        /// <summary>
        /// Trageradius
        /// </summary>
        public const float ApplePortableRadius = 15;

        public AppleItem(ITypeResolver resolver, Vector2 position, int amount)
            : base(resolver, position, Angle.Right)
        {
            #region Apfel abtragen

            var apple = GetProperty<AppleCollectableProperty>();
            apple.OnAmountChanged += (good, newValue) =>
            {
                // Apfel entfernen, falls Value is 0
                if (newValue <= 0)
                    Engine.RemoveItem(this);

                // Capacity sollte sich auch ändern
                good.Capacity = newValue;
            };

            #endregion

            #region Ameisenhügel -> Apfel Interaktion

            var collidable = GetProperty<CollidableProperty>();
            collidable.OnCollision += (item, newValue) =>
            {
                if (newValue.ContainsProperty<CollectableProperty>())
                {
                    var target = newValue.GetProperty<CollectableProperty>();

                    var targetGood = target.GetCollectableGood<AppleCollectableProperty>();

                    // Prüfen, ob Apfel grundsätzlich aufgenommen werden kann
                    if (targetGood == null)
                        return;

                    if (targetGood.Capacity - targetGood.Amount < apple.Amount)
                    {
                        // Apfel passt nicht vollständig in den Bau
                        var diff = targetGood.Capacity - targetGood.Amount;
                        apple.Amount -= diff;
                        targetGood.Amount += diff;
                    }
                    else
                    {
                        // Apfel vollständig übertragen
                        targetGood.Amount += apple.Amount;
                        apple.Amount = 0;
                    }
                }
            };

            #endregion
        }
    }
}