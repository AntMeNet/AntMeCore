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


    }

    internal enum WrapType
    {
        InfoWrap,
        BaseTypeWrap,
        BaseClasses
    }
}
