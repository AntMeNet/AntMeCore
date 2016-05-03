using System.Runtime.Serialization;

namespace AntMe.Runtime.Communication
{
    /// <summary>
    /// Information Class for Simulation Slots.
    /// </summary>
    [DataContract]
    public sealed class Slot
    {
        /// <summary>
        /// Slot Id (Position between 0 and 7).
        /// </summary>
        [DataMember]
        public byte Id { get; set; }

        /// <summary>
        /// Related User Profile.
        /// </summary>
        [DataMember]
        public UserProfile Profile { get; set; }

        /// <summary>
        /// Is a AI for this slot available?
        /// </summary>
        [DataMember]
        public bool PlayerInfo { get; set; }

        /// <summary>
        /// Slot Color.
        /// </summary>
        [DataMember]
        public PlayerColor ColorKey { get; set; }

        /// <summary>
        /// Slot Team.
        /// </summary>
        [DataMember]
        public byte Team { get; set; }

        /// <summary>
        /// Is Player ready?
        /// </summary>
        [DataMember]
        public bool ReadyState { get; set; }
    }
}
