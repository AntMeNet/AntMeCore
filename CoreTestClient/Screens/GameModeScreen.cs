using System;
using System.Windows.Forms;
using AntMe.Runtime.Communication;

namespace CoreTestClient.Screens
{
    public partial class GameModeScreen : UserControl
    {
        public GameModeScreen()
        {
            InitializeComponent();
        }

        public virtual bool CanStart => throw new NotImplementedException();

        public virtual ISimulationClient StartSimulation()
        {
            throw new NotImplementedException();
        }
    }
}