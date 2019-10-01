using System.Drawing;
using System.IO;
using System.Windows.Forms;
using AntMe;

namespace CoreTestClient.Tools
{
    public class SelectionTool : EditorTool
    {
        private Index2? selectedCell;

        private readonly ToolStripButton selectionButton;

        public SelectionTool(SimulationContext context) : base(context)
        {
            var path = Path.Combine(".", "Resources", "select.png");
            var image = Image.FromFile(path);
            selectionButton = new ToolStripButton("Select", image);
            selectionButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
            selectionButton.Click += (s, e) => { Select(); };
        }

        public Index2? SelectedCell
        {
            get => selectedCell;
            set
            {
                selectedCell = value;
                OnSelectedCellChanged?.Invoke(value);
            }
        }

        public override ToolStripItem RootItem => selectionButton;

        protected override void OnApply(Map map, Index2? cell, Vector2? position)
        {
            SelectedCell = cell;
        }

        public event ValueUpdate<Index2?> OnSelectedCellChanged;
    }
}