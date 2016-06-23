namespace CoreTestClient
{
    partial class RenderControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Level");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Map");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Factions");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Items");
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            this.mainSplitter = new System.Windows.Forms.SplitContainer();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.roundLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.itemLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.mapLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.propertySplitter = new System.Windows.Forms.SplitContainer();
            this.itemTree = new System.Windows.Forms.TreeView();
            this.gameUpdateTimer = new System.Windows.Forms.Timer(this.components);
            this.scene = new CoreTestClient.StateSceneControl();
            ((System.ComponentModel.ISupportInitialize)(this.mainSplitter)).BeginInit();
            this.mainSplitter.Panel1.SuspendLayout();
            this.mainSplitter.Panel2.SuspendLayout();
            this.mainSplitter.SuspendLayout();
            this.statusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.propertySplitter)).BeginInit();
            this.propertySplitter.Panel1.SuspendLayout();
            this.propertySplitter.Panel2.SuspendLayout();
            this.propertySplitter.SuspendLayout();
            this.SuspendLayout();
            // 
            // propertyGrid
            // 
            this.propertyGrid.CategoryForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.propertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid.Location = new System.Drawing.Point(0, 0);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.Size = new System.Drawing.Size(196, 205);
            this.propertyGrid.TabIndex = 0;
            // 
            // mainSplitter
            // 
            this.mainSplitter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainSplitter.Location = new System.Drawing.Point(0, 0);
            this.mainSplitter.Name = "mainSplitter";
            // 
            // mainSplitter.Panel1
            // 
            this.mainSplitter.Panel1.Controls.Add(this.scene);
            this.mainSplitter.Panel1.Controls.Add(this.statusStrip);
            // 
            // mainSplitter.Panel2
            // 
            this.mainSplitter.Panel2.Controls.Add(this.propertySplitter);
            this.mainSplitter.Size = new System.Drawing.Size(734, 413);
            this.mainSplitter.SplitterDistance = 534;
            this.mainSplitter.TabIndex = 2;
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.roundLabel,
            this.itemLabel,
            this.mapLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 391);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(534, 22);
            this.statusStrip.SizingGrip = false;
            this.statusStrip.TabIndex = 1;
            this.statusStrip.Text = "statusStrip1";
            // 
            // roundLabel
            // 
            this.roundLabel.Name = "roundLabel";
            this.roundLabel.Size = new System.Drawing.Size(90, 17);
            this.roundLabel.Text = "Round: [Runde]";
            // 
            // itemLabel
            // 
            this.itemLabel.Name = "itemLabel";
            this.itemLabel.Size = new System.Drawing.Size(81, 17);
            this.itemLabel.Text = "Items: [count]";
            // 
            // mapLabel
            // 
            this.mapLabel.Name = "mapLabel";
            this.mapLabel.Size = new System.Drawing.Size(124, 17);
            this.mapLabel.Text = "Mapsize: [x/y] @ [size]";
            // 
            // propertySplitter
            // 
            this.propertySplitter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertySplitter.Location = new System.Drawing.Point(0, 0);
            this.propertySplitter.Name = "propertySplitter";
            this.propertySplitter.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // propertySplitter.Panel1
            // 
            this.propertySplitter.Panel1.Controls.Add(this.itemTree);
            // 
            // propertySplitter.Panel2
            // 
            this.propertySplitter.Panel2.Controls.Add(this.propertyGrid);
            this.propertySplitter.Size = new System.Drawing.Size(196, 413);
            this.propertySplitter.SplitterDistance = 204;
            this.propertySplitter.TabIndex = 0;
            // 
            // itemTree
            // 
            this.itemTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.itemTree.Location = new System.Drawing.Point(0, 0);
            this.itemTree.Name = "itemTree";
            treeNode1.Name = "levelNode";
            treeNode1.Text = "Level";
            treeNode2.Name = "mapNode";
            treeNode2.Text = "Map";
            treeNode3.Name = "factionsNode";
            treeNode3.Text = "Factions";
            treeNode4.Name = "itemsNode";
            treeNode4.Text = "Items";
            this.itemTree.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3,
            treeNode4});
            this.itemTree.Size = new System.Drawing.Size(196, 204);
            this.itemTree.TabIndex = 0;
            this.itemTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.itemTree_AfterSelect);
            // 
            // gameUpdateTimer
            // 
            this.gameUpdateTimer.Enabled = true;
            this.gameUpdateTimer.Interval = 20;
            this.gameUpdateTimer.Tick += new System.EventHandler(this.gameUpdateTimer_Tick);
            // 
            // scene
            // 
            this.scene.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scene.Location = new System.Drawing.Point(0, 0);
            this.scene.Name = "scene";
            this.scene.Size = new System.Drawing.Size(534, 391);
            this.scene.TabIndex = 2;
            this.scene.Text = "stateSceneControl1";
            // 
            // RenderControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.mainSplitter);
            this.DoubleBuffered = true;
            this.Name = "RenderControl";
            this.Size = new System.Drawing.Size(734, 413);
            this.mainSplitter.Panel1.ResumeLayout(false);
            this.mainSplitter.Panel1.PerformLayout();
            this.mainSplitter.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mainSplitter)).EndInit();
            this.mainSplitter.ResumeLayout(false);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.propertySplitter.Panel1.ResumeLayout(false);
            this.propertySplitter.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.propertySplitter)).EndInit();
            this.propertySplitter.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PropertyGrid propertyGrid;
        private System.Windows.Forms.SplitContainer mainSplitter;
        private System.Windows.Forms.SplitContainer propertySplitter;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel roundLabel;
        private System.Windows.Forms.ToolStripStatusLabel itemLabel;
        private System.Windows.Forms.ToolStripStatusLabel mapLabel;
        private System.Windows.Forms.Timer gameUpdateTimer;
        private System.Windows.Forms.TreeView itemTree;
        private StateSceneControl scene;
    }
}
