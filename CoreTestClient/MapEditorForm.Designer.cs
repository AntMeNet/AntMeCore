namespace CoreTestClient
{
    partial class MapEditorForm
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Map");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Selected Cell");
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.newMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.loadMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.saveMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.closeMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.hoverLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.errorContainer = new System.Windows.Forms.SplitContainer();
            this.errorsList = new System.Windows.Forms.ListView();
            this.cellColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.descriptionColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label1 = new System.Windows.Forms.Label();
            this.toolSplitContainer = new System.Windows.Forms.SplitContainer();
            this.treeView = new System.Windows.Forms.TreeView();
            this.label2 = new System.Windows.Forms.Label();
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            this.label3 = new System.Windows.Forms.Label();
            this.scene = new CoreTestClient.EditorSceneControl();
            this.menuStrip.SuspendLayout();
            this.statusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorContainer)).BeginInit();
            this.errorContainer.Panel1.SuspendLayout();
            this.errorContainer.Panel2.SuspendLayout();
            this.errorContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.toolSplitContainer)).BeginInit();
            this.toolSplitContainer.Panel1.SuspendLayout();
            this.toolSplitContainer.Panel2.SuspendLayout();
            this.toolSplitContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileMenu});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(938, 24);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "menuStrip1";
            // 
            // fileMenu
            // 
            this.fileMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newMenu,
            this.loadMenu,
            this.saveMenu,
            this.saveAsMenu,
            this.toolStripMenuItem1,
            this.closeMenu});
            this.fileMenu.Name = "fileMenu";
            this.fileMenu.Size = new System.Drawing.Size(37, 20);
            this.fileMenu.Text = "File";
            // 
            // newMenu
            // 
            this.newMenu.Name = "newMenu";
            this.newMenu.Size = new System.Drawing.Size(152, 22);
            this.newMenu.Text = "New";
            this.newMenu.Click += new System.EventHandler(this.newMenu_Click);
            // 
            // loadMenu
            // 
            this.loadMenu.Name = "loadMenu";
            this.loadMenu.Size = new System.Drawing.Size(152, 22);
            this.loadMenu.Text = "Load...";
            this.loadMenu.Click += new System.EventHandler(this.loadMenu_Click);
            // 
            // saveMenu
            // 
            this.saveMenu.Enabled = false;
            this.saveMenu.Name = "saveMenu";
            this.saveMenu.Size = new System.Drawing.Size(152, 22);
            this.saveMenu.Text = "Save";
            this.saveMenu.Click += new System.EventHandler(this.saveMenu_Click);
            // 
            // saveAsMenu
            // 
            this.saveAsMenu.Name = "saveAsMenu";
            this.saveAsMenu.Size = new System.Drawing.Size(152, 22);
            this.saveAsMenu.Text = "Save as...";
            this.saveAsMenu.Click += new System.EventHandler(this.saveAsMenu_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(149, 6);
            // 
            // closeMenu
            // 
            this.closeMenu.Name = "closeMenu";
            this.closeMenu.Size = new System.Drawing.Size(152, 22);
            this.closeMenu.Text = "Close";
            this.closeMenu.Click += new System.EventHandler(this.closeMenu_Click);
            // 
            // toolStrip
            // 
            this.toolStrip.Location = new System.Drawing.Point(0, 24);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(938, 25);
            this.toolStrip.TabIndex = 1;
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.hoverLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 537);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(938, 22);
            this.statusStrip.TabIndex = 2;
            this.statusStrip.Text = "statusStrip1";
            // 
            // hoverLabel
            // 
            this.hoverLabel.Name = "hoverLabel";
            this.hoverLabel.Size = new System.Drawing.Size(45, 17);
            this.hoverLabel.Text = "[hover]";
            // 
            // openFileDialog
            // 
            this.openFileDialog.DefaultExt = "map";
            this.openFileDialog.Filter = "AntMe! Maps|*.map";
            this.openFileDialog.Title = "Load AntMe! Map";
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.DefaultExt = "map";
            this.saveFileDialog.Filter = "AntMe! Maps|*.map";
            this.saveFileDialog.Title = "Save AntMe! Map";
            // 
            // timer
            // 
            this.timer.Interval = 10;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer.Location = new System.Drawing.Point(0, 49);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.errorContainer);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.toolSplitContainer);
            this.splitContainer.Size = new System.Drawing.Size(938, 488);
            this.splitContainer.SplitterDistance = 680;
            this.splitContainer.TabIndex = 4;
            // 
            // errorContainer
            // 
            this.errorContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.errorContainer.Location = new System.Drawing.Point(0, 0);
            this.errorContainer.Name = "errorContainer";
            this.errorContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // errorContainer.Panel1
            // 
            this.errorContainer.Panel1.Controls.Add(this.scene);
            // 
            // errorContainer.Panel2
            // 
            this.errorContainer.Panel2.Controls.Add(this.errorsList);
            this.errorContainer.Panel2.Controls.Add(this.label1);
            this.errorContainer.Size = new System.Drawing.Size(680, 488);
            this.errorContainer.SplitterDistance = 388;
            this.errorContainer.TabIndex = 1;
            // 
            // errorsList
            // 
            this.errorsList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.cellColumn,
            this.descriptionColumn});
            this.errorsList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.errorsList.FullRowSelect = true;
            this.errorsList.Location = new System.Drawing.Point(0, 18);
            this.errorsList.Name = "errorsList";
            this.errorsList.Size = new System.Drawing.Size(680, 78);
            this.errorsList.TabIndex = 1;
            this.errorsList.UseCompatibleStateImageBehavior = false;
            this.errorsList.View = System.Windows.Forms.View.Details;
            this.errorsList.DoubleClick += new System.EventHandler(this.errorsList_DoubleClick);
            // 
            // cellColumn
            // 
            this.cellColumn.Text = "Cell";
            // 
            // descriptionColumn
            // 
            this.descriptionColumn.Text = "Description";
            this.descriptionColumn.Width = 583;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(2);
            this.label1.Size = new System.Drawing.Size(680, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "Errors";
            // 
            // toolSplitContainer
            // 
            this.toolSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolSplitContainer.Location = new System.Drawing.Point(0, 0);
            this.toolSplitContainer.Name = "toolSplitContainer";
            this.toolSplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // toolSplitContainer.Panel1
            // 
            this.toolSplitContainer.Panel1.Controls.Add(this.treeView);
            this.toolSplitContainer.Panel1.Controls.Add(this.label2);
            // 
            // toolSplitContainer.Panel2
            // 
            this.toolSplitContainer.Panel2.Controls.Add(this.propertyGrid);
            this.toolSplitContainer.Panel2.Controls.Add(this.label3);
            this.toolSplitContainer.Size = new System.Drawing.Size(254, 488);
            this.toolSplitContainer.SplitterDistance = 235;
            this.toolSplitContainer.TabIndex = 0;
            // 
            // treeView
            // 
            this.treeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView.HideSelection = false;
            this.treeView.Location = new System.Drawing.Point(0, 18);
            this.treeView.Name = "treeView";
            treeNode1.Name = "mapNode";
            treeNode1.Text = "Map";
            treeNode2.Name = "cellNode";
            treeNode2.Text = "Selected Cell";
            this.treeView.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2});
            this.treeView.Size = new System.Drawing.Size(254, 217);
            this.treeView.TabIndex = 0;
            this.treeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterSelect);
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Padding = new System.Windows.Forms.Padding(2);
            this.label2.Size = new System.Drawing.Size(254, 18);
            this.label2.TabIndex = 1;
            this.label2.Text = "Map Structure";
            // 
            // propertyGrid
            // 
            this.propertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid.Location = new System.Drawing.Point(0, 18);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.Size = new System.Drawing.Size(254, 231);
            this.propertyGrid.TabIndex = 0;
            this.propertyGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGrid_PropertyValueChanged);
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label3.Dock = System.Windows.Forms.DockStyle.Top;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label3";
            this.label3.Padding = new System.Windows.Forms.Padding(2);
            this.label3.Size = new System.Drawing.Size(254, 18);
            this.label3.TabIndex = 1;
            this.label3.Text = "Properties";
            // 
            // scene
            // 
            this.scene.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scene.Location = new System.Drawing.Point(0, 0);
            this.scene.Name = "scene";
            this.scene.SelectedCell = null;
            this.scene.Size = new System.Drawing.Size(680, 388);
            this.scene.TabIndex = 0;
            // 
            // MapEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(938, 559);
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.menuStrip);
            this.DoubleBuffered = true;
            this.MainMenuStrip = this.menuStrip;
            this.Name = "MapEditorForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Map Editor";
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.errorContainer.Panel1.ResumeLayout(false);
            this.errorContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorContainer)).EndInit();
            this.errorContainer.ResumeLayout(false);
            this.toolSplitContainer.Panel1.ResumeLayout(false);
            this.toolSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.toolSplitContainer)).EndInit();
            this.toolSplitContainer.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileMenu;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripMenuItem newMenu;
        private System.Windows.Forms.ToolStripMenuItem loadMenu;
        private System.Windows.Forms.ToolStripMenuItem saveMenu;
        private System.Windows.Forms.ToolStripMenuItem saveAsMenu;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem closeMenu;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.SplitContainer toolSplitContainer;
        private System.Windows.Forms.PropertyGrid propertyGrid;
        private System.Windows.Forms.ToolStripStatusLabel hoverLabel;
        private System.Windows.Forms.TreeView treeView;
        private EditorSceneControl scene;
        private System.Windows.Forms.SplitContainer errorContainer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListView errorsList;
        private System.Windows.Forms.ColumnHeader cellColumn;
        private System.Windows.Forms.ColumnHeader descriptionColumn;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}