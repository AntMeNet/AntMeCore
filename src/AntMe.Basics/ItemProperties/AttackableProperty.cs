using System;
using System.Collections.Generic;
using System.Linq;

namespace AntMe.Basics.ItemProperties
{
    /// <summary>
    ///     Property of all attackable Items.
    /// </summary>
    public sealed class AttackableProperty : ItemProperty
    {
        private readonly HashSet<AttackerProperty> attackerItems = new HashSet<AttackerProperty>();
        private int attackableHealth;
        private int attackableMaximumHealth;
        private float attackableRadius;

        /// <summary>
        ///     Default Constructor.
        /// </summary>
        /// <param name="item">Item</param>
        public AttackableProperty(Item item) : base(item)
        {
            AttackableRadius = item.Radius;
        }

        /// <summary>
        ///     Gets or sets the current Health.
        /// </summary>
        public int AttackableHealth
        {
            get => attackableHealth;
            set
            {
                attackableHealth = Math.Max(0, value);
                OnAttackableHealthChanged?.Invoke(Item, attackableHealth);
            }
        }

        /// <summary>
        ///     Gets or sets the maximum Health.
        /// </summary>
        public int AttackableMaximumHealth
        {
            get => attackableMaximumHealth;
            set
            {
                attackableMaximumHealth = Math.Max(0, value);
                OnAttackableMaximumHealthChanged?.Invoke(Item, attackableMaximumHealth);
            }
        }

        /// <summary>
        ///     List of all attacking Items.
        /// </summary>
        public IEnumerable<AttackerProperty> AttackerItems => attackerItems.AsEnumerable();

        /// <summary>
        ///     Gets or sets the attackable Radius.
        /// </summary>
        public float AttackableRadius
        {
            get => attackableRadius;
            set
            {
                attackableRadius = Math.Max(value, 0f);
                OnAttackableRadiusChanged?.Invoke(Item, attackableRadius);
            }
        }

        #region Internal Calls

        /// <summary>
        ///     Internal Call to add another Attacker to the List.
        /// </summary>
        /// <param name="item">New Attacker</param>
        internal void AddAttackerItem(AttackerProperty item)
        {
            if (!attackerItems.Contains(item))
            {
                attackerItems.Add(item);
                OnNewAttackerItem?.Invoke(item);
            }
        }

        /// <summary>
        ///     Internal Call to remove an Attacker from the List.
        /// </summary>
        /// <param name="item">Lost Attacker</param>
        internal void RemoveAttackerItem(AttackerProperty item)
        {
            if (attackerItems.Remove(item)) OnLostAttackerItem?.Invoke(item);
        }

        /// <summary>
        ///     Internal Call for every Attacker per Round.
        /// </summary>
        /// <param name="item">Attacker</param>
        internal void NoteAttackerItem(AttackerProperty item)
        {
            OnAttackerItem?.Invoke(item);
        }

        /// <summary>
        ///     Internal Call for a Attacker Hit.
        /// </summary>
        /// <param name="item">Attacker</param>
        /// <param name="hitpoints">Hitpoints</param>
        internal void AttackerHit(AttackerProperty item, int hitpoints)
        {
            OnAttackerHit?.Invoke(item.Item, hitpoints);
        }

        /// <summary>
        ///     Internal Call to Kill the Item.
        /// </summary>
        internal void Kill()
        {
            OnKill?.Invoke(Item);
        }

        #endregion

        #region Events

        /// <summary>
        ///     Signal for a changed Health.
        /// </summary>
        public event ValueChanged<int> OnAttackableHealthChanged;

        /// <summary>
        ///     Signal for a changed maximum Health.
        /// </summary>
        public event ValueChanged<int> OnAttackableMaximumHealthChanged;

        /// <summary>
        ///     Signal for a changed Attackable Radius.
        /// </summary>
        public event ValueChanged<float> OnAttackableRadiusChanged;

        /// <summary>
        ///     Signal for a new Attacker.
        /// </summary>
        public event ChangeItem<AttackerProperty> OnNewAttackerItem;

        /// <summary>
        ///     Signal for a lost Attacker.
        /// </summary>
        public event ChangeItem<AttackerProperty> OnLostAttackerItem;

        /// <summary>
        ///     Signal for every Attacker per Round.
        /// </summary>
        public event ChangeItem<AttackerProperty> OnAttackerItem;

        /// <summary>
        ///     Signal for a Hit.
        /// </summary>
        public event ValueChanged<int> OnAttackerHit;

        /// <summary>
        ///     Signal for a Kill.
        /// </summary>
        public event ChangeItem OnKill;

        #endregion
    }
}