using System;

namespace AntMe.Basics.ItemProperties
{
    /// <summary>
    /// Property for all attacking Items.
    /// </summary>
    public sealed class AttackerProperty : ItemProperty, IPointsCollector
    {
        private float attackRange;
        private int attackRecoveryTime;
        private int attackStrength;
        private AttackableProperty attackTarget;
        private int damageCounter;
        private int killCounter;
        private bool enablePoints;
        private int points;

        /// <summary>
        /// Default Constructor.
        /// </summary>
        /// <param name="item">Item</param>
        public AttackerProperty(Item item) : base(item) { }

        /// <summary>
        /// Returns the Points Category.
        /// </summary>
        public string PointsCategory { get { return "Attacker"; } }

        /// <summary>
        /// Defines of the Counter will be removed after Item Death.
        /// </summary>
        public bool PermanentPoints
        {
            get
            {
                // TODO: Maybe Settings? 
                // ("Keep Damage Points only for living items?")
                return true;
            }
        }

        /// <summary>
        /// Gets or sets the Attack Range.
        /// </summary>
        public float AttackRange
        {
            get { return attackRange; }
            set
            {
                attackRange = Math.Max(value, 0f);
                if (OnAttackRangeChanged != null)
                    OnAttackRangeChanged(Item, attackRange);
            }
        }

        /// <summary>
        /// Gets or sets the recovery time.
        /// </summary>
        public int AttackRecoveryTime
        {
            get { return attackRecoveryTime; }
            set
            {
                attackRecoveryTime = Math.Max(value, 0);
                if (OnAttackRecoveryTimeChanged != null)
                    OnAttackRecoveryTimeChanged(Item, attackRecoveryTime);
            }
        }

        /// <summary>
        /// Gets or sets the Attack Strength.
        /// </summary>
        public int AttackStrength
        {
            get { return attackStrength; }
            set
            {
                attackStrength = Math.Max(value, 0);
                if (OnAttackStrengthChanged != null)
                    OnAttackStrengthChanged(Item, attackStrength);
            }
        }

        /// <summary>
        /// Returns the current Target.
        /// </summary>
        public AttackableProperty AttackTarget
        {
            get { return attackTarget; }
            private set
            {
                attackTarget = value;
                if (OnAttackTargetChanged != null)
                    OnAttackTargetChanged(Item, value);
            }
        }

        /// <summary>
        /// Counts the Damage made by this Item.
        /// </summary>
        public int AttackDamageCounter
        {
            get { return damageCounter; }
            set
            {
                damageCounter = value;
                if (OnDamageCounterChanged != null)
                    OnDamageCounterChanged(Item, value);
            }
        }

        /// <summary>
        /// Counts the Number of Items killed by this Item. (Last Hit counts)
        /// </summary>
        public int AttackKillCounter
        {
            get { return killCounter; }
            set
            {
                killCounter = value;
                if (OnKillCounterChanged != null)
                    OnKillCounterChanged(Item, value);
            }
        }

        /// <summary>
        /// Returns the current Amount of Points.
        /// </summary>
        public int Points
        {
            get { return points; }
            set
            {
                points = value;
                if (OnPointsChanged != null)
                    OnPointsChanged(this, value);
            }
        }

        /// <summary>
        /// Defines if the Points should count.
        /// </summary>
        public bool EnablePoints
        {
            get { return enablePoints; }
            set
            {
                enablePoints = value;
                if (OnEnablePointsChanged != null)
                    OnEnablePointsChanged(this, value);
            }
        }

        #region Internal Methods

        /// <summary>
        /// Internal Property to hold the Recovery time. It starts at 0 after a hit or a 
        /// Target Change and counts up every round. If it is equal RecoveryTime a hit happens.
        /// </summary>
        internal int RecoveryCounter { get; set; }



        /// <summary>
        /// Internal call to perform a hit.
        /// </summary>
        /// <param name="item">Attacked Item</param>
        /// <param name="hitpoints">Hitpoints</param>
        internal void AttackHit(AttackableProperty item, int hitpoints)
        {
            // Count Points
            AttackDamageCounter += hitpoints;
            if (item.AttackableHealth <= 0 && hitpoints > 0)
                AttackKillCounter++;

            // Calculate Points
            // TODO: Create Settings
            Points = AttackDamageCounter + (100 * AttackKillCounter);

            if (OnAttackHit != null)
                OnAttackHit(item.Item, hitpoints);
        }

        #endregion

        #region Events

        /// <summary>
        /// Signal for a changed Attack Range.
        /// </summary>
        public event ValueChanged<float> OnAttackRangeChanged;

        /// <summary>
        /// Signal for a changed Target.
        /// </summary>
        public event ValueChanged<AttackableProperty> OnAttackTargetChanged;

        /// <summary>
        /// Signal for a changed Recovery Time.
        /// </summary>
        public event ValueChanged<int> OnAttackRecoveryTimeChanged;

        /// <summary>
        /// Signal for a changed Attack Strength.
        /// </summary>
        public event ValueChanged<int> OnAttackStrengthChanged;

        /// <summary>
        /// Signal for a succeeded hit.
        /// </summary>
        public event ValueChanged<int> OnAttackHit;

        /// <summary>
        /// Signal for a changed Damage Counter.
        /// </summary>
        public event ValueChanged<int> OnDamageCounterChanged;

        /// <summary>
        /// Signal for a changed Kill Counter.
        /// </summary>
        public event ValueChanged<int> OnKillCounterChanged;

        /// <summary>
        /// Signal for changed Enable Flag.
        /// </summary>
        public event ValueUpdate<IPointsCollector, bool> OnEnablePointsChanged;

        /// <summary>
        /// Signal for a changed Point Counter.
        /// </summary>
        public event ValueUpdate<IPointsCollector, int> OnPointsChanged;

        #endregion

        /// <summary>
        /// Attacks the given Item.
        /// </summary>
        /// <param name="item">Item</param>
        public void Attack(AttackableProperty item)
        {
            // Handling des alten Ziels
            if (AttackTarget != null)
            {
                // Ziel ändert sich nicht - alles gut
                if (AttackTarget == item)
                    return;

                // Altes Ziel entfernen
                StopAttack();
            }

            // Neues Ziel - Counter muss resettet werden
            RecoveryCounter = 0;

            // Prüfen, ob Attacker Teil der Simulation ist
            if (Item.Engine == null)
                throw new NotSupportedException("Attacker is not Part of the Simulation");

            // Prüfen, ob Attackable Teil der Simulation ist
            if (item.Item.Engine == null || item.Item.Engine != Item.Engine)
                throw new NotSupportedException("Attackable is not Part of the same Simulation");

            // Prüfen, ob sich das Element gerade selbst angreifen will
            if (item.Item == Item)
                throw new NotSupportedException("Item can not attack itself");

            // Ziel einfügen
            AttackTarget = item;
        }

        /// <summary>
        /// Stops fighting.
        /// </summary>
        public void StopAttack()
        {
            RecoveryCounter = 0;
            if (AttackTarget != null)
            {
                // Attacker aus Angreiferliste entfernen
                AttackTarget.RemoveAttackerItem(this);
                AttackTarget = null;
            }
        }


    }
}