using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using AntMe;
using AntMe.Runtime;
using CoreTestClient.Renderer;

namespace CoreTestClient
{
    internal class EditorSceneControl : BaseSceneControl
    {
        private readonly Brush errorBrush;

        private readonly Brush hoverBrush;
        private Map map;

        private readonly Dictionary<string, TileRenderer> materials;

        private readonly Pen selectionFrame;

        private readonly Brush startPointBackground;

        private readonly Font startPointFont;

        private readonly Brush startPointFontBrush;

        private readonly Dictionary<string, TileRenderer> tiles;

        public EditorSceneControl()
        {
            ValidationExceptions = new List<Exception>();

            materials = new Dictionary<string, TileRenderer>();
            foreach (var material in ExtensionLoader.DefaultTypeMapper.MapMaterials)
            {
                var path = Path.Combine(".", "Resources", material.Type.Name + ".png");
                var bitmap = new Bitmap(Image.FromFile(path));
                materials.Add(material.Type.FullName, new TileRenderer(bitmap));
            }

            tiles = new Dictionary<string, TileRenderer>();
            foreach (var mapTile in ExtensionLoader.DefaultTypeMapper.MapTiles)
            {
                var path = Path.Combine(".", "Resources", mapTile.Type.Name + ".png");
                var bitmap = new Bitmap(Image.FromFile(path));
                tiles.Add(mapTile.Type.FullName, new TileRenderer(bitmap));
            }

            hoverBrush = new SolidBrush(Color.FromArgb(80, Color.White));
            errorBrush = new SolidBrush(Color.FromArgb(80, Color.Red));
            selectionFrame = new Pen(Color.White, 3f);
            startPointFont = new Font("Courier New", 10f);
            startPointFontBrush = new SolidBrush(Color.White);
            startPointBackground = new SolidBrush(Color.Black);
        }

        public List<Exception> ValidationExceptions { get; }

        public Index2? SelectedCell { get; set; }

        public Index2?[] StartPoints { get; set; }

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

        protected override void OnDraw(Graphics g)
        {
            // Draw hovered
            if (HoveredCell.HasValue)
                g.FillRectangle(hoverBrush,
                    new RectangleF(HoveredCell.Value.X * Map.CELLSIZE, HoveredCell.Value.Y * Map.CELLSIZE, Map.CELLSIZE,
                        Map.CELLSIZE));

            // Draw Selection
            if (SelectedCell.HasValue)
                g.DrawRectangle(selectionFrame,
                    new Rectangle(
                        (int) (SelectedCell.Value.X * Map.CELLSIZE),
                        (int) (SelectedCell.Value.Y * Map.CELLSIZE),
                        (int) Map.CELLSIZE,
                        (int) Map.CELLSIZE));

            // Draw Startpoints
            if (StartPoints != null)
                for (var i = 0; i < StartPoints.Length; i++)
                    if (StartPoints[i].HasValue)
                    {
                        var point = new PointF(
                            (StartPoints[i].Value.X + 0.5f) * Map.CELLSIZE,
                            (StartPoints[i].Value.Y + 0.5f) * Map.CELLSIZE);

                        var size = g.MeasureString((i + 1).ToString(), startPointFont);
                        g.FillRectangle(startPointBackground,
                            new RectangleF(point.X - size.Width / 2, point.Y - size.Height / 2, size.Width,
                                size.Height));
                        g.DrawString((i + 1).ToString(), startPointFont, startPointFontBrush, point.X - size.Width / 2,
                            point.Y - size.Height / 2);
                    }
        }

        #region Buffer Generation

        protected override TileRenderer OnRenderMaterial(int x, int y, out MapTileOrientation orientation)
        {
            orientation = MapTileOrientation.NotRotated;

            // No Map - no Renderer
            if (map == null)
                return null;

            var tile = map[x, y];

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

            var tile = map[x, y];

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
            var invalidCells = ValidationExceptions.OfType<InvalidMapTileException>().Select(e => e.CellIndex)
                .Distinct().ToList();

            for (var y = 0; y < mapSize.Y; y++)
            for (var x = 0; x < mapSize.X; x++)
                if (invalidCells.Contains(new Index2(x, y)))
                    g.FillRectangle(errorBrush, new Rectangle(x * TILEWIDTH, y * TILEWIDTH, TILEWIDTH, TILEWIDTH));
        }

        #endregion
    }
}