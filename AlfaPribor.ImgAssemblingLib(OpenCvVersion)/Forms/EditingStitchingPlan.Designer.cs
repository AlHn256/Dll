using System.Windows.Forms;

namespace ImgAssemblingLibOpenCV.AditionalForms
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
            this.label1 = new System.Windows.Forms.Label();
            this.WorkingDirectoryTxtBox = new System.Windows.Forms.TextBox();
            this.SaveToBtn = new System.Windows.Forms.Button();
            this.FixImgChckBox = new System.Windows.Forms.CheckBox();
            this.AutoChckBox = new System.Windows.Forms.CheckBox();
            this.ChekFixedImgsChckBox = new System.Windows.Forms.CheckBox();
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
            this.StitchingDirectoryTxtBox = new System.Windows.Forms.TextBox();
            this.FindKeyPointsСhckBox = new System.Windows.Forms.CheckBox();
            this.SpeedCountingСhckBox = new System.Windows.Forms.CheckBox();
            this.MillimetersInPixelTxtBox = new System.Windows.Forms.TextBox();
            this.TimePerFrameTxtBox = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.AdditionalFilterChckBox = new System.Windows.Forms.CheckBox();
            this.OpenResultChckBox = new System.Windows.Forms.CheckBox();
            this.BitMapChckBox = new System.Windows.Forms.CheckBox();
            this.InfoLabel = new System.Windows.Forms.Label();
            this.SavingImgWBitmapChckBox = new System.Windows.Forms.CheckBox();
            this.EditImgFixBtn = new System.Windows.Forms.Button();
            this.AdditionalSettingsBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ExitBtn
            // 
            this.ExitBtn.Location = new System.Drawing.Point(441, 339);
            this.ExitBtn.Name = "ExitBtn";
            this.ExitBtn.Size = new System.Drawing.Size(75, 30);
            this.ExitBtn.TabIndex = 0;
            this.ExitBtn.Text = "Выход";
            this.ExitBtn.UseVisualStyleBackColor = true;
            this.ExitBtn.Visible = false;
            this.ExitBtn.Click += new System.EventHandler(this.ExitBtn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Папка с кадрами:";
            // 
            // WorkingDirectoryTxtBox
            // 
            this.WorkingDirectoryTxtBox.Location = new System.Drawing.Point(121, 33);
            this.WorkingDirectoryTxtBox.Name = "WorkingDirectoryTxtBox";
            this.WorkingDirectoryTxtBox.Size = new System.Drawing.Size(393, 20);
            this.WorkingDirectoryTxtBox.TabIndex = 3;
            this.WorkingDirectoryTxtBox.TextChanged += new System.EventHandler(this.WorkingDirectoryTxtBox_TextChanged);
            // 
            // SaveToBtn
            // 
            this.SaveToBtn.Location = new System.Drawing.Point(279, 339);
            this.SaveToBtn.Name = "SaveToBtn";
            this.SaveToBtn.Size = new System.Drawing.Size(75, 30);
            this.SaveToBtn.TabIndex = 4;
            this.SaveToBtn.Text = "Сохранить";
            this.SaveToBtn.UseVisualStyleBackColor = true;
            this.SaveToBtn.Visible = false;
            this.SaveToBtn.Click += new System.EventHandler(this.SaveToBtn_Click);
            // 
            // FixImgChckBox
            // 
            this.FixImgChckBox.AutoSize = true;
            this.FixImgChckBox.Location = new System.Drawing.Point(14, 61);
            this.FixImgChckBox.Name = "FixImgChckBox";
            this.FixImgChckBox.Size = new System.Drawing.Size(200, 17);
            this.FixImgChckBox.TabIndex = 8;
            this.FixImgChckBox.Text = "Включить исправление дисторсии";
            this.FixImgChckBox.UseVisualStyleBackColor = true;
            this.FixImgChckBox.CheckedChanged += new System.EventHandler(this.FixImgChckBox_CheckedChanged);
            // 
            // AutoChckBox
            // 
            this.AutoChckBox.AutoSize = true;
            this.AutoChckBox.Location = new System.Drawing.Point(334, 60);
            this.AutoChckBox.Name = "AutoChckBox";
            this.AutoChckBox.Size = new System.Drawing.Size(48, 17);
            this.AutoChckBox.TabIndex = 9;
            this.AutoChckBox.Text = "Auto";
            this.AutoChckBox.UseVisualStyleBackColor = true;
            this.AutoChckBox.Visible = false;
            this.AutoChckBox.CheckedChanged += new System.EventHandler(this.AutoChckBox_CheckedChanged);
            // 
            // ChekFixedImgsChckBox
            // 
            this.ChekFixedImgsChckBox.AutoSize = true;
            this.ChekFixedImgsChckBox.Location = new System.Drawing.Point(393, 60);
            this.ChekFixedImgsChckBox.Name = "ChekFixedImgsChckBox";
            this.ChekFixedImgsChckBox.Size = new System.Drawing.Size(104, 17);
            this.ChekFixedImgsChckBox.TabIndex = 10;
            this.ChekFixedImgsChckBox.Text = "Chek Fixed Imgs";
            this.ChekFixedImgsChckBox.UseVisualStyleBackColor = true;
            this.ChekFixedImgsChckBox.Visible = false;
            // 
            // FixingImgDirectoryTxtBox
            // 
            this.FixingImgDirectoryTxtBox.Location = new System.Drawing.Point(121, 106);
            this.FixingImgDirectoryTxtBox.Name = "FixingImgDirectoryTxtBox";
            this.FixingImgDirectoryTxtBox.Size = new System.Drawing.Size(367, 20);
            this.FixingImgDirectoryTxtBox.TabIndex = 13;
            this.FixingImgDirectoryTxtBox.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 109);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(102, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Fixing Img Directory:";
            this.label2.Visible = false;
            // 
            // OpenImgFixingPlanBtn
            // 
            this.OpenImgFixingPlanBtn.Location = new System.Drawing.Point(492, 82);
            this.OpenImgFixingPlanBtn.Name = "OpenImgFixingPlanBtn";
            this.OpenImgFixingPlanBtn.Size = new System.Drawing.Size(22, 20);
            this.OpenImgFixingPlanBtn.TabIndex = 17;
            this.OpenImgFixingPlanBtn.Text = "...";
            this.OpenImgFixingPlanBtn.UseVisualStyleBackColor = true;
            this.OpenImgFixingPlanBtn.Visible = false;
            this.OpenImgFixingPlanBtn.Click += new System.EventHandler(this.OpenImgFixingPlanBtn_Click);
            // 
            // ImgFixingPlanTxtBox
            // 
            this.ImgFixingPlanTxtBox.Location = new System.Drawing.Point(121, 82);
            this.ImgFixingPlanTxtBox.Name = "ImgFixingPlanTxtBox";
            this.ImgFixingPlanTxtBox.Size = new System.Drawing.Size(367, 20);
            this.ImgFixingPlanTxtBox.TabIndex = 16;
            this.ImgFixingPlanTxtBox.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 85);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(105, 13);
            this.label3.TabIndex = 15;
            this.label3.Text = "План исправления:";
            this.label3.Visible = false;
            // 
            // StitchСhckBox
            // 
            this.StitchСhckBox.AutoSize = true;
            this.StitchСhckBox.Location = new System.Drawing.Point(14, 198);
            this.StitchСhckBox.Name = "StitchСhckBox";
            this.StitchСhckBox.Size = new System.Drawing.Size(78, 17);
            this.StitchСhckBox.TabIndex = 18;
            this.StitchСhckBox.Text = "Stitch Imgs";
            this.StitchСhckBox.UseVisualStyleBackColor = true;
            this.StitchСhckBox.CheckedChanged += new System.EventHandler(this.StitchСhckBox_CheckedChanged);
            // 
            // ChekStitchPlanСhckBox
            // 
            this.ChekStitchPlanСhckBox.AutoSize = true;
            this.ChekStitchPlanСhckBox.Location = new System.Drawing.Point(138, 130);
            this.ChekStitchPlanСhckBox.Name = "ChekStitchPlanСhckBox";
            this.ChekStitchPlanСhckBox.Size = new System.Drawing.Size(152, 17);
            this.ChekStitchPlanСhckBox.TabIndex = 19;
            this.ChekStitchPlanСhckBox.Text = "Проверка старого плана";
            this.ChekStitchPlanСhckBox.UseVisualStyleBackColor = true;
            this.ChekStitchPlanСhckBox.Visible = false;
            // 
            // DefaultParametersCheckBox
            // 
            this.DefaultParametersCheckBox.AutoSize = true;
            this.DefaultParametersCheckBox.Location = new System.Drawing.Point(362, 178);
            this.DefaultParametersCheckBox.Name = "DefaultParametersCheckBox";
            this.DefaultParametersCheckBox.Size = new System.Drawing.Size(144, 17);
            this.DefaultParametersCheckBox.TabIndex = 20;
            this.DefaultParametersCheckBox.Text = "Стандарные настройки";
            this.DefaultParametersCheckBox.UseVisualStyleBackColor = true;
            this.DefaultParametersCheckBox.CheckedChanged += new System.EventHandler(this.LoadParametrsCheckBox_CheckedChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI Black", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label6.Location = new System.Drawing.Point(308, 204);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(18, 15);
            this.label6.TabIndex = 42;
            this.label6.Text = "%";
            this.label6.Click += new System.EventHandler(this.label6_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI Black", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.Location = new System.Drawing.Point(308, 179);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(18, 15);
            this.label5.TabIndex = 41;
            this.label5.Text = "%";
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // ToLb
            // 
            this.ToLb.AutoSize = true;
            this.ToLb.Font = new System.Drawing.Font("Segoe UI Black", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ToLb.Location = new System.Drawing.Point(232, 201);
            this.ToLb.Name = "ToLb";
            this.ToLb.Size = new System.Drawing.Size(24, 15);
            this.ToLb.TabIndex = 40;
            this.ToLb.Text = "До";
            // 
            // FrLb
            // 
            this.FrLb.AutoSize = true;
            this.FrLb.Font = new System.Drawing.Font("Segoe UI Black", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FrLb.Location = new System.Drawing.Point(229, 179);
            this.FrLb.Name = "FrLb";
            this.FrLb.Size = new System.Drawing.Size(27, 15);
            this.FrLb.TabIndex = 39;
            this.FrLb.Text = "От";
            // 
            // FromTxtBox
            // 
            this.FromTxtBox.Location = new System.Drawing.Point(265, 176);
            this.FromTxtBox.Name = "FromTxtBox";
            this.FromTxtBox.Size = new System.Drawing.Size(38, 20);
            this.FromTxtBox.TabIndex = 38;
            this.FromTxtBox.Text = "0";
            // 
            // ToTxtBox
            // 
            this.ToTxtBox.Location = new System.Drawing.Point(265, 201);
            this.ToTxtBox.Name = "ToTxtBox";
            this.ToTxtBox.Size = new System.Drawing.Size(38, 20);
            this.ToTxtBox.TabIndex = 37;
            this.ToTxtBox.Text = "100";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI Black", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label7.Location = new System.Drawing.Point(124, 200);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(54, 15);
            this.label7.TabIndex = 46;
            this.label7.Text = "Период";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Segoe UI Black", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label8.Location = new System.Drawing.Point(106, 178);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(74, 15);
            this.label8.TabIndex = 45;
            this.label8.Text = "Смещение";
            // 
            // DeltaTxtBox
            // 
            this.DeltaTxtBox.Location = new System.Drawing.Point(187, 176);
            this.DeltaTxtBox.Name = "DeltaTxtBox";
            this.DeltaTxtBox.Size = new System.Drawing.Size(30, 20);
            this.DeltaTxtBox.TabIndex = 44;
            this.DeltaTxtBox.Text = "0";
            // 
            // PeriodTxtBox
            // 
            this.PeriodTxtBox.Location = new System.Drawing.Point(187, 201);
            this.PeriodTxtBox.Name = "PeriodTxtBox";
            this.PeriodTxtBox.Size = new System.Drawing.Size(30, 20);
            this.PeriodTxtBox.TabIndex = 43;
            this.PeriodTxtBox.Text = "1";
            // 
            // LoadBtn
            // 
            this.LoadBtn.Location = new System.Drawing.Point(360, 339);
            this.LoadBtn.Name = "LoadBtn";
            this.LoadBtn.Size = new System.Drawing.Size(75, 30);
            this.LoadBtn.TabIndex = 48;
            this.LoadBtn.Text = "Загрузить";
            this.LoadBtn.UseVisualStyleBackColor = true;
            this.LoadBtn.Visible = false;
            this.LoadBtn.Click += new System.EventHandler(this.LoadBtn_Click);
            // 
            // SaveResultChckBox
            // 
            this.SaveResultChckBox.AutoSize = true;
            this.SaveResultChckBox.Checked = true;
            this.SaveResultChckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.SaveResultChckBox.Location = new System.Drawing.Point(15, 295);
            this.SaveResultChckBox.Name = "SaveResultChckBox";
            this.SaveResultChckBox.Size = new System.Drawing.Size(133, 17);
            this.SaveResultChckBox.TabIndex = 49;
            this.SaveResultChckBox.Text = "Сохранить результат";
            this.SaveResultChckBox.UseVisualStyleBackColor = true;
            this.SaveResultChckBox.CheckedChanged += new System.EventHandler(this.SaveResultChckBox_CheckedChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 152);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(95, 13);
            this.label4.TabIndex = 50;
            this.label4.Text = "Working Directory:";
            this.label4.Visible = false;
            // 
            // StitchingDirectoryTxtBox
            // 
            this.StitchingDirectoryTxtBox.Location = new System.Drawing.Point(121, 150);
            this.StitchingDirectoryTxtBox.Name = "StitchingDirectoryTxtBox";
            this.StitchingDirectoryTxtBox.Size = new System.Drawing.Size(393, 20);
            this.StitchingDirectoryTxtBox.TabIndex = 51;
            this.StitchingDirectoryTxtBox.Visible = false;
            // 
            // FindKeyPointsСhckBox
            // 
            this.FindKeyPointsСhckBox.AutoSize = true;
            this.FindKeyPointsСhckBox.Location = new System.Drawing.Point(14, 130);
            this.FindKeyPointsСhckBox.Name = "FindKeyPointsСhckBox";
            this.FindKeyPointsСhckBox.Size = new System.Drawing.Size(142, 17);
            this.FindKeyPointsСhckBox.TabIndex = 53;
            this.FindKeyPointsСhckBox.Text = "Поиск ключевых точек";
            this.FindKeyPointsСhckBox.UseVisualStyleBackColor = true;
            this.FindKeyPointsСhckBox.Visible = false;
            // 
            // SpeedCountingСhckBox
            // 
            this.SpeedCountingСhckBox.AutoSize = true;
            this.SpeedCountingСhckBox.Location = new System.Drawing.Point(14, 221);
            this.SpeedCountingСhckBox.Name = "SpeedCountingСhckBox";
            this.SpeedCountingСhckBox.Size = new System.Drawing.Size(88, 17);
            this.SpeedCountingСhckBox.TabIndex = 54;
            this.SpeedCountingСhckBox.Text = "Count Speed";
            this.SpeedCountingСhckBox.UseVisualStyleBackColor = true;
            this.SpeedCountingСhckBox.Visible = false;
            this.SpeedCountingСhckBox.CheckedChanged += new System.EventHandler(this.SpeedCountingСhckBox_CheckedChanged);
            // 
            // MillimetersInPixelTxtBox
            // 
            this.MillimetersInPixelTxtBox.Location = new System.Drawing.Point(14, 244);
            this.MillimetersInPixelTxtBox.Name = "MillimetersInPixelTxtBox";
            this.MillimetersInPixelTxtBox.Size = new System.Drawing.Size(37, 20);
            this.MillimetersInPixelTxtBox.TabIndex = 56;
            this.MillimetersInPixelTxtBox.Text = "5.5";
            this.MillimetersInPixelTxtBox.Visible = false;
            this.MillimetersInPixelTxtBox.TextChanged += new System.EventHandler(this.MillimetersInPixelTxtBox_TextChanged);
            // 
            // TimePerFrameTxtBox
            // 
            this.TimePerFrameTxtBox.Location = new System.Drawing.Point(14, 269);
            this.TimePerFrameTxtBox.Name = "TimePerFrameTxtBox";
            this.TimePerFrameTxtBox.Size = new System.Drawing.Size(37, 20);
            this.TimePerFrameTxtBox.TabIndex = 55;
            this.TimePerFrameTxtBox.Text = "40.0";
            this.TimePerFrameTxtBox.Visible = false;
            this.TimePerFrameTxtBox.TextChanged += new System.EventHandler(this.TimePerFrameTxtBox_TextChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(57, 248);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(172, 13);
            this.label9.TabIndex = 57;
            this.label9.Text = "- Миллиметров в одном пикселе";
            this.label9.Visible = false;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(56, 273);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(207, 13);
            this.label10.TabIndex = 58;
            this.label10.Text = "- Время одного кадра в миллисекундах";
            this.label10.Visible = false;
            // 
            // AdditionalFilterChckBox
            // 
            this.AdditionalFilterChckBox.AutoSize = true;
            this.AdditionalFilterChckBox.Location = new System.Drawing.Point(297, 130);
            this.AdditionalFilterChckBox.Name = "AdditionalFilterChckBox";
            this.AdditionalFilterChckBox.Size = new System.Drawing.Size(106, 17);
            this.AdditionalFilterChckBox.TabIndex = 59;
            this.AdditionalFilterChckBox.Text = "Вкл доп фильтр";
            this.AdditionalFilterChckBox.UseVisualStyleBackColor = true;
            this.AdditionalFilterChckBox.Visible = false;
            // 
            // OpenResultChckBox
            // 
            this.OpenResultChckBox.AutoSize = true;
            this.OpenResultChckBox.Checked = true;
            this.OpenResultChckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.OpenResultChckBox.Location = new System.Drawing.Point(15, 318);
            this.OpenResultChckBox.Name = "OpenResultChckBox";
            this.OpenResultChckBox.Size = new System.Drawing.Size(185, 17);
            this.OpenResultChckBox.TabIndex = 61;
            this.OpenResultChckBox.Text = "Показать реультирующий кадр";
            this.OpenResultChckBox.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.OpenResultChckBox.UseVisualStyleBackColor = true;
            this.OpenResultChckBox.CheckedChanged += new System.EventHandler(this.OpenResultChckBox_CheckedChanged);
            // 
            // BitMapChckBox
            // 
            this.BitMapChckBox.AutoSize = true;
            this.BitMapChckBox.Location = new System.Drawing.Point(15, 10);
            this.BitMapChckBox.Name = "BitMapChckBox";
            this.BitMapChckBox.Size = new System.Drawing.Size(165, 17);
            this.BitMapChckBox.TabIndex = 62;
            this.BitMapChckBox.Text = "Работа с массивом кадров";
            this.BitMapChckBox.UseVisualStyleBackColor = true;
            this.BitMapChckBox.Visible = false;
            this.BitMapChckBox.CheckedChanged += new System.EventHandler(this.BitMapChckBox_CheckedChanged);
            // 
            // InfoLabel
            // 
            this.InfoLabel.AutoSize = true;
            this.InfoLabel.Location = new System.Drawing.Point(205, 318);
            this.InfoLabel.Name = "InfoLabel";
            this.InfoLabel.Size = new System.Drawing.Size(0, 13);
            this.InfoLabel.TabIndex = 63;
            // 
            // SavingImgWBitmapChckBox
            // 
            this.SavingImgWBitmapChckBox.AutoSize = true;
            this.SavingImgWBitmapChckBox.Enabled = false;
            this.SavingImgWBitmapChckBox.Location = new System.Drawing.Point(495, 109);
            this.SavingImgWBitmapChckBox.Name = "SavingImgWBitmapChckBox";
            this.SavingImgWBitmapChckBox.Size = new System.Drawing.Size(15, 14);
            this.SavingImgWBitmapChckBox.TabIndex = 64;
            this.SavingImgWBitmapChckBox.UseVisualStyleBackColor = true;
            this.SavingImgWBitmapChckBox.Visible = false;
            this.SavingImgWBitmapChckBox.CheckedChanged += new System.EventHandler(this.SavingImgWBitmapChckBox_CheckedChanged);
            // 
            // EditImgFixBtn
            // 
            this.EditImgFixBtn.Location = new System.Drawing.Point(223, 56);
            this.EditImgFixBtn.Name = "EditImgFixBtn";
            this.EditImgFixBtn.Size = new System.Drawing.Size(95, 22);
            this.EditImgFixBtn.TabIndex = 65;
            this.EditImgFixBtn.Text = "Редактировать";
            this.EditImgFixBtn.UseVisualStyleBackColor = true;
            this.EditImgFixBtn.Click += new System.EventHandler(this.EditImgFixBtn_Click);
            // 
            // AdditionalSettingsBtn
            // 
            this.AdditionalSettingsBtn.Location = new System.Drawing.Point(12, 339);
            this.AdditionalSettingsBtn.Name = "AdditionalSettingsBtn";
            this.AdditionalSettingsBtn.Size = new System.Drawing.Size(97, 30);
            this.AdditionalSettingsBtn.TabIndex = 66;
            this.AdditionalSettingsBtn.Text = "Доп. настройки";
            this.AdditionalSettingsBtn.UseVisualStyleBackColor = true;
            this.AdditionalSettingsBtn.Click += new System.EventHandler(this.AdditionalSettingsBtn_Click);
            // 
            // EditingStitchingPlan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(526, 374);
            this.Controls.Add(this.AdditionalSettingsBtn);
            this.Controls.Add(this.EditImgFixBtn);
            this.Controls.Add(this.SavingImgWBitmapChckBox);
            this.Controls.Add(this.InfoLabel);
            this.Controls.Add(this.BitMapChckBox);
            this.Controls.Add(this.OpenResultChckBox);
            this.Controls.Add(this.AdditionalFilterChckBox);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.MillimetersInPixelTxtBox);
            this.Controls.Add(this.TimePerFrameTxtBox);
            this.Controls.Add(this.SpeedCountingСhckBox);
            this.Controls.Add(this.FindKeyPointsСhckBox);
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
            this.Controls.Add(this.FixingImgDirectoryTxtBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ChekFixedImgsChckBox);
            this.Controls.Add(this.AutoChckBox);
            this.Controls.Add(this.FixImgChckBox);
            this.Controls.Add(this.SaveToBtn);
            this.Controls.Add(this.WorkingDirectoryTxtBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ExitBtn);
            this.MaximumSize = new System.Drawing.Size(542, 413);
            this.MinimumSize = new System.Drawing.Size(542, 413);
            this.Name = "EditingStitchingPlan";
            this.Text = "EditingStitchingPlan";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.EditingStitchingPlan_FormClosing);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.EditingStitchingPlan_MouseClick);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button ExitBtn;
        private Label label1;
        private TextBox WorkingDirectoryTxtBox;
        private Button SaveToBtn;
        private Button OpenWorkingDirectoryBtn;
        private CheckBox FixImgChckBox;
        private CheckBox AutoChckBox;
        private CheckBox ChekFixedImgsChckBox;
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
        private CheckBox FindKeyPointsСhckBox;
        private CheckBox SpeedCountingСhckBox;
        private TextBox MillimetersInPixelTxtBox;
        private TextBox TimePerFrameTxtBox;
        private Label label9;
        private Label label10;
        private CheckBox AdditionalFilterChckBox;
        private CheckBox OpenResultChckBox;
        private CheckBox BitMapChckBox;
        private Label InfoLabel;
        private CheckBox SavingImgWBitmapChckBox;
        private Button EditImgFixBtn;
        private Button AdditionalSettingsBtn;
    }
}