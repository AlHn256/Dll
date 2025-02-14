using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AlfaPribor.Logs.Demo
{
    public partial class FormMain : Form
    {
        private RotateFileLogger _Log;

        private Random _Generator;

        private EncodingInfo[] encodings;

        private string[] _Messages =
        {
            "Приложение запущено",
            "Приложение остановлено",
            "Регистрация выполнена",
            "Ошибка регистрации",
            "Сообщение получено",
            "Сообщение отправлено",
            "Отсутствует связь с устройством"
        };

        public FormMain()
        {
            InitializeComponent();
            _Generator = new Random();
            encodings = Encoding.GetEncodings();
            var query_names = from info in encodings
                              select info.DisplayName;
            comboBoxEncoding.Items.AddRange(query_names.ToArray());
            comboBoxEncoding.Text = Encoding.GetEncoding(1251).EncodingName;
        }

        private void buttonFileChoice_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                textBoxFileName.Text = openFileDialog.FileName;
            }
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            buttonStart.Enabled = false;
            buttonStop.Enabled = true;
            groupBoxLogSettings.Enabled = false;
            numericUpDownTimeInterval.Enabled = false;
            EncodingInfo info = encodings.First(item => item.DisplayName == comboBoxEncoding.Text);
            Encoding enc = info.GetEncoding();
            _Log = new RotateFileLogger((long)numericUpDownPartsCount.Value, (long)numericUpDownPartMaxSize.Value * 1024, enc);
            _Log.FileName = textBoxFileName.Text;
            _Log.AutoCreate = checkBoxAutoCreate.Checked;
            timerSystem.Interval = (int)numericUpDownTimeInterval.Value;
            timerSystem.Enabled = true;
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            buttonStart.Enabled = true;
            buttonStop.Enabled = false;
            groupBoxLogSettings.Enabled = true;
            numericUpDownTimeInterval.Enabled = true;
            timerSystem.Enabled = false;
            _Log = null;
        }

        private void timerSystem_Tick(object sender, EventArgs e)
        {
            if (_Log == null)
            {
                return;
            }
            int index = _Generator.Next(_Messages.Length);
            _Log.DebugPrint(_Messages[index]);
        }

        private void FormMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            Properties.Settings.Default.Save();
        }
    }
}
