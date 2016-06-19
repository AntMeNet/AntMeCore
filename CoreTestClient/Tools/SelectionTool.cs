using System.Windows.Forms;
using AntMe;
using System.Drawing;
using System.IO;

namespace CoreTestClient.Tools
{
    public class SelectionTool : EditorTool
    {
        private Index2? selectedCell;

        private ToolStripButton selectionButton;

        public Index2? SelectedCell
        {
            get { return selectedCell; }
            set
            {
                selectedCell = value;
                if (OnSelectedCellChanged != null)
                    OnSelectedCellChanged(value);
            }
        }

        public override ToolStripItem RootItem { get { return selectionButton; } }

        public SelectionTool(SimulationContext context) : base(context)
        {
            string path = Path.Combine(".", "Resources", "select.png");
            Image image = Image.FromFile(path);
            selectionButton = new ToolStripButton("Select", image);
            selectionButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
            selectionButton.Click += (s, e) => { Select(); };
        }

        protected override void OnApply(Map map, Index2? cell, Vector2? position)
        {
            SelectedCell = cell;
        }

        public event ValueUpdate<Index2?> OnSelectedCellChanged;
    }
}
