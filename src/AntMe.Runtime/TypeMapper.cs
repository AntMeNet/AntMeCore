using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace AntMe.Runtime
{
    /// <summary>
    /// Repository for all extending Types within AntMe!
    /// </summary>
    public partial class TypeMapper : ITypeMapper, ITypeResolver
    {
        // Last ID: 31
        private Tracer tracer = new Tracer("AntMe.Simulation.TypeMapper");

        /// <summary>
        /// Neue Instanz des Type Mappers.
        /// </summary>
        public TypeMapper()
        {
            tracer.Trace(TraceEventType.Information, 1, "TypeMapper started");
        }

        #region Management

        /// <summary>
        /// Removes all Extension Elements with the given Extension Pack Source.
        /// </summary>
        /// <param name="extensionPack">Extension Pack to remove</param>
        public void RemoveExtensionPack(IExtensionPack extensionPack)
        {
            // Items
            foreach (var item in itemsContainer.Where(c => c.ExtensionPack == extensionPack).ToArray())
                itemsContainer.Remove(item);

            // Item Properties
            foreach (var item in itemProperties.Where(c => c.ExtensionPack == extensionPack).ToArray())
                itemProperties.Remove(item);

            // Item Attachement Properties
            foreach (var item in itemAttachments.Where(c => c.ExtensionPack == extensionPack).ToArray())
                itemAttachments.Remove(item);

            // Item Extender
            foreach (var item in itemExtender.Where(c => c.ExtensionPack == extensionPack).ToArray())
                itemExtender.Remove(item);

            // Factions
            foreach (var item in factions.Where(c => c.ExtensionPack == extensionPack).ToArray())
                factions.Remove(item);


            // Faction Properties
            foreach (var item in factionProperties.Where(c => c.ExtensionPack == extensionPack).ToArray())
                factionProperties.Remove(item);


            // Faction Attachement Properites
            foreach (var item in factionAttachments.Where(c => c.ExtensionPack == extensionPack).ToArray())
                factionAttachments.Remove(item);

            // Faction Extender
            foreach (var item in factionExtender.Where(c => c.ExtensionPack == extensionPack).ToArray())
                factionExtender.Remove(item);

            // Interop Attachment Properties
            foreach (var item in factoryInteropAttachments.Where(c => c.ExtensionPack == extensionPack).ToArray())
                factoryInteropAttachments.Remove(item);
            foreach (var item in unitInteropAttachments.Where(c => c.ExtensionPack == extensionPack).ToArray())
                unitInteropAttachments.Remove(item);

            // Interop Extender
            foreach (var item in factoryInteropExtender.Where(c => c.ExtensionPack == extensionPack).ToArray())
                factoryInteropExtender.Remove(item);
            foreach (var item in unitInteropExtender.Where(c => c.ExtensionPack == extensionPack).ToArray())
                unitInteropExtender.Remove(item);

            // Level Properties
            foreach (var item in levelProperties.Where(c => c.ExtensionPack == extensionPack).ToArray())
                levelProperties.Remove(item);

            // Level Extender
            foreach (var item in levelExtender.Where(c => c.ExtensionPack == extensionPack).ToArray())
                levelExtender.Remove(item);
        }

        /// <summary>
        /// Validator for the default Register-Parameter
        /// </summary>
        /// <param name="extensionPack">Extension Pack</param>
        /// <param name="name">Name</param>
        private void ValidateDefaults(IExtensionPack extensionPack, string name)
        {
            // Check for Extension Pack
            if (extensionPack == null)
                throw new ArgumentNullException("extensionPack");

            // Check for empty Name
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");
        }

        /// <summary>
        /// Validator for all kind of Types.
        /// </summary>
        /// <typeparam name="T">Required Base Type</typeparam>
        /// <param name="type">Type to test</param>
        /// <param name="needEmptyConstructor">Checks for empty Constructor</param>
        /// <param name="constructorParameters">Checks for a specific Constructor</param>
        private void ValidateType<T>(Type type, Type[] constructorParameters, bool needEmptyConstructor = false)
        {
            // Empty Type
            if (type == null)
                throw new ArgumentNullException("type");

            // Wrong Type Hierarchy
            if (!typeof(T).IsAssignableFrom(type))
                throw new ArgumentException(string.Format("Type '{0}' does not inherit the given Base Type", type.FullName));

            // Check for Abstract Type
            if (type.IsAbstract)
                throw new ArgumentException(string.Format("Type '{0}' can't be an abstract Type", type.FullName));

            // Check for empty Constructor
            if (needEmptyConstructor && type.GetConstructor(new Type[] { }) == null)
                throw new ArgumentException(string.Format("Type '{0}' does not have an empty Constructor", type.FullName));

            if (constructorParameters != null && type.GetConstructor(constructorParameters) == null)
                throw new ArgumentException(string.Format("Type '{0}' does not have the right Constructor Structure", type.FullName));
        }

        #endregion

        #region Type Maps

        /// <summary>
        /// Default Container for all Mapping Elements.
        /// </summary>
        private class TypeMap : ITypeMapperEntry
        {
            /// <summary>
            /// Reference to the source Extension Pack.
            /// </summary>
            public IExtensionPack ExtensionPack { get; set; }

            /// <summary>
            /// Name of the Mapping Element.
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Type of the Mapping Element.
            /// </summary>
            public Type Type { get; set; }
        }

        /// <summary>
        /// Container for all Extender Mapping Elements.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        private class ExtenderTypeMap<T> : TypeMap, IRankedTypeMapperEntry
        {
            /// <summary>
            /// Delegate to extend the Type.
            /// </summary>
            public T ExtenderDelegate { get; set; }

            /// <summary>
            /// Extender Rank.
            /// </summary>
            public int Rank { get; set; }
        }

        private class StateInfoTypeMap<CS, CI> : TypeMap, IStateInfoTypeMapperEntry
        {
            public Type StateType { get; set; }

            public Type InfoType { get; set; }

            public CS CreateStateDelegate { get; set; }

            public CI CreateInfoDelegate { get; set; }
        }

        private class AttachmentTypeMap<T> : TypeMap, IAttachmentTypeMapperEntry
        {
            public Type AttachmentType { get; set; }

            public T CreateDelegate { get; set; }
        }

        #endregion
    }
}
