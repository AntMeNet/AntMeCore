using AntMe;
using CoreTestClient.Renderer;
using System.Drawing;
using System.Windows.Forms;
using System;

namespace CoreTestClient
{
    internal abstract class BaseSceneControl : Control
    {
        /// <summary>
        /// Gets the Size of a cell in Pixel for the Buffer.
        /// </summary>
        protected const int TILEWIDTH = 64;

        /// <summary>
        /// Gets the Scale Factor for the Buffer.
        /// </summary>
        private const float BUFFER_SCALE = Map.CELLSIZE / TILEWIDTH;

        /// <summary>
        /// Amount of Pixel between Control Border and Map Border.
        /// </summary>
        protected const int BORDER = 20;

        private float minCameraScale = 1f / 50;
        private float maxCameraScale = 1000f;
        private float cameraScale = 1f;
        private Vector2 cameraPosition = Vector2.Zero;
        private Vector2? hoveredPosition = null;
        private Index2? hoveredCell = null;
        private Brush emptyBrush;

        /// <summary>
        /// Gets the minimum Scale Level.
        /// </summary>
        public float MinCameraScale
        {
            get { return minCameraScale; }
            private set
            {
                minCameraScale = Math.Max(1f / 50, value);
                CameraScale = CameraScale;
            }
        }

        /// <summary>
        /// Gets the maximum Scale Level.
        /// </summary>
        public float MaxCameraScale
        {
            get { return maxCameraScale; }
            private set
            {
                maxCameraScale = Math.Min(1000f, value);
                CameraScale = CameraScale;
            }
        }

        /// <summary>
        /// Gets the current Scale Level.
        /// </summary>
        public float CameraScale
        {
            get { return cameraScale; }
            protected set
            {
                cameraScale = Math.Min(MaxCameraScale, Math.Max(MinCameraScale, value));
            }
        }

        /// <summary>
        /// Gets the Position of the Camera in World Coordinate
        /// </summary>
        public Vector2 CameraPosition
        {
            get { return cameraPosition; }
            private set
            {
                // Check against Borders
                float width = ((ClientRectangle.Width - BORDER) / 2) / CameraScale;
                if (width * 2 >= mapSize.X * Map.CELLSIZE)
                {
                    // Map smaller than the Screen
                    value.X = mapSize.X * Map.CELLSIZE / 2;
                }
                else
                {
                    // Too far left
                    if (value.X < width)
                        value.X = width;

                    // Too far right
                    if (value.X > (mapSize.X * Map.CELLSIZE) - width)
                        value.X = (mapSize.X * Map.CELLSIZE) - width;
                }

                float height = ((ClientRectangle.Height - BORDER) / 2) / CameraScale;
                if (height * 2 >= mapSize.Y * Map.CELLSIZE)
                {
                    // Map smaller than the Screen
                    value.Y = mapSize.Y * Map.CELLSIZE / 2;
                }
                else
                {
                    // Too far up
                    if (value.Y < height)
                        value.Y = height;

                    // Too far down
                    if (value.Y > (mapSize.Y * Map.CELLSIZE) - height)
                        value.Y = (mapSize.Y * Map.CELLSIZE) - height;
                }

                cameraPosition = value;
            }
        }

        private Bitmap buffer;
        private Index2 mapSize;
        private bool dirtyBuffer;

        /// <summary>
        /// Gets the current Position of the Mouse in World Coordinates.
        /// </summary>
        public Vector2? HoveredPosition
        {
            get { return hoveredPosition; }
            private set
            {
                hoveredPosition = value;
                if (OnHoveredPositionChanged != null)
                    OnHoveredPositionChanged(value);
            }
        }

        /// <summary>
        /// Gets the current Cell of the Mouse Pointer.
        /// </summary>
        public Index2? HoveredCell {
            get { return hoveredCell; }
            private set
            {
                if (hoveredCell != value && OnHoveredCellChanged != null)
                {
                    hoveredCell = value;
                    OnHoveredCellChanged(value);
                }
            }
        }

        public BaseSceneControl()
        {
            DoubleBuffered = true;
            emptyBrush = new SolidBrush(Color.LightGray);
        }

        #region Scaling and Navigation

        bool mouseDragging = false;
        Vector2 mousePosition = Vector2.Zero;
        Vector2 worldPosition = Vector2.Zero;

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            RecalcMinCameraScale();
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);

