using AntMe.Basics.ItemProperties;
using System.Collections.Generic;

namespace AntMe.Basics.EngineExtensions
{
    /// <summary>
    ///     Extension zur Behandlung aller aktiven Interaktionen wie das Angreifen,
    ///     Einsammeln.
    /// </summary>
    public sealed class InteractionExtension : EngineProperty
    {
        private readonly Dictionary<int, AttackableProperty> attackables = new Dictionary<int, AttackableProperty>();
        private readonly Dictionary<int, AttackerProperty> attackers = new Dictionary<int, AttackerProperty>();

        public InteractionExtension(Engine engine) : base(engine) { }

        public override void Init()
        {
            // Nothing to init
        }

        public override void Insert(Item item)
        {
            // Füge angreifbares Objekt ein
            if (item.ContainsProperty<AttackableProperty>() &&
                !attackables.ContainsKey(item.Id))
            {
                var prop = item.GetProperty<AttackableProperty>();
                attackables.Add(item.Id, prop);
            }
            // Füge angreifendes Objekt ein
            if (item.ContainsProperty<AttackerProperty>() &&
                !attackers.ContainsKey(item.Id))
            {
                var prop = item.GetProperty<AttackerProperty>();
                attackers.Add(item.Id, prop);
            }
        }

        public override void Remove(Item item)
        {
            // Entferne angreifbares Objekt
            if (item.ContainsProperty<AttackableProperty>() &&
                attackables.ContainsKey(item.Id))
            {
                // Angriffe auf ein entferntes Element stoppen
                AttackableProperty attackable = attackables[item.Id];
                foreach (AttackerProperty attacker in attackers.Values)
                    if (attacker.AttackTarget == attackable)
                        attacker.StopAttack();

                // entfernen
                attackables.Remove(item.Id);
            }

            // Entferne angreifendes Objekt
            if (item.ContainsProperty<AttackerProperty>() &&
                attackers.ContainsKey(item.Id))
            {
                // Angriffe stoppen
                AttackerProperty attacker = attackers[item.Id];
                attacker.StopAttack();

                // entfernen
                attackers.Remove(item.Id);
            }
        }

        public override void Update()
        {
            foreach (AttackerProperty attacker in attackers.Values)
            {
                // Prüfen, ob überhaupt ein Ziel existiert
                if (attacker.AttackTarget == null)
                    continue;

                AttackableProperty attackable = attacker.AttackTarget;

                if (Item.GetDistance(attacker.Item, attackable.Item) <=
                    attacker.AttackRange + attackable.AttackableRadius)
                {
                    // Zum ersten mal im Angriffsbereich
                    if (!attackable.AttackerItems.Contains(attacker))
                    {
                        attackable.AddAttackerItem(attacker);
                        attacker.RecoveryCounter = 0;
                    }
                    else
                    {
                        // Mit jeder weiteren Runde im Angriffsradius lädt der Recovery Counter.
                        attacker.RecoveryCounter++;
                    }

                    // Informiere Attackable über einen Angreifer
                    attackable.NoteAttackerItem(attacker);

                    // Angriff
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
                    // Vorher im Angriffsbereich gewesen?
                    if (attackable.AttackerItems.Contains(attacker))
                    {
                        attackable.RemoveAttackerItem(attacker);
                    }
                }
            }

            // Tote Elemente aussortieren
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