using AntMe;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CoreTestClient
{
    public partial class LocalizationEditor : Form
    {

        private KeyValueStore locaStore;

        public LocalizationEditor()
        {
            InitializeComponent();
            foreach (CultureInfo cul in CultureInfo.GetCultures(CultureTypes.AllCultures))
            {
                if (!LanguageToolStripComboBox.Items.Contains(cul.TwoLetterISOLanguageName))
                    LanguageToolStripComboBox.Items.Add(cul.TwoLetterISOLanguageName);
            }
        }

        private string fileName;


        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {

            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() != DialogResult.OK) { return; }

            string[] parts = dialog.FileName.Split('.');
            CultureInfo culture;
            if (parts.Length >= 3 && parts[parts.Length - 2].ToLower() != "language")
            {
                try
                {
                    
                    culture = new CultureInfo(parts[parts.Length - 2]);
                }
                catch (Exception)
                {
                    MessageBox.Show("Unknown Language");
                    return;
                }

                fileName = string.Join(".", parts, 0, parts.Length - 2);
            }
            else
            {
                culture = new CultureInfo("english");
                fileName = parts[0];
            }
            this.Text = string.Format("Localization Editor | [{1}] {0}", dialog.FileName, culture.TwoLetterISOLanguageName);
            LanguageToolStripComboBox.Text = culture.TwoLetterISOLanguageName;
            LanguageToolStripComboBox.Enabled = true;

            locaStore = new KeyValueStore(dialog.OpenFile());

            flowLayoutPanelContainer.Controls.Clear();
            foreach (var typekey in locaStore.Keys.Select(k => k.Substring(0, k.IndexOf(':'))).Distinct())
            {
                flowLayoutPanelContainer.Controls.Add(new LocalizationTypeContainer(typekey, locaStore));
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {

            KeyValueStore saveStore = new KeyValueStore();

            foreach (LocalizationTypeContainer locControl in flowLayoutPanelContainer.Controls)
            {
                foreach (string key in locControl.Keys)
                {
                    string value = locControl.Controls.Find(string.Format("{0}TextBoxValue", key), true).First().Text;
                    string description = locControl.Controls.Find(string.Format("{0}TextBoxDescription", key), true).First().Text;
                    saveStore.Set(string.Format("{0}:{1}", locControl.TypeKey, key), value, description);
                }
            }

            SaveFileDialog dialog = new SaveFileDialog();
            dialog.FileName = string.Format("{0}.{1}.language", fileName.Substring(fileName.LastIndexOf('\\') + 1), LanguageToolStripComboBox.Text.ToLower());
            if (dialog.ShowDialog() != DialogResult.OK) { return; }

            saveStore.Save(dialog.FileName);

            string[] parts = dialog.FileName.Split('.');
            CultureInfo culture;
            if (parts.Length >= 3 && parts[parts.Length - 2].ToLower() != "language")
            {
                try
                {
                    culture = new CultureInfo(parts[parts.Length - 2]);
                }
                catch (Exception)
                {
                    MessageBox.Show("Unknown Language");
                    return;
                }

                fileName = string.Join(".", parts, 0, parts.Length - 2);
            }
            else
            {
                culture = new CultureInfo("english");
                fileName = parts[0];
            }
            this.Text = string.Format("Localization Editor | [{1}] {0}", dialog.FileName, culture.TwoLetterISOLanguageName);
            LanguageToolStripComboBox.Text = culture.TwoLetterISOLanguageName;

        }
    }
}
