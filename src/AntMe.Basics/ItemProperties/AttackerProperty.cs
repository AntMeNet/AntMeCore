using System;
using System.ComponentModel;

namespace AntMe.ItemProperties.Basics
{
    /// <summary>
    ///     Property für Items, die angreifbare Items angreifen können.
    /// </summary>
    public sealed class AttackerProperty : ItemProperty
    {
        private float attackRange;
        private int attackRecoveryTime;
        private int attackStrength;
        private AttackableProperty attackTarget;

        public AttackerProperty(Item item) : base(item)
        {
            // AttackRecoveryTime = recoveryTime;
            //AttackRecoveryTime = 0;
            //AttackRange = range;
            //AttackStrength = strength;
        }

        /// <summary>
        ///     Liefert den Angriffsradius des Spielelements oder legt diesen fest.
        /// </summary>
        [DisplayName("Attack Range")]
        [Description("")]
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
        ///     Liefert die Wartezeit zwischen zwei Angriffen oder legt diese fest.
        /// </summary>
        [DisplayName("Attack Recover Time")]
        [Description("")]
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
        ///     Liefert die Anzahl Lebenspunkte, die die Einheit bei einem Angriff abziehen kann.
        /// </summary>
        [DisplayName("Attack Strength")]
        [Description("")]
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
        ///     Liefert das Element, das aktuell angegriffen wird.
        /// </summary>
        [DisplayName("Target")]
        [Description("")]
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
        ///     Internes Feld zur Berechnung der Recovery-Zeit. Fängt bei 0 an zu
        ///     zählen (Reset bei jedem Target-Wechsel oder EIntritt in den
        ///     Angriffsradius) und feuert bei >= Attacker.RecoveryTime
        /// </summary>
        internal int RecoveryCounter { get; set; }

        #region Events

        /// <summary>
        ///     Muss geworfen werden, wenn sich der Angriffsradius der Einheit
        ///     geändert hat.
        /// </summary>
        public event ValueChanged<float> OnAttackRangeChanged;

        /// <summary>
        ///     Muss aufgerufen werden, wenn sich das aktuelle Angriffsziel ändert.
        /// </summary>
        public event ValueChanged<AttackableProperty> OnAttackTargetChanged;

        /// <summary>
        ///     Informiert über die Änderung der Erholungszeit.
        /// </summary>
        public event ValueChanged<int> OnAttackRecoveryTimeChanged;

        /// <summary>
        ///     Informiert über die Änderung der Attackenstärke.
        /// </summary>
        public event ValueChanged<int> OnAttackStrengthChanged;

        /// <summary>
        ///     Informiert über einen erfolgreichen Schlag.
        /// </summary>
        public event ValueChanged<int> OnAttackHit;

        #endregion

        /// <summary>
        ///     Legt das Angriffsziel auf das angegebene Item. Ein Erfolgreicher
        ///     Angriff ist aber immernoch anhängig von der Entfernung zum Ziel.
        /// </summary>
        /// <param name="item">Anzugreifendes Ziel</param>
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
        ///     Bricht einen Angriff auf das aktuelle Ziel ab.
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

        /// <summary>
        ///     Wird von der Extension aufgerufen um das Event für einen erfolgreichen Schlag aufzurufen.
        /// </summary>
        /// <param name="item">getroffenes Item</param>
        /// <param name="hitpoints">Menge an Hitpoints</param>
        internal void AttackHit(AttackableProperty item, int hitpoints)
        {
            if (OnAttackHit != null)
                OnAttackHit(item.Item, hitpoints);
        }
    }
}