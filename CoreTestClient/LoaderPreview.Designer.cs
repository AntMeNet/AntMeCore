namespace AntMe.Runtime.Debug
{
    partial class LoaderPreview
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Extension Packs");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Engine Extensions");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Item Properties");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Items");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("Levels");
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("Campaigns");
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("Factions");
            System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("Players");
            System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("Messages");
            System.Windows.Forms.TreeNode treeNode10 = new System.Windows.Forms.TreeNode("Loader Info", new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3,
            treeNode4,
            treeNode5,
            treeNode6,
            treeNode7,
            treeNode8,
            treeNode9});
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.treeView = new System.Windows.Forms.TreeView();
            this.outputPanel = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.treeView);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.outputPanel);
            this.splitContainer1.Size = new System.Drawing.Size(721, 408);
            this.splitContainer1.SplitterDistance = 236;
            this.splitContainer1.TabIndex = 0;
            // 
            // treeView
            // 
            this.treeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView.Location = new System.Drawing.Point(0, 0);
            this.treeView.Name = "treeView";
            treeNode1.Name = "extensionPacksNode";
            treeNode1.Text = "Extension Packs";
            treeNode2.Name = "engineExtensionsNode";
            treeNode2.Text = "Engine Extensions";
            treeNode3.Name = "propertiesNode";
            treeNode3.Text = "Item Properties";
            treeNode4.Name = "itemsNode";
            treeNode4.Text = "Items";
            treeNode5.Name = "levelsNode";
            treeNode5.Text = "Levels";
            treeNode6.Name = "campaignsNode";
            treeNode6.Text = "Campaigns";
            treeNode7.Name = "factionsNode";
            treeNode7.Text = "Factions";
            treeNode8.Name = "playersNode";
            treeNode8.Text = "Players";
            treeNode9.Name = "messagesNode";
            treeNode9.Text = "Messages";
            treeNode10.Name = "rootNode";
            treeNode10.Text = "Loader Info";
            this.treeView.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode10});
            this.treeView.Size = new System.Drawing.Size(236, 408);
            this.treeView.TabIndex = 0;
            // 
            // outputPanel
            // 
            this.outputPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.outputPanel.Location = new System.Drawing.Point(0, 0);
            this.outputPanel.Name = "outputPanel";
            this.outputPanel.Size = new System.Drawing.Size(481, 408);
            this.outputPanel.TabIndex = 0;
            // 
            // LoaderPreview
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "LoaderPreview";
            this.Size = new System.Drawing.Size(721, 408);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView treeView;
        private System.Windows.Forms.Panel outputPanel;
    }
}
