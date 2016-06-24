using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace AntMe
{
    /// <summary>
    /// Common Exception for invalid Map Parameter.
    /// </summary>
    public sealed class InvalidMapException : Exception
    {
        public InvalidMapException() { }

        public InvalidMapException(string message) : base(message) { }

        public InvalidMapException(string message, Exception innerException) : base(message, innerException) { }

        protected InvalidMapException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
