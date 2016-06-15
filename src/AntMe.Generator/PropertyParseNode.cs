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
    class PropertyParseNode : BaseParseNode
    {

        public PropertyInfo PropertyInfo { get; private set; }

        public PropertyParseNode(PropertyInfo propertyInfo, WrapType wrapType)
            : base(wrapType)
        {
            PropertyInfo = propertyInfo;
            this.wrapType = wrapType;
            references.Add(propertyInfo.PropertyType);
        }

        public override MemberDeclarationSyntax Generate()
        {
            

            switch (wrapType)
            {
                case WrapType.InfoWrap:
                    PropertyDeclarationSyntax propertySyntax = SyntaxFactory.PropertyDeclaration(
                        GetTypeSyntax(PropertyInfo.PropertyType.FullName), "Loc" + PropertyInfo.Name).AddModifiers(
                        SyntaxFactory.Token(SyntaxKind.PublicKeyword));
                    if (PropertyInfo.CanRead && PropertyInfo.GetMethod.IsPublic)
                    {
                        propertySyntax = propertySyntax.AddAccessorListAccessors(
                            SyntaxFactory.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration,
                            SyntaxFactory.Block(
                                SyntaxFactory.ReturnStatement(
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.IdentifierName("_"+PropertyInfo.ReflectedType.Name),
                                        SyntaxFactory.IdentifierName(PropertyInfo.Name))))));

                    }
                    if (PropertyInfo.CanWrite && PropertyInfo.SetMethod.IsPublic)
                    {
                        propertySyntax = propertySyntax.AddAccessorListAccessors(
                            SyntaxFactory.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration,
                            SyntaxFactory.Block(
                                SyntaxFactory.ExpressionStatement(
                                    SyntaxFactory.AssignmentExpression(
                                        SyntaxKind.SimpleAssignmentExpression,
                                        SyntaxFactory.MemberAccessExpression(
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            SyntaxFactory.IdentifierName("_" + PropertyInfo.ReflectedType.Name),
                                            SyntaxFactory.IdentifierName(PropertyInfo.Name)),
                                        SyntaxFactory.IdentifierName("value"))))));

                    }

                    return propertySyntax;
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
