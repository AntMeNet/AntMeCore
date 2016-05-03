using AntMe;
using AntMe.Runtime;
using AntMe.Runtime.Communication;
using System;
using System.ComponentModel;
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


        public PlaygroundRenderer()
        {
            InitializeComponent();

            gameUpdateTimer.Enabled = true;

            roundLabel.Text = string.Empty;
            mapLabel.Text = string.Empty;
            itemLabel.Text = string.Empty;
            timerDropDown.Text = "Stopped";

            showPlaygroundCheckbox.Checked = RenderPlaygroundInfos;
            showItemCenterCheckbox.Checked = RenderItemCenter;
            showCollisionCheckbox.Checked = RenderItemBody;
            showMovementCheckbox.Checked = RenderItemMovement;
            showViewCheckbox.Checked = RenderItemViewRange;
            showSmellableCheckbox.Checked = RenderItemSmellableRange;
            showAttackerCheckbox.Checked = RenderItemAttackerRange;

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

            if (simulation != null && simulation.ServerState == SimulationState.Running)
            {
                //CurrentState = simulation.NextState();
                //foreach (var faction in CurrentState.Factions)
                //    colors[faction.PlayerIndex] = Convert(faction.PlayerColor);

                //SetScale(CurrentState.Map.GetCellCount());
            }
        }

        #region Renderscreen Events

        void renderScreen_MouseEnter(object sender, EventArgs e)
        {
            renderScreen.Focus();
        }

        void renderScreen_MouseWheel(object sender, MouseEventArgs e)
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

        void renderScreen_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.SkyBlue);

            g = e.Graphics;

            RequestDraw();

            propertyGrid.Refresh();
        }

        void renderScreen_Resize(object sender, EventArgs e)
        {
            Rescale();
        }

        bool dragging = false;
        int mousex = 0;
        int mousey = 0;

        void renderScreen_MouseMove(object sender, MouseEventArgs e)
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

        void renderScreen_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }

        void renderScreen_MouseDown(object sender, MouseEventArgs e)
        {
            dragging = true;
            mousex = e.X;
            mousey = e.Y;
        }

        private void itemTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            propertyGrid.SelectedObject = e.Node.Tag;
        }

        private void showPlaygroundCheckbox_Click(object sender, EventArgs e)
        {
            RenderPlaygroundInfos = !RenderPlaygroundInfos;
            renderScreen.Invalidate();
        }

        private void showItemCenterCheckbox_Click(object sender, EventArgs e)
        {
            RenderItemCenter = !RenderItemCenter;
            renderScreen.Invalidate();
        }

        private void showCollisionCheckbox_Click(object sender, EventArgs e)
        {
            RenderItemBody = !RenderItemBody;
            renderScreen.Invalidate();
        }

        private void showAttackerCheckbox_Click(object sender, EventArgs e)
        {
            RenderItemAttackerRange = !RenderItemAttackerRange;
            renderScreen.Invalidate();
        }

        private void showViewCheckbox_Click(object sender, EventArgs e)
        {
            RenderItemViewRange = !RenderItemViewRange;
            renderScreen.Invalidate();
        }

        private void showSmellableCheckbox_Click(object sender, EventArgs e)
        {
            RenderItemSmellableRange = !RenderItemSmellableRange;
            renderScreen.Invalidate();
        }

        private void showMovementCheckbox_Click(object sender, EventArgs e)
        {
            RenderItemMovement = !RenderItemMovement;
            renderScreen.Invalidate();
        }

        private void timerStopButton_Click(object sender, EventArgs e)
        {
            timerStopButton.Checked = true;
            timer1Button.Checked = false;
            timer10Button.Checked = false;
            timer20Button.Checked = false;
            timer50Button.Checked = false;

            gameUpdateTimer.Enabled = false;
            timerDropDown.Text = "Stopped";
        }

        private void timer1Button_Click(object sender, EventArgs e)
        {
            timerStopButton.Checked = false;
            timer1Button.Checked = true;
            timer10Button.Checked = false;
            timer20Button.Checked = false;
            timer50Button.Checked = false;

            gameUpdateTimer.Enabled = true;
            gameUpdateTimer.Interval = 1000;
            timerDropDown.Text = "1 fps";
        }

        private void timer10Button_Click(object sender, EventArgs e)
        {
            timerStopButton.Checked = false;
            timer1Button.Checked = false;
            timer10Button.Checked = true;
            timer20Button.Checked = false;
            timer50Button.Checked = false;

            gameUpdateTimer.Enabled = true;
            gameUpdateTimer.Interval = 100;
            timerDropDown.Text = "10 fps";
        }

        private void timer20Button_Click(object sender, EventArgs e)
        {
            timerStopButton.Checked = false;
            timer1Button.Checked = false;
            timer10Button.Checked = false;
            timer20Button.Checked = true;
            timer50Button.Checked = false;

            gameUpdateTimer.Enabled = true;
            gameUpdateTimer.Interval = 50;
            timerDropDown.Text = "20 fps";
        }

        private void timer50Button_Click(object sender, EventArgs e)
        {
            timerStopButton.Checked = false;
            timer1Button.Checked = false;
            timer10Button.Checked = false;
            timer20Button.Checked = false;
            timer50Button.Checked = true;

            gameUpdateTimer.Enabled = true;
            gameUpdateTimer.Interval = 20;
            timerDropDown.Text = "50 fps";
        }

        #endregion

        private bool renderPlaygroundInfos = false;
        private bool renderItemCenter = true;
        private bool renderItemBody = true;
        private bool renderItemMovement = true;
        private bool renderItemViewRange = true;
        private bool renderItemSmellableRange = true;
        private bool renderItemAttackerRange = true;

        [Category("AntMe"), DisplayName("Render Playground Infos"), DefaultValue(false)]
        public bool RenderPlaygroundInfos { 
            get { return renderPlaygroundInfos; }
            set { 
                renderPlaygroundInfos = value;
                showPlaygroundCheckbox.Checked = value;
            }
        }

        [Category("AntMe"), DisplayName("Render Item Center")]
        public bool RenderItemCenter { 
            get { return renderItemCenter; }
            set { 
                renderItemCenter = value;
                showItemCenterCheckbox.Checked = value;
            }
        }

        [Category("AntMe"), DisplayName("Render Item Body")]
        public bool RenderItemBody { 
            get { return renderItemBody; }
            set { 
                renderItemBody = value;
                showCollisionCheckbox.Checked = value;
            }
        }

        [Category("AntMe"), DisplayName("Render Item Movement")]
        public bool RenderItemMovement {
            get { return renderItemMovement; }
            set { 
                renderItemMovement = value;
                showMovementCheckbox.Checked = value;
            }
        }

        [Category("AntMe"), DisplayName("Render Item View Range")]
        public bool RenderItemViewRange {
            get { return renderItemViewRange; }
            set { 
                renderItemViewRange = value;
                showViewCheckbox.Checked = value;
            }
        }

        [Category("AntMe"), DisplayName("Render Item Smellable Range")]
        public bool RenderItemSmellableRange {
            get { return renderItemSmellableRange; }
            set { 
                renderItemSmellableRange = value;
                showSmellableCheckbox.Checked = value;
            }
        }

        [Category("AntMe"), DisplayName("Render Item Attacker Range")]
        public bool RenderItemAttackerRange {
            get { return renderItemAttackerRange; }
            set { 
                renderItemAttackerRange = value;
                showAttackerCheckbox.Checked = value;
            }
        }

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

        private void NextRound()
        {
            if (simulation != null && simulation.ServerState == AntMe.Runtime.SimulationState.Running)
            {
                //currentState = Simulation.NextState();
                //Redraw(CurrentState.Round, CurrentState.Items.Count);
                //if (CurrentState != null)
                //    RefreshTree(CurrentState);
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
            if (renderPlaygroundInfos)
            {
                // g.DrawLine(playgroundLines, offsetX, offsetY, offsetX, offsetY + (height * scale));
                // g.DrawLine(playgroundLines, offsetX, offsetY + (height * scale), offsetX + (width * scale), offsetY + (height * scale));
            }

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

                    if (RenderPlaygroundInfos)
                    {
                        // g.DrawLine(playgroundLines, left, top, right, top);
                        // g.DrawLine(playgroundLines, right, top, right, bottom);
                        
                        g.DrawString(tiles[x, y].Height.ToString(), playgroundText, playgroundTextBrush, left, top);
                        g.DrawString(tiles[x, y].Speed.ToString(), playgroundText, playgroundTextBrush, left, top + 9);
                    }
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
            if (RenderItemBody && bodyRadius.HasValue)
            {
                float rad = bodyRadius.Value * scale;

                g.FillEllipse(new SolidBrush(bodyColor.HasValue ? bodyColor.Value : Color.Black), x - rad, y - rad, rad * 2, rad * 2);
                g.DrawEllipse((borderColor.HasValue ? new Pen(borderColor.Value) : playgroundLines), x - rad, y - rad, rad * 2, rad * 2);
            }

            // Sichtkegel zeichnen
            if (RenderItemViewRange && viewerRange.HasValue)
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
            if (RenderItemMovement && bodyDirection.HasValue && bodySpeed.HasValue)
            {
                Vector2 vector = Vector2.FromAngle(Angle.FromRadian(bodyDirection.Value)) * (bodySpeed.Value * scale);
                g.DrawLine(itemCenterPen, x, y, x + vector.X, y + vector.Y);
            }

            // Riechradius zeichnen
            if (RenderItemSmellableRange && smellableRange.HasValue)
            {
                float rad = smellableRange.Value * scale;
                Pen pen = (borderColor.HasValue ? new Pen(borderColor.Value) : playgroundLines);
                g.FillEllipse(new SolidBrush(Color.FromArgb(40, 255, 238, 58)), x - rad, y - rad, rad * 2, rad * 2);
                g.DrawEllipse(pen, x - rad, y - rad, rad * 2, rad * 2);
            }

            // Zentrum und ID
            if (RenderItemCenter)
            {
                g.DrawRectangle(itemCenterPen, x, y, 1, 1);
                g.DrawString(id.ToString(), playgroundText, playgroundTextBrush, x, y);
            }
        }

        #endregion

        private void gameUpdateTimer_Tick(object sender, EventArgs e)
        {
            NextRound();
        }

        private void singleRoundButton_Click(object sender, EventArgs e)
        {
            NextRound();
        }

        private void timerMaxButton_Click(object sender, EventArgs e)
        {
            gameUpdateTimer.Interval = 10;
        }
    }
}
