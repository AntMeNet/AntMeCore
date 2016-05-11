using AntMe.Factions.Ants;
using AntMe.ItemProperties.Basics;
using System;

namespace AntMe.Items.Basics
{
    public class AntItem : FactionItem
    {
        /// <summary>
        /// Default Radius for Ants.
        /// </summary>
        public const float AntRadius = 2f;

        /// <summary>
        /// Referenz auf das Walking Property der Ameise.
        /// </summary>
        private readonly WalkingProperty _walking;

        private readonly SightingProperty sighting;

        public AntItem(SimulationContext context, Vector2 position, Angle orientation, AntFaction faction, string name,
            PrimordialCasteAttribute caste)
            : base(context, faction, position, AntRadius, orientation)
        {
            Caste = caste.Name;
            Name = name;

            // TODO Kasten-Management

            // Sync zwischen den unterschiedlichen Richtungen von Item, Walking und Sighting
            _walking = GetProperty<WalkingProperty>();
            _walking.OnMoveDirectionChanged += (item, value) =>
            {
                Orientation = value;
                sighting.ViewDirection = value;
            };
        }

        /// <summary>
        ///     Gibt den Namen dieser Ameise zurück.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        ///     Gibt den Kasten-Namen dieser Ameise zurück.
        /// </summary>
        public string Caste { get; private set; }


        protected override void OnUpdate()
        {
            HandleMovement();
        }

        protected override void OnBeforeState(ItemState state)
        {
            AntState antState = state as AntState;
            antState.Mode = AntStateMode.Idle;
            if (_angleToGo != 0 || _distanceToGo > 0f)
                antState.Mode = AntStateMode.Walk;
        }

        #region Movement

        private int _angleToGo;
        private float _distanceToGo;
        private ItemInfo _target;

        /// <summary>
        ///     Gibt die noch zurückzulegende Strecke an, die die Ameise noch zu
        ///     gehen hat, bis sie das Zwischenziel erreicht hat.
        /// </summary>
        public float DistanceToGo
        {
            get
            {
                // Gibt die Distanz zum Ziel zurück
                if (_target != null)
                    return _target.Distance;

                // Ansonsten die angestrebte Strecke
                return _distanceToGo;
            }
        }

        /// <summary>
        ///     Gibt den Restwinkel an, den es noch zu drehen gilt.
        /// </summary>
        public int AngleToGo
        {
            get { return _angleToGo; }
        }

        /// <summary>
        ///     Gibt das aktuelle Ziel zurück oder null, falls die Ameise kein
        ///     konkretes Ziel hat.
        /// </summary>
        public ItemInfo CurrentTarget
        {
            get { return _target; }
        }

        private void HandleMovement()
        {
            // Prüfen, ob Target noch existiert und ob das Ziel
            if (_target != null && !_target.IsAlive)
            {
                Stop();
                return;
            }

            // Behandle Bewegungen
            if (_angleToGo != 0)
            {
                // Drehung
                int rot = _angleToGo > 0
                    ? Math.Min(_angleToGo, Faction.Settings.GetInt<AntItem>("AntRotationSpeed") ?? 0)
                    : Math.Max(_angleToGo, -Faction.Settings.GetInt<AntItem>("AntRotationSpeed") ?? 0);

                _walking.Speed = 0f;
                _walking.Direction = Orientation.AddDegree(rot);
                _angleToGo -= rot;
            }
            else if (_distanceToGo > 0)
            {
                // Bewegung
                if (_distanceToGo > _walking.MaximumSpeed)
                {
                    _walking.Speed = _walking.MaximumSpeed;
                    _distanceToGo -= _walking.MaximumSpeed;
                }
                else
                {
                    _walking.Speed = _distanceToGo;
                    _distanceToGo = 0;
                }
            }
            else if (_target != null)
            {
                // Erneut Kurs aufs Ziel nehmen
                Item item = GetItemFromInfo(_target);
                float distance = Item.GetDistance(this, item);
                Angle direction = Item.GetDirection(this, item);
                int angle = Angle.ConvertToDegree(Angle.Diff(Orientation, direction));

                if (distance < Faction.Settings.GetFloat<AntItem>("ZickZackRange"))
                {
                    // Genaue Route
                    _angleToGo = angle;
                    _distanceToGo = distance;
                }
                else
                {
                    // Zickzack
                    _angleToGo = angle + Faction.Random.Next(-Faction.Settings.GetInt<AntItem>("ZickZackAngle") ?? 0, Faction.Settings.GetInt<AntItem>("ZickZackAngle") ?? 0);
                    _distanceToGo = Faction.Settings.GetFloat<AntItem>("ZickZackRange") ?? 0;
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
            _distanceToGo = distance;
            _target = null;
        }

        /// <summary>
        ///     Dreht die Ameise um die angegebene Gradzahl.
        /// </summary>
        /// <param name="angle">Gradzahl</param>
        public void Turn(int angle)
        {
            _angleToGo = angle;
            _target = null;
        }

        /// <summary>
        ///     Löscht alle Navigationsbefehle und setzt das angegebe Ziel.
        /// </summary>
        /// <param name="target"></param>
        public void GoTo(ItemInfo target)
        {
            Stop();
            _target = target;
        }

        /// <summary>
        ///     Stoppt die Navigation.
        /// </summary>
        public void Stop()
        {
            _angleToGo = 0;
            _distanceToGo = 0;
            _target = null;
        }

        public event InteropProperty.InteropEvent OnWaits;

        #endregion
    }
}