using AntMe.Runtime;
using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace CoreTestClient
{
    public partial class PlayerSelectorForm : Form
    {
        public PlayerInfo SelectedPlayer { get; private set; }

        public PlayerSelectorForm()
        {
            InitializeComponent();

            playerList.Items.Clear();
            foreach (var player in ExtensionLoader.Players)
            {
                var item = playerList.Items.Add(player.Name);
                item.Tag = player;
                item.SubItems.Add(player.Author);
            }
        }

        private void playerList_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (playerList.SelectedItems.Count > 0)
            {
                var playerInfo = playerList.SelectedItems[0].Tag as PlayerInfo;
                playerPreview.SetPlayer(playerInfo);
                playerPreview.Visible = true;
                SelectedPlayer = playerInfo;
            }
            else
            {
                playerPreview.SetPlayer(null);
                playerPreview.Visible = false;
                SelectedPlayer = null;
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

                string filename = openFileDialog.FileName;
                byte[] file = File.ReadAllBytes(filename);
                var result = ExtensionLoader.SecureAnalyseExtension(extensionPaths, filename, false, true);

                foreach (var player in result.Players)
                {
                    if (filedumpCheckbox.Checked)
                        player.Type = TypeInfoByDump.FromTypeInfo(player.Type, file);
                    else
                        player.Type = TypeInfoByFilename.FromTypeInfo(player.Type, filename);
                    var item = playerList.Items.Add(player.Name);
                    item.Tag = player;
                    item.SubItems.Add(player.Author);
                }
            }
        }

        private void playerList_DoubleClick(object sender, EventArgs e)
        {
            if (playerList.SelectedItems.Count > 0)
            {
                PlayerInfo player = playerList.SelectedItems[0].Tag as PlayerInfo;
                SelectedPlayer = player;
                DialogResult = DialogResult.OK;
                Close();
            }
        }
    }
}
