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

        public ClassParseNode(Type type)
            :base()
        {
            Type = type;
            references.Add(type);
        } 

        public override MemberDeclarationSyntax Generate()
        {
            return SyntaxFactory.ClassDeclaration("Loc" + Type.Name).WithModifiers(
                SyntaxFactory.TokenList(
                    SyntaxFactory.Token(SyntaxKind.PublicKeyword))).AddMembers(
                ChildNodes.Select(c => c.Generate()).ToArray()).AddMembers(
                SyntaxFactory.FieldDeclaration(
                    SyntaxFactory.VariableDeclaration(
                        SyntaxFactory.IdentifierName(Type.FullName)).AddVariables(
                        SyntaxFactory.VariableDeclarator("_" + Type.Name))
                ).AddModifiers(SyntaxFactory.Token(SyntaxKind.InternalKeyword)));

                                    
        }
    }
}
