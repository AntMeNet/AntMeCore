using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntMe.Basics.LevelProperties
{
    public sealed class HotSwapLevelProperty : LevelProperty
    {
        public HotSwapLevelProperty(Level level) : base(level) { }

        /// <summary>
        /// Tauscht den Factory Type der entsprechenden Faction während der Simulation.
        /// </summary>
        /// <param name="faction"></param>
        /// <param name="factoryType"></param>
        public void SwitchFactoryType(int faction, Type factoryType)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Tauscht beliebige Unit Types durch den angegebenen Type während der Simulation aus.
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="oldUnitType"></param>
        /// <param name="newUnitType"></param>
        public void SwitchUnitType(int factory, Type oldUnitType, Type newUnitType)
        {
            throw new NotImplementedException();
        }
    }
}
