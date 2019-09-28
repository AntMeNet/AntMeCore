using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using AntMe.Runtime;

namespace CoreTestClient
{
    public partial class LevelSelectorForm : Form
    {
        public LevelInfo SelectedLevel { get; private set; }

        public LevelSelectorForm()
        {
            InitializeComponent();

            levelList.Items.Clear();
            foreach (var level in ExtensionLoader.Levels)
            {
                var item = levelList.Items.Add(level.LevelDescription.Name);
                item.Tag = level;
                item.ToolTipText = level.LevelDescription.Description;
            }
        }

        private void loadButton_Click(object sender, System.EventArgs e)
        {
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string[] extensionPaths = new string[] {
                    Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                    Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Extensions",
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\AntMe\\Extensions"
                };

                byte[] file = File.ReadAllBytes(openFileDialog.FileName);
                var result = ExtensionLoader.SecureAnalyseExtension(extensionPaths, file, true, false);

                foreach (var level in result.Levels)
                {
                    level.Type.AssemblyFile = file;
                    var item = levelList.Items.Add(level.LevelDescription.Name);
                    item.Tag = level;
                    item.ToolTipText = level.LevelDescription.Description;
                }
            }
        }

        private void levelList_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (levelList.SelectedItems.Count > 0)
            {
                var levelInfo = levelList.SelectedItems[0].Tag as LevelInfo;
                levelPreview.SetLevel(levelInfo);
                levelPreview.Visible = true;
                SelectedLevel = levelInfo;
            }
            else
            {
                levelPreview.SetLevel(null);
                levelPreview.Visible = false;
                SelectedLevel = null;
            }
        }

        private void levelList_DoubleClick(object sender, EventArgs e)
        {
            if (levelList.SelectedItems.Count > 0)
            {
                LevelInfo level = levelList.SelectedItems[0].Tag as LevelInfo;
                SelectedLevel = level;
                DialogResult = DialogResult.OK;
                Close();
            }
        }
    }
}
