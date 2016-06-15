using System;
using System.Windows.Forms;
using AntMe;
using AntMe.Runtime;
using System.IO;
using System.Drawing;

namespace CoreTestClient.Tools
{
    public class MaterialTool : EditorTool
    {
        private ToolStripDropDownButton button;

        private ToolStripItem selected;

        public override ToolStripItem RootItem { get { return button; } }

        public MaterialTool()
        {
            button = new ToolStripDropDownButton();
            button.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
            button.Text = "Material";
            button.ToolTipText = "Material";
            button.Click += (s, e) => { Select(); };

            // Init Tools
            foreach (var material in ExtensionLoader.DefaultTypeMapper.MapMaterials)
            {
                string path = Path.Combine(".", "Resources", material.Type.Name + ".png");
                Image image = Image.FromFile(path);

                ToolStripItem b = button.DropDownItems.Add(material.Name, image);
                b.Tag = material;
                b.Click += (s, e) => { SelectMaterial(b); };

                // Set Default (Gras)
                if (material.Name == "Gras Material")
                    SelectMaterial(button);
            }
        }

        private void SelectMaterial(ToolStripItem material)
        {
            selected = material;
            button.Image = material.Image;
            button.Text = material.Text;
            Select();
        }

        public override bool CanApply(Map map, Index2 cell)
        {
            return (map != null && map[cell.X, cell.Y] != null);
        }

        protected override void OnApply(Map map, Index2 cell)
        {
            if (map[cell.X, cell.Y] == null)
                throw new NotSupportedException("There is no Map Type at this point");

            //if (SelectedMaterial != null)
            //    map[cell.X, cell.Y].Material = Activator.CreateInstance(SelectedMaterial) as MapMaterial;
            //else
            //    map[cell.X, cell.Y].Material = null;
        }
    }
}
