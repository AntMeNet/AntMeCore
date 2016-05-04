using AntMe.Runtime;
using System.Windows.Forms;

namespace CoreTestClient
{
    public partial class PlayerSelectorForm : Form
    {
        public PlayerInfo SelectedPlayer { get; private set; }

        public PlayerSelectorForm()
        {
            InitializeComponent();
        }
    }
}
