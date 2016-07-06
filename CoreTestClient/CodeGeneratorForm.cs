using AntMe.Generator;
using AntMe.Runtime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace CoreTestClient
{
    public partial class CodeGeneratorForm : Form
    {
        private TemplateGenerator templateGenerator;

        public CodeGeneratorForm()
        {
            InitializeComponent();

            templateGenerator = new TemplateGenerator();

            // List up all available Factions and select the first
            factionCombo.Items.AddRange(templateGenerator.Factions.ToArray());
            if (factionCombo.Items.Count > 0)
                factionCombo.SelectedIndex = 0;

            // List up all available Languages and select the first
            languageCombo.Items.AddRange(templateGenerator.Languages.ToArray());
            if (languageCombo.Items.Count > 0)
                languageCombo.SelectedIndex = 0;

            // List up all available Programming Languages and select the first
            programmingLanguageCombo.Items.AddRange(templateGenerator.ProgrammingLanguages.ToArray());
            if (programmingLanguageCombo.Items.Count > 0)
                programmingLanguageCombo.SelectedIndex = 0;

            // List up all available IDEs and select the first
            environmentComboBox.Items.AddRange(templateGenerator.Environments.ToArray());
            if (environmentComboBox.Items.Count > 0)
                environmentComboBox.SelectedIndex = 0;
        }

        private void browseButton_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog.ShowDialog(this) == DialogResult.OK)
            {
                outputTextBox.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private void CodeGeneratorForm_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void regenerateButton_Click(object sender, EventArgs e)
        {
            string outputFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Extensions";
            new ModpackGenerator(languageCombo.SelectedItem.ToString(), ExtensionLoader.DefaultTypeMapper)
                .GenerateLocaKeys(languageCombo.SelectedItem.ToString())
                .Save(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Extensions\\NewLocaTable.Language.lng");
        }

        private void generateButton_Click(object sender, EventArgs e)
        {
            string outputFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Extensions";
            //try
            //{
            templateGenerator.Generate(nameTextBox.Text,
                authorTextBox.Text,
                factionCombo.SelectedItem.ToString(),
                languageCombo.SelectedItem.ToString(),
                programmingLanguageCombo.SelectedItem.ToString(),
                environmentComboBox.SelectedItem.ToString(),
                outputFolder);
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
        }
    }
}