            Focus();
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);

            CameraScale += e.Delta * 0.0005f * cameraScale;
            CameraPosition = CameraPosition;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (e.Button == MouseButtons.Right)
            {
                mouseDragging = true;
                SetMousePosition(e.X, e.Y);
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            if (e.Button == MouseButtons.Right)
                mouseDragging = false;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (mouseDragging)
            {
                // Camera move
                int deltaX = (int)mousePosition.X - e.X;
                int deltaY = (int)mousePosition.Y - e.Y;
                CameraPosition += new Vector2(deltaX, deltaY) / CameraScale;
                Invalidate();
            }

            SetMousePosition(e.X, e.Y);
        }

        private void RecalcMinCameraScale()
        {
            if (mapSize != Index2.Zero)
            {
                float scaleX = (ClientSize.Width - BORDER) / (mapSize.X * Map.CELLSIZE);
                float scaleY = (ClientSize.Height - BORDER) / (mapSize.Y * Map.CELLSIZE);
                MinCameraScale = Math.Min(scaleX, scaleY);
                CameraPosition = CameraPosition;
            }
            else
            {
                MinCameraScale = 1f;
            }
        }

        /// <summary>
        /// Converts Screen- to World-Coordinates.
        /// </summary>
        /// <param name="position">Screen Position</param>
        /// <returns>World Position</returns>
        protected Vector2 ViewToWorld(Vector2 position)
        {
            Vector2 relativePosition = position -
                (new Vector2(ClientSize.Width, ClientSize.Height) / 2f);

            return (relativePosition / CameraScale) + CameraPosition;
        }

        /// <summary>
        /// Converts World- to Screen-Coordinates.
        /// </summary>
        /// <param name="position">World Position</param>
        /// <returns>Screen Position</returns>
        protected Vector2 WorldToView(Vector2 position)
        {
            Vector2 relativePosition = position - CameraPosition;
            return (relativePosition * CameraScale) + (new Vector2(ClientSize.Width, ClientSize.Height) / 2f);
        }

        private void SetMousePosition(int x, int y)
        {
            mousePosition = new Vector2(x, y);
            worldPosition = ViewToWorld(mousePosition);

            if (worldPosition.X < 0f || worldPosition.X >= mapSize.X * Map.CELLSIZE ||
                worldPosition.Y < 0f || worldPosition.Y >= mapSize.Y * Map.CELLSIZE)
            {
                HoveredPosition = null;
                HoveredCell = null;
            }
            else
            {
                HoveredPosition = worldPosition;
                HoveredCell = new Index2(
                    (int)(worldPosition.X / Map.CELLSIZE),
                    (int)(worldPosition.Y / Map.CELLSIZE));
            }
        }

        #endregion

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.Clear(Color.CornflowerBlue);

            if (buffer != null)
            {
                // Render Playground
                if (dirtyBuffer)
                {
                    using (Graphics g = Graphics.FromImage(buffer))
                    {
                        for (int y = 0; y < mapSize.Y; y++)
                        {
                            for (int x = 0; x < mapSize.X; x++)
                            {
                                MapTileOrientation orientation;
                                TileRenderer materialRenderer = OnRenderMaterial(x, y, out orientation);
                                if (materialRenderer != null)
                                    materialRenderer.Draw(g, x * TILEWIDTH, y * TILEWIDTH, orientation);
                                else
                                {
                                    // Fallback on empty Cells
                                    g.FillRectangle(emptyBrush, new RectangleF(x * TILEWIDTH, y * TILEWIDTH, TILEWIDTH, TILEWIDTH));
                                }

                                TileRenderer tileRenderer = OnRenderTile(x, y, out orientation);
                                if (tileRenderer != null)
                                    tileRenderer.Draw(g, x * TILEWIDTH, y * TILEWIDTH, orientation);

                                
                            }
                        }

                        OnBufferDraw(g, mapSize);
                    }

                    dirtyBuffer = false;
                }

                e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

                // Draw Buffer
                e.Graphics.ResetTransform();
                e.Graphics.TranslateTransform(ClientSize.Width / 2, ClientSize.Height / 2);
                e.Graphics.ScaleTransform(CameraScale, CameraScale);
                e.Graphics.TranslateTransform(-CameraPosition.X, -CameraPosition.Y);
                e.Graphics.ScaleTransform(BUFFER_SCALE, BUFFER_SCALE);
                e.Graphics.DrawImageUnscaled(buffer, 0, 0);
            }

            // Draw overlaying stuff
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(ClientSize.Width / 2, ClientSize.Height / 2);
            e.Graphics.ScaleTransform(CameraScale, CameraScale);
            e.Graphics.TranslateTransform(-CameraPosition.X, -CameraPosition.Y);
            OnDraw(e.Graphics);
        }

        protected virtual void OnBufferDraw(Graphics g, Index2 mapSize) { }

        protected abstract void OnDraw(Graphics g);

        #region Buffer Generation

        protected void SetMapSize(Index2 size)
        {
            // Dispose old Buffer (Size does not fit)
            if (buffer != null && size != mapSize)
            {
                buffer.Dispose();
                buffer = null;
            }

            mapSize = size;

            // No Map available
            if (size == Index2.Zero)
            {
                CameraPosition = Vector2.Zero;
                return;
            }

            // Check for valid Map Size
            if (size.X < Map.MIN_WIDTH || size.X > Map.MAX_WIDTH ||
                size.Y < Map.MIN_HEIGHT || size.Y > Map.MAX_HEIGHT)
                throw new ArgumentOutOfRangeException("size");

            // Recreate a new Buffer
            if (buffer == null)
            {
                mapSize = size;
                buffer = new Bitmap(mapSize.X * TILEWIDTH, mapSize.Y * TILEWIDTH);
            }

            // Redraw Map
            InvalidateMap();

            // Reset Camera
            CameraPosition = new Vector2(size.X, size.Y) * Map.CELLSIZE * 0.5f;
            RecalcMinCameraScale();
            CameraScale = MinCameraScale;
        }

        public void InvalidateMap()
        {
            dirtyBuffer = true;
            Invalidate();
        }

        protected abstract TileRenderer OnRenderMaterial(int x, int y, out MapTileOrientation orientation);

        protected abstract TileRenderer OnRenderTile(int x, int y, out MapTileOrientation orientation);

        #endregion

        public event ValueUpdate<Index2?> OnHoveredCellChanged;

        public event ValueUpdate<Vector2?> OnHoveredPositionChanged;
    }
}
