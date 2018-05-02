using AntMe;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CoreTestClient
{
    public partial class NewMapDialog : Form
    {
        /// <summary>
        /// Returns the selected Map Size for the new Map.
        /// </summary>
        public Index2 MapSize { get; set; }

        /// <summary>
        /// Returns the Border Behavior for the new Map.
        /// </summary>
        public bool BlockedBorder { get; set; }

        /// <summary>
        /// Returns the 
        /// </summary>
        public byte DefaultHeightLevel { get; set; }

        public NewMapDialog()
        {
            InitializeComponent();

            mapSizeXNumberic.Minimum = Map.MinWidth;
            mapSizeXNumberic.Maximum = Map.MaxWidth;
            mapSizeYNumberic.Minimum = Map.MinHeight;
            mapSizeYNumberic.Maximum = Map.MaxHeight;

            MapSize = new Index2(30, 20);
            BlockedBorder = true;
            DefaultHeightLevel = 10;
        }

        private void NewMapDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult == DialogResult.OK)
            {
                MapSize = new Index2((int)mapSizeXNumberic.Value, (int)mapSizeYNumberic.Value);
                BlockedBorder = blockedCheckbox.Checked;
                DefaultHeightLevel = (byte)HeightNumeric.Value;
            }
        }

        private void NewMapDialog_Shown(object sender, EventArgs e)
        {
            mapSizeXNumberic.Value = Math.Min(mapSizeXNumberic.Maximum, Math.Max(mapSizeXNumberic.Minimum, MapSize.X));
            mapSizeYNumberic.Value = Math.Min(mapSizeYNumberic.Maximum, Math.Max(mapSizeYNumberic.Minimum, MapSize.Y));
            blockedCheckbox.Checked = BlockedBorder;
            HeightNumeric.Value = Math.Min(HeightNumeric.Maximum, Math.Max(HeightNumeric.Minimum, DefaultHeightLevel));
        }
    }
}
