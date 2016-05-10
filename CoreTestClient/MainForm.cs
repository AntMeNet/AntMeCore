using AntMe.Runtime;
using AntMe.Runtime.Communication;
using CoreTestClient.Screens;
using System;
using System.Windows.Forms;

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

            if (CurrentClient != null)
            {
                stateLabel.Text = CurrentClient.ServerState.ToString();
            }
            else
            {
                stateLabel.Text = "No Client";
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

        private void CurrentClient_OnSimulationChanged(ISimulationClient client, AntMe.Runtime.SimulationState parameter1, byte parameter2)
        {
            // throw new NotImplementedException();
        }

        private void CurrentClient_OnError(ISimulationClient client, string message)
        {
            MessageBox.Show(message);
        }
    }
}
