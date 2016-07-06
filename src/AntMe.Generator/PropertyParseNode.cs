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

        public PropertyParseNode(PropertyInfo propertyInfo, WrapType wrapType, ModpackGenerator generator) : base(wrapType, generator)
        {
            PropertyInfo = propertyInfo;
            references.Add(propertyInfo.PropertyType);
        }

        public override MemberDeclarationSyntax[] Generate()
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
                                        SyntaxFactory.IdentifierName("_" + PropertyInfo.ReflectedType.Name),
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

                    return new MemberDeclarationSyntax[] { propertySyntax };
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
                    result.Set(PropertyInfo.DeclaringType, PropertyInfo.Name, PropertyInfo.Name);
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
