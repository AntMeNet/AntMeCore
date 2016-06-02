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
            reloadSettings(settings);
            typeTreeView.PathSeparator = ".";
        }

        public SettingsForm() : this(ExtensionLoader.ExtensionSettings) { }

        private void reloadSettings(KeyValueStore settings)
        {
            this.settings = settings;
            keys = new Dictionary<string, string[]>();

            typeTreeView.Nodes.Clear();

            string[] keytemp = settings.Keys.ToArray();

            var typeKeys = keytemp.Select(k => k.Substring(0, k.IndexOf(':'))).Distinct();

            foreach (var key in typeKeys.OrderBy(k => k))
            {

                string[] nodes = key.Split('.');
                TreeNode parentnode = null;
                for (int i = 0; i < nodes.Length; i++)
                {

                    if (parentnode == null)
                    {
                        if (!typeTreeView.Nodes.ContainsKey(nodes[i]))
                        {
                            TreeNode temp = new TreeNode(nodes[i]);
                            temp.Name = nodes[i];
                            typeTreeView.Nodes.Add(temp);
                            parentnode = temp;
                        }
                        else
                            parentnode = typeTreeView.Nodes.Find(nodes[i], false)[0];
                    }
                    else
                    {
                        if (!parentnode.Nodes.ContainsKey(nodes[i]))
                        {
                            TreeNode temp = new TreeNode(nodes[i]);
                            temp.Name = nodes[i];
                            parentnode.Nodes.Add(temp);
                            parentnode = temp;
                        }
                        else
                            parentnode = parentnode.Nodes.Find(nodes[i], false)[0];
                    }
                }

                keys.Add(key, keytemp.Where(k => k.StartsWith(key)).Select(k => k.Substring(k.IndexOf(":") + 1)).ToArray());
            }
            typeTreeView.ExpandAll();
            Refresh();
        }

        private void saveButton_Click(object sender, System.EventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();


            saveDialog.Title = "Save Settings";
            saveDialog.Filter = "Settings-File|*.set";


            if (saveDialog.ShowDialog() == DialogResult.OK && saveDialog.FileName != null)
                settings.Save(saveDialog.FileName, easyReadCheckBox.Checked);


        }

        private void openButton_Click(object sender, System.EventArgs e)
        {

            OpenFileDialog openDialog = new OpenFileDialog();


            openDialog.Title = "Open Settings";
            openDialog.Filter = "Settings-File|*.set";


            if (openDialog.ShowDialog() == DialogResult.OK && openDialog.FileName != null)
                reloadSettings(new KeyValueStore(openDialog.FileName));
        }

        private void typeTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {

            if (e.Node.LastNode != null)
            {
                valuesList.Items.Clear();
                valuesList.SelectedItems.Clear();
                editTextBoxesUpdate();
                return;
            }


            valuesList.Items.Clear();

            string key = e.Node.FullPath;
            foreach (var k in keys[key].OrderBy(k => k))
            {
                var item = valuesList.Items.Add(k);
                string fullkey = string.Format("{0}:{1}", key, k);
                item.SubItems.Add(settings.GetString(fullkey));
                item.SubItems.Add(settings.GetDescription(fullkey));
            }
            valuesList.Items[0].Selected = true;

        }

        private void editTextBoxesUpdate()
        {

            if (valuesList.SelectedItems.Count > 0)
            {
                valueTextBox.Enabled = true;
                valueTextBox.Text = valuesList.SelectedItems[0].SubItems[1].Text;
                valueTextBox.TextAlign = HorizontalAlignment.Left;
                descriptionTextBox.Enabled = true;
                descriptionTextBox.Text = valuesList.SelectedItems[0].SubItems[2].Text;
                descriptionTextBox.TextAlign = HorizontalAlignment.Left;
            }
            else
            {
                valueTextBox.Enabled = false;
                valueTextBox.Text = "(Value)";
                valueTextBox.TextAlign = HorizontalAlignment.Center;
                descriptionTextBox.Enabled = false;
                descriptionTextBox.Text = "(Description)";
                descriptionTextBox.TextAlign = HorizontalAlignment.Center;
            }

        }

        private void valuesList_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            editTextBoxesUpdate();
        }

        private void updateSettings()
        {
            if (valuesList.SelectedItems.Count == 0)
                return;
            if (valueTextBox.Text == valuesList.SelectedItems[0].SubItems[1].Text && descriptionTextBox.Text == valuesList.SelectedItems[0].SubItems[2].Text)
                return;

            settings.Set(string.Format("{0}:{1}", typeTreeView.SelectedNode.FullPath, valuesList.SelectedItems[0].Text), valueTextBox.Text, descriptionTextBox.Text);
            valuesList.SelectedItems[0].SubItems[1].Text = valueTextBox.Text;
            valuesList.SelectedItems[0].SubItems[2].Text = descriptionTextBox.Text;
        }

        private void valueTextBox_Leave(object sender, System.EventArgs e)
        {
            updateSettings();
        }

        private void descriptionTextBox_Leave(object sender, System.EventArgs e)
        {
            updateSettings();
        }

        private void valueTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                updateSettings();
                descriptionTextBox.Select();
                e.Handled = true;
            }

        }

        private void descriptionTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                updateSettings();
                valuesList.Items[valuesList.SelectedItems[0].Index < (valuesList.Items.Count - 1) ? valuesList.SelectedItems[0].Index + 1 : 0].Selected = true;
                valueTextBox.Select();
                e.Handled = true;
            }
        }
    }
}
