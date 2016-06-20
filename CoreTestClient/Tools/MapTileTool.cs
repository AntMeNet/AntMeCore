using System;
using System.Windows.Forms;
using AntMe;
using AntMe.Runtime;
using System.Drawing;
using System.IO;

namespace CoreTestClient.Tools
{
    public class MapTileTool : EditorTool
    {
        private ToolStripDropDownButton button;

        private ToolStripItem selected;

        public override ToolStripItem RootItem { get { return button; } }

        public MapTileTool(SimulationContext context) : base(context)
        {
            button = new ToolStripDropDownButton();
            button.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
            button.Text = "Map Tile";
            button.ToolTipText = "Map Tile";
            button.Click += (s, e) => { Select(); };

            foreach (var mapTile in Context.Mapper.MapTiles)
            {
                string path = Path.Combine(".", "Resources", mapTile.Type.Name + ".png");
                Image image = Image.FromFile(path);

                ToolStripItem b = button.DropDownItems.Add(mapTile.Name, image);
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

            MapTile tile = map[cell.Value.X, cell.Value.Y];
            MapMaterial material = null;
            if (tile != null)
                material = tile.Material;

            if (selected == null)
                throw new NotSupportedException("No Map Tile selected");

            IStateInfoTypeMapperEntry mapTile = selected.Tag as IStateInfoTypeMapperEntry;

            if (tile == null || tile.GetType() != mapTile.Type)
            {
                // Create a new Map Tile
                map[cell.Value.X, cell.Value.Y] = Activator.CreateInstance(mapTile.Type, Context) as MapTile;
                map[cell.Value.X, cell.Value.Y].Material = material;
            }
            else if (tile.GetType() == mapTile.Type)
            {
                // Rotate 90 Degrees
                tile.Orientation = (MapTileOrientation)(((int)tile.Orientation + 90) % 360);
            }
        }
    }
}
