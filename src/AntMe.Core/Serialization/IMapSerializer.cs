using System;
using System.IO;

namespace AntMe.Serialization
{
    /// <summary>
    /// Internal Interface for all Map Serializer
    /// </summary>
    internal interface IMapSerializer : IDisposable
    {
        void Serialize(Stream stream, Map map);
    }
}
