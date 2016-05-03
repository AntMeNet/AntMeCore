using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AntMe.Core.Extensions;
using AntMe.Simulation.Factions.Ants;
using AntMe.Simulation.Items;
using AntMe.Simulation.Factions.Bugs.English;
using AntMe.Simulation.Factions.Bugs;
using AntMe.Core;
using AntMe.Debug;

namespace AntMe.Simulation.Debug
{
    public partial class LevelRenderer : PlaygroundRenderer
    {
        private Level _level;

        public LevelRenderer()
        {
            InitializeComponent();
        }

        public void SetLevel(Level level)
        {
            if (_level != null)
            {
                _level.AddedItem -= _level_AddedItem;
                _level.RemovedItem -= _level_RemovedItem;
            }

            _level = level;

            if (level != null)
            {
                SetScale(level.Map.GetCellCount());
                _level.AddedItem += _level_AddedItem;
                _level.RemovedItem += _level_RemovedItem;
            }

            FillTree();
        }

        void _level_RemovedItem(Item item)
        {
            TreeNode hit = null;
            foreach (TreeNode node in itemsNode.Nodes)
            {
                if (node.Tag == item)
                {
                    hit = node;
                    break;
                }
            }

            if (hit != null)
                itemsNode.Nodes.Remove(hit);
        }

        void _level_AddedItem(Item item)
        {
            TreeNode itemNode = new TreeNode(item.ToString());
            itemNode.Tag = item;
            itemsNode.Nodes.Add(itemNode);

            foreach (var prop in item)
            {
                TreeNode propNode = new TreeNode(prop.ToString());
                propNode.Tag = prop;
                itemNode.Nodes.Add(propNode);
            }
        }

        TreeNode itemsNode;

        private void FillTree()
        {
            ItemTree.Nodes.Clear();

            if (_level != null)
            {
                TreeNode root = new TreeNode("Level");
                root.Tag = _level;
                ItemTree.Nodes.Add(root);

                TreeNode mapNode = new TreeNode("Map");
                mapNode.Tag = _level.Map;
                root.Nodes.Add(mapNode);

                itemsNode = new TreeNode("Items");
                root.Nodes.Add(itemsNode);

                foreach (var item in _level.Items)
                {
                    TreeNode itemNode = new TreeNode(item.ToString());
                    itemNode.Tag = item;
                    itemsNode.Nodes.Add(itemNode);

                    foreach (var prop in item)
                    {
                        TreeNode propNode = new TreeNode(prop.ToString());
                        propNode.Tag = prop;
                        itemNode.Nodes.Add(propNode);
                    }
                }
            }
        }

        protected override void RequestDraw()
        {
            if (_level != null)
            {

                DrawPlayground(_level.Map.GetCellCount(), _level.Map.Tiles);

                foreach (var item in _level.Items)
                {
                    float? bodySpeed = null;
                    float? bodyDirection = null;
                    float? bodyRadius = null;
                    float? smellableRadius = null;
                    float? viewRange = null;
                    float? viewAngle = null;
                    float? viewDirection = null;
                    float? attackRange = null;

                    // Movement
                    if (item.ContainsProperty<WalkingProperty>())
                    {
                        WalkingProperty prop = item.GetProperty<WalkingProperty>();
                        bodySpeed = prop.Speed;
                        bodyDirection = prop.Direction;
                    }

                    // Kollisionsradius
                    if (item.ContainsProperty<CollidableProperty>())
                    {
                        CollidableProperty prop = item.GetProperty<CollidableProperty>();
                        bodyRadius = prop.CollisionRadius;
                    }

                    // Sicht
                    if (item.ContainsProperty<SightingProperty>())
                    {
                        SightingProperty prop = item.GetProperty<SightingProperty>();
                        viewRange = prop.ViewRange;
                        viewDirection = prop.ViewDirection.Radian;
                        viewAngle = prop.ViewAngle;
                    }

                    // Attack
                    if (item.ContainsProperty<AttackerProperty>())
                    {
                        AttackerProperty prop = item.GetProperty<AttackerProperty>();
                        attackRange = prop.AttackRange;
                    }

                    // Stinkender radius
                    if (item.ContainsProperty<SmellableProperty>())
                    {
                        SmellableProperty prop = item.GetProperty<SmellableProperty>();
                        smellableRadius = prop.SmellableRadius;
                    }

                    Color color = Color.Gray;
                    Color? borderColor = null;

                    // Farbe
                    if (item is AnthillItem)
                        color = Color.Brown;
                    else if (item is SugarItem)
                        color = Color.White;
                    else if (item is AppleItem)
                        color = Color.LightGreen;
                    else if (item is BugItem)
                        color = Color.Blue;
                    else if (item is AntItem)
                    {
                        color = Color.LightGray;
                        AntItem ant = item as AntItem;
                        switch (ant.PlayerIndex)
                        {
                            case 0: borderColor = Color.Orange; break;
                            case 1: borderColor = Color.Red; break;
                            case 2: borderColor = Color.Yellow; break;
                            case 3: borderColor = Color.Green; break;
                            case 4: borderColor = Color.Blue; break;
                            case 5: borderColor = Color.Purple; break;
                            case 6: borderColor = Color.White; break;
                            case 7: borderColor = Color.Black; break;
                        }
                    }
                    else if (item is MarkerItem)
                    {
                        color = Color.LightGray;
                        MarkerItem marker = item as MarkerItem;
                        switch (marker.PlayerIndex)
                        {
                            case 0: borderColor = Color.Orange; break;
                            case 1: borderColor = Color.Red; break;
                            case 2: borderColor = Color.Yellow; break;
                            case 3: borderColor = Color.Green; break;
                            case 4: borderColor = Color.Blue; break;
                            case 5: borderColor = Color.Purple; break;
                            case 6: borderColor = Color.White; break;
                            case 7: borderColor = Color.Black; break;
                        }
                    }
                    
                    // Malen
                    DrawItem(item.Id, item.Position,
                        bodyRadius, bodyDirection, bodySpeed, color,
                        viewRange, viewDirection, viewAngle,
                        attackRange, smellableRadius, borderColor);
                }
            }
        }

        protected override void NextRound()
        {
            if (_level != null)
            {
                _level.NextState();
                Redraw(_level.Round, _level.Items.Count);
            }
        }
    }
}
