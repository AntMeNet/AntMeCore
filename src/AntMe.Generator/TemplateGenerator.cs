using AntMe.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AntMe.Generator
{
    /// <summary>
    /// Code Generator to generate the players Source Template.
    /// </summary>
    public sealed class TemplateGenerator
    {
        private List<string> factions;

        private List<string> languages;

        private List<string> programmingLanguages;

        private List<string> environments;

        /// <summary>
        /// List of available Factions to generate for.
        /// </summary>
        public IEnumerable<string> Factions { get { return factions; } }

        /// <summary>
        /// List of available natural Languages.
        /// </summary>
        public IEnumerable<string> Languages { get { return languages; } }

        /// <summary>
        /// List of available Programming Languages.
        /// </summary>
        public IEnumerable<string> ProgrammingLanguages { get { return programmingLanguages; } }

        /// <summary>
        /// List of supported IDEs.
        /// </summary>
        public IEnumerable<string> Environments { get { return environments; } }

        public TemplateGenerator()
        {
            factions = new List<string>(ExtensionLoader.DefaultTypeMapper.Factions.Select(f => f.Name));
            languages = new List<string>(ExtensionLoader.LocalizedLanguages);
            programmingLanguages = new List<string>(new[] { "C#", "VB.NET" });
            environments = new List<string>(new[] { "Visual Studio 2015", "Visual Studio Code" });
        }

        public void Generate(string name, string author, string faction, string language, string programmingLanguage, string environment, string outputFolder)
        {
            // TODO: Do the Magic
            try
            {
                ModpackGenerator.Generate(language, outputFolder, ExtensionLoader.DefaultTypeMapper);
            }
            catch (Exception ex)
            {

            }
           
        }
    }
}
