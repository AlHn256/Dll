namespace StartTestProject.Forms
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
            this.FixingImgsUsingDataArrayBtn = new System.Windows.Forms.Button();
            this.Exampl1Btn = new System.Windows.Forms.Button();
            this.Exampl2Btn = new System.Windows.Forms.Button();
            this.Exampl3Btn = new System.Windows.Forms.Button();
            this.Exampl4Btn = new System.Windows.Forms.Button();
            this.RezultLb = new System.Windows.Forms.Label();
            this.Exampl5Btn = new System.Windows.Forms.Button();
            this.Exampl6Btn = new System.Windows.Forms.Button();
            this.Exampl8Btn = new System.Windows.Forms.Button();
            this.progressBarLabel = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.CpuLb = new System.Windows.Forms.Label();
            this.KeypointsAreaBtn = new System.Windows.Forms.Button();
            this.ExamplDirectory = new System.Windows.Forms.TextBox();
            this.OpenDirBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SecondFormBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // MainFormBtn
            // 
            this.MainFormBtn.Location = new System.Drawing.Point(48, 387);
            this.MainFormBtn.Name = "MainFormBtn";
            this.MainFormBtn.Size = new System.Drawing.Size(527, 23);
            this.MainFormBtn.TabIndex = 0;
            this.MainFormBtn.Text = "Main Form";
            this.MainFormBtn.UseVisualStyleBackColor = true;
            this.MainFormBtn.Click += new System.EventHandler(this.MainFormBtn_Click);
            // 
            // FixingImgsUsingDataArrayBtn
            // 
            this.FixingImgsUsingDataArrayBtn.Location = new System.Drawing.Point(48, 224);
            this.FixingImgsUsingDataArrayBtn.Name = "FixingImgsUsingDataArrayBtn";
            this.FixingImgsUsingDataArrayBtn.Size = new System.Drawing.Size(527, 23);
            this.FixingImgsUsingDataArrayBtn.TabIndex = 3;
            this.FixingImgsUsingDataArrayBtn.Text = "Коррекции изображения без сборки";
            this.FixingImgsUsingDataArrayBtn.UseVisualStyleBackColor = true;
            this.FixingImgsUsingDataArrayBtn.Click += new System.EventHandler(this.FixingImgsUsingDataArrayBtn_Click);
            // 
            // Exampl1Btn
            // 
            this.Exampl1Btn.Location = new System.Drawing.Point(48, 50);
            this.Exampl1Btn.Name = "Exampl1Btn";
            this.Exampl1Btn.Size = new System.Drawing.Size(527, 23);
            this.Exampl1Btn.TabIndex = 4;
            this.Exampl1Btn.Text = "Пример сборки изображения с использованием только файла плана сборки";
            this.Exampl1Btn.UseVisualStyleBackColor = true;
            this.Exampl1Btn.Click += new System.EventHandler(this.Exampl1Btn_Click);
            // 
            // Exampl2Btn
            // 
            this.Exampl2Btn.Location = new System.Drawing.Point(48, 79);
            this.Exampl2Btn.Name = "Exampl2Btn";
            this.Exampl2Btn.Size = new System.Drawing.Size(527, 23);
            this.Exampl2Btn.TabIndex = 5;
            this.Exampl2Btn.Text = "Пример без исправления изображений";
            this.Exampl2Btn.UseVisualStyleBackColor = true;
            this.Exampl2Btn.Click += new System.EventHandler(this.Exampl2Btn_Click);
            // 
            // Exampl3Btn
            // 
            this.Exampl3Btn.Location = new System.Drawing.Point(48, 108);
            this.Exampl3Btn.Name = "Exampl3Btn";
            this.Exampl3Btn.Size = new System.Drawing.Size(527, 23);
            this.Exampl3Btn.TabIndex = 6;
            this.Exampl3Btn.Text = "Пример с установкой другого плана корректировки изображения";
            this.Exampl3Btn.UseVisualStyleBackColor = true;
            this.Exampl3Btn.Click += new System.EventHandler(this.Exampl3Btn_Click);
            // 
            // Exampl4Btn
            // 
            this.Exampl4Btn.Location = new System.Drawing.Point(48, 137);
            this.Exampl4Btn.Name = "Exampl4Btn";
            this.Exampl4Btn.Size = new System.Drawing.Size(527, 23);
            this.Exampl4Btn.TabIndex = 7;
            this.Exampl4Btn.Text = "Пример настройки смещения полосы сборки относительно центра катинки";
            this.Exampl4Btn.UseVisualStyleBackColor = true;
            this.Exampl4Btn.Click += new System.EventHandler(this.Exampl4Btn_Click);
            // 
            // RezultLb
            // 
            this.RezultLb.AutoSize = true;
            this.RezultLb.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.RezultLb.Location = new System.Drawing.Point(51, 362);
            this.RezultLb.Name = "RezultLb";
            this.RezultLb.Size = new System.Drawing.Size(0, 13);
            this.RezultLb.TabIndex = 8;
            // 
            // Exampl5Btn
            // 
            this.Exampl5Btn.Location = new System.Drawing.Point(48, 166);
            this.Exampl5Btn.Name = "Exampl5Btn";
            this.Exampl5Btn.Size = new System.Drawing.Size(527, 23);
            this.Exampl5Btn.TabIndex = 9;
            this.Exampl5Btn.Text = "Подсчет скорости (без сборки изображения)";
            this.Exampl5Btn.UseVisualStyleBackColor = true;
            this.Exampl5Btn.Click += new System.EventHandler(this.Exampl5Btn_Click);
            // 
            // Exampl6Btn
            // 
            this.Exampl6Btn.Location = new System.Drawing.Point(48, 195);
            this.Exampl6Btn.Name = "Exampl6Btn";
            this.Exampl6Btn.Size = new System.Drawing.Size(527, 23);
            this.Exampl6Btn.TabIndex = 10;
            this.Exampl6Btn.Text = "Настройка параметров (без файла)";
            this.Exampl6Btn.UseVisualStyleBackColor = true;
            this.Exampl6Btn.Click += new System.EventHandler(this.Exampl6Btn_Click);
            // 
            // Exampl8Btn
            // 
            this.Exampl8Btn.Location = new System.Drawing.Point(48, 282);
            this.Exampl8Btn.Name = "Exampl8Btn";
            this.Exampl8Btn.Size = new System.Drawing.Size(527, 23);
            this.Exampl8Btn.TabIndex = 11;
            this.Exampl8Btn.Text = "Сборка с прогресбаром";
            this.Exampl8Btn.UseVisualStyleBackColor = true;
            this.Exampl8Btn.Click += new System.EventHandler(this.Exampl8Btn_Click);
            // 
            // progressBarLabel
            // 
            this.progressBarLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBarLabel.AutoSize = true;
            this.progressBarLabel.Font = new System.Drawing.Font("Segoe UI Black", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.progressBarLabel.Location = new System.Drawing.Point(260, 321);
            this.progressBarLabel.Name = "progressBarLabel";
            this.progressBarLabel.Size = new System.Drawing.Size(39, 15);
            this.progressBarLabel.TabIndex = 30;
            this.progressBarLabel.Text = "Point";
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(48, 318);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(527, 20);
            this.progressBar.TabIndex = 29;
            // 
            // CpuLb
            // 
            this.CpuLb.AutoSize = true;
            this.CpuLb.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.CpuLb.Location = new System.Drawing.Point(400, 362);
            this.CpuLb.Name = "CpuLb";
            this.CpuLb.Size = new System.Drawing.Size(0, 15);
            this.CpuLb.TabIndex = 31;
            // 
            // KeypointsAreaBtn
            // 
            this.KeypointsAreaBtn.Location = new System.Drawing.Point(48, 253);
            this.KeypointsAreaBtn.Name = "KeypointsAreaBtn";
            this.KeypointsAreaBtn.Size = new System.Drawing.Size(527, 23);
            this.KeypointsAreaBtn.TabIndex = 32;
            this.KeypointsAreaBtn.Text = "Пример с устновкой области поиска ключевых точек";
            this.KeypointsAreaBtn.UseVisualStyleBackColor = true;
            this.KeypointsAreaBtn.Click += new System.EventHandler(this.KeypointsAreaBtn_Click);
            // 
            // ExamplDirectory
            // 
            this.ExamplDirectory.Location = new System.Drawing.Point(198, 15);
            this.ExamplDirectory.Name = "ExamplDirectory";
            this.ExamplDirectory.Size = new System.Drawing.Size(331, 20);
            this.ExamplDirectory.TabIndex = 33;
            this.ExamplDirectory.Text = "D:\\Work\\C#\\Dll\\AlfaPribor.ImgAssemblingLib(OpenCvVersion)\\AlfaPribor.ImgExample";
            this.ExamplDirectory.TextChanged += new System.EventHandler(this.ExamplDirectory_TextChanged);
            // 
            // OpenDirBtn
            // 
            this.OpenDirBtn.Location = new System.Drawing.Point(539, 14);
            this.OpenDirBtn.Name = "OpenDirBtn";
            this.OpenDirBtn.Size = new System.Drawing.Size(35, 22);
            this.OpenDirBtn.TabIndex = 34;
            this.OpenDirBtn.Text = "...";
            this.OpenDirBtn.UseVisualStyleBackColor = true;
            this.OpenDirBtn.Click += new System.EventHandler(this.OpenDirBtn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(51, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(145, 13);
            this.label1.TabIndex = 35;
            this.label1.Text = "Папка с примерами в CVS:";
            // 
            // SecondFormBtn
            // 
            this.SecondFormBtn.Location = new System.Drawing.Point(48, 416);
            this.SecondFormBtn.Name = "SecondFormBtn";
            this.SecondFormBtn.Size = new System.Drawing.Size(527, 23);
            this.SecondFormBtn.TabIndex = 36;
            this.SecondFormBtn.Text = "Second Form";
            this.SecondFormBtn.UseVisualStyleBackColor = true;
            this.SecondFormBtn.Click += new System.EventHandler(this.SecondFormBtn_Click);
            // 
            // TestingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 451);
            this.Controls.Add(this.SecondFormBtn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.OpenDirBtn);
            this.Controls.Add(this.ExamplDirectory);
            this.Controls.Add(this.KeypointsAreaBtn);
            this.Controls.Add(this.CpuLb);
            this.Controls.Add(this.progressBarLabel);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.Exampl8Btn);
            this.Controls.Add(this.Exampl6Btn);
            this.Controls.Add(this.Exampl5Btn);
            this.Controls.Add(this.RezultLb);
            this.Controls.Add(this.Exampl4Btn);
            this.Controls.Add(this.Exampl3Btn);
            this.Controls.Add(this.Exampl2Btn);
            this.Controls.Add(this.Exampl1Btn);
            this.Controls.Add(this.FixingImgsUsingDataArrayBtn);
            this.Controls.Add(this.MainFormBtn);
            this.MaximumSize = new System.Drawing.Size(640, 490);
            this.MinimumSize = new System.Drawing.Size(640, 490);
            this.Name = "TestingForm";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button MainFormBtn;
        private System.Windows.Forms.Button FixingImgsUsingDataArrayBtn;
        private System.Windows.Forms.Button Exampl1Btn;
        private System.Windows.Forms.Button Exampl2Btn;
        private System.Windows.Forms.Button Exampl3Btn;
        private System.Windows.Forms.Button Exampl4Btn;
        private System.Windows.Forms.Label RezultLb;
        private System.Windows.Forms.Button Exampl5Btn;
        private System.Windows.Forms.Button Exampl6Btn;
        private System.Windows.Forms.Button Exampl8Btn;
        private System.Windows.Forms.Label progressBarLabel;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label CpuLb;
        private System.Windows.Forms.Button KeypointsAreaBtn;
        private System.Windows.Forms.TextBox ExamplDirectory;
        private System.Windows.Forms.Button OpenDirBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button SecondFormBtn;
    }
}

