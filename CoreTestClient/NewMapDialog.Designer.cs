namespace CoreTestClient
{
    partial class NewMapDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.createButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.mapSizeXNumberic = new System.Windows.Forms.NumericUpDown();
            this.mapSizeYNumberic = new System.Windows.Forms.NumericUpDown();
            this.HeightNumeric = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.blockedCheckbox = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.mapSizeXNumberic)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mapSizeYNumberic)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.HeightNumeric)).BeginInit();
            this.SuspendLayout();
            // 
            // createButton
            // 
            this.createButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.createButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.createButton.Location = new System.Drawing.Point(252, 127);
            this.createButton.Name = "createButton";
            this.createButton.Size = new System.Drawing.Size(75, 23);
            this.createButton.TabIndex = 0;
            this.createButton.Text = "Create";
            this.createButton.UseVisualStyleBackColor = true;
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(171, 127);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 1;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // mapSizeXNumberic
            // 
            this.mapSizeXNumberic.Location = new System.Drawing.Point(158, 27);
            this.mapSizeXNumberic.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.mapSizeXNumberic.Name = "mapSizeXNumberic";
            this.mapSizeXNumberic.Size = new System.Drawing.Size(52, 20);
            this.mapSizeXNumberic.TabIndex = 2;
            this.mapSizeXNumberic.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // mapSizeYNumberic
            // 
            this.mapSizeYNumberic.Location = new System.Drawing.Point(229, 27);
            this.mapSizeYNumberic.Name = "mapSizeYNumberic";
            this.mapSizeYNumberic.Size = new System.Drawing.Size(53, 20);
            this.mapSizeYNumberic.TabIndex = 3;
            this.mapSizeYNumberic.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // HeightNumeric
            // 
            this.HeightNumeric.Location = new System.Drawing.Point(158, 84);
            this.HeightNumeric.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.HeightNumeric.Name = "HeightNumeric";
            this.HeightNumeric.Size = new System.Drawing.Size(52, 20);
            this.HeightNumeric.TabIndex = 4;
            this.HeightNumeric.Value = new decimal(new int[] {
            255,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(32, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Map Dimensions";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(32, 86);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(104, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Default Height Level";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(32, 58);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Border Behavior";
            // 
            // blockedCheckbox
            // 
            this.blockedCheckbox.AutoSize = true;
            this.blockedCheckbox.Checked = true;
            this.blockedCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.blockedCheckbox.Location = new System.Drawing.Point(157, 57);
            this.blockedCheckbox.Name = "blockedCheckbox";
            this.blockedCheckbox.Size = new System.Drawing.Size(53, 17);
            this.blockedCheckbox.TabIndex = 8;
            this.blockedCheckbox.Text = "Block";
            this.blockedCheckbox.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(216, 29);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(12, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "x";
            // 
            // NewMapDialog
            // 
            this.AcceptButton = this.createButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(339, 162);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.blockedCheckbox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.HeightNumeric);
            this.Controls.Add(this.mapSizeYNumberic);
            this.Controls.Add(this.mapSizeXNumberic);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.createButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NewMapDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "New Map";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.NewMapDialog_FormClosing);
            this.Shown += new System.EventHandler(this.NewMapDialog_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.mapSizeXNumberic)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mapSizeYNumberic)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.HeightNumeric)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button createButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.NumericUpDown mapSizeXNumberic;
        private System.Windows.Forms.NumericUpDown mapSizeYNumberic;
        private System.Windows.Forms.NumericUpDown HeightNumeric;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox blockedCheckbox;
        private System.Windows.Forms.Label label4;
    }
}