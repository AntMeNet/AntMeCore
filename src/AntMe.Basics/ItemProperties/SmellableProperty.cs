using System;
using System.Collections.Generic;
using System.Linq;

namespace AntMe.Basics.ItemProperties
{
    /// <summary>
    /// Property for all smellable Items.
    /// </summary>
    public sealed class SmellableProperty : ItemProperty
    {
        private readonly HashSet<SnifferProperty> snifferItems = new HashSet<SnifferProperty>();
        private float smellableRadius;

        /// <summary>
        /// Default Constructor.
        /// </summary>
        /// <param name="item">Item</param>
        public SmellableProperty(Item item) : base(item)
        {
            SmellableRadius = item.Radius;
        }

        #region Properties

        /// <summary>
        /// List of all Items that sniffes the Item.
        /// </summary>
        public IQueryable<SnifferProperty> SnifferItems
        {
            get { return snifferItems.AsQueryable(); }
        }

        /// <summary>
        /// Gets or sets the current Smellable Radius.
        /// </summary>
        public float SmellableRadius
        {
            get { return smellableRadius; }
            set
            {
                smellableRadius = Math.Max(0f, value);
                if (OnSmellableRadiusChanged != null)
                    OnSmellableRadiusChanged(Item, smellableRadius);
            }
        }

        #endregion

        #region Internal Calls

        /// <summary>
        /// Internal Call to add another Sniffer to the List.
        /// </summary>
        /// <param name="item">New SnifferProperty of the Sniffing Item</param>
        internal void AddSnifferItem(SnifferProperty item)
        {
            if (!snifferItems.Contains(item))
            {
                snifferItems.Add(item);

                if (OnNewSnifferItem != null)
                    OnNewSnifferItem(item);
            }
        }

        /// <summary>
        /// Internal Call to remove a sniffing Item from the List.
        /// </summary>
        /// <param name="item">Removed SnifferProperty of the Sniffing Item</param>
        internal void RemoveSnifferItem(SnifferProperty item)
        {
            if (snifferItems.Remove(item))
            {
                if (OnLostSnifferItem != null)
                    OnLostSnifferItem(item);
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Signal for a new Sniffing Item.
        /// </summary>
        public event ChangeItem<SnifferProperty> OnNewSnifferItem;

        /// <summary>
        /// Signal for a lost Sniffing Item.
        /// </summary>
        public event ChangeItem<SnifferProperty> OnLostSnifferItem;

        /// <summary>
        /// Signal for a changed smellable Radius.
        /// </summary>
        public event ValueChanged<float> OnSmellableRadiusChanged;

        #endregion
    }
}