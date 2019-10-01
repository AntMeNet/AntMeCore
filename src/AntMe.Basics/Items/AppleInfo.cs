namespace AntMe.Basics.Items
{
    public sealed class AppleInfo : ItemInfo
    {
        public AppleInfo(Item item)
            : base(item)
        {
        }

        public override string ToString()
        {
            return string.Format("Apple ({0})", Item.Id);
        }
    }
}