using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using AntMe;

namespace CoreTestClient.Tools
{
    public class MapTileTool : EditorTool
    {
        private readonly ToolStripDropDownButton button;

        private ToolStripItem selected;

        public MapTileTool(SimulationContext context) : base(context)
        {
            button = new ToolStripDropDownButton();
            button.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
            button.Text = "Map Tile";
            button.ToolTipText = "Map Tile";
            button.Click += (s, e) => { Select(); };

            foreach (var mapTile in Context.Mapper.MapTiles)
            {
                var path = Path.Combine(".", "Resources", mapTile.Type.Name + ".png");
                var image = Image.FromFile(path);

                var b = button.DropDownItems.Add(mapTile.Name, image);
                b.Tag = mapTile;
                b.Click += (s, e) => { SelectMapTile(b); };

                // Set Default (Gras)
                if (mapTile.Name == "Flat Map Tile")
                    SelectMapTile(b);
            }

            // Fallback -> take first
            if (selected == null)
                SelectMapTile(button.DropDownItems[0]);
        }

        public override ToolStripItem RootItem => button;

        private void SelectMapTile(ToolStripItem mapTile)
        {
            selected = mapTile;
            button.Image = mapTile.Image;
            button.Text = mapTile.Text;
            Select();
        }

        protected override void OnApply(Map map, Index2? cell, Vector2? position)
        {
            if (!cell.HasValue)
                return;

            var tile = map[cell.Value.X, cell.Value.Y];
            var material = tile?.Material;

            if (selected == null)
                throw new NotSupportedException("No Map Tile selected");

            var mapTile = selected.Tag as IStateInfoTypeMapperEntry;

            if (tile == null || tile.GetType() != mapTile.Type)
            {
                // Create a new Map Tile
                map[cell.Value.X, cell.Value.Y] = Activator.CreateInstance(mapTile.Type, Context) as MapTile;
                map[cell.Value.X, cell.Value.Y].HeightLevel = tile != null ? tile.HeightLevel : map.BaseLevel;
                map[cell.Value.X, cell.Value.Y].Material = material;
            }
            else if (tile.GetType() == mapTile.Type)
            {
                // Rotate 90 Degrees
                tile.Orientation = (MapTileOrientation) (((int) tile.Orientation + 90) % 360);
            }
        }
    }
}