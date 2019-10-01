using System;
using System.Collections.Generic;
using System.Linq;
using AntMe.Runtime;

namespace AntMe.Generator
{
    /// <summary>
    ///     Code Generator to generate the players Source Template.
    /// </summary>
    public sealed class TemplateGenerator
    {
        private readonly List<string> environments;
        private readonly List<string> factions;

        private readonly List<string> languages;

        private readonly List<string> programmingLanguages;

        public TemplateGenerator()
        {
            factions = new List<string>(ExtensionLoader.DefaultTypeMapper.Factions.Select(f => f.Name));
            languages = new List<string>(new[] {"English", "Deutsch"});
            programmingLanguages = new List<string>(new[] {"C#", "VB.NET"});
            environments = new List<string>(new[] {"Visual Studio 2015", "Visual Studio Code"});
        }

        /// <summary>
        ///     List of available Factions to generate for.
        /// </summary>
        public IEnumerable<string> Factions => factions;

        /// <summary>
        ///     List of available natural Languages.
        /// </summary>
        public IEnumerable<string> Languages => languages;

        /// <summary>
        ///     List of available Programming Languages.
        /// </summary>
        public IEnumerable<string> ProgrammingLanguages => programmingLanguages;

        /// <summary>
        ///     List of supported IDEs.
        /// </summary>
        public IEnumerable<string> Environments => environments;

        public void Generate(string name, string author, string faction, string language, string programmingLanguage,
            string environment, string outputFolder)
        {
            // TODO: Do the Magic
            throw new NotImplementedException();
        }
    }
}