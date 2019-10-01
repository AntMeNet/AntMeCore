using System;

namespace AntMe.Runtime
{
    [Serializable]
    public class PlayerInfo
    {
        /// <summary>
        ///     Eindeutige ID für genau diese KI
        /// </summary>
        public Guid Guid { get; set; }

        /// <summary>
        ///     Quelle der KI
        /// </summary>
        public PlayerSource Source { get; set; }

        /// <summary>
        ///     Name der KI
        /// </summary>
        public string Name { get; set; }

        // Autor
        public string Author { get; set; }

        /// <summary>
        ///     Typ-Informationen zum Laden der Assembly und des Types.
        /// </summary>
        public TypeInfo Type { get; set; }

        /// <summary>
        ///     Zugrunde liegende Faction.
        /// </summary>
        public string FactionType { get; set; }

        /// <summary>
        ///     Gibt an, ob diese KI static ist.
        /// </summary>
        public bool IsStatic { get; set; }

        public override int GetHashCode()
        {
            return Guid.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is PlayerInfo))
                return false;

            var other = obj as PlayerInfo;
            return Guid.Equals(other.Guid);
        }

        public override string ToString()
        {
            return Name;
        }
    }

    /// <summary>
    ///     List of possible AI-Sources.
    /// </summary>
    [Serializable]
    public enum PlayerSource
    {
        /// <summary>
        ///     Build into an imported extension.
        /// </summary>
        Native = 1,

        /// <summary>
        ///     Imported from external dll.
        /// </summary>
        Imported = 2,

        /// <summary>
        ///     Referenced to an existing Solution file.
        /// </summary>
        Sourcecode = 3,

        /// <summary>
        ///     Referenced to an existing visual Editor file.
        /// </summary>
        Visual = 4,

        /// <summary>
        ///     Exists in the AntMe! Online Storage.
        /// </summary>
        Online = 5
    }
}