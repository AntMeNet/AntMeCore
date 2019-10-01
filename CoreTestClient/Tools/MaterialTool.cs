using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using AntMe;

namespace CoreTestClient.Tools
{
    public class MaterialTool : EditorTool
    {
        private readonly ToolStripDropDownButton button;

        private ToolStripItem selected;

        public MaterialTool(SimulationContext context) : base(context)
        {
            button = new ToolStripDropDownButton();
            button.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
            button.Text = "Material";
            button.ToolTipText = "Material";
            button.Click += (s, e) => { Select(); };

            // Init Tools
            foreach (var material in Context.Mapper.MapMaterials)
            {
                var path = Path.Combine(".", "Resources", material.Type.Name + ".png");
                var image = Image.FromFile(path);

                var b = button.DropDownItems.Add(material.Name, image);
                b.Tag = material;
                b.Click += (s, e) => { SelectMaterial(b); };

                // Set Default (Gras)
                if (material.Name == "Gras Material")
                    SelectMaterial(b);
            }
        }

        public override ToolStripItem RootItem => button;

        private void SelectMaterial(ToolStripItem material)
        {
            selected = material;
            button.Image = material.Image;
            button.Text = material.Text;
            Select();
        }

        public override bool CanApply(Map map, Index2? cell, Vector2? position)
        {
            if (!cell.HasValue)
                return false;

            return map?[cell.Value.X, cell.Value.Y] != null;
        }

        protected override void OnApply(Map map, Index2? cell, Vector2? position)
        {
            if (!cell.HasValue)
                return;

            if (map[cell.Value.X, cell.Value.Y] == null)
                throw new NotSupportedException("There is no Map Type at this point");

            var tile = map[cell.Value.X, cell.Value.Y];

            if (selected != null)
            {
                var material = selected.Tag as ITypeMapperEntry;
                if (tile.Material == null || tile.Material.GetType() != material.Type)
                    map[cell.Value.X, cell.Value.Y].Material =
                        Activator.CreateInstance(material.Type, Context) as MapMaterial;
            }
            else
            {
                map[cell.Value.X, cell.Value.Y].Material = null;
            }
        }
    }
}