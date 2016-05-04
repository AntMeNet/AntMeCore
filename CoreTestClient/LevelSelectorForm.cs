using AntMe.Runtime;
using System.IO;
using System.Windows.Forms;

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
                byte[] file = File.ReadAllBytes(openFileDialog.FileName);
                var result = ExtensionLoader.SecureAnalyseExtension(file, true, false);

                foreach (var level in result.Levels)
                {
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
    }
}
