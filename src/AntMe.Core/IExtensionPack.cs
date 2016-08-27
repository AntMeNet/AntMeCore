using System;

namespace AntMe
{
    /// <summary>
    /// Base Interface for all extension packs.
    /// </summary>
    public interface IExtensionPack
    {
        /// <summary>
        /// Returns the name of the extension.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Returns the description of this extension.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Returns the version number of this extension.
        /// </summary>
        Version Version { get; }

        /// <summary>
        /// Returns the name of the Author.
        /// </summary>
        string Author { get; }

        /// <summary>
        /// Called by first load of the extension.
        /// All fragments should be registert in the TypeMapper.
        /// </summary>
        /// <param name="typeMapper">Active Type Mapper</param>
        /// <param name="settings">Basic Settings</param>
        /// <param name="dictionary">Basic Dictionary</param>
        void Load(ITypeMapper typeMapper, KeyValueStore settings, KeyValueStore dictionary);
    }
}
