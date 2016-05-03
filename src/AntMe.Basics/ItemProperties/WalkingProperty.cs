using System;
using System.ComponentModel;

namespace AntMe.ItemProperties.Basics
{
    /// <summary>
    ///     Eigenschaft für eigenstädig laufenden Elemente.
    /// </summary>
    public sealed class WalkingProperty : ItemProperty
    {
        private Angle _direction;
        private float _maxSpeed;
        private float _speed;

        public WalkingProperty(Item item, float maxSpeed) : base(item)
        {
            MaximumSpeed = maxSpeed;
            Direction = Angle.Right;
            Speed = 0f;
        }

        #region Properties

        /// <summary>
        ///     Gibt die gewünschte Bewegungsrichtung an. Dieser Wert kann
        ///     gewaltsam von der Engine geändert werden, falls das
        ///     Spielelement gegen eine reflektierende Wand läuft.
        /// </summary>
        [DisplayName("Direction")]
        [Description("")]
        public Angle Direction
        {
            get { return _direction; }
            set
            {
                _direction = value;
                if (OnMoveDirectionChanged != null)
                    OnMoveDirectionChanged(Item, _direction);
            }
        }

        /// <summary>
        ///     Gibt die Bewegungsgeschwindigkeit des Elementes zurück. Dabei
        ///     handelt es sich um die Basisgeschwindigkeit. Die Engine rechnet
        ///     anschließend noch die Modifikatoren der Map hinzu. Ein Malus
        ///     durch Last muss vom Spielelement behandelt werden.
        /// </summary>
        [DisplayName("Speed")]
        [Description("")]
        public float Speed
        {
            get { return _speed; }
            set
            {
                _speed = Math.Max(value, 0f);
                if (OnMoveSpeedChanged != null)
                    OnMoveSpeedChanged(Item, _speed);
            }
        }

        /// <summary>
        ///     Gibt die maximale Bewegungsgeschwindigkeit des Elementes im Lauf-Modus zurück.
        /// </summary>
        [DisplayName("Maximum Speed")]
        [Description("")]
        public float MaximumSpeed
        {
            get { return _maxSpeed; }
            set
            {
                _maxSpeed = Math.Max(value, 0f);
                if (OnMaximumMoveSpeedChanged != null)
                    OnMaximumMoveSpeedChanged(Item, _maxSpeed);
            }
        }


        /// <summary>
        ///     Kann von anderen Extensions verwendet werden, um einen Malus auf
        ///     die Bewegungsgeschwindigkeit anzuwenden. Der Standard-Wert ist 1f.
        ///     Eine Extension, die die Geschwindigkeit der Einheit halbieren
        ///     will, multipliziert diesen Wert in jeder Runde mit 0.5f.
        /// </summary>
        [Browsable(false)]
        public float MoveMalus { get; set; }

        #endregion

        #region Internal Calls

        /// <summary>
        ///     Interner Aufruf beim Auftreffen auf eine Wand.
        /// </summary>
        /// <param name="direction">Richtung der Wand</param>
        internal void HitWall(Compass direction)
        {
            if (OnHitWall != null)
                OnHitWall(Item, direction);
        }

        /// <summary>
        ///     Interner Aufruf beim Auftreffen an den Rand der Welt.
        /// </summary>
        /// <param name="direction">Richtung der Wand</param>
        internal void HitBorder(Compass direction)
        {
            if (OnHitBorder != null)
                OnHitBorder(Item, direction);
        }

        #endregion

        #region Events

        /// <summary>
        ///     Event, das beim Richtungswechsel geworfen werden muss.
        /// </summary>
        public event ValueChanged<Angle> OnMoveDirectionChanged;

        /// <summary>
        ///     Event, das beim Geschwindigkeitswechsel geworfen werden muss.
        /// </summary>
        public event ValueChanged<float> OnMoveSpeedChanged;

        /// <summary>
        ///     Event, das beim Wechsel der Maximalgeschwindigkeit geworfen werden muss.
        /// </summary>
        public event ValueChanged<float> OnMaximumMoveSpeedChanged;

        /// <summary>
        ///     Event, das aufgerufen wird, wenn das Item an den Rand der Welt
        ///     stößt.
        /// </summary>
        public event ValueChanged<Compass> OnHitBorder;

        /// <summary>
        ///     Event wird aufgerufen, wenn das Item an eine unüberwindbare
        ///     Zellengrenze anstößt.
        /// </summary>
        public event ValueChanged<Compass> OnHitWall;

        #endregion
    }
}