using System;

namespace AntMe
{
    /// <summary>
    /// Attribute to specify allowed Factions for the Level Slots.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public sealed class FactionFilterAttribute : Attribute
    {
        /// <summary>
        /// Short Description / Reason
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Allowed Faction type.
        /// </summary>
        public Type FactionType { get; set; }

        /// <summary>
        /// Affected Slot Id.
        /// </summary>
        public byte SlotIndex { get; set; }
    }
}