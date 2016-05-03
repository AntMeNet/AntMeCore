using AntMe.ItemProperties.Basics;

namespace AntMe.ItemProperties.Basics
{
    public sealed class SugarCollectableProperty : CollectableGoodProperty
    {
        public SugarCollectableProperty(Item item, int capacity, int amount)
            : base(item, capacity, amount)
        {
        }
    }
}