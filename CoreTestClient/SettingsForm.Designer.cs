namespace CoreTestClient
{
    partial class SettingsForm
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
            this.splitter = new System.Windows.Forms.SplitContainer();
            this.typeTreeView = new System.Windows.Forms.TreeView();
            this.valuesList = new System.Windows.Forms.ListView();
            this.keyColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.valueColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.descriptionColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.closeButton = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.openButton = new System.Windows.Forms.Button();
            this.easyReadCheckBox = new System.Windows.Forms.CheckBox();
            this.valueTextBox = new System.Windows.Forms.TextBox();
            this.descriptionTextBox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitter)).BeginInit();
            this.splitter.Panel1.SuspendLayout();
            this.splitter.Panel2.SuspendLayout();
            this.splitter.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitter
            // 
            this.splitter.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitter.Location = new System.Drawing.Point(12, 12);
            this.splitter.Name = "splitter";
            // 
            // splitter.Panel1
            // 
            this.splitter.Panel1.Controls.Add(this.typeTreeView);
            // 
            // splitter.Panel2
            // 
            this.splitter.Panel2.Controls.Add(this.valuesList);
            this.splitter.Size = new System.Drawing.Size(766, 359);
            this.splitter.SplitterDistance = 255;
            this.splitter.TabIndex = 0;
            // 
            // typeTreeView
            // 
            this.typeTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.typeTreeView.Location = new System.Drawing.Point(0, 0);
            this.typeTreeView.Name = "typeTreeView";
            this.typeTreeView.Size = new System.Drawing.Size(255, 359);
            this.typeTreeView.TabIndex = 0;
            this.typeTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.typeTreeView_AfterSelect);
            // 
            // valuesList
            // 
            this.valuesList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.keyColumn,
            this.valueColumn,
            this.descriptionColumn});
            this.valuesList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.valuesList.FullRowSelect = true;
            this.valuesList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.valuesList.HideSelection = false;
            this.valuesList.Location = new System.Drawing.Point(0, 0);
            this.valuesList.MultiSelect = false;
            this.valuesList.Name = "valuesList";
            this.valuesList.Size = new System.Drawing.Size(507, 359);
            this.valuesList.TabIndex = 1;
            this.valuesList.UseCompatibleStateImageBehavior = false;
            this.valuesList.View = System.Windows.Forms.View.Details;
            this.valuesList.SelectedIndexChanged += new System.EventHandler(this.valuesList_SelectedIndexChanged);
            // 
            // keyColumn
            // 
            this.keyColumn.Text = "Key";
            this.keyColumn.Width = 100;
            // 
            // valueColumn
            // 
            this.valueColumn.Text = "Value";
            this.valueColumn.Width = 91;
            // 
            // descriptionColumn
            // 
            this.descriptionColumn.Text = "Description";
            this.descriptionColumn.Width = 244;
            // 
            // closeButton
            // 
            this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.closeButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.closeButton.Location = new System.Drawing.Point(703, 377);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(75, 23);
            this.closeButton.TabIndex = 7;
            this.closeButton.Text = "Close";
            this.closeButton.UseVisualStyleBackColor = true;
            // 
            // saveButton
            // 
            this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.saveButton.Location = new System.Drawing.Point(110, 377);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 3;
            this.saveButton.Text = "Save...";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // openButton
            // 
            this.openButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.openButton.Location = new System.Drawing.Point(190, 377);
            this.openButton.Name = "openButton";
            this.openButton.Size = new System.Drawing.Size(75, 23);
            this.openButton.TabIndex = 4;
            this.openButton.Text = "Open...";
            this.openButton.UseVisualStyleBackColor = true;
            this.openButton.Click += new System.EventHandler(this.openButton_Click);
            // 
            // easyReadCheckBox
            // 
            this.easyReadCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.easyReadCheckBox.AutoSize = true;
            this.easyReadCheckBox.Location = new System.Drawing.Point(12, 381);
            this.easyReadCheckBox.Name = "easyReadCheckBox";
            this.easyReadCheckBox.Size = new System.Drawing.Size(92, 17);
            this.easyReadCheckBox.TabIndex = 2;
            this.easyReadCheckBox.Text = "alignedValues";
            this.easyReadCheckBox.UseVisualStyleBackColor = true;
            // 
            // valueTextBox
            // 
            this.valueTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.valueTextBox.Enabled = false;
            this.valueTextBox.Location = new System.Drawing.Point(271, 379);
            this.valueTextBox.Name = "valueTextBox";
            this.valueTextBox.Size = new System.Drawing.Size(151, 20);
            this.valueTextBox.TabIndex = 5;
            this.valueTextBox.Text = "(Value)";
            this.valueTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.valueTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.valueTextBox_KeyPress);
            this.valueTextBox.Leave += new System.EventHandler(this.valueTextBox_Leave);
            // 
            // descriptionTextBox
            // 
            this.descriptionTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.descriptionTextBox.Enabled = false;
            this.descriptionTextBox.Location = new System.Drawing.Point(428, 379);
            this.descriptionTextBox.Name = "descriptionTextBox";
            this.descriptionTextBox.Size = new System.Drawing.Size(269, 20);
            this.descriptionTextBox.TabIndex = 6;
            this.descriptionTextBox.Text = "(Description)";
            this.descriptionTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.descriptionTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.descriptionTextBox_KeyPress);
            this.descriptionTextBox.Leave += new System.EventHandler(this.descriptionTextBox_Leave);
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.closeButton;
            this.ClientSize = new System.Drawing.Size(790, 412);
            this.Controls.Add(this.descriptionTextBox);
            this.Controls.Add(this.valueTextBox);
            this.Controls.Add(this.easyReadCheckBox);
            this.Controls.Add(this.openButton);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.splitter);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(806, 451);
            this.Name = "SettingsForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Settings";
            this.splitter.Panel1.ResumeLayout(false);
            this.splitter.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitter)).EndInit();
            this.splitter.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitter;
        private System.Windows.Forms.ListView valuesList;
        private System.Windows.Forms.ColumnHeader keyColumn;
        private System.Windows.Forms.ColumnHeader valueColumn;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.ColumnHeader descriptionColumn;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button openButton;
        private System.Windows.Forms.CheckBox easyReadCheckBox;
        private System.Windows.Forms.TreeView typeTreeView;
        private System.Windows.Forms.TextBox valueTextBox;
        private System.Windows.Forms.TextBox descriptionTextBox;
    }
}