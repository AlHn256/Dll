using System.Drawing;
using System.Windows.Forms;

namespace ImgAssemblingLib.AditionalForms
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.picBox_Display = new System.Windows.Forms.PictureBox();
            this.TestBtn = new System.Windows.Forms.Button();
            this.Test2Btn = new System.Windows.Forms.Button();
            this.PeriodTxtBox = new System.Windows.Forms.TextBox();
            this.RTB = new System.Windows.Forms.RichTextBox();
            this.FileDirTxtBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.ZoomLabel = new System.Windows.Forms.Label();
            this.ShowPointsBtn = new System.Windows.Forms.Button();
            this.StitchImgsBtn = new System.Windows.Forms.Button();
            this.AllPointsChkBox = new System.Windows.Forms.CheckBox();
            this.SaveBtn = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.StopBtn = new System.Windows.Forms.Button();
            this.progressBarLabel = new System.Windows.Forms.Label();
            this.SaveThisImgBtn = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.menuToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imgFixingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fileNameFixingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteFileCopyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteResultesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deletePlanToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToTxtBox = new System.Windows.Forms.TextBox();
            this.FromTxtBox = new System.Windows.Forms.TextBox();
            this.FrLb = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.TestImgFixingBtn = new System.Windows.Forms.Button();
            this.StitchingByPlanBtn = new System.Windows.Forms.Button();
            this.Period = new System.Windows.Forms.Label();
            this.GetSpeedBtn = new System.Windows.Forms.Button();
            this.RndBtn = new System.Windows.Forms.Button();
            this.OpenDirDtn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.picBox_Display)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // picBox_Display
            // 
            this.picBox_Display.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.picBox_Display.Location = new System.Drawing.Point(10, 74);
            this.picBox_Display.Name = "picBox_Display";
            this.picBox_Display.Size = new System.Drawing.Size(1712, 692);
            this.picBox_Display.TabIndex = 1;
            this.picBox_Display.TabStop = false;
            this.picBox_Display.MouseDown += picBox_Display_MouseDown;
            this.picBox_Display.MouseUp += picBox_Display_MouseUp;
            this.picBox_Display.MouseWheel += panel1_MouseWheel;
            // 
            // TestBtn
            // 
            this.TestBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.TestBtn.Location = new System.Drawing.Point(1643, 49);
            this.TestBtn.Name = "TestBtn";
            this.TestBtn.Size = new System.Drawing.Size(82, 23);
            this.TestBtn.TabIndex = 3;
            this.TestBtn.Text = "Match Pic By Sift";
            this.TestBtn.UseVisualStyleBackColor = true;
            this.TestBtn.Click += new System.EventHandler(this. TestBtn_Click);
            // 
            // Test2Btn
            // 
            this.Test2Btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Test2Btn.Location = new System.Drawing.Point(1643, 26);
            this.Test2Btn.Name = "Test2Btn";
            this.Test2Btn.Size = new System.Drawing.Size(82, 21);
            this.Test2Btn.TabIndex = 4;
            this.Test2Btn.Text = "Match Pic v2";
            this.Test2Btn.UseVisualStyleBackColor = true;
            // 
            // PeriodTxtBox
            // 
            this.PeriodTxtBox.Location = new System.Drawing.Point(519, 51);
            this.PeriodTxtBox.Name = "PeriodTxtBox";
            this.PeriodTxtBox.Size = new System.Drawing.Size(30, 20);
            this.PeriodTxtBox.TabIndex = 5;
            this.PeriodTxtBox.Text = "1";
            this.PeriodTxtBox.TextChanged += PeriodTxtBox_TextChanged;
            // 
            // RTB
            // 
            this.RTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RTB.Location = new System.Drawing.Point(569, 10);
            this.RTB.Name = "RTB";
            this.RTB.Size = new System.Drawing.Size(882, 62);
            this.RTB.TabIndex = 7;
            this.RTB.Text = "";
            this.RTB.KeyPress += key_Down;
            // 
            // FileDirTxtBox
            // 
            this.FileDirTxtBox.Location = new System.Drawing.Point(10, 26);
            this.FileDirTxtBox.Name = "FileDirTxtBox";
            this.FileDirTxtBox.Size = new System.Drawing.Size(437, 20);
            this.FileDirTxtBox.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI Black", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(350, 52);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(40, 15);
            this.label4.TabIndex = 14;
            this.label4.Text = "Zoom";
            // 
            // ZoomLabel
            // 
            this.ZoomLabel.AutoSize = true;
            this.ZoomLabel.Font = new System.Drawing.Font("Segoe UI Black", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ZoomLabel.Location = new System.Drawing.Point(389, 53);
            this.ZoomLabel.Name = "ZoomLabel";
            this.ZoomLabel.Size = new System.Drawing.Size(13, 15);
            this.ZoomLabel.TabIndex = 15;
            this.ZoomLabel.Text = "1";
            // 
            // ShowPointsBtn
            // 
            this.ShowPointsBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ShowPointsBtn.Location = new System.Drawing.Point(1566, 26);
            this.ShowPointsBtn.Name = "ShowPointsBtn";
            this.ShowPointsBtn.Size = new System.Drawing.Size(74, 21);
            this.ShowPointsBtn.TabIndex = 16;
            this.ShowPointsBtn.Text = "Show Points";
            this.ShowPointsBtn.UseVisualStyleBackColor = true;
            this.ShowPointsBtn.Click += new System.EventHandler(this. ShowPointsBtn_Click);
            // 
            // StitchImgsBtn
            // 
            this.StitchImgsBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.StitchImgsBtn.Location = new System.Drawing.Point(1566, 49);
            this.StitchImgsBtn.Name = "StitchImgsBtn";
            this.StitchImgsBtn.Size = new System.Drawing.Size(74, 23);
            this.StitchImgsBtn.TabIndex = 22;
            this.StitchImgsBtn.Text = "Join Imgs";
            this.StitchImgsBtn.UseVisualStyleBackColor = true;
            this.StitchImgsBtn.Click += new System.EventHandler(this. JoinImgs);
            // 
            // AllPointsChkBox
            // 
            this.AllPointsChkBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.AllPointsChkBox.AutoSize = true;
            this.AllPointsChkBox.Location = new System.Drawing.Point(1460, 30);
            this.AllPointsChkBox.Name = "AllPointsChkBox";
            this.AllPointsChkBox.Size = new System.Drawing.Size(69, 17);
            this.AllPointsChkBox.TabIndex = 23;
            this.AllPointsChkBox.Text = "All Points";
            this.AllPointsChkBox.UseVisualStyleBackColor = true;
            // 
            // SaveBtn
            // 
            this.SaveBtn.Location = new System.Drawing.Point(153, 49);
            this.SaveBtn.Name = "SaveBtn";
            this.SaveBtn.Size = new System.Drawing.Size(64, 23);
            this.SaveBtn.TabIndex = 24;
            this.SaveBtn.Text = "Save Rezult";
            this.SaveBtn.UseVisualStyleBackColor = true;
            this.SaveBtn.Click += new System.EventHandler(this. SaveBtn_Click);
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(10, 771);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(1643, 20);
            this.progressBar.TabIndex = 26;
            // 
            // StopBtn
            // 
            this.StopBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.StopBtn.Location = new System.Drawing.Point(1659, 771);
            this.StopBtn.Name = "StopBtn";
            this.StopBtn.Size = new System.Drawing.Size(63, 20);
            this.StopBtn.TabIndex = 27;
            this.StopBtn.Text = "Stop";
            this.StopBtn.UseVisualStyleBackColor = true;
            // 
            // progressBarLabel
            // 
            this.progressBarLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBarLabel.AutoSize = true;
            this.progressBarLabel.Font = new System.Drawing.Font("Segoe UI Black", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.progressBarLabel.Location = new System.Drawing.Point(760, 775);
            this.progressBarLabel.Name = "progressBarLabel";
            this.progressBarLabel.Size = new System.Drawing.Size(39, 15);
            this.progressBarLabel.TabIndex = 28;
            this.progressBarLabel.Text = "Point";
            // 
            // SaveThisImgBtn
            // 
            this.SaveThisImgBtn.Location = new System.Drawing.Point(222, 49);
            this.SaveThisImgBtn.Name = "SaveThisImgBtn";
            this.SaveThisImgBtn.Size = new System.Drawing.Size(73, 23);
            this.SaveThisImgBtn.TabIndex = 29;
            this.SaveThisImgBtn.Text = "Save this Img";
            this.SaveThisImgBtn.UseVisualStyleBackColor = true;
            this.SaveThisImgBtn.Click += new System.EventHandler(this. SaveThisImgBtn_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(5, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(1734, 24);
            this.menuStrip1.TabIndex = 30;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // menuToolStripMenuItem
            // 
            this.menuToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.imgFixingToolStripMenuItem,
            this.fileNameFixingToolStripMenuItem,
            this.deleteFileCopyToolStripMenuItem,
            this.deleteResultesToolStripMenuItem,
            this.deletePlanToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.menuToolStripMenuItem.Name = "menuToolStripMenuItem";
            this.menuToolStripMenuItem.Size = new System.Drawing.Size(50, 20);
            this.menuToolStripMenuItem.Text = "Menu";
            // 
            // imgFixingToolStripMenuItem
            // 
            this.imgFixingToolStripMenuItem.Name = "imgFixingToolStripMenuItem";
            this.imgFixingToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.imgFixingToolStripMenuItem.Text = "Img Fixing";
            this.imgFixingToolStripMenuItem.Click += new System.EventHandler(this. imgFixingToolStripMenuItem_Click);
            // 
            // fileNameFixingToolStripMenuItem
            // 
            this.fileNameFixingToolStripMenuItem.Name = "fileNameFixingToolStripMenuItem";
            this.fileNameFixingToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.fileNameFixingToolStripMenuItem.Text = "File Name Fixing";
            // 
            // 
            // deleteFileCopyToolStripMenuItem
            // 
            this.deleteFileCopyToolStripMenuItem.Name = "deleteFileCopyToolStripMenuItem";
            this.deleteFileCopyToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.deleteFileCopyToolStripMenuItem.Text = "Delete FileCopy";
            this.deleteFileCopyToolStripMenuItem.Click += new System.EventHandler(this. deleteFileCopyToolStripMenuItem_Click);
            // 
            // deleteResultesToolStripMenuItem
            // 
            this.deleteResultesToolStripMenuItem.Name = "deleteResultesToolStripMenuItem";
            this.deleteResultesToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.deleteResultesToolStripMenuItem.Text = "Delete Resultes";
            this.deleteResultesToolStripMenuItem.Click += new System.EventHandler(this. deleteResultesToolStripMenuItem_Click);
            // 
            // deletePlanToolStripMenuItem
            // 
            this.deletePlanToolStripMenuItem.Name = "deletePlanToolStripMenuItem";
            this.deletePlanToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.deletePlanToolStripMenuItem.Text = "Delete Plan";
            this.deletePlanToolStripMenuItem.Click += new System.EventHandler(this. deletePlanToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this. exitToolStripMenuItem_Click);
            // 
            // ToTxtBox
            // 
            this.ToTxtBox.Location = new System.Drawing.Point(519, 30);
            this.ToTxtBox.Name = "ToTxtBox";
            this.ToTxtBox.Size = new System.Drawing.Size(30, 20);
            this.ToTxtBox.TabIndex = 31;
            this.ToTxtBox.Text = "100";
            this.ToTxtBox.TextChanged += ToTxtBox_TextChanged;
            // 
            // FromTxtBox
            // 
            this.FromTxtBox.Location = new System.Drawing.Point(519, 10);
            this.FromTxtBox.Name = "FromTxtBox";
            this.FromTxtBox.Size = new System.Drawing.Size(30, 20);
            this.FromTxtBox.TabIndex = 32;
            this.FromTxtBox.Text = "0";
            this.FromTxtBox.TextChanged += FromTxtBox_TextChanged;
            // 
            // FrLb
            // 
            this.FrLb.AutoSize = true;
            this.FrLb.Font = new System.Drawing.Font("Segoe UI Black", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FrLb.Location = new System.Drawing.Point(475, 14);
            this.FrLb.Name = "FrLb";
            this.FrLb.Size = new System.Drawing.Size(36, 15);
            this.FrLb.TabIndex = 33;
            this.FrLb.Text = "From";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI Black", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(483, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(21, 15);
            this.label1.TabIndex = 34;
            this.label1.Text = "To";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI Black", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.Location = new System.Drawing.Point(549, 13);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(18, 15);
            this.label5.TabIndex = 35;
            this.label5.Text = "%";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI Black", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label6.Location = new System.Drawing.Point(549, 36);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(18, 15);
            this.label6.TabIndex = 36;
            this.label6.Text = "%";
            // 
            // TestImgFixingBtn
            // 
            this.TestImgFixingBtn.Location = new System.Drawing.Point(93, 49);
            this.TestImgFixingBtn.Name = "TestImgFixingBtn";
            this.TestImgFixingBtn.Size = new System.Drawing.Size(54, 23);
            this.TestImgFixingBtn.TabIndex = 37;
            this.TestImgFixingBtn.Text = "Edit plan";
            this.TestImgFixingBtn.UseVisualStyleBackColor = true;
            this.TestImgFixingBtn.Click += new System.EventHandler(this. TestImgFixingBtn_Click);
            // 
            // StitchingByPlanBtn
            // 
            this.StitchingByPlanBtn.Location = new System.Drawing.Point(10, 49);
            this.StitchingByPlanBtn.Name = "StitchingByPlanBtn";
            this.StitchingByPlanBtn.Size = new System.Drawing.Size(78, 23);
            this.StitchingByPlanBtn.TabIndex = 38;
            this.StitchingByPlanBtn.Text = "Stitch";
            this.StitchingByPlanBtn.UseVisualStyleBackColor = true;
            this.StitchingByPlanBtn.Click += new System.EventHandler(this.StartAssembling);
            
            // 
            // 
            // Period
            // 
            this.Period.AutoSize = true;
            this.Period.Font = new System.Drawing.Font("Segoe UI Black", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Period.Location = new System.Drawing.Point(469, 54);
            this.Period.Name = "Period";
            this.Period.Size = new System.Drawing.Size(46, 15);
            this.Period.TabIndex = 39;
            this.Period.Text = "Period";
            // 
            // GetSpeedBtn
            // 
            this.GetSpeedBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.GetSpeedBtn.Location = new System.Drawing.Point(1494, 49);
            this.GetSpeedBtn.Name = "GetSpeedBtn";
            this.GetSpeedBtn.Size = new System.Drawing.Size(67, 23);
            this.GetSpeedBtn.TabIndex = 40;
            this.GetSpeedBtn.Text = "Get Speed";
            this.GetSpeedBtn.UseVisualStyleBackColor = true;
            OpenDirDtn.Click += new System.EventHandler(this. OpenDirDtn_Click);
            // 
            // RndBtn
            // 
            this.RndBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.RndBtn.Location = new System.Drawing.Point(1457, 49);
            this.RndBtn.Name = "RndBtn";
            this.RndBtn.Size = new System.Drawing.Size(32, 23);
            this.RndBtn.TabIndex = 41;
            this.RndBtn.Text = "Rnd";
            this.RndBtn.UseVisualStyleBackColor = true;
            this.OpenDirDtn.Click += new System.EventHandler(this. OpenDirDtn_Click);
            // 
            // OpenDirDtn
            // 
            this.OpenDirDtn.Location = new System.Drawing.Point(449, 26);
            this.OpenDirDtn.Name = "OpenDirDtn";
            this.OpenDirDtn.Size = new System.Drawing.Size(24, 21);
            this.OpenDirDtn.TabIndex = 43;
            this.OpenDirDtn.Text = "...";
            this.OpenDirDtn.UseVisualStyleBackColor = true;
            this.OpenDirDtn.Click += new System.EventHandler(this. OpenDirDtn_Click);
            // 
            // Form1
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1734, 802);
            this.Controls.Add(this.OpenDirDtn);
            this.Controls.Add(this.RndBtn);
            this.Controls.Add(this.GetSpeedBtn);
            this.Controls.Add(this.Period);
            this.Controls.Add(this.StitchingByPlanBtn);
            this.Controls.Add(this.TestImgFixingBtn);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.FrLb);
            this.Controls.Add(this.FromTxtBox);
            this.Controls.Add(this.ToTxtBox);
            this.Controls.Add(this.SaveThisImgBtn);
            this.Controls.Add(this.progressBarLabel);
            this.Controls.Add(this.StopBtn);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.SaveBtn);
            this.Controls.Add(this.AllPointsChkBox);
            this.Controls.Add(this.StitchImgsBtn);
            this.Controls.Add(this.ShowPointsBtn);
            this.Controls.Add(this.ZoomLabel);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.FileDirTxtBox);
            this.Controls.Add(this.RTB);
            this.Controls.Add(this.PeriodTxtBox);
            this.Controls.Add(this.Test2Btn);
            this.Controls.Add(this.TestBtn);
            this.Controls.Add(this.picBox_Display);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(1750, 600);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.picBox_Display)).EndInit();
            this.FormClosing += Form1_FormClosing;
            this.Load += Loading;
            this.DragDrop += WindowsForm_DragDrop;
            this.DragEnter += WindowsForm_DragEnter;
            ((System.ComponentModel.ISupportInitialize)picBox_Display).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
        private PictureBox picBox_Display;
        private Button TestBtn;
        private Button Test2Btn;
        private TextBox PeriodTxtBox;
        private RichTextBox RTB;
        private TextBox FileDirTxtBox;
        private Label label4;
        private Label ZoomLabel;
        private Button ShowPointsBtn;
        private Button StitchImgsBtn;
        private CheckBox AllPointsChkBox;
        private Button SaveBtn;
        private ProgressBar progressBar;
        private Button StopBtn;
        private Label progressBarLabel;
        private Button SaveThisImgBtn;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem menuToolStripMenuItem;
        private ToolStripMenuItem deletePlanToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem deleteResultesToolStripMenuItem;
        private TextBox ToTxtBox;
        private TextBox FromTxtBox;
        private Label FrLb;
        private Label label1;
        private Label label5;
        private Label label6;
        private ToolStripMenuItem imgFixingToolStripMenuItem;
        private ToolStripMenuItem deleteFileCopyToolStripMenuItem;
        private Button TestImgFixingBtn;
        private ToolStripMenuItem fileNameFixingToolStripMenuItem;
        private Button StitchingByPlanBtn;
        private Label Period;
        private Button GetSpeedBtn;
        private Button RndBtn;
        private Button Zoom1Btn;
        private Button OpenDirDtn;
    }
}