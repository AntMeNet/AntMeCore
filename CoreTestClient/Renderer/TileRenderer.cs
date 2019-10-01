using System.Collections.Generic;
using System.Drawing;
using AntMe;

namespace CoreTestClient.Renderer
{
    public class TileRenderer
    {
        private Bitmap bitmap;

        private readonly Dictionary<MapTileOrientation, Bitmap> bitmaps;

        public TileRenderer(Bitmap bitmap)
        {
            this.bitmap = bitmap;
            bitmaps = new Dictionary<MapTileOrientation, Bitmap>();
            bitmaps.Add(MapTileOrientation.NotRotated, Rotate(bitmap, MapTileOrientation.NotRotated));
            bitmaps.Add(MapTileOrientation.RotBy90Degrees, Rotate(bitmap, MapTileOrientation.RotBy90Degrees));
            bitmaps.Add(MapTileOrientation.RotBy180Degrees, Rotate(bitmap, MapTileOrientation.RotBy180Degrees));
            bitmaps.Add(MapTileOrientation.RotBy270Degrees, Rotate(bitmap, MapTileOrientation.RotBy270Degrees));
        }

        private Bitmap Rotate(Bitmap input, MapTileOrientation orientation)
        {
            var result = new Bitmap(input.Height, input.Width);
            for (var y = 0; y < input.Height; y++)
            for (var x = 0; x < input.Width; x++)
            {
                var pixel = input.GetPixel(x, y);
                switch (orientation)
                {
                    case MapTileOrientation.NotRotated:
                        result.SetPixel(x, y, pixel);
                        break;
                    case MapTileOrientation.RotBy90Degrees:
                        result.SetPixel(result.Width - 1 - y, x, pixel);
                        break;
                    case MapTileOrientation.RotBy180Degrees:
                        result.SetPixel(result.Width - 1 - x, result.Height - 1 - y, pixel);
                        break;
                    case MapTileOrientation.RotBy270Degrees:
                        result.SetPixel(y, result.Height - 1 - x, pixel);
                        break;
                }
            }

            return result;
        }

        public void Draw(Graphics g, int x, int y, MapTileOrientation orientation)
        {
            g.DrawImageUnscaled(bitmaps[orientation], x, y);
        }
    }
}