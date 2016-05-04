using AntMe;
using AntMe.Runtime.Communication;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace CoreTestClient
{
    internal partial class PlaygroundRenderer : UserControl
    {
        private float scale = 0;
        private float minScale = 0;
        private float offsetX = 0;
        private float offsetY = 0;

        private Pen playgroundLines = new Pen(Color.Black);
        private Font playgroundText = new Font("Courier New", 7f);
        private Brush playgroundTextBrush = new SolidBrush(Color.Black);

        private Brush itemBodyBrush = new SolidBrush(Color.Red);
        private Pen itemCenterPen = new Pen(Color.Black);

        private ISimulationClient simulation;
        private LevelState currentState;
        private Color[] colors = new Color[8];


        public PlaygroundRenderer()
        {
            InitializeComponent();

            gameUpdateTimer.Enabled = true;

            roundLabel.Text = string.Empty;
            mapLabel.Text = string.Empty;
            itemLabel.Text = string.Empty;

            renderScreen.Paint += renderScreen_Paint;
            renderScreen.Resize += renderScreen_Resize;
            renderScreen.MouseDown += renderScreen_MouseDown;
            renderScreen.MouseUp += renderScreen_MouseUp;
            renderScreen.MouseMove += renderScreen_MouseMove;
            renderScreen.MouseWheel += renderScreen_MouseWheel;
            renderScreen.MouseEnter += renderScreen_MouseEnter;
        }

        public void SetSimulation(ISimulationClient client)
        {
            simulation = client;

            if (simulation != null)
            {
                simulation.OnSimulationState += Simulation_OnSimulationState;
                //CurrentState = simulation.NextState();
                
            }
        }

        private void Simulation_OnSimulationState(ISimulationClient client, LevelState parameter)
        {
            if (currentState == null)
            {
                foreach (var faction in parameter.Factions)
                    colors[faction.PlayerIndex] = Convert(faction.PlayerColor);

                SetScale(parameter.Map.GetCellCount());
            }

            currentState = parameter;

            Redraw(currentState.Round, currentState.Items.Count);
            //if (currentState != null)
                // RefreshTree(CurrentState);
        }

        #region Renderscreen Events

        private void renderScreen_MouseEnter(object sender, EventArgs e)
        {
            renderScreen.Focus();
        }

        private void renderScreen_MouseWheel(object sender, MouseEventArgs e)
        {
            float oldScale = scale;
            scale += scale * (float)e.Delta * 0.001f;
            if (scale > 1000)
                scale = 1000;
            if (scale < minScale)
                scale = minScale;

            // Offset korrigieren
            Size center = new Size((renderScreen.Width / 2), (renderScreen.Height / 2));
            float factor = scale / oldScale;

            float relativeX = offsetX - center.Width;
            float relativeY = offsetY - center.Height;
            relativeX *= factor;
            relativeY *= factor;
            offsetX = relativeX + center.Width;
            offsetY = relativeY + center.Height;

            renderScreen.Invalidate();
        }

        private void renderScreen_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.SkyBlue);

            g = e.Graphics;

            RequestDraw();

            propertyGrid.Refresh();
        }

        private void renderScreen_Resize(object sender, EventArgs e)
        {
            Rescale();
        }

        private bool dragging = false;
        private int mousex = 0;
        private int mousey = 0;

        private void renderScreen_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                offsetX += e.X - mousex;
                offsetY += e.Y - mousey;
                mousex = e.X;
                mousey = e.Y;
                renderScreen.Invalidate();
            }
        }

        private void renderScreen_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }

        private void renderScreen_MouseDown(object sender, MouseEventArgs e)
        {
            dragging = true;
            mousex = e.X;
            mousey = e.Y;
        }

        private void itemTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            propertyGrid.SelectedObject = e.Node.Tag;
        }

        #endregion

        #region Subclass

        Graphics g;

        private void RequestDraw()
        {
            if (currentState != null)
            {
                DrawPlayground(currentState.Map.GetCellCount(), currentState.Map.Tiles);

                foreach (var item in currentState.Items)
                {
                    //if (item is AnthillState)
                    //{
                    //    AnthillState anthillState = item as AnthillState;
                    //    DrawItem(item.Id, item.Position, item.Radius, null, null, Color.Brown, null, null, null, null, null, colors[anthillState.PlayerIndex]);
                    //}

                    //if (item is AntState)
                    //{
                    //    AntState antState = item as AntState;
                    //    DrawItem(item.Id, item.Position, item.Radius, item.Orientation, null, Color.Black, null, null, null, null, null, colors[antState.PlayerIndex]);
                    //}

                    //if (item is SugarState)
                    //{
                    //    SugarState sugarState = item as SugarState;
                    //    DrawItem(item.Id, item.Position, item.Radius, null, null, Color.White, null, null, null, null, null, null);
                    //}

                    //if (item is AppleState)
                    //{
                    //    AppleState appleState = item as AppleState;
                    //    DrawItem(item.Id, item.Position, item.Radius, null, null, Color.LightGreen, null, null, null, null, null, null);
                    //}

                    //if (item is MarkerState)
                    //{
                    //    MarkerState markerState = item as MarkerState;
                    //    DrawItem(item.Id, item.Position, item.Radius, null, null, Color.FromArgb(30, Color.Yellow), null, null, null, null, null, colors[markerState.PlayerIndex]);
                    //}

                    //if (item is BugState)
                    //{
                    //    // DrawItem(item.Id, item.Position, 10, Color.Blue);
                    //}
                }
            }
        }

        private Color Convert(PlayerColor color)
        {
            switch (color)
            {
                case PlayerColor.Black: return Color.Black;
                case PlayerColor.Blue: return Color.Blue;
                case PlayerColor.Cyan: return Color.Yellow;
                case PlayerColor.Green: return Color.Green;
                case PlayerColor.Orange: return Color.Orange;
                case PlayerColor.Purple: return Color.Purple;
                case PlayerColor.Red: return Color.Red;
                case PlayerColor.White: return Color.White;
            }

            return Color.Black;
        }

        private Index2 _mapCells;
        private Vector2 _mapSize;

        private void Rescale()
        {
            SetScale(_mapCells);
        }

        protected void ResetScale()
        {
            _mapCells = Index2.Zero;
            _mapSize = Vector2.Zero;
        }

        protected void SetScale(Index2 size)
        {
            if (size.X == 0)
                return;

            _mapCells = size;
            _mapSize = new Vector2(size.X * Map.CELLSIZE, size.Y * Map.CELLSIZE);

            float scaleX = (renderScreen.ClientSize.Width - 20) / (_mapSize.X);
            float scaleY = (renderScreen.ClientSize.Height - 20) / (_mapSize.Y);
            scale = Math.Min(scaleX, scaleY);
            minScale = scale;
            offsetX = ((float)renderScreen.ClientSize.Width - (_mapSize.X * scale)) / 2;
            offsetY = ((float)renderScreen.ClientSize.Height - (_mapSize.Y * scale)) / 2;

            mapLabel.Text = "Map: " + _mapCells.X + "/" + _mapCells.Y + " @ " + Map.CELLSIZE;

            renderScreen.Invalidate();
        }

        protected void Redraw(int round, int items)
        {
            itemLabel.Text = "Items: " + items;
            roundLabel.Text = "Round " + round;

            renderScreen.Invalidate();
        }

        protected void DrawPlayground(Index2 size, MapTile[,] tiles)
        {
            float localWidth = size.X * Map.CELLSIZE;
            float localHeight = size.Y * Map.CELLSIZE;

            // Linke, untere Kante
            // g.DrawLine(playgroundLines, offsetX, offsetY, offsetX, offsetY + (height * scale));
            // g.DrawLine(playgroundLines, offsetX, offsetY + (height * scale), offsetX + (width * scale), offsetY + (height * scale));

            // Rechte, obere Kanten der Zellen
            float cell = Map.CELLSIZE * scale;
            for (int x = 0; x < size.X; x++)
            {
                for (int y = 0; y < size.Y; y++)
                {
                    float left = (x * cell) + offsetX;
                    float right = ((x + 1) * cell) + offsetX;
                    float top = (y * cell) + offsetY;
                    float bottom = ((y + 1) * cell) + offsetY;
                    g.FillRectangle(GetCellcolor(tiles[x, y]), left, top, cell, cell);

                    // g.DrawLine(playgroundLines, left, top, right, top);
                    // g.DrawLine(playgroundLines, right, top, right, bottom);
                        
                    g.DrawString(tiles[x, y].Height.ToString(), playgroundText, playgroundTextBrush, left, top);
                    g.DrawString(tiles[x, y].Speed.ToString(), playgroundText, playgroundTextBrush, left, top + 9);
                }
            }
        }

        private SolidBrush GetCellcolor(MapTile tile)
        {
            if (tile.Shape != TileShape.Flat &&
                tile.Shape != TileShape.RampBottom &&
                tile.Shape != TileShape.RampTop &&
                tile.Shape != TileShape.RampLeft &&
                tile.Shape != TileShape.RampRight)
                return new SolidBrush(Color.Black);

            Color baseColor = Color.SandyBrown;
            switch (tile.Speed)
            {
                case TileSpeed.Stop: baseColor = Color.FromArgb(64, 64, 64); break;
                case TileSpeed.Slowest: baseColor = Color.FromArgb(99, 49, 0); break;
                case TileSpeed.Slower: baseColor = Color.FromArgb(204, 143, 83); break;
                case TileSpeed.Normal: baseColor = Color.FromArgb(26, 175, 43); break;
                case TileSpeed.Faster: baseColor = Color.FromArgb(192, 192, 192); break;
            }

            switch (tile.Height)
            {
                case TileHeight.High:
                    baseColor = Color.FromArgb(baseColor.R + 30, baseColor.G + 30, baseColor.B + 30);
                    break;
                case TileHeight.Medium:
                    break;
                case TileHeight.Low:
                    baseColor = Color.FromArgb(baseColor.R - 20, baseColor.G - 20, baseColor.B - 20);
                    break;
            }

            return new SolidBrush(baseColor);
        }

        /// <summary>
        /// Zeichnet ein Item auf das Spielfeld
        /// </summary>
        /// <param name="id">ID des Elements</param>
        /// <param name="position">Position des Elements</param>
        /// <param name="bodyRadius">Radius des Körpers (CollisionRange, VisibleRange, CarriableRange, AttackableRange,...)</param>
        /// <param name="bodyDirection">Richtung als Bogenmaß, in die der Körper ausgerichtet ist</param>
        /// <param name="bodySpeed">Bewegungsgeschwindigkeit des Elements</param>
        /// <param name="bodyColor">Farbe des Körpers</param>
        /// <param name="viewerRange">Sichtradius des Elements oder 0, falls blind</param>
        /// <param name="viewerDirection">Sichtrichtung des Elements</param>
        /// <param name="viewerAngle">Winkel des Sichtkegels des Elements</param>
        /// <param name="attackerRange">Angriffsradius des Elements oder 0, falls kein Attacker</param>
        /// <param name="smellableRange">Riechbarer Radius des Elements oder 0 falls nicht riechbar</param>
        protected void DrawItem(int id, Vector3 position, 
            float? bodyRadius, float? bodyDirection, float? bodySpeed, Color? bodyColor,
            float? viewerRange, float? viewerDirection, float? viewerAngle, 
            float? attackerRange, 
            float? smellableRange, Color? borderColor)
        {
            float x = (position.X * scale) + offsetX;
            float y = (position.Y * scale) + offsetY;

            // Kollisionsbody
            if (bodyRadius.HasValue)
            {
                float rad = bodyRadius.Value * scale;

                g.FillEllipse(new SolidBrush(bodyColor.HasValue ? bodyColor.Value : Color.Black), x - rad, y - rad, rad * 2, rad * 2);
                g.DrawEllipse((borderColor.HasValue ? new Pen(borderColor.Value) : playgroundLines), x - rad, y - rad, rad * 2, rad * 2);
            }

            // Sichtkegel zeichnen
            if (viewerRange.HasValue)
            {
                float rad = viewerRange.Value * scale;
                Pen pen = (borderColor.HasValue ? new Pen(borderColor.Value) : playgroundLines);
                g.DrawEllipse(pen, x - rad, y - rad, rad * 2, rad * 2);

                if (viewerDirection.HasValue)
                {
                    Vector2 vector = Vector2.FromAngle(Angle.FromRadian(viewerDirection.Value)) * rad;
                    g.DrawLine(pen, x, y, x + vector.X, y + vector.Y);
                }
            }

            // Movement
            if (bodyDirection.HasValue && bodySpeed.HasValue)
            {
                Vector2 vector = Vector2.FromAngle(Angle.FromRadian(bodyDirection.Value)) * (bodySpeed.Value * scale);
                g.DrawLine(itemCenterPen, x, y, x + vector.X, y + vector.Y);
            }

            // Riechradius zeichnen
            if (smellableRange.HasValue)
            {
                float rad = smellableRange.Value * scale;
                Pen pen = (borderColor.HasValue ? new Pen(borderColor.Value) : playgroundLines);
                g.FillEllipse(new SolidBrush(Color.FromArgb(40, 255, 238, 58)), x - rad, y - rad, rad * 2, rad * 2);
                g.DrawEllipse(pen, x - rad, y - rad, rad * 2, rad * 2);
            }

            // Zentrum und ID
            g.DrawRectangle(itemCenterPen, x, y, 1, 1);
            g.DrawString(id.ToString(), playgroundText, playgroundTextBrush, x, y);
        }

        #endregion
    }
}
