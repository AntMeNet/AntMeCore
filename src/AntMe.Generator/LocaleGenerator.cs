using System.CodeDom.Compiler;
using Microsoft.CSharp;

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

            var provider = new CSharpCodeProvider();
            var parameter = new CompilerParameters();

            var file = @"";

            parameter.GenerateExecutable = false;
            parameter.GenerateInMemory = false;
            parameter.IncludeDebugInformation = true;
            parameter.OutputAssembly = "test.dll";
            parameter.CompilerOptions = "/doc:test.xml";

            provider.CompileAssemblyFromSource(parameter, file);
        }
    }
}