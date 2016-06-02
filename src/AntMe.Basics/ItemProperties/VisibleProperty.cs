using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace AntMe.Basics.ItemProperties
{
    /// <summary>
    /// Property for all visible Items.
    /// </summary>
    public sealed class VisibleProperty : ItemProperty
    {
        private readonly HashSet<SightingProperty> sightingItems = new HashSet<SightingProperty>();
        private float visibilityRadius;

        /// <summary>
        /// Default Constructor.
        /// </summary>
        /// <param name="item">Item</param>
        public VisibleProperty(Item item) : base(item)
        {
            VisibilityRadius = item.Radius;
        }

        /// <summary>
        /// Gets or sets the Visibility Radius.
        /// </summary>
        public float VisibilityRadius
        {
            get { return visibilityRadius; }
            set
            {
                visibilityRadius = Math.Max(0f, value);
                if (OnVisibilityRadiusChanged != null)
                    OnVisibilityRadiusChanged(Item, visibilityRadius);
            }
        }

        /// <summary>
        /// List of Items that are able to see this Item.
        /// </summary>
        public IEnumerable<SightingProperty> SightingItems
        {
            get { return sightingItems.AsEnumerable(); }
        }

        #region Internal Calls

        /// <summary>
        ///     Wird von der Extension aufgerufen, wenn sich ein Element in den
        ///     sichtbaren Radius bewegt.
        /// </summary>
        /// <param name="item">Neu sehendes Element</param>
        internal void AddSightingItem(SightingProperty item)
        {
            if (!sightingItems.Contains(item))
            {
                sightingItems.Add(item);

                if (OnNewSightingItem != null)
                    OnNewSightingItem(item);
            }
        }

        /// <summary>
        ///     Wird von der Extension aufgerufen, wenn ein Element sich aus dem
        ///     sichtbaren Radius entfernt.
        /// </summary>
        /// <param name="item">Nicht mehr sehendes Element</param>
        internal void RemoveSightingItem(SightingProperty item)
        {
            if (sightingItems.Contains(item))
            {
                sightingItems.Remove(item);

                if (OnLostSightingItem != null)
                    OnLostSightingItem(item);
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Signal for a changed Visibility Radius.
        /// </summary>
        public event ValueChanged<float> OnVisibilityRadiusChanged;

        /// <summary>
        /// Signal for a new Sighting Item.
        /// </summary>
        public event ChangeItem<SightingProperty> OnNewSightingItem;

        /// <summary>
        /// Signal for a removed Sighting Item.
        /// </summary>
        public event ChangeItem<SightingProperty> OnLostSightingItem;

        #endregion
    }
}