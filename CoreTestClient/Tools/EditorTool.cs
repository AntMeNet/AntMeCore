using AntMe;
using System;
using System.Windows.Forms;

namespace CoreTestClient.Tools
{
    public abstract class EditorTool
    {
        public abstract ToolStripItem RootItem { get; }

        public virtual bool CanApply(Map map, Index2 cell)
        {
            return map != null;
        }

        public void Apply(Map map, Index2 cell)
        {
            if (map == null)
                throw new ArgumentNullException("There is no map");

            Index2 mapSize = map.GetCellCount();
            if (cell.X < 0 || cell.X >= mapSize.X ||
                cell.Y < 0 || cell.Y >= mapSize.Y)
                throw new ArgumentOutOfRangeException("Cell Index is out of range");

            OnApply(map, cell);
        }

        protected abstract void OnApply(Map map, Index2 cell);

        protected void Select()
        {
            if (OnSelect != null)
                OnSelect(this, new EventArgs());
        }

        public event EventHandler OnSelect;
    }
}
