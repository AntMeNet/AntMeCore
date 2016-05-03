using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace AntMe.ItemProperties.Basics
{
    /// <summary>
    ///     Property für alle Items, die von riechenden Items wahrnehmbar sind.
    /// </summary>
    public sealed class SmellableProperty : ItemProperty
    {
        private readonly List<SnifferProperty> snifferItems = new List<SnifferProperty>();
        private float smellableRadius;

        public SmellableProperty(Item item, float radius) : base(item)
        {
            SmellableRadius = radius;
        }

        #region Properties

        /// <summary>
        ///     Öffentlich sichtbare readonly list von Sniffer Items
        /// </summary>
        [Browsable(false)]
        public ReadOnlyCollection<SnifferProperty> SnifferItems
        {
            get { return snifferItems.AsReadOnly(); }
        }

        /// <summary>
        ///     Gibt den Radius an, wie weit das Item riechbar ist oder legt
        ///     diesen fest.
        /// </summary>
        [DisplayName("Smellable Radius")]
        [Description("")]
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
        ///     Wird von der Extension aufgerufen, wenn sich ein Element in den
        ///     Riechradius bewegt.
        /// </summary>
        /// <param name="item">Neu riechendes Element</param>
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
        ///     Wird von der Extension aufgerufen, wenn ein Element sich aus dem
        ///     Riechradius entfernt.
        /// </summary>
        /// <param name="item">Nicht mehr riechendes Element</param>
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
        ///     Event, das über ein riechendes Item informiert, das neu in die
        ///     Liste gekommen ist.
        /// </summary>
        public event ChangeItem<SnifferProperty> OnNewSnifferItem;

        /// <summary>
        ///     Event, das über ein riechendes Item informiert, das aus der Liste
        ///     geflogen ist.
        /// </summary>
        public event ChangeItem<SnifferProperty> OnLostSnifferItem;

        /// <summary>
        ///     Event, das angibt, dass sich der Riechradius geändert hat.
        /// </summary>
        public event ValueChanged<float> OnSmellableRadiusChanged;

        #endregion
    }
}