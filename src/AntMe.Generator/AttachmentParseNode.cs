using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using System.Reflection;
using System.Text.RegularExpressions;

namespace AntMe.Generator
{
    class AttachmentParseNode : BaseParseNode
    {
        public Type AttachmentType;

        public AttachmentParseNode(Type attachmentType, WrapType wrapType, ModpackGenerator generator) : base(wrapType, generator)
        {
            AttachmentType = attachmentType;
        }

        public override MemberDeclarationSyntax[] Generate()
        {

            switch (wrapType)
            {
                case WrapType.InfoWrap:

                    List<MemberDeclarationSyntax> result = new List<MemberDeclarationSyntax>();

                    //Adding field for the orginal attachment-instance
                    result.Add(SyntaxFactory.FieldDeclaration(
                        SyntaxFactory.VariableDeclaration(GetTypeSyntax(AttachmentType.FullName)).AddVariables(
                            SyntaxFactory.VariableDeclarator(SyntaxFactory.Identifier("_" + AttachmentType.Name)))).AddModifiers(
                        SyntaxFactory.Token(SyntaxKind.PrivateKeyword)));

                    //Adding bool property for available requests
                    result.Add(SyntaxFactory.PropertyDeclaration(
                        SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.BoolKeyword)),
                        SyntaxFactory.Identifier("IsLoc" + AttachmentType.Name)).AddModifiers(
                        SyntaxFactory.Token(SyntaxKind.PublicKeyword)).AddAccessorListAccessors(
                        SyntaxFactory.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration,
                        SyntaxFactory.Block(
                            SyntaxFactory.IfStatement(
                                SyntaxFactory.BinaryExpression(
                                    SyntaxKind.NotEqualsExpression,
                                    SyntaxFactory.IdentifierName("_" + AttachmentType.Name),
                                    SyntaxFactory.LiteralExpression(
                                        SyntaxKind.NullLiteralExpression)),
                                SyntaxFactory.ReturnStatement(
                                    SyntaxFactory.LiteralExpression(
                                        SyntaxKind.TrueLiteralExpression))),
                            SyntaxFactory.ReturnStatement(
                                SyntaxFactory.LiteralExpression(
                                    SyntaxKind.FalseLiteralExpression))))));

                    //adding Methods
                    foreach (MethodInfo methodInfo in AttachmentType.GetMethods(BindingFlags.Instance | BindingFlags.Public))
                    {
                        if (methodInfo.IsSpecialName || CheckTypeBalckList(string.Format("{0}:{1}", methodInfo.ReflectedType.FullName, methodInfo.Name)))
                            continue;

                        MethodDeclarationSyntax method = SyntaxFactory.MethodDeclaration(
                            GetTypeSyntax(methodInfo.ReturnType.FullName),
                            "Loc" + methodInfo.Name).AddModifiers(
                            SyntaxFactory.Token(
                                SyntaxKind.PublicKeyword));

                        ArgumentListSyntax arguments = SyntaxFactory.ArgumentList();

                        foreach (ParameterInfo parameterInfo in methodInfo.GetParameters())
                        {
                            method = method.AddParameterListParameters(
                                SyntaxFactory.Parameter(
                                    SyntaxFactory.Identifier("Loc" + parameterInfo.Name)).WithType(
                                    GetTypeSyntax(parameterInfo.ParameterType.FullName)));

                            //adding argument to argumentlist. for later use.
                            arguments = arguments.AddArguments(
                                SyntaxFactory.Argument(
                                    SyntaxFactory.IdentifierName("Loc" + parameterInfo.Name)));
                        }


                        method = method.AddBodyStatements(
                            SyntaxFactory.Block(
                                SyntaxFactory.IfStatement(
                                    SyntaxFactory.BinaryExpression(
                                        SyntaxKind.NotEqualsExpression,
                                        SyntaxFactory.IdentifierName("_" + AttachmentType.Name),
                                        SyntaxFactory.LiteralExpression(SyntaxKind.NullLiteralExpression)),
                                    SyntaxFactory.ReturnStatement(
                                        SyntaxFactory.InvocationExpression(
                                            SyntaxFactory.MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                SyntaxFactory.IdentifierName("_" + AttachmentType.Name),
                                                SyntaxFactory.IdentifierName(methodInfo.Name))).WithArgumentList(arguments))),
                                SyntaxFactory.ReturnStatement(
                                    SyntaxFactory.DefaultExpression(
                                        SyntaxFactory.IdentifierName(methodInfo.ReturnType.FullName)))));

                        result.Add(method);
                    }



                    return result.ToArray();
                    break;
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
                    break;
                case WrapType.BaseTypeWrap:
                    break;
                case WrapType.BaseClasses:
                    break;
                default:
                    break;
            }

            return result;
        }
    }
}


