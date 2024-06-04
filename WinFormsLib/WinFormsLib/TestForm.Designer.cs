namespace WinFormsLib
{
    partial class TestForm
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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            StartBtn = new Button();
            progressBar1 = new ProgressBar();
            textBox1 = new TextBox();
            ExitBtn = new Button();
            SuspendLayout();
            // 
            // StartBtn
            // 
            StartBtn.Location = new Point(628, 22);
            StartBtn.Name = "StartBtn";
            StartBtn.Size = new Size(85, 32);
            StartBtn.TabIndex = 0;
            StartBtn.Text = "Start";
            StartBtn.UseVisualStyleBackColor = true;
            StartBtn.Click += StartBtn_Click;
            // 
            // progressBar1
            // 
            progressBar1.Location = new Point(16, 79);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(697, 23);
            progressBar1.TabIndex = 1;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(16, 26);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(606, 23);
            textBox1.TabIndex = 2;
            // 
            // ExitBtn
            // 
            ExitBtn.Location = new Point(628, 228);
            ExitBtn.Name = "ExitBtn";
            ExitBtn.Size = new Size(85, 32);
            ExitBtn.TabIndex = 4;
            ExitBtn.Text = "Exit";
            ExitBtn.UseVisualStyleBackColor = true;
            ExitBtn.Click += ExitBtn_Click;
            // 
            // TestForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(ExitBtn);
            Controls.Add(textBox1);
            Controls.Add(progressBar1);
            Controls.Add(StartBtn);
            Name = "TestForm";
            Size = new Size(734, 272);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button StartBtn;
        private ProgressBar progressBar1;
        private TextBox textBox1;
        private Button ExitBtn;
    }
}
