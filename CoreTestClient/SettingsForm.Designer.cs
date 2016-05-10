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
            this.typeList = new System.Windows.Forms.ListView();
            this.typeColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.valuesList = new System.Windows.Forms.ListView();
            this.keyColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.valueColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.closeButton = new System.Windows.Forms.Button();
            this.descriptionColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
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
            this.splitter.Panel1.Controls.Add(this.typeList);
            // 
            // splitter.Panel2
            // 
            this.splitter.Panel2.Controls.Add(this.valuesList);
            this.splitter.Size = new System.Drawing.Size(711, 359);
            this.splitter.SplitterDistance = 237;
            this.splitter.TabIndex = 0;
            // 
            // typeList
            // 
            this.typeList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.typeColumn});
            this.typeList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.typeList.FullRowSelect = true;
            this.typeList.Location = new System.Drawing.Point(0, 0);
            this.typeList.Name = "typeList";
            this.typeList.Size = new System.Drawing.Size(237, 359);
            this.typeList.TabIndex = 0;
            this.typeList.UseCompatibleStateImageBehavior = false;
            this.typeList.View = System.Windows.Forms.View.Details;
            this.typeList.SelectedIndexChanged += new System.EventHandler(this.typeList_SelectedIndexChanged);
            // 
            // typeColumn
            // 
            this.typeColumn.Text = "Type";
            this.typeColumn.Width = 202;
            // 
            // valuesList
            // 
            this.valuesList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.keyColumn,
            this.valueColumn,
            this.descriptionColumn});
            this.valuesList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.valuesList.FullRowSelect = true;
            this.valuesList.Location = new System.Drawing.Point(0, 0);
            this.valuesList.Name = "valuesList";
            this.valuesList.Size = new System.Drawing.Size(470, 359);
            this.valuesList.TabIndex = 0;
            this.valuesList.UseCompatibleStateImageBehavior = false;
            this.valuesList.View = System.Windows.Forms.View.Details;
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
            // closeButton
            // 
            this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.closeButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.closeButton.Location = new System.Drawing.Point(648, 377);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(75, 23);
            this.closeButton.TabIndex = 1;
            this.closeButton.Text = "Close";
            this.closeButton.UseVisualStyleBackColor = true;
            // 
            // descriptionColumn
            // 
            this.descriptionColumn.Text = "Description";
            this.descriptionColumn.Width = 244;
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.closeButton;
            this.ClientSize = new System.Drawing.Size(735, 412);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.splitter);
            this.MinimizeBox = false;
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

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitter;
        private System.Windows.Forms.ListView typeList;
        private System.Windows.Forms.ListView valuesList;
        private System.Windows.Forms.ColumnHeader typeColumn;
        private System.Windows.Forms.ColumnHeader keyColumn;
        private System.Windows.Forms.ColumnHeader valueColumn;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.ColumnHeader descriptionColumn;
    }
}