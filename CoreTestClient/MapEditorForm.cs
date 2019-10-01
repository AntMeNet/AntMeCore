using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using AntMe;
using AntMe.Runtime;
using AntMe.Serialization;
using CoreTestClient.Tools;

namespace CoreTestClient
{
    public partial class MapEditorForm : Form
    {
        private EditorTool activeTool;

        private readonly TreeNode cellNode;
        private readonly SimulationContext context;

        private string filename;

        private Map map;

        private bool mapChanged;

        private readonly TreeNode mapNode;

        private bool mouseDown;

        private readonly SelectionTool selectionTool;

        private readonly StartPointTool startPointTool;

        private readonly List<EditorTool> tools;

        public MapEditorForm()
        {
            context = ExtensionLoader.CreateSimulationContext();

            InitializeComponent();
            errorContainer.Panel2Collapsed = true;

            scene.MouseDown += Scene_MouseDown;
            scene.MouseUp += Scene_MouseUp;
            scene.MouseLeave += Scene_MouseLeave;
            scene.OnHoveredCellChanged += Scene_OnHoveredCellChanged;

            tools = new List<EditorTool>();

            selectionTool = new SelectionTool(context);
            selectionTool.OnSelectedCellChanged += Selection_OnSelectedCellChanged;
            tools.Add(selectionTool);
            tools.Add(new MapTileTool(context));
            tools.Add(new MaterialTool(context));

            startPointTool = new StartPointTool(context);
            scene.StartPoints = startPointTool.StartPoints;
            tools.Add(startPointTool);

            foreach (var tool in tools)
            {
                toolStrip.Items.Add(tool.RootItem);
                tool.OnSelect += Tool_OnSelect;
            }

            Tool_OnSelect(selectionTool, null);

            mapNode = treeView.Nodes["mapNode"];
            cellNode = treeView.Nodes["cellNode"];

            timer.Enabled = true;
        }

        private Map Map
        {
            get => map;
            set
            {
                map = value;
                SetMap(value);
            }
        }

        private void Scene_MouseLeave(object sender, EventArgs e)
        {
            mouseDown = false;
        }

        private void Selection_OnSelectedCellChanged(Index2? newValue)
        {
            if (!newValue.HasValue)
            {
                SetTile(null, null);
                return;
            }

            if (map == null)
            {
                SetTile(null, null);
                return;
            }

            var tile = map[newValue.Value.X, newValue.Value.Y];
            SetTile(newValue, tile);
        }

        private void Scene_OnHoveredCellChanged(Index2? newValue)
        {
            if (mouseDown) Apply();
        }

        private void Scene_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                mouseDown = false;
        }

