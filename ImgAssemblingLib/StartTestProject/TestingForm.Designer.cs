namespace StartTestProject
{
    partial class TestingForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.MainFormBtn = new System.Windows.Forms.Button();
            this.EditingStitchingPlanBtn = new System.Windows.Forms.Button();
            this.ImgFixingFormBtn = new System.Windows.Forms.Button();
            this.FixingImgsUsingDataArrayBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // MainFormBtn
            // 
            this.MainFormBtn.Location = new System.Drawing.Point(46, 34);
            this.MainFormBtn.Name = "MainFormBtn";
            this.MainFormBtn.Size = new System.Drawing.Size(98, 23);
            this.MainFormBtn.TabIndex = 0;
            this.MainFormBtn.Text = "Main Form";
            this.MainFormBtn.UseVisualStyleBackColor = true;
            this.MainFormBtn.Click += new System.EventHandler(this.MainFormBtn_Click);
            // 
            // EditingStitchingPlanBtn
            // 
            this.EditingStitchingPlanBtn.Location = new System.Drawing.Point(46, 78);
            this.EditingStitchingPlanBtn.Name = "EditingStitchingPlanBtn";
            this.EditingStitchingPlanBtn.Size = new System.Drawing.Size(98, 23);
            this.EditingStitchingPlanBtn.TabIndex = 1;
            this.EditingStitchingPlanBtn.Text = "Editing Stitching Plan";
            this.EditingStitchingPlanBtn.UseVisualStyleBackColor = true;
            this.EditingStitchingPlanBtn.Click += new System.EventHandler(this.EditingStitchingPlanBtn_Click);
            // 
            // ImgFixingFormBtn
            // 
            this.ImgFixingFormBtn.Location = new System.Drawing.Point(46, 123);
            this.ImgFixingFormBtn.Name = "ImgFixingFormBtn";
            this.ImgFixingFormBtn.Size = new System.Drawing.Size(98, 23);
            this.ImgFixingFormBtn.TabIndex = 2;
            this.ImgFixingFormBtn.Text = "Img Fixing Form";
            this.ImgFixingFormBtn.UseVisualStyleBackColor = true;
            this.ImgFixingFormBtn.Click += new System.EventHandler(this.ImgFixingFormBtn_Click);
            // 
            // FixingImgsUsingDataArrayBtn
            // 
            this.FixingImgsUsingDataArrayBtn.Location = new System.Drawing.Point(12, 195);
            this.FixingImgsUsingDataArrayBtn.Name = "FixingImgsUsingDataArrayBtn";
            this.FixingImgsUsingDataArrayBtn.Size = new System.Drawing.Size(167, 23);
            this.FixingImgsUsingDataArrayBtn.TabIndex = 3;
            this.FixingImgsUsingDataArrayBtn.Text = "Fixing Imgs Using DataArray";
            this.FixingImgsUsingDataArrayBtn.UseVisualStyleBackColor = true;
            this.FixingImgsUsingDataArrayBtn.Click += new System.EventHandler(this.FixingImgsUsingDataArrayBtn_Click);
            // 
            // TestingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(197, 450);
            this.Controls.Add(this.FixingImgsUsingDataArrayBtn);
            this.Controls.Add(this.ImgFixingFormBtn);
            this.Controls.Add(this.EditingStitchingPlanBtn);
            this.Controls.Add(this.MainFormBtn);
            this.Name = "TestingForm";
            this.Text = "Form1";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TestingForm_KeyDown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button MainFormBtn;
        private System.Windows.Forms.Button EditingStitchingPlanBtn;
        private System.Windows.Forms.Button ImgFixingFormBtn;
        private System.Windows.Forms.Button FixingImgsUsingDataArrayBtn;
    }
}

