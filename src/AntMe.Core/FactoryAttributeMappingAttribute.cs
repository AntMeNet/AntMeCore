using System;

namespace AntMe
{
    /// <summary>
    ///     Attribute to map additional Name- and Author-Properties of inherited Attributes of the
    ///     <see cref="FactoryAttribute" />.
    ///     This is required for localized Versions of the <see cref="PlayerAttribute" />.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class FactoryAttributeMappingAttribute : Attribute
    {
        /// <summary>
        ///     Defines the name of the Property which holds the Display-Name of the Ai.
        /// </summary>
        public string NameProperty { get; set; }

        /// <summary>
        ///     Defines the name of the Property which holds the Player Name.
        /// </summary>
        public string AuthorProperty { get; set; }
    }
}