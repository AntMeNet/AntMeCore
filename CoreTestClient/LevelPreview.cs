using System.Windows.Forms;
using AntMe.Runtime;

namespace AntMe.Simulation.Debug
{
    public partial class LevelPreview : UserControl
    {
        LevelInfo level;

        public LevelPreview()
        {
            InitializeComponent();
        }

        public void SetLevel(LevelInfo level)
        {
            this.level = level;

            if (level != null)
            {
                nameLabel.Text = level.LevelDescription.Name;
                descriptionLabel.Text = level.LevelDescription.Description;
                mapPreview1.SetMap(Map.Deserialize(ExtensionLoader.CreateSimulationContext(), level.Map));
            }
            else
            {
                nameLabel.Text = string.Empty;
                descriptionLabel.Text = string.Empty;
                mapPreview1.SetMap(null);
            }

        }
    }
}
