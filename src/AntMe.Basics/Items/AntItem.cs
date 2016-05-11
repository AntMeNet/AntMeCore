using AntMe.Factions.Ants;
using AntMe.ItemProperties.Basics;
using System;

namespace AntMe.Items.Basics
{
    /// <summary>
    /// Represents an Ant.
    /// </summary>
    public class AntItem : FactionItem
    {
        /// <summary>
        /// Default Radius for Ants.
        /// </summary>
        public const float AntRadius = 2f;

        /// <summary>
        /// Default Mass of Ants.
        /// </summary>
        public const float AntMass = 1f;

        /// <summary>
        /// Referenz auf das Walking Property der Ameise.
        /// </summary>
        private readonly WalkingProperty walking;

        /// <summary>
        /// Creates a new Instance of an Ant.
        /// </summary>
        /// <param name="context">Simulation Context</param>
        /// <param name="faction">Related Faction</param>
        /// <param name="position">Startposition</param>
        /// <param name="orientation">Startorientation</param>
        /// <param name="name">Name of the Ant</param>
        public AntItem(SimulationContext context, AntFaction faction, Vector2 position, Angle orientation, string name)
            : base(context, faction, position, AntRadius, orientation)
        {
            Name = name;

            // Gets the Reference to the walking Property
            walking = GetProperty<WalkingProperty>();
            if (walking == null)
                throw new NotSupportedException("There is no Walking Property");
        }

        /// <summary>
        /// Returns the Name of this Ant.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Updates the Movement Information.
        /// </summary>
        protected override void OnUpdate()
        {
            HandleMovement();
        }

        /// <summary>
        /// Adds some more Information to the Item State.
        /// </summary>
        /// <param name="state">Item State</param>
        protected override void OnBeforeState(ItemState state)
        {
            AntState antState = (AntState)state;
            antState.Mode = AntStateMode.Idle;
            if (angleToGo != 0 || distanceToGo > 0f)
                antState.Mode = AntStateMode.Walk;
        }

        #region Movement

        private int angleToGo;
        private float distanceToGo;
        private ItemInfo destination;

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

        private void HandleMovement()
        {
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
                int rot = angleToGo > 0
                    ? Math.Min(angleToGo, Faction.Settings.GetInt<AntItem>("AntRotationSpeed") ?? 0)
                    : Math.Max(angleToGo, -Faction.Settings.GetInt<AntItem>("AntRotationSpeed") ?? 0);

                walking.Speed = 0f;
                walking.Direction = Orientation.AddDegree(rot);
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
                Item item = GetItemFromInfo(destination);
                float distance = Item.GetDistance(this, item);
                Angle direction = Item.GetDirection(this, item);
                int angle = Angle.ConvertToDegree(Angle.Diff(Orientation, direction));

                if (distance < Faction.Settings.GetFloat<AntItem>("ZickZackRange"))
                {
                    // Genaue Route
                    angleToGo = angle;
                    distanceToGo = distance;
                }
                else
                {
                    // Zickzack
                    angleToGo = angle + Faction.Random.Next(-Faction.Settings.GetInt<AntItem>("ZickZackAngle") ?? 0, Faction.Settings.GetInt<AntItem>("ZickZackAngle") ?? 0);
                    distanceToGo = Faction.Settings.GetFloat<AntItem>("ZickZackRange") ?? 0;
                }
            }
            else
            {
                // Kein Ziel
                if (OnWaits != null) OnWaits();
            }
        }

        /// <summary>
        ///     Überschreibt das aktuelle Target und bewegt die Ameise um die
        ///     angegebene Schrittmenge nach vorne.
        /// </summary>
        /// <param name="distance">Strecke, die die Ameise gehen soll</param>
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
        /// Clears all navigation Commands and sets the given Target.
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
        /// Signal to inform about a waiting ant.
        /// </summary>
        public event InteropProperty.InteropEvent OnWaits;

        #endregion
    }
}