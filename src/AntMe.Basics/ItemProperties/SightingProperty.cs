using System;
using System.Linq;
using System.Collections.Generic;

namespace AntMe.Basics.ItemProperties
{
    /// <summary>
    /// Property for all sighting Items. This allows to see the Environment, Borders and Visible Items.
    /// </summary>
    public sealed class SightingProperty : ItemProperty
    {
        private readonly VisibleEnvironment environment = new VisibleEnvironment();
        private readonly HashSet<VisibleProperty> visibleItems = new HashSet<VisibleProperty>();
        private Angle viewDirection;
        private float viewangle;
        private float viewrange;

        /// <summary>
        /// Default Constructor.
        /// </summary>
        /// <param name="item">Item</param>
        public SightingProperty(Item item) : base(item) { }

        /// <summary>
        /// Gets or sets the Viewrange.
        /// </summary>
        public float ViewRange
        {
            get { return viewrange; }
            set
            {
                viewrange = Math.Max(0f, value);
                if (OnViewRangeChanged != null)
                    OnViewRangeChanged(Item, viewrange);
            }
        }

        /// <summary>
        /// Gets or sets the View Direction of this Item.
        /// </summary>
        public Angle ViewDirection
        {
            get { return viewDirection; }
            set
            {
                viewDirection = value;
                if (OnViewDirectionChanged != null)
                    OnViewDirectionChanged(Item, value);
            }
        }

        /// <summary>
        /// Gets or sets the View Angle.
        /// 0 = Item can't see anything
        /// 90 = View Range is between -45 and 45 Degrees to the Direction
        /// 360 = No Limitations within the View Radius
        /// </summary>
        public float ViewAngle
        {
            get { return viewangle; }
            set
            {
                viewangle = Math.Max(0f, Math.Min(360, value));
                if (OnViewAngleChanged != null)
                    OnViewAngleChanged(Item, viewangle);
            }
        }

        /// <summary>
        /// Returns a Snapshot of the current Environment.
        /// </summary>
        public VisibleEnvironment Environment
        {
            get { return environment; }
        }

        /// <summary>
        /// List of all visible Items.
        /// </summary>
        public IEnumerable<VisibleProperty> VisibleItems
        {
            get { return visibleItems.AsEnumerable(); }
        }

        #region Internal Calls

        /// <summary>
        /// Internal Call to add another visible Item to the List.
        /// </summary>
        /// <param name="item">VisibleProperty of a new visible Item</param>
        internal void AddVisibleItem(VisibleProperty item)
        {
            if (!visibleItems.Contains(item))
            {
                visibleItems.Add(item);

                if (OnNewVisibleItem != null)
                    OnNewVisibleItem(item);
            }
        }

        /// <summary>
        /// Internal Call to remove a visible Item from the List.
        /// </summary>
        /// <param name="item">Property to remove</param>
        internal void RemoveVisibleItem(VisibleProperty item)
        {
            if (visibleItems.Contains(item))
            {
                visibleItems.Remove(item);

                if (OnLostVisibleItem != null)
                    OnLostVisibleItem(item);
            }
        }

        /// <summary>
        /// Internal call for every visible item per Round.
        /// </summary>
        /// <param name="item">Visible Item</param>
        internal void NoteVisibleItem(VisibleProperty item)
        {
            if (OnVisibleItem != null)
                OnVisibleItem(item);
        }

        /// <summary>
        /// Internal in case of a Cell Switch and a new Environment.
        /// </summary>
        internal void UpdateEnvironment(Map map, Item item, Index2 newCell)
        {
            // Run through neighbour cells
            Index2 limit = map.GetCellCount();

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    var offset = new Index2(x, y);
                    Index2 cell = newCell + offset;

                    if (cell.X < 0 || cell.X >= limit.X ||
                        cell.Y < 0 || cell.Y >= limit.Y)
                    {
                        // No Cell available
                        Environment[offset] = null;
                    }
                    else
                    {
                        // Get Cell Infos
                        MapTile tile = map[cell.X, cell.Y];
                        Environment[offset.X, offset.Y] = tile != null ? tile.GetInfo(item) : null;
                    }
                }
            }

            if (OnEnvironmentChanged != null)
                OnEnvironmentChanged(Item, Environment);
        }

        #endregion

        #region Events

        /// <summary>
        /// Signal for a changed View Range.
        /// </summary>
        public event ValueChanged<float> OnViewRangeChanged;

        /// <summary>
        /// Signal for a changed View Direction.
        /// </summary>
        public event ValueChanged<Angle> OnViewDirectionChanged;

        /// <summary>
        /// Signal for a changed View Angle.
        /// </summary>
        public event ValueChanged<float> OnViewAngleChanged;

        /// <summary>
        /// Signal for a changed Environment.
        /// </summary>
        public event ValueChanged<VisibleEnvironment> OnEnvironmentChanged;

        /// <summary>
        /// Signal for a new Item in the List of visible Items.
        /// </summary>
        public event ChangeItem<VisibleProperty> OnNewVisibleItem;

        /// <summary>
        /// Signal for a lost Item in the List of visible Items.
        /// </summary>
        public event ChangeItem<VisibleProperty> OnLostVisibleItem;

        /// <summary>
        /// Signal for every visible Item per Round.
        /// </summary>
        public event ChangeItem<VisibleProperty> OnVisibleItem;

        #endregion
    }
}