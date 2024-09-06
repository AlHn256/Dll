using System.Drawing;
using System.Windows.Forms;

namespace NewImgFixingLib
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
            this.DistortionMetodLabel = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.CropBeforeChkBox = new System.Windows.Forms.CheckBox();
            this.RotationChkBox = new System.Windows.Forms.CheckBox();
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
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.dYAfterTxtBox = new System.Windows.Forms.TextBox();
            this.dXAfterTxtBox = new System.Windows.Forms.TextBox();
            this.YAfterTxtBox = new System.Windows.Forms.TextBox();
            this.XAfterTxtBox = new System.Windows.Forms.TextBox();
            this.CropAfterChkBox = new System.Windows.Forms.CheckBox();
            this.label18 = new System.Windows.Forms.Label();
            this.DistZeroBtn = new System.Windows.Forms.Button();
            this.SaveAsBtn = new System.Windows.Forms.Button();
            this.LoadFrBtn = new System.Windows.Forms.Button();
            this.ShowGridСhckBox = new System.Windows.Forms.CheckBox();
            this.ETxtBox = new System.Windows.Forms.TextBox();
            this.EBtnDn = new System.Windows.Forms.Button();
            this.EBtnUp = new System.Windows.Forms.Button();
            this.AutoReloadChkBox = new System.Windows.Forms.CheckBox();
            this.label19 = new System.Windows.Forms.Label();
            this.Sm11TxtBox = new System.Windows.Forms.TextBox();
            this.Sm12TxtBox = new System.Windows.Forms.TextBox();
            this.Sm13TxtBox = new System.Windows.Forms.TextBox();
            this.Sm23TxtBox = new System.Windows.Forms.TextBox();
            this.Sm22TxtBox = new System.Windows.Forms.TextBox();
            this.Sm21TxtBox = new System.Windows.Forms.TextBox();
            this.Sm33TxtBox = new System.Windows.Forms.TextBox();
            this.Sm32TxtBox = new System.Windows.Forms.TextBox();
            this.Sm31TxtBox = new System.Windows.Forms.TextBox();
            this.ZeroCropAfterBtn = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.DistChkBox = new System.Windows.Forms.CheckBox();
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
            this.pictureBox1.Location = new System.Drawing.Point(7, 30);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(1100, 802);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // CorrectFilesBtn
            // 
            this.CorrectFilesBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CorrectFilesBtn.Location = new System.Drawing.Point(1115, 836);
            this.CorrectFilesBtn.Name = "CorrectFilesBtn";
            this.CorrectFilesBtn.Size = new System.Drawing.Size(120, 24);
            this.CorrectFilesBtn.TabIndex = 1;
            this.CorrectFilesBtn.Text = "Correct Files";
            this.CorrectFilesBtn.UseVisualStyleBackColor = true;
            this.CorrectFilesBtn.Click += new System.EventHandler(this.CorrectFiles_Click);

            // 
            // ABtnUp
            // 
            this.ABtnUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ABtnUp.Location = new System.Drawing.Point(1173, 233);
            this.ABtnUp.Name = "ABtnUp";
            this.ABtnUp.Size = new System.Drawing.Size(13, 16);
            this.ABtnUp.TabIndex = 3;
            this.ABtnUp.Text = "^";
            this.ABtnUp.UseVisualStyleBackColor = true;
            this.ABtnUp.Click += new System.EventHandler(this.ABtnUp_Click);
            // 
            // ABtnDn
            // 
            this.ABtnDn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ABtnDn.Location = new System.Drawing.Point(1192, 233);
            this.ABtnDn.Name = "ABtnDn";
            this.ABtnDn.Size = new System.Drawing.Size(13, 16);
            this.ABtnDn.TabIndex = 4;
            this.ABtnDn.Text = "v";
            this.ABtnDn.UseVisualStyleBackColor = true;
            this.ABtnDn.Click += new System.EventHandler(this.ABtnDn_Click);
            // 
            // BBtnDn
            // 
            this.BBtnDn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BBtnDn.Location = new System.Drawing.Point(1192, 257);
            this.BBtnDn.Name = "BBtnDn";
            this.BBtnDn.Size = new System.Drawing.Size(13, 16);
            this.BBtnDn.TabIndex = 7;
            this.BBtnDn.Text = "v";
            this.BBtnDn.UseVisualStyleBackColor = true;
            this.BBtnDn.Click += new System.EventHandler(this.BBtnDn_Click);
            // 
            // BBtnUp
            // 
            this.BBtnUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BBtnUp.Location = new System.Drawing.Point(1173, 257);
            this.BBtnUp.Name = "BBtnUp";
            this.BBtnUp.Size = new System.Drawing.Size(13, 16);
            this.BBtnUp.TabIndex = 6;
            this.BBtnUp.Text = "^";
            this.BBtnUp.UseVisualStyleBackColor = true;
            this.BBtnUp.Click += new System.EventHandler(this.BBtnUp_Click);
            // 
            // CBtnDn
            // 
            this.CBtnDn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CBtnDn.Location = new System.Drawing.Point(1192, 282);
            this.CBtnDn.Name = "CBtnDn";
            this.CBtnDn.Size = new System.Drawing.Size(13, 16);
            this.CBtnDn.TabIndex = 10;
            this.CBtnDn.Text = "v";
            this.CBtnDn.UseVisualStyleBackColor = true;
            this.CBtnDn.Click += new System.EventHandler(this.CBtnDn_Click);
            // 
            // CBtnUp
            // 
            this.CBtnUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CBtnUp.Location = new System.Drawing.Point(1173, 282);
            this.CBtnUp.Name = "CBtnUp";
            this.CBtnUp.Size = new System.Drawing.Size(13, 16);
            this.CBtnUp.TabIndex = 9;
            this.CBtnUp.Text = "^";
            this.CBtnUp.UseVisualStyleBackColor = true;
            this.CBtnUp.Click += new System.EventHandler(this.CBtnUp_Click);
            // 
            // DBtnDn
            // 
            this.DBtnDn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.DBtnDn.Location = new System.Drawing.Point(1192, 306);
            this.DBtnDn.Name = "DBtnDn";
            this.DBtnDn.Size = new System.Drawing.Size(13, 16);
            this.DBtnDn.TabIndex = 13;
            this.DBtnDn.Text = "v";
            this.DBtnDn.UseVisualStyleBackColor = true;
            this.DBtnDn.Click += new System.EventHandler(this.DBtnDn_Click);
            // 
            // DBtnUp
            // 
            this.DBtnUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.DBtnUp.Location = new System.Drawing.Point(1173, 306);
            this.DBtnUp.Name = "DBtnUp";
            this.DBtnUp.Size = new System.Drawing.Size(13, 16);
            this.DBtnUp.TabIndex = 12;
            this.DBtnUp.Text = "^";
            this.DBtnUp.UseVisualStyleBackColor = true;
            this.DBtnUp.Click += new System.EventHandler(this.DBtnUp_Click);
            // 
            // InputDirTxtBox
            // 
            this.InputDirTxtBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.InputDirTxtBox.Location = new System.Drawing.Point(73, 4);
            this.InputDirTxtBox.Name = "InputDirTxtBox";
            this.InputDirTxtBox.Size = new System.Drawing.Size(867, 20);
            this.InputDirTxtBox.TabIndex = 14;
            this.InputDirTxtBox.TextChanged += new System.EventHandler(this.InputDirTxtBox_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(9, 6);
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
            this.OutputDirTxtBox.Size = new System.Drawing.Size(1020, 20);
            this.OutputDirTxtBox.TabIndex = 16;
            // 
            // InputFileTxtBox
            // 
            this.InputFileTxtBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.InputFileTxtBox.Location = new System.Drawing.Point(981, 5);
            this.InputFileTxtBox.Name = "InputFileTxtBox";
            this.InputFileTxtBox.Size = new System.Drawing.Size(126, 20);
            this.InputFileTxtBox.TabIndex = 18;
            this.InputFileTxtBox.TextChanged += new System.EventHandler(this.InputFileTxtBox_TextChanged);
            // 
            // ATxtBox
            // 
            this.ATxtBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ATxtBox.Location = new System.Drawing.Point(1113, 232);
            this.ATxtBox.Name = "ATxtBox";
            this.ATxtBox.Size = new System.Drawing.Size(55, 20);
            this.ATxtBox.TabIndex = 19;
            this.ATxtBox.TextChanged += new System.EventHandler(this.ATxtBox_TextChanged);
            // 
            // BTxtBox
            // 
            this.BTxtBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BTxtBox.Location = new System.Drawing.Point(1113, 257);
            this.BTxtBox.Name = "BTxtBox";
            this.BTxtBox.Size = new System.Drawing.Size(55, 20);
            this.BTxtBox.TabIndex = 20;
            this.BTxtBox.TextChanged += new System.EventHandler(this.BTxtBox_TextChanged);
            // 
            // CTxtBox
            // 
            this.CTxtBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CTxtBox.Location = new System.Drawing.Point(1113, 281);
            this.CTxtBox.Name = "CTxtBox";
            this.CTxtBox.Size = new System.Drawing.Size(55, 20);
            this.CTxtBox.TabIndex = 21;
            this.CTxtBox.TextChanged += new System.EventHandler(this.CTxtBox_TextChanged);
            // 
            // DTxtBox
            // 
            this.DTxtBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.DTxtBox.Location = new System.Drawing.Point(1113, 305);
            this.DTxtBox.Name = "DTxtBox";
            this.DTxtBox.Size = new System.Drawing.Size(55, 20);
            this.DTxtBox.TabIndex = 22;
            this.DTxtBox.TextChanged += new System.EventHandler(this.DTxtBox_TextChanged);
            // 
            // RotValTxtBox
            // 
            this.RotValTxtBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.RotValTxtBox.Location = new System.Drawing.Point(1111, 153);
            this.RotValTxtBox.Name = "RotValTxtBox";
            this.RotValTxtBox.Size = new System.Drawing.Size(55, 20);
            this.RotValTxtBox.TabIndex = 25;
            // 
            // RBtnDn001
            // 
            this.RBtnDn001.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.RBtnDn001.Location = new System.Drawing.Point(1189, 153);
            this.RBtnDn001.Name = "RBtnDn001";
            this.RBtnDn001.Size = new System.Drawing.Size(13, 20);
            this.RBtnDn001.TabIndex = 24;
            this.RBtnDn001.Text = "v";
            this.RBtnDn001.UseVisualStyleBackColor = true;
            // 
            // RBtnUp001
            // 
            this.RBtnUp001.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.RBtnUp001.Location = new System.Drawing.Point(1171, 153);
            this.RBtnUp001.Name = "RBtnUp001";
            this.RBtnUp001.Size = new System.Drawing.Size(13, 20);
            this.RBtnUp001.TabIndex = 23;
            this.RBtnUp001.Text = "^";
            this.RBtnUp001.UseVisualStyleBackColor = true;
            // 
            // RBtnDn01
            // 
            this.RBtnDn01.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.RBtnDn01.Location = new System.Drawing.Point(1223, 153);
            this.RBtnDn01.Name = "RBtnDn01";
            this.RBtnDn01.Size = new System.Drawing.Size(13, 20);
            this.RBtnDn01.TabIndex = 27;
            this.RBtnDn01.Text = "v";
            this.RBtnDn01.UseVisualStyleBackColor = true;
            // 
            // RBtnUp01
            // 
            this.RBtnUp01.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.RBtnUp01.Location = new System.Drawing.Point(1206, 153);
            this.RBtnUp01.Name = "RBtnUp01";
            this.RBtnUp01.Size = new System.Drawing.Size(13, 20);
            this.RBtnUp01.TabIndex = 26;
            this.RBtnUp01.Text = "^";
            this.RBtnUp01.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.label3.Location = new System.Drawing.Point(1130, 131);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 19);
            this.label3.TabIndex = 28;
            this.label3.Text = "Rotation";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.label4.Location = new System.Drawing.Point(945, 4);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(36, 19);
            this.label4.TabIndex = 29;
            this.label4.Text = "File:";
            // 
            // DistortionMetodLabel
            // 
            this.DistortionMetodLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.DistortionMetodLabel.AutoSize = true;
            this.DistortionMetodLabel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.DistortionMetodLabel.Location = new System.Drawing.Point(1107, 186);
            this.DistortionMetodLabel.Name = "DistortionMetodLabel";
            this.DistortionMetodLabel.Size = new System.Drawing.Size(123, 19);
            this.DistortionMetodLabel.TabIndex = 31;
            this.DistortionMetodLabel.Text = "Distortion metod";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.label5.Location = new System.Drawing.Point(1123, 7);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(91, 19);
            this.label5.TabIndex = 33;
            this.label5.Text = "Crop Before";
            // 
            // CropBeforeChkBox
            // 
            this.CropBeforeChkBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CropBeforeChkBox.AutoSize = true;
            this.CropBeforeChkBox.Checked = true;
            this.CropBeforeChkBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CropBeforeChkBox.Location = new System.Drawing.Point(1214, 11);
            this.CropBeforeChkBox.Name = "CropBeforeChkBox";
            this.CropBeforeChkBox.Size = new System.Drawing.Size(15, 14);
            this.CropBeforeChkBox.TabIndex = 34;
            this.CropBeforeChkBox.UseVisualStyleBackColor = true;
            // 
            // RotationChkBox
            // 
            this.RotationChkBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.RotationChkBox.AutoSize = true;
            this.RotationChkBox.Location = new System.Drawing.Point(1195, 135);
            this.RotationChkBox.Name = "RotationChkBox";
            this.RotationChkBox.Size = new System.Drawing.Size(15, 14);
            this.RotationChkBox.TabIndex = 35;
            this.RotationChkBox.UseVisualStyleBackColor = true;
            // 
            // HeightBeforeTxtBox
            // 
            this.HeightBeforeTxtBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.HeightBeforeTxtBox.Location = new System.Drawing.Point(1159, 100);
            this.HeightBeforeTxtBox.Name = "HeightBeforeTxtBox";
            this.HeightBeforeTxtBox.Size = new System.Drawing.Size(55, 20);
            this.HeightBeforeTxtBox.TabIndex = 40;
            this.HeightBeforeTxtBox.Text = "100";
            // 
            // WidthBeforeTxtBox
            // 
            this.WidthBeforeTxtBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.WidthBeforeTxtBox.Location = new System.Drawing.Point(1159, 75);
            this.WidthBeforeTxtBox.Name = "WidthBeforeTxtBox";
            this.WidthBeforeTxtBox.Size = new System.Drawing.Size(55, 20);
            this.WidthBeforeTxtBox.TabIndex = 39;
            this.WidthBeforeTxtBox.Text = "100";
            // 
            // YBeforeTxtBox
            // 
            this.YBeforeTxtBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.YBeforeTxtBox.Location = new System.Drawing.Point(1159, 51);
            this.YBeforeTxtBox.Name = "YBeforeTxtBox";
            this.YBeforeTxtBox.Size = new System.Drawing.Size(55, 20);
            this.YBeforeTxtBox.TabIndex = 38;
            this.YBeforeTxtBox.Text = "0";
            // 
            // XBeforeTxtBox
            // 
            this.XBeforeTxtBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.XBeforeTxtBox.Location = new System.Drawing.Point(1159, 27);
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
            this.label6.Location = new System.Drawing.Point(1129, 27);
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
            this.label7.Location = new System.Drawing.Point(1129, 52);
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
            this.label8.Location = new System.Drawing.Point(1115, 77);
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
            this.label10.Location = new System.Drawing.Point(1111, 101);
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
            this.label11.Location = new System.Drawing.Point(1216, 101);
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
            this.label9.Location = new System.Drawing.Point(1215, 77);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(21, 19);
            this.label9.TabIndex = 47;
            this.label9.Text = "%";
            // 
            // ApplyBtn
            // 
            this.ApplyBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ApplyBtn.Location = new System.Drawing.Point(1111, 665);
            this.ApplyBtn.Name = "ApplyBtn";
            this.ApplyBtn.Size = new System.Drawing.Size(123, 36);
            this.ApplyBtn.TabIndex = 48;
            this.ApplyBtn.Text = "Apply";
            this.ApplyBtn.UseVisualStyleBackColor = true;
            this.ApplyBtn.Click += new System.EventHandler(this.ApplyBtn_Click_1);
            // 
            // RezultRTB
            // 
            this.RezultRTB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.RezultRTB.Location = new System.Drawing.Point(1113, 762);
            this.RezultRTB.Name = "RezultRTB";
            this.RezultRTB.Size = new System.Drawing.Size(121, 68);
            this.RezultRTB.TabIndex = 49;
            this.RezultRTB.Text = "";
            // 
            // label14
            // 
            this.label14.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.label14.Location = new System.Drawing.Point(1120, 563);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(27, 19);
            this.label14.TabIndex = 59;
            this.label14.Text = "dY";
            // 
            // label15
            // 
            this.label15.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.label15.Location = new System.Drawing.Point(1120, 511);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(27, 19);
            this.label15.TabIndex = 58;
            this.label15.Text = "dX";
            // 
            // label16
            // 
            this.label16.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.label16.Location = new System.Drawing.Point(1129, 537);
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
            this.label17.Location = new System.Drawing.Point(1129, 484);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(18, 19);
            this.label17.TabIndex = 56;
            this.label17.Text = "X";
            // 
            // dYAfterTxtBox
            // 
            this.dYAfterTxtBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dYAfterTxtBox.Location = new System.Drawing.Point(1155, 562);
            this.dYAfterTxtBox.Name = "dYAfterTxtBox";
            this.dYAfterTxtBox.Size = new System.Drawing.Size(47, 20);
            this.dYAfterTxtBox.TabIndex = 55;
            this.dYAfterTxtBox.Text = "0";
            // 
            // dXAfterTxtBox
            // 
            this.dXAfterTxtBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dXAfterTxtBox.Location = new System.Drawing.Point(1155, 510);
            this.dXAfterTxtBox.Name = "dXAfterTxtBox";
            this.dXAfterTxtBox.Size = new System.Drawing.Size(47, 20);
            this.dXAfterTxtBox.TabIndex = 54;
            this.dXAfterTxtBox.Text = "0";
            // 
            // YAfterTxtBox
            // 
            this.YAfterTxtBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.YAfterTxtBox.Location = new System.Drawing.Point(1155, 536);
            this.YAfterTxtBox.Name = "YAfterTxtBox";
            this.YAfterTxtBox.Size = new System.Drawing.Size(47, 20);
            this.YAfterTxtBox.TabIndex = 53;
            this.YAfterTxtBox.Text = "0";
            // 
            // XAfterTxtBox
            // 
            this.XAfterTxtBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.XAfterTxtBox.Location = new System.Drawing.Point(1155, 484);
            this.XAfterTxtBox.Name = "XAfterTxtBox";
            this.XAfterTxtBox.Size = new System.Drawing.Size(47, 20);
            this.XAfterTxtBox.TabIndex = 52;
            this.XAfterTxtBox.Text = "0";
            // 
            // CropAfterChkBox
            // 
            this.CropAfterChkBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CropAfterChkBox.AutoSize = true;
            this.CropAfterChkBox.Checked = true;
            this.CropAfterChkBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CropAfterChkBox.Location = new System.Drawing.Point(1212, 460);
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
            this.label18.Location = new System.Drawing.Point(1131, 456);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(80, 19);
            this.label18.TabIndex = 50;
            this.label18.Text = "Crop After";
            // 
            // DistZeroBtn
            // 
            this.DistZeroBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.DistZeroBtn.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.DistZeroBtn.Location = new System.Drawing.Point(1213, 231);
            this.DistZeroBtn.Name = "DistZeroBtn";
            this.DistZeroBtn.Size = new System.Drawing.Size(21, 120);
            this.DistZeroBtn.TabIndex = 62;
            this.DistZeroBtn.Text = "zero";
            this.DistZeroBtn.UseVisualStyleBackColor = true;
            this.DistZeroBtn.Click += new System.EventHandler(this.DistZeroBtn_Click);
            // 
            // SaveAsBtn
            // 
            this.SaveAsBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.SaveAsBtn.Location = new System.Drawing.Point(1111, 707);
            this.SaveAsBtn.Name = "SaveAsBtn";
            this.SaveAsBtn.Size = new System.Drawing.Size(53, 50);
            this.SaveAsBtn.TabIndex = 65;
            this.SaveAsBtn.Text = "Save";
            this.SaveAsBtn.UseVisualStyleBackColor = true;
            this.SaveAsBtn.Click += new System.EventHandler(this.SaveAsBtn_Click);
            // 
            // LoadFrBtn
            // 
            this.LoadFrBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.LoadFrBtn.Location = new System.Drawing.Point(1181, 707);
            this.LoadFrBtn.Name = "LoadFrBtn";
            this.LoadFrBtn.Size = new System.Drawing.Size(53, 50);
            this.LoadFrBtn.TabIndex = 66;
            this.LoadFrBtn.Text = "Load";
            this.LoadFrBtn.UseVisualStyleBackColor = true;
            this.LoadFrBtn.Click += new System.EventHandler(this.LoadFrBtn_Click);
            // 
            // ShowGridСhckBox
            // 
            this.ShowGridСhckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ShowGridСhckBox.AutoSize = true;
            this.ShowGridСhckBox.Location = new System.Drawing.Point(1114, 642);
            this.ShowGridСhckBox.Name = "ShowGridСhckBox";
            this.ShowGridСhckBox.Size = new System.Drawing.Size(75, 17);
            this.ShowGridСhckBox.TabIndex = 67;
            this.ShowGridСhckBox.Text = "Show Grid";
            this.ShowGridСhckBox.UseVisualStyleBackColor = true;
            // 
            // ETxtBox
            // 
            this.ETxtBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ETxtBox.Location = new System.Drawing.Point(1113, 331);
            this.ETxtBox.Name = "ETxtBox";
            this.ETxtBox.Size = new System.Drawing.Size(55, 20);
            this.ETxtBox.TabIndex = 70;
            this.ETxtBox.TextChanged += new System.EventHandler(this.ETxtBox_TextChanged);
            // 
            // EBtnDn
            // 
            this.EBtnDn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.EBtnDn.Location = new System.Drawing.Point(1192, 332);
            this.EBtnDn.Name = "EBtnDn";
            this.EBtnDn.Size = new System.Drawing.Size(13, 16);
            this.EBtnDn.TabIndex = 69;
            this.EBtnDn.Text = "v";
            this.EBtnDn.UseVisualStyleBackColor = true;
            this.EBtnDn.Click += new System.EventHandler(this.EBtnDn_Click);
            // 
            // EBtnUp
            // 
            this.EBtnUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.EBtnUp.Location = new System.Drawing.Point(1173, 332);
            this.EBtnUp.Name = "EBtnUp";
            this.EBtnUp.Size = new System.Drawing.Size(13, 16);
            this.EBtnUp.TabIndex = 68;
            this.EBtnUp.Text = "^";
            this.EBtnUp.UseVisualStyleBackColor = true;
            this.EBtnUp.Click += new System.EventHandler(this.EBtnUp_Click);
            // 
            // AutoReloadChkBox
            // 
            this.AutoReloadChkBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.AutoReloadChkBox.AutoSize = true;
            this.AutoReloadChkBox.Checked = true;
            this.AutoReloadChkBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.AutoReloadChkBox.Location = new System.Drawing.Point(1114, 622);
            this.AutoReloadChkBox.Name = "AutoReloadChkBox";
            this.AutoReloadChkBox.Size = new System.Drawing.Size(15, 14);
            this.AutoReloadChkBox.TabIndex = 71;
            this.AutoReloadChkBox.UseVisualStyleBackColor = true;
            // 
            // label19
            // 
            this.label19.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label19.Location = new System.Drawing.Point(1130, 621);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(69, 15);
            this.label19.TabIndex = 72;
            this.label19.Text = "Auto reload";
            // 
            // Sm11TxtBox
            // 
            this.Sm11TxtBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Sm11TxtBox.Location = new System.Drawing.Point(1110, 379);
            this.Sm11TxtBox.Name = "Sm11TxtBox";
            this.Sm11TxtBox.Size = new System.Drawing.Size(46, 20);
            this.Sm11TxtBox.TabIndex = 73;
            this.Sm11TxtBox.TextChanged += new System.EventHandler(this.Sm11TxtBox_TextChanged);
            // 
            // Sm12TxtBox
            // 
            this.Sm12TxtBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Sm12TxtBox.Location = new System.Drawing.Point(1156, 379);
            this.Sm12TxtBox.Name = "Sm12TxtBox";
            this.Sm12TxtBox.Size = new System.Drawing.Size(43, 20);
            this.Sm12TxtBox.TabIndex = 74;
            this.Sm12TxtBox.TextChanged += new System.EventHandler(this.Sm12TxtBox_TextChanged);
            // 
            // Sm13TxtBox
            // 
            this.Sm13TxtBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Sm13TxtBox.Location = new System.Drawing.Point(1199, 379);
            this.Sm13TxtBox.Name = "Sm13TxtBox";
            this.Sm13TxtBox.Size = new System.Drawing.Size(42, 20);
            this.Sm13TxtBox.TabIndex = 75;
            this.Sm13TxtBox.TextChanged += new System.EventHandler(this.Sm13TxtBox_TextChanged);
            // 
            // Sm23TxtBox
            // 
            this.Sm23TxtBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Sm23TxtBox.Location = new System.Drawing.Point(1199, 405);
            this.Sm23TxtBox.Name = "Sm23TxtBox";
            this.Sm23TxtBox.Size = new System.Drawing.Size(42, 20);
            this.Sm23TxtBox.TabIndex = 78;
            this.Sm23TxtBox.TextChanged += new System.EventHandler(this.Sm23TxtBox_TextChanged);
            // 
            // Sm22TxtBox
            // 
            this.Sm22TxtBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Sm22TxtBox.Location = new System.Drawing.Point(1156, 405);
            this.Sm22TxtBox.Name = "Sm22TxtBox";
            this.Sm22TxtBox.Size = new System.Drawing.Size(43, 20);
            this.Sm22TxtBox.TabIndex = 77;
            this.Sm22TxtBox.TextChanged += new System.EventHandler(this.Sm22TxtBox_TextChanged);
            // 
            // Sm21TxtBox
            // 
            this.Sm21TxtBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Sm21TxtBox.Location = new System.Drawing.Point(1110, 405);
            this.Sm21TxtBox.Name = "Sm21TxtBox";
            this.Sm21TxtBox.Size = new System.Drawing.Size(46, 20);
            this.Sm21TxtBox.TabIndex = 76;
            this.Sm21TxtBox.TextChanged += new System.EventHandler(this.Sm21TxtBox_TextChanged);
            // 
            // Sm33TxtBox
            // 
            this.Sm33TxtBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Sm33TxtBox.Location = new System.Drawing.Point(1199, 431);
            this.Sm33TxtBox.Name = "Sm33TxtBox";
            this.Sm33TxtBox.Size = new System.Drawing.Size(42, 20);
            this.Sm33TxtBox.TabIndex = 81;
            this.Sm33TxtBox.TextChanged += new System.EventHandler(this.Sm33TxtBox_TextChanged);
            // 
            // Sm32TxtBox
            // 
            this.Sm32TxtBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Sm32TxtBox.Location = new System.Drawing.Point(1156, 431);
            this.Sm32TxtBox.Name = "Sm32TxtBox";
            this.Sm32TxtBox.Size = new System.Drawing.Size(43, 20);
            this.Sm32TxtBox.TabIndex = 80;
            this.Sm32TxtBox.TextChanged += new System.EventHandler(this.Sm32TxtBox_TextChanged);
            // 
            // Sm31TxtBox
            // 
            this.Sm31TxtBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Sm31TxtBox.Location = new System.Drawing.Point(1110, 431);
            this.Sm31TxtBox.Name = "Sm31TxtBox";
            this.Sm31TxtBox.Size = new System.Drawing.Size(46, 20);
            this.Sm31TxtBox.TabIndex = 79;
            this.Sm31TxtBox.TextChanged += new System.EventHandler(this.Sm31TxtBox_TextChanged);
            // 
            // ZeroCropAfterBtn
            // 
            this.ZeroCropAfterBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ZeroCropAfterBtn.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.ZeroCropAfterBtn.Location = new System.Drawing.Point(1212, 482);
            this.ZeroCropAfterBtn.Name = "ZeroCropAfterBtn";
            this.ZeroCropAfterBtn.Size = new System.Drawing.Size(21, 100);
            this.ZeroCropAfterBtn.TabIndex = 82;
            this.ZeroCropAfterBtn.Text = "zero";
            this.ZeroCropAfterBtn.UseVisualStyleBackColor = true;
            this.ZeroCropAfterBtn.Click += new System.EventHandler(this.ZeroCropAfterBtn_Click);
            // 
            // label12
            // 
            this.label12.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.label12.Location = new System.Drawing.Point(1107, 208);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(143, 19);
            this.label12.TabIndex = 83;
            this.label12.Text = "Distortion parameters";
            // 
            // label13
            // 
            this.label13.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.label13.Location = new System.Drawing.Point(1109, 355);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(129, 19);
            this.label13.TabIndex = 84;
            this.label13.Text = "Camera parameters";
            // 
            // DistChkBox
            // 
            this.DistChkBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.DistChkBox.AutoSize = true;
            this.DistChkBox.Checked = true;
            this.DistChkBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.DistChkBox.Location = new System.Drawing.Point(1228, 190);
            this.DistChkBox.Name = "DistChkBox";
            this.DistChkBox.Size = new System.Drawing.Size(15, 14);
            this.DistChkBox.TabIndex = 85;
            this.DistChkBox.UseVisualStyleBackColor = true;
            // 
            // ImgFixingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1243, 869);
            this.Controls.Add(this.DistChkBox);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.ZeroCropAfterBtn);
            this.Controls.Add(this.Sm33TxtBox);
            this.Controls.Add(this.Sm32TxtBox);
            this.Controls.Add(this.Sm31TxtBox);
            this.Controls.Add(this.Sm23TxtBox);
            this.Controls.Add(this.Sm22TxtBox);
            this.Controls.Add(this.Sm21TxtBox);
            this.Controls.Add(this.Sm13TxtBox);
            this.Controls.Add(this.Sm12TxtBox);
            this.Controls.Add(this.Sm11TxtBox);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.AutoReloadChkBox);
            this.Controls.Add(this.ETxtBox);
            this.Controls.Add(this.EBtnDn);
            this.Controls.Add(this.EBtnUp);
            this.Controls.Add(this.ShowGridСhckBox);
            this.Controls.Add(this.LoadFrBtn);
            this.Controls.Add(this.SaveAsBtn);
            this.Controls.Add(this.DistZeroBtn);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.dYAfterTxtBox);
            this.Controls.Add(this.dXAfterTxtBox);
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
            this.Controls.Add(this.RotationChkBox);
            this.Controls.Add(this.CropBeforeChkBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.DistortionMetodLabel);
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
            this.MinimumSize = new System.Drawing.Size(1117, 785);
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
        private Label DistortionMetodLabel;
        private Label label5;
        private CheckBox CropBeforeChkBox;
        private CheckBox RotationChkBox;
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
        private Label label14;
        private Label label15;
        private Label label16;
        private Label label17;
        private TextBox dYAfterTxtBox;
        private TextBox dXAfterTxtBox;
        private TextBox YAfterTxtBox;
        private TextBox XAfterTxtBox;
        private CheckBox CropAfterChkBox;
        private Label label18;
        private Button DistZeroBtn;
        private Button SaveAsBtn;
        private Button LoadFrBtn;
        private CheckBox ShowGridСhckBox;
        private TextBox ETxtBox;
        private Button EBtnDn;
        private Button EBtnUp;
        private CheckBox AutoReloadChkBox;
        private Label label19;
        private TextBox Sm11TxtBox;
        private TextBox Sm12TxtBox;
        private TextBox Sm13TxtBox;
        private TextBox Sm23TxtBox;
        private TextBox Sm22TxtBox;
        private TextBox Sm21TxtBox;
        private TextBox Sm33TxtBox;
        private TextBox Sm32TxtBox;
        private TextBox Sm31TxtBox;
        private Button ZeroCropAfterBtn;
        private Label label12;
        private Label label13;
        private CheckBox DistChkBox;
    }
}