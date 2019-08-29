
using AntMe.Basics.ItemProperties;

namespace AntMe.Basics.Items
{
    public class SugarInfo : ItemInfo
    {
        private readonly SugarItem _item;

        private readonly SugarCollectableProperty _sugar;

        public SugarInfo(SugarItem item, Item observer)
            : base(item, observer)
        {
            _item = item;

            _sugar = _item.GetProperty<SugarCollectableProperty>();
        }

        /// <summary>
        /// Gibt die verfügbare Zuckermenge zurück.
        /// </summary>
        public int Amount
        {
            get { return _sugar.Amount; }
        }

        /// <summary>
        /// Gibt die Größe des Zuckerberges an.
        /// </summary>
        public new float Radius
        {
            get { return _item.Radius; }
        }
    }
}