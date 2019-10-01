using System;

namespace AntMe
{
    /// <summary>
    ///     Base Attribute to decorate a Factory Class of any kind of Faction. This is required
    ///     to add the Name and the Autor of a player to the related factory class.
    ///     Inherited Attribute should use <see cref="FactoryAttributeMappingAttribute" /> to map
    ///     additional Properties to Name and Author.
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public abstract class FactoryAttribute : Attribute
    {
        /// <summary>
        ///     Indicates if the Player is related to a story level and therefore shouldn't be
        ///     displayed in the player selection.
        /// </summary>
        public bool Hidden { get; set; }
    }
}