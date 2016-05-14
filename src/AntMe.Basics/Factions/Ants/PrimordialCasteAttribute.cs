using System;

namespace AntMe.Basics.Factions.Ants
{
    [CasteAttributeMapping(
        NameProperty = "Name",
        SpeedProperty = "Speed",
        AttackProperty = "Attack",
        DefenseProperty = "Defense",
        StrengthProperty = "Strength",
        AttentionProperty = "Attention"
    )]
    public sealed class PrimordialCasteAttribute : CasteAttribute
    {
        public string Name { get; set; }

        public int Speed { get; set; }
        
        public int Attack { get; set; }
        
        public int Defense { get; set; }

        public int Strength { get; set; }

        public int Attention { get; set; }

        public PrimordialCasteAttribute()
        {
            Name = "Default";
            Speed = 0;
            Attack = 0;
            Defense = 0;
            Strength = 0;
            Attention = 0;            
        }

        public void Check()
        {
            // TODO: Prüfung
            if (string.IsNullOrEmpty(Name))
                throw new ArgumentException("Name of Caste can't be empty");
        }
    }
}
