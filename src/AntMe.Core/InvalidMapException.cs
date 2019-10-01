using System;
using System.Runtime.Serialization;

namespace AntMe
{
    /// <summary>
    ///     Common Exception for invalid Map Parameter.
    /// </summary>
    [Serializable]
    public sealed class InvalidMapException : Exception
    {
        public InvalidMapException()
        {
        }

        public InvalidMapException(string message) : base(message)
        {
        }

        public InvalidMapException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidMapException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}