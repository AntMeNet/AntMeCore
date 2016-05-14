using AntMe.Basics.Factions.Bugs;
using System.Threading;

namespace AntMe.Basics.Items
{
    /// <summary>
    /// A bug item.
    /// TODO: is this a FactionItem since there is a BugFaction?
    /// </summary>
    public class BugItem : FactionItem
    {
        private const float BugRadius = 4f;

        private const bool BUG_HEAL_BY_SUGAR = false;
        private const bool BUG_HEAL_BY_APPLE = false;
        private const bool BUG_HEAL_BY_ANT = false;
        private const int BUG_ROTATIONSPEED = 25;
        private const int BUG_ZICKZACKANGLE = 10;
        private const int BUG_ZICKZACKRANGE = 30;

        private readonly BugFaction faction;

        private readonly Thread thread;
        private readonly ManualResetEvent reset;

        public BugItem(SimulationContext context, Vector2 position, Angle direction, BugFaction faction)
            : base(context, faction, position, BugRadius, direction)
        {
            this.faction = faction;
        }
    }
}
