using System;
using System.Collections.Generic;
using System.Linq;
using AntMe.Basics.ItemProperties;
using AntMe.Basics.Items;

namespace AntMe.Basics.Factions.Ants.Interop
{
    /// <summary>
    ///     Interaction Interop for Ants.
    /// </summary>
    public sealed class InteractionInterop : UnitInteropProperty
    {
        private readonly AppleCollectorProperty apple;
        private readonly AttackableProperty attackable;
        private readonly AttackerProperty attacker;

        private readonly HashSet<ItemInfo> attackerItems = new HashSet<ItemInfo>();
        private readonly CarrierProperty carrier;
        private readonly SugarCollectorProperty sugar;

        /// <summary>
        ///     Default Constructor for the Type Mapper.
        /// </summary>
        /// <param name="faction">Faction</param>
        /// <param name="item">Item</param>
        /// <param name="interop">UnitInterop</param>
        public InteractionInterop(Faction faction, FactionItem item, UnitInterop interop) : base(faction, item, interop)
        {
            #region Collector

            sugar = item.GetProperty<SugarCollectorProperty>();
            if (sugar == null)
                throw new ArgumentException("Item does not contain SugarCollector");

            apple = item.GetProperty<AppleCollectorProperty>();
            if (apple == null)
                throw new ArgumentException("Item does not contain AppleCollector");

            #endregion

            #region Carrier

            carrier = item.GetProperty<CarrierProperty>();
            if (carrier == null)
                throw new ArgumentException("Item does not contain CarrierProperty");

            #endregion

            #region Attackable

            attackable = item.GetProperty<AttackableProperty>();
            if (attackable == null)
                throw new ArgumentException("Item does not contain AttackableProperty");

            attackable.OnKill += i => { OnKill?.Invoke(); };

            attackable.OnAttackerHit += (i, value) => { OnHit?.Invoke(value); };

            attackable.OnNewAttackerItem += i =>
            {
                var info = Item.GetItemInfo(i.Item);

                if (!attackerItems.Contains(info))
                    attackerItems.Add(info);
            };

            attackable.OnLostAttackerItem += i =>
            {
                var info = Item.GetItemInfo(i.Item);

                if (attackerItems.Contains(info))
                    attackerItems.Remove(info);
            };

            #endregion

            #region Attacker

            attacker = item.GetProperty<AttackerProperty>();
            if (attacker == null)
                throw new ArgumentException("Item does not contain AttackerProperty");

            #endregion

            // Automatic Resource Transfer on Anthill Collision.
            var collidable = item.GetProperty<CollidableProperty>();
            if (collidable == null)
                throw new ArgumentException("Item does not contain AttackerProperty");

            collidable.OnCollision += (i, value) =>
            {
                // Ignore if it's not a Anthill
                if (!(value is AnthillItem)) return;

                var anthill = value as AnthillItem;

                // Ignore if it's not the right faction
                if (anthill.Faction != item.Faction) return;

                // Transfer all collectables
                Give(anthill);
            };
        }

        #region Methods

        /// <summary>
        ///     Takes as much as possible Food from the Item.
        /// </summary>
        /// <param name="food">Item to take from</param>
        public int Collect(ItemInfo food)
        {
            // Aktuelle Last fallen lassen
            if (IsLoaded)
                Drop();

            var i = Item.GetItemFromInfo(food);

            // Enumerate over all Collectable Properties to find a matching Type
            foreach (var property in i.Properties.OfType<CollectableProperty>())
            {
                // Search for a Collector Property fitting to the Collectable
                var hit = Item.Properties.OfType<CollectorProperty>()
                    .FirstOrDefault(p => p.GetType() == property.AcceptedCollectorType);
                if (hit != null)
                {
                    // Try to Take as much as possible
                    var amount = hit.Take(property, int.MaxValue);
                    if (amount > 0) return amount;
                }
            }

            return 0;
        }

        /// <summary>
        ///     Drops all stuff
        /// </summary>
        public void Drop()
        {
            // Drop portable
            carrier.Drop();

            // Drops Collectables
            apple.Amount = 0;

            // Drop Sugar
            var amount = sugar.Amount;
            if (Item.Settings.GetBool<AntItem>("DropSugar").Value)
                Item.Engine.InsertItem(new SugarItem(Item.Faction.Level.Context, Item.Position.ToVector2XY(), amount));
        }

