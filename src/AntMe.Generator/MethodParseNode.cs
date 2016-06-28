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

        public MethodParseNode(MethodInfo methodeInfo, WrapType wrapType)
            : base(wrapType)
        {
            MethodInfo = methodeInfo;
        }

        public override MemberDeclarationSyntax Generate()
        {

            MethodDeclarationSyntax Method;

            switch (wrapType)
            {
                case WrapType.InfoWrap:

                    Method = SyntaxFactory.MethodDeclaration(
                        GetTypeSyntax(MethodInfo.ReturnType.FullName),
                        "Loc" + MethodInfo.Name).AddModifiers(
                        SyntaxFactory.Token(
                            SyntaxKind.PublicKeyword));

                    if (MethodInfo.IsVirtual)
                    {
                         Type baseType = MethodInfo.GetBaseDefinition().DeclaringType;

                        if (baseType is ItemInfo || !(baseType is PropertyList<ItemInfoProperty>))
                        {
                            Method = SyntaxFactory.MethodDeclaration(
                                GetTypeSyntax(MethodInfo.ReturnType.FullName),
                                MethodInfo.Name).AddModifiers(
                                SyntaxFactory.Token(
                                    SyntaxKind.PublicKeyword)).AddModifiers(
                                        SyntaxFactory.Token(
                                            SyntaxKind.OverrideKeyword));
                        }
                    }
                    ArgumentListSyntax arguments = SyntaxFactory.ArgumentList();

                    foreach (ParameterInfo parameterInfo in MethodInfo.GetParameters())
                    {
                        Method = Method.AddParameterListParameters(
                            SyntaxFactory.Parameter(
                                SyntaxFactory.Identifier("Loc" + parameterInfo.Name)).WithType(
                                GetTypeSyntax(parameterInfo.ParameterType.FullName)));

                        // adding Arguments to argumentsList, for later use.
                        arguments = arguments.AddArguments(
                            SyntaxFactory.Argument(
                                SyntaxFactory.IdentifierName("Loc" + parameterInfo.Name)));
                    }

                    InvocationExpressionSyntax invocation =
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName("_" + MethodInfo.DeclaringType.Name),
                                SyntaxFactory.IdentifierName(MethodInfo.Name))).WithArgumentList(arguments);

                    if (MethodInfo.ReturnType == typeof(void))
                    {

                        Method = Method.WithBody(
                            SyntaxFactory.Block(
                            SyntaxFactory.ExpressionStatement(invocation)));
                    }
                    else
                    {
                        Method = Method.WithBody(
                            SyntaxFactory.Block(
                                SyntaxFactory.ReturnStatement(invocation)));
                    }



                    return Method;
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
