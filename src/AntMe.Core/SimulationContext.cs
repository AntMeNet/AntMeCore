﻿using System;

namespace AntMe
{
    /// <summary>
    /// Holds all relevant References for the current Simulation Context.
    /// </summary>
    public sealed class SimulationContext
    {
        /// <summary>
        /// Reference to the Type Resolver.
        /// </summary>
        public ITypeResolver Resolver { get; private set; }

        /// <summary>
        /// Reference to the Type Mapper.
        /// </summary>
        public ITypeMapper Mapper { get; private set; }

        /// <summary>
        /// Settings for the current Context.
        /// </summary>
        public KeyValueStore Settings { get; private set; }

        /// <summary>
        /// Randomizer for the current Context.
        /// </summary>
        public Random Random { get; private set; }

        /// <summary>
        /// Default Constructor.
        /// </summary>
        /// <param name="resolver">Reference to the Type Resolver</param>
        /// <param name="mapper">Reference to the Type Mapper</param>
        /// <param name="settings">Settings for the current Context</param>
        /// <param name="random">Randomizer for the current Context</param>
        public SimulationContext(ITypeResolver resolver, ITypeMapper mapper, KeyValueStore settings, Random random = null)
        {
            Resolver = resolver;
            Mapper = mapper;
            Settings = settings;
            Random = random;
        }
    }
}
