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
        private const int TILEWIDTH = 64;

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
                cameraPosition = value;
            }
        }

        private Bitmap buffer;
        private Index2 mapSize;
        private bool dirtyBuffer;

        public Index2? HoveredCell { get; private set; }

        public BaseSceneControl()
        {
            DoubleBuffered = true;
        }

        #region Scaling and Navigation

        bool mouseDragging = false;
        int mousePositionX = 0;
        int mousePositionY = 0;
        float worldPositionX = 0f;
        float worldPositionY = 0f;

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            RecalcMinCameraScale();
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);

            CameraScale += e.Delta * 0.0005f * cameraScale;
            Invalidate();
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
                int deltaX = mousePositionX - e.X;
                int deltaY = mousePositionY - e.Y;
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

        private void SetMousePosition(int x, int y)
        {
            mousePositionX = x;
            mousePositionY = y;

            // Calc Camera Relative Position
            int relX = mousePositionX - ((ClientSize.Width) / 2);
            int relY = mousePositionY - ((ClientSize.Height) / 2);

            worldPositionX = (relX / CameraScale) + CameraPosition.X;
            worldPositionY = (relY / CameraScale) + CameraPosition.Y;

            if (worldPositionX < 0f || worldPositionX >= mapSize.X * Map.CELLSIZE ||
                worldPositionY < 0f || worldPositionY >= mapSize.Y * Map.CELLSIZE)
                HoveredCell = null;
            else
                HoveredCell = new Index2(
                    (int)(worldPositionX / Map.CELLSIZE), 
                    (int)(worldPositionY / Map.CELLSIZE));
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
                                Compass orientation;
                                TileRenderer renderer = OnRenderMaterial(x, y, out orientation);
                                if (renderer != null)
                                    renderer.Draw(g, x * TILEWIDTH, y * TILEWIDTH, orientation);

                                renderer = OnRenderTile(x, y, out orientation);
                                if (renderer != null)
                                    renderer.Draw(g, x * TILEWIDTH, y * TILEWIDTH, orientation);
                            }
                        }
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

        protected void InvalidateMap()
        {
            dirtyBuffer = true;
            Invalidate();
        }

        protected abstract TileRenderer OnRenderMaterial(int x, int y, out Compass orientation);

        protected abstract TileRenderer OnRenderTile(int x, int y, out Compass orientation);

        #endregion
    }
}
