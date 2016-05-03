using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace AntMe.ItemProperties.Basics
{
    /// <summary>
    ///     Property für angreifbare Spielelemente
    /// </summary>
    public sealed class AttackableProperty : ItemProperty
    {
        private readonly List<AttackerProperty> attackerItems = new List<AttackerProperty>();
        private int attackableHealth;
        private int attackableMaximumHealth;
        private float attackableRadius;

        public AttackableProperty(Item item) : base(item)
        {
            //AttackableRadius = radius;
            //AttackableHealth = health;
            //AttackableMaximumHealth = maximumHealth;
        }

        /// <summary>
        ///     Liefert die aktuellen Hitpoints des Elements oder legt diesen fest.
        ///     Erreicht dieser Wert kleiner gleich 0, wird dieses Element
        ///     automatisch aus der Liste enternt.
        /// </summary>
        [DisplayName("Health")]
        [Description("")]
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
        ///     Liefert die aktuellen Hitpoints des Elements oder legt diesen fest.
        ///     Erreicht dieser Wert kleiner gleich 0, wird dieses Element
        ///     automatisch aus der Liste enternt.
        /// </summary>
        [DisplayName(" Maximum Health")]
        [Description("")]
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
        ///     Liefert eine Liste der angreifenden Spielelemente.
        /// </summary>
        [Browsable(false)]
        public ReadOnlyCollection<AttackerProperty> AttackerItems
        {
            get { return attackerItems.AsReadOnly(); }
        }

        /// <summary>
        ///     Liefert den Angreifbaren Radius des Elements oder legt diesen fest.
        ///     Dies entspricht in der Regel dem Radius des Körpers.
        /// </summary>
        [DisplayName("Attackable Radius")]
        [Description("")]
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
        ///     Wird von der Engine aufgerufen, wenn ein Attacker dieses Element
        ///     als Angriffsziel gewählt hat.
        /// </summary>
        /// <param name="item">Neuer Angreifer</param>
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
        ///     Wird von der Engine aufgerufen, wenn ein Element sich aus dem
        ///     sichtbaren Radius entfernt.
        /// </summary>
        /// <param name="item">Angreifer</param>
        internal void RemoveAttackerItem(AttackerProperty item)
        {
            if (attackerItems.Remove(item))
            {
                if (OnLostAttackerItem != null)
                    OnLostAttackerItem(item);
            }
        }

        /// <summary>
        ///     Wird von der Engine in jeder Runde für jedes angreifende Element aufgerufen.
        /// </summary>
        /// <param name="item"></param>
        internal void NoteAttackerItem(AttackerProperty item)
        {
            if (OnAttackerItem != null)
                OnAttackerItem(item);
        }

        /// <summary>
        ///     Wird von der Engine aufgerufen, wenn ein Schlag durchgeführt wurde.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="hitpoints"></param>
        internal void AttackerHit(AttackerProperty item, int hitpoints)
        {
            if (OnAttackerHit != null)
                OnAttackerHit(item.Item, hitpoints);
        }

        /// <summary>
        ///     Wird von der Engine aufgerufen, um dem Attackable den eigenen Tod
        ///     zu signalisieren.
        /// </summary>
        internal void Kill()
        {
            if (OnKill != null)
                OnKill(Item);
        }

        #endregion

        #region Events

        /// <summary>
        ///     Informiert über die Änderung der Lebenspunkte dieser Einheit.
        /// </summary>
        public event ValueChanged<int> OnAttackableHealthChanged;

        /// <summary>
        ///     Informiert über die Änderung der maximalen Lebenspunkte dieser Einheit.
        /// </summary>
        public event ValueChanged<int> OnAttackableMaximumHealthChanged;

        /// <summary>
        ///     Informiert über die Änderung des angreifbaren Radius dieser Einheit.
        /// </summary>
        public event ValueChanged<float> OnAttackableRadiusChanged;

        /// <summary>
        ///     Informiert über einen neuen Angreifer.
        /// </summary>
        public event ChangeItem<AttackerProperty> OnNewAttackerItem;

        /// <summary>
        ///     Informiert über einen verlorenen Angreifer.
        /// </summary>
        public event ChangeItem<AttackerProperty> OnLostAttackerItem;

        /// <summary>
        ///     Wird in jeder Runde für jedes angreifende Element aufgerufen.
        /// </summary>
        public event ChangeItem<AttackerProperty> OnAttackerItem;

        /// <summary>
        ///     Gibt einen Treffer durch einen Angreifer bekannt.
        /// </summary>
        public event ValueChanged<int> OnAttackerHit;

        /// <summary>
        ///     Signalisiert den Tod durch einen Angriff.
        /// </summary>
        public event ChangeItem OnKill;

        #endregion
    }
}