using AntMe.Basics.ItemProperties;
using AntMe.Basics.Items;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AntMe.Basics.Factions.Ants.Interop
{
    /// <summary>
    /// Physics Interop for Ants.
    /// </summary>
    public sealed class AntMovementInterop : UnitInteropProperty
    {
        private readonly WalkingProperty walking;
        private readonly CollidableProperty collidable;
        private readonly List<ItemInfo> collidedItems = new List<ItemInfo>();

        private int angleToGo;
        private float distanceToGo;
        private ItemInfo destination;

        /// <summary>
        /// Default Constructor for the Type Mapper.
        /// </summary>
        /// <param name="faction">Faction</param>
        /// <param name="item">Item</param>
        /// <param name="interop">UnitInterop</param>
        public AntMovementInterop(Faction faction, FactionItem item, UnitInterop interop) : base(faction, item, interop)
        {
            // Get Walking Property
            walking = Item.GetProperty<WalkingProperty>();
            if (walking == null)
                throw new NotSupportedException("There is no Walking Property");

            // Get Collision Property
            collidable = Item.GetProperty<CollidableProperty>();
            if (collidable == null)
                throw new NotSupportedException("There is no Collidable Property");

            // Handle Collisions with Walls and Borders.
            walking.OnHitBorder += (i, v) => { if (OnHitWall != null) OnHitWall(v); };
            walking.OnHitWall += (i, v) => { if (OnHitWall != null) OnHitWall(v); };

            // Kollisionen mit anderen Items füllt die Liste der Kollisionsitems 
            // und prüft, ob es sich beim getroffenen Item um das Ziel handelt.
            collidable.OnCollision += (i, v) =>
            {
                // Zur Liste der kollidierten Items hinzufügen.
                collidedItems.Add(v.GetItemInfo(Item));

                // Prüfen, ob es sich um das aktuelle Ziel handelt.
                if (CurrentDestination != null &&
                    Item == Item.GetItemFromInfo(CurrentDestination))
                {
                    // Alles anhalten und Reach melden
                    Stop();
                    if (OnTargetReched != null)
                        OnTargetReched(i.GetItemInfo(Item));
                }
            };
        }

        protected override void Update(int round)
        {
            // Sollten Kollisionen passiert sein, Event werfen
            if (OnCollision != null && collidedItems.Count > 0)
                OnCollision();

            collidedItems.Clear();

            // Prüfen, ob Target noch existiert und ob das Ziel
            if (destination != null && !destination.IsAlive)
            {
                Stop();
                return;
            }

            // Behandle Bewegungen
            if (angleToGo != 0)
            {
                // Drehung
                int rotSpeed = Faction.Settings.GetInt<AntItem>("RotationSpeed").Value;
                int rot = angleToGo > 0 ? Math.Min(angleToGo, rotSpeed) : Math.Max(angleToGo, -rotSpeed);

                walking.Speed = 0f;
                walking.Direction = Item.Orientation.AddDegree(rot);
                angleToGo -= rot;
            }
            else if (distanceToGo > 0)
            {
                // Bewegung
                if (distanceToGo > walking.MaximumSpeed)
                {
                    walking.Speed = walking.MaximumSpeed;
                    distanceToGo -= walking.MaximumSpeed;
                }
                else
                {
                    walking.Speed = distanceToGo;
                    distanceToGo = 0;
                }
            }
            else if (destination != null)
            {
                // Erneut Kurs aufs Ziel nehmen
                float distance = destination.Distance;
                Angle direction = destination.Direction;
                int angle = Angle.ConvertToDegree(Angle.Diff(Item.Orientation, direction));

                if (distance < Faction.Settings.GetFloat<AntItem>("ZickZackRange").Value)
                {
                    // Genaue Route
                    angleToGo = angle;
                    distanceToGo = distance;
                }
                else
                {
                    // Zickzack
                    int zzAngle = Faction.Settings.GetInt<AntItem>("ZickZackAngle").Value;
                    angleToGo = angle + Faction.Random.Next(-zzAngle, zzAngle);
                    distanceToGo = Faction.Settings.GetFloat<AntItem>("ZickZackRange").Value;
                }
            }
            else
            {
                // Kein Ziel
                if (OnWaits != null) OnWaits();
            }
        }

        /// <summary>
        /// Returns the Distance to go.
        /// </summary>
        public float DistanceToGo
        {
            get
            {
                // In Case of a Destination return Distance
                if (destination != null)
                    return destination.Distance;

                // otherwise return the freestyle Distance to go
                return distanceToGo;
            }
        }

        /// <summary>
        /// Returns the Angle to turn.
        /// </summary>
        public int AngleToGo
        {
            get { return angleToGo; }
        }

        /// <summary>
        /// Returns the current Destination Item.
        /// </summary>
        public ItemInfo CurrentDestination
        {
            get { return destination; }
        }

        public void Goahead()
        {
            Goahead(float.MaxValue);
        }

        public void Goahead(float distance)
        {
            distanceToGo = distance;
            destination = null;
        }

        /// <summary>
        /// Turns the Ant.
        /// </summary>
        /// <param name="angle">Degrees</param>
        public void Turn(int angle)
        {
            angleToGo = angle;
            destination = null;
        }

        /// <summary>
        /// Turns the Ant around (180 Degrees).
        /// </summary>
        public void TurnAround()
        {
            Turn(180);
        }

        /// <summary>
        /// Turns the Ant into the given Direction.
        /// </summary>
        /// <param name="angle">Direction</param>
        public void TurnTo(int angle)
        {
            Turn(Angle.Diff(Item.Orientation.Degree, angle));
        }

        /// <summary>
        /// Turns the Ant into the given Direction.
        /// </summary>
        /// <param name="direction">Direction</param>
        public void TurnTo(Compass direction)
        {
            TurnTo((int)direction);
        }

        /// <summary>
        /// Turns the Ant toward the given Item.
        /// </summary>
        /// <param name="destination">Destination Item</param>
        public void TurnTo(ItemInfo destination)
        {
            TurnTo(destination.Direction);
        }

        /// <summary>
        /// Clears all navigation Commands and sets the given Destination.
        /// </summary>
        /// <param name="destination">Destination</param>
        public void GoTo(ItemInfo destination)
        {
            Stop();
            this.destination = destination;
        }

        /// <summary>
        /// Stops everything.
        /// </summary>
        public void Stop()
        {
            angleToGo = 0;
            distanceToGo = 0;
            destination = null;
        }

        /// <summary>
        /// Nimmt den heimatlichen Ameisenhügel als Ziel.
        /// </summary>
        public void GoToAnthill()
        {
            Stop();

            // Anthill ermitteln
            AntFaction antFaction = Faction as AntFaction;

            // In Case of wrong Faction
            if (antFaction == null)
                return;

            var target = antFaction.GetClosestAnthill(Item);

            // No Anthill was found
            if (target == null)
                return;

            GoTo(target);
        }

        #region Properties

        /// <summary>
        /// Gibt die maximale Geschwindigkeit der Ameise zurück.
        /// </summary>
        public float MaximumSpeed { get { return walking.MaximumSpeed; } }

        /// <summary>
        /// Gibt die Kollisionsmasse der Ameise zurück.
        /// </summary>
        public float Mass { get { return collidable.CollisionFixed ? float.MaxValue : collidable.CollisionMass; } }

        /// <summary>
        /// Liefert eine Liste der Elemente, mit der die Ameise gerade kollidiert ist.
        /// </summary>
        public ReadOnlyCollection<ItemInfo> CollidedItems { get { return collidedItems.AsReadOnly(); } }

        #endregion

        #region Events

        /// <summary>
        /// Signal to inform about a waiting ant.
        /// </summary>
        public event InteropEvent OnWaits;

        public event InteropEvent<Compass> OnHitWall;

        public event InteropEvent OnCollision;

        public event InteropEvent<ItemInfo> OnTargetReched;

        #endregion
    }
}
