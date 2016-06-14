using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Reflection;
using Microsoft.CodeAnalysis.CSharp;

namespace AntMe.Generator
{
    class MethodParseNode : BaseParseNode
    {

        public MethodInfo MethodInfo { get; private set; }

        public MethodParseNode(MethodInfo methodeInfo,WrapType wrapType)
            : base(wrapType)
        {
            MethodInfo = methodeInfo;
        }

        public override MemberDeclarationSyntax Generate()
        {
            switch (wrapType)
            {
                case WrapType.InfoWrap:
                    return SyntaxFactory.MethodDeclaration(
                            SyntaxFactory.PredefinedType(
                                SyntaxFactory.Token(SyntaxKind.VoidKeyword)),
                            MethodInfo.Name).WithBody(
                            SyntaxFactory.Block());
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
