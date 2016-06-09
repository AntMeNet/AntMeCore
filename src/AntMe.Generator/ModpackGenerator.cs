using AntMe.Runtime;
using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.IO;
using Microsoft.CodeAnalysis;
using System.Linq;

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
            string outputFile = "Summary.dll";

            string frameworkRoot = @"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5.1\{0}.dll";
            List<MetadataReference> references = new List<MetadataReference>();
            references.Add(MetadataReference.CreateFromFile(string.Format(frameworkRoot, "mscorlib")));
            references.Add(MetadataReference.CreateFromFile(string.Format(frameworkRoot, "System")));
            references.Add(MetadataReference.CreateFromFile(string.Format(frameworkRoot, "System.Core")));

            CSharpCompilationOptions options =
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
                    .WithOverflowChecks(true).WithOptimizationLevel(OptimizationLevel.Release);

            List<SyntaxTree> syntaxTrees = new List<SyntaxTree>();

            BaseParseNode root = new NamespaceParseNode("AntMe.Deutsch");

            foreach (var item in ExtensionLoader.DefaultTypeMapper.Items.Where(i => i.InfoType != null).Select(i => i.InfoType).Distinct())
            {
                ClassParseNode classParseNode = new ClassParseNode(item);
                root.ChildNodes.Add(classParseNode);

                foreach (ConstructorInfo constructor in item.GetConstructors())
                {
                    classParseNode.ChildNodes.Add(new ConstructorParseNode(constructor));
                }

                foreach (MethodInfo methodInfo in item.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly ))
                {
                    //classParseNode.ChildNodes.Add(new MethodParseNode(methodInfo));
                }
            }

            List<Type> RootReferences = root.GetReferences();

            foreach (string codebase in RootReferences.Select(c => c.Assembly.CodeBase).Distinct())
            {
                references.Add(MetadataReference.CreateFromFile(codebase.Remove(0,8)));
            }

            syntaxTrees.Add(SyntaxFactory.SyntaxTree(SyntaxFactory.CompilationUnit().AddMembers(root.Generate())));
            var compilation = CSharpCompilation.Create(outputFile, syntaxTrees, references, options);
            var result = compilation.Emit(Path.Combine(output, outputFile));

            if (!result.Success)
                throw new Exception(string.Join(Environment.NewLine,result.Diagnostics.Select(d => d.ToString())));
            
            return Path.Combine(output, outputFile);
        }

        private static SyntaxTree GenerateItem(Type item)
        {


            return SyntaxFactory.SyntaxTree(
                SyntaxFactory.CompilationUnit().AddMembers(
                    SyntaxFactory.NamespaceDeclaration(
                        SyntaxFactory.IdentifierName("AntMe.Deutsch")).AddMembers(
                            SyntaxFactory.ClassDeclaration(item.Name).WithModifiers(
                                SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.PublicKeyword))).AddMembers(
                                    generateLinkedMethods(item)
                                ))).AddUsings(

                                ));

        }

        #region Methods

        private static MemberDeclarationSyntax[] generateLinkedMethods(Type item)
        {
            List<MemberDeclarationSyntax> members = new List<MemberDeclarationSyntax>();

            foreach (MethodInfo methodInfo in item.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public))
            {
                members.Add(SyntaxFactory.MethodDeclaration(
                    generateMethodTypeSyntax(methodInfo),
                    methodInfo.Name).WithModifiers(
                        SyntaxFactory.TokenList(
                            SyntaxFactory.Token(SyntaxKind.PublicKeyword))).WithBody(
                                SyntaxFactory.Block(
                                    SyntaxFactory.ReturnStatement(
                                        SyntaxFactory.LiteralExpression(
                                            SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal(item.Name)
                                            )))));
            }

            return members.ToArray();
        }

        private static TypeSyntax generateMethodTypeSyntax(MethodInfo info)
        {
            // returntype
            SyntaxKind syntaxKind;
            if (info.ReturnType == typeof(string))
                syntaxKind = SyntaxKind.StringKeyword;
            else if (info.ReturnType == typeof(bool))
                syntaxKind = SyntaxKind.BoolKeyword;
            else if (info.ReturnType == typeof(byte))
                syntaxKind = SyntaxKind.ByteKeyword;
            else if (info.ReturnType == typeof(char))
                syntaxKind = SyntaxKind.CharKeyword;
            else if (info.ReturnType == typeof(decimal))
                syntaxKind = SyntaxKind.DecimalKeyword;
            else if (info.ReturnType == typeof(double))
                syntaxKind = SyntaxKind.DoubleKeyword;
            else if (info.ReturnType == typeof(float))
                syntaxKind = SyntaxKind.FloatKeyword;
            else if (info.ReturnType == typeof(long))
                syntaxKind = SyntaxKind.LongKeyword;
            else if (info.ReturnType == typeof(object))
                syntaxKind = SyntaxKind.ObjectKeyword;
            else if (info.ReturnType == typeof(sbyte))
                syntaxKind = SyntaxKind.SByteKeyword;
            else if (info.ReturnType == typeof(short))
                syntaxKind = SyntaxKind.ShortKeyword;
            else if (info.ReturnType == typeof(uint))
                syntaxKind = SyntaxKind.UIntKeyword;
            else if (info.ReturnType == typeof(ulong))
                syntaxKind = SyntaxKind.ULongKeyword;
            else if (info.ReturnType == typeof(ushort))
                syntaxKind = SyntaxKind.UShortKeyword;
            else
                return SyntaxFactory.IdentifierName(info.ReturnType.Name);
            return SyntaxFactory.PredefinedType(SyntaxFactory.Token(syntaxKind));
        }



        #endregion

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
