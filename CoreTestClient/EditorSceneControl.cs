using AntMe;
using CoreTestClient.Renderer;
using System.Collections.Generic;
using AntMe.Runtime;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System;
using System.Linq;

namespace CoreTestClient
{
    internal class EditorSceneControl : BaseSceneControl
    {
        private Map map;

        private Dictionary<string, TileRenderer> materials;

        private Dictionary<string, TileRenderer> tiles;

        private Brush hoverBrush;

        private Brush errorBrush;

        private Pen selectionFrame;

        private Font startPointFont;

        private Brush startPointBackground;

        private Brush startPointFontBrush;

        public List<Exception> ValidationExceptions { get; private set; }

        public Index2? SelectedCell { get; set; }

        public Index2?[] StartPoints { get; set; }

        public EditorSceneControl()
        {
            ValidationExceptions = new List<Exception>();

            materials = new Dictionary<string, TileRenderer>();
            foreach (var material in ExtensionLoader.DefaultTypeMapper.MapMaterials)
            {
                string path = Path.Combine(".", "Resources", material.Type.Name + ".png");
                Bitmap bitmap = new Bitmap(Image.FromFile(path));
                materials.Add(material.Type.FullName, new TileRenderer(bitmap));
            }

            tiles = new Dictionary<string, TileRenderer>();
            foreach (var mapTile in ExtensionLoader.DefaultTypeMapper.MapTiles)
            {
                string path = Path.Combine(".", "Resources", mapTile.Type.Name + ".png");
                Bitmap bitmap = new Bitmap(Image.FromFile(path));
                tiles.Add(mapTile.Type.FullName, new TileRenderer(bitmap));
            }

            hoverBrush = new SolidBrush(Color.FromArgb(80, Color.White));
            errorBrush = new SolidBrush(Color.FromArgb(80, Color.Red));
            selectionFrame = new Pen(Color.White, 3f);
            startPointFont = new Font("Courier New", 10f);
            startPointFontBrush = new SolidBrush(Color.White);
            startPointBackground = new SolidBrush(Color.Black);
        }

        public void SetMap(Map map)
        {
            if (this.map != map)
            {
                this.map = map;
                SetMapSize(map?.GetCellCount() ?? Index2.Zero);
            }
        }

        public void Revalidate()
        {
            ValidationExceptions.Clear();
            if (map != null)
            {
                try
                {
                    map.ValidateMap();
                }
                catch (AggregateException ex)
                {
                    ValidationExceptions.AddRange(ex.InnerExceptions);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        protected override void OnDraw(Graphics g)
        {
            // Draw hovered
            if (HoveredCell.HasValue)
                g.FillRectangle(hoverBrush,
                    new RectangleF(HoveredCell.Value.X * Map.Cellsize, HoveredCell.Value.Y * Map.Cellsize, Map.Cellsize, Map.Cellsize));

            // Draw Selection
            if (SelectedCell.HasValue)
            {
                g.DrawRectangle(selectionFrame, 
                    new Rectangle(
                        (int)(SelectedCell.Value.X * Map.Cellsize), 
                        (int)(SelectedCell.Value.Y * Map.Cellsize), 
                        (int)Map.Cellsize, 
                        (int)Map.Cellsize));
            }

            // Draw Startpoints
            if (StartPoints != null)
            {
                for (int i = 0; i < StartPoints.Length; i++)
                {
                    if (StartPoints[i].HasValue)
                    {
                        PointF point = new PointF(
                            ((StartPoints[i].Value.X + 0.5f) * Map.Cellsize), 
                            ((StartPoints[i].Value.Y + 0.5f) * Map.Cellsize));

                        SizeF size = g.MeasureString((i + 1).ToString(), startPointFont);
                        g.FillRectangle(startPointBackground, new RectangleF(point.X - (size.Width / 2), point.Y - (size.Height / 2), size.Width, size.Height));
                        g.DrawString((i + 1).ToString(), startPointFont, startPointFontBrush, point.X - (size.Width / 2), point.Y - (size.Height / 2));
                    }
                }
            }

        }

        #region Buffer Generation

        protected override TileRenderer OnRenderMaterial(int x, int y, out MapTileOrientation orientation)
        {
            orientation = MapTileOrientation.NotRotated;

            // No Map - no Renderer
            if (map == null)
                return null;

            MapTile tile = map[x, y];

            // No Tile or Material - no Renderer
            if (tile == null || tile.Material == null)
                return null;

            TileRenderer renderer;
            orientation = tile.Orientation;
            if (materials.TryGetValue(tile.Material.GetType().FullName, out renderer))
                return renderer;

            return null;            
        }

        protected override TileRenderer OnRenderTile(int x, int y, out MapTileOrientation orientation)
        {
            orientation = MapTileOrientation.NotRotated;

            // No Map - no Renderer
            if (map == null)
                return null;

            MapTile tile = map[x, y];

            // No Tile - no Renderer
            if (tile == null)
                return null;

            TileRenderer renderer;
            orientation = tile.Orientation;
            if (tiles.TryGetValue(tile.GetType().FullName, out renderer))
                return renderer;

            return null;
        }

        protected override void OnBufferDraw(Graphics g, Index2 mapSize)
        {
            List<Index2> invalidCells = ValidationExceptions.OfType<InvalidMapTileException>().Select(e => e.CellIndex).Distinct().ToList();

            for (int y = 0; y < mapSize.Y; y++)
            {
                for (int x = 0; x < mapSize.X; x++)
                {
                    if (invalidCells.Contains(new Index2(x, y)))
                        g.FillRectangle(errorBrush, new Rectangle(x * TILEWIDTH, y * TILEWIDTH, TILEWIDTH, TILEWIDTH));
                }
            }
        }

        #endregion
    }
}
