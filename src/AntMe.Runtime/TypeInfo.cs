using System;
using System.Runtime.Serialization;

namespace AntMe.Runtime
{
    /// <summary>
    /// Container to hold all relevant Information to create an instance of a class 
    /// This includes Assembly-Info as byte[], The assembly Name and the full name of the Type.
    /// </summary>
    [Serializable]
    [DataContract]
    public sealed class TypeInfo
    {
        /// <summary>
        /// Complete Dump of the related Assembly File.
        /// </summary>
        [DataMember]
        public byte[] AssemblyFile { get; set; }

        /// <summary>
        /// Path to the related Assembly File.
        /// This may be null if there is an AssemblyFile Dump set.
        /// </summary>
        [DataMember]
        public string AssemblyName { get; set; }

        /// <summary>
        /// Full name of the related Type.
        /// </summary>
        [DataMember]
        public string TypeName { get; set; }

        /// <summary>
        /// Creates a new Instance of TypeInfo based on the given Type.
        /// </summary>
        /// <param name="type">Type to create a TypeInfo from.</param>
        /// <returns>New Type Info</returns>
        public static TypeInfo FromType(Type type)
        {
            return new TypeInfo()
            {
                AssemblyName = type.Assembly.FullName,
                TypeName = type.FullName
            };
        }

        /// <summary>
        /// Compares to instances of TypeInfo.
        /// </summary>
        /// <param name="obj">Other Instance</param>
        /// <returns>Returns true if both Types contains the same Assembly Name and Type Name</returns>
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

        /// <summary>
        /// Generates a Hash Code for this Instance.
        /// </summary>
        /// <returns>Hash Code</returns>
        public override int GetHashCode()
        {
            return AssemblyName.GetHashCode() + 
                TypeName.GetHashCode();
        }
    }
}
