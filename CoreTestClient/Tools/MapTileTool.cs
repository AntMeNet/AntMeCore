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

        public MapTileTool()
        {
            button = new ToolStripDropDownButton();
            button.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
            button.Text = "Map Tile";
            button.ToolTipText = "Map Tile";
            button.Click += (s, e) => { Select(); };

            foreach (var mapTile in ExtensionLoader.DefaultTypeMapper.MapTiles)
            {
                string path = Path.Combine(".", "Resources", mapTile.Type.Name + ".png");
                Image image = Image.FromFile(path);

                ToolStripItem b = button.DropDownItems.Add(mapTile.Name, image);
                b.Tag = mapTile;
                b.Click += (s, e) => { SelectMapTile(b); };

                // Set Default (Gras)
                if (mapTile.Name == "Flat Map Tile")
                    SelectMapTile(button);
            }
        }

        private void SelectMapTile(ToolStripItem mapTile)
        {
            selected = mapTile;
            button.Image = mapTile.Image;
            button.Text = mapTile.Text;
            Select();
        }

        protected override void OnApply(Map map, Index2 cell)
        {
            throw new NotImplementedException();
        }
    }
}
