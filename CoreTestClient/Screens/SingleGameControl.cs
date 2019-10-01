using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using AntMe;
using AntMe.Runtime;
using AntMe.Runtime.Communication;

namespace CoreTestClient.Screens
{
    public partial class SingleGameControl : GameModeScreen
    {
        private LevelInfo level;

        private readonly PlayerInfo[] slots = new PlayerInfo[8];

        public SingleGameControl()
        {
            InitializeComponent();
            UpdateView();
        }

        public override bool CanStart
        {
            get
            {
                // Level must be available
                if (level == null)
                    return false;

                // The required minimum of Slots must be filled
                var playerCount = 0;
                for (var i = 0; i < level.LevelDescription.MaxPlayerCount; i++)
                    if (slots[i] != null)
                        playerCount++;
                if (playerCount < level.LevelDescription.MinPlayerCount)
                    return false;
                if (playerCount > level.LevelDescription.MaxPlayerCount)
                    return false;

                return true;
            }
        }

        public override ISimulationClient StartSimulation()
        {
            string[] extensionPaths =
            {
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Extensions",
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\AntMe\\Extensions"
            };

            var result = SimulationClient.CreateSecure(extensionPaths, ExtensionLoader.DefaultTypeResolver);
            result.AquireMaster();
            result.UploadLevel(level.Type);
            for (byte i = 0; i < 8; i++)
                if (slots[i] != null)
                {
                    result.UploadMaster(i, slots[i].Type);
                    result.SetMasterState(i, (PlayerColor) i, i, true);
                }

            result.StartSimulation();
            return result;
        }

        private void levelButton_Click(object sender, EventArgs e)
        {
            using (var form = new LevelSelectorForm())
            {
                if (form.ShowDialog(this) == DialogResult.OK && form.SelectedLevel != null)
                {
                    level = form.SelectedLevel;
                    levelRemoveButton.Visible = true;
                    levelLabel.Text = level.LevelDescription.Name;
                    UpdateView();
                }
            }
        }

        private void levelRemoveButton_Click(object sender, EventArgs e)
        {
            level = null;
            levelRemoveButton.Visible = false;
            levelLabel.Text = "No Level selected";
            UpdateView();
        }

        private void UpdateView()
        {
            var min = level?.LevelDescription.MinPlayerCount ?? 0;
            var max = level?.LevelDescription.MaxPlayerCount ?? 0;

            for (var i = 1; i <= 8; i++)
            {
                var slotButton = Controls["slot" + i + "Button"] as Button;
                var slotRemoveButton = Controls["slot" + i + "RemoveButton"] as Button;
                if (i > max)
                    slotButton.Enabled = false;
                else
                    slotButton.Enabled = true;
            }
        }

        private void slotbutton_Click(object sender, EventArgs e)
        {
            var slot = int.Parse((sender as Button).Name.Substring(4, 1));

            using (var form = new PlayerSelectorForm())
            {
                if (form.ShowDialog(this) == DialogResult.OK && form.SelectedPlayer != null)
                {
                    var player = form.SelectedPlayer;
                    slots[slot - 1] = player;
                    var slotRemoveButton = Controls["slot" + slot + "RemoveButton"] as Button;
                    var slotLabel = Controls["slot" + slot + "Label"] as Label;
                    slotLabel.Text = player.Name;
                    slotRemoveButton.Visible = true;
                }
            }
        }

        private void slotRemoveButton_Click(object sender, EventArgs e)
        {
            var slot = int.Parse((sender as Button).Name.Substring(4, 1));

            slots[slot - 1] = null;
            var slotRemoveButton = Controls["slot" + slot + "RemoveButton"] as Button;
            var slotLabel = Controls["slot" + slot + "Label"] as Label;
            slotLabel.Text = "No Player selected";
            slotRemoveButton.Visible = false;
        }
    }
}