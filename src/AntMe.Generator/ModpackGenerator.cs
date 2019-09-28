using System;
using System.Collections.Generic;
using System.Reflection;
using AntMe.Runtime;

namespace AntMe.Generator
{
    /// <summary>
    /// Code Generator to build a localization- and summarization-Assembly.
    /// </summary>
    public static class ModpackGenerator
    {
        /// <summary>
        /// Generates the Summary-Assembly.
        /// </summary>
        /// <param name="paths">Import Pathes for Extensions</param>
        /// <param name="output">Output Path for the File</param>
        /// <param name="token">Optional Progress Token</param>
        /// <returns>Filename</returns>
        public static string Generate(string[] paths, string output, ProgressToken token)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static Dictionary<Type, List<string>> GetLocaKeys()
        {
            Dictionary<Type, List<string>> dictionary = new Dictionary<Type, List<string>>();

            // Collect all Item Infos
            foreach (var item in ExtensionLoader.DefaultTypeMapper.Items)
            {
                if (item.InfoType == null) continue;
                AnalyseType<ItemInfo>(item.InfoType, dictionary);
            }

            // Collect all Item Property Infos
            foreach (var item in ExtensionLoader.DefaultTypeMapper.ItemProperties)
            {
                if (item.InfoType == null) continue;
                AnalyseType<ItemInfoProperty>(item.InfoType, dictionary);
            }

            // Collection Faction Stuff
            foreach (var item in ExtensionLoader.DefaultTypeMapper.Factions)
            {
                if (item.InfoType == null) continue;

                AnalyseType<FactionInfo>(item.InfoType, dictionary);
                AnalyseType<FactionFactory>(item.FactoryType, dictionary);

                // TODO: Ingnoriere Vererbungsstack
                AnalyseType<FactoryInterop>(item.FactoryInteropType, dictionary);

                AnalyseType<FactionUnit>(item.UnitType, dictionary);

                // TODO: Ingnoriere Vererbungsstack
                AnalyseType<UnitInterop>(item.UnitInteropType, dictionary);
            }

            foreach (var item in ExtensionLoader.DefaultTypeMapper.FactoryInteropAttachments)
            {
                // TODO: Ingnoriere Vererbungsstack
                AnalyseType<FactoryInteropProperty>(item.AttachmentType, dictionary);
            }

            foreach (var item in ExtensionLoader.DefaultTypeMapper.UnitInteropAttachments)
            {
                // TODO: Ingnoriere Vererbungsstack
                AnalyseType<UnitInteropProperty>(item.AttachmentType, dictionary);
            }


            // Collect all Factory Interops
            foreach (var item in ExtensionLoader.DefaultTypeMapper.FactionProperties)
            {
                if (item.InfoType == null) continue;
                AnalyseType<FactionInfoProperty>(item.InfoType, dictionary);
            }

            return dictionary;
        }

        private static void AnalyseType<T>(Type type, Dictionary<Type, List<string>> dict)
        {
            Type t = type;
            AnalyseType(t, dict);

            while (t != typeof(T) && t != null)
            {
                t = t.BaseType;
                AnalyseType(t, dict);
            }
        }

        private static void AnalyseType(Type type, Dictionary<Type, List<string>> dict)
        {
            if (dict.ContainsKey(type)) return;

            if (type.IsGenericType) return;

            List<string> result = new List<string>();
            dict.Add(type, result);

            // Name
            string name = type.Name;
            if (!result.Contains(type.Name))
                result.Add(type.Name);

            // Enum
            if (type.IsEnum)
                foreach (var value in Enum.GetNames(type))
                    result.Add(value);

            // Methods / Parameter
            foreach (var method in type.GetMethods(BindingFlags.Instance | BindingFlags.Public))
            {
                if (method.IsSpecialName || method.IsAbstract || method.IsVirtual)
                    continue;

                if (method.DeclaringType != type)
                    continue;

                if (!result.Contains(method.Name))
                    result.Add(method.Name);

                foreach (var parameter in method.GetParameters())
                {
                    if (parameter.ParameterType.FullName.StartsWith("AntMe."))
                        AnalyseType(parameter.ParameterType, dict);

                    if (!result.Contains(parameter.Name))
                        result.Add(parameter.Name);
                }
            }

            // Properties
            foreach (var property in type.GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                if (property.DeclaringType != type)
                    continue;

                if (property.PropertyType.FullName.StartsWith("AntMe."))
                    AnalyseType(property.PropertyType, dict);

                if (!result.Contains(property.Name))
                    result.Add(property.Name);
            }

            // Events
            foreach (var e in type.GetEvents(BindingFlags.Instance | BindingFlags.Public))
            {
                // TODO: Check Parameter

                if (!result.Contains(e.Name))
                    result.Add(e.Name);
            }
        }
    }
}
