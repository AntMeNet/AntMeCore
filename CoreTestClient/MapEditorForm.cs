using AntMe;
using AntMe.Runtime;
using System;
using System.IO;
using System.Windows.Forms;

namespace CoreTestClient
{
    public partial class MapEditorForm : Form
    {
        private Map map;

        public MapEditorForm()
        {
            InitializeComponent();
            timer.Enabled = true;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            saveAsMenu.Enabled = map != null;

            if (editorPanel.HoveredCell.HasValue)
            {
                hoverLabel.Text = string.Format("{0}/{1}", editorPanel.HoveredCell.Value.X, editorPanel.HoveredCell.Value.Y);
            } 
            else
            {
                hoverLabel.Text = string.Empty;
            }

            if (editorPanel.SelectedCell.HasValue)
            {
                Index2 cell = editorPanel.SelectedCell.Value;
                MapTile tile = map[cell.X, cell.Y];
                if (tile != propertyGrid.SelectedObject)
                    propertyGrid.SelectedObject = tile;
                selectedLabel.Text = string.Format("{0}/{1}", editorPanel.SelectedCell.Value.X, editorPanel.SelectedCell.Value.Y);
            }
            else
            {
                propertyGrid.SelectedObject = null;
                selectedLabel.Text = string.Empty;
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
                        SimulationContext context = new SimulationContext(
                            ExtensionLoader.DefaultTypeResolver, 
                            ExtensionLoader.DefaultTypeMapper, 
                            ExtensionLoader.ExtensionSettings, new Random());
                        map = Map.Deserialize(context, stream);
                        editorPanel.Map = map;
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
    }
}
