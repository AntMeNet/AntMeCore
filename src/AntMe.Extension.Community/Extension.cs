using System;

namespace AntMe.Extension.Community
{
    public sealed class Extension : IExtensionPack
    {
        public string Author => "Tom Wendel @ AntMe! GmbH";

        public string Description => "Test Extension um zu sehen wie das so läuft";

        public string Name => "Community Extension";

        public Version Version => new Version(2, 0);

        public void Load(ITypeMapper typeMapper, KeyValueStore settings, KeyValueStore dictionary)
        {
        }
    }
}