using System;
using System.Windows.Forms;
using AntMe.Runtime;
using AntMe.Serialization;

namespace AntMe.Simulation.Debug
{
    public partial class LevelPreview : UserControl
    {
        private LevelInfo level;

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
                    var mapBuffer = level.Map;
                    if (mapBuffer == null) return;

                    var map = MapSerializer.Deserialize(ExtensionLoader.CreateSimulationContext(), level.Map);
                    mapPreview.SetMap(map);
                }
                catch (Exception)
                {
                }
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