using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace AntMe.Runtime
{
    /// <summary>
    /// Container to hold all relevant Information to create an instance of a class 
    /// This includes Assembly-Info as byte[], The assembly Name and the full name of the Type.
    /// </summary>
    [Serializable]
    [DataContract]
    public abstract class TypeInfo
    {
        /// <summary>
        /// Local Assembly Cache.
        /// </summary>
        private Assembly assembly = null;

        /// <summary>
        /// Full Assembly Name.
        /// </summary>
        [DataMember]
        public string AssemblyName { get; set; }

        /// <summary>
        /// Full name of the related Type.
        /// </summary>
        [DataMember]
        public string TypeName { get; set; }

        /// <summary>
        /// Compares to instances of TypeInfo.
        /// </summary>
        /// <param name="obj">Other Instance</param>
        /// <returns>Returns true if both Types contains the same Assembly Name and Type Name</returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (!(obj is TypeInfo))
                return false;

            TypeInfo other = obj as TypeInfo;
            return TypeName.Equals(other.TypeName);
        }

        /// <summary>
        /// Generates a Hash Code for this Instance.
        /// </summary>
        /// <returns>Hash Code</returns>
        public override int GetHashCode()
        {
            return TypeName.GetHashCode();
        }

        /// <summary>
        /// Returns the Assembly Instance for this Type.
        /// </summary>
        /// <returns>Assembly</returns>
        protected abstract Assembly GetAssembly();

        /// <summary>
        /// Returns the Assembly File as File Dump.
        /// </summary>
        /// <returns>File dump of the related Assembly</returns>
        public abstract byte[] GetAssemblyDump();

        /// <summary>
        /// Returns the Type Instance for this Type Info. 
        /// </summary>
        /// <returns></returns>
        public Type GetFrameworkType()
        {
            if (assembly == null)
            {
                assembly = GetAssembly();

                if (assembly == null)
                    throw new Exception(string.Format("Could not load Assembly for Type {0}", TypeName));

                if (!assembly.FullName.Equals(AssemblyName))
                    throw new Exception("Wrong Assembly loaded");
            }

            return assembly.GetType(TypeName);
        }
    }

    /// <summary>
    /// Container for a Type which came from an default loaded Extension.
    /// </summary>
    [Serializable]
    [DataContract]
    public sealed class TypeInfoByReference : TypeInfo
    {
        /// <summary>
        /// Tries to find the Assembly with the given Name in the list of loaded Assemblies.
        /// </summary>
        /// <returns>Assembly Reference</returns>
        protected override Assembly GetAssembly()
        {
            return AppDomain.CurrentDomain.GetAssemblies().
                FirstOrDefault(a => a.FullName == AssemblyName);
        }

        /// <summary>
        /// Returns the Assembly File as File Dump.
        /// </summary>
        /// <returns>File dump of the related Assembly</returns>
        public override byte[] GetAssemblyDump()
        {
            Assembly assembly = GetAssembly();
            if (assembly == null)
                throw new Exception("Could not find the fitting Assembly");

            using (var stream = assembly.GetFiles()[0])
            {
                byte[] result = new byte[stream.Length];
                stream.Read(result, 0, (int)stream.Length);
                return result;
            }
        }

        /// <summary>
        /// Generates a Type Info from a Type.
        /// </summary>
        /// <param name="type">base Type</param>
        /// <returns>Type Info Instance</returns>
        public static TypeInfoByReference FromType(Type type)
        {
            return new TypeInfoByReference()
            {
                AssemblyName = type.Assembly.FullName,
                TypeName = type.FullName
            };
        }
    }

    /// <summary>
    /// Container for a Type which was loaded by File Dump.
    /// </summary>
    [Serializable]
    [DataContract]
    public sealed class TypeInfoByDump : TypeInfo
    {
        /// <summary>
        /// Complete Dump of the related Assembly File.
        /// </summary>
        [DataMember]
        public byte[] AssemblyFile { get; set; }

        /// <summary>
        /// Optional Dump of the related Symbol Store for Debugging.
        /// </summary>
        [DataMember]
        public byte[] SymbolStore { get; set; }

        /// <summary>
        /// Generates a new Instance of the Assembly based on the given File Dump.
        /// </summary>
        /// <returns>Assembly Instance</returns>
        protected override Assembly GetAssembly()
        {
            if (SymbolStore != null)
                return Assembly.Load(AssemblyFile, SymbolStore);
            return Assembly.Load(AssemblyFile);
        }

        /// <summary>
        /// Returns the Assembly File as File Dump.
        /// </summary>
        /// <returns>File dump of the related Assembly</returns>
        public override byte[] GetAssemblyDump()
        {
            return AssemblyFile;
        }

        /// <summary>
        /// Generates a Type Info from type.
        /// </summary>
        /// <param name="type">Base Type</param>
        /// <param name="assembly">File dump from the source Assembly</param>
        /// <param name="symbolStore">Optional File dump of the related symbol store</param>
        /// <returns>Type Info Instance</returns>
        public static TypeInfoByDump FromType(Type type, byte[] assembly, byte[] symbolStore = null)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            if (assembly == null)
                throw new ArgumentNullException("assembly");

            return new TypeInfoByDump()
            {
                AssemblyFile = assembly,
                SymbolStore = symbolStore,
                AssemblyName = type.Assembly.FullName,
                TypeName = type.FullName
            };
        }

        /// <summary>
        /// Generates a Type Info from another Type Info
        /// </summary>
        /// <param name="typeInfo">Type Info</param>
        /// <param name="assembly">File dump from the source Assembly</param>
        /// <param name="symbolStore">Optional File dump of the related symbol store</param>
        /// <returns></returns>
        public static TypeInfoByDump FromTypeInfo(TypeInfo typeInfo, byte[] assembly, byte[] symbolStore = null)
        {
            if (typeInfo == null)
                throw new ArgumentNullException("typeInfo");

            if (assembly == null)
                throw new ArgumentNullException("assembly");

            return new TypeInfoByDump()
            {
                AssemblyFile = assembly,
                SymbolStore = symbolStore,
                AssemblyName = typeInfo.AssemblyName,
                TypeName = typeInfo.TypeName,
            };
        }

        /// <summary>
        /// Generates a Type Info from another Type Info and a Source Path.
        /// </summary>
        /// <param name="typeInfo">Type Info</param>
        /// <param name="filename">Path to the File</param>
        public static TypeInfo FromTypeInfo(TypeInfo typeInfo, string filename)
        {
            if (typeInfo == null)
                throw new ArgumentNullException("typeInfo");

            if (string.IsNullOrEmpty(filename))
                throw new ArgumentNullException("filename");

            byte[] filedump = File.ReadAllBytes(filename);

            string path = Path.GetDirectoryName(filename);
            string name = Path.GetFileNameWithoutExtension(filename);
            string symbolPath = Path.Combine(path, string.Format("{0}.pdb", name));

            byte[] symbols = null;
            //if (File.Exists(symbolPath))
            //    symbols = File.ReadAllBytes(symbolPath);

            return new TypeInfoByDump()
            {
                AssemblyFile = filedump,
                SymbolStore = symbols,
                AssemblyName = typeInfo.AssemblyName,
                TypeName = typeInfo.TypeName,
            };
        }
    }

    /// <summary>
    /// Container for a Type which was loaded from a file.
    /// </summary>
    [Serializable]
    [DataContract]
    public sealed class TypeInfoByFilename : TypeInfo
    {
        /// <summary>
        /// Path to the related Assembly File.
        /// </summary>
        [DataMember]
        public string Filename { get; set; }

        /// <summary>
        /// Loads the Assembly from the given Location.
        /// </summary>
        /// <returns></returns>
        protected override Assembly GetAssembly()
        {
            return Assembly.LoadFile(Filename);
        }

        /// <summary>
        /// Returns the Assembly File as File Dump.
        /// </summary>
        /// <returns>File dump of the related Assembly</returns>
        public override byte[] GetAssemblyDump()
        {
            return File.ReadAllBytes(Filename);
        }

        /// <summary>
        /// Generates a Type Info from Type.
        /// </summary>
        /// <param name="type">Base Type</param>
        /// <param name="file">Filename</param>
        /// <returns>Type Info Instance</returns>
        public static TypeInfoByFilename FromType(Type type, string file)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            if (string.IsNullOrEmpty(file))
                throw new ArgumentNullException("file");

            return new TypeInfoByFilename()
            {
                Filename = file,
                AssemblyName = type.Assembly.FullName,
                TypeName = type.FullName
            };
        }

        /// <summary>
        /// Generates a Type Info from another Type Info.
        /// </summary>
        /// <param name="typeInfo">Base Type</param>
        /// <param name="file">Filename</param>
        /// <returns></returns>
        public static TypeInfoByFilename FromTypeInfo(TypeInfo typeInfo, string file)
        {
            if (typeInfo == null)
                throw new ArgumentNullException("typeInfo");

            if (string.IsNullOrEmpty(file))
                throw new ArgumentNullException("file");

            return new TypeInfoByFilename()
            {
                Filename = file,
                AssemblyName = typeInfo.AssemblyName,
                TypeName = typeInfo.TypeName
            };
        }
    }
}
