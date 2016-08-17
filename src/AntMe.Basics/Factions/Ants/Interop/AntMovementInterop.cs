using AntMe.Basics.ItemProperties;
using AntMe.Basics.Items;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AntMe.Basics.Factions.Ants.Interop
{
    /// <summary>
    /// Physics Interop for Ants.
    /// </summary>
    public sealed class AntMovementInterop : UnitInteropProperty
    {
        private int rotationSpeed;

        /// <summary>
        /// Reference to the Walking Property.
        /// </summary>
        private readonly WalkingProperty walking;

        /// <summary>
        /// Reference to the Collidable Property.
        /// </summary>
        private readonly CollidableProperty collidable;

        /// <summary>
        /// List of Collisions during the last Round.
        /// </summary>
        private readonly HashSet<ItemInfo> collidedItems = new HashSet<ItemInfo>();

        /// <summary>
        /// Angle to Rotate until Wait()
        /// </summary>
        private int angleToGo;

        /// <summary>
        /// Distance to go until Wait()
        /// </summary>
        private float distanceToGo;

        /// <summary>
        /// Current Destination Item.
        /// </summary>
        private ItemInfo destination;

        /// <summary>
        /// Default Constructor for the Type Mapper.
        /// </summary>
        /// <param name="faction">Faction</param>
        /// <param name="item">Item</param>
        /// <param name="interop">UnitInterop</param>
        public AntMovementInterop(Faction faction, FactionItem item, UnitInterop interop)
            : base(faction, item, interop)
        {
            // Get Walking Property
            walking = Item.GetProperty<WalkingProperty>();
            if (walking == null)
                throw new NotSupportedException("There is no Walking Property");

            // Get Collision Property
            collidable = Item.GetProperty<CollidableProperty>();
            if (collidable == null)
                throw new NotSupportedException("There is no Collidable Property");

            int speedAttribute = 0;
            if (item.Attributes != null)
                item.Attributes.GetValue("speed");

            switch (speedAttribute)
            {
                case -1: rotationSpeed = Faction.Settings.GetInt<AntItem>("RotationSpeed[-1]").Value; break;
                case 1: rotationSpeed = Faction.Settings.GetInt<AntItem>("RotationSpeed[1]").Value; break;
                case 2: rotationSpeed = Faction.Settings.GetInt<AntItem>("RotationSpeed[2]").Value; break;
                default: rotationSpeed = Faction.Settings.GetInt<AntItem>("RotationSpeed[0]").Value; break;
            }

            // Handle Collisions with Walls and Borders.
            walking.OnHitBorder += InternalHitWall;
            walking.OnHitWall += InternalHitWall;

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
                    OnTargetReched?.Invoke(i.GetItemInfo(Item));
                }
            };
        }

        private void InternalHitWall(Item item, Compass direction)
        {
            if (item.Settings.GetBool<AntItem>("ClassicBorderBehavior").Value)
            {
                // React on Walls like in the old Game (reflect)
                if (direction == Compass.North || direction == Compass.South)
                    item.Orientation = item.Orientation.InvertY();
                else
                    item.Orientation = item.Orientation.InvertX();
            }
            OnHitWall?.Invoke(direction);
        }

        protected override void Update(int round)
        {
            // Sollten Kollisionen passiert sein, Event werfen
            if (collidedItems.Count > 0)
                OnCollision?.Invoke();

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
                int rot = angleToGo > 0 ? Math.Min(angleToGo, rotationSpeed) : Math.Max(angleToGo, -rotationSpeed);

                walking.Speed = 0f;
                Item.Orientation = Item.Orientation.AddDegree(rot);
                angleToGo -= rot;
            }
            else if (distanceToGo > 0)
            {
                // TODO: Calculate right (based on Position, not MAxSpeed)
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
                // Recalc Direction
                float distance = destination.Distance + Item.Radius + destination.Radius;
                Angle direction = destination.Direction;
                int angle = Angle.ConvertToDegree(Angle.Diff(Item.Orientation, direction));

                if (distance < Faction.Settings.GetFloat<AntItem>("ZigZagRange").Value)
                {
                    // Genaue Route
                    angleToGo = angle;
                    distanceToGo = distance;
                }
                else
                {
                    // ZigZag
                    int zzAngle = Faction.Settings.GetInt<AntItem>("ZigZagAngle").Value;
                    angleToGo = angle + Faction.Random.Next(-zzAngle, zzAngle);
                    distanceToGo = Faction.Settings.GetFloat<AntItem>("ZigZagRange").Value;
                }
            }
            else
            {
                // Kein Ziel
                OnWaits?.Invoke();
            }
        }



        /// <summary>
        /// Let the Ant walk ahead.
        /// </summary>
        public void Goahead()
        {
            Goahead(float.MaxValue);
        }

        /// <summary>
        /// Let the Ant walk ahead for a given Distance.
        /// </summary>
        /// <param name="distance">Distance to go</param>
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
        public IEnumerable<ItemInfo> CollidedItems { get { return collidedItems.AsEnumerable(); } }

        #endregion

        #region Events

        /// <summary>
        /// Signal to inform about a waiting ant.
        /// </summary>
        public event InteropEvent OnWaits;

        /// <summary>
        /// Signals a Collision with a wall (includes Map Borders).
        /// </summary>
        public event InteropEvent<Compass> OnHitWall;

        /// <summary>
        /// Signals a Collision with another Item.
        /// </summary>
        public event InteropEvent OnCollision;

        public event InteropEvent<ItemInfo> OnTargetReched;

        #endregion
    }
}
