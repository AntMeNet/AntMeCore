namespace AntMe.Basics.Items
{
    public class AppleInfo : ItemInfo
    {
        public AppleInfo(Item item, Item observer)
            : base(item, observer)
        {
        }

        public override string ToString()
        {
            return string.Format("Apple ({0})", Item.Id);
        }

    }
}