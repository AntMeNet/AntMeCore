using System;

namespace AntMe
{
    /// <summary>
    /// Attribute to name a Group of Units.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class UnitGroupAttribute : Attribute
    {
        /// <summary>
        /// Default Constructor.
        /// </summary>
        /// <param name="name">Name of the Group</param>
        public UnitGroupAttribute(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Name of the Group.
        /// </summary>
        public string Name { get; set; }
    }
}
