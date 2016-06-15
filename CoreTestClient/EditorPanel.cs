using System.Windows.Forms;
using AntMe;
using System.Collections.Generic;
using CoreTestClient.Renderer;
using System;
using System.Drawing;
using AntMe.Runtime;
using System.IO;

namespace CoreTestClient
{
    public partial class EditorPanel : UserControl
    {
        private const int TILEWIDTH = 64;

        private float scale = 30f;

        private Dictionary<string, MaterialRenderer> materials;

        private Dictionary<string, TileRenderer> tiles;

        private Vector2 offset = Vector2.Zero;

        private Index2 mapSize;

        private Map map;

        private Bitmap buffer;

        private Brush hoverBrush;

        private Pen highlightPen;

        public Map Map
        {
            get { return map; }
            set
            {
                map = value;

                if (buffer != null)
                {
                    buffer.Dispose();
                    buffer = null;
                }

                if (map != null)
                {
                    mapSize = map.GetCellCount();
                    buffer = new Bitmap(mapSize.X * 64, mapSize.Y * 64);
                    RedrawBuffer();
                    RecalcScale();
                }
            }
        }

        public Index2? HoveredCell { get; private set; }

        /// <summary>
        /// Gets or sets the Cell that should be highlighted
        /// </summary>
        public Index2? HighlightedCell { get; set; }

        /// <summary>
        /// Gets or sets whenever the Buffer is dirty.
        /// </summary>
        public bool DirtyBuffer { get; set; }

        public EditorPanel()
        {
            DoubleBuffered = true;

            hoverBrush = new SolidBrush(Color.FromArgb(80, Color.White));
            highlightPen = new Pen(Color.Red, 3);

            materials = new Dictionary<string, MaterialRenderer>();
            foreach (var material in ExtensionLoader.DefaultTypeMapper.MapMaterials)
            {
                string path = Path.Combine(".", "Resources", material.Type.Name + ".png");
                Bitmap bitmap = new Bitmap(Image.FromFile(path));
                materials.Add(material.Type.FullName, new MaterialRenderer(bitmap));
            } 

            tiles = new Dictionary<string, TileRenderer>();
            foreach (var mapTile in ExtensionLoader.DefaultTypeMapper.MapTiles)
            {
                string path = Path.Combine(".", "Resources", mapTile.Type.Name + ".png");
                Bitmap bitmap = new Bitmap(Image.FromFile(path));
                tiles.Add(mapTile.Type.FullName, new TileRenderer(bitmap));
            }

            Timer timer = new Timer();
            timer.Interval = 20;
            timer.Tick += (s,e) => { Invalidate(); };
            timer.Enabled = true;

        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(Color.CornflowerBlue);

            if (DirtyBuffer)
                RedrawBuffer();

            if (buffer != null)
            {
                e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                e.Graphics.ResetTransform();
                e.Graphics.TranslateTransform(offset.X, offset.Y);
                e.Graphics.ScaleTransform(scale / TILEWIDTH, scale / TILEWIDTH);
                e.Graphics.DrawImageUnscaled(buffer, 0, 0);

                // Draw Highlighted Cell
                if (HighlightedCell.HasValue)
                    e.Graphics.DrawRectangle(highlightPen, new Rectangle(HighlightedCell.Value.X * TILEWIDTH, HighlightedCell.Value.Y * TILEWIDTH, TILEWIDTH, TILEWIDTH));

                // Draw hovered
                if (HoveredCell.HasValue)
                    e.Graphics.FillRectangle(hoverBrush, new Rectangle(HoveredCell.Value.X * TILEWIDTH, HoveredCell.Value.Y * TILEWIDTH, TILEWIDTH, TILEWIDTH));
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

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            if (e.Clicks == 1 && e.Button == MouseButtons.Left)
                OnApply(this, e);
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

                offset = new Vector2(
                    (ClientSize.Width - (mapSize.X * scale)) / 2,
                    (ClientSize.Height - (mapSize.Y * scale)) / 2);
            }
            Invalidate();
        }

        private void RedrawBuffer()
        {
            using (Graphics g = Graphics.FromImage(buffer))
            {
                var size = Map.GetCellCount();
                for (int y = 0; y < size.Y; y++)
                {
                    for (int x = 0; x < size.X; x++)
                    {
                        MapTile tile = Map[x, y];
                        if (tile == null) continue;

                        MaterialRenderer material;
                        if (materials.TryGetValue(tile.Material.GetType().FullName, out material))
                            material.Draw(g, x * TILEWIDTH, y * TILEWIDTH, Compass.East);

                        TileRenderer tileRenderer;
                        if (tiles.TryGetValue(tile.GetType().FullName, out tileRenderer))
                            tileRenderer.Draw(g, x * TILEWIDTH, y * TILEWIDTH, tile.Orientation);
                    }
                }
            }
        }

        public event EventHandler<MouseEventArgs> OnApply;
    }
}
