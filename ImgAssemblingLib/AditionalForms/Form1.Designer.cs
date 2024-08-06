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
            picBox_Display = new PictureBox();
            TestBtn = new Button();
            Test2Btn = new Button();
            PeriodTxtBox = new TextBox();
            RTB = new RichTextBox();
            FileDirTxtBox = new TextBox();
            label4 = new Label();
            ZoomLabel = new Label();
            ShowPointsBtn = new Button();
            StitchImgsBtn = new Button();
            AllPointsChkBox = new CheckBox();
            SaveBtn = new Button();
            progressBar = new ProgressBar();
            StopBtn = new Button();
            progressBarLabel = new Label();
            SaveThisImgBtn = new Button();
            menuStrip1 = new MenuStrip();
            menuToolStripMenuItem = new ToolStripMenuItem();
            imgFixingToolStripMenuItem = new ToolStripMenuItem();
            fileNameFixingToolStripMenuItem = new ToolStripMenuItem();
            deleteFileCopyToolStripMenuItem = new ToolStripMenuItem();
            deleteResultesToolStripMenuItem = new ToolStripMenuItem();
            deletePlanToolStripMenuItem = new ToolStripMenuItem();
            exitToolStripMenuItem = new ToolStripMenuItem();
            ToTxtBox = new TextBox();
            FromTxtBox = new TextBox();
            FrLb = new Label();
            label1 = new Label();
            label5 = new Label();
            label6 = new Label();
            TestImgFixingBtn = new Button();
            StitchingByPlanBtn = new Button();
            Period = new Label();
            GetSpeedBtn = new Button();
            RndBtn = new Button();
            OpenDirDtn = new Button();
            ((System.ComponentModel.ISupportInitialize)picBox_Display).BeginInit();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // picBox_Display
            // 
            picBox_Display.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            picBox_Display.Location = new Point(12, 85);
            picBox_Display.Name = "picBox_Display";
            picBox_Display.Size = new Size(1841, 799);
            picBox_Display.TabIndex = 1;
            picBox_Display.TabStop = false;
            picBox_Display.MouseDown += picBox_Display_MouseDown;
            picBox_Display.MouseUp += picBox_Display_MouseUp;
            picBox_Display.MouseWheel += panel1_MouseWheel;
            // 
            // TestBtn
            // 
            TestBtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            TestBtn.Location = new Point(1754, 54);
            TestBtn.Name = "TestBtn";
            TestBtn.Size = new Size(96, 23);
            TestBtn.TabIndex = 3;
            TestBtn.Text = "Match Pic By Sift";
            TestBtn.UseVisualStyleBackColor = true;
            TestBtn.Click += TestBtn_Click;
            // 
            // Test2Btn
            // 
            Test2Btn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            Test2Btn.Location = new Point(1754, 27);
            Test2Btn.Name = "Test2Btn";
            Test2Btn.Size = new Size(96, 24);
            Test2Btn.TabIndex = 4;
            Test2Btn.Text = "Match Pic v2";
            Test2Btn.UseVisualStyleBackColor = true;
            Test2Btn.Click += Test2Btn_Click;
            // 
            // PeriodTxtBox
            // 
            PeriodTxtBox.Location = new Point(605, 59);
            PeriodTxtBox.Name = "PeriodTxtBox";
            PeriodTxtBox.Size = new Size(34, 23);
            PeriodTxtBox.TabIndex = 5;
            PeriodTxtBox.Text = "1";
            PeriodTxtBox.TextChanged += PeriodTxtBox_TextChanged;
            // 
            // RTB
            // 
            RTB.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            RTB.Location = new Point(664, 11);
            RTB.Name = "RTB";
            RTB.Size = new Size(867, 71);
            RTB.TabIndex = 7;
            RTB.Text = "";
            RTB.KeyPress += key_Down;
            // 
            // FileDirTxtBox
            // 
            FileDirTxtBox.Location = new Point(12, 30);
            FileDirTxtBox.Name = "FileDirTxtBox";
            FileDirTxtBox.Size = new Size(509, 23);
            FileDirTxtBox.TabIndex = 8;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI Black", 9F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 204);
            label4.Location = new Point(399, 61);
            label4.Name = "label4";
            label4.Size = new Size(40, 15);
            label4.TabIndex = 14;
            label4.Text = "Zoom";
            // 
            // ZoomLabel
            // 
            ZoomLabel.AutoSize = true;
            ZoomLabel.Font = new Font("Segoe UI Black", 9F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 204);
            ZoomLabel.Location = new Point(444, 62);
            ZoomLabel.Name = "ZoomLabel";
            ZoomLabel.Size = new Size(13, 15);
            ZoomLabel.TabIndex = 15;
            ZoomLabel.Text = "1";
            // 
            // ShowPointsBtn
            // 
            ShowPointsBtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            ShowPointsBtn.Location = new Point(1664, 27);
            ShowPointsBtn.Name = "ShowPointsBtn";
            ShowPointsBtn.Size = new Size(86, 24);
            ShowPointsBtn.TabIndex = 16;
            ShowPointsBtn.Text = "Show Points";
            ShowPointsBtn.UseVisualStyleBackColor = true;
            ShowPointsBtn.Click += ShowPointsBtn_Click;
            // 
            // StitchImgsBtn
            // 
            StitchImgsBtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            StitchImgsBtn.Location = new Point(1664, 54);
            StitchImgsBtn.Name = "StitchImgsBtn";
            StitchImgsBtn.Size = new Size(86, 23);
            StitchImgsBtn.TabIndex = 22;
            StitchImgsBtn.Text = "Stitch Imgs";
            StitchImgsBtn.UseVisualStyleBackColor = true;
            StitchImgsBtn.Click += Stitch2ImgsBtn_Click;
            // 
            // AllPointsChkBox
            // 
            AllPointsChkBox.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            AllPointsChkBox.AutoSize = true;
            AllPointsChkBox.Location = new Point(1582, 31);
            AllPointsChkBox.Name = "AllPointsChkBox";
            AllPointsChkBox.Size = new Size(76, 19);
            AllPointsChkBox.TabIndex = 23;
            AllPointsChkBox.Text = "All Points";
            AllPointsChkBox.UseVisualStyleBackColor = true;
            // 
            // SaveBtn
            // 
            SaveBtn.Location = new Point(178, 56);
            SaveBtn.Name = "SaveBtn";
            SaveBtn.Size = new Size(75, 26);
            SaveBtn.TabIndex = 24;
            SaveBtn.Text = "Save Rezult";
            SaveBtn.UseVisualStyleBackColor = true;
            SaveBtn.Click += SaveBtn_Click;
            // 
            // progressBar
            // 
            progressBar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            progressBar.Location = new Point(12, 890);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(1762, 23);
            progressBar.TabIndex = 26;
            // 
            // StopBtn
            // 
            StopBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            StopBtn.Location = new Point(1780, 890);
            StopBtn.Name = "StopBtn";
            StopBtn.Size = new Size(73, 23);
            StopBtn.TabIndex = 27;
            StopBtn.Text = "Stop";
            StopBtn.UseVisualStyleBackColor = true;
            // 
            // progressBarLabel
            // 
            progressBarLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            progressBarLabel.AutoSize = true;
            progressBarLabel.Font = new Font("Segoe UI Black", 9F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 204);
            progressBarLabel.Location = new Point(887, 894);
            progressBarLabel.Name = "progressBarLabel";
            progressBarLabel.Size = new Size(39, 15);
            progressBarLabel.TabIndex = 28;
            progressBarLabel.Text = "Point";
            // 
            // SaveThisImgBtn
            // 
            SaveThisImgBtn.Location = new Point(259, 56);
            SaveThisImgBtn.Name = "SaveThisImgBtn";
            SaveThisImgBtn.Size = new Size(85, 26);
            SaveThisImgBtn.TabIndex = 29;
            SaveThisImgBtn.Text = "Save this Img";
            SaveThisImgBtn.UseVisualStyleBackColor = true;
            SaveThisImgBtn.Click += SaveThisImgBtn_Click;
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { menuToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(1862, 24);
            menuStrip1.TabIndex = 30;
            menuStrip1.Text = "menuStrip1";
            // 
            // menuToolStripMenuItem
            // 
            menuToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { imgFixingToolStripMenuItem, fileNameFixingToolStripMenuItem, deleteFileCopyToolStripMenuItem, deleteResultesToolStripMenuItem, deletePlanToolStripMenuItem, exitToolStripMenuItem });
            menuToolStripMenuItem.Name = "menuToolStripMenuItem";
            menuToolStripMenuItem.Size = new Size(50, 20);
            menuToolStripMenuItem.Text = "Menu";
            // 
            // imgFixingToolStripMenuItem
            // 
            imgFixingToolStripMenuItem.Name = "imgFixingToolStripMenuItem";
            imgFixingToolStripMenuItem.Size = new Size(162, 22);
            imgFixingToolStripMenuItem.Text = "Img Fixing";
            imgFixingToolStripMenuItem.Click += imgFixingToolStripMenuItem_Click;
            // 
            // fileNameFixingToolStripMenuItem
            // 
            fileNameFixingToolStripMenuItem.Name = "fileNameFixingToolStripMenuItem";
            fileNameFixingToolStripMenuItem.Size = new Size(162, 22);
            fileNameFixingToolStripMenuItem.Text = "File Name Fixing";
            fileNameFixingToolStripMenuItem.Click += FileNameFixingToolStripMenuItemClick;
            // 
            // deleteFileCopyToolStripMenuItem
            // 
            deleteFileCopyToolStripMenuItem.Name = "deleteFileCopyToolStripMenuItem";
            deleteFileCopyToolStripMenuItem.Size = new Size(162, 22);
            deleteFileCopyToolStripMenuItem.Text = "Delete FileCopy";
            deleteFileCopyToolStripMenuItem.Click += deleteFileCopyToolStripMenuItem_Click;
            // 
            // deleteResultesToolStripMenuItem
            // 
            deleteResultesToolStripMenuItem.Name = "deleteResultesToolStripMenuItem";
            deleteResultesToolStripMenuItem.Size = new Size(162, 22);
            deleteResultesToolStripMenuItem.Text = "Delete Resultes";
            deleteResultesToolStripMenuItem.Click += deleteResultesToolStripMenuItem_Click;
            // 
            // deletePlanToolStripMenuItem
            // 
            deletePlanToolStripMenuItem.Name = "deletePlanToolStripMenuItem";
            deletePlanToolStripMenuItem.Size = new Size(162, 22);
            deletePlanToolStripMenuItem.Text = "Delete Plan";
            deletePlanToolStripMenuItem.Click += deletePlanToolStripMenuItem_Click;
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new Size(162, 22);
            exitToolStripMenuItem.Text = "Exit";
            exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
            // 
            // ToTxtBox
            // 
            ToTxtBox.Location = new Point(605, 35);
            ToTxtBox.Name = "ToTxtBox";
            ToTxtBox.Size = new Size(34, 23);
            ToTxtBox.TabIndex = 31;
            ToTxtBox.Text = "100";
            ToTxtBox.TextChanged += ToTxtBox_TextChanged;
            // 
            // FromTxtBox
            // 
            FromTxtBox.Location = new Point(605, 11);
            FromTxtBox.Name = "FromTxtBox";
            FromTxtBox.Size = new Size(34, 23);
            FromTxtBox.TabIndex = 32;
            FromTxtBox.Text = "0";
            FromTxtBox.TextChanged += FromTxtBox_TextChanged;
            // 
            // FrLb
            // 
            FrLb.AutoSize = true;
            FrLb.Font = new Font("Segoe UI Black", 9F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 204);
            FrLb.Location = new Point(561, 14);
            FrLb.Name = "FrLb";
            FrLb.Size = new Size(36, 15);
            FrLb.TabIndex = 33;
            FrLb.Text = "From";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Black", 9F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 204);
            label1.Location = new Point(568, 39);
            label1.Name = "label1";
            label1.Size = new Size(21, 15);
            label1.TabIndex = 34;
            label1.Text = "To";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI Black", 9F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 204);
            label5.Location = new Point(640, 15);
            label5.Name = "label5";
            label5.Size = new Size(18, 15);
            label5.TabIndex = 35;
            label5.Text = "%";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI Black", 9F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 204);
            label6.Location = new Point(640, 41);
            label6.Name = "label6";
            label6.Size = new Size(18, 15);
            label6.TabIndex = 36;
            label6.Text = "%";
            // 
            // TestImgFixingBtn
            // 
            TestImgFixingBtn.Location = new Point(109, 56);
            TestImgFixingBtn.Name = "TestImgFixingBtn";
            TestImgFixingBtn.Size = new Size(63, 26);
            TestImgFixingBtn.TabIndex = 37;
            TestImgFixingBtn.Text = "Edit plan";
            TestImgFixingBtn.UseVisualStyleBackColor = true;
            TestImgFixingBtn.Click += TestImgFixingBtn_Click;
            // 
            // StitchingByPlanBtn
            // 
            StitchingByPlanBtn.Location = new Point(12, 56);
            StitchingByPlanBtn.Name = "StitchingByPlanBtn";
            StitchingByPlanBtn.Size = new Size(91, 26);
            StitchingByPlanBtn.TabIndex = 38;
            StitchingByPlanBtn.Text = "Stitch";
            StitchingByPlanBtn.UseVisualStyleBackColor = true;
            StitchingByPlanBtn.Click += StitchingByPlanBtn_Click;
            // 
            // Period
            // 
            Period.AutoSize = true;
            Period.Font = new Font("Segoe UI Black", 9F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 204);
            Period.Location = new Point(557, 63);
            Period.Name = "Period";
            Period.Size = new Size(46, 15);
            Period.TabIndex = 39;
            Period.Text = "Period";
            // 
            // GetSpeedBtn
            // 
            GetSpeedBtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            GetSpeedBtn.Location = new Point(1580, 54);
            GetSpeedBtn.Name = "GetSpeedBtn";
            GetSpeedBtn.Size = new Size(78, 23);
            GetSpeedBtn.TabIndex = 40;
            GetSpeedBtn.Text = "Get Speed";
            GetSpeedBtn.UseVisualStyleBackColor = true;
            GetSpeedBtn.Click += GetSpeedBtn_Click;
            // 
            // RndBtn
            // 
            RndBtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            RndBtn.Location = new Point(1537, 54);
            RndBtn.Name = "RndBtn";
            RndBtn.Size = new Size(37, 23);
            RndBtn.TabIndex = 41;
            RndBtn.Text = "Rnd";
            RndBtn.UseVisualStyleBackColor = true;
            RndBtn.Click += Random_Click;
            // 
            // OpenDirDtn
            // 
            OpenDirDtn.Location = new Point(523, 29);
            OpenDirDtn.Name = "OpenDirDtn";
            OpenDirDtn.Size = new Size(28, 24);
            OpenDirDtn.TabIndex = 43;
            OpenDirDtn.Text = "...";
            OpenDirDtn.UseVisualStyleBackColor = true;
            OpenDirDtn.Click += OpenDirDtn_Click;
            // 
            // Form1
            // 
            AllowDrop = true;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1862, 925);
            Controls.Add(OpenDirDtn);
            Controls.Add(RndBtn);
            Controls.Add(GetSpeedBtn);
            Controls.Add(Period);
            Controls.Add(StitchingByPlanBtn);
            Controls.Add(TestImgFixingBtn);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(label1);
            Controls.Add(FrLb);
            Controls.Add(FromTxtBox);
            Controls.Add(ToTxtBox);
            Controls.Add(SaveThisImgBtn);
            Controls.Add(progressBarLabel);
            Controls.Add(StopBtn);
            Controls.Add(progressBar);
            Controls.Add(SaveBtn);
            Controls.Add(AllPointsChkBox);
            Controls.Add(StitchImgsBtn);
            Controls.Add(ShowPointsBtn);
            Controls.Add(ZoomLabel);
            Controls.Add(label4);
            Controls.Add(FileDirTxtBox);
            Controls.Add(RTB);
            Controls.Add(PeriodTxtBox);
            Controls.Add(Test2Btn);
            Controls.Add(TestBtn);
            Controls.Add(picBox_Display);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            MinimumSize = new Size(1750, 600);
            Name = "Form1";
            Text = "Form1";
            FormClosing += Form1_FormClosing;
            Load += Loading;
            DragDrop += WindowsForm_DragDrop;
            DragEnter += WindowsForm_DragEnter;
            ((System.ComponentModel.ISupportInitialize)picBox_Display).EndInit();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
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