
using System;

namespace AntMe.Extension.Community
{
    public sealed class Extension : IExtensionPack
    {
        public string Author { get { return "Tom Wendel @ AntMe! GmbH"; } }

        public string Description { get { return "Test Extension um zu sehen wie das so läuft"; } }

        public string Name { get { return "Community Extension"; } }

        public Version Version { get { return new Version(2, 0); } }

        public void Load(ITypeMapper typeMapper, Settings settings)
        {
        }
    }
}
