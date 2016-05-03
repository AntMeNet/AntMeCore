using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntMe
{
    /// <summary>
    /// Attribut zum dekorieren von CodeGenerator Klassen.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class CodeGeneratorAttribute : Attribute
    {        
        /// <summary>
        /// Gibt den Namen der Faction zurück, die dieser Generator unterstützt.
        /// </summary>
        public string Faction { get; set; }

        /// <summary>
        /// Gibt die Sprache an, in der der Code dieses Generators erzeugt wird.
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// Gibt die Programmiersprache an, die dieser Generator erzeugt.
        /// </summary>
        public string ProgrammingLanguage { get; set; }

        /// <summary>
        /// Gibt den Code Editor an, für den der Code dieses Generators erzeugt wird.
        /// </summary>
        public string Editor { get; set; }

        /// <summary>
        /// Puffer zum Zwischenspeichern der generator-Instanz.
        /// </summary>
        public ICodeGenerator Generator { get; set; }
    }
}
