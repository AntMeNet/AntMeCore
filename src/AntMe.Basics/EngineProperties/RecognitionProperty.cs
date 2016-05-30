using AntMe.Basics.ItemProperties;

using System.Collections.Generic;
using System.Linq;

namespace AntMe.Basics.EngineProperties
{
    /// <summary>
    /// Engine Extension to handle all Recognition Issues (Sighting, Smelling,...)
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
        /// Default Constructor for the Type Mapper.
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
        /// Gets a call after Engine Initialization.
        /// </summary>
        public override void Init()
        {
            Vector2 size = Engine.Map.GetSize();
            float mapWidth = size.X;
            float mapHeight = size.Y;
            visiblesMap = new MipMap<VisibleProperty>(mapWidth, mapHeight);
            smellablesMap = new MipMap<SmellableProperty>(mapWidth, mapHeight);
        }

        /// <summary>
        /// Gets a call after adding a new Item to the Engine.
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
        /// Gets a call before removing an item from Engine.
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
                foreach (SightingProperty sighting in prop.SightingItems.ToArray())
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
                foreach (VisibleProperty visible in prop.VisibleItems.ToArray())
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
                foreach (SnifferProperty sniffer in prop.SnifferItems.ToArray())
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
                foreach (SmellableProperty smellable in prop.SmellableItems.ToArray())
                {
                    prop.RemoveSmellableItem(smellable);
                    smellable.RemoveSnifferItem(prop);
                }

                sniffers.Remove(item.Id);
            }
        }

        /// <summary>
        /// Gets a call after every Engine Update.
        /// </summary>
        public override void Update()
        {
            UpdateVisibles();
            UpdateSniffer();
        }

        #region Visibility

        /// <summary>
        /// Handle all visiblity Issues.
        /// </summary>
        private void UpdateVisibles()
        {
            // Remake the visibles map
            visiblesMap.Clear();
            foreach (VisibleProperty visible in visibles.Values)
            {
                visiblesMap.Add(visible, visible.Item.Position, visible.VisibilityRadius);
            }

            // Run through sighting Items
            foreach (SightingProperty sighting in viewers.Values)
            {
                var visibleItems = new List<VisibleProperty>();

                // Run through all visible elements in the vincinity
                foreach (VisibleProperty visible in visiblesMap.FindAll(sighting.Item.Position, sighting.ViewRange))
                {
                    // Ignore myself
                    if (visible.Item == sighting.Item)
                        continue;

                    float max = sighting.ViewRange + visible.VisibilityRadius;

                    // Check Distance
                    // TODO: Include Sighting Angle
                    if (Item.GetDistance(sighting.Item, visible.Item) <= max)
                    {
                        // Spots new Item
                        if (!sighting.VisibleItems.Contains(visible))
                        {
                            sighting.AddVisibleItem(visible);
                            visible.AddSightingItem(sighting);
                        }

                        // Inform about visible Items.
                        sighting.NoteVisibleItem(visible);
                        visibleItems.Add(visible);
                    }
                }

                // Run through visible Items to remove if not visible anymore
                foreach (VisibleProperty visible in sighting.VisibleItems.ToArray())
                {
                    if (!visibleItems.Contains(visible))
                    {
                        sighting.RemoveVisibleItem(visible);
                        visible.RemoveSightingItem(sighting);
                    }
                }
            }
        }

        private void item_CellChanged(Item item, Index2 newValue)
        {
            // Update Environment Data
            VisibleEnvironment env = viewers[item.Id].Environment;

            // Item is out of Map
            Index2 limit = Engine.Map.GetCellCount();
            if (newValue.X < 0 || newValue.X >= limit.X ||
                newValue.Y < 0 || newValue.Y >= limit.Y)
            {
                env.Center = null;
                env.North = null;
                env.South = null;
                env.West = null;
                env.East = null;
                env.NorthWest = null;
                env.NorthEast = null;
                env.SouthWest = null;
                env.SouthEast = null;
                return;
            }

            // Run through neighbor cells
            for (int x = -1; x <= 1; x++)
                for (int y = -1; y <= 1; y++)
                {
                    var offset = new Index2(x, y);
                    Index2 cell = newValue + offset;

                    if (cell.X < 0 || cell.X >= limit.X ||
                        cell.Y < 0 || cell.Y >= limit.Y)
                    {
                        // No Cell available
                        env[offset] = null;
                    }
                    else
                    {
                        // Get Cell Infos
                        float speed = Engine.Map.Tiles[cell.X, cell.Y].GetSpeedMultiplicator();
                        TileHeight height = Engine.Map.Tiles[cell.X, cell.Y].Height;
                        env[offset] = new VisibleCell
                        {
                            Speed = speed,
                            Height = height
                        };
                    }
                }

            // Inform user
            viewers[item.Id].RefreshEnvironment();
        }

        #endregion

        #region Smelling

        /// <summary>
        /// Handles the sniffing Stuff
        /// </summary>
        private void UpdateSniffer()
        {
            // Remake the smellables map
            smellablesMap.Clear();
            foreach (SmellableProperty smellable in smellables.Values)
            {
                smellablesMap.Add(smellable, smellable.Item.Position, smellable.SmellableRadius);
            }

            // Run through all sniffing Items
            foreach (SnifferProperty sniffer in sniffers.Values)
            {
                var smellableItems = new List<SmellableProperty>();

                // Run through potential smallable Items
                foreach (SmellableProperty smellable in smellablesMap.FindAll(sniffer.Item.Position, 0f))
                {
                    // Ignore myself
                    if (smellable.Item == sniffer.Item)
                        continue;

                    // Check for Distance
                    if (Item.GetDistance(sniffer.Item, smellable.Item) <= smellable.SmellableRadius)
                    {
                        // Add if not sniffable yet
                        if (!sniffer.SmellableItems.Contains(smellable))
                        {
                            smellable.AddSnifferItem(sniffer);
                            sniffer.AddSmellableItem(smellable);
                        }

                        // Inform about all smellable Items
                        sniffer.NoteSmellableItem(smellable);
                        smellableItems.Add(smellable);
                    }
                }

                // Run through Smellable Items
                foreach (SmellableProperty smellable in sniffer.SmellableItems.ToArray())
                {
                    if (!smellableItems.Contains(smellable))
                    {
                        sniffer.RemoveSmellableItem(smellable);
                        smellable.RemoveSnifferItem(sniffer);
                    }
                }
            }
        }

        #endregion
    }
}