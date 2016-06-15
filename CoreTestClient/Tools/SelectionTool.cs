using System;
using System.Windows.Forms;
using AntMe;
using System.Drawing;
using System.IO;

namespace CoreTestClient.Tools
{
    public class SelectionTool : EditorTool
    {
        private ToolStripButton selectionButton;

        public override ToolStripItem RootItem { get { return selectionButton; } }

        public SelectionTool(SimulationContext context) : base(context)
        {
            string path = Path.Combine(".", "Resources", "select.png");
            Image image = Image.FromFile(path);
            selectionButton = new ToolStripButton("Select", image);
            selectionButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
            selectionButton.Click += (s, e) => { Select(); };
        }

        protected override void OnApply(Map map, Index2 cell)
        {
        }
    }
}
