using AntMe;
using System;
using System.Windows.Forms;

namespace CoreTestClient.Tools
{
    public abstract class EditorTool
    {
        protected readonly SimulationContext Context;

        public abstract ToolStripItem RootItem { get; }

        public EditorTool(SimulationContext context)
        {
            Context = context;
        }

        public virtual bool CanApply(Map map, Index2? cell, Vector2? position)
        {
            return map != null;
        }

        public void Apply(Map map, Index2? cell, Vector2? position)
        {
            if (map == null)
                throw new ArgumentNullException("There is no map");

            Index2 mapSize = map.GetCellCount();
            if (cell.HasValue && 
                (cell.Value.X < 0 || cell.Value.X >= mapSize.X ||
                cell.Value.Y < 0 || cell.Value.Y >= mapSize.Y))
                throw new ArgumentOutOfRangeException("Cell Index is out of range");

            if (position.HasValue && 
                (position.Value.X < 0 || position.Value.X >= mapSize.X * Map.Cellsize ||
                position.Value.Y < 0 || position.Value.Y >= mapSize.Y * Map.Cellsize))
                throw new ArgumentOutOfRangeException("Position is out of range");

            OnApply(map, cell, position);
        }

        protected abstract void OnApply(Map map, Index2? cell, Vector2? position);

        protected void Select()
        {
            OnSelect?.Invoke(this, new EventArgs());
        }

        public event EventHandler OnSelect;
    }
}
