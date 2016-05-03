using System;

namespace AntMe
{
    /// <summary>
    /// Basis-Klasse für alle Info-Objekte.
    /// </summary>
    public class ItemInfo : PropertyList<ItemInfoProperty>
    {
        /// <summary>
        /// Referenz auf das bezogene Spielelement.
        /// </summary>
        protected readonly Item Item;

        /// <summary>
        /// Beobachtendes Spielelement.
        /// </summary>
        protected readonly Item Observer;

        /// <summary>
        /// Neue Instanz eines Info-Objektes.
        /// </summary>
        /// <param name="item">Bezogenes Spielelement</param>
        /// <param name="observer">Betrachtendes Spielelement</param>
        public ItemInfo(Item item, Item observer)
        {
            Item = item;
            Observer = observer;
        }

        /// <summary>
        /// Gibt den Abstand zu diesem Objekt abzüglich der Radien zurück.
        /// </summary>
        public float Distance
        {
            get { return Math.Max(0, GetDistance(Observer, Item) - Radius - Observer.Radius); }
        }

        /// <summary>
        /// Gibt die Himmelsrichtung zum Objekt zurück.
        /// </summary>
        public int Direction
        {
            get { return GetDirection(Observer, Item); }
        }

        /// <summary>
        /// Gibt den Radius des Spielelements zurück.
        /// </summary>
        public float Radius
        {
            get { return Item.Radius; }
        }

        /// <summary>
        /// Gibt an, ob das Objekt noch Teil des Spiels ist.
        /// </summary>
        public bool IsAlive
        {
            get { return Item.Id > 0; }
        }

        /// <summary>
        /// Interner Call um innerhalb von Items aus einem Info wieder ein 
        /// Item zu ermitteln.
        /// </summary>
        /// <returns>Item des aktuellen Info-Objektes</returns>
        internal Item GetItem()
        {
            return Item;
        }

        /// <summary>
        /// Ermittelt den Hashwert dieses Info-Objektes (ergibt sich aus der 
        /// Kombination aus Item- und Observer-Id).
        /// </summary>
        /// <returns>Hashwert</returns>
        public override int GetHashCode()
        {
            return Item.Id.GetHashCode() + 
                Observer.Id.GetHashCode();
        }

        /// <summary>
        /// Ermittelt, ob es sich bei den beiden Info-Objekten um die selbe 
        /// Instanz handelt. Diese Entscheidung wird auf Basis der beiden 
        /// IDs von Item und Observer getroffen.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (!(obj is ItemInfo))
                return false;

            var other = obj as ItemInfo;
            return Item == other.Item && 
                Observer == other.Observer;
        }
        
        #region Static Methoden, private

        private static float GetDistance(ItemInfo item1, ItemInfo item2)
        {
            return GetDistance(item1.Item, item2.Item);
        }

        private static float GetDistance(Item item1, Item item2)
        {
            return Item.GetDistance(item1, item2);
        }

        private static int GetDirection(ItemInfo source, ItemInfo target)
        {
            return Item.GetDirection(source.Item, target.Item).Degree;
        }

        private static int GetDirection(Item source, Item target)
        {
            return Item.GetDirection(source, target).Degree;
        }
        #endregion
        
    }
}