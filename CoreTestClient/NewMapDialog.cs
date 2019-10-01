using System;
using System.Windows.Forms;
using AntMe;

namespace CoreTestClient
{
    public partial class NewMapDialog : Form
    {
        public NewMapDialog()
        {
            InitializeComponent();

            mapSizeXNumberic.Minimum = Map.MIN_WIDTH;
            mapSizeXNumberic.Maximum = Map.MAX_WIDTH;
            mapSizeYNumberic.Minimum = Map.MIN_HEIGHT;
            mapSizeYNumberic.Maximum = Map.MAX_HEIGHT;

            MapSize = new Index2(30, 20);
            BlockedBorder = true;
            DefaultHeightLevel = 10;
        }

        /// <summary>
        ///     Returns the selected Map Size for the new Map.
        /// </summary>
        public Index2 MapSize { get; set; }

        /// <summary>
        ///     Returns the Border Behavior for the new Map.
        /// </summary>
        public bool BlockedBorder { get; set; }

        /// <summary>
        ///     Returns the
        /// </summary>
        public byte DefaultHeightLevel { get; set; }

        private void NewMapDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult == DialogResult.OK)
            {
                MapSize = new Index2((int) mapSizeXNumberic.Value, (int) mapSizeYNumberic.Value);
                BlockedBorder = blockedCheckbox.Checked;
                DefaultHeightLevel = (byte) HeightNumeric.Value;
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