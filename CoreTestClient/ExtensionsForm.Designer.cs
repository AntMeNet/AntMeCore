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
            System.Windows.Forms.TreeNode treeNode21 = new System.Windows.Forms.TreeNode("ExtensionPacks");
            System.Windows.Forms.TreeNode treeNode22 = new System.Windows.Forms.TreeNode("Engine Properties");
            System.Windows.Forms.TreeNode treeNode23 = new System.Windows.Forms.TreeNode("ItemProperties");
            System.Windows.Forms.TreeNode treeNode24 = new System.Windows.Forms.TreeNode("Item Attachment Properties");
            System.Windows.Forms.TreeNode treeNode25 = new System.Windows.Forms.TreeNode("Item Extender");
            System.Windows.Forms.TreeNode treeNode26 = new System.Windows.Forms.TreeNode("Items", new System.Windows.Forms.TreeNode[] {
            treeNode23,
            treeNode24,
            treeNode25});
            System.Windows.Forms.TreeNode treeNode27 = new System.Windows.Forms.TreeNode("Faction Properties");
            System.Windows.Forms.TreeNode treeNode28 = new System.Windows.Forms.TreeNode("Faction Attachment Properties");
            System.Windows.Forms.TreeNode treeNode29 = new System.Windows.Forms.TreeNode("Faction Extender");
            System.Windows.Forms.TreeNode treeNode30 = new System.Windows.Forms.TreeNode("Factions", new System.Windows.Forms.TreeNode[] {
            treeNode27,
            treeNode28,
            treeNode29});
            System.Windows.Forms.TreeNode treeNode31 = new System.Windows.Forms.TreeNode("Factory Interop Properties");
            System.Windows.Forms.TreeNode treeNode32 = new System.Windows.Forms.TreeNode("Factory Interop Attachment Properties");
            System.Windows.Forms.TreeNode treeNode33 = new System.Windows.Forms.TreeNode("Factory Interop Extender");
            System.Windows.Forms.TreeNode treeNode34 = new System.Windows.Forms.TreeNode("Unit Interop Properties");
            System.Windows.Forms.TreeNode treeNode35 = new System.Windows.Forms.TreeNode("Unit Interop Attachment Properties");
            System.Windows.Forms.TreeNode treeNode36 = new System.Windows.Forms.TreeNode("Unit Interop Extender");
            System.Windows.Forms.TreeNode treeNode37 = new System.Windows.Forms.TreeNode("Interop", new System.Windows.Forms.TreeNode[] {
            treeNode31,
            treeNode32,
            treeNode33,
            treeNode34,
            treeNode35,
            treeNode36});
            System.Windows.Forms.TreeNode treeNode38 = new System.Windows.Forms.TreeNode("Level Properties");
            System.Windows.Forms.TreeNode treeNode39 = new System.Windows.Forms.TreeNode("Level Extender");
            System.Windows.Forms.TreeNode treeNode40 = new System.Windows.Forms.TreeNode("Level", new System.Windows.Forms.TreeNode[] {
            treeNode38,
            treeNode39});
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
            treeNode21.Name = "extensionPacksNode";
            treeNode21.Text = "ExtensionPacks";
            treeNode22.Name = "enginePropertiesNode";
            treeNode22.Text = "Engine Properties";
            treeNode23.Name = "itemPropertiesNode";
            treeNode23.Text = "ItemProperties";
            treeNode24.Name = "itemAttachmentPropertiesNode";
            treeNode24.Text = "Item Attachment Properties";
            treeNode25.Name = "itemExtenderNode";
            treeNode25.Text = "Item Extender";
            treeNode26.Name = "itemsNode";
            treeNode26.Text = "Items";
            treeNode27.Name = "factionPropertiesNode";
            treeNode27.Text = "Faction Properties";
            treeNode28.Name = "factionAttachmentPropertiesNode";
            treeNode28.Text = "Faction Attachment Properties";
            treeNode29.Name = "factionExtenderNode";
            treeNode29.Text = "Faction Extender";
            treeNode30.Name = "factionsNode";
            treeNode30.Text = "Factions";
            treeNode31.Name = "factoryInteropPropertiesNode";
            treeNode31.Text = "Factory Interop Properties";
            treeNode32.Name = "factoryInteropAttachmentPropertiesNode";
            treeNode32.Text = "Factory Interop Attachment Properties";
            treeNode33.Name = "factoryInteropExtenderNode";
            treeNode33.Text = "Factory Interop Extender";
            treeNode34.Name = "unitInteropPropertiesNode";
            treeNode34.Text = "Unit Interop Properties";
            treeNode35.Name = "unitInteropAttachmentPropertiesNode";
            treeNode35.Text = "Unit Interop Attachment Properties";
            treeNode36.Name = "unitInteropExtenderNode";
            treeNode36.Text = "Unit Interop Extender";
            treeNode37.Name = "interopNode";
            treeNode37.Text = "Interop";
            treeNode38.Name = "levelPropertiesNode";
            treeNode38.Text = "Level Properties";
            treeNode39.Name = "levelExtenderNode";
            treeNode39.Text = "Level Extender";
            treeNode40.Name = "levelNode";
            treeNode40.Text = "Level";
            this.treeView.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode21,
            treeNode22,
            treeNode26,
            treeNode30,
            treeNode37,
            treeNode40});
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