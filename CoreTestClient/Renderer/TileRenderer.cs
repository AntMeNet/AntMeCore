using AntMe;
using System.Drawing;

namespace CoreTestClient.Renderer
{
    public class TileRenderer
    {
        private Bitmap bitmap;

        public TileRenderer(Bitmap bitmap)
        {
            this.bitmap = bitmap;
        }

        public void Draw(Graphics g, Rectangle destination, Compass orientation)
        {

        }
    }
}
