using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;

namespace AntMe.Generator
{
    class EnumParseNode : BaseParseNode
    {

        public Type EnumType { get; private set; }

        public EnumParseNode(Type enumType,WrapType wrapType, ModpackGenerator generator) : base(wrapType, generator)
        {
            EnumType = enumType;
            references.Add(enumType);
        }

        public override MemberDeclarationSyntax[] Generate()
        {
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
            return new MemberDeclarationSyntax[] { };
        }

        public override KeyValueStore GetLocaKeys()
        {
            KeyValueStore result = new KeyValueStore();

            //switch (wrapType)
            //{
            //    case WrapType.InfoWrap:
            //        result.Set(Type, Type.Name, string.Format("TO_LOC_{0}", Type.Name));
            //        break;
            //    case WrapType.BaseTypeWrap:
            //        break;
            //    case WrapType.BaseClasses:
            //        break;
            //    default:
            //        break;
            //}

            foreach (BaseParseNode node in ChildNodes)
            {
                result.Merge(node.GetLocaKeys());
            }

            return result;
        }
    }
}
