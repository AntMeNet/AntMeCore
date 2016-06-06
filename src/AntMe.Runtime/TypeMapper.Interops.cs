using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntMe.Runtime
{
    public partial class TypeMapper
    {
        #region Interop Attachment Properties

        private class FactoryAttchmentTypeMap : AttachmentTypeMap<Func<Faction, FactoryInterop, FactoryInteropProperty>> { }

        private class UnitAttchmentTypeMap : AttachmentTypeMap<Func<UnitInterop, UnitInteropProperty>> { }

        private List<FactoryAttchmentTypeMap> factoryInteropAttachments = new List<FactoryAttchmentTypeMap>();

        private List<UnitAttchmentTypeMap> unitInteropAttachments = new List<UnitAttchmentTypeMap>();

        public IEnumerable<IAttachmentTypeMapperEntry> FactoryInteropAttachments { get { return factoryInteropAttachments; } }

        public IEnumerable<IAttachmentTypeMapperEntry> UnitInteropAttachments { get { return unitInteropAttachments; } }

        /// <summary>
        /// Hängt ein Property an eine gegebene Factory Interop an.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="P"></typeparam>
        /// <param name="extensionPack"></param>
        /// <param name="name"></param>
        /// <param name="createPropertyDelegate"></param>
        public void AttachFactoryInteropProperty<T, P>(IExtensionPack extensionPack, string name,
            Func<Faction, FactoryInterop, P> createPropertyDelegate = null)
            where T : FactoryInterop
            where P : FactoryInteropProperty
        {
            // TODO: Prüfungen
            factoryInteropAttachments.Add(new FactoryAttchmentTypeMap()
            {
                ExtensionPack = extensionPack,
                Name = name,
                Type = typeof(T),
                AttachmentType = typeof(P),
                CreateDelegate = createPropertyDelegate
            });
        }

        /// <summary>
        /// Hängt ein Property an eine gegebene Unit Interop an.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="P"></typeparam>
        /// <param name="extensionPack"></param>
        /// <param name="name"></param>
        /// <param name="createPropertyDelegate"></param>
        public void AttachUnitInteropProperty<T, P>(IExtensionPack extensionPack, string name,
            Func<UnitInterop, P> createPropertyDelegate = null)
            where T : UnitInterop
            where P : UnitInteropProperty
        {
            // TODO: Prüfungen
            unitInteropAttachments.Add(new UnitAttchmentTypeMap()
            {
                ExtensionPack = extensionPack,
                Name = name,
                Type = typeof(T),
                AttachmentType = typeof(P),
                CreateDelegate = createPropertyDelegate
            });
        }

        #endregion

        #region Interop Extender

        private class FactoryExtenderTypeMap : ExtenderTypeMap<Action<FactoryInterop>> { }

        private class UnitExtenderTypeMap : ExtenderTypeMap<Action<UnitInterop>> { }

        private List<FactoryExtenderTypeMap> factoryInteropExtender = new List<FactoryExtenderTypeMap>();

        private List<UnitExtenderTypeMap> unitInteropExtender = new List<UnitExtenderTypeMap>();

        /// <summary>
        /// List of all available Factory Interop Extender.
        /// </summary>
        public IEnumerable<IRankedTypeMapperEntry> FactoryInteropExtender { get { return factoryInteropExtender; } }

        /// <summary>
        /// List of all available Unit Interop Extender.
        /// </summary>
        public IEnumerable<IRankedTypeMapperEntry> UnitInteropExtender { get { return unitInteropExtender; } }

        /// <summary>
        /// Registriert einen Extender für das gegebene Factory Interop
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="extensionPack"></param>
        /// <param name="name"></param>
        /// <param name="rank"></param>
        /// <param name="extenderDelegate"></param>
        public void RegisterFactoryInteropExtender<T>(IExtensionPack extensionPack, string name, int rank, Action<FactoryInterop> extenderDelegate)
            where T : FactoryInterop
        {
            // TODO: Prüfungen
            factoryInteropExtender.Add(new FactoryExtenderTypeMap()
            {
                ExtensionPack = extensionPack,
                Name = name,
                Type = typeof(T),
                Rank = rank,
                ExtenderDelegate = extenderDelegate
            });
        }

        /// <summary>
        /// Registriert einen Extender für das gegebene Unit Interop
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="extensionPack"></param>
        /// <param name="name"></param>
        /// <param name="rank"></param>
        /// <param name="extenderDelegate"></param>
        public void RegisterUnitInteropExtender<T>(IExtensionPack extensionPack, string name, int rank, Action<UnitInterop> extenderDelegate)
            where T : UnitInterop
        {
            // TODO: prüfungen
            unitInteropExtender.Add(new UnitExtenderTypeMap()
            {
                ExtensionPack = extensionPack,
                Name = name,
                Type = typeof(T),
                Rank = rank,
                ExtenderDelegate = extenderDelegate
            });
        }

        #endregion

        #region Interop Resolver

        /// <summary>
        /// Creates a filled Factory Interop (all registered Properties and applied Extender) 
        /// for the given Faction.
        /// </summary>
        /// <param name="faction">Reference to the Faction</param>
        /// <returns>New Instance of a full filled Factory Interop</returns>
        public FactoryInterop CreateFactoryInterop(Faction faction)
        {
            if (faction == null)
                throw new ArgumentNullException("faction");

            // Search for the related Faction Definition
            var factionDefintion = factions.FirstOrDefault(f => f.Type == faction.GetType());
            if (factionDefintion == null)
                throw new ArgumentException("Faction is not registered");

            // Creates a new Instance of the registered Interop Type
            FactoryInterop result = Activator.CreateInstance(factionDefintion.FactoryInteropType, faction) as FactoryInterop;
            if (result == null)
                throw new Exception("Error during Factory Interop Creation.");

            // Creates all Attachment Properties for the selected Interop Type
            foreach (var item in factoryInteropAttachments.Where(a => a.Type == result.GetType()))
            {
                FactoryInteropProperty property = null;
                if (item.CreateDelegate != null)
                {
                    // Create by Delegate
                    property = item.CreateDelegate(faction, result);
                }
                else
                {
                    // Create by Default
                    property = Activator.CreateInstance(item.AttachmentType, faction, result) as FactoryInteropProperty;
                    if (property == null)
                        throw new Exception("Error during Factory Interop Property Creation.");
                }

                // Attach created Property
                if (property != null)
                    result.AddProperty(property);
            }

            // Execute all Extender for the selected Interop Type.
            foreach (var extender in factoryInteropExtender.Where(e => e.Type == result.GetType()).OrderBy(e => e.Rank))
            {
                extender.ExtenderDelegate(result);
            }

            return result;
        }

        /// <summary>
        /// Erzeugt eine neue Instanz eines passenden Unit Interop.
        /// </summary>
        /// <param name="faction"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public UnitInterop CreateUnitInterop(Faction faction, FactionItem item)
        {
            if (faction == null)
                throw new ArgumentNullException("faction");

            // Search for the related Faction Definition
            var factionDefintion = factions.FirstOrDefault(f => f.Type == faction.GetType());
            if (factionDefintion == null)
                throw new ArgumentException("Faction is not registered");

            // Creates a new Instance of the registered Interop Type
            UnitInterop result = Activator.CreateInstance(factionDefintion.UnitInteropType, faction, item) as UnitInterop;
            if (result == null)
                throw new Exception("Error during Factory Interop Creation.");

            // Creates all Attachment Properties for the selected Interop Type
            foreach (var attachment in unitInteropAttachments.Where(a => a.Type == result.GetType()))
            {
                UnitInteropProperty property = null;
                if (attachment.CreateDelegate != null)
                {
                    // Create by Delegate
                    property = attachment.CreateDelegate(result);
                }
                else
                {
                    // Create by Default
                    property = Activator.CreateInstance(attachment.AttachmentType, faction, item, result) as UnitInteropProperty;
                    if (property == null)
                        throw new Exception("Error during Factory Interop Property Creation.");
                }

                // Attach
                if (property != null)
                    result.AddProperty(property);
            }

            // Execute all Extender for the selected Interop Type.
            foreach (var extender in unitInteropExtender.Where(e => e.Type == result.GetType()).OrderBy(e => e.Rank))
            {
                extender.ExtenderDelegate(result);
            }

            return result;
        }

        #endregion
    }
}
