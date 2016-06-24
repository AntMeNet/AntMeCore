using AntMe;
using AntMe.Runtime;
using CoreTestClient.Tools;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace CoreTestClient
{
    public partial class MapEditorForm : Form
    {
        SimulationContext context;

        private string filename;

        private Map map;

        private Map Map
        {
            get { return map; }
            set
            {
                map = value;
                SetMap(value);
            }
        }

        private TreeNode mapNode;

        private TreeNode cellNode;

        private List<EditorTool> tools;

        private SelectionTool selectionTool;

        private EditorTool activeTool;

        private bool mouseDown = false;

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

            MapTile tile = map[newValue.Value.X, newValue.Value.Y];
            SetTile(newValue, tile);
        }

        private void Scene_OnHoveredCellChanged(Index2? newValue)
        {
            Console.WriteLine("Cell Changed {0}", newValue);
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
                tool.RootItem.BackColor = (activeTool == tool ? Color.LightBlue : Color.Transparent);
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            scene.Invalidate();
            saveAsMenu.Enabled = map != null;
            saveMenu.Enabled = !string.IsNullOrEmpty(filename);

            if (scene.HoveredCell.HasValue)
            {
                hoverLabel.Text = string.Format("{0}/{1}", scene.HoveredCell.Value.X, scene.HoveredCell.Value.Y);
            }
            else
            {
                hoverLabel.Text = string.Empty;
            }
        }

        private void closeMenu_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void loadMenu_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    using (Stream stream = File.Open(openFileDialog.FileName, FileMode.Open))
                    {
                        Map = Map.Deserialize(context, stream);
                        filename = openFileDialog.FileName;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void saveAsMenu_Click(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    using (Stream stream = File.Open(saveFileDialog.FileName, FileMode.Create))
                    {
                        Map.Serialize(stream, map);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void SetMap(Map map)
        {
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
        }

        private void RevalidateMap()
        {
            scene.Revalidate();

            errorsList.Items.Clear();
            foreach (var error in scene.ValidationExceptions)
            {
                List<string> messages = new List<string>();
                Exception ex = error;
                do
                {
                    messages.Add(ex.Message);
                    ex = ex.InnerException;
                } while (ex != null);

                ListViewItem item = errorsList.Items.Add("");
                if (error is InvalidMapTileException)
                {
                    InvalidMapTileException mapTileError = error as InvalidMapTileException;
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
                cellNode.Tag = tile;
                treeView.SelectedNode = cellNode;
                propertyGrid.SelectedObject = tile;

                if (tile.Material != null)
                {
                    var node = cellNode.Nodes.Add(tile.Material.ToString());
                    node.Tag = tile.Material;
                }

                foreach (var property in tile.Properties)
                {
                    var node = cellNode.Nodes.Add(property.ToString());
                    node.Tag = property;
                }
            }
        }

        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            propertyGrid.SelectedObject = e.Node.Tag;
        }

        private void saveMenu_Click(object sender, EventArgs e)
        {
            try
            {
                using (Stream stream = File.Open(filename, FileMode.Create))
                {
                    Map.Serialize(stream, map);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void errorsList_DoubleClick(object sender, EventArgs e)
        {
            if (map != null &&
                errorsList.SelectedItems.Count > 0 &&
                errorsList.SelectedItems[0].Tag is InvalidMapTileException)
            {
                InvalidMapTileException ex = errorsList.SelectedItems[0].Tag as InvalidMapTileException;
                selectionTool.Apply(map, ex.CellIndex, new Vector2((ex.CellIndex.X + 0.5f) * Map.CELLSIZE, (ex.CellIndex.Y + 0.5f) * Map.CELLSIZE));
            }
        }
    }
}
