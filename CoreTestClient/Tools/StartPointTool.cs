using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using AntMe;

namespace CoreTestClient.Tools
{
    internal sealed class StartPointTool : EditorTool
    {
        private byte ActiveSlot;
        private readonly ToolStripDropDownButton button;

        private readonly ToolStripItem[] buttons;

        public StartPointTool(SimulationContext context) : base(context)
        {
            button = new ToolStripDropDownButton();
            button.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
            button.ToolTipText = "Start Point";
            button.Click += (s, e) => { Select(); };

            var path = Path.Combine(".", "Resources", "start.png");
            var image = Image.FromFile(path);
            button.Image = image;

            buttons = new ToolStripItem[Map.MAX_STARTPOINTS];
            StartPoints = new Index2?[Map.MAX_STARTPOINTS];
            for (byte i = 0; i < Map.MAX_STARTPOINTS; i++)
            {
                var b = button.DropDownItems.Add(string.Format("Slot {0}", i + 1));
                buttons[i] = b;
                b.Tag = i;
                b.Click += (s, e) => { SelectSlot((byte) b.Tag); };
            }

            SelectSlot(0);
        }

        public Index2?[] StartPoints { get; }

        public override ToolStripItem RootItem => button;

        private void SelectSlot(byte slot)
        {
            ActiveSlot = slot;
            button.Text = string.Format("Start Point {0}", ActiveSlot + 1);
        }

        protected override void OnApply(Map map, Index2? cell, Vector2? position)
        {
            if (StartPoints[ActiveSlot].HasValue &&
                cell.HasValue &&
                StartPoints[ActiveSlot].Value == cell.Value)
                StartPoints[ActiveSlot] = null;
            else
                StartPoints[ActiveSlot] = cell;

            map.StartPoints = StartPoints.Where(p => p.HasValue).Select(p => p.Value).ToArray();
        }
    }
}