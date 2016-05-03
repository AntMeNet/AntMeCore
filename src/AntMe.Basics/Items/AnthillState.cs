namespace AntMe.Items.Basics
{
    public sealed class AnthillState : FactionItemState
    {
        public AnthillState() : base() { }

        public AnthillState(AnthillItem item) : base(item)
        {
        }

        public override string ToString()
        {
            return "AntHill";
        }
    }
}