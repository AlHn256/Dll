using ImgAssemblingLibOpenCV.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImgAssemblingLibOpenCV.AditionalForms
{
    //unblok
    public partial class ImgFixingForm : Form
    {
        #region Params
        
        private FixFrames FixFrames = new FixFrames();
        private FileEdit fileEdit = new FileEdit(new string[] { "*.jpeg", "*.jpg", "*.png", "*.bmp" });
        private ImgFixingSettings ImgFixingSettings = new ImgFixingSettings();
        private int rotation90 = 0, FileNumber = 0;
        private const string imgDefoltFixingFile = "imgFixingSettings.oip";
        private string ImgFixingPlan = imgDefoltFixingFile;
        private bool SaveRezultToFile;
        private string SavingRezultDir;
        public event Action<int> ProcessChanged;
        private Bitmap LoadedBitmapFrame { get; set; }
        public bool IsErr { get; set; } = false;
        public static bool StopProcess = false;
        public string ErrText { get; set; } = string.Empty;
        private List<string> FileList = new List<string>();
        private int MidleWidthPoint { get; set; } = 0;
        private int MidleHeightPoint { get; set; } = 0;
        private bool diminshIn = false;
        private readonly List<bool> dimList = new List<bool>() { false, false, false };
        private string prevDir = string.Empty;
        #endregion
        /// <summary>
        /// Конструктор для стандартной работы с окном и файлами 
        /// </summary>
        /// <param name="directory">Папка с кадрами (.jpeg, .bmp)</param>
        public ImgFixingForm(string directory)
        {
            InitializeComponent();
            InputDirTxtBox.Text = directory;
            Load += OnLoad;
        }

        /// <summary>
        /// Конструктор для работы с дополнительным окном
        /// </summary>
        /// <param name="imgFixingPlan">Файл с параметрами настройки</param>
        /// <param name="directory">Папка с кадрами</param>
        public ImgFixingForm(string imgFixingPlan, string directory)
        {
            InitializeComponent();
            if (!string.IsNullOrEmpty(imgFixingPlan)) ImgFixingPlan = imgFixingPlan;
            InputDirTxtBox.Text = directory;
            Load += OnLoad;
        }

        /// <summary>
        /// Конструктор для работы с Bitmapами
        /// </summary>
        /// <param name="imgFixingPlan"></param>
        /// <param name="saveRezultToFile"></param>
        /// <param name="fixingImgDirectory"></param>
        //public ImgFixingForm(string imgFixingPlan, bool saveRezultToFile = false, string fixingImgDirectory = "")
        //{
        //    InitializeComponent();
        //    if (!string.IsNullOrEmpty(imgFixingPlan))
        //    {
        //        ImgFixingPlan = imgFixingPlan;
        //        TryReadSettings();
        //        SaveRezultToFile = saveRezultToFile;
        //        SavingRezultDir = fixingImgDirectory;
        //    }
        //}
        // Тестовая версия конструктора
        public ImgFixingForm(string imgFixingPlan, bool test = false)
        {
            InitializeComponent();
            if (!string.IsNullOrEmpty(imgFixingPlan)) ImgFixingPlan = imgFixingPlan;
            TryReadSettings();
        }

        /// <summary>
        /// Установка ошибки
        /// </summary>
        /// <param name="errText">Текст ошибки</param>
        /// <returns></returns>
        private bool SetErr(string errText)
        {
            IsErr = true;
            ErrText = errText;
            RezultRTB.Text = errText;
            return false;
        }
        public void OnProgressChanged(object i)
        {
            if (ProcessChanged != null) ProcessChanged((int)i);
        }
        public void OnTextChanged(object txt)
        {
            //if (TextChanged != null) TextChanged((string)txt);
        }
        private void WindowsForm_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }
        private void WindowsForm_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files == null) return;
            if (files.Length == 0) return;
            if (files.Length == 1) InputDirTxtBox.Text = Path.GetDirectoryName(files[0]);
            FixFrames.ChangeFirstFrame(files[0]);
            InputFileTxtBox.Text = Path.GetFileName(files[0]);
        }
        /// <summary> Загрузка кадров плюс параметров для их исправления</summary>
        private void OnLoad(object sender, EventArgs e)
        {
            AllowDrop = true;
            DragEnter += new DragEventHandler(WindowsForm_DragEnter);
            DragDrop += new DragEventHandler(WindowsForm_DragDrop);
            pictureBox1.AllowDrop = true;
            pictureBox1.DragEnter += new DragEventHandler(WindowsForm_DragEnter);
            pictureBox1.DragDrop += new DragEventHandler(WindowsForm_DragDrop);

            ZoomLbl.Text = ImgFixingSettings.Zoom.ToString();
            this.MouseWheel += panel1_MouseWheel;

            if (!string.IsNullOrEmpty(ImgFixingPlan)) TryReadSettings();
            else
            {
                ZoomLbl.Text = ImgFixingSettings.Zoom.ToString();
                ATxtBox.Text = ImgFixingSettings.DistorSettings.A.ToString();
                BTxtBox.Text = ImgFixingSettings.DistorSettings.B.ToString();
                CTxtBox.Text = ImgFixingSettings.DistorSettings.C.ToString();
                DTxtBox.Text = ImgFixingSettings.DistorSettings.D.ToString();
                ETxtBox.Text = ImgFixingSettings.DistorSettings.E.ToString();
                Sm11TxtBox.Text = ImgFixingSettings.DistorSettings.Sm11.ToString();
                Sm12TxtBox.Text = ImgFixingSettings.DistorSettings.Sm12.ToString();
                Sm13TxtBox.Text = ImgFixingSettings.DistorSettings.Sm13.ToString();
                Sm21TxtBox.Text = ImgFixingSettings.DistorSettings.Sm21.ToString();
                Sm22TxtBox.Text = ImgFixingSettings.DistorSettings.Sm22.ToString();
                Sm23TxtBox.Text = ImgFixingSettings.DistorSettings.Sm23.ToString();
                Sm31TxtBox.Text = ImgFixingSettings.DistorSettings.Sm31.ToString();
                Sm32TxtBox.Text = ImgFixingSettings.DistorSettings.Sm32.ToString();
                Sm33TxtBox.Text = ImgFixingSettings.DistorSettings.Sm33.ToString();
            }

            AutoReloadChkBox.Checked = true;
            BlockOn();
            OpenCvReloadImg();
        }
        /// <summary> Блокировка \ разблокировка доп параметров настройки дисторсии</summary>
        private void BlockOn() => Block(false);
        private void UnBlock() => Block(true);
        private void Block(bool block)
        {
            Sm11TxtBox.Enabled = block;
            Sm12TxtBox.Enabled = block;
            Sm13TxtBox.Enabled = block;
            Sm21TxtBox.Enabled = block;
            Sm22TxtBox.Enabled = block;
            Sm23TxtBox.Enabled = block;
            Sm31TxtBox.Enabled = block;
            Sm32TxtBox.Enabled = block;
            Sm33TxtBox.Enabled = block;
            BBtnDn.Enabled = block;
            BBtnUp.Enabled = block;
            BTxtBox.Enabled = block;
            CBtnDn.Enabled = block;
            CBtnUp.Enabled = block;
            CTxtBox.Enabled = block;
            DBtnDn.Enabled = block;
            DBtnUp.Enabled = block;
            DTxtBox.Enabled = block;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Z) DistZero();
            else if (keyData == Keys.NumPad7 || keyData == Keys.Home) ChangeA(false);
            else if (keyData == Keys.NumPad9 || keyData == Keys.PageUp) ChangeA(true);
            else if (keyData == Keys.NumPad8 || keyData == Keys.Up) ChangeC(true);
            else if (keyData == Keys.NumPad2 || keyData == Keys.NumPad5 || keyData == Keys.Down) ChangeC(false);
            else if (keyData == Keys.NumPad4) ChangeD(true);
            else if (keyData == Keys.NumPad6) ChangeD(false);
            else if (keyData == Keys.NumPad1 || keyData == Keys.End) Rotation(-10);
            else if (keyData == Keys.NumPad3 || keyData == Keys.PageDown) Rotation(10);
            else if (keyData == Keys.Add) ChangZoom(false);
            else if (keyData == Keys.Subtract) ChangZoom(true);
            else if (keyData == Keys.Right || keyData == Keys.Left)
            {
                if (FileList.Count == 0) return false;
                if (keyData == Keys.Right) FileNumber++;
                if (keyData == Keys.Left) FileNumber--;
                if (FileNumber < 0) FileNumber = FileList.Count - 1;
                if (FileNumber > FileList.Count - 1) FileNumber = 0;
                InputDirTxtBox.Text = Path.GetDirectoryName(FileList[FileNumber]);
                InputFileTxtBox.Text = Path.GetFileName(FileList[FileNumber]);
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        /// <summary>
        /// Разблокировка дополнительных параметров
        /// </summary>
        private void RezultRTB_TextChanged(object sender, EventArgs e)
        {
            if (RezultRTB.Text.IndexOf("unblok") != -1 || RezultRTB.Text.IndexOf("Unblok") != -1 ||
                RezultRTB.Text.IndexOf("гтидщл") != -1 || RezultRTB.Text.IndexOf("Гтидщл") != -1) UnBlock();
        }

        /// <summary>
        /// Загрузка списка изображений для редактирования
        /// </summary>
        /// <param name="serchingDir">Папка с изображениями</param>
        /// <returns></returns>
        private FileInfo[] LoadFileList(string serchingDir = null)
        {
            FileInfo[] list = new FileInfo[0];
            if (string.IsNullOrEmpty(serchingDir) && string.IsNullOrEmpty(InputDirTxtBox.Text))
            {
                SetErr("InputDirTxtBox не заполнен!!!");
                return list;
            }
            if (string.IsNullOrEmpty(serchingDir)) serchingDir = InputDirTxtBox.Text;
            if (!Directory.Exists(serchingDir))
            {
                SetErr("Такой папки " + serchingDir + " не существует");
                return list;
            }
            list = fileEdit.SearchFiles(serchingDir);
            if (list.Length == 0)
            {
                SetErr("Файлы для редактирования не найдены");
                FileList.Clear();
                return list;
            }
            
            FileList = list.Select(f => f.FullName).ToList();
            return list;
        }

        private void ABtnUp_Click(object sender, EventArgs e) => ChangeA(true);
        private void ABtnDn_Click(object sender, EventArgs e) => ChangeA(false);
        private void ChangeA(bool increase)
        {
            double.TryParse(ATxtBox.Text, out double A);
            if (increase) ImgFixingSettings.DistorSettings.A = Math.Round(A + 0.01, 2);
            else ImgFixingSettings.DistorSettings.A = Math.Round(A - 0.01, 2);

            if(ATxtBox.Text == ImgFixingSettings.DistorSettings.A.ToString())
            {
                if (AutoReloadChkBox.Checked) OpenCvReloadImg();
            }
            else ATxtBox.Text = ImgFixingSettings.DistorSettings.A.ToString();
        }
        private void CBtnUp_Click(object sender, EventArgs e) => ChangeC(true);
        private void CBtnDn_Click(object sender, EventArgs e) => ChangeC(false);
        private void ChangeC(bool increase)
        {
            double.TryParse(CTxtBox.Text, out double C);
            if (increase) ImgFixingSettings.DistorSettings.C = Math.Round(C + 0.01, 2);
            else ImgFixingSettings.DistorSettings.C = Math.Round(C - 0.01, 2);
            CTxtBox.Text = ImgFixingSettings.DistorSettings.C.ToString();
            if (AutoReloadChkBox.Checked) OpenCvReloadImg();
        }
        private void DBtnUp_Click(object sender, EventArgs e) => ChangeD(true);
        private void DBtnDn_Click(object sender, EventArgs e) => ChangeD(false);
        private void ChangeD(bool increase)
        {
            double D = 0;
            double.TryParse(DTxtBox.Text, out D);
            if (increase) ImgFixingSettings.DistorSettings.D = Math.Round(D + 0.01, 2);
            else ImgFixingSettings.DistorSettings.D = Math.Round(D - 0.01, 2);
            DTxtBox.Text = ImgFixingSettings.DistorSettings.D.ToString();
            if (AutoReloadChkBox.Checked) OpenCvReloadImg();
        }
        private void CorrectFiles_Click(object sender, EventArgs e)
        {

            if (!fileEdit.ChkDir(OutputDirTxtBox.Text))
            {
                SetErr("Err CorrectFiles.Directory.Exists(InputDirTxtBox.Text)!!!");
                RezultRTB.Text = ErrText;
                return;
            }

            ShowGridСhckBox.Checked = false;

            //if (!Directory.Exists(OutputDirTxtBox.Text)) return;
            //var sdf = FileList;
            //FileInfo[] fileList = fileEdit.SearchFiles(InputDirTxtBox.Text);
            //foreach (var file in fileList)
            //{
            //    string outputFileNumber = OutputDirTxtBox.Text + Path.DirectorySeparatorChar + file.Name;
            //    FixFrames.EditImg(file.FullName).Save(outputFileNumber);
            //}

            foreach (var file in FileList)
            {
                string outputFileNumber = OutputDirTxtBox.Text + Path.DirectorySeparatorChar + Path.GetFileName(file);
                FixFrames.EditImg(file).Save(outputFileNumber);
            }
        }


        //public bool FixImges(object param, string outputDir = "")
        //{
        //    SynchronizationContext context = (SynchronizationContext)param;
        //    ShowGridСhckBox.Checked = false;
        //    if (string.IsNullOrEmpty(outputDir))
        //    {
        //        if (!Directory.Exists(InputDirTxtBox.Text)) return false;
        //        outputDir = OutputDirTxtBox.Text;
        //    }

        //    if (!fileEdit.ChkDir(outputDir)) return false;
        //    FileInfo[] fileList = fileEdit.SearchFiles(InputDirTxtBox.Text);

        //    if (fileList == null) return SetErr("ERR FixImges.fileList == null !!!");
        //    for (int i = 0; i < fileList.Length; i++)
        //    {
        //        string outputFileNumber = outputDir + Path.DirectorySeparatorChar + fileList[i].Name;
        //        FixFrames.EditImg(fileList[i].FullName).Save(outputFileNumber);
        //        //EditImg(fileList[i].FullName).SaveImage(outputFileNumber);
        //        context.Send(OnProgressChanged, i * 100 / fileList.Length);
        //        context.Send(OnTextChanged, "Imges Fixing " + i * 100 / fileList.Length + " %");
        //    }

        //    context.Send(OnProgressChanged, 100);
        //    context.Send(OnTextChanged, "Imges Fixing 100 %");
        //    return IsErr;
        //}


        /// <summary>
        /// Исправлени кадров
        /// </summary>
        /// <param name="param">Параметр синхронизации</param>
        /// <param name="dataArray">Набор кадров для коррекции</param>
        /// <returns></returns>
        public Bitmap[] FixImges(object param, Bitmap[] dataArray)
        {
            if (dataArray == null || dataArray.Length == 0)
            {
                SetErr("ERR FixImges.fileList == null !!!");
                return null;
            }

            bool sinchoniztioIsOn = param == null ? false : true;
            SynchronizationContext context = (SynchronizationContext)param;
            ShowGridСhckBox.Checked = false;
            bool fileSaving = false;
            if (SaveRezultToFile)
            {
                if (fileEdit.ChkDir(SavingRezultDir)) fileSaving = true;
                else SetErr($"ERR FixImges.!Directory.Exists({SavingRezultDir}) !!!");
            }

            List<Bitmap> bitMapList = new List<Bitmap>();
            for (int i = 0; i < dataArray.Length; i++)
            {
                Bitmap img = FixFrames.EditImg(dataArray[i]);
                bitMapList.Add(img);

                if (SaveRezultToFile)
                {
                    string file = i < 10 ? fileEdit.DirFile(SavingRezultDir, "0" + i + ".bmp") : fileEdit.DirFile(SavingRezultDir, i + ".bmp");
                    img.Save(file);
                }

                if (sinchoniztioIsOn)
                {
                    context.Send(OnProgressChanged, i * 100 / dataArray.Length);
                    context.Send(OnTextChanged, "Imges Fixing " + i * 100 / dataArray.Length + " %");
                }
            }
            if (sinchoniztioIsOn)
            {
                context.Send(OnProgressChanged, 100);
                context.Send(OnTextChanged, "Imges Fixing 100 %");
            }
            return bitMapList.ToArray();
        }
        //public bool CheckFixingImg(string imgFixingDir = "")
        //{
        //    if (string.IsNullOrEmpty(imgFixingDir)) imgFixingDir = OutputDirTxtBox.Text;
        //    if (!Directory.Exists(imgFixingDir)) return false;

        //    FileInfo[] fileList = fileEdit.SearchFiles(InputDirTxtBox.Text);
        //    for (int i = 0; i < fileList.Length; i++)
        //        if (!File.Exists(imgFixingDir + Path.DirectorySeparatorChar + fileList[i].Name)) return false;
        //    return true;
        //}
        private void panel1_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0) ChangZoom(true);
            else ChangZoom(false);
        }
        private void MZoomBtn_Click(object sender, EventArgs e) => ChangZoom(false);
        private void PZoomBtn_Click(object sender, EventArgs e) => ChangZoom(true);
        private void ChangZoom(bool increase)
        {
            if (increase) ImgFixingSettings.Zoom += 0.01;
            else
            {
                ImgFixingSettings.Zoom -= 0.01;
                if (ImgFixingSettings.Zoom < 1) ImgFixingSettings.Zoom = 1;
            }

            bool AutoReloadSave = AutoReloadChkBox.Checked;
            AutoReloadChkBox.Checked = false;
            ZoomLbl.Text = ImgFixingSettings.Zoom.ToString();
            ZeroCropAfter();
            SetSm13Sm23();
            AutoReloadChkBox.Checked = AutoReloadSave;
            if (AutoReloadChkBox.Checked) OpenCvReloadImg();
        }
        private void InputDirTxtBox_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(InputDirTxtBox.Text) || !Directory.Exists(InputDirTxtBox.Text)) return;
            //if (!Directory.Exists(InputDirTxtBox.Text)) return;

            if (prevDir == InputDirTxtBox.Text) return;
            prevDir = InputDirTxtBox.Text;

            LoadFileList();
            //fileEdit.SearchFiles(InputDirTxtBox.Text);
            //FileList = files.Select(f => f.FullName).ToList();
            //FileList = fileEdit.SearchFiles(InputDirTxtBox.Text).ToList();
            OutputDirTxtBox.Text = InputDirTxtBox.Text.FirstOf(Path.DirectorySeparatorChar.ToString()) + Path.DirectorySeparatorChar + InputDirTxtBox.Text.LastOf('\\') + "Out";

            if (FileList == null) return;
            if (FileList[0] == null) return;
            
            InputFileTxtBox.Text = Path.GetFileName(FileList[0]);
            //pictureBox1.BackgroundImage = Image.FromFile(files[0].FullName);
            //if (AutoReloadChkBox.Checked)
            //OpenCvReloadImg();
        }

        
        private void InputFileTxtBox_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(InputDirTxtBox.Text) || string.IsNullOrEmpty(InputFileTxtBox.Text) || FixFrames == null) return;
            string file = fileEdit.DirFile(InputDirTxtBox.Text, InputFileTxtBox.Text);
            if (File.Exists(file))
            {
                FixFrames.ChangeFirstFrame(file);
                OpenCvReloadImg();
            }
            else SetErr("File: " + file + " не найден");
        }

        private void ATxtBox_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ATxtBox.Text)) return;
            double.TryParse(ATxtBox.Text, out double A);
            ImgFixingSettings.DistorSettings.A = Math.Round(A, 2);
            ATxtBox.Text = ImgFixingSettings.DistorSettings.A.ToString();
            if (AutoReloadChkBox.Checked) OpenCvReloadImg();
        }

        private void BTxtBox_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(BTxtBox.Text)) return;
            double.TryParse(BTxtBox.Text, out double B);
            ImgFixingSettings.DistorSettings.B = Math.Round(B, 2);
            BTxtBox.Text = ImgFixingSettings.DistorSettings.B.ToString();
            if (AutoReloadChkBox.Checked) OpenCvReloadImg();
        }

        private void CTxtBox_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(CTxtBox.Text)) return;
            double.TryParse(CTxtBox.Text, out double C);
            ImgFixingSettings.DistorSettings.C = Math.Round(C, 2);
            CTxtBox.Text = ImgFixingSettings.DistorSettings.C.ToString();
            if (AutoReloadChkBox.Checked) OpenCvReloadImg();
        }

        private void DTxtBox_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(DTxtBox.Text)) return;
            double.TryParse(DTxtBox.Text, out double D);
            ImgFixingSettings.DistorSettings.D = Math.Round(D, 2);
            DTxtBox.Text = ImgFixingSettings.DistorSettings.D.ToString();
            if (AutoReloadChkBox.Checked) OpenCvReloadImg();
        }

        private void ETxtBox_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(BTxtBox.Text)) return;
            double.TryParse(ETxtBox.Text, out double E);
            ImgFixingSettings.DistorSettings.E = Math.Round(E, 2);
            ETxtBox.Text = ImgFixingSettings.DistorSettings.E.ToString();
            if (AutoReloadChkBox.Checked) OpenCvReloadImg();
        }

        private void Sm11TxtBox_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Sm11TxtBox.Text)) return;
            double.TryParse(Sm11TxtBox.Text, out double Sm11);
            ImgFixingSettings.DistorSettings.Sm11 = Math.Round(Sm11, 3);
            Sm11TxtBox.Text = ImgFixingSettings.DistorSettings.Sm11.ToString();
            if (AutoReloadChkBox.Checked) OpenCvReloadImg();
        }
        private void Sm12TxtBox_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Sm12TxtBox.Text)) return;
            Double.TryParse(Sm12TxtBox.Text, out double Sm12);
            ImgFixingSettings.DistorSettings.Sm12 = Math.Round(Sm12, 3);
            Sm12TxtBox.Text = ImgFixingSettings.DistorSettings.Sm12.ToString();
            if (AutoReloadChkBox.Checked) OpenCvReloadImg();
        }
        private void Sm13TxtBox_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Sm13TxtBox.Text)) return;
            Double.TryParse(Sm13TxtBox.Text, out double Sm13);
            ImgFixingSettings.DistorSettings.Sm13 = Math.Round(Convert.ToDouble(Sm13TxtBox.Text), 3);
            Sm13TxtBox.Text = ImgFixingSettings.DistorSettings.Sm13.ToString();
            if (AutoReloadChkBox.Checked) OpenCvReloadImg();
        }
        private void Sm21TxtBox_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Sm21TxtBox.Text)) return;
            Double.TryParse(Sm21TxtBox.Text, out double Sm21);
            ImgFixingSettings.DistorSettings.Sm21 = Math.Round(Convert.ToDouble(Sm21TxtBox.Text), 3);
            Sm21TxtBox.Text = ImgFixingSettings.DistorSettings.Sm21.ToString();
            if (AutoReloadChkBox.Checked) OpenCvReloadImg();
        }

        private void Sm22TxtBox_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Sm22TxtBox.Text)) return;
            Double.TryParse(Sm22TxtBox.Text, out double Sm22);
            ImgFixingSettings.DistorSettings.Sm22 = Math.Round(Convert.ToDouble(Sm22TxtBox.Text), 3);
            Sm22TxtBox.Text = ImgFixingSettings.DistorSettings.Sm22.ToString();
            if (AutoReloadChkBox.Checked) OpenCvReloadImg();
        }

        private void Sm23TxtBox_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Sm23TxtBox.Text)) return;
            Double.TryParse(Sm23TxtBox.Text, out double Sm23);
            ImgFixingSettings.DistorSettings.Sm23 = Math.Round(Convert.ToDouble(Sm23TxtBox.Text), 3);
            Sm23TxtBox.Text = ImgFixingSettings.DistorSettings.Sm23.ToString();
            if (AutoReloadChkBox.Checked) OpenCvReloadImg();
        }

        private void Sm31TxtBox_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Sm31TxtBox.Text)) return;
            Double.TryParse(Sm31TxtBox.Text, out double Sm31);
            ImgFixingSettings.DistorSettings.Sm31 = Math.Round(Convert.ToDouble(Sm31TxtBox.Text), 3);
            Sm31TxtBox.Text = ImgFixingSettings.DistorSettings.Sm31.ToString();
            if (AutoReloadChkBox.Checked) OpenCvReloadImg();
        }

        private void Sm32TxtBox_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Sm32TxtBox.Text)) return;
            Double.TryParse(Sm32TxtBox.Text, out double Sm32);
            ImgFixingSettings.DistorSettings.Sm32 = Math.Round(Convert.ToDouble(Sm32TxtBox.Text), 3);
            Sm32TxtBox.Text = ImgFixingSettings.DistorSettings.Sm32.ToString();
            if (AutoReloadChkBox.Checked) OpenCvReloadImg();
        }

        private void Sm33TxtBox_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Sm33TxtBox.Text)) return;
            Double.TryParse(Sm33TxtBox.Text, out double Sm33);
            ImgFixingSettings.DistorSettings.Sm33 = Math.Round(Convert.ToDouble(Sm33TxtBox.Text), 3);
            Sm33TxtBox.Text = ImgFixingSettings.DistorSettings.Sm33.ToString();
            if (AutoReloadChkBox.Checked) OpenCvReloadImg();
        }

        private void BBtnUp_Click(object sender, EventArgs e)
        {
            double.TryParse(BTxtBox.Text, out double B);
            ImgFixingSettings.DistorSettings.B = Math.Round(B + 0.01, 2);
            BTxtBox.Text = ImgFixingSettings.DistorSettings.B.ToString();
            if (AutoReloadChkBox.Checked) OpenCvReloadImg();
        }

        private void BBtnDn_Click(object sender, EventArgs e)
        {
            double.TryParse(BTxtBox.Text, out double B);
            ImgFixingSettings.DistorSettings.B = Math.Round(B - 0.01, 2);
            BTxtBox.Text = ImgFixingSettings.DistorSettings.B.ToString();
            if (AutoReloadChkBox.Checked) OpenCvReloadImg();
        }

        private void EBtnDn_Click(object sender, EventArgs e)
        {
            double.TryParse(ETxtBox.Text, out double E);
            ImgFixingSettings.DistorSettings.E = Math.Round(E - 0.01, 2);
            ETxtBox.Text = ImgFixingSettings.DistorSettings.E.ToString();
            if (AutoReloadChkBox.Checked) OpenCvReloadImg();
        }

        private void EBtnUp_Click(object sender, EventArgs e)
        {
            double.TryParse(ETxtBox.Text, out double E);
            ImgFixingSettings.DistorSettings.E = Math.Round(E + 0.01, 2);
            ETxtBox.Text = ImgFixingSettings.DistorSettings.E.ToString();
            if (AutoReloadChkBox.Checked) OpenCvReloadImg();
        }

        //private void EBtnUp_Click(object sender, EventArgs e)
        //{
        //    double.TryParse(ETxtBox.Text, out E);
        //    E = Math.Round(E + 0.01, 2);
        //    ETxtBox.Text = E.ToString();
        //    if (AutoReloadChkBox.Checked) OpenCvReloadImg();
        //}
        //private void EBtnDn_Click(object sender, EventArgs e)
        //{
        //    double.TryParse(ETxtBox.Text, out E);
        //    E = Math.Round(E - 0.01, 2);
        //    ETxtBox.Text = E.ToString();
        //    if (AutoReloadChkBox.Checked) OpenCvReloadImg();
        //}

        /// <summary> Сброс настроек предварительной обрезки кадра </summary>
        private void ZeroCropBeforBtn_Click(object sender, EventArgs e)
        {
            bool AutoReloadSave = AutoReloadChkBox.Checked;
            AutoReloadChkBox.Checked = false;

            if (LoadedBitmapFrame == null)
            {
                XBeforTxtBox.Text = "0";
                dXBeforTxtBox.Text = "0";
                YBeforTxtBox.Text = "0";
                dYBeforTxtBox.Text = "0";
            }
            else
            {
                XBeforTxtBox.Text = "0";
                dXBeforTxtBox.Text = LoadedBitmapFrame.Width.ToString();
                YBeforTxtBox.Text = "0";
                dYBeforTxtBox.Text = LoadedBitmapFrame.Height.ToString();
            }

            AutoReloadChkBox.Checked = AutoReloadSave;
            if (AutoReloadChkBox.Checked) OpenCvReloadImg();

            ZeroCropAfter();
        }

        private void DistZeroBtn_Click(object sender, EventArgs e) => DistZero();

        /// <summary>Сброс настроек дисторсии </summary>
        private void DistZero()
        {
            bool AutoReloadSave = AutoReloadChkBox.Checked;
            AutoReloadChkBox.Checked = false;

            //ImgFixingSettings.Zoom = 1;
            //ZoomLbl.Text = "1";

            ImgFixingSettings.DistorSettings.A = 0;
            ImgFixingSettings.DistorSettings.B = 0;
            ImgFixingSettings.DistorSettings.C = 0;
            ImgFixingSettings.DistorSettings.D = 0;
            ImgFixingSettings.DistorSettings.E = 0;

            ATxtBox.Text = ImgFixingSettings.DistorSettings.A.ToString();
            BTxtBox.Text = ImgFixingSettings.DistorSettings.B.ToString();
            CTxtBox.Text = ImgFixingSettings.DistorSettings.C.ToString();
            DTxtBox.Text = ImgFixingSettings.DistorSettings.D.ToString();
            ETxtBox.Text = ImgFixingSettings.DistorSettings.E.ToString();

            SetSm13Sm23();
            ImgFixingSettings.DistorSettings.Sm11 = 1500;
            ImgFixingSettings.DistorSettings.Sm22 = 1500;
            ImgFixingSettings.DistorSettings.Sm12 = 0;
            ImgFixingSettings.DistorSettings.Sm21 = 0;
            ImgFixingSettings.DistorSettings.Sm31 = 0;
            ImgFixingSettings.DistorSettings.Sm32 = 0;
            ImgFixingSettings.DistorSettings.Sm33 = 1;

            Sm11TxtBox.Text = ImgFixingSettings.DistorSettings.Sm11.ToString();
            Sm22TxtBox.Text = ImgFixingSettings.DistorSettings.Sm22.ToString();
            Sm12TxtBox.Text = ImgFixingSettings.DistorSettings.Sm12.ToString();
            Sm21TxtBox.Text = ImgFixingSettings.DistorSettings.Sm21.ToString();
            Sm31TxtBox.Text = ImgFixingSettings.DistorSettings.Sm31.ToString();
            Sm32TxtBox.Text = ImgFixingSettings.DistorSettings.Sm32.ToString();
            Sm33TxtBox.Text = ImgFixingSettings.DistorSettings.Sm33.ToString();

            AutoReloadChkBox.Checked = AutoReloadSave;
            ZeroCropAfter();
        }
        private void SetSm13Sm23()
        {
            bool AutoReloadSave = AutoReloadChkBox.Checked;
            AutoReloadChkBox.Checked = false;
            ImgFixingSettings.DistorSettings.Sm13 = MidleWidthPoint * ImgFixingSettings.Zoom;
            ImgFixingSettings.DistorSettings.Sm23 = MidleHeightPoint * ImgFixingSettings.Zoom;

            Sm13TxtBox.Text = ImgFixingSettings.DistorSettings.Sm13.ToString();
            Sm23TxtBox.Text = ImgFixingSettings.DistorSettings.Sm23.ToString();
            AutoReloadChkBox.Checked = AutoReloadSave;
        }
        private void ZeroCropAfterBtn_Click(object sender, EventArgs e) => ZeroCropAfter();

        /// <summary>
        /// Сброс настроек обрезки кадра после всех других обработок
        /// </summary>
        private void ZeroCropAfter()
        {
            bool AutoReloadSave = AutoReloadChkBox.Checked;
            AutoReloadChkBox.Checked = false;
            XAfterTxtBox.Text = "0";
            YAfterTxtBox.Text = "0";
            dXAfterTxtBox.Text = "0";
            dYAfterTxtBox.Text = "0";
            AutoReloadChkBox.Checked = AutoReloadSave;
            if (AutoReloadChkBox.Checked) OpenCvReloadImg();
        }

        private async void SaveAsBtn_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = fileEdit.GetDefoltDirectory();
            saveFileDialog.Filter = "Fixing img plan (*.oip)|*.oip|All files(*.*)|*.*";
            saveFileDialog.FilterIndex = 1;
            if (saveFileDialog.ShowDialog() == DialogResult.Cancel) return;
            await SaveSetting(saveFileDialog.FileName);
        }

        private async Task<bool> SaveSetting(string file)
        {
            try
            {
                UpdateSettings();
                ImgFixingSettings.ShowGrid = false;
                if (await fileEdit.SaveJsonAsync(file, ImgFixingSettings, true)) RezultRTB.Text = "Settings save in " + file;
                ImgFixingPlan = file;
            }
            catch (Exception ex)
            {
                RezultRTB.Text = ex.Message;
                return false;
            }

            return true;
        }

        private void LoadFrBtn_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = fileEdit.GetDefoltDirectory();
                openFileDialog.Filter = "Fixing img plan (*.oip)|*.oip|All files(*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    ImgFixingPlan = openFileDialog.FileName;
                    TryReadSettings(openFileDialog.FileName);
                    //if (!string.IsNullOrEmpty(ImgFixingPlan)) SaveSetting(ImgFixingPlan);
                    //var sdf = ImgFixingSettings;
                    //UpdateForm();
                    OpenCvReloadImg(true);
                }
            }
        }

        /// <summary>
        /// Пробуем прочитать настройки из файла
        /// </summary>
        /// <param name="loadingFile">Файл с настройками</param>
        public void TryReadSettings(string loadingFile = null)
        {
            if (File.Exists(ImgFixingPlan) || File.Exists(loadingFile))
            {
                if (string.IsNullOrEmpty(loadingFile)) loadingFile = ImgFixingPlan;
                else if (!File.Exists(loadingFile)) loadingFile = ImgFixingPlan;
            }
            else SetErr("Err Файл загрузки не найден!!!\n Загруженны настройки поумолчанию.");

            if (File.Exists(loadingFile))
            {
                try
                {
                    fileEdit.LoadeJson(loadingFile, out ImgFixingSettings);
                    ImgFixingSettings.ShowGrid = false;
                    SetImgFixingSettings(ImgFixingSettings);
                }
                catch (IOException e)
                {
                    SetErr("The file could not be read: " + e.Message + "!!!\n");
                }
            }
            else SetErr("Err TryReadSettings.файл загрузки не найден!!!\n Загруженны настройки поумолчанию.");
        }
        public void SetImgFixingSettings(ImgFixingSettings imgFixingSettings)
        {
            
            AutoReloadChkBox.Checked = false;

            SetImgFixingSettingsWD(imgFixingSettings);
            DistChkBox.Checked = imgFixingSettings.Distortion;

            if (imgFixingSettings.DistorSettings == null)
            {
                DistZero();
                AutoReloadChkBox.Checked = imgFixingSettings.AutoReload;
                return;
            }

            DistorSettings distorSettings = imgFixingSettings.DistorSettings;
            ATxtBox.Text = distorSettings.A.ToString();
            BTxtBox.Text = distorSettings.B.ToString();
            CTxtBox.Text = distorSettings.C.ToString();
            DTxtBox.Text = distorSettings.D.ToString();
            ETxtBox.Text = distorSettings.E.ToString();
            Sm11TxtBox.Text = distorSettings.Sm11.ToString();
            Sm12TxtBox.Text = distorSettings.Sm12.ToString();
            Sm13TxtBox.Text = distorSettings.Sm13.ToString();
            Sm21TxtBox.Text = distorSettings.Sm21.ToString();
            Sm22TxtBox.Text = distorSettings.Sm22.ToString();
            Sm23TxtBox.Text = distorSettings.Sm23.ToString();
            Sm31TxtBox.Text = distorSettings.Sm31.ToString();
            Sm32TxtBox.Text = distorSettings.Sm32.ToString();
            Sm33TxtBox.Text = distorSettings.Sm33.ToString();

            AutoReloadChkBox.Checked = imgFixingSettings.AutoReload;
        }
       
        /// <summary>
        /// обновление настроек без дисторсии
        /// </summary>
        /// <param name="imgFixingSettings"></param>
        private void SetImgFixingSettingsWD(ImgFixingSettings imgFixingSettings)
        {
            bool AutoReloadSave = AutoReloadChkBox.Checked;
            AutoReloadChkBox.Checked = false;
            DistChkBox.Checked = false;

            if (imgFixingSettings.Zoom < 1) imgFixingSettings.Zoom = 1;
            ZoomLbl.Text = imgFixingSettings.Zoom.ToString();
            rotation90 = imgFixingSettings.Rotation90;
            BlackWhiteChkBox.Checked = imgFixingSettings.BlackWhiteMode;

            CropBeforChkBox.Checked = imgFixingSettings.CropBeforChkBox;
            XBeforTxtBox.Text = imgFixingSettings.XBefor.ToString();
            YBeforTxtBox.Text = imgFixingSettings.YBefor.ToString();
            dXBeforTxtBox.Text = imgFixingSettings.DXBefor.ToString();
            dYBeforTxtBox.Text = imgFixingSettings.DYBefor.ToString();

            CropAfterChkBox.Checked = imgFixingSettings.CropAfterChkBox;
            XAfterTxtBox.Text = imgFixingSettings.XAfter.ToString();
            YAfterTxtBox.Text = imgFixingSettings.YAfter.ToString();
            dXAfterTxtBox.Text = imgFixingSettings.DXAfter.ToString();
            dYAfterTxtBox.Text = imgFixingSettings.DYAfter.ToString();
            AutoReloadChkBox.Checked = AutoReloadSave;
        }

        private void RBtnUpDn_Click(object sender, EventArgs e) { Rotation90(false); ZeroCropAfter(); SetSm13Sm23(); }
        private void RBtnUp90_Click(object sender, EventArgs e) { Rotation90(true); ZeroCropAfter(); SetSm13Sm23(); }
        private void RBtnUp001_Click(object sender, EventArgs e) => Rotation(100);
        private void RBtnDn001_Click(object sender, EventArgs e) => Rotation(-100);
        private void RBtnUp01_Click(object sender, EventArgs e) => Rotation(10);
        private void RBtnDn01_Click(object sender, EventArgs e) => Rotation(-10);

        private async void ImgFixingForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!string.IsNullOrEmpty(ImgFixingPlan)) await SaveSetting(ImgFixingPlan);
        }

        private void BlackWhiteChkBox_CheckedChanged(object sender, EventArgs e)
        {
            ImgFixingSettings.BlackWhiteMode = BlackWhiteChkBox.Checked;
            if (AutoReloadChkBox.Checked) OpenCvReloadImg();
        }

        private void Rotation90(bool direction)
        {
            if (direction) rotation90++;
            else rotation90--;
            if (rotation90 < 0) rotation90 = 3;
            if (rotation90 > 3) rotation90 = 0;
            ImgFixingSettings.Rotation90 = rotation90;
            if (AutoReloadChkBox.Checked) OpenCvReloadImg();
        }

        private void Rotation(int N)
        {
            bool AutoReloadSave = AutoReloadChkBox.Checked; // Нужно для того что бы картинки лишний раз не ребуталась
            AutoReloadChkBox.Checked = false;
            Sm21TxtBox.Text = (ImgFixingSettings.DistorSettings.Sm21 + N).ToString();
            Sm12TxtBox.Text = (ImgFixingSettings.DistorSettings.Sm12 - N).ToString();
            AutoReloadChkBox.Checked = AutoReloadSave;
            if (AutoReloadChkBox.Checked) OpenCvReloadImg();
        }

        private void ShowGridСhckBox_CheckedChanged(object sender, EventArgs e)
        {
            ImgFixingSettings.ShowGrid = ShowGridСhckBox.Checked;
            if (AutoReloadChkBox.Checked) OpenCvReloadImg();
        }

        private void UpdateForm()
        {
            AutoReloadChkBox.Checked = false;

            CropBeforChkBox.Checked = ImgFixingSettings.CropBeforChkBox;
            XBeforTxtBox.Text = ImgFixingSettings.XBefor.ToString();
            YBeforTxtBox.Text = ImgFixingSettings.YBefor.ToString();
            dXBeforTxtBox.Text = ImgFixingSettings.DXBefor.ToString();
            dYBeforTxtBox.Text = ImgFixingSettings.DYBefor.ToString();

            BlackWhiteChkBox.Checked = ImgFixingSettings.BlackWhiteMode;
            ZoomLbl.Text = ImgFixingSettings.Zoom.ToString();

            DistChkBox.Checked = ImgFixingSettings.Distortion;
            ATxtBox.Text = ImgFixingSettings.DistorSettings.A.ToString();
            BTxtBox.Text = ImgFixingSettings.DistorSettings.B.ToString();
            CTxtBox.Text = ImgFixingSettings.DistorSettings.C.ToString();
            DTxtBox.Text = ImgFixingSettings.DistorSettings.D.ToString();
            ETxtBox.Text = ImgFixingSettings.DistorSettings.E.ToString();
            Sm11TxtBox.Text = ImgFixingSettings.DistorSettings.Sm11.ToString();
            Sm12TxtBox.Text = ImgFixingSettings.DistorSettings.Sm12.ToString();
            Sm13TxtBox.Text = ImgFixingSettings.DistorSettings.Sm13.ToString();
            Sm21TxtBox.Text = ImgFixingSettings.DistorSettings.Sm21.ToString();
            Sm22TxtBox.Text = ImgFixingSettings.DistorSettings.Sm22.ToString();
            Sm23TxtBox.Text = ImgFixingSettings.DistorSettings.Sm23.ToString();
            Sm31TxtBox.Text = ImgFixingSettings.DistorSettings.Sm31.ToString();
            Sm32TxtBox.Text = ImgFixingSettings.DistorSettings.Sm32.ToString();
            Sm33TxtBox.Text = ImgFixingSettings.DistorSettings.Sm33.ToString();

            switch (ImgFixingSettings.Diminish)
            {
                case 1.5:
                    diminish0СhсkBox.Checked = true;
                    diminish1СhсkBox.Checked = false;
                    diminish2СhсkBox.Checked = false;
                    break;
                case 2:
                    diminish0СhсkBox.Checked = false;
                    diminish1СhсkBox.Checked = true;
                    diminish2СhсkBox.Checked = false;
                    break;
                case 2.5:
                    diminish0СhсkBox.Checked = false;
                    diminish1СhсkBox.Checked = false;
                    diminish2СhсkBox.Checked = true;
                    break;
                default:
                    diminish0СhсkBox.Checked = false;
                    diminish1СhсkBox.Checked = false;
                    diminish2СhсkBox.Checked = false;
                    break;
            }

            CropAfterChkBox.Checked = ImgFixingSettings.CropAfterChkBox;
            XAfterTxtBox.Text = ImgFixingSettings.XAfter.ToString();
            YAfterTxtBox.Text = ImgFixingSettings.YAfter.ToString();
            dXAfterTxtBox.Text = ImgFixingSettings.DXAfter.ToString();
            dYAfterTxtBox.Text = ImgFixingSettings.DYAfter.ToString();

            ShowGridСhckBox.Checked = ImgFixingSettings.ShowGrid;
            AutoReloadChkBox.Checked = ImgFixingSettings.AutoReload;
        }

        private void UpdateSettings()
        {
            ImgFixingSettings.BlackWhiteMode = BlackWhiteChkBox.Checked;

            ImgFixingSettings.CropBeforChkBox = CropBeforChkBox.Checked;
            int.TryParse(XBeforTxtBox.Text, out int param);
            ImgFixingSettings.XBefor = param;
            int.TryParse(YBeforTxtBox.Text, out param);
            ImgFixingSettings.YBefor = param;
            int.TryParse(dXBeforTxtBox.Text, out param);
            ImgFixingSettings.DXBefor = param;
            int.TryParse(dYBeforTxtBox.Text, out param);
            ImgFixingSettings.DYBefor = param;

            double.TryParse(ZoomLbl.Text, out double doublParam);
            ImgFixingSettings.Zoom = doublParam;
            ImgFixingSettings.Distortion = DistChkBox.Checked;

            double.TryParse(ATxtBox.Text, out doublParam);
            ImgFixingSettings.DistorSettings.A = doublParam;
            double.TryParse(BTxtBox.Text, out doublParam);
            ImgFixingSettings.DistorSettings.B = doublParam;
            double.TryParse(CTxtBox.Text, out doublParam);
            ImgFixingSettings.DistorSettings.C = doublParam;
            double.TryParse(DTxtBox.Text, out doublParam);
            ImgFixingSettings.DistorSettings.D = doublParam;
            double.TryParse(ETxtBox.Text, out doublParam);
            ImgFixingSettings.DistorSettings.E = doublParam;
            double.TryParse(Sm11TxtBox.Text, out doublParam);
            ImgFixingSettings.DistorSettings.Sm11 = doublParam;
            double.TryParse(Sm12TxtBox.Text, out doublParam);
            ImgFixingSettings.DistorSettings.Sm12 = doublParam;
            double.TryParse(Sm13TxtBox.Text, out doublParam);
            ImgFixingSettings.DistorSettings.Sm13 = doublParam;
            double.TryParse(Sm21TxtBox.Text, out doublParam);
            ImgFixingSettings.DistorSettings.Sm21 = doublParam;
            double.TryParse(Sm22TxtBox.Text, out doublParam);
            ImgFixingSettings.DistorSettings.Sm22 = doublParam;
            double.TryParse(Sm23TxtBox.Text, out doublParam);
            ImgFixingSettings.DistorSettings.Sm23 = doublParam;
            double.TryParse(Sm31TxtBox.Text, out doublParam);
            ImgFixingSettings.DistorSettings.Sm31 = doublParam;
            double.TryParse(Sm32TxtBox.Text, out doublParam);
            ImgFixingSettings.DistorSettings.Sm32 = doublParam;
            double.TryParse(Sm33TxtBox.Text, out doublParam);
            ImgFixingSettings.DistorSettings.Sm33 = doublParam;

            ImgFixingSettings.CropAfterChkBox = CropAfterChkBox.Checked;
            int.TryParse(XAfterTxtBox.Text, out param);
            ImgFixingSettings.XAfter = param;
            int.TryParse(YAfterTxtBox.Text, out param);
            ImgFixingSettings.YAfter = param;
            int.TryParse(dXAfterTxtBox.Text, out param);
            ImgFixingSettings.DXAfter = param;
            int.TryParse(dYAfterTxtBox.Text, out param);
            ImgFixingSettings.DYAfter = param;

            ImgFixingSettings.AutoReload = AutoReloadChkBox.Checked;
            ImgFixingSettings.ShowGrid = ShowGridСhckBox.Checked;
        }

        /// <summary>
        /// Применить настройки с формы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ApplyBtn_Click_1(object sender, EventArgs e)=>OpenCvReloadImg();
        
        /// <summary>
        /// Перезагрузка картинки в форме
        /// </summary>
        /// <param name="UpDateFormFirst">Определяет берутся ли настройки с формы или из ImgFixingSettings</param>
        private void OpenCvReloadImg(bool UpDateFormFirst = false)
        {
            if(UpDateFormFirst) UpdateForm();
            else UpdateSettings();
            string file = fileEdit.DirFile(InputDirTxtBox.Text, InputFileTxtBox.Text);
            if (string.IsNullOrEmpty(file))
            {
                SetErr("Основные поля не заполнены");
                return;
            }
            if (!File.Exists(file))
            {
                SetErr("File: " + file + " не найден");
                return;
            }

            LoadedBitmapFrame = (Bitmap)Bitmap.FromFile(file);
            
            FindMidlePoint();
            pictureBox1.BackgroundImage = FixFrames.EditImg(ImgFixingSettings, file);
            UpdateForm();
        }

        private void ChkBorder()
        {
            if (LoadedBitmapFrame == null) return;
            int Y = ImgFixingSettings.YBefor, X = ImgFixingSettings.XBefor, dY = ImgFixingSettings.DYBefor, dX = ImgFixingSettings.DXBefor;

            if (Y < 0) Y = 0; 
            if (X < 0) X = 0;
            if (Y > LoadedBitmapFrame.Height - 1) Y = LoadedBitmapFrame.Height / 2;
            if (X > LoadedBitmapFrame.Width - 1) X = LoadedBitmapFrame.Width / 2;

            if (dY <= 0) dY = LoadedBitmapFrame.Height; 
            if (dX <= 0) dX = LoadedBitmapFrame.Width;
            if (Y + dY > LoadedBitmapFrame.Height) dY = LoadedBitmapFrame.Height - Y;
            if (X + dX > LoadedBitmapFrame.Width) dX = LoadedBitmapFrame.Width - X;

            if (Y != ImgFixingSettings.YBefor)
            {
                ImgFixingSettings.YBefor = Y;
                YBeforTxtBox.Text = Y.ToString();
            }

            if(X != ImgFixingSettings.XBefor)
            {
                ImgFixingSettings.XBefor = X;
                XBeforTxtBox.Text = X.ToString();
            }

            if (dY != ImgFixingSettings.DYBefor)
            {
                ImgFixingSettings.DYBefor = dY;
                dYBeforTxtBox.Text = dY.ToString();
            }

            if (dX != ImgFixingSettings.DXBefor)
            {
                ImgFixingSettings.DXBefor = dX;
                dXBeforTxtBox.Text = dX.ToString();
            }
        }

        private void FindMidlePoint()
        {
            if (LoadedBitmapFrame == null) return;
            if (CropBeforChkBox.Checked && DistChkBox.Checked)
            {
                MidleWidthPoint = (int)(ImgFixingSettings.DXBefor / 2 / ImgFixingSettings.Diminish);
                MidleHeightPoint = (int)(ImgFixingSettings.DYBefor / 2 / ImgFixingSettings.Diminish);
            }
            else
            {
                MidleWidthPoint = (int)(LoadedBitmapFrame.Width / 2 / ImgFixingSettings.Diminish);
                MidleHeightPoint = (int)(LoadedBitmapFrame.Height / 2 / ImgFixingSettings.Diminish);
            }

            if (DistChkBox.Checked)SetSm13Sm23();
        }

        private void diminish2СhсkBox_CheckedChanged(object sender, EventArgs e) => DiminishСhсkBox(0);
        private void diminish3СhсkBox_CheckedChanged(object sender, EventArgs e) => DiminishСhсkBox(1);
        private void diminish4СhсkBox_CheckedChanged(object sender, EventArgs e) => DiminishСhсkBox(2);
        private void DiminishСhсkBox(int number)
        {
            if (diminshIn) return;

            diminshIn = true;
            for (int i = 0; i < dimList.Count(); i++)
            {
                if (i != number) dimList[i] = false;
                else
                {
                    switch (number)
                    {
                        case 0:
                            dimList[0] = diminish0СhсkBox.Checked;
                            if (diminish0СhсkBox.Checked) ImgFixingSettings.Diminish = 1.5;
                            else ImgFixingSettings.Diminish = 1;
                            break;
                        case 1:
                            dimList[1] = diminish1СhсkBox.Checked;
                            if (diminish1СhсkBox.Checked) ImgFixingSettings.Diminish = 2;
                            else ImgFixingSettings.Diminish = 1;
                            break;
                        case 2:
                            dimList[2] = diminish2СhсkBox.Checked;
                            if (diminish2СhсkBox.Checked) ImgFixingSettings.Diminish = 2.5;
                            else ImgFixingSettings.Diminish = 1;
                            break;
                        default:
                            ImgFixingSettings.Diminish = 1;
                            break;
                    }
                }
            }

            diminish0СhсkBox.Checked = dimList[0];
            diminish1СhсkBox.Checked = dimList[1];
            diminish2СhсkBox.Checked = dimList[2];
            ZeroCropAfter();
            diminshIn = false;
        }

        private bool CropSession = false;
        private void CropBefor(TextBox textBox, EventArgs e)
        {
            if (CropSession) return;
            else CropSession = true;

            if (string.IsNullOrEmpty(textBox.Text)) return;
            Int32.TryParse(textBox.Text, out int N);

            if (textBox.Name == "XBeforTxtBox") ImgFixingSettings.XBefor = N;
            else if (textBox.Name == "dXBeforTxtBox") ImgFixingSettings.DXBefor = N;
            else if (textBox.Name == "YBeforTxtBox") ImgFixingSettings.YBefor = N;
            else if (textBox.Name == "dYBeforTxtBox") ImgFixingSettings.DYBefor = N;

            ChkBorder();
            FindMidlePoint();
            if (AutoReloadChkBox.Checked) OpenCvReloadImg();

            CropSession = false;
        }

        private void XBeforTxtBox_TextChanged(object sender, EventArgs e) => CropBefor((TextBox)sender, e);
        private void dXBeforTxtBox_TextChanged(object sender, EventArgs e)=> CropBefor((TextBox)sender, e);
        private void YBeforTxtBox_TextChanged(object sender, EventArgs e) => CropBefor((TextBox)sender, e);
        private void dYBeforTxtBox_TextChanged(object sender, EventArgs e) => CropBefor((TextBox)sender, e);
        public string GetImgFixingPlan() => ImgFixingPlan;
    }
}