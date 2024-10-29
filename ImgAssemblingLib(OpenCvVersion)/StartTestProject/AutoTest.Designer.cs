namespace StartTestProject
{
    partial class AutoTest
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
            this.PuskBtn = new System.Windows.Forms.Button();
            this.GeneralProgressBar = new System.Windows.Forms.ProgressBar();
            this.CurentProgressBar = new System.Windows.Forms.ProgressBar();
            this.CurrentProgressLb = new System.Windows.Forms.Label();
            this.GeneralProgressLb = new System.Windows.Forms.Label();
            this.notShowRezultChkBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // PuskBtn
            // 
            this.PuskBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.PuskBtn.Location = new System.Drawing.Point(27, 517);
            this.PuskBtn.Name = "PuskBtn";
            this.PuskBtn.Size = new System.Drawing.Size(75, 37);
            this.PuskBtn.TabIndex = 0;
            this.PuskBtn.Text = "Пуск";
            this.PuskBtn.UseVisualStyleBackColor = true;
            this.PuskBtn.Click += new System.EventHandler(this.PuskBtn_Click);
            // 
            // GeneralProgressBar
            // 
            this.GeneralProgressBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.GeneralProgressBar.Location = new System.Drawing.Point(12, 456);
            this.GeneralProgressBar.Name = "GeneralProgressBar";
            this.GeneralProgressBar.Size = new System.Drawing.Size(224, 23);
            this.GeneralProgressBar.TabIndex = 1;
            // 
            // CurentProgressBar
            // 
            this.CurentProgressBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.CurentProgressBar.Location = new System.Drawing.Point(12, 485);
            this.CurentProgressBar.Name = "CurentProgressBar";
            this.CurentProgressBar.Size = new System.Drawing.Size(224, 23);
            this.CurentProgressBar.TabIndex = 2;
            // 
            // CurrentProgressLb
            // 
            this.CurrentProgressLb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.CurrentProgressLb.AutoSize = true;
            this.CurrentProgressLb.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.CurrentProgressLb.Location = new System.Drawing.Point(40, 488);
            this.CurrentProgressLb.Name = "CurrentProgressLb";
            this.CurrentProgressLb.Size = new System.Drawing.Size(0, 17);
            this.CurrentProgressLb.TabIndex = 12;
            // 
            // GeneralProgressLb
            // 
            this.GeneralProgressLb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.GeneralProgressLb.AutoSize = true;
            this.GeneralProgressLb.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.GeneralProgressLb.Location = new System.Drawing.Point(109, 459);
            this.GeneralProgressLb.Name = "GeneralProgressLb";
            this.GeneralProgressLb.Size = new System.Drawing.Size(0, 17);
            this.GeneralProgressLb.TabIndex = 13;
            // 
            // notShowRezultChkBox
            // 
            this.notShowRezultChkBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.notShowRezultChkBox.AutoSize = true;
            this.notShowRezultChkBox.Location = new System.Drawing.Point(119, 528);
            this.notShowRezultChkBox.Name = "notShowRezultChkBox";
            this.notShowRezultChkBox.Size = new System.Drawing.Size(107, 17);
            this.notShowRezultChkBox.TabIndex = 14;
            this.notShowRezultChkBox.Text = "Don\'t show rezult";
            this.notShowRezultChkBox.UseVisualStyleBackColor = true;
            // 
            // AutoTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(249, 566);
            this.Controls.Add(this.notShowRezultChkBox);
            this.Controls.Add(this.CurrentProgressLb);
            this.Controls.Add(this.GeneralProgressLb);
            this.Controls.Add(this.CurentProgressBar);
            this.Controls.Add(this.GeneralProgressBar);
            this.Controls.Add(this.PuskBtn);
            this.MinimumSize = new System.Drawing.Size(265, 500);
            this.Name = "AutoTest";
            this.Text = "AutoTest";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button PuskBtn;
        private System.Windows.Forms.ProgressBar GeneralProgressBar;
        private System.Windows.Forms.ProgressBar CurentProgressBar;
        private System.Windows.Forms.Label CurrentProgressLb;
        private System.Windows.Forms.Label GeneralProgressLb;
        private System.Windows.Forms.CheckBox notShowRezultChkBox;
    }
}