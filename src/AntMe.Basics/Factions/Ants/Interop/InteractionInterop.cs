using AntMe.Factions.Ants;
using AntMe.ItemProperties.Basics;
using AntMe.Items.Basics;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AntMe.Simulation.Factions.Ants.Interop
{
    public sealed class InteractionInterop : InteropProperty
    {
        private readonly AntItem _antItem;
        private readonly AntFactionSettings _settings;

        private readonly CollectorProperty _collector;
        private readonly CarrierProperty _carrier;
        private readonly AttackableProperty _attackable;
        private readonly AttackerProperty _attacker;
        private readonly SugarCollectableProperty _sugar;
        private readonly AppleCollectableProperty _apple;

        private readonly List<ItemInfo> _attackerItems = new List<ItemInfo>();

        public InteractionInterop(AntItem antItem)
        {
            _antItem = antItem;
            _settings = new AntFactionSettings();

            #region Collector

            // Relevanten Properties identifizieren
            _collector = _antItem.GetProperty<CollectorProperty>();
            if (_collector == null)
                throw new ArgumentException("Item does not contain CollectorProperty");

            _sugar = _collector.GetCollectableGood<SugarCollectableProperty>();
            if (_sugar == null)
                throw new ArgumentException("Item does not contain SugarCollectabale");

            _apple = _collector.GetCollectableGood<AppleCollectableProperty>();
            if (_apple == null)
                throw new ArgumentException("Item does not contain AppleCollectable");

            #endregion

            #region Carrier

            _carrier = _antItem.GetProperty<CarrierProperty>();
            if (_carrier == null)
                throw new ArgumentException("Item does not contain CarrierProperty");

            #endregion

            #region Attackable

            _attackable = _antItem.GetProperty<AttackableProperty>();
            if (_attackable == null)
                throw new ArgumentException("Item does not contain AttackableProperty");

            _attackable.OnKill += item =>
            {
                if (OnKill != null)
                    OnKill();
            };

            _attackable.OnAttackerHit += (item, value) =>
            {
                if (OnHit != null)
                    OnHit(value);
            };

            _attackable.OnNewAttackerItem += item =>
            {
                var Item = (Item)item.Item;
                var info = Item.GetItemInfo(antItem);

                if (!_attackerItems.Contains(info))
                    _attackerItems.Add(info);
            };

            _attackable.OnLostAttackerItem += item =>
            {
                var Item = (Item)item.Item;
                var info = Item.GetItemInfo(antItem);

                if (_attackerItems.Contains(info))
                    _attackerItems.Remove(info);
            };

            #endregion

            #region Attacker

            _attacker = _antItem.GetProperty<AttackerProperty>();
            if (_attacker == null)
                throw new ArgumentException("Item does not contain AttackerProperty");

            #endregion

            // Automatischer Collectables Handover
            var collidable = antItem.GetProperty<CollidableProperty>();
            if (collidable == null)
                throw new ArgumentException("Item does not contain AttackerProperty");

            collidable.OnCollision += (item, value) =>
            {
                // Weiter machen, wenn es sich um einen Ameisenhügel handelt.
                if (!(value is AnthillItem)) return;

                var anthill = value as AnthillItem;

                // Prüfen, ob es sich um den eigenen Ameisenhügel handelt.
                if (anthill.Faction != _antItem.Faction) return;

                // Ressourcen übertragen, sofern welche da sind
                Give(anthill);
            };
        }

        #region Methods

        /// <summary>
        /// Sammelt die passenden Ressourcen ein.
        /// </summary>
        /// <param name="food">Item, von dem eingesammelt wird.</param>
        public int Collect(ItemInfo food)
        {
            // Aktuelle Last fallen lassen
            if (IsLoaded)
                Drop();

            var item = _antItem.GetItemFromInfo(food);

            // Prüfen, ob es sich überhaupt um ein Collectable handelt.
            var collectable = item.GetProperty<CollectableProperty>();
            if (collectable == null)
                return 0;

            // Zuerst Zucker abbauen
            var result = _collector.Collect<SugarCollectableProperty>(collectable);
            if (result > 0)
                return result;

            // Alterantiv Apfel sammeln
            return _collector.Collect<AppleCollectableProperty>(collectable);
        }

        /// <summary>
        /// Lässt alle aufgesammelte Nahrung fallen.
        /// </summary>
        public void Drop()
        {
            // Drop portable
            _carrier.Drop();

            // Drop Apple
            _apple.Amount = 0;

            // Drop Sugar
            int amount = _sugar.Amount;
            if (_settings.ANT_DROP_SUGARHEAP)
                _antItem.Engine.InsertItem(new SugarItem(_antItem.Engine.TypeResolver, _antItem.Position.ToVector2XY(), amount));
        }

        /// <summary>
        /// Gibt die aktuelle Last an das gegebene Item ab.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int Give(Item item)
        {
            // Prüfen, ob es sich überhaupt um ein Collectable handelt.
            var collectable = item.GetProperty<CollectableProperty>();
            if (collectable == null)
                return 0;

            // Gib Zucker
            if (_sugar.Amount > 0)
            {
                var result = _collector.Give<SugarCollectableProperty>(collectable);
                if (result > 0)
                    return result;
            }

            // Gib Apfel
            return _collector.Give<AppleCollectableProperty>(collectable);
        }

        /// <summary>
        /// Hebt das angegebene Ziel auf, sofern es sich um etwas tragbares handelt.
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public bool Carry(ItemInfo info)
        {
            // Aktuelle Last fallen lassen
            if (IsLoaded)
                Drop();

            var item = _antItem.GetItemFromInfo(info);

            // Prüfen, ob es sich überhaupt um ein Portable handelt.
            var portable = item.GetProperty<PortableProperty>();
            if (portable == null)
                return false;

            // Aufnehmen
            return _carrier.Carry(portable);
        }

        /// <summary>
        /// Startet den Angriff auf das angegebene Ziel, sofern dieses Ziel angreifbar ist und sich in Reichweite befindet.
        /// </summary>
        /// <param name="info">Gegner</param>
        public void Attack(ItemInfo info)
        {
            var item = _antItem.GetItemFromInfo(info);
            if (!item.ContainsProperty<AttackableProperty>())
                return;

            var property = item.GetProperty<AttackableProperty>();
            _attacker.Attack(property);
        }

        /// <summary>
        /// Stoppt jegliche Angriffe.
        /// </summary>
        public void StopAttack()
        {
            _attacker.StopAttack();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Liefert eine Liste der aktuell angreifenden Einheiten.
        /// </summary>
        public ReadOnlyCollection<ItemInfo> AttackingItems { get { return _attackerItems.AsReadOnly(); } }

        /// <summary>
        /// Gibt den aktuellen Gesundheitsstand zurück.
        /// </summary>
        public int Health { get { return _attackable.AttackableHealth; } }

        /// <summary>
        /// Gibt den maximalen Wert der Gesundheit zurück.
        /// </summary>
        public int MaximumHealth { get { return _attackable.AttackableMaximumHealth; } }

        /// <summary>
        /// Gibt die Angriffsreichweite der Ameise an.
        /// </summary>
        public float AttackRange { get { return _attacker.AttackRange; } }

        /// <summary>
        /// Gibt die Stärke an, mit der die Ameise angreifen kann.
        /// </summary>
        public int AttackStrength { get { return _attacker.AttackStrength; } }

        /// <summary>
        /// Gibt die Zeit in Runden an, die die Ameise für einen Schlag braucht.
        /// </summary>
        public int AttackRecovery { get { return _attacker.AttackRecoveryTime; } }

        /// <summary>
        /// Gibt das aktuelle Angriffsziel zurück.
        /// </summary>
        public ItemInfo AttackTarget
        {
            get
            {
                return _attacker.AttackTarget != null ? ((Item) _attacker.AttackTarget.Item).GetItemInfo(_antItem) : null;
            }
        }

        /// <summary>
        /// Gibt die Tragkraft dieser Ameise zurück.
        /// </summary>
        public float Strength { get { return _carrier.CarrierStrength; } }

        /// <summary>
        /// Gibt die aktuell getragene Menge Zucker zurück.
        /// </summary>
        public int SugarLoad { get { return _sugar.Amount; } }

        /// <summary>
        /// Gibt die maximale Menge an Zucker zurück.
        /// </summary>
        public int MaximumSugarLoad { get { return _sugar.Capacity; } }

        /// <summary>
        /// Gibt die aktuell getragene Menge Apfelteilchen zurück.
        /// </summary>
        public int AppleLoad { get { return _apple.Amount; } }

        /// <summary>
        /// Gibt die maximale Menge an Apfelteilchen zurück.
        /// </summary>
        public int MaximumAppleLoad { get { return _apple.Capacity; } }

        /// <summary>
        /// Gibt das aktuell getragene Objekt zurück.
        /// </summary>
        public ItemInfo CurrentLoad
        {
            get
            {
                if (_carrier.CarrierLoad == null) return null;
                var item = _carrier.CarrierLoad.Item as Item;
                return (item != null) ? item.GetItemInfo(_antItem) : null;
            }
        }

        /// <summary>
        /// Gibt an, ob die Ameise irgendetwas geladen hat.
        /// </summary>
        public bool IsLoaded
        {
            get
            {
                return (_sugar.Amount > 0 ||
                    _apple.Amount > 0 ||
                    _carrier.CarrierLoad != null);
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Wird geworfen, wenn die Ameise von einem Angreifer getroffen wurde.
        /// </summary>
        public event InteropEvent<int> OnHit;

        /// <summary>
        /// Wird geworfen, wenn die Ameise von einem Angreifer getötet wurde.
        /// </summary>
        public event InteropEvent OnKill;

        #endregion

    }
}
