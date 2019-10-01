using System.Collections.Generic;
using System.Linq;
using AntMe.Basics.ItemProperties;

namespace AntMe.Basics.EngineProperties
{
    /// <summary>
    ///     Engine Extension to handle all Recognition Issues (Sighting, Smelling,...)
    /// </summary>
    public sealed class RecognitionProperty : EngineProperty
    {
        private readonly Dictionary<int, SmellableProperty> smellables;
        private readonly Dictionary<int, SnifferProperty> sniffers;
        private readonly Dictionary<int, SightingProperty> viewers;
        private readonly Dictionary<int, VisibleProperty> visibles;
        private MipMap<SmellableProperty> smellablesMap;
        private MipMap<VisibleProperty> visiblesMap;

        /// <summary>
        ///     Default Constructor for the Type Mapper.
        /// </summary>
        /// <param name="engine">Reference to the Engine</param>
        public RecognitionProperty(Engine engine) : base(engine)
        {
            smellables = new Dictionary<int, SmellableProperty>();
            sniffers = new Dictionary<int, SnifferProperty>();
            viewers = new Dictionary<int, SightingProperty>();
            visibles = new Dictionary<int, VisibleProperty>();
        }

        /// <summary>
        ///     Gets a call after Engine Initialization.
        /// </summary>
        public override void Init()
        {
            var size = Engine.Map.GetSize();
            var mapWidth = size.X;
            var mapHeight = size.Y;
            visiblesMap = new MipMap<VisibleProperty>(mapWidth, mapHeight);
            smellablesMap = new MipMap<SmellableProperty>(mapWidth, mapHeight);
        }

        /// <summary>
        ///     Gets a call after adding a new Item to the Engine.
        /// </summary>
        /// <param name="item">New Item</param>
        protected override void Insert(Item item)
        {
            // Track visible Items.
            if (item.ContainsProperty<VisibleProperty>() &&
                !visibles.ContainsKey(item.Id))
            {
                var prop = item.GetProperty<VisibleProperty>();
                visibles.Add(item.Id, prop);
            }

            // Track sighting Items.
            if (item.ContainsProperty<SightingProperty>() &&
                !viewers.ContainsKey(item.Id))
            {
                viewers.Add(item.Id, item.GetProperty<SightingProperty>());
                item.CellChanged += item_CellChanged;
                item_CellChanged(item, item.Cell);
            }

            // Track smallable Items.
            if (item.ContainsProperty<SmellableProperty>() &&
                !smellables.ContainsKey(item.Id))
            {
                var prop = item.GetProperty<SmellableProperty>();
                smellables.Add(item.Id, prop);
            }

            // Track sniffing Items.
            if (item.ContainsProperty<SnifferProperty>() &&
                !sniffers.ContainsKey(item.Id))
            {
                var prop = item.GetProperty<SnifferProperty>();
                sniffers.Add(item.Id, prop);
            }
        }

        /// <summary>
        ///     Gets a call before removing an item from Engine.
        /// </summary>
        /// <param name="item">Removed Item</param>
        protected override void Remove(Item item)
        {
            // Remove visible Items
            if (item.ContainsProperty<VisibleProperty>() &&
                visibles.ContainsKey(item.Id))
            {
                // Remove also the references to this Items
                var prop = item.GetProperty<VisibleProperty>();
                foreach (var sighting in prop.SightingItems.ToArray())
                {
                    sighting.RemoveVisibleItem(prop);
                    prop.RemoveSightingItem(sighting);
                }

                visibles.Remove(item.Id);
            }

            // Remove sighting Items
            if (item.ContainsProperty<SightingProperty>() &&
                viewers.ContainsKey(item.Id))
            {
                // Remove also References
                var prop = item.GetProperty<SightingProperty>();
                foreach (var visible in prop.VisibleItems.ToArray())
                {
                    prop.RemoveVisibleItem(visible);
                    visible.RemoveSightingItem(prop);
                }

                item.CellChanged -= item_CellChanged;
                viewers.Remove(item.Id);
            }

            // Remove smallable Items
            if (item.ContainsProperty<SmellableProperty>() &&
                smellables.ContainsKey(item.Id))
            {
                // Remove also References
                var prop = item.GetProperty<SmellableProperty>();
                foreach (var sniffer in prop.SnifferItems.ToArray())
                {
                    sniffer.RemoveSmellableItem(prop);
                    prop.RemoveSnifferItem(sniffer);
                }

                smellables.Remove(item.Id);
            }

            // Remove smelling Items
            if (item.ContainsProperty<SnifferProperty>() &&
                sniffers.ContainsKey(item.Id))
            {
                // Also remove References
                var prop = item.GetProperty<SnifferProperty>();
                foreach (var smellable in prop.SmellableItems.ToArray())
                {
                    prop.RemoveSmellableItem(smellable);
                    smellable.RemoveSnifferItem(prop);
                }

                sniffers.Remove(item.Id);
            }
        }

