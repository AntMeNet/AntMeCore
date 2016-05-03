using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace AntMe.Runtime
{
    [Serializable]
    [DataContract]
    public sealed class TypeInfo
    {
        [DataMember]
        public byte[] AssemblyFile { get; set; }

        [DataMember]
        public string AssemblyName { get; set; }

        [DataMember]
        public string TypeName { get; set; }

        public static TypeInfo FromType(Type type)
        {
            return new TypeInfo()
            {
                AssemblyName = type.Assembly.FullName,
                TypeName = type.FullName
            };
        }

        public override bool Equals(object obj)
        {
            if (!(obj is TypeInfo))
                return false;

            if (obj == null)
                return false;

            TypeInfo other = obj as TypeInfo;
            return AssemblyName.Equals(other.AssemblyName) && 
                TypeName.Equals(other.TypeName);
        }
    }
}
