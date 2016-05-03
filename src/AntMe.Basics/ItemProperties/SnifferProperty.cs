using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace AntMe.ItemProperties.Basics
{
    /// <summary>
    ///     Property für alle riechenden Einheiten.
    /// </summary>
    public sealed class SnifferProperty : ItemProperty
    {
        public SnifferProperty(Item item) : base(item) { }

        private readonly List<SmellableProperty> smellableItems = new List<SmellableProperty>();

        /// <summary>
        ///     Öffentlich sichtbare readonly list von Smellable Items
        /// </summary>
        [Browsable(false)]
        public ReadOnlyCollection<SmellableProperty> SmellableItems
        {
            get { return smellableItems.AsReadOnly(); }
        }

        #region Internal Calls

        /// <summary>
        ///     Wird von der Extension aufgerufen, wenn sich ein Element in den
        ///     Riechradius bewegt.
        /// </summary>
        /// <param name="item">Neu riechbares Element</param>
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
        ///     Wird von der Extension aufgerufen, wenn ein Element sich aus dem
        ///     Riechradius entfernt.
        /// </summary>
        /// <param name="item">Nicht mehr riechbares Element</param>
        internal void RemoveSmellableItem(SmellableProperty item)
        {
            if (smellableItems.Remove(item))
            {
                if (OnLostSmellableItem != null)
                    OnLostSmellableItem(item);
            }
        }

        /// <summary>
        ///     Wird von der Extension so lange in jeder Runde aufgerufen, solange
        ///     sich das Element im Riechradius befindet.
        /// </summary>
        /// <param name="item">Riechbares Element</param>
        internal void NoteSmellableItem(SmellableProperty item)
        {
            if (OnSmellableItem != null)
                OnSmellableItem(item);
        }

        #endregion

        #region Events

        /// <summary>
        ///     Event, das über ein riechbares Item informiert, das neu in die
        ///     Liste gekommen ist.
        /// </summary>
        public event ChangeItem<SmellableProperty> OnNewSmellableItem;

        /// <summary>
        ///     Event, das über ein riechbares Item informiert, das aus der Liste
        ///     geflogen ist.
        /// </summary>
        public event ChangeItem<SmellableProperty> OnLostSmellableItem;

        /// <summary>
        ///     Event, das über ein riechbares Item informiert, jeden Tick in dem
        ///     es riechbar ist.
        /// </summary>
        public event ChangeItem<SmellableProperty> OnSmellableItem;

        #endregion
    }
}