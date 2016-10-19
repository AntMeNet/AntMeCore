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
                        SyntaxFactory.Identifier("Is" + GetLocalization(AttachmentType))).AddModifiers(
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
                        if (methodInfo.IsSpecialName || methodInfo.IsGenericMethod || CheckTypeBalckList(string.Format("{0}:{1}", methodInfo.ReflectedType.FullName, methodInfo.Name)))
                            continue;

                        MethodDeclarationSyntax method = SyntaxFactory.MethodDeclaration(
                            GetTypeSyntax(methodInfo.ReturnType.FullName),
                            GetLocalization(methodInfo.DeclaringType, methodInfo.Name)).AddModifiers(
                            SyntaxFactory.Token(
                                SyntaxKind.PublicKeyword));

                        ArgumentListSyntax arguments = SyntaxFactory.ArgumentList();

                        foreach (ParameterInfo parameterInfo in methodInfo.GetParameters())
                        {
                            method = method.AddParameterListParameters(
                                SyntaxFactory.Parameter(
                                    SyntaxFactory.Identifier(GetLocalization(methodInfo.DeclaringType, parameterInfo.Name))).WithType(
                                    GetTypeSyntax(parameterInfo.ParameterType.FullName)));

                            //adding argument to argumentlist. for later use.
                            arguments = arguments.AddArguments(
                                SyntaxFactory.Argument(
                                    SyntaxFactory.IdentifierName(GetLocalization(methodInfo.DeclaringType, parameterInfo.Name))));
                        }


                        if (methodInfo.ReturnType == typeof(void))
                        {
                            method = method.AddBodyStatements(
                                        SyntaxFactory.Block(
                                            SyntaxFactory.IfStatement(
                                                SyntaxFactory.BinaryExpression(
                                                    SyntaxKind.NotEqualsExpression,
                                                    SyntaxFactory.IdentifierName("_" + AttachmentType.Name),
                                                    SyntaxFactory.LiteralExpression(SyntaxKind.NullLiteralExpression)),
                                                SyntaxFactory.ExpressionStatement(
                                                    SyntaxFactory.InvocationExpression(
                                                        SyntaxFactory.MemberAccessExpression(
                                                            SyntaxKind.SimpleMemberAccessExpression,
                                                            SyntaxFactory.IdentifierName("_" + AttachmentType.Name),
                                                            SyntaxFactory.IdentifierName(methodInfo.Name))).WithArgumentList(arguments)))));
                        }
                        else
                        {
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
                        }

                        result.Add(method);
                    }

                    foreach (PropertyInfo propertyInfo in AttachmentType.GetProperties(BindingFlags.Instance | BindingFlags.Public))
                    {

                        if (propertyInfo.IsSpecialName || propertyInfo.PropertyType.IsGenericType || CheckTypeBalckList(string.Format("{0}:{1}", propertyInfo.ReflectedType.FullName, propertyInfo.Name)))
                            continue;

                        PropertyDeclarationSyntax property = SyntaxFactory.PropertyDeclaration(
                        GetTypeSyntax(propertyInfo.PropertyType.FullName), GetLocalization(propertyInfo.DeclaringType, propertyInfo.Name)).AddModifiers(
                        SyntaxFactory.Token(SyntaxKind.PublicKeyword));
                        if (propertyInfo.CanRead && propertyInfo.GetMethod.IsPublic)
                        {
                            property = property.AddAccessorListAccessors(
                                SyntaxFactory.AccessorDeclaration(
                                    SyntaxKind.GetAccessorDeclaration,
                                    SyntaxFactory.Block(
                                        SyntaxFactory.IfStatement(
                                            SyntaxFactory.BinaryExpression(
                                                SyntaxKind.NotEqualsExpression,
                                                SyntaxFactory.IdentifierName("_" + AttachmentType.Name),
                                                SyntaxFactory.LiteralExpression(SyntaxKind.NullLiteralExpression)),
                                            SyntaxFactory.ReturnStatement(
                                                SyntaxFactory.MemberAccessExpression(
                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                    SyntaxFactory.IdentifierName("_" + AttachmentType.Name),
                                                    SyntaxFactory.IdentifierName(propertyInfo.Name)))),
                                        SyntaxFactory.ReturnStatement(
                                            SyntaxFactory.DefaultExpression(
                                                SyntaxFactory.IdentifierName(propertyInfo.PropertyType.FullName))))));


                        }
                        if (propertyInfo.CanWrite && propertyInfo.SetMethod.IsPublic)
                        {

                            property = property.AddAccessorListAccessors(
                                SyntaxFactory.AccessorDeclaration(
                                    SyntaxKind.SetAccessorDeclaration,
                                    SyntaxFactory.Block(
                                        SyntaxFactory.IfStatement(
                                            SyntaxFactory.BinaryExpression(
                                                SyntaxKind.NotEqualsExpression,
                                                SyntaxFactory.IdentifierName("_" + AttachmentType.Name),
                                                SyntaxFactory.LiteralExpression(SyntaxKind.NullLiteralExpression)),
                                            SyntaxFactory.ExpressionStatement(
                                                SyntaxFactory.AssignmentExpression(
                                                    SyntaxKind.SimpleAssignmentExpression,
                                                    SyntaxFactory.MemberAccessExpression(
                                                        SyntaxKind.SimpleMemberAccessExpression,
                                                        SyntaxFactory.IdentifierName("_" + AttachmentType.Name),
                                                        SyntaxFactory.IdentifierName(propertyInfo.Name)),
                                                    SyntaxFactory.IdentifierName("value")))))));

                        }

                        result.Add(property);
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
                    result.Set(AttachmentType, AttachmentType.Name, string.Format("TO_LOC_{0}", AttachmentType.Name));

                    foreach (MethodInfo methodInfo in AttachmentType.GetMethods(BindingFlags.Instance | BindingFlags.Public))
                    {
                        if (methodInfo.IsSpecialName || methodInfo.IsGenericMethod || CheckTypeBalckList(string.Format("{0}:{1}", methodInfo.ReflectedType.FullName, methodInfo.Name)))
                            continue;
                        result.Set(methodInfo.DeclaringType, methodInfo.Name, string.Format("TO_LOC_{0}", methodInfo.Name));

                        foreach (ParameterInfo parameter in methodInfo.GetParameters())
                        {
                            result.Set(methodInfo.DeclaringType, parameter.Name, string.Format("TO_LOC_{0}", parameter.Name));
                            if (CheckLocalizableType(parameter.ParameterType))
                                result.Set(parameter.ParameterType, parameter.ParameterType.Name, string.Format("TO_LOC_{0}", parameter.ParameterType.Name));
                        }
                        if (CheckLocalizableType(methodInfo.ReturnType))
                            result.Set(methodInfo.ReturnType, methodInfo.ReturnType.Name, string.Format("TO_LOC_{0}", methodInfo.ReturnType.Name));
                    }

                    foreach (PropertyInfo propertyInfo in AttachmentType.GetProperties(BindingFlags.Instance | BindingFlags.Public))
                    {

                        if (propertyInfo.IsSpecialName || propertyInfo.PropertyType.IsGenericType || CheckTypeBalckList(string.Format("{0}:{1}", propertyInfo.ReflectedType.FullName, propertyInfo.Name)))
                            continue;

                        result.Set(propertyInfo.DeclaringType, propertyInfo.Name, string.Format("TO_LOC_{0}", propertyInfo.Name));
                    }
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


