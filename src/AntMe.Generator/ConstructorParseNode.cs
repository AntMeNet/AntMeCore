using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using System.Reflection;
using Microsoft.CodeAnalysis;

namespace AntMe.Generator
{
    class ConstructorParseNode : BaseParseNode
    {

        public ConstructorInfo ConstructorInfo { get; private set; }

        public ConstructorParseNode(ConstructorInfo constructorInfo)
            : base()
        {
            ConstructorInfo = constructorInfo;
        }

        public override MemberDeclarationSyntax Generate()
        {

            List<ParameterSyntax> parameterList = new List<ParameterSyntax>();
            List<ArgumentSyntax> argumentList = new List<ArgumentSyntax>();

            foreach (ParameterInfo info in ConstructorInfo.GetParameters())
            {

                parameterList.Add(SyntaxFactory.Parameter(
                    SyntaxFactory.Identifier("Loc" + info.Name)).WithType(
                    SyntaxFactory.IdentifierName("Loc" + info.ParameterType.Name)));

                argumentList.Add(SyntaxFactory.Argument(
                    SyntaxFactory.IdentifierName(string.Format("Loc{0}._{0}", info.Name))));

                references.Add(info.ParameterType);

            }

            return SyntaxFactory.ConstructorDeclaration(
                SyntaxFactory.Identifier("Loc" + ConstructorInfo.ReflectedType.Name)).AddModifiers(
                SyntaxFactory.Token(SyntaxKind.PublicKeyword)).AddParameterListParameters(
                parameterList.ToArray()).WithBody(
                SyntaxFactory.Block(SyntaxFactory.ExpressionStatement(
                    SyntaxFactory.AssignmentExpression(
                        SyntaxKind.SimpleAssignmentExpression,
                        SyntaxFactory.IdentifierName("_" + ConstructorInfo.DeclaringType.Name),
                        SyntaxFactory.ObjectCreationExpression(
                            SyntaxFactory.IdentifierName(ConstructorInfo.DeclaringType.FullName)).AddArgumentListArguments(
                            argumentList.ToArray())
                            ))));
        }
    }
}
