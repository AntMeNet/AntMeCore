using System.Windows.Forms;
using AntMe;
using System.Drawing;
using System.IO;
using System.Linq;

namespace CoreTestClient.Tools
{
    internal sealed class StartPointTool : EditorTool
    {
        private ToolStripDropDownButton button;

        private byte ActiveSlot;

        private ToolStripItem[] buttons;

        public Index2?[] StartPoints { get; private set; }

        public StartPointTool(SimulationContext context) : base(context)
        {
            button = new ToolStripDropDownButton();
            button.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
            button.ToolTipText = "Start Point";
            button.Click += (s, e) => { Select(); };

            string path = Path.Combine(".", "Resources", "start.png");
            Image image = Image.FromFile(path);
            button.Image = image;

            buttons = new ToolStripItem[Map.MaxStartpoints];
            StartPoints = new Index2?[Map.MaxStartpoints];
            for (byte i = 0; i < Map.MaxStartpoints; i++)
            {
                ToolStripItem b = button.DropDownItems.Add(string.Format("Slot {0}", i + 1));
                buttons[i] = b;
                b.Tag = i;
                b.Click += (s, e) =>
                {
                    SelectSlot((byte)b.Tag);
                };
            }

            SelectSlot(0);
        }

        private void SelectSlot(byte slot)
        {
            ActiveSlot = slot;
            button.Text = string.Format("Start Point {0}", ActiveSlot + 1);
        }

        public override ToolStripItem RootItem
        {
            get { return button; }
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
