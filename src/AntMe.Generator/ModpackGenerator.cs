using AntMe.Runtime;
using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.IO;
using Microsoft.CodeAnalysis;
using System.Linq;
using Microsoft.CodeAnalysis.Text;

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
        public static string Generate(string culture, string output, ProgressToken token)
        {
            string outputFile = "Summary.dll_";


            if (!ExtensionLoader.LocalizedLanguages.Contains(culture))
                throw new ArgumentException("Language not available.");

            List<Type> typeReferences = new List<Type>();
             
            CSharpCompilationOptions options =
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
                    .WithOverflowChecks(true).WithOptimizationLevel(OptimizationLevel.Release);


            List<SyntaxTree> syntaxTrees = new List<SyntaxTree>();
            List<BaseParseNode> parseNodeTrees = GenerateParseNodeTrees(culture);

            foreach (BaseParseNode node in parseNodeTrees)
            {
                typeReferences.AddRange(node.GetReferences());
                syntaxTrees.Add(SyntaxFactory.SyntaxTree(SyntaxFactory.CompilationUnit().AddMembers(node.Generate())));
            }
            
            List<MetadataReference> references = new List<MetadataReference>();

            foreach (string location in typeReferences.Select(c => c.Assembly.Location).Distinct())
            {
                references.Add(MetadataReference.CreateFromFile(location));
            }
            
            StreamWriter streamWriter = new StreamWriter(File.Open(Path.Combine(output, "Assembly.cs"), FileMode.Create));
            syntaxTrees[0].GetRoot().NormalizeWhitespace().GetText().Write(streamWriter);
            streamWriter.Flush();
            streamWriter.Close();

            var compilation = CSharpCompilation.Create(outputFile, syntaxTrees, references, options);

            var result = compilation.Emit(Path.Combine(output, outputFile));

            if (!result.Success)
                throw new Exception(string.Join(Environment.NewLine, result.Diagnostics.Select(d => d.ToString())));

            return Path.Combine(output, outputFile);
        }

        private static List<BaseParseNode> GenerateParseNodeTrees(string culture)
        {
            List<BaseParseNode> trees = new List<BaseParseNode>();
            KeyValueStore Translations = ExtensionLoader.GetDictionary(culture);

            #region Info-Wrapper

            BaseParseNode root = new NamespaceParseNode("AntMe.Deutsch", WrapType.InfoWrap,Translations);
            List<Type> wrapped = new List<Type>();

            // Collect all Item Infos and link them to the Localized ItemInfos
            foreach (var item in ExtensionLoader.DefaultTypeMapper.Items)
            {
                if (item.InfoType == null) continue;
                Type t = item.InfoType;

                while (t != typeof(PropertyList<ItemInfoProperty>) && t != null && !wrapped.Contains(t))
                {

                    ClassParseNode classNode = new ClassParseNode(t, WrapType.InfoWrap, Translations);
                    wrapped.Add(t);
                    root.add(classNode);

                    foreach (MethodInfo methodInfo in t.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly))
                    {
                        if (methodInfo.IsSpecialName)
                            continue;
                        classNode.add(new MethodParseNode(methodInfo, WrapType.InfoWrap, Translations));
                    }

                    foreach (PropertyInfo propertyInfo in t.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly))
                    {
                        classNode.add(new PropertyParseNode(propertyInfo, WrapType.InfoWrap, Translations));
                    }

                    t = t.BaseType;
                }
            }

            trees.Add(root);
            #endregion

            return trees;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static KeyValueStore GenerateLocaKeys()
        {
            KeyValueStore result = new KeyValueStore();
            foreach (BaseParseNode node in GenerateParseNodeTrees("en"))
            {
                result.Merge(node.GetLocaKeys());
            }

            return result;


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
