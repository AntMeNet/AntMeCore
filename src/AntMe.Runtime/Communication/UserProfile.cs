using System.Runtime.Serialization;

namespace AntMe.Runtime.Communication
{
    /// <summary>
    /// Information Class for Users.
    /// </summary>
    [DataContract]
    public sealed class UserProfile
    {
        /// <summary>
        /// Client Id.
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Display Name.
        /// </summary>
        [DataMember]
        public string Username { get; set; }
    }
}
