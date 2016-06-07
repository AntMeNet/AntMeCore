using System.Windows.Forms;
using AntMe;
using System.Collections.Generic;
using CoreTestClient.Renderer;
using System;
using System.Drawing;
using CoreTestClient.Properties;

namespace CoreTestClient
{
    public partial class EditorPanel : UserControl
    {
        private float scale = 30f;

        private Dictionary<string, MaterialRenderer> materials;

        private Dictionary<string, TileRenderer> tiles;

        private Index2 offset = Index2.Zero;

        private Index2 mapSize;

        private Map map;

        public Map Map
        {
            get { return map; }
            set
            {
                map = value;
                if (map != null)
                {
                    mapSize = map.GetCellCount();
                    RecalcScale();
                }
            }
        }

        public Index2? HoveredCell { get; private set; }

        public Index2? SelectedCell { get; private set; }

        public EditorPanel()
        {
            DoubleBuffered = true;

            materials = new Dictionary<string, MaterialRenderer>();
            materials.Add("AntMe.Basics.MapTiles.TarMaterial", new MaterialRenderer(Resources.tar));
            materials.Add("AntMe.Basics.MapTiles.MudMaterial", new MaterialRenderer(Resources.mud));
            materials.Add("AntMe.Basics.MapTiles.SandMaterial", new MaterialRenderer(Resources.sand));
            materials.Add("AntMe.Basics.MapTiles.GrasMaterial", new MaterialRenderer(Resources.gras));
            materials.Add("AntMe.Basics.MapTiles.StoneMaterial", new MaterialRenderer(Resources.stone));
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(Color.CornflowerBlue);

            if (Map != null)
            {
                var size = Map.GetCellCount();
                for (int y = 0; y < size.Y; y++)
                {
                    for (int x = 0; x < size.X; x++)
                    {
                        MapTile tile = Map[x, y];
                        if (tile == null) continue;

                        int x1 = (int)(x * scale);
                        int y1 = (int)(y * scale);
                        int x2 = (int)((x + 1) * scale);
                        int y2 = (int)((y + 1) * scale);

                        Rectangle rect = new Rectangle(x1 + offset.X, y1 + offset.Y, x2 - x1, y2 - y1);

                        MaterialRenderer material;
                        if (materials.TryGetValue(tile.Material.GetType().FullName, out material))
                            material.Draw(e.Graphics, rect);
                    }
                }
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            HoveredCell = null;
            if (Map == null) return;

            int cellX = (int)Math.Floor((e.X - offset.X) / scale);
            int cellY = (int)Math.Floor((e.Y - offset.Y) / scale);
            if (cellX >= 0 && cellX < mapSize.X && cellY >= 0 && cellY < mapSize.Y)
                HoveredCell = new Index2(cellX, cellY);
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);

            scale += e.Delta * 0.01f;
            scale = Math.Max(scale, 10f);
            Invalidate();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            RecalcScale();
        }

        private void RecalcScale()
        {
            scale = 10f;
            if (map != null)
            {
                float scaleX = (float)(ClientSize.Width - 20) / mapSize.X;
                float scaleY = (float)(ClientSize.Height - 20) / mapSize.Y;

                float smallestScale = Math.Min(scaleX, scaleY);
                scale = Math.Max(scale, smallestScale);

                offset = new Index2(
                    (ClientSize.Width - (int)(mapSize.X * scale)) / 2,
                    (ClientSize.Height - (int)(mapSize.Y * scale)) / 2);
            }
            Invalidate();
        }
    }
}
