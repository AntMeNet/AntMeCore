using AntMe;
using AntMe.Runtime.Communication;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;

namespace CoreTestClient
{
    internal sealed partial class RenderControl : UserControl
    {
        private float scale = 0;
        private float minScale = 0;
        private float offsetX = 0;
        private float offsetY = 0;

        private Font playgroundText = new Font("Courier New", 7f);
        private Brush playgroundTextBrush = new SolidBrush(Color.Black);

        private Brush itemBodyBrush = new SolidBrush(Color.FromArgb(128, 50, 50, 50));
        private Pen itemDirectionPen = new Pen(Color.Black);
        private Pen[] itemFactionPens = new[] {
            new Pen(Color.Black),
            new Pen(Color.Red),
            new Pen(Color.Blue),
            new Pen(Color.Yellow),
            new Pen(Color.Purple),
            new Pen(Color.Orange),
            new Pen(Color.Green),
            new Pen(Color.White),
        };
        private Pen[] slotPens = new Pen[8];

        private ISimulationClient simulation;
        private LevelState currentState;

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
                SetScale(levelState.Map.GetCellCount());
                foreach (var faction in levelState.Factions)
                    slotPens[faction.SlotIndex] = itemFactionPens[(int)faction.PlayerColor];
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
            try
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

                    foreach (var item in state.Items.ToArray())
                    {
                        float x = (item.Position.X * scale) + offsetX;
                        float y = (item.Position.Y * scale) + offsetY;

                        // Kollisionsbody
                        float rad = item.Radius * scale;
                        e.Graphics.FillEllipse(itemBodyBrush, x - rad, y - rad, rad * 2, rad * 2);

                        // Orientation
                        Vector2 angle = Vector2.FromAngle(Angle.FromDegree(item.Orientation)) * rad;
                        e.Graphics.DrawLine(itemDirectionPen, x, y, x + angle.X, y + angle.Y);

                        // Faction-colored Outline
                        if (item is FactionItemState)
                        {
                            FactionItemState factionItem = item as FactionItemState;
                            Pen slotPen = slotPens[factionItem.SlotIndex];
                            e.Graphics.DrawEllipse(slotPen, x - rad, y - rad, rad * 2, rad * 2);
                        }

                        // ID
                        e.Graphics.DrawString(item.Id.ToString(), playgroundText, playgroundTextBrush, x, y);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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

            // Update Tree
            UpdateTree();
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

        private SolidBrush GetCellcolor(MapTileState tile)
        {
            if (!tile.CanEnter)
                return new SolidBrush(Color.Black);

            Color baseColor = Color.SandyBrown;

            // TODO: Workaround aufheben -> ExtensionLoader sollte MapTiles kennen
            int speed = (int)(tile.Material.Speed * 10);
            switch (speed)
            {
                case 1: baseColor = Color.FromArgb(64, 64, 64); break;
                case 5: baseColor = Color.FromArgb(99, 49, 0); break;
                case 8: baseColor = Color.FromArgb(204, 143, 83); break;
                case 10: baseColor = Color.FromArgb(26, 175, 43); break;
                case 12: baseColor = Color.FromArgb(192, 192, 192); break;
            }

            int level = tile.HeightLevel - 1;
            baseColor = Color.FromArgb(
                baseColor.R + (level * 30), 
                baseColor.G + (level * 30), 
                baseColor.B + (level * 30));

            return new SolidBrush(baseColor);
        }

        private TreeNode levelNode;
        private TreeNode mapNode;
        private TreeNode factionsNode;
        private TreeNode itemsNode;
        private ItemState[] treeState = null;

        private void UpdateTree()
        {
            LevelState state = currentState;

            if (state != null)
            {
                ItemState[] itemStates = state.Items.ToArray();

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

                    treeState = new ItemState[0];
                }

                // Items entfernen
                foreach (var item in treeState)
                {
                    if (!state.Items.Contains(item))
                    {
                        var node = itemsNode.Nodes["item" + item.Id];
                        if (node != null) node.Remove();
                    }
                }

                // Items einfügen
                foreach (var item in itemStates)
                {
                    if (!treeState.Contains(item))
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

                treeState = itemStates;
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
                    treeState = null;
                }
            }


        }
    }
}
