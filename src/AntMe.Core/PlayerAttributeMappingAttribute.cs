using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntMe
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class PlayerAttributeMappingAttribute : Attribute
    {
        /// <summary>
        /// Gibt den Eigenschaftsnamen an, der den Namen der KI enthält.
        /// </summary>
        public string NameProperty { get; set; }

        /// <summary>
        /// Gibt die Eigenschaft an, die den Namen des Autors enthält.
        /// </summary>
        public string AuthorProperty { get; set; }
    }
}
