using System;

namespace AntMe.Runtime
{
    /// <summary>
    ///     Information Container for Levels.
    /// </summary>
    [Serializable]
    public sealed class LevelInfo
    {
        /// <summary>
        ///     Type Description.
        /// </summary>
        public TypeInfo Type { get; set; }

        /// <summary>
        ///     Level Description.
        /// </summary>
        public LevelDescriptionAttribute LevelDescription { get; set; }

        /// <summary>
        ///     List of applied Faction Filter.
        /// </summary>
        public LevelFilterInfo[] FactionFilter { get; set; }

        /// <summary>
        ///     Level Map.
        /// </summary>
        public byte[] Map { get; set; }

        /// <summary>
        ///     Statistics for this Level.
        /// </summary>
        public LevelStatistics Statistics { get; set; }

        /// <summary>
        ///     Returns the Name of this Level.
        /// </summary>
        /// <returns>Name</returns>
        public override string ToString()
        {
            if (LevelDescription != null)
                return LevelDescription.Name;
            return string.Empty;
        }
    }
}