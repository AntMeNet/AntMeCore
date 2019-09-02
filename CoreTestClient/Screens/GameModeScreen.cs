using System;
using System.Windows.Forms;

namespace CoreTestClient.Screens
{
    public partial class GameModeScreen : UserControl
    {
        public virtual bool CanStart { get { throw new NotImplementedException(); } }

        public virtual ISimulationClient StartSimulation()
        {
            throw new NotImplementedException();
        }

        public GameModeScreen()
        {
            InitializeComponent();
        }
    }
}
