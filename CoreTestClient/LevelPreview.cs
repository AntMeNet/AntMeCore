using System.Windows.Forms;
using System;
using AntMe.Serialization;

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

                try
                {
                    byte[] mapBuffer = level.Map;
                    if (mapBuffer == null) return;

                    Map map = MapSerializer.Deserialize(ExtensionLoader.CreateSimulationContext(), level.Map);
                    mapPreview.SetMap(map);
                }
                catch (Exception) { }

            }
            else
            {
                nameLabel.Text = string.Empty;
                descriptionLabel.Text = string.Empty;
                mapPreview.SetMap(null);
            }

        }
    }
}
