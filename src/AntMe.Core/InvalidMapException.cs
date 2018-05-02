using System;
using System.Runtime.Serialization;

namespace AntMe
{
    /// <summary>
    /// Common Exception for invalid Map Parameter.
    /// </summary>
    [Serializable]
    public sealed class InvalidMapException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        public InvalidMapException() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public InvalidMapException(string message) : base(message) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public InvalidMapException(string message, Exception innerException) : base(message, innerException) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public InvalidMapException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
