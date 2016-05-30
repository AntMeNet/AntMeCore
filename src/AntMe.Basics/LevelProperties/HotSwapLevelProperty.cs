using System;

namespace AntMe.Basics.LevelProperties
{
    /// <summary>
    /// Level Property for switching Player Classes (Factory and Unit) during a running Simulation.
    /// </summary>
    public sealed class HotSwapLevelProperty : LevelProperty
    {
        /// <summary>
        /// Default Constructor for the Type Mapper.
        /// </summary>
        /// <param name="level">Reference to the Level</param>
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
