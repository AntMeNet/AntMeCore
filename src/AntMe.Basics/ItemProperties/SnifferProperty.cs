using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AntMe.Basics.ItemProperties
{
    /// <summary>
    /// Property for all sniffing Items.
    /// </summary>
    public sealed class SnifferProperty : ItemProperty
    {
        /// <summary>
        /// Default Constructor.
        /// </summary>
        /// <param name="item">Item</param>
        public SnifferProperty(Item item) : base(item) { }

        private readonly List<SmellableProperty> smellableItems = new List<SmellableProperty>();

        /// <summary>
        /// List of all sniffed Items.
        /// </summary>
        public ReadOnlyCollection<SmellableProperty> SmellableItems
        {
            get { return smellableItems.AsReadOnly(); }
        }

        #region Internal Calls

        /// <summary>
        /// Internal Call to Add a new smellable Item to the List.
        /// </summary>
        /// <param name="item">Property of the new Item</param>
        internal void AddSmellableItem(SmellableProperty item)
        {
            if (!smellableItems.Contains(item))
            {
                smellableItems.Add(item);

                if (OnNewSmellableItem != null)
                    OnNewSmellableItem(item);
            }
        }

        /// <summary>
        /// Internal Call to remove a smellable Item from the List.
        /// </summary>
        /// <param name="item">Property of the removed Item</param>
        internal void RemoveSmellableItem(SmellableProperty item)
        {
            if (smellableItems.Remove(item))
            {
                if (OnLostSmellableItem != null)
                    OnLostSmellableItem(item);
            }
        }

        /// <summary>
        /// Internal call for every sniffed item per Round.
        /// </summary>
        /// <param name="item">Sniffed Item</param>
        internal void NoteSmellableItem(SmellableProperty item)
        {
            if (OnSmellableItem != null)
                OnSmellableItem(item);
        }

        #endregion

        #region Events

        /// <summary>
        /// Signal for a new sniffed Item in the List.
        /// </summary>
        public event ChangeItem<SmellableProperty> OnNewSmellableItem;

        /// <summary>
        /// Signal for a lost sniffed Item in the List.
        /// </summary>
        public event ChangeItem<SmellableProperty> OnLostSmellableItem;

        /// <summary>
        /// Signal for every sniffed Item per Round.
        /// </summary>
        public event ChangeItem<SmellableProperty> OnSmellableItem;

        #endregion
    }
}