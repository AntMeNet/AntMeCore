namespace CoreTestClient
{
    partial class ExtensionsForm
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("ExtensionPacks");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Engine Properties");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("ItemProperties");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Item Attachment Properties");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("Item Extender");
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("Items", new System.Windows.Forms.TreeNode[] {
            treeNode3,
            treeNode4,
            treeNode5});
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("Faction Properties");
            System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("Faction Attachment Properties");
            System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("Faction Extender");
            System.Windows.Forms.TreeNode treeNode10 = new System.Windows.Forms.TreeNode("Factions", new System.Windows.Forms.TreeNode[] {
            treeNode7,
            treeNode8,
            treeNode9});
            System.Windows.Forms.TreeNode treeNode11 = new System.Windows.Forms.TreeNode("Factory Interop Attachment Properties");
            System.Windows.Forms.TreeNode treeNode12 = new System.Windows.Forms.TreeNode("Factory Interop Extender");
            System.Windows.Forms.TreeNode treeNode13 = new System.Windows.Forms.TreeNode("Factory Interop", new System.Windows.Forms.TreeNode[] {
            treeNode11,
            treeNode12});
            System.Windows.Forms.TreeNode treeNode14 = new System.Windows.Forms.TreeNode("Unit Interop Attachment Properties");
            System.Windows.Forms.TreeNode treeNode15 = new System.Windows.Forms.TreeNode("Unit Interop Extender");
            System.Windows.Forms.TreeNode treeNode16 = new System.Windows.Forms.TreeNode("Unit Interop", new System.Windows.Forms.TreeNode[] {
            treeNode14,
            treeNode15});
            System.Windows.Forms.TreeNode treeNode17 = new System.Windows.Forms.TreeNode("Level Properties");
            System.Windows.Forms.TreeNode treeNode18 = new System.Windows.Forms.TreeNode("Level Extender");
            System.Windows.Forms.TreeNode treeNode19 = new System.Windows.Forms.TreeNode("Level", new System.Windows.Forms.TreeNode[] {
            treeNode17,
            treeNode18});
            this.splitter = new System.Windows.Forms.SplitContainer();
            this.treeView = new System.Windows.Forms.TreeView();
            this.listView = new System.Windows.Forms.ListView();
            this.nameColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.typeColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.packColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.closeButton = new System.Windows.Forms.Button();
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
            this.splitter.Panel1.Controls.Add(this.treeView);
            // 
            // splitter.Panel2
            // 
            this.splitter.Panel2.Controls.Add(this.listView);
            this.splitter.Size = new System.Drawing.Size(589, 331);
            this.splitter.SplitterDistance = 196;
            this.splitter.TabIndex = 0;
            // 
            // treeView
            // 
            this.treeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView.Location = new System.Drawing.Point(0, 0);
            this.treeView.Name = "treeView";
            treeNode1.Name = "extensionPacksNode";
            treeNode1.Text = "ExtensionPacks";
            treeNode2.Name = "enginePropertiesNode";
            treeNode2.Text = "Engine Properties";
            treeNode3.Name = "itemPropertiesNode";
            treeNode3.Text = "ItemProperties";
            treeNode4.Name = "itemAttachmentPropertiesNode";
            treeNode4.Text = "Item Attachment Properties";
            treeNode5.Name = "itemExtenderNode";
            treeNode5.Text = "Item Extender";
            treeNode6.Name = "itemsNode";
            treeNode6.Text = "Items";
            treeNode7.Name = "factionPropertiesNode";
            treeNode7.Text = "Faction Properties";
            treeNode8.Name = "factionAttachmentPropertiesNode";
            treeNode8.Text = "Faction Attachment Properties";
            treeNode9.Name = "factionExtenderNode";
            treeNode9.Text = "Faction Extender";
            treeNode10.Name = "factionsNode";
            treeNode10.Text = "Factions";
            treeNode11.Name = "factoryInteropAttachmentPropertiesNode";
            treeNode11.Text = "Factory Interop Attachment Properties";
            treeNode12.Name = "factoryInteropExtenderNode";
            treeNode12.Text = "Factory Interop Extender";
            treeNode13.Name = "factoryInteropNode";
            treeNode13.Text = "Factory Interop";
            treeNode14.Name = "unitInteropAttachmentPropertiesNode";
            treeNode14.Text = "Unit Interop Attachment Properties";
            treeNode15.Name = "unitInteropExtenderNode";
            treeNode15.Text = "Unit Interop Extender";
            treeNode16.Name = "unitInteropNode";
            treeNode16.Text = "Unit Interop";
            treeNode17.Name = "levelPropertiesNode";
            treeNode17.Text = "Level Properties";
            treeNode18.Name = "levelExtenderNode";
            treeNode18.Text = "Level Extender";
            treeNode19.Name = "levelNode";
            treeNode19.Text = "Level";
            this.treeView.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode6,
            treeNode10,
            treeNode13,
            treeNode16,
            treeNode19});
            this.treeView.Size = new System.Drawing.Size(196, 331);
            this.treeView.TabIndex = 0;
            this.treeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterSelect);
            // 
            // listView
            // 
            this.listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.nameColumn,
            this.typeColumn,
            this.packColumn});
            this.listView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView.Location = new System.Drawing.Point(0, 0);
            this.listView.Name = "listView";
            this.listView.Size = new System.Drawing.Size(389, 331);
            this.listView.TabIndex = 0;
            this.listView.UseCompatibleStateImageBehavior = false;
            this.listView.View = System.Windows.Forms.View.Details;
            // 
            // nameColumn
            // 
            this.nameColumn.Text = "Name";
            this.nameColumn.Width = 87;
            // 
            // typeColumn
            // 
            this.typeColumn.Text = "Type";
            this.typeColumn.Width = 102;
            // 
            // packColumn
            // 
            this.packColumn.Text = "Extension Pack";
            this.packColumn.Width = 108;
            // 
            // closeButton
            // 
            this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.closeButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.closeButton.Location = new System.Drawing.Point(526, 349);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(75, 23);
            this.closeButton.TabIndex = 1;
            this.closeButton.Text = "Close";
            this.closeButton.UseVisualStyleBackColor = true;
            // 
            // ExtensionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.closeButton;
            this.ClientSize = new System.Drawing.Size(613, 384);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.splitter);
            this.Name = "ExtensionsForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Loaded Extensions";
            this.splitter.Panel1.ResumeLayout(false);
            this.splitter.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitter)).EndInit();
            this.splitter.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitter;
        private System.Windows.Forms.TreeView treeView;
        private System.Windows.Forms.ListView listView;
        private System.Windows.Forms.ColumnHeader nameColumn;
        private System.Windows.Forms.ColumnHeader typeColumn;
        private System.Windows.Forms.ColumnHeader packColumn;
        private System.Windows.Forms.Button closeButton;
    }
}