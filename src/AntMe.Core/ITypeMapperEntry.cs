using System;

namespace AntMe
{
    /// <summary>
    ///     Base Interface for all Type Mapper Entries.
    /// </summary>
    public interface ITypeMapperEntry
    {
        /// <summary>
        ///     Name of the Mapping Element.
        /// </summary>
        string Name { get; }

        /// <summary>
        ///     Type of the Mapping Element.
        /// </summary>
        Type Type { get; }

        /// <summary>
        ///     Reference to the Source Extension Pack.
        /// </summary>
        IExtensionPack ExtensionPack { get; }
    }

    /// <summary>
    ///     Basis Info Container mit Ranking.
    /// </summary>
    public interface IRankedTypeMapperEntry : ITypeMapperEntry
    {
        /// <summary>
        ///     Rangposition
        /// </summary>
        int Rank { get; }
    }

    /// <summary>
    ///     Basis Info Container mit State- und Info-Type.
    /// </summary>
    public interface IStateInfoTypeMapperEntry : ITypeMapperEntry
    {
        /// <summary>
        ///     Passender State Type
        /// </summary>
        Type StateType { get; }

        /// <summary>
        ///     Passender Info Type
        /// </summary>
        Type InfoType { get; }
    }

    /// <summary>
    ///     Basis Info Container mit Attachment Type.
    /// </summary>
    public interface IAttachmentTypeMapperEntry : ITypeMapperEntry
    {
        /// <summary>
        ///     Typ des Attachments.
        /// </summary>
        Type AttachmentType { get; }
    }

    /// <summary>
    ///     Info Container für Factions.
    /// </summary>
    public interface ITypeMapperFactionEntry : IStateInfoTypeMapperEntry
    {
        /// <summary>
        ///     Typ der Factory-Klasse.
        /// </summary>
        Type FactoryType { get; }

        /// <summary>
        ///     Type der Unit-Klasse.
        /// </summary>
        Type UnitType { get; }

        /// <summary>
        ///     Typ des Factory Interop Objektes.
        /// </summary>
        Type FactoryInteropType { get; }

        /// <summary>
        ///     Type des Unit Interop Objektes.
        /// </summary>
        Type UnitInteropType { get; }
    }
}