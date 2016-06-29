namespace CoreTestClient
{
    partial class CodeGeneratorForm
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
            this.generateButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.regenerateButton = new System.Windows.Forms.Button();
            this.factionCombo = new System.Windows.Forms.ComboBox();
            this.languageCombo = new System.Windows.Forms.ComboBox();
            this.programmingLanguageCombo = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.authorTextBox = new System.Windows.Forms.TextBox();
            this.outputTextBox = new System.Windows.Forms.TextBox();
            this.browseButton = new System.Windows.Forms.Button();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.environmentComboBox = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // generateButton
            // 
            this.generateButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.generateButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.generateButton.Location = new System.Drawing.Point(576, 306);
            this.generateButton.Name = "generateButton";
            this.generateButton.Size = new System.Drawing.Size(75, 23);
            this.generateButton.TabIndex = 0;
            this.generateButton.Text = "Generate";
            this.generateButton.UseVisualStyleBackColor = true;
            this.generateButton.Click += new System.EventHandler(this.generateButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(495, 306);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 1;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // regenerateButton
            // 
            this.regenerateButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.regenerateButton.Location = new System.Drawing.Point(507, 12);
            this.regenerateButton.Name = "regenerateButton";
            this.regenerateButton.Size = new System.Drawing.Size(144, 23);
            this.regenerateButton.TabIndex = 2;
            this.regenerateButton.Text = "Regenerate Localization";
            this.regenerateButton.UseVisualStyleBackColor = true;
            this.regenerateButton.Click += new System.EventHandler(this.regenerateButton_Click);
            // 
            // factionCombo
            // 
            this.factionCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.factionCombo.FormattingEnabled = true;
            this.factionCombo.Location = new System.Drawing.Point(181, 138);
            this.factionCombo.Name = "factionCombo";
            this.factionCombo.Size = new System.Drawing.Size(132, 21);
            this.factionCombo.TabIndex = 3;
            // 
            // languageCombo
            // 
            this.languageCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.languageCombo.FormattingEnabled = true;
            this.languageCombo.Location = new System.Drawing.Point(181, 165);
            this.languageCombo.Name = "languageCombo";
            this.languageCombo.Size = new System.Drawing.Size(132, 21);
            this.languageCombo.TabIndex = 4;
            // 
            // programmingLanguageCombo
            // 
            this.programmingLanguageCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.programmingLanguageCombo.FormattingEnabled = true;
            this.programmingLanguageCombo.Location = new System.Drawing.Point(181, 192);
            this.programmingLanguageCombo.Name = "programmingLanguageCombo";
            this.programmingLanguageCombo.Size = new System.Drawing.Size(132, 21);
            this.programmingLanguageCombo.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(38, 141);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Faction";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(38, 168);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Language";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(38, 89);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Name";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(38, 195);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(119, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Programming Language";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(38, 115);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(38, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Author";
            // 
            // nameTextBox
            // 
            this.nameTextBox.Location = new System.Drawing.Point(181, 86);
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size(166, 20);
            this.nameTextBox.TabIndex = 11;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(38, 249);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(71, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "Output Folder";
            // 
            // authorTextBox
            // 
            this.authorTextBox.Location = new System.Drawing.Point(181, 112);
            this.authorTextBox.Name = "authorTextBox";
            this.authorTextBox.Size = new System.Drawing.Size(166, 20);
            this.authorTextBox.TabIndex = 13;
            // 
            // outputTextBox
            // 
            this.outputTextBox.Location = new System.Drawing.Point(181, 246);
            this.outputTextBox.Name = "outputTextBox";
            this.outputTextBox.ReadOnly = true;
            this.outputTextBox.Size = new System.Drawing.Size(394, 20);
            this.outputTextBox.TabIndex = 14;
            // 
            // browseButton
            // 
            this.browseButton.Location = new System.Drawing.Point(581, 246);
            this.browseButton.Name = "browseButton";
            this.browseButton.Size = new System.Drawing.Size(36, 20);
            this.browseButton.TabIndex = 15;
            this.browseButton.Text = "...";
            this.browseButton.UseVisualStyleBackColor = true;
            this.browseButton.Click += new System.EventHandler(this.browseButton_Click);
            // 
            // environmentComboBox
            // 
            this.environmentComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.environmentComboBox.FormattingEnabled = true;
            this.environmentComboBox.Location = new System.Drawing.Point(181, 219);
            this.environmentComboBox.Name = "environmentComboBox";
            this.environmentComboBox.Size = new System.Drawing.Size(132, 21);
            this.environmentComboBox.TabIndex = 16;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(38, 222);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(25, 13);
            this.label7.TabIndex = 17;
            this.label7.Text = "IDE";
            // 
            // CodeGeneratorForm
            // 
            this.AcceptButton = this.generateButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(663, 341);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.environmentComboBox);
            this.Controls.Add(this.browseButton);
            this.Controls.Add(this.outputTextBox);
            this.Controls.Add(this.authorTextBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.nameTextBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.programmingLanguageCombo);
            this.Controls.Add(this.languageCombo);
            this.Controls.Add(this.factionCombo);
            this.Controls.Add(this.regenerateButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.generateButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MinimizeBox = false;
            this.Name = "CodeGeneratorForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Generate Code";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CodeGeneratorForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button generateButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button regenerateButton;
        private System.Windows.Forms.ComboBox factionCombo;
        private System.Windows.Forms.ComboBox languageCombo;
        private System.Windows.Forms.ComboBox programmingLanguageCombo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox nameTextBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox authorTextBox;
        private System.Windows.Forms.TextBox outputTextBox;
        private System.Windows.Forms.Button browseButton;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.ComboBox environmentComboBox;
        private System.Windows.Forms.Label label7;
    }
}