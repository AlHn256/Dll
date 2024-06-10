namespace TestStartingPr
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
            ImgFixingFormBtn = new Button();
            HidenFixingExampelBtn = new Button();
            ExitBtn = new Button();
            HidenFixingExampel2Btn = new Button();
            label1 = new Label();
            progressBar1 = new ProgressBar();
            SuspendLayout();
            // 
            // ImgFixingFormBtn
            // 
            ImgFixingFormBtn.Location = new Point(12, 12);
            ImgFixingFormBtn.Name = "ImgFixingFormBtn";
            ImgFixingFormBtn.Size = new Size(163, 23);
            ImgFixingFormBtn.TabIndex = 0;
            ImgFixingFormBtn.Text = "Img Fixing Form";
            ImgFixingFormBtn.UseVisualStyleBackColor = true;
            ImgFixingFormBtn.Click += ImgFixingFormBtn_Click;
            // 
            // HidenFixingExampelBtn
            // 
            HidenFixingExampelBtn.Location = new Point(12, 60);
            HidenFixingExampelBtn.Name = "HidenFixingExampelBtn";
            HidenFixingExampelBtn.Size = new Size(163, 23);
            HidenFixingExampelBtn.TabIndex = 1;
            HidenFixingExampelBtn.Text = "Hiden Fixing Exampel";
            HidenFixingExampelBtn.UseVisualStyleBackColor = true;
            HidenFixingExampelBtn.Click += HidenFixingExampelBtn_Click;
            // 
            // ExitBtn
            // 
            ExitBtn.Location = new Point(12, 197);
            ExitBtn.Name = "ExitBtn";
            ExitBtn.Size = new Size(163, 23);
            ExitBtn.TabIndex = 2;
            ExitBtn.Text = "Exit";
            ExitBtn.UseVisualStyleBackColor = true;
            ExitBtn.Click += ExitBtn_Click;
            // 
            // HidenFixingExampel2Btn
            // 
            HidenFixingExampel2Btn.Location = new Point(12, 105);
            HidenFixingExampel2Btn.Name = "HidenFixingExampel2Btn";
            HidenFixingExampel2Btn.Size = new Size(163, 23);
            HidenFixingExampel2Btn.TabIndex = 3;
            HidenFixingExampel2Btn.Text = "Hiden Fixing Exampel 2";
            HidenFixingExampel2Btn.UseVisualStyleBackColor = true;
            HidenFixingExampel2Btn.Click += HidenFixingExampel2Btn_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(63, 155);
            label1.Name = "label1";
            label1.Size = new Size(0, 15);
            label1.TabIndex = 4;
            // 
            // progressBar1
            // 
            progressBar1.Location = new Point(12, 151);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(163, 23);
            progressBar1.TabIndex = 5;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(192, 232);
            Controls.Add(label1);
            Controls.Add(progressBar1);
            Controls.Add(HidenFixingExampel2Btn);
            Controls.Add(ExitBtn);
            Controls.Add(HidenFixingExampelBtn);
            Controls.Add(ImgFixingFormBtn);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button ImgFixingFormBtn;
        private Button HidenFixingExampelBtn;
        private Button ExitBtn;
        private Button HidenFixingExampel2Btn;
        private Label label1;
        private ProgressBar progressBar1;
    }
}
