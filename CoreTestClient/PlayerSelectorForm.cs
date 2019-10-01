using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using AntMe.Runtime;

namespace CoreTestClient
{
    public partial class PlayerSelectorForm : Form
    {
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

        public PlayerInfo SelectedPlayer { get; private set; }

        private void playerList_SelectedIndexChanged(object sender, EventArgs e)
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

        private void loadButton_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string[] extensionPaths =
                {
                    Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                    Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Extensions",
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\AntMe\\Extensions"
                };

                var file = File.ReadAllBytes(openFileDialog.FileName);
                var result = ExtensionLoader.SecureAnalyseExtension(extensionPaths, file, false, true);

                foreach (var player in result.Players)
                {
                    player.Type.AssemblyFile = file;
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
                var player = playerList.SelectedItems[0].Tag as PlayerInfo;
                SelectedPlayer = player;
                DialogResult = DialogResult.OK;
                Close();
            }
        }
    }
}