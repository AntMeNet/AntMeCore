using System.Threading;
using AntMe.Basics.Factions.Bugs;

namespace AntMe.Basics.Items
{
    /// <summary>
    ///     A bug item.
    ///     TODO: is this a FactionItem since there is a BugFaction?
    /// </summary>
    public class BugItem : FactionItem
    {
        private const float BugRadius = 4f;

        private const bool BUG_HEAL_BY_SUGAR = false;
        private const bool BUG_HEAL_BY_APPLE = false;
        private const bool BUG_HEAL_BY_ANT = false;
        private const int BUG_ROTATIONSPEED = 25;
        private const int BUG_ZIGZAGANGLE = 10;
        private const int BUG_ZIGZAGRANGE = 30;

        private readonly BugFaction faction;
        private readonly ManualResetEvent reset;

        private readonly Thread thread;

        public BugItem(SimulationContext context, Vector2 position, Angle direction, BugFaction faction)
            : base(context, faction, position, BugRadius, direction)
        {
            this.faction = faction;
        }
    }
}