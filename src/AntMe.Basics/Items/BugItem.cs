using AntMe.Factions.Bugs;
using AntMe.ItemProperties.Basics;
using System.Threading;

namespace AntMe.Items.Basics
{
    /// <summary>
    /// A bug item.
    /// TODO: is this a FactionItem since there is a BugFaction?
    /// </summary>
    public class BugItem : FactionItem
    {
        private const float BUG_RADIUS = 4f;
        private const int BUG_HITPOINTS = 1000;
        private const float BUG_RANGE = 5f;
        private const int BUG_ATTACK_STRENGHT = 10;
        private const bool BUG_HEAL_BY_SUGAR = false;
        private const bool BUG_HEAL_BY_APPLE = false;
        private const bool BUG_HEAL_BY_ANT = false;
        private const int BUG_SUGAR_CAPACITY = 4;
        private const int BUG_APPLE_CAPACITY = 2;
        private const float BUG_MAX_SPEED = 2f;
        private const float BUG_VIEWRANGE = 20f;
        private const int BUG_ROTATIONSPEED = 25;
        private const int BUG_ZICKZACKANGLE = 10;
        private const int BUG_ZICKZACKRANGE = 30;

        private readonly WalkingProperty moving;
        private readonly CollidableProperty collidable;

        private readonly SightingProperty sighting;
        private readonly SnifferProperty sniffer;
        private readonly VisibleProperty visible;

        private readonly AttackableProperty attackable;
        private readonly AttackerProperty attacker;
        private readonly CollectorProperty collector;

        private readonly SugarCollectableProperty sugar;
        private readonly AppleCollectableProperty apple;

        private readonly BugFaction faction;

        private readonly Thread thread;
        private readonly ManualResetEvent reset;

        public BugItem(ITypeResolver resolver, Vector2 position, Angle direction, BugFaction faction)
            : base(resolver, faction, position, direction)
        {
            this.faction = faction;
        }
    }
}
