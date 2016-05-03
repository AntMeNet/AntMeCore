using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntMe.Extension.Test
{
    internal class DebugItemStatePropertyNoConstructors : ItemStateProperty
    {
        public override void DeserializeFirst(BinaryReader stream, byte version)
        {
            throw new NotImplementedException();
        }

        public override void DeserializeUpdate(BinaryReader stream, byte version)
        {
            throw new NotImplementedException();
        }

        public override void SerializeFirst(BinaryWriter stream, byte version)
        {
            throw new NotImplementedException();
        }

        public override void SerializeUpdate(BinaryWriter stream, byte version)
        {
            throw new NotImplementedException();
        }
    }

    internal class DebugItemStatePropertyNoEmptyConstructor : ItemStateProperty
    {
        public DebugItemStatePropertyNoEmptyConstructor(Item item, DebugItemProperty1 prop) { }

        public override void DeserializeFirst(BinaryReader stream, byte version)
        {
            throw new NotImplementedException();
        }

        public override void DeserializeUpdate(BinaryReader stream, byte version)
        {
            throw new NotImplementedException();
        }

        public override void SerializeFirst(BinaryWriter stream, byte version)
        {
            throw new NotImplementedException();
        }

        public override void SerializeUpdate(BinaryWriter stream, byte version)
        {
            throw new NotImplementedException();
        }
    }

    internal class DebugItemStateProperty1 : ItemStateProperty
    {
        public DebugItemStateProperty1()
        {

        }

        public DebugItemStateProperty1(Item item, DebugItemProperty1 prop)
        {

        }

        public override void DeserializeFirst(BinaryReader stream, byte version)
        {
            throw new NotImplementedException();
        }

        public override void DeserializeUpdate(BinaryReader stream, byte version)
        {
            throw new NotImplementedException();
        }

        public override void SerializeFirst(BinaryWriter stream, byte version)
        {
            throw new NotImplementedException();
        }

        public override void SerializeUpdate(BinaryWriter stream, byte version)
        {
            throw new NotImplementedException();
        }
    }

    internal class DebugItemStateProperty1Specialized : DebugItemStateProperty1
    {
        public DebugItemStateProperty1Specialized(Item item, DebugItemProperty1 prop) : base(item, prop) { }
    }
}
