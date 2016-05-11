using System;

namespace AntMe.ItemProperties.Basics
{
    /// <summary>
    /// Property for all attacking Items.
    /// </summary>
    public sealed class AttackerProperty : ItemProperty
    {
        private float attackRange;
        private int attackRecoveryTime;
        private int attackStrength;
        private AttackableProperty attackTarget;

        /// <summary>
        /// Default Constructor.
        /// </summary>
        /// <param name="item">Item</param>
        public AttackerProperty(Item item) : base(item) { }

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