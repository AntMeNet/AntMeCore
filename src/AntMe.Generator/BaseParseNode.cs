using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntMe.Generator
{
    internal abstract class BaseParseNode
    {

        public IList<BaseParseNode> ChildNodes { get; private set; }

        internal List<Type> references;

        protected WrapType wrapType;

        public BaseParseNode(WrapType wrapType)
        {
            this.wrapType = wrapType;
            ChildNodes = new List<BaseParseNode>();
            references = new List<Type>();
        }

        public abstract MemberDeclarationSyntax Generate();

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

        protected static TypeSyntax GetTypeSyntax(string fullName)
        {
            string[] parts = fullName.Split('.');

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
