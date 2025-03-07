using AlfaPribor.Logs;
using ImgAssemblingLibOpenCV.Models;
using StartTestProject.Forms;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace StartTestProject
{
    /// <summary>
    /// Скрытая форма, отвечает за логи и автосохранение\загрузк параметров
    /// </summary>
    public partial class HidenForm : Form
    {
        private RotateFileLogger _Log;
        private FileEdit fileEdit;
        private string[] fileFilter = new string[] { "*.jpeg", "*.jpg", "*.png", "*.bmp" };
        private int threadNumber = 4;
        private ThreadsNumber trN;

        public HidenForm()
        {
            InitializeComponent();
            Loading();
        }

        /// <summary>
        /// Загрузка и установка параметров
        /// </summary>
        private void Loading()
        {
            SetVisibleCore(false);
            fileEdit = new FileEdit(fileFilter);

            string save = fileEdit.AutoLoade().Trim();
            Int32.TryParse(save, out threadNumber);

            StartLoger();
            _Log.DebugPrint("Program start");

            trN = new ThreadsNumber(threadNumber);
            trN.AddErrEvent += AddErr;
            trN.AddLogEvent += AddLog;
            trN.FormClosed += ThreadsFormClosed;
            trN.Show();
        }
        /// <summary> Скрываем форму </summary>
        protected override void SetVisibleCore(bool value) => base.SetVisibleCore(false);
        /// <summary> При закрытии ThreadsForm закрываем и эту </summary>
        private void ThreadsFormClosed(object sender, FormClosedEventArgs e) => CloseForm();
        /// <summary> Запись ошибок в логи </summary>
        private void AddErr(string txt) => _Log?.DebugPrint("Err " + txt + "!!!");
        /// <summary> Запись информации в логи </summary>
        private void AddLog(string txt) => _Log?.DebugPrint(txt);
        /// <summary> Закрытии формы </summary>
        private void CloseForm()
        {
            _Log.DebugPrint("Program close");
            base.SetVisibleCore(true);

            if (trN != null) fileEdit.AutoSave(new string[] { trN.Threads.ToString() });
            this.Close();
        }
        /// <summary>
        /// Запуск логера
        /// </summary>
        private void StartLoger()
        {
            EncodingInfo[] encodings = Encoding.GetEncodings();
            EncodingInfo info = encodings.First(item => item.DisplayName == "Кириллица (Windows)");
            _Log = new RotateFileLogger(10, 1048576, info.GetEncoding());
            string logDir = fileEdit.GetDefoltDirectory() + "logs";
            if (!fileEdit.ChkDir(logDir)) return;

            _Log.FileName = logDir + Path.DirectorySeparatorChar + "LogExample.log";
            _Log.AutoCreate = true;
        }
    }
}

