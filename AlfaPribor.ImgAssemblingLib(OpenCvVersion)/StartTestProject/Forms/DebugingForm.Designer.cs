using System.Windows.Forms;

namespace ImgAssemblingLibOpenCV.AditionalForms
{
    partial class DebugingForm
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
            this.PeriodTxtBox = new System.Windows.Forms.TextBox();
            this.RTB = new System.Windows.Forms.RichTextBox();
            this.FileDirTxtBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.ZoomLabel = new System.Windows.Forms.Label();
            this.ShowPointsBtn = new System.Windows.Forms.Button();
            this.StitchImgsBtn = new System.Windows.Forms.Button();
            this.AllPointsChkBox = new System.Windows.Forms.CheckBox();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.StopBtn = new System.Windows.Forms.Button();
            this.progressBarLabel = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.menuToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveOriginalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveWindowImgToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteFileCopyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deletePlanToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteResultesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
            this.UseBitmapChckBox = new System.Windows.Forms.CheckBox();
            this.FixImgChckBox = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.picBox_Display)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // picBox_Display
            // 
            this.picBox_Display.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.picBox_Display.Location = new System.Drawing.Point(10, 78);
            this.picBox_Display.Name = "picBox_Display";
            this.picBox_Display.Size = new System.Drawing.Size(1712, 688);
            this.picBox_Display.TabIndex = 1;
            this.picBox_Display.TabStop = false;
            // 
            // PeriodTxtBox
            // 
            this.PeriodTxtBox.Location = new System.Drawing.Point(519, 51);
            this.PeriodTxtBox.Name = "PeriodTxtBox";
            this.PeriodTxtBox.Size = new System.Drawing.Size(30, 20);
            this.PeriodTxtBox.TabIndex = 5;
            this.PeriodTxtBox.Text = "1";
            this.PeriodTxtBox.TextChanged += new System.EventHandler(this.PeriodTxtBox_TextChanged);
            // 
            // RTB
            // 
            this.RTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RTB.Location = new System.Drawing.Point(573, 10);
            this.RTB.Name = "RTB";
            this.RTB.Size = new System.Drawing.Size(878, 62);
            this.RTB.TabIndex = 7;
            this.RTB.Text = "";
            // 
            // FileDirTxtBox
            // 
            this.FileDirTxtBox.Location = new System.Drawing.Point(9, 24);
            this.FileDirTxtBox.Name = "FileDirTxtBox";
            this.FileDirTxtBox.Size = new System.Drawing.Size(437, 20);
            this.FileDirTxtBox.TabIndex = 8;
            this.FileDirTxtBox.TextChanged += new System.EventHandler(this.FileDirTxtBox_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI Black", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(92, 54);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(82, 15);
            this.label4.TabIndex = 14;
            this.label4.Text = "Увеличение";
            // 
            // ZoomLabel
            // 
            this.ZoomLabel.AutoSize = true;
            this.ZoomLabel.Font = new System.Drawing.Font("Segoe UI Black", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ZoomLabel.Location = new System.Drawing.Point(193, 54);
            this.ZoomLabel.Name = "ZoomLabel";
            this.ZoomLabel.Size = new System.Drawing.Size(13, 15);
            this.ZoomLabel.TabIndex = 15;
            this.ZoomLabel.Text = "1";
            // 
            // ShowPointsBtn
            // 
            this.ShowPointsBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ShowPointsBtn.Location = new System.Drawing.Point(1458, 26);
            this.ShowPointsBtn.Name = "ShowPointsBtn";
            this.ShowPointsBtn.Size = new System.Drawing.Size(159, 21);
            this.ShowPointsBtn.TabIndex = 16;
            this.ShowPointsBtn.Text = "Показать точки";
            this.ShowPointsBtn.UseVisualStyleBackColor = true;
            this.ShowPointsBtn.Click += new System.EventHandler(this.ShowPointsBtn_Click);
            // 
            // StitchImgsBtn
            // 
            this.StitchImgsBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.StitchImgsBtn.Location = new System.Drawing.Point(1458, 49);
            this.StitchImgsBtn.Name = "StitchImgsBtn";
            this.StitchImgsBtn.Size = new System.Drawing.Size(159, 23);
            this.StitchImgsBtn.TabIndex = 22;
            this.StitchImgsBtn.Text = "Объеденить изображения";
            this.StitchImgsBtn.UseVisualStyleBackColor = true;
            this.StitchImgsBtn.Click += new System.EventHandler(this.JoinImgs);
            // 
            // AllPointsChkBox
            // 
            this.AllPointsChkBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.AllPointsChkBox.AutoSize = true;
            this.AllPointsChkBox.Location = new System.Drawing.Point(1629, 28);
            this.AllPointsChkBox.Name = "AllPointsChkBox";
            this.AllPointsChkBox.Size = new System.Drawing.Size(76, 17);
            this.AllPointsChkBox.TabIndex = 23;
            this.AllPointsChkBox.Text = "Все точки";
            this.AllPointsChkBox.UseVisualStyleBackColor = true;
            this.AllPointsChkBox.CheckedChanged += new System.EventHandler(this.AllPointsChkBox_CheckedChanged);
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
            this.StopBtn.Click += new System.EventHandler(this.StopBtn_Click);
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
            this.SaveOriginalToolStripMenuItem,
            this.SaveWindowImgToolStripMenuItem,
            this.deleteFileCopyToolStripMenuItem,
            this.deletePlanToolStripMenuItem,
            this.deleteResultesToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.menuToolStripMenuItem.Name = "menuToolStripMenuItem";
            this.menuToolStripMenuItem.Size = new System.Drawing.Size(50, 20);
            this.menuToolStripMenuItem.Text = "Menu";
            // 
            // SaveOriginalToolStripMenuItem
            // 
            this.SaveOriginalToolStripMenuItem.Name = "SaveOriginalToolStripMenuItem";
            this.SaveOriginalToolStripMenuItem.Size = new System.Drawing.Size(242, 22);
            this.SaveOriginalToolStripMenuItem.Text = "Сохранить оригинал";
            this.SaveOriginalToolStripMenuItem.Click += new System.EventHandler(this.SaveOriginalToolStripMenuItem_Click);
            // 
            // SaveWindowImgToolStripMenuItem
            // 
            this.SaveWindowImgToolStripMenuItem.Name = "SaveWindowImgToolStripMenuItem";
            this.SaveWindowImgToolStripMenuItem.Size = new System.Drawing.Size(242, 22);
            this.SaveWindowImgToolStripMenuItem.Text = "Сохрнить изображение в окне";
            this.SaveWindowImgToolStripMenuItem.Click += new System.EventHandler(this.SaveWindowImgToolStripMenuItem_Click);
            // 
            // deleteFileCopyToolStripMenuItem
            // 
            this.deleteFileCopyToolStripMenuItem.Name = "deleteFileCopyToolStripMenuItem";
            this.deleteFileCopyToolStripMenuItem.Size = new System.Drawing.Size(242, 22);
            this.deleteFileCopyToolStripMenuItem.Text = "Bmp в Jpeg";
            this.deleteFileCopyToolStripMenuItem.Click += new System.EventHandler(this.deleteFileCopyToolStripMenuItem_Click);
            // 
            // deletePlanToolStripMenuItem
            // 
            this.deletePlanToolStripMenuItem.Name = "deletePlanToolStripMenuItem";
            this.deletePlanToolStripMenuItem.Size = new System.Drawing.Size(242, 22);
            this.deletePlanToolStripMenuItem.Text = "Удалить план сборки";
            this.deletePlanToolStripMenuItem.Click += new System.EventHandler(this.deletePlanToolStripMenuItem_Click);
            // 
            // deleteResultesToolStripMenuItem
            // 
            this.deleteResultesToolStripMenuItem.Name = "deleteResultesToolStripMenuItem";
            this.deleteResultesToolStripMenuItem.Size = new System.Drawing.Size(242, 22);
            this.deleteResultesToolStripMenuItem.Text = "Удалить результаты";
            this.deleteResultesToolStripMenuItem.Click += new System.EventHandler(this.deleteResultesToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(242, 22);
            this.exitToolStripMenuItem.Text = "Выход";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // ToTxtBox
            // 
            this.ToTxtBox.Location = new System.Drawing.Point(519, 30);
            this.ToTxtBox.Name = "ToTxtBox";
            this.ToTxtBox.Size = new System.Drawing.Size(30, 20);
            this.ToTxtBox.TabIndex = 31;
            this.ToTxtBox.Text = "100";
            this.ToTxtBox.TextChanged += new System.EventHandler(this.ToTxtBox_TextChanged);
            // 
            // FromTxtBox
            // 
            this.FromTxtBox.Location = new System.Drawing.Point(519, 10);
            this.FromTxtBox.Name = "FromTxtBox";
            this.FromTxtBox.Size = new System.Drawing.Size(30, 20);
            this.FromTxtBox.TabIndex = 32;
            this.FromTxtBox.Text = "0";
            this.FromTxtBox.TextChanged += new System.EventHandler(this.FromTxtBox_TextChanged);
            // 
            // FrLb
            // 
            this.FrLb.AutoSize = true;
            this.FrLb.Font = new System.Drawing.Font("Segoe UI Black", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FrLb.Location = new System.Drawing.Point(477, 13);
            this.FrLb.Name = "FrLb";
            this.FrLb.Size = new System.Drawing.Size(36, 15);
            this.FrLb.TabIndex = 33;
            this.FrLb.Text = "From";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI Black", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(485, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(21, 15);
            this.label1.TabIndex = 34;
            this.label1.Text = "To";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI Black", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.Location = new System.Drawing.Point(550, 13);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(18, 15);
            this.label5.TabIndex = 35;
            this.label5.Text = "%";
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI Black", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label6.Location = new System.Drawing.Point(550, 36);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(18, 15);
            this.label6.TabIndex = 36;
            this.label6.Text = "%";
            this.label6.Click += new System.EventHandler(this.label6_Click);
            // 
            // TestImgFixingBtn
            // 
            this.TestImgFixingBtn.Location = new System.Drawing.Point(227, 50);
            this.TestImgFixingBtn.Name = "TestImgFixingBtn";
            this.TestImgFixingBtn.Size = new System.Drawing.Size(144, 23);
            this.TestImgFixingBtn.TabIndex = 37;
            this.TestImgFixingBtn.Text = "Исправить план сборки";
            this.TestImgFixingBtn.UseVisualStyleBackColor = true;
            this.TestImgFixingBtn.Click += new System.EventHandler(this.TestImgFixingBtn_Click);
            // 
            // StitchingByPlanBtn
            // 
            this.StitchingByPlanBtn.Location = new System.Drawing.Point(384, 50);
            this.StitchingByPlanBtn.Name = "StitchingByPlanBtn";
            this.StitchingByPlanBtn.Size = new System.Drawing.Size(88, 23);
            this.StitchingByPlanBtn.TabIndex = 38;
            this.StitchingByPlanBtn.Text = "Старт";
            this.StitchingByPlanBtn.UseVisualStyleBackColor = true;
            this.StitchingByPlanBtn.Click += new System.EventHandler(this.StartAssembling);
            // 
            // Period
            // 
            this.Period.AutoSize = true;
            this.Period.Font = new System.Drawing.Font("Segoe UI Black", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Period.Location = new System.Drawing.Point(472, 54);
            this.Period.Name = "Period";
            this.Period.Size = new System.Drawing.Size(46, 15);
            this.Period.TabIndex = 39;
            this.Period.Text = "Period";
            // 
            // GetSpeedBtn
            // 
            this.GetSpeedBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.GetSpeedBtn.Location = new System.Drawing.Point(1645, 48);
            this.GetSpeedBtn.Name = "GetSpeedBtn";
            this.GetSpeedBtn.Size = new System.Drawing.Size(67, 23);
            this.GetSpeedBtn.TabIndex = 40;
            this.GetSpeedBtn.Text = "Get Speed";
            this.GetSpeedBtn.UseVisualStyleBackColor = true;
            this.GetSpeedBtn.Visible = false;
            this.GetSpeedBtn.Click += new System.EventHandler(this.GetSpeedBtn_Click);
            // 
            // RndBtn
            // 
            this.RndBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.RndBtn.Location = new System.Drawing.Point(1687, 46);
            this.RndBtn.Name = "RndBtn";
            this.RndBtn.Size = new System.Drawing.Size(38, 23);
            this.RndBtn.TabIndex = 41;
            this.RndBtn.Text = "Rnd";
            this.RndBtn.UseVisualStyleBackColor = true;
            this.RndBtn.Visible = false;
            this.RndBtn.Click += new System.EventHandler(this.Random_Click);
            // 
            // OpenDirDtn
            // 
            this.OpenDirDtn.Location = new System.Drawing.Point(448, 23);
            this.OpenDirDtn.Name = "OpenDirDtn";
            this.OpenDirDtn.Size = new System.Drawing.Size(24, 21);
            this.OpenDirDtn.TabIndex = 43;
            this.OpenDirDtn.Text = "...";
            this.OpenDirDtn.UseVisualStyleBackColor = true;
            this.OpenDirDtn.Click += new System.EventHandler(this.OpenDirDtn_Click);
            // 
            // UseBitmapChckBox
            // 
            this.UseBitmapChckBox.AutoSize = true;
            this.UseBitmapChckBox.Location = new System.Drawing.Point(9, 54);
            this.UseBitmapChckBox.Name = "UseBitmapChckBox";
            this.UseBitmapChckBox.Size = new System.Drawing.Size(80, 17);
            this.UseBitmapChckBox.TabIndex = 44;
            this.UseBitmapChckBox.Text = "Use Bitmap";
            this.UseBitmapChckBox.UseVisualStyleBackColor = true;
            this.UseBitmapChckBox.CheckedChanged += new System.EventHandler(this.UseBitmapChckBox_CheckedChanged);
            // 
            // FixImgChckBox
            // 
            this.FixImgChckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.FixImgChckBox.AutoSize = true;
            this.FixImgChckBox.Location = new System.Drawing.Point(1460, 7);
            this.FixImgChckBox.Name = "FixImgChckBox";
            this.FixImgChckBox.Size = new System.Drawing.Size(200, 17);
            this.FixImgChckBox.TabIndex = 45;
            this.FixImgChckBox.Text = "Включить исправление дисторсии";
            this.FixImgChckBox.UseVisualStyleBackColor = true;
            this.FixImgChckBox.CheckedChanged += new System.EventHandler(this.FixImgChckBox_CheckedChanged);
            // 
            // DebugingForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1734, 802);
            this.Controls.Add(this.FixImgChckBox);
            this.Controls.Add(this.UseBitmapChckBox);
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
            this.Controls.Add(this.progressBarLabel);
            this.Controls.Add(this.StopBtn);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.AllPointsChkBox);
            this.Controls.Add(this.StitchImgsBtn);
            this.Controls.Add(this.ShowPointsBtn);
            this.Controls.Add(this.ZoomLabel);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.FileDirTxtBox);
            this.Controls.Add(this.RTB);
            this.Controls.Add(this.PeriodTxtBox);
            this.Controls.Add(this.picBox_Display);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(1750, 600);
            this.Name = "DebugingForm";
            this.Text = "Debug Form";
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseClick);
            ((System.ComponentModel.ISupportInitialize)(this.picBox_Display)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private PictureBox picBox_Display;
        private TextBox PeriodTxtBox;
        private RichTextBox RTB;
        private TextBox FileDirTxtBox;
        private Label label4;
        private Label ZoomLabel;
        private Button ShowPointsBtn;
        private Button StitchImgsBtn;
        private CheckBox AllPointsChkBox;
        private ProgressBar progressBar;
        private Button StopBtn;
        private Label progressBarLabel;
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
        private ToolStripMenuItem deleteFileCopyToolStripMenuItem;
        private Button TestImgFixingBtn;
        private Button StitchingByPlanBtn;
        private Label Period;
        private Button GetSpeedBtn;
        private Button RndBtn;
        private Button Zoom1Btn;
        private Button OpenDirDtn;
        private CheckBox UseBitmapChckBox;
        private ToolStripMenuItem SaveOriginalToolStripMenuItem;
        private ToolStripMenuItem SaveWindowImgToolStripMenuItem;
        private CheckBox FixImgChckBox;
    }
}