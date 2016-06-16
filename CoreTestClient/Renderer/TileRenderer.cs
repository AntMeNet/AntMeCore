using AntMe;
using System.Collections.Generic;
using System.Drawing;

namespace CoreTestClient.Renderer
{
    public class TileRenderer
    {
        private Bitmap bitmap;

        private Dictionary<Compass, Bitmap> bitmaps;

        public TileRenderer(Bitmap bitmap)
        {
            this.bitmap = bitmap;
            bitmaps = new Dictionary<Compass, Bitmap>();
            bitmaps.Add(Compass.East, Rotate(bitmap, Compass.East));
            bitmaps.Add(Compass.South, Rotate(bitmap, Compass.South));
            bitmaps.Add(Compass.West, Rotate(bitmap, Compass.West));
            bitmaps.Add(Compass.North, Rotate(bitmap, Compass.North));
        }

        private Bitmap Rotate(Bitmap input, Compass orientation)
        {
            Bitmap result = new Bitmap(input.Height, input.Width);
            for (int y = 0; y < input.Height; y++)
            {
                for (int x = 0; x < input.Width; x++)
                {
                    Color pixel = input.GetPixel(x, y);
                    switch (orientation)
                    {
                        case Compass.East:
                            result.SetPixel(x, y, pixel);
                            break;
                        case Compass.South:
                            result.SetPixel(result.Width - 1 - y, x, pixel);
                            break;
                        case Compass.West:
                            result.SetPixel(result.Width - 1 - x, result.Height - 1 - y, pixel);
                            break;
                        case Compass.North:
                            result.SetPixel(y, result.Height - 1 - x, pixel);
                            break;
                    }
                }
            }

            return result;
        }

        public void Draw(Graphics g, int x, int y, Compass orientation)
        {
            g.DrawImageUnscaled(bitmaps[orientation], x, y);
        }
    }
}
