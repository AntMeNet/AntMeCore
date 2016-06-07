using AntMe;
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
                selectedLabel.Text = string.Format("{0}/{1}", editorPanel.SelectedCell.Value.X, editorPanel.SelectedCell.Value.Y);
            }
            else
            {
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
                        map = Map.Deserialize(stream, true);
                        editorPanel.Map = map;
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
