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

        public MethodParseNode(MethodInfo methodeInfo, WrapType wrapType, ModpackGenerator generator) : base(wrapType, generator)
        {
            MethodInfo = methodeInfo;
        }

        public override MemberDeclarationSyntax[] Generate()
        {
            MethodDeclarationSyntax Method;
            switch (wrapType)
            {
                case WrapType.InfoWrap:

                    Method = SyntaxFactory.MethodDeclaration(
                        GetTypeSyntax(MethodInfo.ReturnType.FullName),
                        GetLocalization(MethodInfo.DeclaringType, MethodInfo.Name)).AddModifiers(
                        SyntaxFactory.Token(
                            SyntaxKind.PublicKeyword));

                    if (MethodInfo.IsVirtual)
                    {
                        Type baseType = MethodInfo.GetBaseDefinition().DeclaringType;

                        if (baseType is ItemInfo || !(baseType is PropertyList<ItemInfoProperty>))
                        {
                            Method = SyntaxFactory.MethodDeclaration(
                                GetTypeSyntax(GetLocalization(MethodInfo.ReturnType)),
                                GetLocalization(MethodInfo.DeclaringType, MethodInfo.Name)).AddModifiers(
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
                                SyntaxFactory.Identifier(GetLocalization(MethodInfo.DeclaringType, parameterInfo.Name))).WithType(
                                GetTypeSyntax(GetLocalization(parameterInfo.ParameterType))));

                        // adding Arguments to argumentslist, for later use.
                        arguments = arguments.AddArguments(GetLocaArgument(MethodInfo.DeclaringType, parameterInfo));
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
                        if (isPrimitiveType(MethodInfo.ReturnType))
                        {
                            Method = Method.WithBody(
                                SyntaxFactory.Block(
                                    SyntaxFactory.ReturnStatement(invocation)));
                        }
                        else
                        {
                            Method = Method.WithBody(
                                SyntaxFactory.Block(
                                    SyntaxFactory.ReturnStatement(
                                        SyntaxFactory.ObjectCreationExpression(
                                            SyntaxFactory.IdentifierName("Loc" + MethodInfo.ReturnType.Name)).AddArgumentListArguments(
                                            SyntaxFactory.Argument(invocation)))));
                        }
                    }

                    return new MemberDeclarationSyntax[] { Method };
                case WrapType.BaseTypeWrap:
                    break;
                case WrapType.BaseClasses:
                    break;
                default:
                    break;
            }
            return new MemberDeclarationSyntax[] { };
        }

        public override KeyValueStore GetLocaKeys()
        {

            KeyValueStore result = new KeyValueStore();

            switch (wrapType)
            {
                case WrapType.InfoWrap:
                    result.Set(MethodInfo.DeclaringType, MethodInfo.Name, string.Format("TO_LOC_{0}", MethodInfo.Name));

                    foreach (ParameterInfo parameter in MethodInfo.GetParameters())
                    {
                        result.Set(MethodInfo.DeclaringType, parameter.Name, string.Format("TO_LOC_{0}", parameter.Name));
                        if (CheckLocalizableType(parameter.ParameterType))
                            result.Set(parameter.ParameterType, parameter.ParameterType.Name, string.Format("TO_LOC_{0}", parameter.ParameterType.Name));
                    }

                    if (CheckLocalizableType(MethodInfo.ReturnType))
                        result.Set(MethodInfo.ReturnType, MethodInfo.ReturnType.Name, string.Format("TO_LOC_{0}", MethodInfo.ReturnType.Name));
                    break;
                case WrapType.BaseTypeWrap:
                    break;
                case WrapType.BaseClasses:
                    break;
                default:
                    break;
            }

            foreach (BaseParseNode node in ChildNodes)
            {
                result.Merge(node.GetLocaKeys());
            }

            return result;
        }

    }
}
