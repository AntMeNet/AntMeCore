using System;

namespace AntMe.Factions.Ants
{
    [AttributeUsage(
        AttributeTargets.Class,
        AllowMultiple = false,
        Inherited = false
    )]
    public sealed class CasteAttributeMappingAttribute : Attribute
    {
        /// <summary>
        /// Legt das Property für den Kastennamen fest.
        /// </summary>
        public string NameProperty { get; set; }

        /// <summary>
        /// Legt das Property für die Bewegungsgeschwindigkeit fest.
        /// </summary>
        public string SpeedProperty { get; set; }

        /// <summary>
        /// Legt das Property für die Stärke (Tragkraft) fest.
        /// </summary>
        public string StrengthProperty { get; set; }

        /// <summary>
        /// Legt das Property für die Aufmerksamkeit (Sichtweite) fest.
        /// </summary>
        public string AttentionProperty { get; set; }

        /// <summary>
        /// Legt das Property für den Angriffswert fest.
        /// </summary>
        public string AttackProperty { get; set; }

        /// <summary>
        /// Legt das Property für den Verteidigungswert (Hitpoints) fest.
        /// </summary>
        public string DefenseProperty { get; set; }
    }
}
