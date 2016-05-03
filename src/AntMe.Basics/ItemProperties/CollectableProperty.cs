using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace AntMe.ItemProperties.Basics
{
    /// <summary>
    ///     Property für alle Items, die in der Lage sein sollen, Ressourcen zum
    ///     Abbau zur Verfügung zu stellen.
    /// </summary>
    public sealed class CollectableProperty : ItemProperty
    {
        private readonly Dictionary<Type, CollectableGoodProperty> collectableGoods = new Dictionary<Type, CollectableGoodProperty>();
        private float collectableRadius;

        public CollectableProperty(Item item) : base(item)
        {
            CollectableRadius = item.Radius;
            item.RadiusChanged += (i, v) =>
            {
                CollectableRadius = v;
            };
        }

        /// <summary>
        ///     Gibt den Radius an, innerhalb dessen gesammelt werden kann.
        /// </summary>
        [DisplayName("Collectable radius")]
        [Description("The radius inside which a collector can collect this collectable.")]
        public float CollectableRadius
        {
            get { return collectableRadius; }
            set
            {
                collectableRadius = Math.Max(value, 0f);
                if (OnCollectableRadiusChanged != null)
                    OnCollectableRadiusChanged(Item, collectableRadius);
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
            if (collectableGoods.ContainsKey(typeof(T)))
            {
                CollectableGoodProperty good = collectableGoods[typeof(T)];
                collectableGoods.Remove(typeof(T));
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
            return collectableGoods.ContainsKey(typeof(T));
        }

        /// <summary>
        ///     Gibt die entsprechende Ressource zurück.
        /// </summary>
        /// <typeparam name="T">Typ der Ressource</typeparam>
        /// <returns>Ressource oder null, falls nicht vorhanden</returns>
        public T GetCollectableGood<T>() where T : CollectableGoodProperty
        {
            if (collectableGoods.ContainsKey(typeof(T)))
                return (T)collectableGoods[typeof(T)];
            return null;
        }

        /// <summary>
        ///     Wird von der Engine zum echten Austausch von Ressourcen verwendet.
        /// </summary>
        /// <typeparam name="T">Typ der Ressource</typeparam>
        /// <param name="request">Angefragte Menge</param>
        /// <returns>Tatsächlich abgegebene Menge</returns>
        internal int Collect<T>(int request) where T : CollectableGoodProperty
        {
            var good = GetCollectableGood<T>();

            // Kein Austausch, falls kein Good vorhanden ist.
            if (good == null)
                return 0;

            // Negative Zahlen verhindern
            request = Math.Max(0, request);

            // Nachsehen, ob genug da ist
            int collected = Math.Max(0, Math.Min(request, good.Amount));

            // Transaktion durchführen
            if (collected > 0)
                good.Amount -= collected;
            return collected;
        }

        /// <summary>
        ///     Wird von der Engine zur Abgabe von Ressourcen verwendet.
        /// </summary>
        /// <typeparam name="T">Typ der Ressource</typeparam>
        /// <param name="amount">Abzugebende Menge</param>
        /// <returns>Tatsächlich entgegengenommene Menge</returns>
        internal int Give<T>(int amount) where T : CollectableGoodProperty
        {
            var good = GetCollectableGood<T>();

            // Falls Good nicht vorhanden, keine Transaktion
            if (good == null)
                return 0;

            // Nachsehen, ob genug Platz ist
            int given = Math.Min(amount, good.Capacity - good.Amount);

            // Transaktion durchführen
            if (given > 0)
                good.Amount += given;
            return given;
        }

        /// <summary>
        ///     Transferiert alle Güter (wenn möglich) in das Ziel.
        /// </summary>
        /// <param name="target">Ziel</param>
        /// <returns>
        ///     Erfolgsmeldung. true, wenn alles übertragen wurde oder false,
        ///     falls die Typen inkompatibel sind oder nicht ausreichend Kapazitäten
        ///     vorhanden sind.
        /// </returns>
        public bool Transfer(CollectableProperty target)
        {
            bool result = true;

            // Transfer zu sich selbst verhindern
            if (this == target)
                return false;

            foreach (CollectableGoodProperty good in collectableGoods.Values)
            {
                // Unterstützt das Ziel dieses Gut?
                if (!target.collectableGoods.ContainsKey(good.GetType()))
                {
                    result = false;
                    continue;
                }

                // Transfer
                CollectableGoodProperty temp = target.collectableGoods[good.GetType()];
                result &= good.Transfer(temp);
            }

            return result;
        }

        #region Events

        /// <summary>
        ///     Event, das bei Änderung des Sammelradius geworfen wird.
        /// </summary>
        public event ValueChanged<float> OnCollectableRadiusChanged;

        /// <summary>
        ///     Wird geworfen, wenn zur Auflistung der Ressourcen ein Eintrag hinzugefügt wird.
        /// </summary>
        public event ValueChanged<CollectableGoodProperty> OnNewCollectableGood;

        /// <summary>
        ///     Wird geworfen, wenn aus der Auflistung der Ressourcen etwas entfernt wird.
        /// </summary>
        public event ValueChanged<CollectableGoodProperty> OnLostCollectableGood;

        #endregion
    }
}