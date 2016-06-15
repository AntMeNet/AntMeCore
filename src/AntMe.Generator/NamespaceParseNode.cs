using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AntMe.Generator
{
    class NamespaceParseNode : BaseParseNode
    {

        public string Name { get; private set; }

        public NamespaceParseNode(string name, WrapType wrapType)
            : base(wrapType)
        {
            Name = name;
        }

        public override MemberDeclarationSyntax Generate()
        {
            return SyntaxFactory.NamespaceDeclaration(
                SyntaxFactory.IdentifierName(Name)).AddMembers(
                ChildNodes.Select(c => c.Generate()).ToArray());
        }

    }
}
