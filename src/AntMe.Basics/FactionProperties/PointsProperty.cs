using System.Collections.Generic;
using System.Linq;

namespace AntMe.Basics.FactionProperties
{
    /// <summary>
    ///     Faction Property that is responsible for counting the Points.
    /// </summary>
    public sealed class PointsProperty : FactionProperty
    {
        /// <summary>
        ///     List of all Point-Relevant Elements.
        ///     (including Factions, FactionProperties, Items and ItemProperties)
        /// </summary>
        private readonly HashSet<IPointsCollector> collectors;

        /// <summary>
        ///     Total Points Counter
        /// </summary>
        private int points;

        /// <summary>
        ///     Category based Points Counter
        /// </summary>
        private readonly Dictionary<string, int> pointsPerCategory;

        /// <summary>
        ///     Default Constructor.
        /// </summary>
        /// <param name="faction">Reference to the related Faction</param>
        public PointsProperty(Faction faction) : base(faction)
        {
            collectors = new HashSet<IPointsCollector>();
            pointsPerCategory = new Dictionary<string, int>();
            PointsPerCategory = new ReadOnlyDictionary<string, int>(pointsPerCategory);
        }

        /// <summary>
        ///     List of Points, ordered by Category.
        /// </summary>
        public ReadOnlyDictionary<string, int> PointsPerCategory { get; }

        /// <summary>
        ///     Total Points.
        /// </summary>
        public int Points
        {
            get => points;
            private set
            {
                points = value;
                OnPointsChanged?.Invoke(value);
            }
        }

        /// <summary>
        ///     Initializer. Should be called by the Faction.
        /// </summary>
        public override void Init()
        {
            // Store Faction if relevant
            var collectorFaction = Faction as IPointsCollector;
            if (collectorFaction != null)
            {
                collectors.Add(collectorFaction);
                collectorFaction.OnEnablePointsChanged += EnablePointsChanged;
                collectorFaction.OnPointsChanged += PointsChanged;
                Recalc(collectorFaction);
            }

            // Store relevant Faction Properties
            foreach (var collectorProperty in Faction.Properties.OfType<IPointsCollector>())
            {
                collectors.Add(collectorProperty);
                collectorProperty.OnEnablePointsChanged += EnablePointsChanged;
                collectorProperty.OnPointsChanged += PointsChanged;
                Recalc(collectorProperty);
            }

            // Attach Events
            Faction.Level.Engine.OnInsertItem += InsertItem;
            Faction.Level.Engine.OnRemoveItem += RemoveItem;
        }

        private void EnablePointsChanged(IPointsCollector item, bool value)
        {
            Recalc(item);
        }

        private void PointsChanged(IPointsCollector item, int value)
        {
            Recalc(item);
        }

        private void InsertItem(Item item)
        {
            var factionItem = item as FactionItem;

            // Ignore non Faction Items
            if (factionItem == null)
                return;

            // Ignore Items from other Factions
            if (factionItem.Faction != Faction)
                return;

            // Add Item of relevant
            var collectorItem = factionItem as IPointsCollector;
            if (collectorItem != null)
            {
                collectors.Add(collectorItem);
                collectorItem.OnEnablePointsChanged += EnablePointsChanged;
                collectorItem.OnPointsChanged += PointsChanged;
                Recalc(collectorItem);
            }

            // Get all relevant Properties
            foreach (var collectorProperty in factionItem.Properties.OfType<IPointsCollector>())
            {
                collectors.Add(collectorProperty);
                collectorProperty.OnEnablePointsChanged += EnablePointsChanged;
                collectorProperty.OnPointsChanged += PointsChanged;
                Recalc(collectorProperty);
            }
        }

        private void RemoveItem(Item item)
        {
            var factionItem = item as FactionItem;

            // Ignore non Faction Items
            if (factionItem == null)
                return;

            // Ignore Items from other Factions
            if (factionItem.Faction != Faction)
                return;

            // Remove Item
            var collectorItem = factionItem as IPointsCollector;
            if (collectorItem != null &&
                !collectorItem.PermanentPoints &&
                collectors.Contains(collectorItem))
            {
                collectors.Remove(collectorItem);
                collectorItem.OnEnablePointsChanged -= EnablePointsChanged;
                collectorItem.OnPointsChanged -= PointsChanged;
                Recalc(collectorItem);
            }

            // Remove all relevant and not permanent Properties
            foreach (var collectorProperty in factionItem.Properties.OfType<IPointsCollector>()
                .Where(p => !p.PermanentPoints && collectors.Contains(p)))
            {
                collectors.Remove(collectorProperty);
                collectorProperty.OnEnablePointsChanged -= EnablePointsChanged;
                collectorProperty.OnPointsChanged -= PointsChanged;
                Recalc(collectorProperty);
            }
        }

        private void Recalc(IPointsCollector item)
        {
            Points = collectors.Where(p => p.EnablePoints).Sum(p => p.Points);

            // Generate Key if not available
            var category = item.PointsCategory;
            if (!pointsPerCategory.ContainsKey(category))
                pointsPerCategory.Add(category, 0);

            var value = collectors.Where(p => p.EnablePoints && p.PointsCategory.Equals(category)).Sum(p => p.Points);
            pointsPerCategory[category] = value;
            OnCategoryPointsChanged?.Invoke(category, value);
        }

        /// <summary>
        ///     Signal for a changed Point Counter.
        /// </summary>
        public event ValueUpdate<int> OnPointsChanged;

        /// <summary>
        ///     Signal for a changed Point Counter in a specific Category.
        /// </summary>
        public event ValueUpdate<string, int> OnCategoryPointsChanged;
    }
}