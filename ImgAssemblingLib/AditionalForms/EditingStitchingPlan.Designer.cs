using System.Windows.Forms;

namespace ImgAssemblingLib.AditionalForms
{
    partial class EditingStitchingPlan
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
            this.ExitBtn = new System.Windows.Forms.Button();
            this.SaveBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.WorkingDirectoryTxtBox = new System.Windows.Forms.TextBox();
            this.SaveToBtn = new System.Windows.Forms.Button();
            this.OpenWorkDirectoryBtn = new System.Windows.Forms.Button();
            this.FixImgChckBox = new System.Windows.Forms.CheckBox();
            this.AutoChckBox = new System.Windows.Forms.CheckBox();
            this.ChekFixedImgsChckBox = new System.Windows.Forms.CheckBox();
            this.OpenFixingImgDirectoryBtn = new System.Windows.Forms.Button();
            this.FixingImgDirectoryTxtBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.OpenImgFixingPlanBtn = new System.Windows.Forms.Button();
            this.ImgFixingPlanTxtBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.StitchСhckBox = new System.Windows.Forms.CheckBox();
            this.ChekStitchPlanСhckBox = new System.Windows.Forms.CheckBox();
            this.DefaultParametersCheckBox = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.ToLb = new System.Windows.Forms.Label();
            this.FrLb = new System.Windows.Forms.Label();
            this.FromTxtBox = new System.Windows.Forms.TextBox();
            this.ToTxtBox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.DeltaTxtBox = new System.Windows.Forms.TextBox();
            this.PeriodTxtBox = new System.Windows.Forms.TextBox();
            this.LoadBtn = new System.Windows.Forms.Button();
            this.SaveResultChckBox = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.OpenStitchingDirectoryBtn = new System.Windows.Forms.Button();
            this.StitchingDirectoryTxtBox = new System.Windows.Forms.TextBox();
            this.FindKeyPointsСhckBox = new System.Windows.Forms.CheckBox();
            this.SpeedCountingСhckBox = new System.Windows.Forms.CheckBox();
            this.MillimetersInPixelTxtBox = new System.Windows.Forms.TextBox();
            this.TimePerFrameTxtBox = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.AdditionalFilterChckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // ExitBtn
            // 
            this.ExitBtn.Location = new System.Drawing.Point(450, 342);
            this.ExitBtn.Name = "ExitBtn";
            this.ExitBtn.Size = new System.Drawing.Size(64, 20);
            this.ExitBtn.TabIndex = 0;
            this.ExitBtn.Text = "Exit";
            this.ExitBtn.UseVisualStyleBackColor = true;
            this.ExitBtn.Click += ExitBtn_Click;
            // 
            // SaveBtn
            // 
            this.SaveBtn.Location = new System.Drawing.Point(8, 342);
            this.SaveBtn.Name = "SaveBtn";
            this.SaveBtn.Size = new System.Drawing.Size(64, 20);
            this.SaveBtn.TabIndex = 1;
            this.SaveBtn.Text = "Save";
            this.SaveBtn.UseVisualStyleBackColor = true;
            this.SaveBtn.Click += new System.EventHandler(SaveBtn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Working Directory:";
            // 
            // WorkingDirectoryTxtBox
            // 
            this.WorkingDirectoryTxtBox.Location = new System.Drawing.Point(107, 5);
            this.WorkingDirectoryTxtBox.Name = "WorkingDirectoryTxtBox";
            this.WorkingDirectoryTxtBox.Size = new System.Drawing.Size(381, 20);
            this.WorkingDirectoryTxtBox.TabIndex = 3;
            // 
            // SaveToBtn
            // 
            this.SaveToBtn.Location = new System.Drawing.Point(78, 342);
            this.SaveToBtn.Name = "SaveToBtn";
            this.SaveToBtn.Size = new System.Drawing.Size(64, 20);
            this.SaveToBtn.TabIndex = 4;
            this.SaveToBtn.Text = "Save to";
            this.SaveToBtn.UseVisualStyleBackColor = true;
            this.SaveToBtn.Click += SaveToBtn_Click;
            // 
            // OpenWorkDirectoryBtn
            // 
            this.OpenWorkDirectoryBtn.Location = new System.Drawing.Point(494, 5);
            this.OpenWorkDirectoryBtn.Name = "OpenWorkDirectoryBtn";
            this.OpenWorkDirectoryBtn.Size = new System.Drawing.Size(23, 20);
            this.OpenWorkDirectoryBtn.TabIndex = 5;
            this.OpenWorkDirectoryBtn.Text = "...";
            this.OpenWorkDirectoryBtn.UseVisualStyleBackColor = true;
            // 
            // FixImgChckBox
            // 
            this.FixImgChckBox.AutoSize = true;
            this.FixImgChckBox.Location = new System.Drawing.Point(10, 38);
            this.FixImgChckBox.Name = "FixImgChckBox";
            this.FixImgChckBox.Size = new System.Drawing.Size(64, 17);
            this.FixImgChckBox.TabIndex = 8;
            this.FixImgChckBox.Text = "Fix Imgs";
            this.FixImgChckBox.UseVisualStyleBackColor = true;
            this.FixImgChckBox.CheckedChanged += new System.EventHandler(FixImgChckBox_CheckedChanged);
            // 
            // AutoChckBox
            // 
            this.AutoChckBox.AutoSize = true;
            this.AutoChckBox.Location = new System.Drawing.Point(109, 38);
            this.AutoChckBox.Name = "AutoChckBox";
            this.AutoChckBox.Size = new System.Drawing.Size(48, 17);
            this.AutoChckBox.TabIndex = 9;
            this.AutoChckBox.Text = "Auto";
            this.AutoChckBox.UseVisualStyleBackColor = true;
            this.AutoChckBox.CheckedChanged += new System.EventHandler(AutoChckBox_CheckedChanged);
            // 
            // ChekFixedImgsChckBox
            // 
            this.ChekFixedImgsChckBox.AutoSize = true;
            this.ChekFixedImgsChckBox.Location = new System.Drawing.Point(216, 38);
            this.ChekFixedImgsChckBox.Name = "ChekFixedImgsChckBox";
            this.ChekFixedImgsChckBox.Size = new System.Drawing.Size(104, 17);
            this.ChekFixedImgsChckBox.TabIndex = 10;
            this.ChekFixedImgsChckBox.Text = "Chek Fixed Imgs";
            this.ChekFixedImgsChckBox.UseVisualStyleBackColor = true;
            // 
            // OpenFixingImgDirectoryBtn
            // 
            this.OpenFixingImgDirectoryBtn.Location = new System.Drawing.Point(494, 55);
            this.OpenFixingImgDirectoryBtn.Name = "OpenFixingImgDirectoryBtn";
            this.OpenFixingImgDirectoryBtn.Size = new System.Drawing.Size(23, 20);
            this.OpenFixingImgDirectoryBtn.TabIndex = 14;
            this.OpenFixingImgDirectoryBtn.Text = "...";
            this.OpenFixingImgDirectoryBtn.UseVisualStyleBackColor = true;
            // 
            // FixingImgDirectoryTxtBox
            // 
            this.FixingImgDirectoryTxtBox.Location = new System.Drawing.Point(107, 55);
            this.FixingImgDirectoryTxtBox.Name = "FixingImgDirectoryTxtBox";
            this.FixingImgDirectoryTxtBox.Size = new System.Drawing.Size(381, 20);
            this.FixingImgDirectoryTxtBox.TabIndex = 13;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(102, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Fixing Img Directory:";
            // 
            // OpenImgFixingPlanBtn
            // 
            this.OpenImgFixingPlanBtn.Location = new System.Drawing.Point(494, 80);
            this.OpenImgFixingPlanBtn.Name = "OpenImgFixingPlanBtn";
            this.OpenImgFixingPlanBtn.Size = new System.Drawing.Size(23, 20);
            this.OpenImgFixingPlanBtn.TabIndex = 17;
            this.OpenImgFixingPlanBtn.Text = "...";
            this.OpenImgFixingPlanBtn.UseVisualStyleBackColor = true;
            this.OpenImgFixingPlanBtn.Click += new System.EventHandler(OpenImgFixingPlanBtn_Click);
            // 
            // ImgFixingPlanTxtBox
            // 
            this.ImgFixingPlanTxtBox.Location = new System.Drawing.Point(107, 80);
            this.ImgFixingPlanTxtBox.Name = "ImgFixingPlanTxtBox";
            this.ImgFixingPlanTxtBox.Size = new System.Drawing.Size(381, 20);
            this.ImgFixingPlanTxtBox.TabIndex = 16;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 82);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 13);
            this.label3.TabIndex = 15;
            this.label3.Text = "Fixing Plan:";
            // 
            // StitchСhckBox
            // 
            this.StitchСhckBox.AutoSize = true;
            this.StitchСhckBox.Location = new System.Drawing.Point(10, 173);
            this.StitchСhckBox.Name = "StitchСhckBox";
            this.StitchСhckBox.Size = new System.Drawing.Size(78, 17);
            this.StitchСhckBox.TabIndex = 18;
            this.StitchСhckBox.Text = "Stitch Imgs";
            this.StitchСhckBox.UseVisualStyleBackColor = true;
            this.StitchСhckBox.CheckedChanged += new System.EventHandler(StitchСhckBox_CheckedChanged);
            // 
            // ChekStitchPlanСhckBox
            // 
            this.ChekStitchPlanСhckBox.AutoSize = true;
            this.ChekStitchPlanСhckBox.Location = new System.Drawing.Point(109, 105);
            this.ChekStitchPlanСhckBox.Name = "ChekStitchPlanСhckBox";
            this.ChekStitchPlanСhckBox.Size = new System.Drawing.Size(105, 17);
            this.ChekStitchPlanСhckBox.TabIndex = 19;
            this.ChekStitchPlanСhckBox.Text = "Chek Stitch Plan";
            this.ChekStitchPlanСhckBox.UseVisualStyleBackColor = true;
            // 
            // DefaultParametersCheckBox
            // 
            this.DefaultParametersCheckBox.AutoSize = true;
            this.DefaultParametersCheckBox.Location = new System.Drawing.Point(10, 151);
            this.DefaultParametersCheckBox.Name = "DefaultParametersCheckBox";
            this.DefaultParametersCheckBox.Size = new System.Drawing.Size(116, 17);
            this.DefaultParametersCheckBox.TabIndex = 20;
            this.DefaultParametersCheckBox.Text = "Default Parameters";
            this.DefaultParametersCheckBox.UseVisualStyleBackColor = true;
            this.DefaultParametersCheckBox.CheckedChanged += new System.EventHandler(LoadParametrsCheckBox_CheckedChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI Black", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label6.Location = new System.Drawing.Point(308, 179);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(18, 15);
            this.label6.TabIndex = 42;
            this.label6.Text = "%";
            this.label6.Click += new System.EventHandler(label6_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI Black", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.Location = new System.Drawing.Point(308, 154);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(18, 15);
            this.label5.TabIndex = 41;
            this.label5.Text = "%";
            this.label5.Click += new System.EventHandler(label5_Click);
            // 
            // ToLb
            // 
            this.ToLb.AutoSize = true;
            this.ToLb.Font = new System.Drawing.Font("Segoe UI Black", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ToLb.Location = new System.Drawing.Point(232, 176);
            this.ToLb.Name = "ToLb";
            this.ToLb.Size = new System.Drawing.Size(21, 15);
            this.ToLb.TabIndex = 40;
            this.ToLb.Text = "To";
            // 
            // FrLb
            // 
            this.FrLb.AutoSize = true;
            this.FrLb.Font = new System.Drawing.Font("Segoe UI Black", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FrLb.Location = new System.Drawing.Point(229, 154);
            this.FrLb.Name = "FrLb";
            this.FrLb.Size = new System.Drawing.Size(36, 15);
            this.FrLb.TabIndex = 39;
            this.FrLb.Text = "From";
            // 
            // FromTxtBox
            // 
            this.FromTxtBox.Location = new System.Drawing.Point(265, 151);
            this.FromTxtBox.Name = "FromTxtBox";
            this.FromTxtBox.Size = new System.Drawing.Size(38, 20);
            this.FromTxtBox.TabIndex = 38;
            this.FromTxtBox.Text = "0";
            // 
            // ToTxtBox
            // 
            this.ToTxtBox.Location = new System.Drawing.Point(265, 176);
            this.ToTxtBox.Name = "ToTxtBox";
            this.ToTxtBox.Size = new System.Drawing.Size(38, 20);
            this.ToTxtBox.TabIndex = 37;
            this.ToTxtBox.Text = "100";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI Black", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label7.Location = new System.Drawing.Point(136, 175);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(46, 15);
            this.label7.TabIndex = 46;
            this.label7.Text = "Period";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Segoe UI Black", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label8.Location = new System.Drawing.Point(136, 153);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(40, 15);
            this.label8.TabIndex = 45;
            this.label8.Text = "Delta";
            // 
            // DeltaTxtBox
            // 
            this.DeltaTxtBox.Location = new System.Drawing.Point(187, 151);
            this.DeltaTxtBox.Name = "DeltaTxtBox";
            this.DeltaTxtBox.Size = new System.Drawing.Size(30, 20);
            this.DeltaTxtBox.TabIndex = 44;
            this.DeltaTxtBox.Text = "20";
            // 
            // PeriodTxtBox
            // 
            this.PeriodTxtBox.Location = new System.Drawing.Point(187, 176);
            this.PeriodTxtBox.Name = "PeriodTxtBox";
            this.PeriodTxtBox.Size = new System.Drawing.Size(30, 20);
            this.PeriodTxtBox.TabIndex = 43;
            this.PeriodTxtBox.Text = "1";
            // 
            // LoadBtn
            // 
            this.LoadBtn.Location = new System.Drawing.Point(146, 342);
            this.LoadBtn.Name = "LoadBtn";
            this.LoadBtn.Size = new System.Drawing.Size(64, 20);
            this.LoadBtn.TabIndex = 48;
            this.LoadBtn.Text = "Load";
            this.LoadBtn.UseVisualStyleBackColor = true;
            // 
            // SaveResultChckBox
            // 
            this.SaveResultChckBox.AutoSize = true;
            this.SaveResultChckBox.Location = new System.Drawing.Point(10, 195);
            this.SaveResultChckBox.Name = "SaveResultChckBox";
            this.SaveResultChckBox.Size = new System.Drawing.Size(84, 17);
            this.SaveResultChckBox.TabIndex = 49;
            this.SaveResultChckBox.Text = "Save Result";
            this.SaveResultChckBox.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 127);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(95, 13);
            this.label4.TabIndex = 50;
            this.label4.Text = "Working Directory:";
            // 
            // OpenStitchingDirectoryBtn
            // 
            this.OpenStitchingDirectoryBtn.Location = new System.Drawing.Point(494, 125);
            this.OpenStitchingDirectoryBtn.Name = "OpenStitchingDirectoryBtn";
            this.OpenStitchingDirectoryBtn.Size = new System.Drawing.Size(23, 20);
            this.OpenStitchingDirectoryBtn.TabIndex = 52;
            this.OpenStitchingDirectoryBtn.Text = "...";
            this.OpenStitchingDirectoryBtn.UseVisualStyleBackColor = true;
            // 
            // StitchingDirectoryTxtBox
            // 
            this.StitchingDirectoryTxtBox.Location = new System.Drawing.Point(107, 125);
            this.StitchingDirectoryTxtBox.Name = "StitchingDirectoryTxtBox";
            this.StitchingDirectoryTxtBox.Size = new System.Drawing.Size(381, 20);
            this.StitchingDirectoryTxtBox.TabIndex = 51;
            // 
            // FindKeyPointsСhckBox
            // 
            this.FindKeyPointsСhckBox.AutoSize = true;
            this.FindKeyPointsСhckBox.Location = new System.Drawing.Point(10, 105);
            this.FindKeyPointsСhckBox.Name = "FindKeyPointsСhckBox";
            this.FindKeyPointsСhckBox.Size = new System.Drawing.Size(99, 17);
            this.FindKeyPointsСhckBox.TabIndex = 53;
            this.FindKeyPointsСhckBox.Text = "Find Key Points";
            this.FindKeyPointsСhckBox.UseVisualStyleBackColor = true;
            // 
            // SpeedCountingСhckBox
            // 
            this.SpeedCountingСhckBox.AutoSize = true;
            this.SpeedCountingСhckBox.Location = new System.Drawing.Point(10, 216);
            this.SpeedCountingСhckBox.Name = "SpeedCountingСhckBox";
            this.SpeedCountingСhckBox.Size = new System.Drawing.Size(88, 17);
            this.SpeedCountingСhckBox.TabIndex = 54;
            this.SpeedCountingСhckBox.Text = "Count Speed";
            this.SpeedCountingСhckBox.UseVisualStyleBackColor = true;
            this.SpeedCountingСhckBox.CheckedChanged += new System.EventHandler(SpeedCountingСhckBox_CheckedChanged);
            // 
            // MillimetersInPixelTxtBox
            // 
            this.MillimetersInPixelTxtBox.Location = new System.Drawing.Point(9, 239);
            this.MillimetersInPixelTxtBox.Name = "MillimetersInPixelTxtBox";
            this.MillimetersInPixelTxtBox.Size = new System.Drawing.Size(30, 20);
            this.MillimetersInPixelTxtBox.TabIndex = 56;
            this.MillimetersInPixelTxtBox.Text = "5.5";
            this.MillimetersInPixelTxtBox.TextChanged += new System.EventHandler(MillimetersInPixelTxtBox_TextChanged);
            // 
            // TimePerFrameTxtBox
            // 
            this.TimePerFrameTxtBox.Location = new System.Drawing.Point(9, 264);
            this.TimePerFrameTxtBox.Name = "TimePerFrameTxtBox";
            this.TimePerFrameTxtBox.Size = new System.Drawing.Size(30, 20);
            this.TimePerFrameTxtBox.TabIndex = 55;
            this.TimePerFrameTxtBox.Text = "40.0";
            this.TimePerFrameTxtBox.TextChanged += new System.EventHandler(TimePerFrameTxtBox_TextChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(42, 243);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(101, 13);
            this.label9.TabIndex = 57;
            this.label9.Text = " - Millimeters In Pixel";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(41, 268);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(103, 13);
            this.label10.TabIndex = 58;
            this.label10.Text = " - MilliSek Per Frame";
            // 
            // AdditionalFilterChckBox
            // 
            this.AdditionalFilterChckBox.AutoSize = true;
            this.AdditionalFilterChckBox.Location = new System.Drawing.Point(217, 105);
            this.AdditionalFilterChckBox.Name = "AdditionalFilterChckBox";
            this.AdditionalFilterChckBox.Size = new System.Drawing.Size(97, 17);
            this.AdditionalFilterChckBox.TabIndex = 59;
            this.AdditionalFilterChckBox.Text = "Additional Filter";
            this.AdditionalFilterChckBox.UseVisualStyleBackColor = true;
            // 
            // EditingStitchingPlan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(526, 374);
            this.Controls.Add(this.AdditionalFilterChckBox);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.MillimetersInPixelTxtBox);
            this.Controls.Add(this.TimePerFrameTxtBox);
            this.Controls.Add(this.SpeedCountingСhckBox);
            this.Controls.Add(this.FindKeyPointsСhckBox);
            this.Controls.Add(this.OpenStitchingDirectoryBtn);
            this.Controls.Add(this.StitchingDirectoryTxtBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.SaveResultChckBox);
            this.Controls.Add(this.LoadBtn);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.DeltaTxtBox);
            this.Controls.Add(this.PeriodTxtBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.ToLb);
            this.Controls.Add(this.FrLb);
            this.Controls.Add(this.FromTxtBox);
            this.Controls.Add(this.ToTxtBox);
            this.Controls.Add(this.DefaultParametersCheckBox);
            this.Controls.Add(this.ChekStitchPlanСhckBox);
            this.Controls.Add(this.StitchСhckBox);
            this.Controls.Add(this.OpenImgFixingPlanBtn);
            this.Controls.Add(this.ImgFixingPlanTxtBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.OpenFixingImgDirectoryBtn);
            this.Controls.Add(this.FixingImgDirectoryTxtBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ChekFixedImgsChckBox);
            this.Controls.Add(this.AutoChckBox);
            this.Controls.Add(this.FixImgChckBox);
            this.Controls.Add(this.OpenWorkDirectoryBtn);
            this.Controls.Add(this.SaveToBtn);
            this.Controls.Add(this.WorkingDirectoryTxtBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.SaveBtn);
            this.Controls.Add(this.ExitBtn);
            this.MaximumSize = new System.Drawing.Size(542, 413);
            this.MinimumSize = new System.Drawing.Size(542, 413);
            this.Name = "EditingStitchingPlan";
            this.Text = "EditingStitchingPlan";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button ExitBtn;
        private Button SaveBtn;
        private Label label1;
        private TextBox WorkingDirectoryTxtBox;
        private Button SaveToBtn;
        private Button OpenWorkingDirectoryBtn;
        private CheckBox FixImgChckBox;
        private CheckBox AutoChckBox;
        private CheckBox ChekFixedImgsChckBox;
        private Button OpenFixingImgDirectoryBtn;
        private TextBox FixingImgDirectoryTxtBox;
        private Label label2;
        private Button OpenImgFixingPlanBtn;
        private TextBox ImgFixingPlanTxtBox;
        private Label label3;
        private CheckBox StitchСhckBox;
        private CheckBox ChekStitchPlanСhckBox;
        private CheckBox DefaultParametersCheckBox;
        private Label label6;
        private Label label5;
        private Label ToLb;
        private Label FrLb;
        private TextBox FromTxtBox;
        private TextBox ToTxtBox;
        private Label label7;
        private Label label8;
        private TextBox DeltaTxtBox;
        private TextBox PeriodTxtBox;
        private Button LoadBtn;
        private CheckBox SaveResultChckBox;
        private Label label4;
        private Button button1;
        private TextBox StitchingDirectoryTxtBox;
        private Button OpenWorkDirectoryBtn;
        private Button OpenStitchingDirectoryBtn;
        private CheckBox FindKeyPointsСhckBox;
        private CheckBox SpeedCountingСhckBox;
        private TextBox MillimetersInPixelTxtBox;
        private TextBox TimePerFrameTxtBox;
        private Label label9;
        private Label label10;
        private CheckBox AdditionalFilterChckBox;
    }
}