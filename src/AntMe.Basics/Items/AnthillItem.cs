using AntMe.Factions.Ants;
using AntMe.ItemProperties.Basics;
using System;

namespace AntMe.Items.Basics
{
    public class AnthillItem : FactionItem
    {
        public const float HillRadius = 20f;

        private readonly SugarCollectableProperty _sugar;
        private readonly AppleCollectableProperty _apple;
        private readonly AttackableProperty _attackable;

        public AnthillItem(ITypeResolver resolver, AntFaction faction, Vector2 position)
            : base(resolver, faction, position, Angle.Right)
        {
            AntFactionSettings settings = new AntFactionSettings();

            #region Property

            var collidable = new CollidableProperty(this, HillRadius);
            AddProperty(collidable);

            if (settings.AntHillDestructable)
            {
                _attackable = new AttackableProperty(this);
                AddProperty(_attackable);
            }

            _sugar = new SugarCollectableProperty(this);
            _apple = new AppleCollectableProperty(this);
            var collectable = new CollectableProperty(this);
            AddProperty(collectable);
            AddProperty(_sugar);
            AddProperty(_apple);

            var visible = new VisibleProperty(this);
            AddProperty(visible);

            #endregion

            #region Todesbedingung

            if (settings.AntHillDestructable && _attackable != null)
            {
                _attackable.OnAttackableHealthChanged += (item, value) =>
                {
                    // Sollten die Hitpoints unter 0 kommen, ist der Ameisenhügel zerstört
                    if (value <= 0)
                        Engine.RemoveItem(this);
                };
            }

            #endregion
        }

        /// <summary>
        /// Liefert den aktuellen Zuckerstand dieses Ameisenhügels zurück.
        /// </summary>
        public int SugarAmount {
            get { return _sugar.Amount; }
        }

        /// <summary>
        /// Liefert den aktuellen Apfelstand dieses Ameisenhügels zurück.
        /// </summary>
        public int AppleAmount {
            get { return _apple.Amount; }
        }

        /// <summary>
        /// Gibt den aktuellen Gesundheitszustand des Ameisenhügels zurück.
        /// </summary>
        public int Hitpoints {
            get { return _attackable != null ? _attackable.AttackableHealth : 0; }
        }

        /// <summary>
        /// Gibt den maximalen Gesundheitszustand des Ameisenhügels zurück.
        /// </summary>
        public int MaximumHitpoints { get { return _attackable != null ? _attackable.AttackableMaximumHealth : 0; } }
    }
}