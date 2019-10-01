using AntMe.Basics.ItemProperties;

namespace AntMe.Basics.Items
{
    public sealed class SugarInfo : ItemInfo
    {
        private readonly SugarItem _item;

        private readonly SugarCollectableProperty _sugar;

        public SugarInfo(SugarItem item) : base(item)
        {
            _item = item;
            _sugar = _item.GetProperty<SugarCollectableProperty>();
        }

        /// <summary>
        ///     Gibt die verfügbare Zuckermenge zurück.
        /// </summary>
        public int Amount => _sugar.Amount;

        /// <summary>
        ///     Gibt die Größe des Zuckerberges an.
        /// </summary>
        public new float Radius => _item.Radius;
    }
}