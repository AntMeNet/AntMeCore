using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using AntMe.Runtime;

namespace AntMe.Generator
{
    class ClassParseNode : BaseParseNode
    {

        public Type Type { get; private set; }

        public ClassParseNode(Type type, WrapType wrapType, ModpackGenerator generator) : base(wrapType, generator)
        {
            Type = type;
            references.Add(type);
        }

        public override MemberDeclarationSyntax[] Generate()
        {
            references.Add(Type);

            switch (wrapType)
            {
                case WrapType.InfoWrap:
                    ClassDeclarationSyntax classSyntax =
                        SyntaxFactory.ClassDeclaration(GetLocalization(Type)).WithModifiers(
                            SyntaxFactory.TokenList(
                                SyntaxFactory.Token(SyntaxKind.PublicKeyword))).AddMembers(
                        ChildNodes.SelectMany(c => c.Generate()).ToArray()).AddMembers(
                            SyntaxFactory.FieldDeclaration(
                                SyntaxFactory.VariableDeclaration(
                                     GetTypeSyntax(Type.FullName)).AddVariables(
                                    SyntaxFactory.VariableDeclarator("_" + Type.Name))).AddModifiers(
                                SyntaxFactory.Token(SyntaxKind.InternalKeyword)));


                    ConstructorDeclarationSyntax constructor = SyntaxFactory.ConstructorDeclaration(GetLocalization(Type)).AddModifiers(
                            SyntaxFactory.Token(SyntaxKind.PublicKeyword)).AddParameterListParameters(
                            SyntaxFactory.Parameter(SyntaxFactory.Identifier("info")).WithType(
                                 GetTypeSyntax(Type.FullName))).WithBody(
                            SyntaxFactory.Block(SyntaxFactory.ExpressionStatement(
                                SyntaxFactory.AssignmentExpression(
                                    SyntaxKind.SimpleAssignmentExpression,
                                    SyntaxFactory.IdentifierName("_" + Type.Name),
                                    SyntaxFactory.IdentifierName("info")))));

                    if (Type.BaseType != typeof(PropertyList<ItemInfoProperty>))
                    {
                        classSyntax = classSyntax.AddBaseListTypes(SyntaxFactory.SimpleBaseType(SyntaxFactory.IdentifierName(GetLocalization(Type.BaseType))));
                        constructor = constructor.WithInitializer(SyntaxFactory.ConstructorInitializer(
                            SyntaxKind.BaseConstructorInitializer,
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SingletonSeparatedList(
                                SyntaxFactory.Argument(
                                    SyntaxFactory.IdentifierName("info"))))));
                    }

                    classSyntax = classSyntax.AddMembers(constructor);

                    return new MemberDeclarationSyntax[] { classSyntax };
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
                    result.Set(Type, Type.Name, string.Format("TO_LOC_{0}", Type.Name));
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
