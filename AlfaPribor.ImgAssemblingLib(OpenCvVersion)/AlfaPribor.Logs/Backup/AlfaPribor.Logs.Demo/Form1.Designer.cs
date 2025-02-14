namespace AlfaPribor.Logs.Demo
{
    partial class FormMain
    {
        /// <summary>
        /// Требуется переменная конструктора.
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
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.groupBoxLogSettings = new System.Windows.Forms.GroupBox();
            this.checkBoxAutoCreate = new System.Windows.Forms.CheckBox();
            this.numericUpDownPartMaxSize = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.numericUpDownPartsCount = new System.Windows.Forms.NumericUpDown();
            this.labelParts = new System.Windows.Forms.Label();
            this.buttonFileChoice = new System.Windows.Forms.Button();
            this.textBoxFileName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.label4 = new System.Windows.Forms.Label();
            this.buttonStart = new System.Windows.Forms.Button();
            this.buttonStop = new System.Windows.Forms.Button();
            this.timerSystem = new System.Windows.Forms.Timer(this.components);
            this.numericUpDownTimeInterval = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBoxEncoding = new System.Windows.Forms.ComboBox();
            this.groupBoxLogSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPartMaxSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPartsCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTimeInterval)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBoxLogSettings
            // 
            this.groupBoxLogSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxLogSettings.Controls.Add(this.comboBoxEncoding);
            this.groupBoxLogSettings.Controls.Add(this.label3);
            this.groupBoxLogSettings.Controls.Add(this.checkBoxAutoCreate);
            this.groupBoxLogSettings.Controls.Add(this.numericUpDownPartMaxSize);
            this.groupBoxLogSettings.Controls.Add(this.label2);
            this.groupBoxLogSettings.Controls.Add(this.numericUpDownPartsCount);
            this.groupBoxLogSettings.Controls.Add(this.labelParts);
            this.groupBoxLogSettings.Controls.Add(this.buttonFileChoice);
            this.groupBoxLogSettings.Controls.Add(this.textBoxFileName);
            this.groupBoxLogSettings.Controls.Add(this.label1);
            this.groupBoxLogSettings.Location = new System.Drawing.Point(12, 12);
            this.groupBoxLogSettings.Name = "groupBoxLogSettings";
            this.groupBoxLogSettings.Size = new System.Drawing.Size(398, 175);
            this.groupBoxLogSettings.TabIndex = 0;
            this.groupBoxLogSettings.TabStop = false;
            this.groupBoxLogSettings.Text = "Настройки журнала";
            // 
            // checkBoxAutoCreate
            // 
            this.checkBoxAutoCreate.AutoSize = true;
            this.checkBoxAutoCreate.Checked = global::AlfaPribor.Logs.Demo.Properties.Settings.Default.LogAutoCreate;
            this.checkBoxAutoCreate.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxAutoCreate.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::AlfaPribor.Logs.Demo.Properties.Settings.Default, "LogAutoCreate", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBoxAutoCreate.Location = new System.Drawing.Point(9, 120);
            this.checkBoxAutoCreate.Name = "checkBoxAutoCreate";
            this.checkBoxAutoCreate.Size = new System.Drawing.Size(302, 17);
            this.checkBoxAutoCreate.TabIndex = 7;
            this.checkBoxAutoCreate.Text = "Автоматически создавать файл журнала регистрации";
            this.checkBoxAutoCreate.UseVisualStyleBackColor = true;
            // 
            // numericUpDownPartMaxSize
            // 
            this.numericUpDownPartMaxSize.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::AlfaPribor.Logs.Demo.Properties.Settings.Default, "LogPartMaxSize", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.numericUpDownPartMaxSize.Location = new System.Drawing.Point(236, 94);
            this.numericUpDownPartMaxSize.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numericUpDownPartMaxSize.Name = "numericUpDownPartMaxSize";
            this.numericUpDownPartMaxSize.Size = new System.Drawing.Size(81, 20);
            this.numericUpDownPartMaxSize.TabIndex = 6;
            this.numericUpDownPartMaxSize.Value = global::AlfaPribor.Logs.Demo.Properties.Settings.Default.LogPartMaxSize;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 96);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(184, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Макс.размер каждой части, КБайт";
            // 
            // numericUpDownPartsCount
            // 
            this.numericUpDownPartsCount.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::AlfaPribor.Logs.Demo.Properties.Settings.Default, "LogPartsCount", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.numericUpDownPartsCount.Location = new System.Drawing.Point(236, 68);
            this.numericUpDownPartsCount.Name = "numericUpDownPartsCount";
            this.numericUpDownPartsCount.Size = new System.Drawing.Size(81, 20);
            this.numericUpDownPartsCount.TabIndex = 4;
            this.numericUpDownPartsCount.Value = global::AlfaPribor.Logs.Demo.Properties.Settings.Default.LogPartsCount;
            // 
            // labelParts
            // 
            this.labelParts.AutoSize = true;
            this.labelParts.Location = new System.Drawing.Point(6, 70);
            this.labelParts.Name = "labelParts";
            this.labelParts.Size = new System.Drawing.Size(103, 13);
            this.labelParts.TabIndex = 3;
            this.labelParts.Text = "Количество частей";
            // 
            // buttonFileChoice
            // 
            this.buttonFileChoice.Location = new System.Drawing.Point(323, 40);
            this.buttonFileChoice.Name = "buttonFileChoice";
            this.buttonFileChoice.Size = new System.Drawing.Size(69, 23);
            this.buttonFileChoice.TabIndex = 2;
            this.buttonFileChoice.Text = "Обзор";
            this.buttonFileChoice.UseVisualStyleBackColor = true;
            this.buttonFileChoice.Click += new System.EventHandler(this.buttonFileChoice_Click);
            // 
            // textBoxFileName
            // 
            this.textBoxFileName.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::AlfaPribor.Logs.Demo.Properties.Settings.Default, "LogFileName", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBoxFileName.Location = new System.Drawing.Point(9, 42);
            this.textBoxFileName.Name = "textBoxFileName";
            this.textBoxFileName.Size = new System.Drawing.Size(308, 20);
            this.textBoxFileName.TabIndex = 1;
            this.textBoxFileName.Text = global::AlfaPribor.Logs.Demo.Properties.Settings.Default.LogFileName;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Имя файла";
            // 
            // openFileDialog
            // 
            this.openFileDialog.CheckFileExists = false;
            this.openFileDialog.DefaultExt = "log";
            this.openFileDialog.FileName = "LogExample.log";
            this.openFileDialog.Filter = "Файлы журнала регистрации (*.log)|*.log|Все файлы (*.*)|*.*";
            this.openFileDialog.ReadOnlyChecked = true;
            this.openFileDialog.Title = "Выберите файл жернала регистрации";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(18, 199);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(176, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Интервал записи в журнал, мсек";
            // 
            // buttonStart
            // 
            this.buttonStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonStart.Location = new System.Drawing.Point(12, 226);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(109, 36);
            this.buttonStart.TabIndex = 12;
            this.buttonStart.Text = "Начать запись";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // buttonStop
            // 
            this.buttonStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonStop.Enabled = false;
            this.buttonStop.Location = new System.Drawing.Point(133, 226);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(109, 36);
            this.buttonStop.TabIndex = 13;
            this.buttonStop.Text = "Закончить запись";
            this.buttonStop.UseVisualStyleBackColor = true;
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // timerSystem
            // 
            this.timerSystem.Tick += new System.EventHandler(this.timerSystem_Tick);
            // 
            // numericUpDownTimeInterval
            // 
            this.numericUpDownTimeInterval.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.numericUpDownTimeInterval.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::AlfaPribor.Logs.Demo.Properties.Settings.Default, "LogTimerInterval", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.numericUpDownTimeInterval.Location = new System.Drawing.Point(248, 197);
            this.numericUpDownTimeInterval.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numericUpDownTimeInterval.Name = "numericUpDownTimeInterval";
            this.numericUpDownTimeInterval.Size = new System.Drawing.Size(81, 20);
            this.numericUpDownTimeInterval.TabIndex = 11;
            this.numericUpDownTimeInterval.Value = global::AlfaPribor.Logs.Demo.Properties.Settings.Default.LogTimerInterval;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 146);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(115, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Кодировка символов";
            // 
            // comboBoxEncoding
            // 
            this.comboBoxEncoding.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxEncoding.FormattingEnabled = true;
            this.comboBoxEncoding.Location = new System.Drawing.Point(155, 143);
            this.comboBoxEncoding.Name = "comboBoxEncoding";
            this.comboBoxEncoding.Size = new System.Drawing.Size(162, 21);
            this.comboBoxEncoding.TabIndex = 9;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(419, 276);
            this.Controls.Add(this.buttonStop);
            this.Controls.Add(this.buttonStart);
            this.Controls.Add(this.numericUpDownTimeInterval);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.groupBoxLogSettings);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Демонстрация ведения журнала регистрации";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormMain_FormClosed);
            this.groupBoxLogSettings.ResumeLayout(false);
            this.groupBoxLogSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPartMaxSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPartsCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTimeInterval)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxLogSettings;
        private System.Windows.Forms.Button buttonFileChoice;
        private System.Windows.Forms.TextBox textBoxFileName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericUpDownPartMaxSize;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numericUpDownPartsCount;
        private System.Windows.Forms.Label labelParts;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numericUpDownTimeInterval;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.Timer timerSystem;
        private System.Windows.Forms.CheckBox checkBoxAutoCreate;
        private System.Windows.Forms.ComboBox comboBoxEncoding;
        private System.Windows.Forms.Label label3;
    }
}

