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
            // Engine Properties
            foreach (var item in enginePropertyContainer.Where(c => c.ExtensionPack == extensionPack).ToArray())
                enginePropertyContainer.Remove(item);

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

        #region Engine Properties

        private class EnginePropertyContainer : ExtenderTypeMap<Func<Engine, EngineProperty>> { }

        private readonly List<EnginePropertyContainer> enginePropertyContainer =
            new List<EnginePropertyContainer>();

        /// <summary>
        /// Listet alle registrierten Extensions auf.
        /// </summary>
        public IEnumerable<IRankedTypeMapperEntry> EngineProperties
        {
            get
            {
                tracer.Trace(TraceEventType.Information, 17, "Requested Engine Property List");
                return enginePropertyContainer.OrderBy(e => e.Rank);
            }
        }

        /// <summary>
        /// Registriert eine Engine Extension. Die Extension braucht entweder einen Konstruktor mit 
        /// den Parametern (Engine) oder einen Create Delegaten.
        /// </summary>
        /// <param name="extensionPack">Referenz auf den verantwortlichen Extension Pack</param>
        /// <param name="name">Name der Extension</param>
        /// <param name="rank">Rang der Extension</param>
        /// <param name="createExtensionDelegate">Delegat zum Erstellen einer neuen Instanz</param>
        /// <typeparam name="T">Extension Type</typeparam>
        public void RegisterEngineProperty<T>(IExtensionPack extensionPack, string name, int rank, Func<Engine, T> createExtensionDelegate = null) where T : EngineProperty
        {
            Type t = typeof(T);

            tracer.Trace(TraceEventType.Information, 3, "Try to register new Extension '{0}' ({1})", name, t.FullName);

            // Extension Pack darf nicht null sein.
            if (extensionPack == null)
            {
                tracer.Trace(TraceEventType.Critical, 4, "Extension Pack is null ({0})", t.FullName);
                throw new ArgumentNullException("extensionPack");
            }

            // Existenz eines Namens sicher stellen
            if (string.IsNullOrEmpty(name))
            {
                tracer.Trace(TraceEventType.Critical, 4, "Extension Name is not set ({0})", t.FullName);
                throw new ArgumentNullException("name");
            }

            // Instaniierbarkeit prüfen
            if (t.IsAbstract)
            {
                tracer.Trace(TraceEventType.Critical, 5, "Extension is abstract '{0}' ({1})", name, t.FullName);
                throw new NotSupportedException("Extension is abstract");
            }

            // Collision checken
            if (enginePropertyContainer.Any(e => e.Type == t))
            {
                tracer.Trace(TraceEventType.Critical, 6, "Extension is already registered '{0}' ({1})", name, t.FullName);
                throw new ArgumentException("This Extension is already registered");
            }

            if (createExtensionDelegate == null)
            {
                tracer.Trace(TraceEventType.Information, 7, "Extension contains no CreateDelegate");

                // Der Standard-Prozess erwartet einen mit der Struktur ctor(Engine)
                if (t.GetConstructor(new[] { typeof(Engine) }) == null)
                {
                    tracer.Trace(TraceEventType.Critical, 9, "Extension has no empty Constructor '{0}' ({1})", name, t.FullName);
                    throw new NotSupportedException(string.Format("Extension has no empty Constructor '{0}' ({1})", name, t.FullName));
                }
            }
            else
            {
                tracer.Trace(TraceEventType.Information, 8, "Extension contains CreateDelegate");
            }

            EnginePropertyContainer container = new EnginePropertyContainer()
            {
                ExtensionPack = extensionPack,
                Name = name,
                Rank = rank,
                Type = t,
                ExtenderDelegate = createExtensionDelegate
            };

            tracer.Trace(TraceEventType.Information, 2, "Register Engine Extension succeeded '{0}' ({1})", name, t.FullName);

            enginePropertyContainer.Add(container);
        }

        #endregion

        #region Engine Resolver

        /// <summary>
        /// Ermittelt alle registrierten Extensions für die übergebene Engine.
        /// </summary>
        /// <param name="engine">Referenz auf die neue Engine</param>
        public void ResolveEngine(Engine engine)
        {
            tracer.Trace(TraceEventType.Information, 10, "Apply Extensions to a new Engine");

            // Engine muss gesetzt sein
            if (engine == null)
            {
                tracer.Trace(TraceEventType.Critical, 11, "No Engine was set");
                throw new ArgumentNullException("engine");
            }

            var extensions = enginePropertyContainer.OrderBy(e => e.Rank).ToArray();
            for (int i = 0; i < extensions.Length; i++)
            {
                EnginePropertyContainer item = extensions[i];

                tracer.Trace(TraceEventType.Information, 12, "Try to apply Extension {0}", item.Name);

                EngineProperty property = null;
                if (item.ExtenderDelegate != null)
                {
                    // Benutze CreateDelegate
                    tracer.Trace(TraceEventType.Information, 13, "Use Delegate");
                    property = item.ExtenderDelegate(engine);
                }
                else
                {
                    // Automatische Instanz
                    tracer.Trace(TraceEventType.Information, 14, "Use Activator");
                    property = Activator.CreateInstance(item.Type, engine) as EngineProperty;
                }

                // Existenz prüfen
                if (property == null)
                {
                    tracer.Trace(TraceEventType.Critical, 15, "Could not create Extension '{0}'", item.Name);
                    throw new NullReferenceException(
                        string.Format("Es konnte keine Instanz der Engine Extension {0} erstellt werden.", item.Name));
                }

                engine.AddProperty(property);

                tracer.Trace(TraceEventType.Information, 13, "Apply Extension Successful {0}", item.Name);
            }
        }

        #endregion
    }
}
