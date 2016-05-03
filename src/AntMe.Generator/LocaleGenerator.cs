using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntMe.Generator
{
    public sealed class LocaleGenerator
    {
        public void Generate()
        {
            // Localized Base Classes
            // 1) Wrapper für Basis-Typen (Kompass, SpielerAttribut, Umgebung, Zufall)
            // 2) Wrapper für Info-Items (Items und Faction-Items) inkl. Properties
            // 3) Basis-Klassen für Factory und Unit inkl. Interop-Properties
            // 4) Wrapper für Faction-Spezifische Attribute (Caste)

            CSharpCodeProvider provider = new CSharpCodeProvider();
            CompilerParameters parameter = new CompilerParameters();

            string file = @"";

            parameter.GenerateExecutable = false;
            parameter.GenerateInMemory = false;
            parameter.IncludeDebugInformation = true;
            parameter.OutputAssembly = "test.dll";
            parameter.CompilerOptions = "/doc:test.xml";

            provider.CompileAssemblyFromSource(parameter, file);
        }

    }
}
