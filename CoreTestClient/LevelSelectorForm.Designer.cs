namespace CoreTestClient
{
    partial class LevelSelectorForm
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
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.levelList = new System.Windows.Forms.ListView();
            this.nameColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.levelPreview = new AntMe.Simulation.Debug.LevelPreview();
            this.panel = new System.Windows.Forms.Panel();
            this.loadButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer
            // 
            this.splitContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer.Location = new System.Drawing.Point(12, 12);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.levelList);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.levelPreview);
            this.splitContainer.Size = new System.Drawing.Size(834, 390);
            this.splitContainer.SplitterDistance = 333;
            this.splitContainer.TabIndex = 0;
            // 
            // levelList
            // 
            this.levelList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.nameColumn});
            this.levelList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.levelList.FullRowSelect = true;
            this.levelList.Location = new System.Drawing.Point(0, 0);
            this.levelList.Name = "levelList";
            this.levelList.Size = new System.Drawing.Size(333, 390);
            this.levelList.TabIndex = 0;
            this.levelList.UseCompatibleStateImageBehavior = false;
            this.levelList.View = System.Windows.Forms.View.Details;
            this.levelList.SelectedIndexChanged += new System.EventHandler(this.levelList_SelectedIndexChanged);
            this.levelList.DoubleClick += new System.EventHandler(this.levelList_DoubleClick);
            // 
            // nameColumn
            // 
            this.nameColumn.Text = "Name";
            this.nameColumn.Width = 223;
            // 
            // levelPreview
            // 
            this.levelPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.levelPreview.Location = new System.Drawing.Point(0, 0);
            this.levelPreview.Name = "levelPreview";
            this.levelPreview.Size = new System.Drawing.Size(497, 390);
            this.levelPreview.TabIndex = 0;
            // 
            // panel
            // 
            this.panel.Controls.Add(this.loadButton);
            this.panel.Controls.Add(this.cancelButton);
            this.panel.Controls.Add(this.okButton);
            this.panel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel.Location = new System.Drawing.Point(0, 408);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(858, 35);
            this.panel.TabIndex = 1;
            // 
            // loadButton
            // 
            this.loadButton.Location = new System.Drawing.Point(12, 6);
            this.loadButton.Name = "loadButton";
            this.loadButton.Size = new System.Drawing.Size(93, 23);
            this.loadButton.TabIndex = 2;
            this.loadButton.Text = "Open File...";
            this.loadButton.UseVisualStyleBackColor = true;
            this.loadButton.Click += new System.EventHandler(this.loadButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(690, 6);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 1;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(771, 6);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 0;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            // 
            // openFileDialog
            // 
            this.openFileDialog.DefaultExt = "dll";
            // 
            // LevelSelectorForm
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(858, 443);
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.panel);
            this.MinimizeBox = false;
            this.Name = "LevelSelectorForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Select a level";
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.panel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.Panel panel;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button loadButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.ListView levelList;
        private System.Windows.Forms.ColumnHeader nameColumn;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private AntMe.Simulation.Debug.LevelPreview levelPreview;
    }
}