        /// <summary>
        ///     Transfers all Collectable Ressources to the Destination.
        /// </summary>
        /// <param name="item">Destination</param>
        /// <returns>Transfered Amount</returns>
        public int Give(ItemInfo item)
        {
            var i = Item.GetItemFromInfo(item);
            return Give(i);
        }

        /// <summary>
        ///     Transfers all Collectable Ressources to the Destination.
        /// </summary>
        /// <param name="item">Destination</param>
        /// <returns>Transfered Amount</returns>
        private int Give(Item item)
        {
            var result = 0;
            foreach (var property in item.Properties.OfType<CollectableProperty>())
            {
                var hit = Item.Properties.OfType<CollectorProperty>()
                    .FirstOrDefault(p => p.GetType() == property.AcceptedCollectorType);
                if (hit != null) result += hit.Give(property, int.MaxValue);
            }

            return result;
        }

        /// <summary>
        ///     Tries to pick up the target item.
        /// </summary>
        /// <param name="info">Item to pick up</param>
        /// <returns>Success</returns>
        public bool Carry(ItemInfo info)
        {
            // Drops old Items
            if (carrier.CarrierLoad != null)
                carrier.Drop();

            var item = Item.GetItemFromInfo(info);

            // Make sure target is portable
            var portable = item.GetProperty<PortableProperty>();
            if (portable == null) return false;

            // Take
            return carrier.Carry(portable);
        }

        /// <summary>
        ///     Starts to attack the target Item.
        /// </summary>
        /// <param name="info">Enemy</param>
        public void Attack(ItemInfo info)
        {
            var item = Item.GetItemFromInfo(info);
            var attackable = item.GetProperty<AttackableProperty>();

            // Make sure target is attackable
            if (attackable == null) return;

            // Start to attack
            attacker.Attack(attackable);
        }

        /// <summary>
        ///     Stop to attack Items.
        /// </summary>
        public void StopAttack()
        {
            attacker.StopAttack();
        }

        #endregion

        #region Properties

        /// <summary>
        ///     List of attacking Items.
        /// </summary>
        public IEnumerable<ItemInfo> AttackingItems => attackerItems.AsEnumerable();

        /// <summary>
        ///     Gets the current Health state.
        /// </summary>
        public int Health => attackable.AttackableHealth;

        /// <summary>
        ///     Gets the maximum possible Health.
        /// </summary>
        public int MaximumHealth => attackable.AttackableMaximumHealth;

        /// <summary>
        ///     Returns the own Attack Range.
        /// </summary>
        public float AttackRange => attacker.AttackRange;

        /// <summary>
        ///     Returns the own Attack Strength.
        /// </summary>
        public int AttackStrength => attacker.AttackStrength;

        /// <summary>
        ///     Returns the Recovery Time per Hit.
        /// </summary>
        public int AttackRecovery => attacker.AttackRecoveryTime;

        /// <summary>
        ///     Returns the current Target.
        /// </summary>
        public ItemInfo AttackTarget => attacker.AttackTarget?.Item.GetItemInfo(Item);

        /// <summary>
        ///     Returns the own Carrier Strength.
        /// </summary>
        public float Strength => carrier.CarrierStrength;

        /// <summary>
        ///     Returns the current Sugar Load.
        /// </summary>
        public int SugarLoad => sugar.Amount;

        /// <summary>
        ///     Returns the total Sugar Capacity.
        /// </summary>
        public int MaximumSugarLoad => sugar.Capacity;

        /// <summary>
        ///     Returns the current Load of Apple Parts.
        /// </summary>
        public int AppleLoad => apple.Amount;

        /// <summary>
        ///     Returns the total Apple Capacity.
        /// </summary>
        public int MaximumAppleLoad => apple.Capacity;

        /// <summary>
        ///     Returns the current Load.
        /// </summary>
        public ItemInfo CurrentLoad => carrier.CarrierLoad?.Item.GetItemInfo(Item);

        /// <summary>
        ///     Returns whenever the Item carries anything.
        /// </summary>
        public bool IsLoaded
        {
            get
            {
                if (carrier.CarrierLoad != null) return true;
                return Item.Properties.OfType<CollectorProperty>().Any(p => p.Amount > 0);
            }
        }

        #endregion

        #region Events

        /// <summary>
        ///     Signals a Hit by an Enemy.
        /// </summary>
        public event InteropEvent<int> OnHit;

        /// <summary>
        ///     Signals the death of the Ant.
        /// </summary>
        public event InteropEvent OnKill;

        #endregion
    }
}