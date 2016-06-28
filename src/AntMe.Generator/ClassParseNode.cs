using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AntMe.Generator
{
    class ClassParseNode : BaseParseNode
    {

        public Type Type { get; private set; }

        public ClassParseNode(Type type, WrapType wrapType)
            : base(wrapType)
        {
            Type = type;
            references.Add(type);
        }

        public override MemberDeclarationSyntax Generate()
        {
            references.Add(Type);

            switch (wrapType)
            {
                case WrapType.InfoWrap:
                    ClassDeclarationSyntax classSyntax =
                        SyntaxFactory.ClassDeclaration("Loc" + Type.Name).WithModifiers(
                            SyntaxFactory.TokenList(
                                SyntaxFactory.Token(SyntaxKind.PublicKeyword))).AddMembers(
                        ChildNodes.Select(c => c.Generate()).ToArray()).AddMembers(
                            SyntaxFactory.FieldDeclaration(
                                SyntaxFactory.VariableDeclaration(
                                     GetTypeSyntax(Type.FullName)).AddVariables(
                                    SyntaxFactory.VariableDeclarator("_" + Type.Name))).AddModifiers(
                                SyntaxFactory.Token(SyntaxKind.InternalKeyword)));


                    ConstructorDeclarationSyntax constructor = SyntaxFactory.ConstructorDeclaration("Loc" + Type.Name).AddModifiers(
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
                        classSyntax = classSyntax.AddBaseListTypes(SyntaxFactory.SimpleBaseType(SyntaxFactory.IdentifierName("Loc" + Type.BaseType.Name)));
                        constructor = constructor.WithInitializer(SyntaxFactory.ConstructorInitializer(
                            SyntaxKind.BaseConstructorInitializer, 
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SingletonSeparatedList(
                                SyntaxFactory.Argument(
                                    SyntaxFactory.IdentifierName("info"))))));
                    }

                    classSyntax = classSyntax.AddMembers(constructor);

                    return classSyntax;
                case WrapType.BaseTypeWrap:
                    return null;
                case WrapType.BaseClasses:
                    return null;
                default:
                    return null;
            }
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

            foreach (BaseParseNode node in ChildNodes)
            {
                result.Merge(node.GetLocaKeys());
            }

            return result;
        }
    }
}
