﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

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
        public int Id { get; internal set; }

        /// <summary>
        /// Display Name.
        /// </summary>
        [DataMember]
        public string Username { get; internal set; }
    }
}
