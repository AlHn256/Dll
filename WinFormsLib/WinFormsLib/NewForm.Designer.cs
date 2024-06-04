namespace WinFormsLib
{
    partial class NewForm
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
            button1 = new Button();
            label1 = new Label();
            button2 = new Button();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(301, 140);
            button1.Name = "button1";
            button1.Size = new Size(99, 81);
            button1.TabIndex = 0;
            button1.Text = "Close";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 32F, FontStyle.Bold);
            label1.Location = new Point(100, 27);
            label1.Name = "label1";
            label1.Size = new Size(232, 59);
            label1.TabIndex = 1;
            label1.Text = "New Form";
            // 
            // button2
            // 
            button2.Location = new Point(12, 140);
            button2.Name = "button2";
            button2.Size = new Size(98, 81);
            button2.TabIndex = 2;
            button2.Text = "Ok";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button1_Click;
            // 
            // NewForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(431, 240);
            Controls.Add(button2);
            Controls.Add(label1);
            Controls.Add(button1);
            Name = "NewForm";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private Label label1;
        private Button button2;
    }
}