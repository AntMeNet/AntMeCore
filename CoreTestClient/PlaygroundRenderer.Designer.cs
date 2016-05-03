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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PlaygroundRenderer));
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.showPlaygroundCheckbox = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.showItemCenterCheckbox = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.showCollisionCheckbox = new System.Windows.Forms.ToolStripMenuItem();
            this.showMovementCheckbox = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.showAttackerCheckbox = new System.Windows.Forms.ToolStripMenuItem();
            this.showViewCheckbox = new System.Windows.Forms.ToolStripMenuItem();
            this.showSmellableCheckbox = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.singleRoundButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.timerDropDown = new System.Windows.Forms.ToolStripDropDownButton();
            this.timerStopButton = new System.Windows.Forms.ToolStripMenuItem();
            this.timer1Button = new System.Windows.Forms.ToolStripMenuItem();
            this.timer10Button = new System.Windows.Forms.ToolStripMenuItem();
            this.timer20Button = new System.Windows.Forms.ToolStripMenuItem();
            this.timer50Button = new System.Windows.Forms.ToolStripMenuItem();
            this.mainSplitter = new System.Windows.Forms.SplitContainer();
            this.renderScreen = new System.Windows.Forms.PictureBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.roundLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.itemLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.mapLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.propertySplitter = new System.Windows.Forms.SplitContainer();
            this.ItemTree = new System.Windows.Forms.TreeView();
            this.gameUpdateTimer = new System.Windows.Forms.Timer(this.components);
            this.timerMaxButton = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip.SuspendLayout();
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
            this.propertyGrid.Size = new System.Drawing.Size(196, 192);
            this.propertyGrid.TabIndex = 0;
            // 
            // toolStrip
            // 
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDropDownButton1,
            this.toolStripSeparator1,
            this.singleRoundButton,
            this.toolStripLabel1,
            this.timerDropDown});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(734, 25);
            this.toolStrip.TabIndex = 1;
            this.toolStrip.Text = "toolStrip1";
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showPlaygroundCheckbox,
            this.toolStripMenuItem1,
            this.showItemCenterCheckbox,
            this.toolStripMenuItem2,
            this.showCollisionCheckbox,
            this.showMovementCheckbox,
            this.toolStripMenuItem3,
            this.showAttackerCheckbox,
            this.showViewCheckbox,
            this.showSmellableCheckbox});
            this.toolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton1.Image")));
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(58, 22);
            this.toolStripDropDownButton1.Text = "Display";
            // 
            // showPlaygroundCheckbox
            // 
            this.showPlaygroundCheckbox.Name = "showPlaygroundCheckbox";
            this.showPlaygroundCheckbox.Size = new System.Drawing.Size(220, 22);
            this.showPlaygroundCheckbox.Text = "Show Playground Info";
            this.showPlaygroundCheckbox.Click += new System.EventHandler(this.showPlaygroundCheckbox_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(217, 6);
            // 
            // showItemCenterCheckbox
            // 
            this.showItemCenterCheckbox.Checked = true;
            this.showItemCenterCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showItemCenterCheckbox.Name = "showItemCenterCheckbox";
            this.showItemCenterCheckbox.Size = new System.Drawing.Size(220, 22);
            this.showItemCenterCheckbox.Text = "Show Item Center";
            this.showItemCenterCheckbox.Click += new System.EventHandler(this.showItemCenterCheckbox_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(217, 6);
            // 
            // showCollisionCheckbox
            // 
            this.showCollisionCheckbox.Checked = true;
            this.showCollisionCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showCollisionCheckbox.Name = "showCollisionCheckbox";
            this.showCollisionCheckbox.Size = new System.Drawing.Size(220, 22);
            this.showCollisionCheckbox.Text = "Show Item Collision Body";
            this.showCollisionCheckbox.Click += new System.EventHandler(this.showCollisionCheckbox_Click);
            // 
            // showMovementCheckbox
            // 
            this.showMovementCheckbox.Checked = true;
            this.showMovementCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showMovementCheckbox.Name = "showMovementCheckbox";
            this.showMovementCheckbox.Size = new System.Drawing.Size(220, 22);
            this.showMovementCheckbox.Text = "Show Item Movement";
            this.showMovementCheckbox.Click += new System.EventHandler(this.showMovementCheckbox_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(217, 6);
            // 
            // showAttackerCheckbox
            // 
            this.showAttackerCheckbox.Checked = true;
            this.showAttackerCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showAttackerCheckbox.Name = "showAttackerCheckbox";
            this.showAttackerCheckbox.Size = new System.Drawing.Size(220, 22);
            this.showAttackerCheckbox.Text = "Show Item Attacker Range";
            this.showAttackerCheckbox.Click += new System.EventHandler(this.showAttackerCheckbox_Click);
            // 
            // showViewCheckbox
            // 
            this.showViewCheckbox.Checked = true;
            this.showViewCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showViewCheckbox.Name = "showViewCheckbox";
            this.showViewCheckbox.Size = new System.Drawing.Size(220, 22);
            this.showViewCheckbox.Text = "Show Item View Range";
            this.showViewCheckbox.Click += new System.EventHandler(this.showViewCheckbox_Click);
            // 
            // showSmellableCheckbox
            // 
            this.showSmellableCheckbox.Checked = true;
            this.showSmellableCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showSmellableCheckbox.Name = "showSmellableCheckbox";
            this.showSmellableCheckbox.Size = new System.Drawing.Size(220, 22);
            this.showSmellableCheckbox.Text = "Show Item Smellable Range";
            this.showSmellableCheckbox.Click += new System.EventHandler(this.showSmellableCheckbox_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // singleRoundButton
            // 
            this.singleRoundButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.singleRoundButton.Image = ((System.Drawing.Image)(resources.GetObject("singleRoundButton.Image")));
            this.singleRoundButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.singleRoundButton.Name = "singleRoundButton";
            this.singleRoundButton.Size = new System.Drawing.Size(81, 22);
            this.singleRoundButton.Text = "Single Round";
            this.singleRoundButton.Click += new System.EventHandler(this.singleRoundButton_Click);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(41, 22);
            this.toolStripLabel1.Text = "Timer:";
            // 
            // timerDropDown
            // 
            this.timerDropDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.timerDropDown.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.timerStopButton,
            this.timer1Button,
            this.timer10Button,
            this.timer20Button,
            this.timer50Button,
            this.timerMaxButton});
            this.timerDropDown.Image = ((System.Drawing.Image)(resources.GetObject("timerDropDown.Image")));
            this.timerDropDown.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.timerDropDown.Name = "timerDropDown";
            this.timerDropDown.Size = new System.Drawing.Size(71, 22);
            this.timerDropDown.Text = "[stopped]";
            // 
            // timerStopButton
            // 
            this.timerStopButton.Name = "timerStopButton";
            this.timerStopButton.Size = new System.Drawing.Size(155, 22);
            this.timerStopButton.Text = "Stop";
            this.timerStopButton.Click += new System.EventHandler(this.timerStopButton_Click);
            // 
            // timer1Button
            // 
            this.timer1Button.Name = "timer1Button";
            this.timer1Button.Size = new System.Drawing.Size(155, 22);
            this.timer1Button.Text = "1 pro Sekunde";
            this.timer1Button.Click += new System.EventHandler(this.timer1Button_Click);
            // 
            // timer10Button
            // 
            this.timer10Button.Name = "timer10Button";
            this.timer10Button.Size = new System.Drawing.Size(155, 22);
            this.timer10Button.Text = "10 pro Sekunde";
            this.timer10Button.Click += new System.EventHandler(this.timer10Button_Click);
            // 
            // timer20Button
            // 
            this.timer20Button.Name = "timer20Button";
            this.timer20Button.Size = new System.Drawing.Size(155, 22);
            this.timer20Button.Text = "20 pro Sekunde";
            this.timer20Button.Click += new System.EventHandler(this.timer20Button_Click);
            // 
            // timer50Button
            // 
            this.timer50Button.Name = "timer50Button";
            this.timer50Button.Size = new System.Drawing.Size(155, 22);
            this.timer50Button.Text = "50 pro Sekunde";
            this.timer50Button.Click += new System.EventHandler(this.timer50Button_Click);
            // 
            // mainSplitter
            // 
            this.mainSplitter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainSplitter.Location = new System.Drawing.Point(0, 25);
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
            this.mainSplitter.Size = new System.Drawing.Size(734, 388);
            this.mainSplitter.SplitterDistance = 534;
            this.mainSplitter.TabIndex = 2;
            // 
            // renderScreen
            // 
            this.renderScreen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.renderScreen.Location = new System.Drawing.Point(0, 0);
            this.renderScreen.Name = "renderScreen";
            this.renderScreen.Size = new System.Drawing.Size(534, 366);
            this.renderScreen.TabIndex = 0;
            this.renderScreen.TabStop = false;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.roundLabel,
            this.itemLabel,
            this.mapLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 366);
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
            this.propertySplitter.Size = new System.Drawing.Size(196, 388);
            this.propertySplitter.SplitterDistance = 192;
            this.propertySplitter.TabIndex = 0;
            // 
            // ItemTree
            // 
            this.ItemTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ItemTree.Location = new System.Drawing.Point(0, 0);
            this.ItemTree.Name = "ItemTree";
            this.ItemTree.Size = new System.Drawing.Size(196, 192);
            this.ItemTree.TabIndex = 0;
            this.ItemTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.itemTree_AfterSelect);
            // 
            // gameUpdateTimer
            // 
            this.gameUpdateTimer.Interval = 20;
            this.gameUpdateTimer.Tick += new System.EventHandler(this.gameUpdateTimer_Tick);
            // 
            // timerMaxButton
            // 
            this.timerMaxButton.Name = "timerMaxButton";
            this.timerMaxButton.Size = new System.Drawing.Size(155, 22);
            this.timerMaxButton.Text = "Max";
            this.timerMaxButton.Click += new System.EventHandler(this.timerMaxButton_Click);
            // 
            // PlaygroundRenderer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.mainSplitter);
            this.Controls.Add(this.toolStrip);
            this.DoubleBuffered = true;
            this.Name = "PlaygroundRenderer";
            this.Size = new System.Drawing.Size(734, 413);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
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
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PropertyGrid propertyGrid;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.SplitContainer mainSplitter;
        private System.Windows.Forms.PictureBox renderScreen;
        private System.Windows.Forms.SplitContainer propertySplitter;
        protected System.Windows.Forms.TreeView ItemTree;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel roundLabel;
        private System.Windows.Forms.ToolStripStatusLabel itemLabel;
        private System.Windows.Forms.ToolStripStatusLabel mapLabel;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem showPlaygroundCheckbox;
        private System.Windows.Forms.ToolStripMenuItem showItemCenterCheckbox;
        private System.Windows.Forms.ToolStripMenuItem showCollisionCheckbox;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.Timer gameUpdateTimer;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripButton singleRoundButton;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripDropDownButton timerDropDown;
        private System.Windows.Forms.ToolStripMenuItem timerStopButton;
        private System.Windows.Forms.ToolStripMenuItem timer1Button;
        private System.Windows.Forms.ToolStripMenuItem timer10Button;
        private System.Windows.Forms.ToolStripMenuItem timer20Button;
        private System.Windows.Forms.ToolStripMenuItem timer50Button;
        private System.Windows.Forms.ToolStripMenuItem showAttackerCheckbox;
        private System.Windows.Forms.ToolStripMenuItem showViewCheckbox;
        private System.Windows.Forms.ToolStripMenuItem showSmellableCheckbox;
        private System.Windows.Forms.ToolStripMenuItem showMovementCheckbox;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem timerMaxButton;
    }
}
