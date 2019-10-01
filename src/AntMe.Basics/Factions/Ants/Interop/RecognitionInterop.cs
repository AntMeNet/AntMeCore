using System;
using System.Collections.Generic;
using System.Linq;
using AntMe.Basics.ItemProperties;

namespace AntMe.Basics.Factions.Ants.Interop
{
    /// <summary>
    ///     Recognition Interop for Ants.
    /// </summary>
    public sealed class RecognitionInterop : UnitInteropProperty
    {
        private readonly SightingProperty _sighting;

        private readonly HashSet<ItemInfo> _smellableItems = new HashSet<ItemInfo>();
        private readonly HashSet<ItemInfo> _visibleItems = new HashSet<ItemInfo>();

        /// <summary>
        ///     Default Constructor for the Type Mapper.
        /// </summary>
        /// <param name="faction">Faction</param>
        /// <param name="item">Item</param>
        /// <param name="interop">UnitInterop</param>
        public RecognitionInterop(Faction faction, FactionItem item, UnitInterop interop) : base(faction, item, interop)
        {
            // Get Sniffer Reference
            var sniffer = Item.GetProperty<SnifferProperty>();
            if (sniffer == null)
                throw new ArgumentException("Item does not contain SnifferProperty");

            // Insert sniffed Items into the List.
            sniffer.OnNewSmellableItem += property =>
            {
                var info = property.Item.GetItemInfo();
                if (!_smellableItems.Contains(info))
                    _smellableItems.Add(info);
            };

            // Remove sniffed Items from List.
            sniffer.OnLostSmellableItem += property =>
            {
                var info = property.Item.GetItemInfo();
                if (_smellableItems.Contains(info))
                    _smellableItems.Remove(info);
            };

            // Get Sighting Property
            _sighting = Item.GetProperty<SightingProperty>();
            if (_sighting == null)
                throw new ArgumentException("Item does not contain SightingProperty");

            // Add visible items to List.
            _sighting.OnNewVisibleItem += property =>
            {
                var info = property.Item.GetItemInfo();
                if (!_visibleItems.Contains(info))
                    _visibleItems.Add(info);
            };

            // Remove visible Items from List.
            _sighting.OnLostVisibleItem += property =>
            {
                var info = property.Item.GetItemInfo();
                if (_visibleItems.Contains(info))
                    _visibleItems.Remove(info);
            };

            // Set new Environment on Cell Switch
            _sighting.OnEnvironmentChanged += (i, value) => { OnEnvironmentChanged?.Invoke(value); };
        }

        protected override void Update(int round)
        {
            // Visible prüfen
            if (_visibleItems.Count > 0)
                Spots?.Invoke();

            // Smellable prüfen
            if (_smellableItems.Count > 0)
                Smells?.Invoke();
        }

        #region Properties

        /// <summary>
        ///     List of visible Items in Range.
        /// </summary>
        public IEnumerable<ItemInfo> VisibleItems => _visibleItems.AsEnumerable();

        /// <summary>
        ///     List of smellable items in Range.
        /// </summary>
        public IEnumerable<ItemInfo> SmellableItems => _smellableItems.AsEnumerable();

        /// <summary>
        ///     Visible Environment.
        /// </summary>
        public VisibleEnvironment Environment => _sighting.Environment;

        /// <summary>
        ///     Gets the current View Range.
        /// </summary>
        public float ViewRange => _sighting.ViewRange;

        #endregion

        #region Events

        /// <summary>
        ///     Informiert über eine geänderte Umgebung.
        /// </summary>
        public event InteropEvent<VisibleEnvironment> OnEnvironmentChanged;

        /// <summary>
        ///     Informiert über gesichtete Objekte.
        /// </summary>
        public event InteropEvent Spots;

        /// <summary>
        ///     Informiert über gerochene Objekte.
        /// </summary>
        public event InteropEvent Smells;

        #endregion
    }
}