using AntMe.Basics.ItemProperties;

using System.Collections.Generic;
using System.Linq;

namespace AntMe.Basics.EngineExtensions
{
    /// <summary>
    ///     Extension zur Abhandlung aller passiver Interaktionsmöglichkeiten.
    ///     Dies umfasst alle Arten der Wahrnehmung wie die Sichtbarkeit und
    ///     das Riechorgan.
    /// </summary>
    public sealed class RecognitionExtension : EngineProperty
    {
        private readonly Dictionary<int, SmellableProperty> smellables = new Dictionary<int, SmellableProperty>();
        private readonly Dictionary<int, SnifferProperty> sniffers = new Dictionary<int, SnifferProperty>();
        private readonly Dictionary<int, SightingProperty> viewers = new Dictionary<int, SightingProperty>();
        private readonly Dictionary<int, VisibleProperty> visibles = new Dictionary<int, VisibleProperty>();
        private MipMap<SmellableProperty> smellablesMap;
        private MipMap<VisibleProperty> visiblesMap;

        public RecognitionExtension(Engine engine) : base(engine) { }

        public override void Init()
        {
            Vector2 size = Engine.Map.GetSize();
            float mapWidth = size.X;
            float mapHeight = size.Y;
            visiblesMap = new MipMap<VisibleProperty>(mapWidth, mapHeight);
            smellablesMap = new MipMap<SmellableProperty>(mapWidth, mapHeight);
        }

        public override void Insert(Item item)
        {
            // Füge sichtbares Objekt ein
            if (item.ContainsProperty<VisibleProperty>() &&
                !visibles.ContainsKey(item.Id))
            {
                var prop = item.GetProperty<VisibleProperty>();
                visibles.Add(item.Id, prop);
            }

            // Füge sehende Objekt ein
            if (item.ContainsProperty<SightingProperty>() &&
                !viewers.ContainsKey(item.Id))
            {
                viewers.Add(item.Id, item.GetProperty<SightingProperty>());
                item.CellChanged += item_CellChanged;
                item_CellChanged(item, item.Cell);
            }

            // Füge riechbare Objekt ein
            if (item.ContainsProperty<SmellableProperty>() &&
                !smellables.ContainsKey(item.Id))
            {
                var prop = item.GetProperty<SmellableProperty>();
                smellables.Add(item.Id, prop);
            }

            // Füge riechenden Objekt ein
            if (item.ContainsProperty<SnifferProperty>() &&
                !sniffers.ContainsKey(item.Id))
            {
                var prop = item.GetProperty<SnifferProperty>();
                sniffers.Add(item.Id, prop);
            }
        }

        public override void Remove(Item item)
        {
            // Entferne sichtbares Objekt
            if (item.ContainsProperty<VisibleProperty>() &&
                visibles.ContainsKey(item.Id))
            {
                // Entferne Referenzen aus allen sichtbaren Elementen
                var prop = item.GetProperty<VisibleProperty>();
                foreach (SightingProperty sighting in prop.SightingItems.ToArray())
                {
                    sighting.RemoveVisibleItem(prop);
                    prop.RemoveSightingItem(sighting);
                }

                visibles.Remove(item.Id);
            }

            // Entferne sehendes Objekt
            if (item.ContainsProperty<SightingProperty>() &&
                viewers.ContainsKey(item.Id))
            {
                // Enferne alle sichtbaren Elemente
                var prop = item.GetProperty<SightingProperty>();
                foreach (VisibleProperty visible in prop.VisibleItems.ToArray())
                {
                    prop.RemoveVisibleItem(visible);
                    visible.RemoveSightingItem(prop);
                }

                item.CellChanged -= item_CellChanged;
                viewers.Remove(item.Id);
            }

            // Entferne riechbares Objekt
            if (item.ContainsProperty<SmellableProperty>() &&
                smellables.ContainsKey(item.Id))
            {
                // Entferne Referenzen aus allen riechenden Elementen
                var prop = item.GetProperty<SmellableProperty>();
                foreach (SnifferProperty sniffer in prop.SnifferItems.ToArray())
                {
                    sniffer.RemoveSmellableItem(prop);
                    prop.RemoveSnifferItem(sniffer);
                }

                smellables.Remove(item.Id);
            }

            // Entferne riechendes Objekt
            if (item.ContainsProperty<SnifferProperty>() &&
                sniffers.ContainsKey(item.Id))
            {
                // Enferne alle riechenden Elemente
                var prop = item.GetProperty<SnifferProperty>();
                foreach (SmellableProperty smellable in prop.SmellableItems.ToArray())
                {
                    prop.RemoveSmellableItem(smellable);
                    smellable.RemoveSnifferItem(prop);
                }

                sniffers.Remove(item.Id);
            }
        }

        public override void Update()
        {
            UpdateVisibles();
            UpdateSniffer();
        }

        #region Visibility Methods

