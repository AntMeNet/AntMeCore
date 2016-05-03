using AntMe.Factions.Ants;
using AntMe.ItemProperties.Basics;
using AntMe.Items.Basics;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AntMe.Simulation.Factions.Ants.Interop
{
    public sealed class PhysicsInterop : InteropProperty
    {
        private readonly AntItem _antItem;
        private readonly WalkingProperty _walking;
        private readonly CollidableProperty _collidable;

        private readonly List<ItemInfo> _collidedItems = new List<ItemInfo>(); 

        public PhysicsInterop(AntItem antItem)
        {
            _antItem = antItem;

            // Wenn die Ameise wartet, muss das an den Spieler weiter gegeben werden.
            _antItem.OnWaits += () => { if (OnWaits != null) OnWaits(); };

            // Walking
            _walking = _antItem.GetProperty<WalkingProperty>();
            
            // Kollision mit Rand und Zellenwänden führen zum Aufruf des OnHitWall-Events
            _walking.OnHitBorder += (item, value) => { if (OnHitWall != null) OnHitWall(value); };
            _walking.OnHitWall += (item, value) => { if (OnHitWall != null) OnHitWall(value); };

            // Collision
            _collidable = _antItem.GetProperty<CollidableProperty>();

            // Kollisionen mit anderen Items füllt die Liste der Kollisionsitems 
            // und prüft, ob es sich beim getroffenen Item um das Ziel handelt.
            _collidable.OnCollision += (item, value) =>
            {
                var Item = value as Item;
                if (Item == null) return;

                // Zur Liste der kollidierten Items hinzufügen.
                _collidedItems.Add((value as Item).GetItemInfo(_antItem));

                // Prüfen, ob es sich um das aktuelle Ziel handelt.
                if (_antItem.CurrentTarget != null && 
                    Item == _antItem.GetItemFromInfo(_antItem.CurrentTarget))
                {
                    // Alles anhalten und Reach melden
                    _antItem.Stop();
                    if (OnTargetReched != null)
                        OnTargetReched(Item.GetItemInfo(_antItem));
                }
            };
        }

        protected override void Update(int round)
        {
            // Sollten Kollisionen passiert sein, Event werfen
            if (OnCollision != null && _collidedItems.Count > 0)
                OnCollision();

            _collidedItems.Clear();
        }

        #region Methods

        /// <summary>
        /// Stoppt alle Aktivitäten der Ameise.
        /// </summary>
        public void Stop()
        {
            _antItem.Stop();
        }

        /// <summary>
        /// Bewegt die Ameise nach vorne, bis ein ein anderes Event stoppt.
        /// </summary>
        public void Goahead()
        {
            _antItem.Goahead(int.MaxValue);
        }

        /// <summary>
        /// Bewegt die Ameise um die angegebene Distanz nach vorne.
        /// </summary>
        /// <param name="distance"></param>
        public void Goahead(float distance)
        {
            _antItem.Goahead(distance);
        }

        /// <summary>
        /// Versucht zum angegebenen Element zu laufen.
        /// </summary>
        /// <param name="target"></param>
        public void GoTo(ItemInfo target)
        {
            _antItem.GoTo(target);
        }

        /// <summary>
        /// Nimmt den heimatlichen Ameisenhügel als Ziel.
        /// </summary>
        public void GoToAnthill()
        {
            Stop();

            // Faction ermitteln
            var faction = _antItem.Faction as AntFaction;
            if (faction == null)
                return;

            // Anthill ermitteln
            var target = faction.GetClosestAnthill(_antItem);
            if (target == null) // Kein Ameisenhügel mehr gefunden
                return;

            GoTo(target);
        }

        /// <summary>
        /// Dreht die Ameise in die angegebene Richtung
        /// </summary>
        /// <param name="angle"></param>
        public void TurnTo(int angle)
        {
            Turn(Angle.Diff(_antItem.Orientation.Degree, angle));
        }

        /// <summary>
        /// Dreht die Ameise in die angegebene Himmelsrichtung.
        /// </summary>
        /// <param name="direction"></param>
        public void TurnTo(Compass direction)
        {
            TurnTo((int)direction);
        }

        /// <summary>
        /// Dreht die Ameise in Richtung des angegeben Ziels.
        /// </summary>
        /// <param name="target"></param>
        public void TurnTo(ItemInfo target)
        {
            TurnTo(target.Direction);
        }

        /// <summary>
        /// Dreht die Ameise um den angegebenen Winkel.
        /// </summary>
        /// <param name="angle">Winkel</param>
        public void Turn(int angle)
        {
            _antItem.Turn(angle);
        }

        /// <summary>
        /// Dreht die Ameise in die andere Richtung.
        /// </summary>
        public void TurnAround()
        {
            _antItem.Turn(180);
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gibt die noch zurückzulegende Strecke an, die die Ameise noch zu
        ///     gehen hat, bis sie das Zwischenziel erreicht hat.
        /// </summary>
        public float DistanceToGo { get { return _antItem.DistanceToGo; } }

        /// <summary>
        ///     Gibt den Restwinkel an, den es noch zu drehen gilt.
        /// </summary>
        public float AngleToGo { get { return _antItem.AngleToGo; } }

        /// <summary>
        ///     Gibt das aktuelle Ziel zurück oder null, falls die Ameise kein
        ///     konkretes Ziel hat.
        /// </summary>

        public ItemInfo CurrentTarget { get { return _antItem.CurrentTarget; }}

        /// <summary>
        /// Gibt die maximale Geschwindigkeit der Ameise zurück.
        /// </summary>
        public float MaximumSpeed { get { return _walking.MaximumSpeed; } }

        /// <summary>
        /// Gibt die Kollisionsmasse der Ameise zurück.
        /// </summary>
        public float Mass { get { return _collidable.CollisionFixed ? float.MaxValue : _collidable.CollisionMass; } }

        /// <summary>
        /// Liefert eine Liste der Elemente, mit der die Ameise gerade kollidiert ist.
        /// </summary>
        public ReadOnlyCollection<ItemInfo> CollidedItems { get { return _collidedItems.AsReadOnly(); } }

        #endregion

        #region Events

        /// <summary>
        /// Wird aufgerufen, wenn die Ameise ihre Wegstrecke zurück gelegt hat und Arbeitslos ist.
        /// </summary>
        public event InteropEvent OnWaits;

        /// <summary>
        /// Wird aufgerufen, wenn die Ameise an den Spielfeld- oder Zellenrand stößt.
        /// </summary>
        public event InteropEvent<Compass> OnHitWall;

        /// <summary>
        /// Wird aufgerufen, sobald mindestens eine Kollision stattgefunden hat.
        /// </summary>
        public event InteropEvent OnCollision;

        /// <summary>
        /// Wird aufgerufen, sobald die Ameise mit dem angestrebten Ziel kollidiert.
        /// </summary>
        public event InteropEvent<ItemInfo> OnTargetReched;

        #endregion
    }
}
