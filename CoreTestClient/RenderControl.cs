using AntMe;
using AntMe.Runtime.Communication;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace CoreTestClient
{
    internal sealed partial class RenderControl : UserControl
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

        public RenderControl()
        {
            InitializeComponent();

            levelNode = itemTree.Nodes["levelNode"];
            mapNode = itemTree.Nodes["mapNode"];
            factionsNode = itemTree.Nodes["factionsNode"];
            itemsNode = itemTree.Nodes["itemsNode"];

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

        #region Simulation Client Handling

        public void SetSimulation(ISimulationClient client)
        {
            // Unattach old Simulation
            if (simulation != null)
            {
                simulation.OnSimulationState -= Simulation_OnSimulationState;
                simulation = null;
                currentState = null;
            }

            // Attach new Simulation
            if (client != null)
            {
                simulation = client;
                simulation.OnSimulationState += Simulation_OnSimulationState;
            }
        }

        private void Simulation_OnSimulationState(ISimulationClient client, LevelState levelState)
        {
            if (currentState == null)
            {
                foreach (var faction in levelState.Factions)
                    colors[faction.PlayerIndex] = Convert(faction.PlayerColor);

                SetScale(levelState.Map.GetCellCount());
            }

            currentState = levelState;
        }

        #endregion

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
        }

        private void renderScreen_Paint(object sender, PaintEventArgs e)
        {
            LevelState state = currentState;

            e.Graphics.Clear(Color.SkyBlue);

            if (state != null)
            {
                Index2 mapSize = state.Map.GetCellCount();

                // Draw Playground
                float localWidth = mapSize.X * Map.CELLSIZE;
                float localHeight = mapSize.Y * Map.CELLSIZE;

                // Rechte, obere Kanten der Zellen
                float cell = Map.CELLSIZE * scale;
                for (int x = 0; x < mapSize.X; x++)
                {
                    for (int y = 0; y < mapSize.Y; y++)
                    {
                        float left = (x * cell) + offsetX;
                        float right = ((x + 1) * cell) + offsetX;
                        float top = (y * cell) + offsetY;
                        float bottom = ((y + 1) * cell) + offsetY;
                        e.Graphics.FillRectangle(GetCellcolor(state.Map.Tiles[x, y]), left, top, cell, cell);
                    }
                }

                foreach (var item in currentState.Items)
                {
                    float x = (item.Position.X * scale) + offsetX;
                    float y = (item.Position.Y * scale) + offsetY;

                    // Kollisionsbody
                    float rad = item.Radius * scale;
                    e.Graphics.FillEllipse(new SolidBrush(Color.Gray), x - rad, y - rad, rad * 2, rad * 2);

                    if (item is FactionItemState)
                    {
                        FactionItemState factionItem = item as FactionItemState;
                        // e.Graphics.DrawEllipse((borderColor.HasValue ? new Pen(borderColor.Value) : playgroundLines), x - rad, y - rad, rad * 2, rad * 2);
                    }

                    //    // Sichtkegel zeichnen
                    //    if (viewerRange.HasValue)
                    //    {
                    //        float rad = viewerRange.Value * scale;
                    //        Pen pen = (borderColor.HasValue ? new Pen(borderColor.Value) : playgroundLines);
                    //        g.DrawEllipse(pen, x - rad, y - rad, rad * 2, rad * 2);

                    //        if (viewerDirection.HasValue)
                    //        {
                    //            Vector2 vector = Vector2.FromAngle(Angle.FromRadian(viewerDirection.Value)) * rad;
                    //            g.DrawLine(pen, x, y, x + vector.X, y + vector.Y);
                    //        }
                    //    }

                    //    // Movement
                    //    if (bodyDirection.HasValue && bodySpeed.HasValue)
                    //    {
                    //        Vector2 vector = Vector2.FromAngle(Angle.FromRadian(bodyDirection.Value)) * (bodySpeed.Value * scale);
                    //        g.DrawLine(itemCenterPen, x, y, x + vector.X, y + vector.Y);
                    //    }

                    //    // Riechradius zeichnen
                    //    if (smellableRange.HasValue)
                    //    {
                    //        float rad = smellableRange.Value * scale;
                    //        Pen pen = (borderColor.HasValue ? new Pen(borderColor.Value) : playgroundLines);
                    //        g.FillEllipse(new SolidBrush(Color.FromArgb(40, 255, 238, 58)), x - rad, y - rad, rad * 2, rad * 2);
                    //        g.DrawEllipse(pen, x - rad, y - rad, rad * 2, rad * 2);
                    //    }

                    // Zentrum und ID
                    e.Graphics.DrawRectangle(itemCenterPen, x, y, 1, 1);
                    e.Graphics.DrawString(item.Id.ToString(), playgroundText, playgroundTextBrush, x, y);
                }
            }
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

        private void gameUpdateTimer_Tick(object sender, EventArgs e)
        {
            // Update Scene
            renderScreen.Invalidate();

            // Update Infos
            LevelState state = currentState;
            if (state != null)
            {
                itemLabel.Text = "Items: " + state.Items.Count;
                roundLabel.Text = "Round: " + state.Round;
            }
            else
            {
                itemLabel.Text = string.Empty;
                roundLabel.Text = string.Empty;
            }

            // UPdate Tree
            UpdateTree();
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

        private void ResetScale()
        {
            _mapCells = Index2.Zero;
            _mapSize = Vector2.Zero;
        }

        private void SetScale(Index2 size)
        {
            if (size.X == 0)
                return;

            _mapCells = size;
            _mapSize = new Vector2(size.X * Map.CELLSIZE, size.Y * Map.CELLSIZE);

            float scaleX = (renderScreen.ClientSize.Width - 20) / (_mapSize.X);
            float scaleY = (renderScreen.ClientSize.Height - 20) / (_mapSize.Y);
            scale = Math.Min(scaleX, scaleY);
            minScale = scale;
            offsetX = (renderScreen.ClientSize.Width - (_mapSize.X * scale)) / 2;
            offsetY = (renderScreen.ClientSize.Height - (_mapSize.Y * scale)) / 2;

            mapLabel.Text = "Map: " + _mapCells.X + "/" + _mapCells.Y + " @ " + Map.CELLSIZE;
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

        private TreeNode levelNode;
        private TreeNode mapNode;
        private TreeNode factionsNode;
        private TreeNode itemsNode;
        private LevelState treeState = null;

        private void UpdateTree()
        {
            LevelState state = currentState;

            if (state != null)
            {
                // Initial
                if (treeState == null)
                {
                    // Level
                    levelNode.Tag = state;
                    foreach (var property in state.Properties)
                    {
                        var node = levelNode.Nodes.Add(property.ToString());
                        node.Tag = property;
                    }

                    // Map
                    mapNode.Tag = state.Map;

                    // Factions
                    foreach (var faction in state.Factions)
                    {
                        var node = factionsNode.Nodes.Add(faction.ToString());
                        node.Tag = faction;

                        foreach (var property in faction.Properties)
                        {
                            var subnode = node.Nodes.Add(property.ToString());
                            subnode.Tag = property;
                        }
                    }

                    // Items
                    foreach (var item in state.Items)
                    {
                        var node = itemsNode.Nodes.Add(item.ToString());
                        node.Tag = item;

                        foreach (var property in item.Properties)
                        {
                            var subnode = node.Nodes.Add(property.ToString());
                            subnode.Tag = property;
                        }
                    }
                }
                else
                {
                    // Update
                    levelNode.Tag = state;

                    // Items entfernen
                    foreach (var item in treeState.Items)
                    {
                        if (!state.Items.Contains(item))
                            itemsNode.Nodes["item" + item.Id].Remove();
                    }

                    // Items einfügen
                    foreach (var item in state.Items)
                    {
                        if (!treeState.Items.Contains(item))
                        {
                            var node = itemsNode.Nodes.Add("item" + item.Id, item.ToString());
                            node.Tag = item;

                            foreach (var property in item.Properties)
                            {
                                var subnode = node.Nodes.Add(property.ToString());
                                subnode.Tag = property;
                            }
                        }
                    }
                }


            }
            else
            {
                if (treeState != null)
                {
                    // Reset
                    levelNode.Nodes.Clear();
                    levelNode.Tag = null;
                    mapNode.Nodes.Clear();
                    mapNode.Tag = null;
                    factionsNode.Nodes.Clear();
                    factionsNode.Tag = null;
                    itemsNode.Nodes.Clear();
                    itemsNode.Tag = null;
                    propertyGrid.SelectedObject = null;
                }
            }

            treeState = state;
        }
    }
}
