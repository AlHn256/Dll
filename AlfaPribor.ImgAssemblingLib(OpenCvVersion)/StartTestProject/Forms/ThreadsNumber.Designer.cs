namespace StartTestProject.Forms
{
    partial class ThreadsNumber
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
            this.DelBtn = new System.Windows.Forms.Button();
            this.AddBtn = new System.Windows.Forms.Button();
            this.RTB = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.OpenSettingsDirBtn = new System.Windows.Forms.Button();
            this.SettingsDirTxtBox = new System.Windows.Forms.TextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.clearAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearAllToolStripMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TestFormMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // DelBtn
            // 
            this.DelBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.DelBtn.Location = new System.Drawing.Point(259, 43);
            this.DelBtn.Name = "DelBtn";
            this.DelBtn.Size = new System.Drawing.Size(43, 39);
            this.DelBtn.TabIndex = 2;
            this.DelBtn.Text = "-";
            this.DelBtn.UseVisualStyleBackColor = true;
            this.DelBtn.Click += new System.EventHandler(this.DelBtn_Click);
            // 
            // AddBtn
            // 
            this.AddBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.AddBtn.Location = new System.Drawing.Point(356, 43);
            this.AddBtn.Name = "AddBtn";
            this.AddBtn.Size = new System.Drawing.Size(41, 39);
            this.AddBtn.TabIndex = 1;
            this.AddBtn.Text = "+";
            this.AddBtn.UseVisualStyleBackColor = true;
            this.AddBtn.Click += new System.EventHandler(this.AddBtn_Click);
            // 
            // RTB
            // 
            this.RTB.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.RTB.Location = new System.Drawing.Point(308, 44);
            this.RTB.Name = "RTB";
            this.RTB.Size = new System.Drawing.Size(42, 39);
            this.RTB.TabIndex = 0;
            this.RTB.Text = "1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(202, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(250, 20);
            this.label1.TabIndex = 3;
            this.label1.Text = "Количество потоков \\ камер";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(12, 83);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(175, 20);
            this.label2.TabIndex = 4;
            this.label2.Text = "Папка с настройками:";
            // 
            // OpenSettingsDirBtn
            // 
            this.OpenSettingsDirBtn.Location = new System.Drawing.Point(632, 115);
            this.OpenSettingsDirBtn.Name = "OpenSettingsDirBtn";
            this.OpenSettingsDirBtn.Size = new System.Drawing.Size(34, 23);
            this.OpenSettingsDirBtn.TabIndex = 6;
            this.OpenSettingsDirBtn.Text = "...";
            this.OpenSettingsDirBtn.UseVisualStyleBackColor = true;
            this.OpenSettingsDirBtn.Click += new System.EventHandler(this.OpenSettingsDirBtn_Click);
            // 
            // SettingsDirTxtBox
            // 
            this.SettingsDirTxtBox.Location = new System.Drawing.Point(12, 116);
            this.SettingsDirTxtBox.Name = "SettingsDirTxtBox";
            this.SettingsDirTxtBox.ReadOnly = true;
            this.SettingsDirTxtBox.Size = new System.Drawing.Size(613, 20);
            this.SettingsDirTxtBox.TabIndex = 7;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clearAllToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(681, 24);
            this.menuStrip1.TabIndex = 8;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // clearAllToolStripMenuItem
            // 
            this.clearAllToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TestFormMenuItem,
            this.clearAllToolStripMenu,
            this.exitToolStripMenuItem});
            this.clearAllToolStripMenuItem.Name = "clearAllToolStripMenuItem";
            this.clearAllToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.clearAllToolStripMenuItem.Text = "Файл";
            // 
            // clearAllToolStripMenu
            // 
            this.clearAllToolStripMenu.Name = "clearAllToolStripMenu";
            this.clearAllToolStripMenu.Size = new System.Drawing.Size(180, 22);
            this.clearAllToolStripMenu.Text = "Удалить планы";
            this.clearAllToolStripMenu.Click += new System.EventHandler(this.clearAllToolStripMenu_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // TestFormMenuItem
            // 
            this.TestFormMenuItem.Name = "TestFormMenuItem";
            this.TestFormMenuItem.Size = new System.Drawing.Size(180, 22);
            this.TestFormMenuItem.Text = "Тестовая форма";
            this.TestFormMenuItem.Click += new System.EventHandler(this.TestFormMenuItem_Click);
            // 
            // ThreadsNumber
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(681, 299);
            this.Controls.Add(this.SettingsDirTxtBox);
            this.Controls.Add(this.OpenSettingsDirBtn);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.RTB);
            this.Controls.Add(this.AddBtn);
            this.Controls.Add(this.DelBtn);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MaximumSize = new System.Drawing.Size(697, 900);
            this.MinimumSize = new System.Drawing.Size(697, 250);
            this.Name = "ThreadsNumber";
            this.Text = "ThreadNumber";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ThreadsNumber_FormClosed);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button DelBtn;
        private System.Windows.Forms.Button AddBtn;
        private System.Windows.Forms.RichTextBox RTB;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button OpenSettingsDirBtn;
        private System.Windows.Forms.TextBox SettingsDirTxtBox;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem clearAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearAllToolStripMenu;
        private System.Windows.Forms.ToolStripMenuItem TestFormMenuItem;
    }
}