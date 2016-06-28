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
            if (DialogResult == DialogResult.OK)
            {
                try
                {
                    templateGenerator.Generate(
                        nameTextBox.Text,
                        authorTextBox.Text,
                        (string)factionCombo.SelectedItem,
                        (string)languageCombo.SelectedItem,
                        (string)programmingLanguageCombo.SelectedItem,
                        (string)environmentComboBox.SelectedItem,
                        outputTextBox.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    e.Cancel = true;
                }
            }
        }

        private void regenerateButton_Click(object sender, EventArgs e)
        {
            string outputFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Extensions";

            string[] importPaths = new string[] {
                    Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                    outputFolder,
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\AntMe\\Extensions"
                };

            try
            {
                string output = ModpackGenerator.Generate(importPaths, outputFolder, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}
