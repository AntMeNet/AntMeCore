using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace AntMe.ItemProperties.Basics
{
    /// <summary>
    ///     Property für alle Items, die in der Lage sein sollen, Ressourcen von
    ///     Collectables azubauen.
    /// </summary>
    public sealed class CollectorProperty : ItemProperty
    {
        private readonly Dictionary<Type, CollectableGoodProperty> collectableGoods = new Dictionary<Type, CollectableGoodProperty>();
        private float collectorRange;

        public CollectorProperty(Item item) : base(item)
        {
            // CollectorRange = range;
        }

        /// <summary>
        ///     Liefert den Sammelradius des Spielelements oder legt diesen fest.
        /// </summary>
        [DisplayName("Collector Range")]
        [Description("")]
        public float CollectorRange
        {
            get { return collectorRange; }
            set
            {
                collectorRange = Math.Max(value, 0f);
                if (OnCollectorRangeChanged != null)
                    OnCollectorRangeChanged(Item, collectorRange);
            }
        }

        /// <summary>
        ///     Gibt eine Auflistung unterstützter Ressourcen zurück.
        /// </summary>
        [DisplayName("Collectable Goods")]
        [Description("")]
        public IEnumerable<CollectableGoodProperty> CollectableGoods
        {
            get { return collectableGoods.Values; }
        }

        /// <summary>
        ///     Fügt eine neue Ressource in die Liste hinzu.
        /// </summary>
        /// <param name="good">Neue Ressource</param>
        public void AddCollectableGood(CollectableGoodProperty good)
        {
            if (good == null)
                throw new ArgumentNullException();

            if (good.Property != null)
                throw new InvalidOperationException("Collectable Good is already in use");

            if (collectableGoods.ContainsKey(good.GetType()))
                throw new InvalidOperationException("This type of Good is already in the list");

            good.Property = this;
            collectableGoods.Add(good.GetType(), good);
            if (OnNewCollectableGood != null)
                OnNewCollectableGood(Item, good);
        }

        /// <summary>
        ///     Entfernt eine Ressource aus der Liste.
        /// </summary>
        /// <param name="good">Ressource</param>
        public void RemoveCollectableGood<T>() where T : CollectableGoodProperty
        {
            if (collectableGoods.ContainsKey(typeof (T)))
            {
                CollectableGoodProperty good = collectableGoods[typeof (T)];
                collectableGoods.Remove(typeof (T));
                if (OnLostCollectableGood != null)
                    OnLostCollectableGood(Item, good);
            }
        }

        /// <summary>
        ///     Ermittelt, ob die angegebene Ressource zur Verfügung steht.
        /// </summary>
        /// <typeparam name="T">Typ der Ressource</typeparam>
        /// <returns>Gibt an, ob diese vorhanden ist</returns>
        public bool ContainsGood<T>() where T : CollectableGoodProperty
        {
            return collectableGoods.ContainsKey(typeof (T));
        }

        /// <summary>
        ///     Gibt die entsprechende Ressource zurück.
        /// </summary>
        /// <typeparam name="T">Typ der Ressource</typeparam>
        /// <returns>Ressource oder null, falls nicht vorhanden</returns>
        public T GetCollectableGood<T>() where T : CollectableGoodProperty
        {
            if (collectableGoods.ContainsKey(typeof (T)))
                return (T) collectableGoods[typeof (T)];
            return null;
        }

        /// <summary>
        ///     Sammelt eine Ressource von einem Collectable ein.
        /// </summary>
        /// <typeparam name="T">Typ der Ressource</typeparam>
        /// <param name="item">Collectable Item von dem gesammelt werden soll</param>
        /// <param name="amount">Angefragte Menge</param>
        /// <returns>Tatsächlich übertragene Menge</returns>
        public int Collect<T>(CollectableProperty item, int amount) where T : CollectableGoodProperty
        {
            // Zugehörigkeit zur selben Engine prüfen
            if (Item.Engine == null)
                throw new NotSupportedException("Collector is not part of the Engine");
            if (item.Item.Engine == null)
                throw new NotSupportedException("Collectable is not part of the Engine");
            if (Item.Engine != item.Item.Engine)
                throw new NotSupportedException("Collector and Collectable are not part of the same Engine");

            // Prüfen, ob das Good überhaupt akzeptiert wird
            var good = GetCollectableGood<T>();
            if (good == null)
                return 0;

            // Distanz überprüfen
            if (Item.GetDistance(Item, item.Item) > collectorRange + item.CollectableRadius)
                return 0;

            // Prüfen, ob überhaupt genügend Platz ist
            amount = Math.Min(amount, good.Capacity - good.Amount);

            // Ressource anfragen
            int result = item.Collect<T>(amount);

            // Tatsächliche Transaktionsmenge einlagern
            good.Amount += result;

            return result;
        }

        /// <summary>
        ///     Sammelt die maximale Menge einer Ressource von einem Collectable ein.
        /// </summary>
        /// <typeparam name="T">Typ der Ressource</typeparam>
        /// <param name="item">Collectable Item von dem gesammelt werden soll</param>
        /// <returns>Tatsächlich übertragene Menge</returns>
        public int Collect<T>(CollectableProperty item) where T : CollectableGoodProperty
        {
            return Collect<T>(item, int.MaxValue);
        }

        /// <summary>
        ///     Gibt die angegebene Ressource an das Collectable ab.
        /// </summary>
        /// <typeparam name="T">Typ der Ressource</typeparam>
        /// <param name="item">Collectable Item, das die Ware entgegen nimmt</param>
        /// <param name="amount">Zu übertragende Menge</param>
        /// <returns>Tatsächlich entgegen genommene Menge</returns>
        public int Give<T>(CollectableProperty item, int amount) where T : CollectableGoodProperty
        {
            // Zugehörigkeit zur selben Engine prüfen
            if (Item.Engine == null)
                throw new NotSupportedException("Collector is not part of the Engine");
            if (item.Item.Engine == null)
                throw new NotSupportedException("Collectable is not part of the Engine");
            if (Item.Engine != item.Item.Engine)
                throw new NotSupportedException("Collector and Collectable are not part of the same Engine");

            // Prüfen, ob das Good überhaupt verfügbar wird
            var good = GetCollectableGood<T>();
            if (good == null)
                return 0;

            // Distanz überprüfen
            if (Item.GetDistance(Item, item.Item) > collectorRange + item.CollectableRadius)
                return 0;

            // Prüfen, ob überhaupt eine ausreichende Menge verfügbar ist
            amount = Math.Min(amount, good.Amount);

            // Ressource abgeben
            int result = item.Give<T>(amount);

            // Tatsächliche Transaktionsmenge abbuchen
            good.Amount -= result;

            return result;
        }

        /// <summary>
        ///     Gibt die gesamte verfügbare Menge der angegebenen Ressource an das
        ///     Collectable ab.
        /// </summary>
        /// <typeparam name="T">Typ der Ressource</typeparam>
        /// <param name="item">Collectable Item, das die Ware entgegen nimmt</param>
        /// <returns>Tatsächlich entgegen genommene Menge</returns>
        public int Give<T>(CollectableProperty item) where T : CollectableGoodProperty
        {
            return Give<T>(item, int.MaxValue);
        }

        /// <summary>
        ///     Event, das geworfen wird, wenn sich die Range des Collectors ändert.
        /// </summary>
        public event ValueChanged<float> OnCollectorRangeChanged;

        /// <summary>
        ///     Wird geworfen, wenn zur Auflistung der Ressourcen ein Eintrag hinzugefügt wird.
        /// </summary>
        public event ValueChanged<CollectableGoodProperty> OnNewCollectableGood;

        /// <summary>
        ///     Wird geworfen, wenn aus der Auflistung der Ressourcen etwas entfernt wird.
        /// </summary>
        public event ValueChanged<CollectableGoodProperty> OnLostCollectableGood;
    }
}