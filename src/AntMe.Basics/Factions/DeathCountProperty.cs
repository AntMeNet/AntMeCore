using AntMe.Basics.ItemProperties;
using System.Collections.Generic;
using System.Linq;

namespace AntMe.Basics.Factions
{
    /// <summary>
    /// Faction Property to count own dead Items.
    /// </summary>
    public abstract class DeathCountProperty<T> : FactionProperty, IPointsCollector where T : Item
    {
        private bool enabled;
        private int points;
        private HashSet<T> observedItems;
        private HashSet<AttackableProperty> observedAttackables;
        private int removedItemsCount;
        private int killedItemsCount;
        private int damageCount;

        /// <summary>
        /// Default Constructor for the Type Mapper.
        /// </summary>
        /// <param name="faction">Reference to the related Faction</param>
        public DeathCountProperty(Faction faction) : base(faction)
        {
            observedAttackables = new HashSet<AttackableProperty>();
            observedItems = new HashSet<T>();
        }

        /// <summary>
        /// Gets called by the Engine during Faction Initialization.
        /// </summary>
        public override void Init()
        {
            base.Init();

            Faction.Level.InsertItem += InsertItem;
            Faction.Level.RemoveItem += RemoveItem;
        }

        private void RemoveItem(Item item)
        {
            // Ignore unobserved Items.
            if (!observedItems.Contains(item))
                return;

            // Count Remove
            RemovedItemsCount++;
            observedItems.Remove(item as T);

            AttackableProperty property = item.GetProperty<AttackableProperty>();
            if (property == null || !observedAttackables.Contains(property))
                return;

            // Unattach Property Events
            property.OnAttackerHit -= Property_OnAttackerHit;
            property.OnKill -= Property_OnKill;
            observedAttackables.Remove(property);

            Points = RecalculatePoints();
        }

        private void InsertItem(Item item)
        {
            FactionItem factionItem = item as FactionItem;

            // Ignore non-faction Items.
            if (factionItem == null)
                return;

            // Ignore foreign Items.
            if (factionItem.Faction != Faction)
                return;

            // Ignore Items from other types than T
            T specialItem = factionItem as T;
            if (!factionItem.GetType().Equals(typeof(T)))
                return;

            observedItems.Add(specialItem);

            // Ignore Items without Attackable Property
            AttackableProperty property = factionItem.GetProperty<AttackableProperty>();
            if (property == null)
                return;

            property.OnAttackerHit += Property_OnAttackerHit;
            property.OnKill += Property_OnKill;
            observedAttackables.Add(property);
        }

        private void Property_OnKill(Item item)
        {
            KilledItemsCount++;
            Points = RecalculatePoints();
        }

        private void Property_OnAttackerHit(Item item, int newValue)
        {
            DamageCount += newValue;
            Points = RecalculatePoints();
        }

        /// <summary>
        /// Calculates the Points for the current Counter States.
        /// </summary>
        /// <returns>Points</returns>
        protected abstract int RecalculatePoints();

        /// <summary>
        /// Defines if the Points should count.
        /// </summary>
        public bool EnablePoints
        {
            get { return enabled; }
            set
            {
                enabled = value;
                OnEnablePointsChanged?.Invoke(this, value);
            }
        }

        /// <summary>
        /// Defines of the Counter will be removed after Item Death.
        /// </summary>
        public bool PermanentPoints { get { return true; } }

        /// <summary>
        /// Returns the current Amount of Points.
        /// </summary>
        public int Points
        {
            get { return points; }
            private set
            {
                points = value;
                OnPointsChanged?.Invoke(this, value);
            }
        }

        /// <summary>
        /// Counts the Number of removed Items.
        /// </summary>
        public int RemovedItemsCount
        {
            get { return removedItemsCount; }
            set
            {
                removedItemsCount = value;
                OnRemovedItemCountChanged?.Invoke(value);
            }
        }

        /// <summary>
        /// Counts the Number of Items killed by Attack.
        /// </summary>
        public int KilledItemsCount
        {
            get { return killedItemsCount; }
            set
            {
                killedItemsCount = value;
                OnKilledItemCountChanged?.Invoke(value);
            }
        }

        /// <summary>
        /// Counts the Number of Hitpoints lost by other Attackers.
        /// </summary>
        public int DamageCount
        {
            get { return damageCount; }
            set
            {
                damageCount = value;
                OnDamageCountChanged?.Invoke(value);
            }
        }

        /// <summary>
        /// Returns the Points Category.
        /// </summary>
        public abstract string PointsCategory { get; }

        /// <summary>
        /// Signal for changed Enable Flag.
        /// </summary>
        public event ValueUpdate<IPointsCollector, bool> OnEnablePointsChanged;

        /// <summary>
        /// Signal for a changed Point Counter.
        /// </summary>
        public event ValueUpdate<IPointsCollector, int> OnPointsChanged;

        /// <summary>
        /// Signal for a changed Removed Items Counter.
        /// </summary>
        public event ValueUpdate<int> OnRemovedItemCountChanged;

        /// <summary>
        /// Signal for a changed Killed Items Counter.
        /// </summary>
        public event ValueUpdate<int> OnKilledItemCountChanged;

        /// <summary>
        /// Signal for a changed Damage Counter.
        /// </summary>
        public event ValueUpdate<int> OnDamageCountChanged;
    }
}
