using System;

namespace AntMe.Runtime
{
    /// <summary>
    /// Simulation Setup for AntMe! Simulations.
    /// </summary>
    [Serializable]
    public sealed class Setup
    {
        /// <summary>
        /// Constructor for an empty set.
        /// </summary>
        public Setup()
        {
            Player = new TypeInfo[8];
            Colors = new PlayerColor[8];
            Teams = new byte[8];
            for (byte i = 0; i < 8; i++)
            {
                Colors[i] = (PlayerColor)i;
                Teams[i] = i;
            }
        }

        /// <summary>
        /// This is the random seed for the Level- and Player-Radomizer.
        /// </summary>
        public int Seed { get; set; }

        /// <summary>
        /// Contains the Level Info.
        /// </summary>
        public TypeInfo Level { get; set; }

        /// <summary>
        /// Contains the Player Info, ordered in Start-Positions.
        /// </summary>
        public TypeInfo[] Player { get; set; }

        /// <summary>
        /// Colors for the Players, ordered in Start-Positions.
        /// </summary>
        public PlayerColor[] Colors { get; set; }

        /// <summary>
        /// Player Teams, ordered in Start-Positions.
        /// </summary>
        public byte[] Teams { get; set; }
    }
}
