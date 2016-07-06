using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AntMe.Generator
{
    internal abstract class BaseParseNode
    {

        public IList<BaseParseNode> ChildNodes { get; private set; }

        protected List<Type> references;
        protected ModpackGenerator generator;
        public WrapType wrapType;

        public BaseParseNode(WrapType wrapType, ModpackGenerator generator)
        {
            ChildNodes = new List<BaseParseNode>();
            references = new List<Type>();
            this.generator = generator;
            this.wrapType = wrapType;
        }

        public abstract MemberDeclarationSyntax[] Generate();

        public abstract KeyValueStore GetLocaKeys();

        public void add(BaseParseNode node)
        {
            ChildNodes.Add(node);
        }

        public List<Type> GetReferences()
        {
            List<Type> returnUsings = references.GetRange(0, references.Count);
            foreach (BaseParseNode node in ChildNodes)
            {
                returnUsings.AddRange(node.GetReferences());
            }

            return returnUsings;
        }

        protected bool CheckLocalizableType(Type type)
        {
            switch (wrapType)
            {
                case WrapType.InfoWrap:
                    if (type.DeclaringType is ItemInfo)
                        return true;
                    break;
                case WrapType.BaseTypeWrap:
                    break;
                case WrapType.BaseClasses:
                    break;
                default:
                    break;
            }


            return false;
        }

        protected bool CheckTypeBalckList(string fullKey)
        {
            return generator.CheckBlackList(fullKey);
        }

        protected static TypeSyntax GetTypeSyntax(string fullName)
        {
            string[] parts = fullName.Split('.');

            //special Types
            switch (fullName)
            {
                case "System.Void":
                    return SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.VoidKeyword));
                default:
                    break;
            }

            if (parts.Length > 1)
            {
                return GenerateQualifedName(parts);
            }
            else if (parts.Length == 1)
            {
                return SyntaxFactory.IdentifierName(fullName);
            }
            throw new Exception("Failed to generate TypeName");


        }

        private static QualifiedNameSyntax GenerateQualifedName(string[] parts)
        {
            if (parts.Length > 2)
            {
                return SyntaxFactory.QualifiedName(GenerateQualifedName(parts.Take(parts.Length - 1).ToArray()), SyntaxFactory.IdentifierName(parts[parts.Length - 1]));
            }
            else
            {
                return SyntaxFactory.QualifiedName(SyntaxFactory.IdentifierName(parts[0]), SyntaxFactory.IdentifierName(parts[1]));
            }
        }





    }

    internal enum WrapType
    {
        InfoWrap,
        BaseTypeWrap,
        BaseClasses
    }
}