        /// <summary>
        ///     Führt die Berechnung rund um die Sichtbarkeit durch
        /// </summary>
        private void UpdateVisibles()
        {
            // Remake the visibles map
            visiblesMap.Clear();
            foreach (VisibleProperty visible in visibles.Values)
            {
                visiblesMap.Add(visible, visible.Item.Position, visible.VisibilityRadius);
            }

            // Durchläuft alle sehenden Elemente
            foreach (SightingProperty sighting in viewers.Values)
            {
                var visibleItems = new List<VisibleProperty>();

                // Run through all visible elements in the vincinity
                foreach (VisibleProperty visible in visiblesMap.FindAll(sighting.Item.Position, sighting.ViewRange))
                {
                    // Skip, falls es das selbe Item ist.
                    if (visible.Item == sighting.Item)
                        continue;

                    float max = sighting.ViewRange + visible.VisibilityRadius;

                    // Umgang mit der Distanz
                    // TODO: Berücksichtigung von Richtung und Öffnungswinkel
                    if (Item.GetDistance(sighting.Item, visible.Item) <= max)
                    {
                        // Neues Element erspäht
                        if (!sighting.VisibleItems.Contains(visible))
                        {
                            sighting.AddVisibleItem(visible);
                            visible.AddSightingItem(sighting);
                        }

                        // Aufruf für jedes sichtbare Element pro Runde
                        sighting.NoteVisibleItem(visible);
                        visibleItems.Add(visible);
                    }
                }

                // Durchläuft alle bisher sichtbaren Items um zu sehen ob die 
                // Aktualität des Visible-Eintrags noch stimmt.
                foreach (VisibleProperty visible in sighting.VisibleItems.ToArray())
                {
                    if (!visibleItems.Contains(visible))
                    {
                        sighting.RemoveVisibleItem(visible);
                        visible.RemoveSightingItem(sighting);
                    }
                }
            }
        }

        private void item_CellChanged(Item item, Index2 newValue)
        {
            // Environment Information aktualisieren
            VisibleEnvironment env = viewers[item.Id].Environment;

            // Element außerhalb des Spielfeldes
            Index2 limit = Engine.Map.GetCellCount();
            if (newValue.X < 0 || newValue.X >= limit.X ||
                newValue.Y < 0 || newValue.Y >= limit.Y)
            {
                env.Center = null;
                env.North = null;
                env.South = null;
                env.West = null;
                env.East = null;
                env.NorthWest = null;
                env.NorthEast = null;
                env.SouthWest = null;
                env.SouthEast = null;
                return;
            }

            // Umgebungszellen durchlaufen
            for (int x = -1; x <= 1; x++)
                for (int y = -1; y <= 1; y++)
                {
                    var offset = new Index2(x, y);
                    Index2 cell = newValue + offset;

                    if (cell.X < 0 || cell.X >= limit.X ||
                        cell.Y < 0 || cell.Y >= limit.Y)
                    {
                        // Es existiert keine Zelle mehr
                        env[offset] = null;
                    }
                    else
                    {
                        // Zelleninfos ermitteln
                        float speed = Engine.Map.Tiles[cell.X, cell.Y].GetSpeedMultiplicator();
                        TileHeight height = Engine.Map.Tiles[cell.X, cell.Y].Height;
                        env[offset] = new VisibleCell
                        {
                            Speed = speed,
                            Height = height
                        };
                    }
                }

            // Inform user
            viewers[item.Id].RefreshEnvironment();
        }

        #endregion

        #region Smelling Methods

        private void UpdateSniffer()
        {
            // Remake the smellables map
            smellablesMap.Clear();
            foreach (SmellableProperty smellable in smellables.Values)
            {
                smellablesMap.Add(smellable, smellable.Item.Position, smellable.SmellableRadius);
            }

            // Durchläuft alle riechenden Elemente
            foreach (SnifferProperty sniffer in sniffers.Values)
            {
                var smellableItems = new List<SmellableProperty>();

                // Durchläuft alle riechbaren Elemente, sucht in einem "nullradius" nach riechenden Elementen.
                // Diese haben einen bestimmten Radius und können somit die riechenden Elemente überdecken.
                foreach (SmellableProperty smellable in smellablesMap.FindAll(sniffer.Item.Position, 0f))
                {
                    // Skip, falls es das selbe Item ist.
                    if (smellable.Item == sniffer.Item)
                        continue;

                    // Umgang mit der Distanz
                    if (Item.GetDistance(sniffer.Item, smellable.Item) <= smellable.SmellableRadius)
                    {
                        // Hinzufügen, falls bisher nicht gerochen
                        if (!sniffer.SmellableItems.Contains(smellable))
                        {
                            smellable.AddSnifferItem(sniffer);
                            sniffer.AddSmellableItem(smellable);
                        }

                        // Aufruf für jedes aktuell riechbare Element
                        sniffer.NoteSmellableItem(smellable);
                        smellableItems.Add(smellable);
                    }
                }

                // Durchläuft alle bisher riechbaren Items um zu sehen ob die 
                // Aktualität des Smellable-Eintrags noch stimmt.
                foreach (SmellableProperty smellable in sniffer.SmellableItems.ToArray())
                {
                    if (!smellableItems.Contains(smellable))
                    {
                        sniffer.RemoveSmellableItem(smellable);
                        smellable.RemoveSnifferItem(sniffer);
                    }
                }
            }
        }

        #endregion
    }
}