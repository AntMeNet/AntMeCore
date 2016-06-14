using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using System.Reflection;
using Microsoft.CodeAnalysis;

namespace AntMe.Generator
{
    class ConstructorParseNode : BaseParseNode
    {

        public ConstructorInfo ConstructorInfo { get; private set; }

        public ConstructorParseNode(ConstructorInfo constructorInfo, WrapType wrapType)
            : base(wrapType)
        {
            ConstructorInfo = constructorInfo;
        }

        public override MemberDeclarationSyntax Generate()
        {
            switch (wrapType)
            {
                case WrapType.InfoWrap:
                    return null;
                case WrapType.BaseTypeWrap:
                    return null;
                case WrapType.BaseClasses:
                    return null;
                default:
                    return null;
            }

        }
    }
}
