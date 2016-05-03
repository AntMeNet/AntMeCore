using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntMe.Extension.Test
{
    internal class DebugExtensionPack : IExtensionPack
    {
        public int CallCounter { get; set; }

        public string Name
        {
            get { return "Debug Extension Pack"; }
        }

        public string Description
        {
            get { return "Debug Extension Pack"; }
        }

        public Version Version
        {
            get { return new Version(1,0); }
        }

        public string Author
        {
            get { return "Tom Wendel"; }
        }

        public void Load(ITypeMapper typeMapper)
        {
            CallCounter++;
        }
    }
}
