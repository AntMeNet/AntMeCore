using System;

namespace AntMe
{
    /// <summary>
    /// Attribut zum Erweitern von Levels um Slots für bestimmte Factions zu limitieren.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    [Serializable]
    [Obsolete]
    public sealed class FactionFilterAttribute : Attribute
    {
        /// <summary>
        /// Hilfstext für die Anzeige im Frontend.
        /// </summary>
        public string Comment;

        /// <summary>
        /// Type der Faction die für diesen Slot erlaubt ist.
        /// </summary>
        public Type FactionType;

        /// <summary>
        /// ID des Slots für den der Filter gilt.
        /// </summary>
        public int PlayerIndex;
    }
}