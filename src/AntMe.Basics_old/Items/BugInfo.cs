
namespace AntMe.Basics.Items
{
    public class BugInfo : ItemInfo
    {
        public BugInfo(BugItem item, Item observer)
            : base(item, observer)
        {
        }

        public BugInfo(ClassicBugItem item, Item observer)
            : base(item, observer)
        {
        }
    }
}
