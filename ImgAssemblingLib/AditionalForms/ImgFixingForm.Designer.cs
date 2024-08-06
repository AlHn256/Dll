using System.Drawing;
using System.Windows.Forms;

namespace ImgAssemblingLib.AditionalForms
{
    partial class ImgFixingForm
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.CorrectFilesBtn = new System.Windows.Forms.Button();
            this.ABtnUp = new System.Windows.Forms.Button();
            this.ABtnDn = new System.Windows.Forms.Button();
            this.BBtnDn = new System.Windows.Forms.Button();
            this.BBtnUp = new System.Windows.Forms.Button();
            this.CBtnDn = new System.Windows.Forms.Button();
            this.CBtnUp = new System.Windows.Forms.Button();
            this.DBtnDn = new System.Windows.Forms.Button();
            this.DBtnUp = new System.Windows.Forms.Button();
            this.InputDirTxtBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.OutputDirTxtBox = new System.Windows.Forms.TextBox();
            this.InputFileTxtBox = new System.Windows.Forms.TextBox();
            this.ATxtBox = new System.Windows.Forms.TextBox();
            this.BTxtBox = new System.Windows.Forms.TextBox();
            this.CTxtBox = new System.Windows.Forms.TextBox();
            this.DTxtBox = new System.Windows.Forms.TextBox();
            this.RotValTxtBox = new System.Windows.Forms.TextBox();
            this.RBtnDn001 = new System.Windows.Forms.Button();
            this.RBtnUp001 = new System.Windows.Forms.Button();
            this.RBtnDn01 = new System.Windows.Forms.Button();
            this.RBtnUp01 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.DistortionMetodComBox = new System.Windows.Forms.ComboBox();
            this.DistortionMetodLabel = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.CropBeforeChkBox = new System.Windows.Forms.CheckBox();
            this.RotationChkBox = new System.Windows.Forms.CheckBox();
            this.DistortionChkBox = new System.Windows.Forms.CheckBox();
            this.HeightBeforeTxtBox = new System.Windows.Forms.TextBox();
            this.WidthBeforeTxtBox = new System.Windows.Forms.TextBox();
            this.YBeforeTxtBox = new System.Windows.Forms.TextBox();
            this.XBeforeTxtBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.ApplyBtn = new System.Windows.Forms.Button();
            this.RezultRTB = new System.Windows.Forms.RichTextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.HeightAfterTxtBox = new System.Windows.Forms.TextBox();
            this.WidthAfterTxtBox = new System.Windows.Forms.TextBox();
            this.YAfterTxtBox = new System.Windows.Forms.TextBox();
            this.XAfterTxtBox = new System.Windows.Forms.TextBox();
            this.CropAfterChkBox = new System.Windows.Forms.CheckBox();
            this.label18 = new System.Windows.Forms.Label();
            this.DistZeroBtn = new System.Windows.Forms.Button();
            this.SaveAsBtn = new System.Windows.Forms.Button();
            this.LoadFrBtn = new System.Windows.Forms.Button();
            this.ShowGridСhckBox = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new System.Drawing.Point(7, 31);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(1102, 802);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseDown += pictureBox1_MouseDown;
            this.pictureBox1.MouseUp += pictureBox1_MouseUp;
            // 
            // CorrectFilesBtn
            // 
            this.CorrectFilesBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CorrectFilesBtn.Location = new System.Drawing.Point(1115, 836);
            this.CorrectFilesBtn.Name = "CorrectFilesBtn";
            this.CorrectFilesBtn.Size = new System.Drawing.Size(130, 24);
            this.CorrectFilesBtn.TabIndex = 1;
            this.CorrectFilesBtn.Text = "Correct Files";
            this.CorrectFilesBtn.UseVisualStyleBackColor = true;
            this.CorrectFilesBtn.Click += CorrectFiles_Click;
            // 
            // ABtnUp
            // 
            this.ABtnUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ABtnUp.Location = new System.Drawing.Point(1178, 233);
            this.ABtnUp.Name = "ABtnUp";
            this.ABtnUp.Size = new System.Drawing.Size(15, 19);
            this.ABtnUp.TabIndex = 3;
            this.ABtnUp.Text = "^";
            this.ABtnUp.UseVisualStyleBackColor = true;
            this.ABtnUp.Click += ABtnUp_Click;
            // 
            // ABtnDn
            // 
            this.ABtnDn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ABtnDn.Location = new System.Drawing.Point(1197, 233);
            this.ABtnDn.Name = "ABtnDn";
            this.ABtnDn.Size = new System.Drawing.Size(15, 19);
            this.ABtnDn.TabIndex = 4;
            this.ABtnDn.Text = "v";
            this.ABtnDn.UseVisualStyleBackColor = true;
            this.ABtnDn.Click += ABtnDn_Click;
            // 
            // BBtnDn
            // 
            this.BBtnDn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BBtnDn.Location = new System.Drawing.Point(1197, 257);
            this.BBtnDn.Name = "BBtnDn";
            this.BBtnDn.Size = new System.Drawing.Size(15, 20);
            this.BBtnDn.TabIndex = 7;
            this.BBtnDn.Text = "v";
            this.BBtnDn.UseVisualStyleBackColor = true;
            this.BBtnDn.Click += BBtnDn_Click;
            // 
            // BBtnUp
            // 
            this.BBtnUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BBtnUp.Location = new System.Drawing.Point(1178, 257);
            this.BBtnUp.Name = "BBtnUp";
            this.BBtnUp.Size = new System.Drawing.Size(15, 19);
            this.BBtnUp.TabIndex = 6;
            this.BBtnUp.Text = "^";
            this.BBtnUp.UseVisualStyleBackColor = true;
            this.BBtnUp.Click += BBtnUp_Click;
            // 
            // CBtnDn
            // 
            this.CBtnDn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CBtnDn.Location = new System.Drawing.Point(1197, 282);
            this.CBtnDn.Name = "CBtnDn";
            this.CBtnDn.Size = new System.Drawing.Size(15, 19);
            this.CBtnDn.TabIndex = 10;
            this.CBtnDn.Text = "v";
            this.CBtnDn.UseVisualStyleBackColor = true;
            this.CBtnDn.Click += CBtnDn_Click;
            // 
            // CBtnUp
            // 
            this.CBtnUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CBtnUp.Location = new System.Drawing.Point(1178, 282);
            this.CBtnUp.Name = "CBtnUp";
            this.CBtnUp.Size = new System.Drawing.Size(15, 19);
            this.CBtnUp.TabIndex = 9;
            this.CBtnUp.Text = "^";
            this.CBtnUp.UseVisualStyleBackColor = true;
            this.CBtnUp.Click += CBtnUp_Click;
            // 
            // DBtnDn
            // 
            this.DBtnDn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.DBtnDn.Location = new System.Drawing.Point(1197, 306);
            this.DBtnDn.Name = "DBtnDn";
            this.DBtnDn.Size = new System.Drawing.Size(15, 19);
            this.DBtnDn.TabIndex = 13;
            this.DBtnDn.Text = "v";
            this.DBtnDn.UseVisualStyleBackColor = true;
            this.DBtnDn.Click += DBtnDn_Click;
            // 
            // DBtnUp
            // 
            this.DBtnUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.DBtnUp.Location = new System.Drawing.Point(1178, 306);
            this.DBtnUp.Name = "DBtnUp";
            this.DBtnUp.Size = new System.Drawing.Size(15, 19);
            this.DBtnUp.TabIndex = 12;
            this.DBtnUp.Text = "^";
            this.DBtnUp.UseVisualStyleBackColor = true;
            this.DBtnUp.Click += DBtnUp_Click;
            // 
            // InputDirTxtBox
            // 
            this.InputDirTxtBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.InputDirTxtBox.Location = new System.Drawing.Point(73, 4);
            this.InputDirTxtBox.Name = "InputDirTxtBox";
            this.InputDirTxtBox.Size = new System.Drawing.Size(875, 20);
            this.InputDirTxtBox.TabIndex = 14;
            this.InputDirTxtBox.TextChanged += InputDirTxtBox_TextChanged;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(9, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 15);
            this.label1.TabIndex = 15;
            this.label1.Text = "Input dir:";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 842);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 13);
            this.label2.TabIndex = 17;
            this.label2.Text = "Outpt directory:";
            // 
            // OutputDirTxtBox
            // 
            this.OutputDirTxtBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.OutputDirTxtBox.Location = new System.Drawing.Point(87, 839);
            this.OutputDirTxtBox.Name = "OutputDirTxtBox";
            this.OutputDirTxtBox.Size = new System.Drawing.Size(1022, 20);
            this.OutputDirTxtBox.TabIndex = 16;
            // 
            // InputFileTxtBox
            // 
            this.InputFileTxtBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.InputFileTxtBox.Location = new System.Drawing.Point(989, 5);
            this.InputFileTxtBox.Name = "InputFileTxtBox";
            this.InputFileTxtBox.Size = new System.Drawing.Size(120, 20);
            this.InputFileTxtBox.TabIndex = 18;
            this.InputFileTxtBox.TextChanged += InputFileTxtBox_TextChanged;
            // 
            // ATxtBox
            // 
            this.ATxtBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ATxtBox.Location = new System.Drawing.Point(1116, 232);
            this.ATxtBox.Name = "ATxtBox";
            this.ATxtBox.Size = new System.Drawing.Size(55, 20);
            this.ATxtBox.TabIndex = 19;
            this.ATxtBox.TextChanged += ATxtBox_TextChanged;
            // 
            // BTxtBox
            // 
            this.BTxtBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BTxtBox.Location = new System.Drawing.Point(1116, 257);
            this.BTxtBox.Name = "BTxtBox";
            this.BTxtBox.Size = new System.Drawing.Size(55, 20);
            this.BTxtBox.TabIndex = 20;
            this.BTxtBox.TextChanged += BTxtBox_TextChanged;
            // 
            // CTxtBox
            // 
            this.CTxtBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CTxtBox.Location = new System.Drawing.Point(1116, 281);
            this.CTxtBox.Name = "CTxtBox";
            this.CTxtBox.Size = new System.Drawing.Size(55, 20);
            this.CTxtBox.TabIndex = 21;
            this.CTxtBox.TextChanged += CTxtBox_TextChanged;
            // 
            // DTxtBox
            // 
            this.DTxtBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.DTxtBox.Location = new System.Drawing.Point(1116, 305);
            this.DTxtBox.Name = "DTxtBox";
            this.DTxtBox.Size = new System.Drawing.Size(55, 20);
            this.DTxtBox.TabIndex = 22;
            this.DTxtBox.TextChanged += DTxtBox_TextChanged;
            // 
            // RotValTxtBox
            // 
            this.RotValTxtBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.RotValTxtBox.Location = new System.Drawing.Point(1119, 153);
            this.RotValTxtBox.Name = "RotValTxtBox";
            this.RotValTxtBox.Size = new System.Drawing.Size(55, 20);
            this.RotValTxtBox.TabIndex = 25;
            this.RotValTxtBox.TextChanged += RotValTxtBox_TextChanged;
            // 
            // RBtnDn001
            // 
            this.RBtnDn001.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.RBtnDn001.Location = new System.Drawing.Point(1197, 153);
            this.RBtnDn001.Name = "RBtnDn001";
            this.RBtnDn001.Size = new System.Drawing.Size(13, 20);
            this.RBtnDn001.TabIndex = 24;
            this.RBtnDn001.Text = "v";
            this.RBtnDn001.UseVisualStyleBackColor = true;
            this.RBtnDn001.Click += RBtnDn001_Click;
            // 
            // RBtnUp001
            // 
            this.RBtnUp001.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.RBtnUp001.Location = new System.Drawing.Point(1179, 153);
            this.RBtnUp001.Name = "RBtnUp001";
            this.RBtnUp001.Size = new System.Drawing.Size(13, 20);
            this.RBtnUp001.TabIndex = 23;
            this.RBtnUp001.Text = "^";
            this.RBtnUp001.UseVisualStyleBackColor = true;
            this.RBtnUp001.Click += RBtnUp001_Click;
            // 
            // RBtnDn01
            // 
            this.RBtnDn01.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.RBtnDn01.Location = new System.Drawing.Point(1231, 153);
            this.RBtnDn01.Name = "RBtnDn01";
            this.RBtnDn01.Size = new System.Drawing.Size(13, 20);
            this.RBtnDn01.TabIndex = 27;
            this.RBtnDn01.Text = "v";
            this.RBtnDn01.UseVisualStyleBackColor = true;
            this.RBtnDn01.Click += RBtnDn01_Click;
            // 
            // RBtnUp01
            // 
            this.RBtnUp01.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.RBtnUp01.Location = new System.Drawing.Point(1214, 153);
            this.RBtnUp01.Name = "RBtnUp01";
            this.RBtnUp01.Size = new System.Drawing.Size(13, 20);
            this.RBtnUp01.TabIndex = 26;
            this.RBtnUp01.Text = "^";
            this.RBtnUp01.UseVisualStyleBackColor = true;
            this.RBtnUp01.Click += RBtnUp01_Click;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.label3.Location = new System.Drawing.Point(1142, 132);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 19);
            this.label3.TabIndex = 28;
            this.label3.Text = "Rotation";
            this.label3.Click += label3_Click;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.label4.Location = new System.Drawing.Point(953, 6);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(36, 19);
            this.label4.TabIndex = 29;
            this.label4.Text = "File:";
            // 
            // DistortionMetodComBox
            // 
            this.DistortionMetodComBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.DistortionMetodComBox.FormattingEnabled = true;
            this.DistortionMetodComBox.Location = new System.Drawing.Point(1116, 207);
            this.DistortionMetodComBox.Name = "DistortionMetodComBox";
            this.DistortionMetodComBox.Size = new System.Drawing.Size(125, 21);
            this.DistortionMetodComBox.TabIndex = 30;
            this.DistortionMetodComBox.SelectedIndexChanged += DistortionMetodComBox_SelectedIndexChanged;
            // 
            // DistortionMetodLabel
            // 
            this.DistortionMetodLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.DistortionMetodLabel.AutoSize = true;
            this.DistortionMetodLabel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.DistortionMetodLabel.Location = new System.Drawing.Point(1111, 186);
            this.DistortionMetodLabel.Name = "DistortionMetodLabel";
            this.DistortionMetodLabel.Size = new System.Drawing.Size(123, 19);
            this.DistortionMetodLabel.TabIndex = 31;
            this.DistortionMetodLabel.Text = "Distortion metod";
            this.DistortionMetodLabel.Click += DistortionMetodLabel_Click;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.label5.Location = new System.Drawing.Point(1124, 7);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(91, 19);
            this.label5.TabIndex = 33;
            this.label5.Text = "Crop Before";
            this.label5.Click += label5_Click;
            // 
            // CropBeforeChkBox
            // 
            this.CropBeforeChkBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CropBeforeChkBox.AutoSize = true;
            this.CropBeforeChkBox.Checked = true;
            this.CropBeforeChkBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CropBeforeChkBox.Location = new System.Drawing.Point(1214, 10);
            this.CropBeforeChkBox.Name = "CropBeforeChkBox";
            this.CropBeforeChkBox.Size = new System.Drawing.Size(15, 14);
            this.CropBeforeChkBox.TabIndex = 34;
            this.CropBeforeChkBox.UseVisualStyleBackColor = true;
            // 
            // RotationChkBox
            // 
            this.RotationChkBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.RotationChkBox.AutoSize = true;
            this.RotationChkBox.Location = new System.Drawing.Point(1208, 135);
            this.RotationChkBox.Name = "RotationChkBox";
            this.RotationChkBox.Size = new System.Drawing.Size(15, 14);
            this.RotationChkBox.TabIndex = 35;
            this.RotationChkBox.UseVisualStyleBackColor = true;
            // 
            // DistortionChkBox
            // 
            this.DistortionChkBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.DistortionChkBox.AutoSize = true;
            this.DistortionChkBox.Location = new System.Drawing.Point(1232, 190);
            this.DistortionChkBox.Name = "DistortionChkBox";
            this.DistortionChkBox.Size = new System.Drawing.Size(15, 14);
            this.DistortionChkBox.TabIndex = 36;
            this.DistortionChkBox.UseVisualStyleBackColor = true;
            // 
            // HeightBeforeTxtBox
            // 
            this.HeightBeforeTxtBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.HeightBeforeTxtBox.Location = new System.Drawing.Point(1168, 100);
            this.HeightBeforeTxtBox.Name = "HeightBeforeTxtBox";
            this.HeightBeforeTxtBox.Size = new System.Drawing.Size(55, 20);
            this.HeightBeforeTxtBox.TabIndex = 40;
            this.HeightBeforeTxtBox.Text = "100";
            // 
            // WidthBeforeTxtBox
            // 
            this.WidthBeforeTxtBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.WidthBeforeTxtBox.Location = new System.Drawing.Point(1168, 75);
            this.WidthBeforeTxtBox.Name = "WidthBeforeTxtBox";
            this.WidthBeforeTxtBox.Size = new System.Drawing.Size(55, 20);
            this.WidthBeforeTxtBox.TabIndex = 39;
            this.WidthBeforeTxtBox.Text = "100";
            // 
            // YBeforeTxtBox
            // 
            this.YBeforeTxtBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.YBeforeTxtBox.Location = new System.Drawing.Point(1168, 51);
            this.YBeforeTxtBox.Name = "YBeforeTxtBox";
            this.YBeforeTxtBox.Size = new System.Drawing.Size(55, 20);
            this.YBeforeTxtBox.TabIndex = 38;
            this.YBeforeTxtBox.Text = "0";
            // 
            // XBeforeTxtBox
            // 
            this.XBeforeTxtBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.XBeforeTxtBox.Location = new System.Drawing.Point(1168, 27);
            this.XBeforeTxtBox.Name = "XBeforeTxtBox";
            this.XBeforeTxtBox.Size = new System.Drawing.Size(55, 20);
            this.XBeforeTxtBox.TabIndex = 37;
            this.XBeforeTxtBox.Text = "0";
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.label6.Location = new System.Drawing.Point(1130, 27);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(18, 19);
            this.label6.TabIndex = 41;
            this.label6.Text = "X";
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.label7.Location = new System.Drawing.Point(1130, 52);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(18, 19);
            this.label7.TabIndex = 42;
            this.label7.Text = "Y";
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.label8.Location = new System.Drawing.Point(1116, 77);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(49, 19);
            this.label8.TabIndex = 43;
            this.label8.Text = "Width";
            // 
            // label10
            // 
            this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.label10.Location = new System.Drawing.Point(1112, 101);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(54, 19);
            this.label10.TabIndex = 45;
            this.label10.Text = "Height";
            // 
            // label11
            // 
            this.label11.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.label11.Location = new System.Drawing.Point(1225, 101);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(21, 19);
            this.label11.TabIndex = 46;
            this.label11.Text = "%";
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.label9.Location = new System.Drawing.Point(1224, 77);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(21, 19);
            this.label9.TabIndex = 47;
            this.label9.Text = "%";
            // 
            // ApplyBtn
            // 
            this.ApplyBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ApplyBtn.Location = new System.Drawing.Point(1115, 484);
            this.ApplyBtn.Name = "ApplyBtn";
            this.ApplyBtn.Size = new System.Drawing.Size(127, 36);
            this.ApplyBtn.TabIndex = 48;
            this.ApplyBtn.Text = "Apply";
            this.ApplyBtn.UseVisualStyleBackColor = true;
            this.ApplyBtn.Click += ApplyBtn_Click;
            // 
            // RezultRTB
            // 
            this.RezultRTB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.RezultRTB.Location = new System.Drawing.Point(1115, 582);
            this.RezultRTB.Name = "RezultRTB";
            this.RezultRTB.Size = new System.Drawing.Size(127, 248);
            this.RezultRTB.TabIndex = 49;
            this.RezultRTB.Text = "";
            // 
            // label12
            // 
            this.label12.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.label12.Location = new System.Drawing.Point(1226, 407);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(21, 19);
            this.label12.TabIndex = 61;
            this.label12.Text = "%";
            // 
            // label13
            // 
            this.label13.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.label13.Location = new System.Drawing.Point(1227, 432);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(21, 19);
            this.label13.TabIndex = 60;
            this.label13.Text = "%";
            // 
            // label14
            // 
            this.label14.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.label14.Location = new System.Drawing.Point(1115, 432);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(54, 19);
            this.label14.TabIndex = 59;
            this.label14.Text = "Height";
            // 
            // label15
            // 
            this.label15.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.label15.Location = new System.Drawing.Point(1117, 407);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(49, 19);
            this.label15.TabIndex = 58;
            this.label15.Text = "Width";
            // 
            // label16
            // 
            this.label16.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.label16.Location = new System.Drawing.Point(1130, 382);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(18, 19);
            this.label16.TabIndex = 57;
            this.label16.Text = "Y";
            // 
            // label17
            // 
            this.label17.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.label17.Location = new System.Drawing.Point(1130, 357);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(18, 19);
            this.label17.TabIndex = 56;
            this.label17.Text = "X";
            // 
            // HeightAfterTxtBox
            // 
            this.HeightAfterTxtBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.HeightAfterTxtBox.Location = new System.Drawing.Point(1169, 430);
            this.HeightAfterTxtBox.Name = "HeightAfterTxtBox";
            this.HeightAfterTxtBox.Size = new System.Drawing.Size(55, 20);
            this.HeightAfterTxtBox.TabIndex = 55;
            this.HeightAfterTxtBox.Text = "100";
            // 
            // WidthAfterTxtBox
            // 
            this.WidthAfterTxtBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.WidthAfterTxtBox.Location = new System.Drawing.Point(1169, 406);
            this.WidthAfterTxtBox.Name = "WidthAfterTxtBox";
            this.WidthAfterTxtBox.Size = new System.Drawing.Size(55, 20);
            this.WidthAfterTxtBox.TabIndex = 54;
            this.WidthAfterTxtBox.Text = "100";
            // 
            // YAfterTxtBox
            // 
            this.YAfterTxtBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.YAfterTxtBox.Location = new System.Drawing.Point(1169, 381);
            this.YAfterTxtBox.Name = "YAfterTxtBox";
            this.YAfterTxtBox.Size = new System.Drawing.Size(55, 20);
            this.YAfterTxtBox.TabIndex = 53;
            this.YAfterTxtBox.Text = "0";
            // 
            // XAfterTxtBox
            // 
            this.XAfterTxtBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.XAfterTxtBox.Location = new System.Drawing.Point(1169, 357);
            this.XAfterTxtBox.Name = "XAfterTxtBox";
            this.XAfterTxtBox.Size = new System.Drawing.Size(55, 20);
            this.XAfterTxtBox.TabIndex = 52;
            this.XAfterTxtBox.Text = "0";
            // 
            // CropAfterChkBox
            // 
            this.CropAfterChkBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CropAfterChkBox.AutoSize = true;
            this.CropAfterChkBox.Checked = true;
            this.CropAfterChkBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CropAfterChkBox.Location = new System.Drawing.Point(1210, 338);
            this.CropAfterChkBox.Name = "CropAfterChkBox";
            this.CropAfterChkBox.Size = new System.Drawing.Size(15, 14);
            this.CropAfterChkBox.TabIndex = 51;
            this.CropAfterChkBox.UseVisualStyleBackColor = true;
            // 
            // label18
            // 
            this.label18.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.label18.Location = new System.Drawing.Point(1120, 335);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(80, 19);
            this.label18.TabIndex = 50;
            this.label18.Text = "Crop After";
            // 
            // DistZeroBtn
            // 
            this.DistZeroBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.DistZeroBtn.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.DistZeroBtn.Location = new System.Drawing.Point(1221, 231);
            this.DistZeroBtn.Name = "DistZeroBtn";
            this.DistZeroBtn.Size = new System.Drawing.Size(21, 93);
            this.DistZeroBtn.TabIndex = 62;
            this.DistZeroBtn.Text = "zero";
            this.DistZeroBtn.UseVisualStyleBackColor = true;
            this.DistZeroBtn.Click += DistZeroBtn_Click;
            // 
            // SaveAsBtn
            // 
            this.SaveAsBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SaveAsBtn.Location = new System.Drawing.Point(1115, 526);
            this.SaveAsBtn.Name = "SaveAsBtn";
            this.SaveAsBtn.Size = new System.Drawing.Size(53, 50);
            this.SaveAsBtn.TabIndex = 65;
            this.SaveAsBtn.Text = "Save";
            this.SaveAsBtn.UseVisualStyleBackColor = true;
            this.SaveAsBtn.Click += new System.EventHandler(this.SaveAsBtn_Click_1);
            // 
            // LoadFrBtn
            // 
            this.LoadFrBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.LoadFrBtn.Location = new System.Drawing.Point(1189, 526);
            this.LoadFrBtn.Name = "LoadFrBtn";
            this.LoadFrBtn.Size = new System.Drawing.Size(53, 50);
            this.LoadFrBtn.TabIndex = 66;
            this.LoadFrBtn.Text = "Load";
            this.LoadFrBtn.UseVisualStyleBackColor = true;
            this.LoadFrBtn.Click += LoadFrBtn_Click;
            // 
            // ShowGridСhckBox
            // 
            this.ShowGridСhckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ShowGridСhckBox.AutoSize = true;
            this.ShowGridСhckBox.Location = new System.Drawing.Point(1124, 460);
            this.ShowGridСhckBox.Name = "ShowGridСhckBox";
            this.ShowGridСhckBox.Size = new System.Drawing.Size(75, 17);
            this.ShowGridСhckBox.TabIndex = 67;
            this.ShowGridСhckBox.Text = "Show Grid";
            this.ShowGridСhckBox.UseVisualStyleBackColor = true;
            this.ShowGridСhckBox.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // ImgFixingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1251, 869);
            this.Controls.Add(this.ShowGridСhckBox);
            this.Controls.Add(this.LoadFrBtn);
            this.Controls.Add(this.SaveAsBtn);
            this.Controls.Add(this.DistZeroBtn);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.HeightAfterTxtBox);
            this.Controls.Add(this.WidthAfterTxtBox);
            this.Controls.Add(this.YAfterTxtBox);
            this.Controls.Add(this.XAfterTxtBox);
            this.Controls.Add(this.CropAfterChkBox);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.RezultRTB);
            this.Controls.Add(this.ApplyBtn);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.HeightBeforeTxtBox);
            this.Controls.Add(this.WidthBeforeTxtBox);
            this.Controls.Add(this.YBeforeTxtBox);
            this.Controls.Add(this.XBeforeTxtBox);
            this.Controls.Add(this.DistortionChkBox);
            this.Controls.Add(this.RotationChkBox);
            this.Controls.Add(this.CropBeforeChkBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.DistortionMetodLabel);
            this.Controls.Add(this.DistortionMetodComBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.RBtnDn01);
            this.Controls.Add(this.RBtnUp01);
            this.Controls.Add(this.RotValTxtBox);
            this.Controls.Add(this.RBtnDn001);
            this.Controls.Add(this.RBtnUp001);
            this.Controls.Add(this.DTxtBox);
            this.Controls.Add(this.CTxtBox);
            this.Controls.Add(this.BTxtBox);
            this.Controls.Add(this.ATxtBox);
            this.Controls.Add(this.InputFileTxtBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.OutputDirTxtBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.InputDirTxtBox);
            this.Controls.Add(this.DBtnDn);
            this.Controls.Add(this.DBtnUp);
            this.Controls.Add(this.CBtnDn);
            this.Controls.Add(this.CBtnUp);
            this.Controls.Add(this.BBtnDn);
            this.Controls.Add(this.BBtnUp);
            this.Controls.Add(this.ABtnDn);
            this.Controls.Add(this.ABtnUp);
            this.Controls.Add(this.CorrectFilesBtn);
            this.Controls.Add(this.pictureBox1);
            this.MinimumSize = new System.Drawing.Size(1117, 905);
            this.Name = "ImgFixingForm";
            this.Text = "DistortionTest";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private PictureBox pictureBox1;
        private Button CorrectFilesBtn;
        private Button ABtnUp;
        private Button ABtnDn;
        private Button BBtnDn;
        private Button BBtnUp;
        private Button CBtnDn;
        private Button CBtnUp;
        private Button DBtnDn;
        private Button DBtnUp;
        private TextBox InputDirTxtBox;
        private Label label1;
        private Label label2;
        private TextBox OutputDirTxtBox;
        private TextBox InputFileTxtBox;
        private TextBox ATxtBox;
        private TextBox BTxtBox;
        private TextBox CTxtBox;
        private TextBox DTxtBox;
        private TextBox RotValTxtBox;
        private Button RBtnDn001;
        private Button RBtnUp001;
        private Button RBtnDn01;
        private Button RBtnUp01;
        private Label label3;
        private Label label4;
        private ComboBox DistortionMetodComBox;
        private Label DistortionMetodLabel;
        private Label label5;
        private CheckBox CropBeforeChkBox;
        private CheckBox RotationChkBox;
        private CheckBox DistortionChkBox;
        private TextBox HeightBeforeTxtBox;
        private TextBox WidthBeforeTxtBox;
        private TextBox YBeforeTxtBox;
        private TextBox XBeforeTxtBox;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label label10;
        private Label label11;
        private Label label9;
        private Button ApplyBtn;
        private RichTextBox RezultRTB;
        private Label label12;
        private Label label13;
        private Label label14;
        private Label label15;
        private Label label16;
        private Label label17;
        private TextBox HeightAfterTxtBox;
        private TextBox WidthAfterTxtBox;
        private TextBox YAfterTxtBox;
        private TextBox XAfterTxtBox;
        private CheckBox CropAfterChkBox;
        private Label label18;
        private Button DistZeroBtn;
        private Button SaveAsBtn;
        private Button LoadFrBtn;
        private CheckBox ShowGridСhckBox;
    }
}