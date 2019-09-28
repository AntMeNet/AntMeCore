namespace CoreTestClient
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.mainMenu = new System.Windows.Forms.MenuStrip();
            this.programMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.singleGameMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.closeMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.loaderMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.extensionsMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.globalSettingsMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.codeGeneratorMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.localizationMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.mapEditorMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.stateLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.timeLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.mainPanel = new System.Windows.Forms.Panel();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.startToolButton = new System.Windows.Forms.ToolStripButton();
            this.framesToolButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.frames1ToolButton = new System.Windows.Forms.ToolStripMenuItem();
            this.frames5ToolButton = new System.Windows.Forms.ToolStripMenuItem();
            this.frames10ToolButton = new System.Windows.Forms.ToolStripMenuItem();
            this.frames20ToolButton = new System.Windows.Forms.ToolStripMenuItem();
            this.frames40ToolButton = new System.Windows.Forms.ToolStripMenuItem();
            this.frames80ToolButton = new System.Windows.Forms.ToolStripMenuItem();
            this.framesMaxToolButton = new System.Windows.Forms.ToolStripMenuItem();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.serverGameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mainMenu.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenu
            // 
            this.mainMenu.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.programMenu,
            this.loaderMenu,
            this.mapEditorMenu});
            this.mainMenu.Location = new System.Drawing.Point(0, 0);
            this.mainMenu.Name = "mainMenu";
            this.mainMenu.Padding = new System.Windows.Forms.Padding(9, 3, 0, 3);
            this.mainMenu.Size = new System.Drawing.Size(1176, 35);
            this.mainMenu.TabIndex = 0;
            this.mainMenu.Text = "menuStrip1";
            // 
            // programMenu
            // 
            this.programMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.singleGameMenu,
            this.serverGameToolStripMenuItem,
            this.toolStripMenuItem1,
            this.closeMenu});
            this.programMenu.Name = "programMenu";
            this.programMenu.Size = new System.Drawing.Size(97, 29);
            this.programMenu.Text = "Program";
            // 
            // singleGameMenu
            // 
            this.singleGameMenu.Name = "singleGameMenu";
            this.singleGameMenu.Size = new System.Drawing.Size(270, 34);
            this.singleGameMenu.Text = "Single Game";
            this.singleGameMenu.Click += new System.EventHandler(this.singleGameMenu_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(267, 6);
            // 
            // closeMenu
            // 
            this.closeMenu.Name = "closeMenu";
            this.closeMenu.Size = new System.Drawing.Size(270, 34);
            this.closeMenu.Text = "Close";
            this.closeMenu.Click += new System.EventHandler(this.closeMenu_Click);
            // 
            // loaderMenu
            // 
            this.loaderMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.extensionsMenu,
            this.globalSettingsMenu,
            this.codeGeneratorMenu,
            this.localizationMenu});
            this.loaderMenu.Name = "loaderMenu";
            this.loaderMenu.Size = new System.Drawing.Size(82, 29);
            this.loaderMenu.Text = "Loader";
            // 
            // extensionsMenu
            // 
            this.extensionsMenu.Name = "extensionsMenu";
            this.extensionsMenu.Size = new System.Drawing.Size(291, 34);
            this.extensionsMenu.Text = "Extensions";
            this.extensionsMenu.Click += new System.EventHandler(this.extensionsMenu_Click);
            // 
            // globalSettingsMenu
            // 
            this.globalSettingsMenu.Name = "globalSettingsMenu";
            this.globalSettingsMenu.Size = new System.Drawing.Size(291, 34);
            this.globalSettingsMenu.Text = "Global Settings";
            this.globalSettingsMenu.Click += new System.EventHandler(this.globalSettingsMenu_Click);
            // 
            // codeGeneratorMenu
            // 
            this.codeGeneratorMenu.Name = "codeGeneratorMenu";
            this.codeGeneratorMenu.Size = new System.Drawing.Size(291, 34);
            this.codeGeneratorMenu.Text = "Code Generator";
            this.codeGeneratorMenu.Click += new System.EventHandler(this.codeGeneratorMenu_Click);
            // 
            // localizationMenu
            // 
            this.localizationMenu.Name = "localizationMenu";
            this.localizationMenu.Size = new System.Drawing.Size(291, 34);
            this.localizationMenu.Text = "Localization Dictionary";
            this.localizationMenu.Click += new System.EventHandler(this.localizationMenu_Click);
            // 
            // mapEditorMenu
            // 
            this.mapEditorMenu.Name = "mapEditorMenu";
            this.mapEditorMenu.Size = new System.Drawing.Size(116, 29);
            this.mapEditorMenu.Text = "Map Editor";
            this.mapEditorMenu.Click += new System.EventHandler(this.mapEditorMenu_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.stateLabel,
            this.timeLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 646);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Padding = new System.Windows.Forms.Padding(2, 0, 21, 0);
            this.statusStrip.Size = new System.Drawing.Size(1176, 32);
            this.statusStrip.TabIndex = 1;
            this.statusStrip.Text = "statusStrip1";
            // 
            // stateLabel
            // 
            this.stateLabel.Name = "stateLabel";
            this.stateLabel.Size = new System.Drawing.Size(60, 25);
            this.stateLabel.Text = "[state]";
            // 
            // timeLabel
            // 
            this.timeLabel.Name = "timeLabel";
            this.timeLabel.Size = new System.Drawing.Size(57, 25);
            this.timeLabel.Text = "[time]";
            // 
            // mainPanel
            // 
            this.mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainPanel.Location = new System.Drawing.Point(0, 69);
            this.mainPanel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(1176, 577);
            this.mainPanel.TabIndex = 2;
            // 
            // toolStrip
            // 
            this.toolStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.startToolButton,
            this.framesToolButton});
            this.toolStrip.Location = new System.Drawing.Point(0, 35);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Padding = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.toolStrip.Size = new System.Drawing.Size(1176, 34);
            this.toolStrip.TabIndex = 3;
            this.toolStrip.Text = "toolStrip1";
            // 
            // startToolButton
            // 
            this.startToolButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.startToolButton.Image = ((System.Drawing.Image)(resources.GetObject("startToolButton.Image")));
            this.startToolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.startToolButton.Name = "startToolButton";
            this.startToolButton.Size = new System.Drawing.Size(52, 29);
            this.startToolButton.Text = "Start";
            this.startToolButton.Click += new System.EventHandler(this.startToolButton_Click);
            // 
            // framesToolButton
            // 
            this.framesToolButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.framesToolButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.frames1ToolButton,
            this.frames5ToolButton,
            this.frames10ToolButton,
            this.frames20ToolButton,
            this.frames40ToolButton,
            this.frames80ToolButton,
            this.framesMaxToolButton});
            this.framesToolButton.Image = ((System.Drawing.Image)(resources.GetObject("framesToolButton.Image")));
            this.framesToolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.framesToolButton.Name = "framesToolButton";
            this.framesToolButton.Size = new System.Drawing.Size(80, 29);
            this.framesToolButton.Text = "20 fps";
            // 
            // frames1ToolButton
            // 
            this.frames1ToolButton.Name = "frames1ToolButton";
            this.frames1ToolButton.Size = new System.Drawing.Size(234, 34);
            this.frames1ToolButton.Text = "1 fps";
            this.frames1ToolButton.Click += new System.EventHandler(this.frames1ToolButton_Click);
            // 
            // frames5ToolButton
            // 
            this.frames5ToolButton.Name = "frames5ToolButton";
            this.frames5ToolButton.Size = new System.Drawing.Size(234, 34);
            this.frames5ToolButton.Text = "5 fps";
            this.frames5ToolButton.Click += new System.EventHandler(this.frames5ToolButton_Click);
            // 
            // frames10ToolButton
            // 
            this.frames10ToolButton.Name = "frames10ToolButton";
            this.frames10ToolButton.Size = new System.Drawing.Size(234, 34);
            this.frames10ToolButton.Text = "10 fps";
            this.frames10ToolButton.Click += new System.EventHandler(this.frames10ToolButton_Click);
            // 
            // frames20ToolButton
            // 
            this.frames20ToolButton.Name = "frames20ToolButton";
            this.frames20ToolButton.Size = new System.Drawing.Size(234, 34);
            this.frames20ToolButton.Text = "20 fps (default)";
            this.frames20ToolButton.Click += new System.EventHandler(this.frames20ToolButton_Click);
            // 
            // frames40ToolButton
            // 
            this.frames40ToolButton.Name = "frames40ToolButton";
            this.frames40ToolButton.Size = new System.Drawing.Size(234, 34);
            this.frames40ToolButton.Text = "40 fps";
            this.frames40ToolButton.Click += new System.EventHandler(this.frames40ToolButton_Click);
            // 
            // frames80ToolButton
            // 
            this.frames80ToolButton.Name = "frames80ToolButton";
            this.frames80ToolButton.Size = new System.Drawing.Size(234, 34);
            this.frames80ToolButton.Text = "80 fps";
            this.frames80ToolButton.Click += new System.EventHandler(this.frames80ToolButton_Click);
            // 
            // framesMaxToolButton
            // 
            this.framesMaxToolButton.Name = "framesMaxToolButton";
            this.framesMaxToolButton.Size = new System.Drawing.Size(234, 34);
            this.framesMaxToolButton.Text = "Max";
            this.framesMaxToolButton.Click += new System.EventHandler(this.framesMaxToolButton_Click);
            // 
            // timer
            // 
            this.timer.Enabled = true;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // serverGameToolStripMenuItem
            // 
            this.serverGameToolStripMenuItem.Name = "serverGameToolStripMenuItem";
            this.serverGameToolStripMenuItem.Size = new System.Drawing.Size(270, 34);
            this.serverGameToolStripMenuItem.Text = "Server Game";
            this.serverGameToolStripMenuItem.Click += new System.EventHandler(this.ServerGameToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1176, 678);
            this.Controls.Add(this.mainPanel);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.mainMenu);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.mainMenu;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(1189, 708);
            this.Name = "MainForm";
            this.Text = "AntMe! Core Client";
            this.mainMenu.ResumeLayout(false);
            this.mainMenu.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mainMenu;
        private System.Windows.Forms.ToolStripMenuItem programMenu;
        private System.Windows.Forms.ToolStripMenuItem closeMenu;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.Panel mainPanel;
        private System.Windows.Forms.ToolStripMenuItem loaderMenu;
        private System.Windows.Forms.ToolStripMenuItem extensionsMenu;
        private System.Windows.Forms.ToolStripMenuItem globalSettingsMenu;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton startToolButton;
        private System.Windows.Forms.ToolStripMenuItem singleGameMenu;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.ToolStripStatusLabel stateLabel;
        private System.Windows.Forms.ToolStripStatusLabel timeLabel;
        private System.Windows.Forms.ToolStripMenuItem codeGeneratorMenu;
        private System.Windows.Forms.ToolStripDropDownButton framesToolButton;
        private System.Windows.Forms.ToolStripMenuItem frames1ToolButton;
        private System.Windows.Forms.ToolStripMenuItem frames5ToolButton;
        private System.Windows.Forms.ToolStripMenuItem frames10ToolButton;
        private System.Windows.Forms.ToolStripMenuItem frames20ToolButton;
        private System.Windows.Forms.ToolStripMenuItem frames40ToolButton;
        private System.Windows.Forms.ToolStripMenuItem frames80ToolButton;
        private System.Windows.Forms.ToolStripMenuItem framesMaxToolButton;
        private System.Windows.Forms.ToolStripMenuItem localizationMenu;
        private System.Windows.Forms.ToolStripMenuItem mapEditorMenu;
        private System.Windows.Forms.ToolStripMenuItem serverGameToolStripMenuItem;
    }
}

