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
        private readonly SightingProperty sighting;

        private readonly HashSet<ItemInfo> smellableItems = new HashSet<ItemInfo>();
        private readonly HashSet<ItemInfo> visibleItems = new HashSet<ItemInfo>();

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
                var info = property.Item.GetItemInfo(Item);
                if (!smellableItems.Contains(info))
                    smellableItems.Add(info);
            };

            // Remove sniffed Items from List.
            sniffer.OnLostSmellableItem += property =>
            {
                var info = property.Item.GetItemInfo(Item);
                if (smellableItems.Contains(info))
                    smellableItems.Remove(info);
            };

            // Get Sighting Property
            sighting = Item.GetProperty<SightingProperty>();
            if (sighting == null)
                throw new ArgumentException("Item does not contain SightingProperty");

            // Add visible items to List.
            sighting.OnNewVisibleItem += property =>
            {
                var info = property.Item.GetItemInfo(Item);
                if (!visibleItems.Contains(info))
                    visibleItems.Add(info);
            };

            // Remove visible Items from List.
            sighting.OnLostVisibleItem += property =>
            {
                var info = property.Item.GetItemInfo(Item);
                if (visibleItems.Contains(info))
                    visibleItems.Remove(info);
            };

            // Set new Environment on Cell Switch
            sighting.OnEnvironmentChanged += (i, value) => { OnEnvironmentChanged?.Invoke(value); };
        }

        protected override void Update(int round)
        {
            // Visible prüfen
            if (visibleItems.Count > 0)
                Spots?.Invoke();

            // Smellable prüfen
            if (smellableItems.Count > 0)
                Smells?.Invoke();
        }

        #region Properties

        /// <summary>
        ///     List of visible Items in Range.
        /// </summary>
        public IEnumerable<ItemInfo> VisibleItems => visibleItems.AsEnumerable();

        /// <summary>
        ///     List of smellable items in Range.
        /// </summary>
        public IEnumerable<ItemInfo> SmellableItems => smellableItems.AsEnumerable();

        /// <summary>
        ///     Visible Environment.
        /// </summary>
        public VisibleEnvironment Environment => sighting.Environment;

        /// <summary>
        ///     Gets the current View Range.
        /// </summary>
        public float ViewRange => sighting.ViewRange;

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