namespace AntMe.Items.Basics
{
    public sealed class AppleState : ItemState
    {
        public AppleState() : base() { }

        public AppleState(AppleItem item) : base(item)
        {
        }

        public override string ToString()
        {
            return "Apple (" + Id + ")";
        }
    }
}