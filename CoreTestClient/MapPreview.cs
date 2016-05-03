using System;
using System.Drawing;
using System.Windows.Forms;
namespace AntMe.Simulation.Debug
{
    public partial class MapPreview : UserControl
    {
        private Map _map;

        public MapPreview()
        {
            InitializeComponent();
        }

        public void SetMap(Map map)
        {
            _map = map;
            Invalidate();
        }

        private void MapPreview_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.SkyBlue);
            if (_map != null)
            {
                
            }
        }

        private void MapPreview_Resize(object sender, EventArgs e)
        {
            Invalidate();
        }
    }
}
