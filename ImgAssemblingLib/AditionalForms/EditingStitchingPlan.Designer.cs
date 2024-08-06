using System.Drawing;
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
            ExitBtn = new Button();
            SaveBtn = new Button();
            label1 = new Label();
            WorkingDirectoryTxtBox = new TextBox();
            SaveToBtn = new Button();
            OpenWorkDirectoryBtn = new Button();
            FixFileNamesChckBox = new CheckBox();
            CheckFileNamesChckBox = new CheckBox();
            FixImgChckBox = new CheckBox();
            AutoChckBox = new CheckBox();
            ChekFixedImgsChckBox = new CheckBox();
            FindCopyChckBox = new CheckBox();
            OpenFixingImgDirectoryBtn = new Button();
            FixingImgDirectoryTxtBox = new TextBox();
            label2 = new Label();
            OpenImgFixingPlanBtn = new Button();
            ImgFixingPlanTxtBox = new TextBox();
            label3 = new Label();
            StitchСhckBox = new CheckBox();
            ChekStitchPlanСhckBox = new CheckBox();
            DefaultParametersCheckBox = new CheckBox();
            label6 = new Label();
            label5 = new Label();
            ToLb = new Label();
            FrLb = new Label();
            FromTxtBox = new TextBox();
            ToTxtBox = new TextBox();
            label7 = new Label();
            label8 = new Label();
            DeltaTxtBox = new TextBox();
            PeriodTxtBox = new TextBox();
            LoadBtn = new Button();
            SaveResultChckBox = new CheckBox();
            label4 = new Label();
            OpenStitchingDirectoryBtn = new Button();
            StitchingDirectoryTxtBox = new TextBox();
            FindKeyPointsСhckBox = new CheckBox();
            SpeedCountingСhckBox = new CheckBox();
            MillimetersInPixelTxtBox = new TextBox();
            TimePerFrameTxtBox = new TextBox();
            label9 = new Label();
            label10 = new Label();
            AdditionalFilterChckBox = new CheckBox();
            SuspendLayout();
            // 
            // ExitBtn
            // 
            ExitBtn.Location = new Point(511, 381);
            ExitBtn.Name = "ExitBtn";
            ExitBtn.Size = new Size(75, 23);
            ExitBtn.TabIndex = 0;
            ExitBtn.Text = "Exit";
            ExitBtn.UseVisualStyleBackColor = true;
            ExitBtn.Click += ExitBtn_Click;
            // 
            // SaveBtn
            // 
            SaveBtn.Location = new Point(270, 381);
            SaveBtn.Name = "SaveBtn";
            SaveBtn.Size = new Size(75, 23);
            SaveBtn.TabIndex = 1;
            SaveBtn.Text = "Save";
            SaveBtn.UseVisualStyleBackColor = true;
            SaveBtn.Click += SaveBtn_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(10, 9);
            label1.Name = "label1";
            label1.Size = new Size(106, 15);
            label1.TabIndex = 2;
            label1.Text = "Working Directory:";
            // 
            // WorkingDirectoryTxtBox
            // 
            WorkingDirectoryTxtBox.Location = new Point(121, 6);
            WorkingDirectoryTxtBox.Name = "WorkingDirectoryTxtBox";
            WorkingDirectoryTxtBox.Size = new Size(432, 23);
            WorkingDirectoryTxtBox.TabIndex = 3;
            // 
            // SaveToBtn
            // 
            SaveToBtn.Location = new Point(351, 381);
            SaveToBtn.Name = "SaveToBtn";
            SaveToBtn.Size = new Size(75, 23);
            SaveToBtn.TabIndex = 4;
            SaveToBtn.Text = "Save to";
            SaveToBtn.UseVisualStyleBackColor = true;
            SaveToBtn.Click += SaveToBtn_Click;
            // 
            // OpenWorkDirectoryBtn
            // 
            OpenWorkDirectoryBtn.Location = new Point(559, 6);
            OpenWorkDirectoryBtn.Name = "OpenWorkDirectoryBtn";
            OpenWorkDirectoryBtn.Size = new Size(27, 23);
            OpenWorkDirectoryBtn.TabIndex = 5;
            OpenWorkDirectoryBtn.Text = "...";
            OpenWorkDirectoryBtn.UseVisualStyleBackColor = true;
            // 
            // FixFileNamesChckBox
            // 
            FixFileNamesChckBox.AutoSize = true;
            FixFileNamesChckBox.Location = new Point(12, 71);
            FixFileNamesChckBox.Name = "FixFileNamesChckBox";
            FixFileNamesChckBox.Size = new Size(102, 19);
            FixFileNamesChckBox.TabIndex = 6;
            FixFileNamesChckBox.Text = "Fix File Names";
            FixFileNamesChckBox.UseVisualStyleBackColor = true;
            // 
            // CheckFileNamesChckBox
            // 
            CheckFileNamesChckBox.AutoSize = true;
            CheckFileNamesChckBox.Location = new Point(12, 46);
            CheckFileNamesChckBox.Name = "CheckFileNamesChckBox";
            CheckFileNamesChckBox.Size = new Size(120, 19);
            CheckFileNamesChckBox.TabIndex = 7;
            CheckFileNamesChckBox.Text = "Check File Names";
            CheckFileNamesChckBox.UseVisualStyleBackColor = true;
            // 
            // FixImgChckBox
            // 
            FixImgChckBox.AutoSize = true;
            FixImgChckBox.Location = new Point(12, 121);
            FixImgChckBox.Name = "FixImgChckBox";
            FixImgChckBox.Size = new Size(70, 19);
            FixImgChckBox.TabIndex = 8;
            FixImgChckBox.Text = "Fix Imgs";
            FixImgChckBox.UseVisualStyleBackColor = true;
            FixImgChckBox.CheckedChanged += FixImgChckBox_CheckedChanged;
            // 
            // AutoChckBox
            // 
            AutoChckBox.AutoSize = true;
            AutoChckBox.Location = new Point(121, 121);
            AutoChckBox.Name = "AutoChckBox";
            AutoChckBox.Size = new Size(52, 19);
            AutoChckBox.TabIndex = 9;
            AutoChckBox.Text = "Auto";
            AutoChckBox.UseVisualStyleBackColor = true;
            AutoChckBox.CheckedChanged += AutoChckBox_CheckedChanged;
            // 
            // ChekFixedImgsChckBox
            // 
            ChekFixedImgsChckBox.AutoSize = true;
            ChekFixedImgsChckBox.Location = new Point(179, 121);
            ChekFixedImgsChckBox.Name = "ChekFixedImgsChckBox";
            ChekFixedImgsChckBox.Size = new Size(113, 19);
            ChekFixedImgsChckBox.TabIndex = 10;
            ChekFixedImgsChckBox.Text = "Chek Fixed Imgs";
            ChekFixedImgsChckBox.UseVisualStyleBackColor = true;
            // 
            // FindCopyChckBox
            // 
            FindCopyChckBox.AutoSize = true;
            FindCopyChckBox.Location = new Point(12, 96);
            FindCopyChckBox.Name = "FindCopyChckBox";
            FindCopyChckBox.Size = new Size(123, 19);
            FindCopyChckBox.TabIndex = 11;
            FindCopyChckBox.Text = "Find and Del Copy";
            FindCopyChckBox.UseVisualStyleBackColor = true;
            // 
            // OpenFixingImgDirectoryBtn
            // 
            OpenFixingImgDirectoryBtn.Location = new Point(559, 141);
            OpenFixingImgDirectoryBtn.Name = "OpenFixingImgDirectoryBtn";
            OpenFixingImgDirectoryBtn.Size = new Size(27, 23);
            OpenFixingImgDirectoryBtn.TabIndex = 14;
            OpenFixingImgDirectoryBtn.Text = "...";
            OpenFixingImgDirectoryBtn.UseVisualStyleBackColor = true;
            // 
            // FixingImgDirectoryTxtBox
            // 
            FixingImgDirectoryTxtBox.Location = new Point(121, 141);
            FixingImgDirectoryTxtBox.Name = "FixingImgDirectoryTxtBox";
            FixingImgDirectoryTxtBox.Size = new Size(432, 23);
            FixingImgDirectoryTxtBox.TabIndex = 13;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(7, 144);
            label2.Name = "label2";
            label2.Size = new Size(117, 15);
            label2.TabIndex = 12;
            label2.Text = "Fixing Img Directory:";
            // 
            // OpenImgFixingPlanBtn
            // 
            OpenImgFixingPlanBtn.Location = new Point(559, 170);
            OpenImgFixingPlanBtn.Name = "OpenImgFixingPlanBtn";
            OpenImgFixingPlanBtn.Size = new Size(27, 23);
            OpenImgFixingPlanBtn.TabIndex = 17;
            OpenImgFixingPlanBtn.Text = "...";
            OpenImgFixingPlanBtn.UseVisualStyleBackColor = true;
            OpenImgFixingPlanBtn.Click += OpenImgFixingPlanBtn_Click;
            // 
            // ImgFixingPlanTxtBox
            // 
            ImgFixingPlanTxtBox.Location = new Point(121, 170);
            ImgFixingPlanTxtBox.Name = "ImgFixingPlanTxtBox";
            ImgFixingPlanTxtBox.Size = new Size(432, 23);
            ImgFixingPlanTxtBox.TabIndex = 16;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(9, 172);
            label3.Name = "label3";
            label3.Size = new Size(68, 15);
            label3.TabIndex = 15;
            label3.Text = "Fixing Plan:";
            // 
            // StitchСhckBox
            // 
            StitchСhckBox.AutoSize = true;
            StitchСhckBox.Location = new Point(12, 277);
            StitchСhckBox.Name = "StitchСhckBox";
            StitchСhckBox.Size = new Size(85, 19);
            StitchСhckBox.TabIndex = 18;
            StitchСhckBox.Text = "Stitch Imgs";
            StitchСhckBox.UseVisualStyleBackColor = true;
            StitchСhckBox.CheckedChanged += StitchСhckBox_CheckedChanged;
            // 
            // ChekStitchPlanСhckBox
            // 
            ChekStitchPlanСhckBox.AutoSize = true;
            ChekStitchPlanСhckBox.Location = new Point(123, 199);
            ChekStitchPlanСhckBox.Name = "ChekStitchPlanСhckBox";
            ChekStitchPlanСhckBox.Size = new Size(112, 19);
            ChekStitchPlanСhckBox.TabIndex = 19;
            ChekStitchPlanСhckBox.Text = "Chek Stitch Plan";
            ChekStitchPlanСhckBox.UseVisualStyleBackColor = true;
            // 
            // DefaultParametersCheckBox
            // 
            DefaultParametersCheckBox.AutoSize = true;
            DefaultParametersCheckBox.Location = new Point(12, 252);
            DefaultParametersCheckBox.Name = "DefaultParametersCheckBox";
            DefaultParametersCheckBox.Size = new Size(126, 19);
            DefaultParametersCheckBox.TabIndex = 20;
            DefaultParametersCheckBox.Text = "Default Parameters";
            DefaultParametersCheckBox.UseVisualStyleBackColor = true;
            DefaultParametersCheckBox.CheckedChanged += LoadParametrsCheckBox_CheckedChanged;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI Black", 9F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 204);
            label6.Location = new Point(335, 287);
            label6.Name = "label6";
            label6.Size = new Size(18, 15);
            label6.TabIndex = 42;
            label6.Text = "%";
            label6.Click += label6_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI Black", 9F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 204);
            label5.Location = new Point(335, 258);
            label5.Name = "label5";
            label5.Size = new Size(18, 15);
            label5.TabIndex = 41;
            label5.Text = "%";
            label5.Click += label5_Click;
            // 
            // ToLb
            // 
            ToLb.AutoSize = true;
            ToLb.Font = new Font("Segoe UI Black", 9F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 204);
            ToLb.Location = new Point(246, 282);
            ToLb.Name = "ToLb";
            ToLb.Size = new Size(21, 15);
            ToLb.TabIndex = 40;
            ToLb.Text = "To";
            // 
            // FrLb
            // 
            FrLb.AutoSize = true;
            FrLb.Font = new Font("Segoe UI Black", 9F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 204);
            FrLb.Location = new Point(243, 256);
            FrLb.Name = "FrLb";
            FrLb.Size = new Size(36, 15);
            FrLb.TabIndex = 39;
            FrLb.Text = "From";
            // 
            // FromTxtBox
            // 
            FromTxtBox.Location = new Point(285, 253);
            FromTxtBox.Name = "FromTxtBox";
            FromTxtBox.Size = new Size(44, 23);
            FromTxtBox.TabIndex = 38;
            FromTxtBox.Text = "0";
            // 
            // ToTxtBox
            // 
            ToTxtBox.Location = new Point(285, 282);
            ToTxtBox.Name = "ToTxtBox";
            ToTxtBox.Size = new Size(44, 23);
            ToTxtBox.TabIndex = 37;
            ToTxtBox.Text = "100";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI Black", 9F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 204);
            label7.Location = new Point(143, 282);
            label7.Name = "label7";
            label7.Size = new Size(46, 15);
            label7.TabIndex = 46;
            label7.Text = "Period";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Segoe UI Black", 9F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 204);
            label8.Location = new Point(144, 256);
            label8.Name = "label8";
            label8.Size = new Size(40, 15);
            label8.TabIndex = 45;
            label8.Text = "Delta";
            // 
            // DeltaTxtBox
            // 
            DeltaTxtBox.Location = new Point(194, 253);
            DeltaTxtBox.Name = "DeltaTxtBox";
            DeltaTxtBox.Size = new Size(34, 23);
            DeltaTxtBox.TabIndex = 44;
            DeltaTxtBox.Text = "20";
            // 
            // PeriodTxtBox
            // 
            PeriodTxtBox.Location = new Point(194, 282);
            PeriodTxtBox.Name = "PeriodTxtBox";
            PeriodTxtBox.Size = new Size(34, 23);
            PeriodTxtBox.TabIndex = 43;
            PeriodTxtBox.Text = "1";
            // 
            // LoadBtn
            // 
            LoadBtn.Location = new Point(430, 381);
            LoadBtn.Name = "LoadBtn";
            LoadBtn.Size = new Size(75, 23);
            LoadBtn.TabIndex = 48;
            LoadBtn.Text = "Load";
            LoadBtn.UseVisualStyleBackColor = true;
            LoadBtn.Click += LoadBtn_Click;
            // 
            // SaveResultChckBox
            // 
            SaveResultChckBox.AutoSize = true;
            SaveResultChckBox.Location = new Point(12, 302);
            SaveResultChckBox.Name = "SaveResultChckBox";
            SaveResultChckBox.Size = new Size(85, 19);
            SaveResultChckBox.TabIndex = 49;
            SaveResultChckBox.Text = "Save Result";
            SaveResultChckBox.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(8, 224);
            label4.Name = "label4";
            label4.Size = new Size(106, 15);
            label4.TabIndex = 50;
            label4.Text = "Working Directory:";
            // 
            // OpenStitchingDirectoryBtn
            // 
            OpenStitchingDirectoryBtn.Location = new Point(559, 221);
            OpenStitchingDirectoryBtn.Name = "OpenStitchingDirectoryBtn";
            OpenStitchingDirectoryBtn.Size = new Size(27, 23);
            OpenStitchingDirectoryBtn.TabIndex = 52;
            OpenStitchingDirectoryBtn.Text = "...";
            OpenStitchingDirectoryBtn.UseVisualStyleBackColor = true;
            // 
            // StitchingDirectoryTxtBox
            // 
            StitchingDirectoryTxtBox.Location = new Point(121, 221);
            StitchingDirectoryTxtBox.Name = "StitchingDirectoryTxtBox";
            StitchingDirectoryTxtBox.Size = new Size(432, 23);
            StitchingDirectoryTxtBox.TabIndex = 51;
            // 
            // FindKeyPointsСhckBox
            // 
            FindKeyPointsСhckBox.AutoSize = true;
            FindKeyPointsСhckBox.Location = new Point(12, 199);
            FindKeyPointsСhckBox.Name = "FindKeyPointsСhckBox";
            FindKeyPointsСhckBox.Size = new Size(107, 19);
            FindKeyPointsСhckBox.TabIndex = 53;
            FindKeyPointsСhckBox.Text = "Find Key Points";
            FindKeyPointsСhckBox.UseVisualStyleBackColor = true;
            // 
            // SpeedCountingСhckBox
            // 
            SpeedCountingСhckBox.AutoSize = true;
            SpeedCountingСhckBox.Location = new Point(12, 327);
            SpeedCountingСhckBox.Name = "SpeedCountingСhckBox";
            SpeedCountingСhckBox.Size = new Size(94, 19);
            SpeedCountingСhckBox.TabIndex = 54;
            SpeedCountingСhckBox.Text = "Count Speed";
            SpeedCountingСhckBox.UseVisualStyleBackColor = true;
            SpeedCountingСhckBox.CheckedChanged += SpeedCountingСhckBox_CheckedChanged;
            // 
            // MillimetersInPixelTxtBox
            // 
            MillimetersInPixelTxtBox.Location = new Point(12, 352);
            MillimetersInPixelTxtBox.Name = "MillimetersInPixelTxtBox";
            MillimetersInPixelTxtBox.Size = new Size(34, 23);
            MillimetersInPixelTxtBox.TabIndex = 56;
            MillimetersInPixelTxtBox.Text = "5.5";
            MillimetersInPixelTxtBox.TextChanged += MillimetersInPixelTxtBox_TextChanged;
            // 
            // TimePerFrameTxtBox
            // 
            TimePerFrameTxtBox.Location = new Point(12, 381);
            TimePerFrameTxtBox.Name = "TimePerFrameTxtBox";
            TimePerFrameTxtBox.Size = new Size(34, 23);
            TimePerFrameTxtBox.TabIndex = 55;
            TimePerFrameTxtBox.Text = "40.0";
            TimePerFrameTxtBox.TextChanged += TimePerFrameTxtBox_TextChanged;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(47, 356);
            label9.Name = "label9";
            label9.Size = new Size(118, 15);
            label9.TabIndex = 57;
            label9.Text = " - Millimeters In Pixel";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(46, 385);
            label10.Name = "label10";
            label10.Size = new Size(115, 15);
            label10.TabIndex = 58;
            label10.Text = " - MilliSek Per Frame";
            // 
            // AdditionalFilterChckBox
            // 
            AdditionalFilterChckBox.AutoSize = true;
            AdditionalFilterChckBox.Location = new Point(246, 199);
            AdditionalFilterChckBox.Name = "AdditionalFilterChckBox";
            AdditionalFilterChckBox.Size = new Size(110, 19);
            AdditionalFilterChckBox.TabIndex = 59;
            AdditionalFilterChckBox.Text = "Additional Filter";
            AdditionalFilterChckBox.UseVisualStyleBackColor = true;
            // 
            // EditingStitchingPlan
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(614, 431);
            Controls.Add(AdditionalFilterChckBox);
            Controls.Add(label10);
            Controls.Add(label9);
            Controls.Add(MillimetersInPixelTxtBox);
            Controls.Add(TimePerFrameTxtBox);
            Controls.Add(SpeedCountingСhckBox);
            Controls.Add(FindKeyPointsСhckBox);
            Controls.Add(OpenStitchingDirectoryBtn);
            Controls.Add(StitchingDirectoryTxtBox);
            Controls.Add(label4);
            Controls.Add(SaveResultChckBox);
            Controls.Add(LoadBtn);
            Controls.Add(label7);
            Controls.Add(label8);
            Controls.Add(DeltaTxtBox);
            Controls.Add(PeriodTxtBox);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(ToLb);
            Controls.Add(FrLb);
            Controls.Add(FromTxtBox);
            Controls.Add(ToTxtBox);
            Controls.Add(DefaultParametersCheckBox);
            Controls.Add(ChekStitchPlanСhckBox);
            Controls.Add(StitchСhckBox);
            Controls.Add(OpenImgFixingPlanBtn);
            Controls.Add(ImgFixingPlanTxtBox);
            Controls.Add(label3);
            Controls.Add(OpenFixingImgDirectoryBtn);
            Controls.Add(FixingImgDirectoryTxtBox);
            Controls.Add(label2);
            Controls.Add(FindCopyChckBox);
            Controls.Add(ChekFixedImgsChckBox);
            Controls.Add(AutoChckBox);
            Controls.Add(FixImgChckBox);
            Controls.Add(CheckFileNamesChckBox);
            Controls.Add(FixFileNamesChckBox);
            Controls.Add(OpenWorkDirectoryBtn);
            Controls.Add(SaveToBtn);
            Controls.Add(WorkingDirectoryTxtBox);
            Controls.Add(label1);
            Controls.Add(SaveBtn);
            Controls.Add(ExitBtn);
            MaximumSize = new Size(630, 470);
            MinimumSize = new Size(630, 470);
            Name = "EditingStitchingPlan";
            Text = "EditingStitchingPlan";
            MouseClick += EditingStitchingPlan_MouseClick;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button ExitBtn;
        private Button SaveBtn;
        private Label label1;
        private TextBox WorkingDirectoryTxtBox;
        private Button SaveToBtn;
        private Button OpenWorkingDirectoryBtn;
        private CheckBox FixFileNamesChckBox;
        private CheckBox CheckFileNamesChckBox;
        private CheckBox FixImgChckBox;
        private CheckBox AutoChckBox;
        private CheckBox ChekFixedImgsChckBox;
        private CheckBox FindCopyChckBox;
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