        private void Scene_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mouseDown = true;
                Apply();
            }
        }

        private void Apply()
        {
            if (activeTool != null &&
                activeTool.CanApply(Map, scene.HoveredCell, scene.HoveredPosition))
            {
                try
                {
                    activeTool.Apply(Map, scene.HoveredCell, scene.HoveredPosition);
                    if (activeTool != selectionTool)
                        mapChanged = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                scene.InvalidateMap();
                RevalidateMap();
            }
        }

        private void Tool_OnSelect(object sender, EventArgs e)
        {
            activeTool = sender as EditorTool;
            foreach (var tool in tools)
                tool.RootItem.BackColor = activeTool == tool ? Color.LightBlue : Color.Transparent;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            scene.Invalidate();
            saveAsMenu.Enabled = map != null;
            saveMenu.Enabled = !string.IsNullOrEmpty(filename);

            if (scene.HoveredCell.HasValue)
                hoverLabel.Text = string.Format("{0}/{1}", scene.HoveredCell.Value.X, scene.HoveredCell.Value.Y);
            else
                hoverLabel.Text = string.Empty;

            if (map != null)
            {
                if (!string.IsNullOrEmpty(filename))
                    Text = string.Format("Map Editor ({0}){1}", filename, mapChanged ? "*" : "");
                else
                    Text = "Map Editor (New Map)*";
            }
            else
            {
                Text = "Map Editor";
            }
        }

        private void closeMenu_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void loadMenu_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
                try
                {
                    using (Stream stream = File.Open(openFileDialog.FileName, FileMode.Open))
                    {
                        Map = MapSerializer.Deserialize(context, stream);
                        filename = openFileDialog.FileName;
                        mapChanged = false;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
        }

        private void saveAsMenu_Click(object sender, EventArgs e)
        {
            // Prefill Filename if there is one
            if (!string.IsNullOrEmpty(filename))
                saveFileDialog.FileName = filename;

            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
                try
                {
                    using (Stream stream = File.Open(saveFileDialog.FileName, FileMode.Create))
                    {
                        MapSerializer.Serialize(stream, map);
                        filename = saveFileDialog.FileName;
                        mapChanged = false;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(string.Format("Could not save: {0}", ex.Message));
                }
        }

        private void SetMap(Map map)
        {
            for (var i = 0; i < startPointTool.StartPoints.Length; i++)
                if (map != null && map.StartPoints.Length >= i + 1)
                    startPointTool.StartPoints[i] = map.StartPoints[i];
                else
                    startPointTool.StartPoints[i] = null;

            scene.SetMap(map);
            RevalidateMap();

            mapNode.Nodes.Clear();
            mapNode.Tag = null;
            if (map != null)
            {
                mapNode.Tag = map;
                foreach (var property in map.Properties)
                {
                    var node = mapNode.Nodes.Add(property.ToString());
                    node.Tag = property;
                }
            }

            mapNode.Expand();
        }

        private void RevalidateMap()
        {
            scene.Revalidate();

            errorsList.Items.Clear();
            foreach (var error in scene.ValidationExceptions)
            {
                var messages = new List<string>();
                var ex = error;
                do
                {
                    messages.Add(ex.Message);
                    ex = ex.InnerException;
                } while (ex != null);

                var item = errorsList.Items.Add("");
                if (error is InvalidMapTileException)
                {
                    var mapTileError = error as InvalidMapTileException;
                    item.Text = mapTileError.CellIndex.ToString();
                }

                item.Tag = error;
                item.SubItems.Add(string.Join(" => ", messages));
            }

            errorContainer.Panel2Collapsed = scene.ValidationExceptions.Count == 0;
        }

        private void SetTile(Index2? cell, MapTile tile)
        {
            cellNode.Nodes.Clear();
            if (cell.HasValue)
                cellNode.Text = string.Format("Selected Cell ({0})", cell.ToString());
            else cellNode.Text = "No Selected Cell";
            scene.SelectedCell = cell;

            if (tile != null)
            {
                if (tile.Material != null)
                {
                    var node = cellNode.Nodes.Add(tile.Material.ToString());
                    node.Tag = tile.Material;
                }

                var tileNode = cellNode.Nodes.Add(tile.ToString());
                tileNode.Tag = tile;
                treeView.SelectedNode = tileNode;

                foreach (var property in tile.Properties)
                {
                    var node = tileNode.Nodes.Add(property.ToString());
                    node.Tag = property;
                }

                cellNode.Expand();
                tileNode.Expand();
            }
        }

        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            propertyGrid.SelectedObject = e.Node.Tag;
        }

        private void saveMenu_Click(object sender, EventArgs e)
        {
            Save();
        }

        private bool Save()
        {
            if (map != null && !string.IsNullOrEmpty(filename))
                try
                {
                    using (Stream stream = File.Open(filename, FileMode.Create))
                    {
                        MapSerializer.Serialize(stream, map);
                        mapChanged = false;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(string.Format("Could not save: {0}", ex.Message));
                    return false;
                }

            return true;
        }

        private void errorsList_DoubleClick(object sender, EventArgs e)
        {
            if (map != null &&
                errorsList.SelectedItems.Count > 0 &&
                errorsList.SelectedItems[0].Tag is InvalidMapTileException)
            {
                var ex = errorsList.SelectedItems[0].Tag as InvalidMapTileException;
                selectionTool.Apply(map, ex.CellIndex,
                    new Vector2((ex.CellIndex.X + 0.5f) * Map.CELLSIZE, (ex.CellIndex.Y + 0.5f) * Map.CELLSIZE));
            }
        }

        private void propertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            RevalidateMap();
            scene.InvalidateMap();
            mapChanged = true;
        }

        private void newMenu_Click(object sender, EventArgs e)
        {
            using (var dialog = new NewMapDialog())
            {
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    var m = new Map(context, dialog.MapSize.X, dialog.MapSize.Y);
                    m.BlockBorder = dialog.BlockedBorder;
                    m.BaseLevel = dialog.DefaultHeightLevel;

                    // Default Map Tile
                    var flatTile = context.Mapper.MapTiles.First(t => t.Name.Equals("Flat Map Tile"));
                    if (flatTile == null)
                    {
                        MessageBox.Show("Missing default Map Tile. Make sure the Basic Extension is loaded.");
                        return;
                    }

                    var size = m.GetCellCount();
                    for (var y = 0; y < size.Y; y++)
                    for (var x = 0; x < size.X; x++)
                    {
                        m[x, y] = Activator.CreateInstance(flatTile.Type, context) as MapTile;
                        m[x, y].HeightLevel = m.BaseLevel;
                    }

                    Map = m;
                    filename = string.Empty;
                    mapChanged = true;
                }
            }
        }

        private void MapEditorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (mapChanged)
            {
                if (string.IsNullOrEmpty(filename))
                {
                    // Unsaved Changes in a new Map
                    if (MessageBox.Show(this,
                            "There are some unsaved Changes. Are you sure you want to close the Editor?",
                            "Unsaved Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        e.Cancel = true;
                }
                else
                {
                    // Unsaved Changes in an existing Map
                    switch (MessageBox.Show(this,
                        "There are some unsaved Changes. Do you want to save before close the Editor?",
                        "Unsaved Changes", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
                    {
                        case DialogResult.Cancel:
                            e.Cancel = true;
                            break;
                        case DialogResult.Yes:
                            if (!Save())
                                e.Cancel = true;
                            break;
                    }
                }
            }
        }
    }
}