namespace AntMe.Items.Basics
{
    public class AnthillInfo : FactionItemInfo
    {
        private readonly AnthillItem anthillItem;

        public AnthillInfo(FactionItem item, Item observer)
            : base(item, observer)
        {
            anthillItem = item as AnthillItem;
        }
    }
}