using AntMe;
using CoreTestClient.Screens;
using System;
using System.Windows.Forms;
using AntMe.Runtime;
using AntMe.Runtime.Client.Communication;
using AntMe.Runtime.Simulation.Communication;

namespace CoreTestClient
{
    public partial class MainForm : Form
    {
        private GameModeScreen CurrentMode = null;

        private ISimulationClient CurrentClient = null;

        private RenderControl Renderer = null;

        public MainForm()
        {
            InitializeComponent();

            using (LoaderForm form = new LoaderForm())
            {
                form.ShowDialog(this);
            }
        }

        private void closeMenu_Click(object sender, System.EventArgs e)
        {
            Close();
        }

        private void extensionsMenu_Click(object sender, EventArgs e)
        {
            using (ExtensionsForm form = new ExtensionsForm())
            {
                form.ShowDialog(this);
            }
        }

        private void globalSettingsMenu_Click(object sender, EventArgs e)
        {
            using (SettingsForm form = new SettingsForm(ExtensionLoader.ExtensionSettings))
            {
                form.ShowDialog(this);
            }
        }

        private void singleGameMenu_Click(object sender, EventArgs e)
        {
            SwitchMode(new SingleGameControl());
        }

        private void SwitchMode(GameModeScreen screen)
        {
            // Stop runnung Clients
            if (CurrentClient != null)
            {
                // Stop Simulation
                CurrentClient.StopSimulation();
                
                // Dispose Viewer
                mainPanel.Controls.Remove(Renderer);
                Renderer.Dispose();
                Renderer = null;

                // Dispose Client
                CurrentClient.Dispose();
                CurrentClient = null;
                SimulationServer.Stop();
            }

            // Close open Modes
            if (CurrentMode != null)
            {
                // Dispose Mode
                mainPanel.Controls.Remove(CurrentMode);
                CurrentMode.Dispose();
                CurrentMode = null;
            }

            // Create new Mode
            CurrentMode = screen;
            mainPanel.Controls.Add(CurrentMode);
            CurrentMode.Dock = DockStyle.Fill;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            startToolButton.Enabled = CurrentMode != null && CurrentMode.CanStart;

            // Display Simulation State in Status Bar
            if (CurrentClient != null)
            {
                stateLabel.Text = CurrentClient.ServerState.ToString();
            }
            else
            {
                stateLabel.Text = "No Client";
            }

            // Display Game Time in Status Bar
            if (CurrentClient != null && CurrentClient.CurrentState != null)
            {
                timeLabel.Text = TimeSpan.FromSeconds((double)CurrentClient.CurrentState.Round / Level.FRAMES_PER_SECOND).ToString("h\\:mm\\:ss");
            }
            else
            {
                timeLabel.Text = string.Empty;
            }
        }

        private void startToolButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Create Client
                CurrentClient = CurrentMode.StartSimulation();
                CurrentClient.OnError += CurrentClient_OnError;
                CurrentClient.OnSimulationChanged += CurrentClient_OnSimulationChanged;

                // Dispose Mode
                mainPanel.Controls.Remove(CurrentMode);
                CurrentMode.Dispose();
                CurrentMode = null;

                // Start Simulation
                Renderer = new RenderControl();
                mainPanel.Controls.Add(Renderer);
                Renderer.Dock = DockStyle.Fill;
                Renderer.SetSimulation(CurrentClient);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CurrentClient_OnError(object sender, Exception e)
        {
            MessageBox.Show(e.Message);
        }

        private void CurrentClient_OnSimulationChanged(ISimulationClient client, SimulationState simulationState, byte frames)
        {
            framesToolButton.Text = string.Format("{0} fps", frames);
            // throw new NotImplementedException();
        }



        private void codeGeneratorMenu_Click(object sender, EventArgs e)
        {
            using (CodeGeneratorForm form = new CodeGeneratorForm())
            {
                form.ShowDialog(this);
            }
        }

        private void frames1ToolButton_Click(object sender, EventArgs e)
        {
            if (CurrentClient != null && CurrentClient.IsMaster)
                CurrentClient.PitchSimulation(1);
        }

        private void frames5ToolButton_Click(object sender, EventArgs e)
        {
            if (CurrentClient != null && CurrentClient.IsMaster)
                CurrentClient.PitchSimulation(5);

        }

        private void frames10ToolButton_Click(object sender, EventArgs e)
        {
            if (CurrentClient != null && CurrentClient.IsMaster)
                CurrentClient.PitchSimulation(10);
        }

        private void frames20ToolButton_Click(object sender, EventArgs e)
        {
            if (CurrentClient != null && CurrentClient.IsMaster)
                CurrentClient.PitchSimulation(20);
        }

        private void frames40ToolButton_Click(object sender, EventArgs e)
        {
            if (CurrentClient != null && CurrentClient.IsMaster)
                CurrentClient.PitchSimulation(40);
        }

        private void frames80ToolButton_Click(object sender, EventArgs e)
        {
            if (CurrentClient != null && CurrentClient.IsMaster)
                CurrentClient.PitchSimulation(80);
        }

        private void framesMaxToolButton_Click(object sender, EventArgs e)
        {
            if (CurrentClient != null && CurrentClient.IsMaster)
                CurrentClient.PitchSimulation(byte.MaxValue);
        }

        private void localizationMenu_Click(object sender, EventArgs e)
        {
            using (LocalizationForm form = new LocalizationForm())
            {
                form.ShowDialog(this);
            }
        }

        private void mapEditorMenu_Click(object sender, EventArgs e)
        {
            using (MapEditorForm form = new MapEditorForm())
            {
                form.ShowDialog(this);
            }
        }

        private void ServerGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SwitchMode(new ServerGameControl());
        }

    }
}
