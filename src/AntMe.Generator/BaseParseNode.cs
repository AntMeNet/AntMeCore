using Microsoft.CodeAnalysis;
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

        public BaseParseNode()
        {
            ChildNodes = new List<BaseParseNode>();
        }

        public abstract MemberDeclarationSyntax Generate();


    }
}
