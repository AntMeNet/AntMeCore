using AntMe.ItemProperties.Basics;

namespace AntMe.ItemProperties.Basics
{
    public sealed class SugarCollectableState : CollectableGoodState
    {
        public SugarCollectableState() : base() { }

        public SugarCollectableState(Item item, SugarCollectableProperty property) : base(item, property)
        {

        }
    }
}
