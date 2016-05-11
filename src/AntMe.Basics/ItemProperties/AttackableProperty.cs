using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AntMe.ItemProperties.Basics
{
    /// <summary>
    /// Property of all attackable Items.
    /// </summary>
    public sealed class AttackableProperty : ItemProperty
    {
        private readonly List<AttackerProperty> attackerItems = new List<AttackerProperty>();
        private int attackableHealth;
        private int attackableMaximumHealth;
        private float attackableRadius;

        /// <summary>
        /// Default Constructor.
        /// </summary>
        /// <param name="item">Item</param>
        public AttackableProperty(Item item) : base(item)
        {
            AttackableRadius = item.Radius;
        }

        /// <summary>
        /// Gets or sets the current Health.
        /// </summary>
        public int AttackableHealth
        {
            get { return attackableHealth; }
            set
            {
                attackableHealth = Math.Max(0, value);
                if (OnAttackableHealthChanged != null)
                    OnAttackableHealthChanged(Item, attackableHealth);
            }
        }

        /// <summary>
        /// Gets or sets the maximum Health.
        /// </summary>
        public int AttackableMaximumHealth
        {
            get { return attackableMaximumHealth; }
            set
            {
                attackableMaximumHealth = Math.Max(0, value);
                if (OnAttackableMaximumHealthChanged != null)
                    OnAttackableMaximumHealthChanged(Item, attackableMaximumHealth);
            }
        }

        /// <summary>
        /// List of all attacking Items.
        /// </summary>
        public ReadOnlyCollection<AttackerProperty> AttackerItems
        {
            get { return attackerItems.AsReadOnly(); }
        }

        /// <summary>
        /// Gets or sets the attackable Radius.
        /// </summary>
        public float AttackableRadius
        {
            get { return attackableRadius; }
            set
            {
                attackableRadius = Math.Max(value, 0f);
                if (OnAttackableRadiusChanged != null)
                    OnAttackableRadiusChanged(Item, attackableRadius);
            }
        }

        #region Internal Calls

        /// <summary>
        /// Internal Call to add another Attacker to the List.
        /// </summary>
        /// <param name="item">New Attacker</param>
        internal void AddAttackerItem(AttackerProperty item)
        {
            if (!attackerItems.Contains(item))
            {
                attackerItems.Add(item);
                if (OnNewAttackerItem != null)
                    OnNewAttackerItem(item);
            }
        }

        /// <summary>
        /// Internal Call to remove an Attacker from the List.
        /// </summary>
        /// <param name="item">Lost Attacker</param>
        internal void RemoveAttackerItem(AttackerProperty item)
        {
            if (attackerItems.Remove(item))
            {
                if (OnLostAttackerItem != null)
                    OnLostAttackerItem(item);
            }
        }

        /// <summary>
        /// Internal Call for every Attacker per Round.
        /// </summary>
        /// <param name="item">Attacker</param>
        internal void NoteAttackerItem(AttackerProperty item)
        {
            if (OnAttackerItem != null)
                OnAttackerItem(item);
        }

        /// <summary>
        /// Internal Call for a Attacker Hit.
        /// </summary>
        /// <param name="item">Attacker</param>
        /// <param name="hitpoints">Hitpoints</param>
        internal void AttackerHit(AttackerProperty item, int hitpoints)
        {
            if (OnAttackerHit != null)
                OnAttackerHit(item.Item, hitpoints);
        }

        /// <summary>
        /// Internal Call to Kill the Item.
        /// </summary>
        internal void Kill()
        {
            if (OnKill != null)
                OnKill(Item);
        }

        #endregion

        #region Events

        /// <summary>
        /// Signal for a changed Health.
        /// </summary>
        public event ValueChanged<int> OnAttackableHealthChanged;

        /// <summary>
        /// Signal for a changed maximum Health.
        /// </summary>
        public event ValueChanged<int> OnAttackableMaximumHealthChanged;

        /// <summary>
        /// Signal for a changed Attackable Radius.
        /// </summary>
        public event ValueChanged<float> OnAttackableRadiusChanged;

        /// <summary>
        /// Signal for a new Attacker.
        /// </summary>
        public event ChangeItem<AttackerProperty> OnNewAttackerItem;

        /// <summary>
        /// Signal for a lost Attacker.
        /// </summary>
        public event ChangeItem<AttackerProperty> OnLostAttackerItem;

        /// <summary>
        /// Signal for every Attacker per Round.
        /// </summary>
        public event ChangeItem<AttackerProperty> OnAttackerItem;

        /// <summary>
        /// Signal for a Hit.
        /// </summary>
        public event ValueChanged<int> OnAttackerHit;

        /// <summary>
        /// Signal for a Kill.
        /// </summary>
        public event ChangeItem OnKill;

        #endregion
    }
}