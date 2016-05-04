using AntMe.Runtime.Communication;
using System.Windows.Forms;

namespace CoreTestClient.Screens
{
    public abstract partial class GameModeScreen : UserControl
    {
        public abstract bool CanStart { get; }

        public abstract ISimulationClient StartSimulation();

        public GameModeScreen()
        {
            InitializeComponent();
        }
    }
}
