using System;

namespace AntMe.Basics.LevelProperties
{
    /// <summary>
    ///     Level Property for switching Player Classes (Factory and Unit) during a running Simulation.
    /// </summary>
    public sealed class HotSwapLevelProperty : LevelProperty
    {
        /// <summary>
        ///     Default Constructor for the Type Mapper.
        /// </summary>
        /// <param name="level">Reference to the Level</param>
        public HotSwapLevelProperty(Level level) : base(level)
        {
        }

        /// <summary>
        ///     Swaps the Factory Class during Simulation.
        /// </summary>
        /// <param name="slot">Affected Player Slot</param>
        /// <param name="factoryType">Type of new Factory Class</param>
        public void SwapFactoryType(byte slot, Type factoryType)
        {
            // TODO: Implement
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Swaps the Unit Classes during Simulation.
        /// </summary>
        /// <param name="slot">Affected Player Slot</param>
        /// <param name="oldUnitType">Type of new Factory Class</param>
        /// <param name="newUnitType">Type of new Unit Class</param>
        public void SwapUnitType(byte slot, Type oldUnitType, Type newUnitType)
        {
            // TODO: Implement
            throw new NotImplementedException();
        }
    }
}