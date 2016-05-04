namespace CoreTestClient
{
    partial class PlaygroundRenderer
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
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            this.mainSplitter = new System.Windows.Forms.SplitContainer();
            this.renderScreen = new System.Windows.Forms.PictureBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.roundLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.itemLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.mapLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.propertySplitter = new System.Windows.Forms.SplitContainer();
            this.ItemTree = new System.Windows.Forms.TreeView();
            this.gameUpdateTimer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.mainSplitter)).BeginInit();
            this.mainSplitter.Panel1.SuspendLayout();
            this.mainSplitter.Panel2.SuspendLayout();
            this.mainSplitter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.renderScreen)).BeginInit();
            this.statusStrip1.SuspendLayout();
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
            this.mainSplitter.Panel1.Controls.Add(this.renderScreen);
            this.mainSplitter.Panel1.Controls.Add(this.statusStrip1);
            // 
            // mainSplitter.Panel2
            // 
            this.mainSplitter.Panel2.Controls.Add(this.propertySplitter);
            this.mainSplitter.Size = new System.Drawing.Size(734, 413);
            this.mainSplitter.SplitterDistance = 534;
            this.mainSplitter.TabIndex = 2;
            // 
            // renderScreen
            // 
            this.renderScreen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.renderScreen.Location = new System.Drawing.Point(0, 0);
            this.renderScreen.Name = "renderScreen";
            this.renderScreen.Size = new System.Drawing.Size(534, 391);
            this.renderScreen.TabIndex = 0;
            this.renderScreen.TabStop = false;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.roundLabel,
            this.itemLabel,
            this.mapLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 391);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(534, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
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
            this.propertySplitter.Panel1.Controls.Add(this.ItemTree);
            // 
            // propertySplitter.Panel2
            // 
            this.propertySplitter.Panel2.Controls.Add(this.propertyGrid);
            this.propertySplitter.Size = new System.Drawing.Size(196, 413);
            this.propertySplitter.SplitterDistance = 204;
            this.propertySplitter.TabIndex = 0;
            // 
            // ItemTree
            // 
            this.ItemTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ItemTree.Location = new System.Drawing.Point(0, 0);
            this.ItemTree.Name = "ItemTree";
            this.ItemTree.Size = new System.Drawing.Size(196, 204);
            this.ItemTree.TabIndex = 0;
            this.ItemTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.itemTree_AfterSelect);
            // 
            // gameUpdateTimer
            // 
            this.gameUpdateTimer.Interval = 20;
            // 
            // PlaygroundRenderer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.mainSplitter);
            this.DoubleBuffered = true;
            this.Name = "PlaygroundRenderer";
            this.Size = new System.Drawing.Size(734, 413);
            this.mainSplitter.Panel1.ResumeLayout(false);
            this.mainSplitter.Panel1.PerformLayout();
            this.mainSplitter.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mainSplitter)).EndInit();
            this.mainSplitter.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.renderScreen)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.propertySplitter.Panel1.ResumeLayout(false);
            this.propertySplitter.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.propertySplitter)).EndInit();
            this.propertySplitter.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PropertyGrid propertyGrid;
        private System.Windows.Forms.SplitContainer mainSplitter;
        private System.Windows.Forms.PictureBox renderScreen;
        private System.Windows.Forms.SplitContainer propertySplitter;
        protected System.Windows.Forms.TreeView ItemTree;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel roundLabel;
        private System.Windows.Forms.ToolStripStatusLabel itemLabel;
        private System.Windows.Forms.ToolStripStatusLabel mapLabel;
        private System.Windows.Forms.Timer gameUpdateTimer;
    }
}
