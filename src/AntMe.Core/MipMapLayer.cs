using System;
using System.Collections.Generic;

namespace AntMe
{
    /// <summary>
    /// Repräsentiert einen Layer innerhalb eines MipMap-Containers.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class MipMapLayer<T>
    {
        /// <summary>
        ///     The grid of lists of expanded objects (T objects with position and radius) in this layer.
        ///     Is used to store the objects of different radii.
        /// </summary>
        private List<ExpandedObject<T>>[,] _grid;

        /// <summary>
        /// Neue Instanz eines MipMap Layers.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="maxRadius"></param>
        public MipMapLayer(float width, float height, float maxRadius)
        {
            Width = width;
            Height = height;
            MaxRadius = maxRadius;

            CheckValues();
            Initialize();
        }

        /// <summary>
        ///     The radius above which this layer rejects objects because they are too big for the cells.
        /// </summary>
        public float MaxRadius { get; private set; }

        /// <summary>
        ///     Height of the layer.
        /// </summary>
        public float Height { get; private set; }

        public int HeightCells { get; private set; }

        /// <summary>
        ///     Width of the layer.
        /// </summary>
        public float Width { get; private set; }

        public int WidthCells { get; private set; }

        /// <summary>
        ///     Width of one cell in this layer.
        /// </summary>
        public float CellWidth
        {
            get { return Width / WidthCells; }
        }

        /// <summary>
        ///     Height of one cell in this layer.
        /// </summary>
        public float CellHeight
        {
            get { return Height / HeightCells; }
        }

        /// <summary>
        ///     Checks the dimensions of the layer and throws an ArgumentException if they are not positive.
        /// </summary>
        private void CheckValues()
        {
            if (Width < Vector2.EpsMin)
                throw new ArgumentException("A MipMapLayer must have a positive width.");
            if (Height < Vector2.EpsMin)
                throw new ArgumentException("A MipMapLayer must have a positive height.");
        }

        /// <summary>
        ///     Initializes the grid that comprises this layer with empty lists on every cell.
        /// </summary>
        private void Initialize()
        {
            HeightCells = (int)Math.Max(1.0, Height / Math.Min(Height, Math.Max(1.0, MaxRadius)));
            WidthCells = (int)Math.Max(1.0, Width / Math.Min(Width, Math.Max(1.0, MaxRadius)));

            _grid = new List<ExpandedObject<T>>[HeightCells, WidthCells];
            // initialize the cells with an empty list each

            for (int row = 0; row < HeightCells; row++)
            {
                for (int col = 0; col < WidthCells; col++)
                {
                    _grid[row, col] = new List<ExpandedObject<T>>();
                }
            }
        }

        /// <summary>
        ///     Removes all objects from this layer.
        /// </summary>
        public void Clear()
        {
            for (int row = 0; row < HeightCells; row++)
            {
                for (int col = 0; col < WidthCells; col++)
                {
                    _grid[row, col].Clear();
                }
            }
        }

        /// <summary>
        ///     Adds an object to this layer.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="pos">Position of the object relative to the left upper corner of this layer.</param>
        /// <param name="radius"></param>
        public void Add(T obj, Vector3 pos, float radius)
        {
            int col = FloatToCellIndex(pos.X, Width, WidthCells);
            int row = FloatToCellIndex(pos.Y, Height, HeightCells);
            var cells = (int)Math.Ceiling(radius / CellWidth);

            // Run through all cells in the vincinity of the given position, add the object in every one of these cells
            /*for (int runrow = row - cells; runrow < row + cells; runrow++)
            {
                for (int runcol = col - cells; runcol < col + cells; runcol++)
                {
                    if (ValidCellIndex(runrow, runcol))
                    {*/
            _grid[row, col].Add(new ExpandedObject<T>(obj, pos, radius));
            /*         }
                }
            }*/
        }

        /// <summary>
        ///     Find objects that intersect the given ball with their expansion (pos, radius).
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="radius"></param>
        /// <returns>list of all objects found intersecting the given ball.</returns>
        public HashSet<T> FindAll(Vector3 pos, float radius)
        {
            var result = new HashSet<T>();
            int col = FloatToCellIndex(pos.X, Width, WidthCells);
            int row = FloatToCellIndex(pos.Y, Height, HeightCells);
            var cells = (int)Math.Min(int.MaxValue, Math.Ceiling((radius + MaxRadius) / CellWidth));

            // Run through all cells in the vincinity of the given position
            for (int runrow = Math.Max(0, row - cells); runrow < Math.Min(row + cells, HeightCells); runrow++)
            {
                for (int runcol = Math.Max(0, col - cells); runcol < Math.Min(col + cells, WidthCells); runcol++)
                {
                    foreach (var item in (List<ExpandedObject<T>>)_grid.GetValue(runrow, runcol))
                    {
                        // Check the intersection with every object in this cell
                        float radiuses = radius + item.radius;
                        float x = item.pos.X - pos.X;
                        float y = item.pos.Y - pos.Y;
                        float z = item.pos.Z - pos.Z;
                        float distance = (x * x) + (y * y) + (z * z);
                        if (distance <= radiuses * radiuses)
                        {
                            result.Add(item.obj);
                        }
                    }
                }
            }
            return result;
        }

        /// <summary>
        ///     Clamps the given float part of a position to a cell index.
        /// </summary>
        /// <param name="f"></param>
        /// <param name="length"></param>
        /// <param name="cells"></param>
        /// <returns></returns>
        private int FloatToCellIndex(float f, float length, int cells)
        {
            var index = (int)(f / length * cells);
            index = Math.Max(0, index);
            index = Math.Min(cells - 1, index);

            return index;
        }

        /// <summary>
        ///     Removes an object at a given position
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="pos"></param>
        internal void Remove(T obj, Vector3 pos)
        {
            int col = FloatToCellIndex(pos.X, Width, WidthCells);
            int row = FloatToCellIndex(pos.Y, Height, HeightCells);

            // try to find the radius of the given object and remove all linked objects in the adjacent cells
            /*List<ExpandedObject<T>> objs = _grid[row, col];
            List<ExpandedObject<T>> filter = objs.FindAll(expobj => expobj.obj.Equals(obj));
            if (filter.Count > 0)
            {
                int cells = (int)Math.Ceiling(filter[0].radius / CellWidth);

                for (int runrow = row - cells; runrow < row + cells; runrow++)
                {
                    for (int runcol = col - cells; runcol < col + cells; runcol++)
                    {
                        if (ValidCellIndex(runrow, runcol))
                        {*/
            _grid[row, col].RemoveAll(expobj => expobj.obj.Equals(obj));
            /*             }
                    }
                }
            }*/
        }
    }
}