using AntMe.ItemProperties.Basics;
using AntMe.Items.Basics;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AntMe.Simulation.Factions.Ants.Interop
{
    public sealed class RecognitionInterop : InteropProperty
    {
        private const int ANT_MARKER_DELAY = 20; // Erholungszeit in Runden, bis eine neue Markierung gesprüht werden kann


        private readonly AntItem _antItem;
        private int _markerDelay;

        private readonly SightingProperty _sighting;

        private readonly List<ItemInfo> _smellableItems = new List<ItemInfo>();
        private readonly List<ItemInfo> _visibleItems = new List<ItemInfo>();

        public RecognitionInterop(AntItem antItem)
        {
            _antItem = antItem;

            #region Sniffing

            // Relevante Props abgreifen
            var sniffer = _antItem.GetProperty<SnifferProperty>();
            if (sniffer == null)
                throw new ArgumentException("Item does not contain SnifferProperty");

            sniffer.OnNewSmellableItem += item =>
            {
                var Item = (Item) item.Item;
                var info = Item.GetItemInfo(antItem);

                if (!_smellableItems.Contains(info))
                    _smellableItems.Add(info);
            };
            sniffer.OnLostSmellableItem += item =>
            {
                var Item = (Item) item.Item;
                var info = Item.GetItemInfo(antItem);

                if (_smellableItems.Contains(info))
                    _smellableItems.Remove(info);
            };

            #endregion

            #region Sighting

            _sighting = _antItem.GetProperty<SightingProperty>();
            if (_sighting == null)
                throw new ArgumentException("Item does not contain SightingProperty");

            _sighting.OnNewVisibleItem += item =>
            {
                var Item = (Item) item.Item;
                var info = Item.GetItemInfo(antItem);

                if (!_visibleItems.Contains(info))
                    _visibleItems.Add(info);
            };

            _sighting.OnLostVisibleItem += item =>
            {
                var Item = (Item) item.Item;
                var info = Item.GetItemInfo(antItem);

                if (_visibleItems.Contains(info))
                    _visibleItems.Remove(info);
            };

            _sighting.OnEnvironmentChanged += (item, value) =>
            {
                if (OnEnvironmentChanged != null)
                    OnEnvironmentChanged(value);
            };

            #endregion
        }

        protected override void Update(int round)
        {
            // Marker Delay aktualisieren
            _markerDelay = Math.Max(0, _markerDelay - 1);

            // Visible prüfen
            if (Spots != null && _visibleItems.Count > 0)
                Spots();

            // Smellable prüfen
            if (Smells != null && _smellableItems.Count > 0)
                Smells();
        }

        #region Methods
        
        /// <summary>
        ///     Erzeugt eine neue Markierung an der Stelle, an der sich die Ameise befindet.
        /// </summary>
        /// <param name="information">Enthaltene Information</param>
        /// <param name="size">Maximale Größe</param>
        /// <returns>Erfolgsmeldung</returns>
        public bool MakeMark(int information, float size)
        {
            // Wartezeit abwarten
            if (_markerDelay > 0)
                return false;

            // Marker erstellen
            var marker = new MarkerItem(_antItem.Faction.Context, _antItem.Faction,
                _antItem.Position.ToVector2XY(),
                size, information);

            _antItem.Engine.InsertItem(marker);
            _markerDelay = ANT_MARKER_DELAY;
            return true;
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Liefert eine Liste der aktuell sichtbaren Items.
        /// </summary>
        public ReadOnlyCollection<ItemInfo> VisibleItems { get { return _visibleItems.AsReadOnly(); } }

        /// <summary>
        ///     Liefert eine Liste der Elemente, die aktuell riechbar sind.
        /// </summary>
        public ReadOnlyCollection<ItemInfo> SmellableItems { get { return _smellableItems.AsReadOnly(); } }

        /// <summary>
        ///     Zeigt die sichtbare Umgebung um die Ameise herum.
        /// </summary>
        public VisibleEnvironment Environment { get { return _sighting.Environment; } }

        /// <summary>
        /// Gibt den Sichtradius der Ameise zurück.
        /// </summary>
        public float ViewRange { get { return _sighting.ViewRange; }}

        #endregion

        #region Events

        /// <summary>
        /// Informiert über eine geänderte Umgebung.
        /// </summary>
        public event InteropEvent<VisibleEnvironment> OnEnvironmentChanged;
        
        /// <summary>
        /// Informiert über gesichtete Objekte.
        /// </summary>
        public event InteropEvent Spots;

        /// <summary>
        /// Informiert über gerochene Objekte.
        /// </summary>
        public event InteropEvent Smells;

        #endregion
    }
}