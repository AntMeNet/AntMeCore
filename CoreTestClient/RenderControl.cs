using System;
using System.Linq;
using System.Windows.Forms;
using AntMe;
using AntMe.Runtime.Communication;

namespace CoreTestClient
{
    internal sealed partial class RenderControl : UserControl
    {
        private Index2 _mapCells;
        private Vector2 _mapSize;
        private LevelState currentState;
        private readonly TreeNode factionsNode;
        private readonly TreeNode itemsNode;

        private readonly TreeNode levelNode;
        private readonly TreeNode mapNode;
        private ISimulationClient simulation;
        private ItemState[] treeState;

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
        }

        #region Renderscreen Events

        private void itemTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            propertyGrid.SelectedObject = e.Node.Tag;
        }

        #endregion

        private void gameUpdateTimer_Tick(object sender, EventArgs e)
        {
            // Update Scene
            scene.Invalidate();

            // Update Infos
            var state = currentState;
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

            mapLabel.Text = "Map: " + _mapCells.X + "/" + _mapCells.Y + " @ " + Map.CELLSIZE;
        }

        private void UpdateTree()
        {
            var state = currentState;

            if (state != null)
            {
                var itemStates = state.Items.ToArray();

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
                    if (!state.Items.Contains(item))
                    {
                        var node = itemsNode.Nodes["item" + item.Id];
                        if (node != null) node.Remove();
                    }

                // Items einfügen
                foreach (var item in itemStates)
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
            if (currentState == null) SetScale(levelState.Map.GetCellCount());
            currentState = levelState;
            scene.SetState(levelState);
        }

        #endregion
    }
}