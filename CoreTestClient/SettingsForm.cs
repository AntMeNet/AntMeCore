using AntMe;
using AntMe.Runtime;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;

namespace CoreTestClient
{
    public partial class SettingsForm : Form
    {
        private KeyValueStore settings;

        private Dictionary<string, string[]> keys = new Dictionary<string, string[]>();

        public SettingsForm(KeyValueStore settings)
        {
            InitializeComponent();
            this.settings = settings;

            string[] keytemp = settings.Keys.ToArray();

            var typeKeys = keytemp.Select(k => k.Substring(0, k.IndexOf(':'))).Distinct();
            foreach (var key in typeKeys.OrderBy(k => k))
            {
                var item = typeList.Items.Add(key);
                item.Tag = key;
                keys.Add(key, keytemp.Where(k => k.StartsWith(key)).Select(k => k.Substring(k.IndexOf(":") + 1)).ToArray());
            }
        }

        public SettingsForm() : this(ExtensionLoader.ExtensionSettings) { }

        private void typeList_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            valuesList.Items.Clear();
            if (typeList.SelectedItems.Count > 0)
            {
                string key = typeList.SelectedItems[0].Tag as string;
                foreach (var k in keys[key].OrderBy(k => k))
                {
                    var item = valuesList.Items.Add(k);
                    string fullkey = string.Format("{0}:{1}", key, k);
                    item.SubItems.Add(settings.GetString(fullkey));
                    item.SubItems.Add(settings.GetDescription(fullkey));
                }
            }
        }

        private void saveButton_Click(object sender, System.EventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            

            saveDialog.Title = "Save Settings";
            saveDialog.Filter = "Settings-File|*.set";


            if (saveDialog.ShowDialog() == DialogResult.OK && saveDialog.FileName != null)
                settings.Save(saveDialog.FileName);


        }

        private void openButton_Click(object sender, System.EventArgs e)
        {
            //work in progress (Patrick Kirsch)
            //OpenFileDialog saveDialog = new OpenFileDialog();


            //saveDialog.Title = "Open Settings";
            //saveDialog.Filter = "Settings-File|*.set";


            //if (saveDialog.ShowDialog() == DialogResult.OK && saveDialog.FileName != null)
            //    settings = KeyValueStore.Load(saveDialog.FileName);
        }
    }
}
