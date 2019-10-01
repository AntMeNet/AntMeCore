namespace AntMe.Basics.Items
{
    public sealed class AnthillInfo : FactionItemInfo
    {
        private readonly AnthillItem _anthillItem;

        public AnthillInfo(FactionItem item) : base(item)
        {
            _anthillItem = item as AnthillItem;
        }
    }
}