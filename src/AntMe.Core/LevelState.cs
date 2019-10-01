using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace AntMe
{
    /// <summary>
    ///     Root Element for the State Tree, the Level State.
    /// </summary>
    public sealed class LevelState : PropertyList<LevelStateProperty>, ISerializableState
    {
        /// <summary>
        ///     List of all Factions.
        /// </summary>
        [Browsable(false)] public ICollection<FactionState> Factions;

        /// <summary>
        ///     List of all Items.
        /// </summary>
        [Browsable(false)] public ICollection<ItemState> Items;

        /// <summary>
        ///     Map State.
        /// </summary>
        [Browsable(false)] public MapState Map;

        /// <summary>
        ///     Default Constructor for the Deserializer.
        /// </summary>
        public LevelState()
        {
            Round = 0;
            Map = null;
            Mode = LevelMode.Uninit;

            Date = DateTimeOffset.Now;
            Factions = new HashSet<FactionState>();
            Items = new HashSet<ItemState>();
        }

        /// <summary>
        ///     Original Create Date for this Frame.
        /// </summary>
        [DisplayName("Create Date")]
        [Description("Original Create Date for this Frame.")]
        [ReadOnly(true)]
        [Category("Dynamic")]
        public DateTimeOffset Date { get; set; }

        /// <summary>
        ///     Game Mode for this State.
        /// </summary>
        [DisplayName("State")]
        [Description("Game Mode for this State")]
        [ReadOnly(true)]
        [Category("Dynamic")]
        public LevelMode Mode { get; set; }

        /// <summary>
        ///     Current Round.
        /// </summary>
        [DisplayName("Round")]
        [Description("Current Round")]
        [ReadOnly(true)]
        [Category("Dynamic")]
        public int Round { get; set; }

        /// <summary>
        ///     Serializes the first Frame of this State.
        /// </summary>
        /// <param name="stream">Output Stream</param>
        /// <param name="version">Protocol Version</param>
        public void SerializeFirst(BinaryWriter stream, byte version)
        {
            if (version != 2)
                throw new NotSupportedException("Stream Version not supported");

            stream.Write(Date.ToString("u"));
            stream.Write((byte) Mode);
            stream.Write(Round);
        }

        /// <summary>
        ///     Serializes following Frames of this State.
        /// </summary>
        /// <param name="stream">Output Stream</param>
        /// <param name="version">Protocol Version</param>
        public void SerializeUpdate(BinaryWriter stream, byte version)
        {
            if (version != 2)
                throw new NotSupportedException("Stream Version not supported");

            stream.Write(Date.ToString("u"));
            stream.Write((byte) Mode);
            stream.Write(Round);
        }

        /// <summary>
        ///     Deserializes the first Frame of this State.
        /// </summary>
        /// <param name="stream">Input Stream</param>
        /// <param name="version">Protocol Version</param>
        public void DeserializeFirst(BinaryReader stream, byte version)
        {
            if (version != 2)
                throw new NotSupportedException("Stream Version not supported");

            Date = DateTimeOffset.Parse(stream.ReadString()).ToLocalTime();
            Mode = (LevelMode) stream.ReadByte();
            Round = stream.ReadInt32();
        }

        /// <summary>
        ///     Deserializes all following Frames of this State.
        /// </summary>
        /// <param name="stream">Input Stream</param>
        /// <param name="version">Protocol Version</param>
        public void DeserializeUpdate(BinaryReader stream, byte version)
        {
            if (version != 2)
                throw new NotSupportedException("Stream Version not supported");

            Date = DateTimeOffset.Parse(stream.ReadString()).ToLocalTime();
            Mode = (LevelMode) stream.ReadByte();
            Round = stream.ReadInt32();
        }
    }
}