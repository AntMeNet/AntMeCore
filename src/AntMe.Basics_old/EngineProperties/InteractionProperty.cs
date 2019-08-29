using AntMe.Basics.ItemProperties;
using System.Collections.Generic;
using System.Linq;

namespace AntMe.Basics.EngineProperties
{
    /// <summary>
    /// Engine Extension to handle all Interactions
    /// </summary>
    public sealed class InteractionProperty : EngineProperty
    {
        private readonly Dictionary<int, AttackableProperty> attackables;
        private readonly Dictionary<int, AttackerProperty> attackers;

        /// <summary>
        /// Default Constructor for Type Mapper.
        /// </summary>
        /// <param name="engine">Reference to the Engine</param>
        public InteractionProperty(Engine engine) : base(engine)
        {
            attackables = new Dictionary<int, AttackableProperty>();
            attackers = new Dictionary<int, AttackerProperty>();
        }

        /// <summary>
        /// Gets a call after Engine Initialization.
        /// </summary>
        public override void Init()
        {
            // Nothing to init
        }

        /// <summary>
        /// Gets a call after adding a new Item to the Engine.
        /// </summary>
        /// <param name="item">New Item</param>
        protected override void Insert(Item item)
        {
            // Track attackable Items.
            if (item.ContainsProperty<AttackableProperty>() &&
                !attackables.ContainsKey(item.Id))
            {
                var prop = item.GetProperty<AttackableProperty>();
                attackables.Add(item.Id, prop);
            }
            // Track attacking Items.
            if (item.ContainsProperty<AttackerProperty>() &&
                !attackers.ContainsKey(item.Id))
            {
                var prop = item.GetProperty<AttackerProperty>();
                attackers.Add(item.Id, prop);
            }
        }

        /// <summary>
        /// Gets a call before removing an item from Engine.
        /// </summary>
        /// <param name="item">Removed Item</param>
        protected override void Remove(Item item)
        {
            // Remove attackable Items.
            if (item.ContainsProperty<AttackableProperty>() &&
                attackables.ContainsKey(item.Id))
            {
                // Stop all running Fights.
                AttackableProperty attackable = attackables[item.Id];
                foreach (AttackerProperty attacker in attackers.Values)
                    if (attacker.AttackTarget == attackable)
                        attacker.StopAttack();

                attackables.Remove(item.Id);
            }

            // Remove all attacking Items.
            if (item.ContainsProperty<AttackerProperty>() &&
                attackers.ContainsKey(item.Id))
            {
                // Stop all running Fights.
                AttackerProperty attacker = attackers[item.Id];
                attacker.StopAttack();

                attackers.Remove(item.Id);
            }
        }

        /// <summary>
        /// Gets a call after every Engine Update.
        /// </summary>
        public override void Update()
        {
            foreach (AttackerProperty attacker in attackers.Values)
            {
                // Check for existing Targets
                if (attacker.AttackTarget == null)
                    continue;

                AttackableProperty attackable = attacker.AttackTarget;

                if (Item.GetDistance(attacker.Item, attackable.Item) <=
                    attacker.AttackRange + attackable.AttackableRadius)
                {
                    // Check Distance to the Target
                    if (!attackable.AttackerItems.Contains(attacker))
                    {
                        attackable.AddAttackerItem(attacker);
                        attacker.RecoveryCounter = 0;
                    }
                    else
                    {
                        // Update Recovery Timer
                        attacker.RecoveryCounter++;
                    }

                    // Inform Attackable about a new Attacker
                    attackable.NoteAttackerItem(attacker);

                    // Fight
                    if (attacker.RecoveryCounter >= attacker.AttackRecoveryTime)
                    {
                        int hitpoints = attacker.AttackStrength;
                        attackable.AttackableHealth -= hitpoints;
                        attackable.AttackerHit(attacker, hitpoints);
                        attacker.AttackHit(attackable, hitpoints);
                        attacker.RecoveryCounter = -1;
                    }
                }
                else
                {
                    // Left Attack Range
                    if (attackable.AttackerItems.Contains(attacker))
                    {
                        attackable.RemoveAttackerItem(attacker);
                    }
                }
            }

            // Remove dead Items
            foreach (AttackableProperty attackable in attackables.Values)
            {
                if (attackable.AttackableHealth <= 0)
                {
                    attackable.Kill();
                    Engine.RemoveItem(attackable.Item);
                }
            }
        }
    }
}