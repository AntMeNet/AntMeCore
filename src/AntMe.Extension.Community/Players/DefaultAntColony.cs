using AntMe.Factions.Ants;
using System;

namespace AntMe.Extension.Community.Players
{
    [Player(Hidden = false, Name = "Default Ants", Author = "Random Dude")]
    public class DefaultAntColony : AntFactory
    {
        public override void Init(FactoryInterop interop)
        {
            throw new NotImplementedException();
        }
    }
}