        /// <summary>
        ///     Gets a call after every Engine Update.
        /// </summary>
        public override void Update()
        {
            UpdateVisibles();
            UpdateSniffer();
        }

        #region Smelling

        /// <summary>
        ///     Handles the sniffing Stuff
        /// </summary>
        private void UpdateSniffer()
        {
            // Remake the smellables map
            smellablesMap.Clear();
            foreach (var smellable in smellables.Values)
                smellablesMap.Add(smellable, smellable.Item.Position, smellable.SmellableRadius);

            // Run through all sniffing Items
            foreach (var sniffer in sniffers.Values)
            {
                var smellableItems = new HashSet<SmellableProperty>();

                // Run through potential smallable Items
                foreach (var smellable in smellablesMap.FindAll(sniffer.Item.Position, 0f))
                {
                    // Ignore myself
                    if (smellable.Item == sniffer.Item)
                        continue;

                    // Check for Distance
                    if (Item.GetDistance(sniffer.Item, smellable.Item) <= smellable.SmellableRadius)
                    {
                        // Inform about all smellable Items
                        sniffer.NoteSmellableItem(smellable);
                        smellableItems.Add(smellable);
                    }
                }

                // Add new Items and remove old once
                var addSmellable = smellableItems.Except(sniffer.SmellableItems).ToArray();
                var removeSmellable = sniffer.SmellableItems.Except(smellableItems).ToArray();

                foreach (var item in addSmellable)
                {
                    item.AddSnifferItem(sniffer);
                    sniffer.AddSmellableItem(item);
                }

                foreach (var item in removeSmellable)
                {
                    sniffer.RemoveSmellableItem(item);
                    item.RemoveSnifferItem(sniffer);
                }
            }
        }

        #endregion

        #region Visibility

        /// <summary>
        ///     Handle all visiblity Issues.
        /// </summary>
        private void UpdateVisibles()
        {
            // Remake the visibles map
            visiblesMap.Clear();
            foreach (var visible in visibles.Values)
                visiblesMap.Add(visible, visible.Item.Position, visible.VisibilityRadius);

            // Run through sighting Items
            foreach (var sighting in viewers.Values)
            {
                var visibleItems = new HashSet<VisibleProperty>();

                // Run through all visible elements in the vincinity
                foreach (var visible in visiblesMap.FindAll(sighting.Item.Position, sighting.ViewRange))
                {
                    // Ignore myself
                    if (visible.Item == sighting.Item)
                        continue;

                    var max = sighting.ViewRange + visible.VisibilityRadius;

                    // Check Distance
                    // TODO: Include Sighting Angle
                    if (Item.GetDistance(sighting.Item, visible.Item) <= max)
                    {
                        // Inform about visible Items.
                        sighting.NoteVisibleItem(visible);
                        visibleItems.Add(visible);
                    }
                }

                // Add new Items and remove old once
                var addVisible = visibleItems.Except(sighting.VisibleItems).ToArray();
                var removeVisible = sighting.VisibleItems.Except(visibleItems).ToArray();

                foreach (var item in addVisible)
                {
                    sighting.AddVisibleItem(item);
                    item.AddSightingItem(sighting);
                }


                foreach (var item in removeVisible)
                {
                    sighting.RemoveVisibleItem(item);
                    item.RemoveSightingItem(sighting);
                }
            }
        }

        private void item_CellChanged(Item item, Index2 newValue)
        {
            viewers[item.Id].UpdateEnvironment(Engine.Map, item, newValue);
        }

        #endregion
    }
}