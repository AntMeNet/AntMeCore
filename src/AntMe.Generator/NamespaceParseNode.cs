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

        public NamespaceParseNode(string name, WrapType wrapType, KeyValueStore locaDictionary, string[] blackList) : base(wrapType, locaDictionary, blackList)
        {
            Name = name;
        }

        public override MemberDeclarationSyntax[] Generate()
        {
            return new MemberDeclarationSyntax[] { SyntaxFactory.NamespaceDeclaration(
                SyntaxFactory.IdentifierName(Name)).AddMembers(
                ChildNodes.SelectMany(c => c.Generate()).ToArray()) };
        }

        public override KeyValueStore GetLocaKeys()
        {
            KeyValueStore result = new KeyValueStore();

            foreach (BaseParseNode node in ChildNodes)
            {
                result.Merge(node.GetLocaKeys());
            }

            return result;
        }
    }
}
