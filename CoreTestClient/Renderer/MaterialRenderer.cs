using System;
using System.Collections.Generic;
using System.Drawing;

namespace CoreTestClient.Renderer
{
    public class MaterialRenderer
    {
        private Dictionary<int, Bitmap> mipMaps;

        private int levels;

        private Bitmap bitmap;

        public MaterialRenderer(Bitmap bitmap)
        {
            this.bitmap = bitmap;
            mipMaps = new Dictionary<int, Bitmap>();

            int level = 0;
            while (Math.Pow(2, level) <= bitmap.Width)
            {
                int width = (int)Math.Pow(2, level);
                Bitmap bm = new Bitmap(width, width);
                using (Graphics g = Graphics.FromImage(bm))
                {
                    g.DrawImage(bitmap, new Rectangle(0, 0, width, width));
                }
                mipMaps.Add(level, bm);
                levels = level;
                level++;
            }
        }

        public void Draw(Graphics g, Rectangle destination)
        {
            int mipMapLevel = Math.Min((int)Math.Ceiling(Math.Log(destination.Width, 2)), levels);

            g.DrawImage(mipMaps[mipMapLevel], destination);
        }
    }
}
