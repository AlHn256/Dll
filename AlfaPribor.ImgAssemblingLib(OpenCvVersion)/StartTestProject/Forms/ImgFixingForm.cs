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
        private FixFrames FixFrames = new FixFrames();
        private FileEdit fileEdit = new FileEdit(new string[] { "*.jpeg", "*.jpg", "*.png", "*.bmp" });
        private ImgFixingSettings ImgFixingSettings = new ImgFixingSettings();
        private int rotation90 = 0, FileNumber = 0;
        private const string imgDefoltFixingFile = "imgFixingSettings.oip";
        private string ImgFixingPlan = imgDefoltFixingFile;
        private bool SaveRezultToFile;
        private string SavingRezultDir;
        public event Action<int> ProcessChanged;
        public event Action<string> TextChanged;
        public bool IsErr { get; set; } = false;
        public static bool StopProcess = false;
        public string ErrText { get; set; } = string.Empty;
        private List<string> FileList = new List<string>();
        private int MidleWidthPoint { get; set; } = 0;
        private int MidleHeightPoint { get; set; } = 0;

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
        public ImgFixingForm(string imgFixingPlan, bool saveRezultToFile = false, string fixingImgDirectory = "")
        {
            InitializeComponent();
            if (!string.IsNullOrEmpty(imgFixingPlan))
            {
                ImgFixingPlan = imgFixingPlan;
                TryReadSettings();
                SaveRezultToFile = saveRezultToFile;
                SavingRezultDir = fixingImgDirectory;
            }
        }
        // Тестовая версия конструктора
        public ImgFixingForm(string imgFixingPlan, bool test = false)
        {
            InitializeComponent();
            if (!string.IsNullOrEmpty(imgFixingPlan)) ImgFixingPlan = imgFixingPlan;
            TryReadSettings();
        }
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
            if (TextChanged != null) TextChanged((string)txt);
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

            if(!LoadFileList())return;
            OpenCvReloadImg();
            BlockOn();
        }
        /// <summary> Блокировка \ разблокировка доп параметров </summary>
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
                if (AutoReloadChkBox.Checked) OpenCvReloadImg();
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
        private bool LoadFileList(string serchingDir = null)
        {
            if (string.IsNullOrEmpty(serchingDir) && string.IsNullOrEmpty(InputDirTxtBox.Text)) return false;

            if (string.IsNullOrEmpty(serchingDir)) serchingDir = InputDirTxtBox.Text;
            var list = fileEdit.SearchFiles(serchingDir);
            if (!Directory.Exists(serchingDir)) return SetErr("Такой папки "+ serchingDir + " не существует");
            if (list.Length == 0) return SetErr("Файлы для редактирования не найдены");
            FileList = list.Select(f => f.FullName).ToList();

            return true;
        }

        private void ABtnUp_Click(object sender, EventArgs e) => ChangeA(true);
        private void ABtnDn_Click(object sender, EventArgs e) => ChangeA(false);
        private void ChangeA(bool increase)
        {
            double A = 0;
            double.TryParse(ATxtBox.Text, out A);
            if (increase) ImgFixingSettings.DistorSettings.A = Math.Round(A + 0.01, 2);
            else ImgFixingSettings.DistorSettings.A = Math.Round(A - 0.01, 2);
            ATxtBox.Text = ImgFixingSettings.DistorSettings.A.ToString();
            if (AutoReloadChkBox.Checked) OpenCvReloadImg();
        }
        private void CBtnUp_Click(object sender, EventArgs e) => ChangeC(true);
        private void CBtnDn_Click(object sender, EventArgs e) => ChangeC(false);
        private void ChangeC(bool increase)
        {
            double C = 0;
            double.TryParse(CTxtBox.Text, out C);
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
            if (!Directory.Exists(InputDirTxtBox.Text))
            {
                SetErr("Err CorrectFiles.Directory.Exists(InputDirTxtBox.Text)!!!");
                RezultRTB.Text = "Err CorrectFiles.Directory.Exists(InputDirTxtBox.Text)!!!";
                return;
            }
            ShowGridСhckBox.Checked = false;
            if (!fileEdit.ChkDir(InputDirTxtBox.Text)) return;
            if (!Directory.Exists(InputDirTxtBox.Text)) return;
            FileInfo[] fileList = fileEdit.SearchFiles(InputDirTxtBox.Text);
            foreach (var file in fileList)
            {
                string outputFileNumber = InputDirTxtBox.Text + Path.DirectorySeparatorChar + file.Name;
                FixFrames.EditImg(file.FullName).Save(outputFileNumber);
                //EditImg(file.FullName).SaveImage(outputFileNumber);
            }
        }
        public bool FixImges(object param, string outputDir = "")
        {
            SynchronizationContext context = (SynchronizationContext)param;
            ShowGridСhckBox.Checked = false;
            if (string.IsNullOrEmpty(outputDir))
            {
                if (!Directory.Exists(InputDirTxtBox.Text)) return false;
                outputDir = OutputDirTxtBox.Text;
            }

            if (!fileEdit.ChkDir(outputDir)) return false;
            FileInfo[] fileList = fileEdit.SearchFiles(InputDirTxtBox.Text);

            if (fileList == null) return SetErr("ERR FixImges.fileList == null !!!");
            for (int i = 0; i < fileList.Length; i++)
            {
                string outputFileNumber = outputDir + Path.DirectorySeparatorChar + fileList[i].Name;
                FixFrames.EditImg(fileList[i].FullName).Save(outputFileNumber);
                //EditImg(fileList[i].FullName).SaveImage(outputFileNumber);
                context.Send(OnProgressChanged, i * 100 / fileList.Length);
                context.Send(OnTextChanged, "Imges Fixing " + i * 100 / fileList.Length + " %");
            }

            context.Send(OnProgressChanged, 100);
            context.Send(OnTextChanged, "Imges Fixing 100 %");
            return IsErr;
        }
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
        public bool CheckFixingImg(string imgFixingDir = "")
        {
            if (string.IsNullOrEmpty(imgFixingDir)) imgFixingDir = OutputDirTxtBox.Text;
            if (!Directory.Exists(imgFixingDir)) return false;

            FileInfo[] fileList = fileEdit.SearchFiles(InputDirTxtBox.Text);
            for (int i = 0; i < fileList.Length; i++)
                if (!File.Exists(imgFixingDir + Path.DirectorySeparatorChar + fileList[i].Name)) return false;
            return true;
        }
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
            ZoomLbl.Text = ImgFixingSettings.Zoom.ToString();
            ZeroCropAfter(false);
            SetSm13Sm23();
            if (AutoReloadChkBox.Checked) OpenCvReloadImg();
        }
        private void InputDirTxtBox_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(InputDirTxtBox.Text)) return;
            if (!Directory.Exists(InputDirTxtBox.Text)) return;
            var files = fileEdit.SearchFiles(InputDirTxtBox.Text);
            if (files[0] != null)
            {
                InputFileTxtBox.Text = files[0].Name;
                pictureBox1.BackgroundImage = Image.FromFile(files[0].FullName);
            }
            OutputDirTxtBox.Text = InputDirTxtBox.Text.FirstOf(Path.DirectorySeparatorChar.ToString()) + Path.DirectorySeparatorChar + InputDirTxtBox.Text.LastOf('\\') + "Out";
            if (AutoReloadChkBox.Checked) OpenCvReloadImg();
        }

        private string prevDir = string.Empty;
        private void InputFileTxtBox_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(InputDirTxtBox.Text) || string.IsNullOrEmpty(InputFileTxtBox.Text)) return;
            string file = fileEdit.DirFile(InputDirTxtBox.Text, InputFileTxtBox.Text);
            if (!File.Exists(file))
            {
                SetErr("File: " + file + " не найден");
                return;
            }
            FixFrames.ChangeFirstFrame(file);

            if (prevDir != InputDirTxtBox.Text) LoadFileList();
            prevDir = InputDirTxtBox.Text;

            if (AutoReloadChkBox.Checked) OpenCvReloadImg();
        }

        private void ATxtBox_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ATxtBox.Text)) return;
            double A = 0;
            double.TryParse(ATxtBox.Text, out A);
            ImgFixingSettings.DistorSettings.A = Math.Round(A, 2);
            ATxtBox.Text = ImgFixingSettings.DistorSettings.A.ToString();
            if (AutoReloadChkBox.Checked) OpenCvReloadImg();
        }
        private void BTxtBox_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(BTxtBox.Text)) return;
            double B = 0;
            double.TryParse(BTxtBox.Text, out B);
            ImgFixingSettings.DistorSettings.B = Math.Round(B, 2);
            BTxtBox.Text = ImgFixingSettings.DistorSettings.B.ToString();
            if (AutoReloadChkBox.Checked) OpenCvReloadImg();
        }
        private void CTxtBox_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(CTxtBox.Text)) return;
            double C = 0;
            double.TryParse(CTxtBox.Text, out C);
            ImgFixingSettings.DistorSettings.C = Math.Round(C, 2);
            CTxtBox.Text = ImgFixingSettings.DistorSettings.C.ToString();
            if (AutoReloadChkBox.Checked) OpenCvReloadImg();
        }

        private void DTxtBox_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(DTxtBox.Text)) return;
            double D = 0;
            double.TryParse(DTxtBox.Text, out D);
            ImgFixingSettings.DistorSettings.D = Math.Round(D, 2);
            DTxtBox.Text = ImgFixingSettings.DistorSettings.D.ToString();
            if (AutoReloadChkBox.Checked) OpenCvReloadImg();
        }

        private void ETxtBox_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(BTxtBox.Text)) return;
            double E = 0;
            double.TryParse(ETxtBox.Text, out E);
            ImgFixingSettings.DistorSettings.E = Math.Round(E, 2);
            ETxtBox.Text = ImgFixingSettings.DistorSettings.E.ToString();
            if (AutoReloadChkBox.Checked) OpenCvReloadImg();
        }

        //private void ETxtBox_TextChanged(object sender, EventArgs e)
        //{
        //    if (string.IsNullOrEmpty(ETxtBox.Text)) return;
        //    double.TryParse(ETxtBox.Text, out E);
        //    E = Math.Round(E, 2);
        //    if (AutoReloadChkBox.Checked) OpenCvReloadImg();
        //}
        //private void Sm11TxtBox_TextChanged(object sender, EventArgs e)
        //{
        //    if (string.IsNullOrEmpty(Sm11TxtBox.Text)) return;
        //    double.TryParse(Sm11TxtBox.Text, out Sm11);
        //    Sm11 = Math.Round(Sm11, 3);
        //    Sm11TxtBox.Text = Sm11.ToString();
        //    if (AutoReloadChkBox.Checked) OpenCvReloadImg();
        //}
        //private void Sm12TxtBox_TextChanged(object sender, EventArgs e)
        //{
        //    if (string.IsNullOrEmpty(Sm12TxtBox.Text)) return;
        //    Double.TryParse(Sm12TxtBox.Text, out Sm12);
        //    Sm12 = Math.Round(Sm12, 3);
        //    Sm12TxtBox.Text = Sm12.ToString();
        //    if (AutoReloadChkBox.Checked) OpenCvReloadImg();
        //}
        //private void Sm13TxtBox_TextChanged(object sender, EventArgs e)
        //{
        //    if (string.IsNullOrEmpty(Sm13TxtBox.Text)) return;
        //    Double.TryParse(Sm13TxtBox.Text, out Sm13);
        //    //Sm13 = Math.Round(Convert.ToDouble(Sm13TxtBox.Text), 3);
        //    Sm13TxtBox.Text = Sm13.ToString();
        //    if (AutoReloadChkBox.Checked) OpenCvReloadImg();
        //}
        //private void Sm21TxtBox_TextChanged(object sender, EventArgs e)
        //{
        //    if (string.IsNullOrEmpty(Sm21TxtBox.Text)) return;
        //    Double.TryParse(Sm21TxtBox.Text, out Sm21);
        //    //Sm21 = Math.Round(Convert.ToDouble(Sm21TxtBox.Text), 3);
        //    Sm21TxtBox.Text = Sm21.ToString();
        //    if (AutoReloadChkBox.Checked) OpenCvReloadImg();
        //}

        //private void Sm22TxtBox_TextChanged(object sender, EventArgs e)
        //{
        //    if (string.IsNullOrEmpty(Sm22TxtBox.Text)) return;
        //    Double.TryParse(Sm22TxtBox.Text, out Sm22);
        //    //Sm22 = Math.Round(Convert.ToDouble(Sm22TxtBox.Text), 3);
        //    Sm22TxtBox.Text = Sm22.ToString();
        //    if (AutoReloadChkBox.Checked) OpenCvReloadImg();
        //}

        //private void Sm23TxtBox_TextChanged(object sender, EventArgs e)
        //{
        //    if (string.IsNullOrEmpty(Sm23TxtBox.Text)) return;
        //    Double.TryParse(Sm23TxtBox.Text, out Sm23);
        //    //Sm23 = Math.Round(Convert.ToDouble(Sm23TxtBox.Text), 3);
        //    Sm23TxtBox.Text = Sm23.ToString();
        //    if (AutoReloadChkBox.Checked) OpenCvReloadImg();
        //}

        //private void Sm31TxtBox_TextChanged(object sender, EventArgs e)
        //{
        //    if (string.IsNullOrEmpty(Sm31TxtBox.Text)) return;
        //    Double.TryParse(Sm31TxtBox.Text, out Sm31);
        //    //Sm31 = Math.Round(Convert.ToDouble(Sm31TxtBox.Text), 3);
        //    Sm31TxtBox.Text = Sm31.ToString();
        //    if (AutoReloadChkBox.Checked) OpenCvReloadImg();
        //}

        //private void Sm32TxtBox_TextChanged(object sender, EventArgs e)
        //{
        //    if (string.IsNullOrEmpty(Sm32TxtBox.Text)) return;
        //    Double.TryParse(Sm32TxtBox.Text, out Sm32);
        //    //Sm32 = Math.Round(Convert.ToDouble(Sm32TxtBox.Text), 3);
        //    Sm32TxtBox.Text = Sm32.ToString();
        //    if (AutoReloadChkBox.Checked) OpenCvReloadImg();
        //}

        //private void Sm33TxtBox_TextChanged(object sender, EventArgs e)
        //{
        //    if (string.IsNullOrEmpty(Sm33TxtBox.Text)) return;
        //    Double.TryParse(Sm33TxtBox.Text, out Sm33);
        //    //Sm33 = Math.Round(Convert.ToDouble(Sm33TxtBox.Text), 3);
        //    Sm33TxtBox.Text = Sm33.ToString();
        //    if (AutoReloadChkBox.Checked) OpenCvReloadImg();
        //}

        private void BBtnUp_Click(object sender, EventArgs e)
        {
            double B = 0;
            double.TryParse(BTxtBox.Text, out B);
            ImgFixingSettings.DistorSettings.B = Math.Round(B + 0.01, 2);
            BTxtBox.Text = ImgFixingSettings.DistorSettings.B.ToString();
            if (AutoReloadChkBox.Checked) OpenCvReloadImg();
        }

        private void BBtnDn_Click(object sender, EventArgs e)
        {
            double B = 0;
            double.TryParse(BTxtBox.Text, out B);
            ImgFixingSettings.DistorSettings.B = Math.Round(B - 0.01, 2);
            BTxtBox.Text = ImgFixingSettings.DistorSettings.B.ToString();
            if (AutoReloadChkBox.Checked) OpenCvReloadImg();
        }

        //private void BBtnUp_Click(object sender, EventArgs e)
        //{
        //    double.TryParse(BTxtBox.Text, out B);
        //    B = Math.Round(B + 0.01, 2);
        //    BTxtBox.Text = B.ToString();
        //    if (AutoReloadChkBox.Checked) OpenCvReloadImg();
        //}
        //private void BBtnDn_Click(object sender, EventArgs e)
        //{
        //    double.TryParse(BTxtBox.Text, out B);
        //    B = Math.Round(B - 0.01, 2);
        //    BTxtBox.Text = B.ToString();
        //    if (AutoReloadChkBox.Checked) OpenCvReloadImg();



        private void EBtnDn_Click(object sender, EventArgs e)
        {
            double E = 0;
            double.TryParse(ETxtBox.Text, out E);
            ImgFixingSettings.DistorSettings.E = Math.Round(E - 0.01, 2);
            ETxtBox.Text = ImgFixingSettings.DistorSettings.E.ToString();
            if (AutoReloadChkBox.Checked) OpenCvReloadImg();
            //    double.TryParse(ETxtBox.Text, out E);
            //    E = Math.Round(E - 0.01, 2);
            //    ETxtBox.Text = E.ToString();
            //    if (AutoReloadChkBox.Checked) OpenCvReloadImg();
        }

        private void EBtnUp_Click(object sender, EventArgs e)
        {
            double E = 0;
            double.TryParse(ETxtBox.Text, out E);
            ImgFixingSettings.DistorSettings.E = Math.Round(E + 0.01, 2);
            ETxtBox.Text = ImgFixingSettings.DistorSettings.E.ToString();
            if (AutoReloadChkBox.Checked) OpenCvReloadImg();
            //    double.TryParse(ETxtBox.Text, out E);
            //    E = Math.Round(E + 0.01, 2);
            //    ETxtBox.Text = E.ToString();
            //    if (AutoReloadChkBox.Checked) OpenCvReloadImg();
        }

        //}
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
        private void DistZeroBtn_Click(object sender, EventArgs e) => DistZero();
        private void DistZero()
        {
            bool AutoReloadSave = AutoReloadChkBox.Checked;
            AutoReloadChkBox.Checked = false;

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
            ZeroCropAfter(true);
        }
        private void SetSm13Sm23()
        {
            Sm13TxtBox.Text = MidleWidthPoint.ToString();   
            Sm23TxtBox.Text = MidleHeightPoint.ToString();
            ImgFixingSettings.DistorSettings.Sm13 = MidleWidthPoint;
            ImgFixingSettings.DistorSettings.Sm23 = MidleHeightPoint;
        }
        private void ZeroCropAfterBtn_Click(object sender, EventArgs e) => ZeroCropAfter(true);
        private void ZeroCropAfter(bool ReloadImg = false)
        {
            if (AutoReloadChkBox.Checked)
            {
                ImgFixingSettings.DXAfter = 10000;
                ImgFixingSettings.DYAfter = 10000;
                OpenCvReloadImg();
            }

            if (FixFrames==null)return;
            Bitmap OriginalFrame =  FixFrames.GetOriginalFrame();
            Bitmap RezultFrame = FixFrames.GetRezult();
            //if (bitmap == null || bitmap.Width ==0|| bitmap.Height == 0) return;

            bool AutoReloadSave = AutoReloadChkBox.Checked;
            AutoReloadChkBox.Checked = false;

            ImgFixingSettings.XAfter = 0;
            ImgFixingSettings.YAfter = 0;
            ImgFixingSettings.DXAfter = RezultFrame.Width;
            ImgFixingSettings.DYAfter = RezultFrame.Height;

            XAfterTxtBox.Text = ImgFixingSettings.XAfter.ToString();
            YAfterTxtBox.Text = ImgFixingSettings.YAfter.ToString();
            dXAfterTxtBox.Text = ImgFixingSettings.DXAfter.ToString();
            dYAfterTxtBox.Text = ImgFixingSettings.DYAfter.ToString();

            AutoReloadChkBox.Checked = AutoReloadSave;
            //if (ReloadImg) OpenCvReloadImg();
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
                if (await fileEdit.SaveJsonAsync(file, ImgFixingSettings)) RezultRTB.Text = "Settings save in " + file;
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
                    UpdateForm();
                    OpenCvReloadImg();
                }
            }
        }

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
            //bool AutoReloadSave = AutoReloadChkBox.Checked;
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
        private void SetImgFixingSettingsWD(ImgFixingSettings imgFixingSettings) // для старых версий загрузка плана без настроек дисторсии
        {
            bool AutoReloadSave = AutoReloadChkBox.Checked;
            AutoReloadChkBox.Checked = false;
            DistChkBox.Checked = false;

            if (imgFixingSettings.Zoom < 1) imgFixingSettings.Zoom = 1;
            //Zoom = imgFixingSettings.Zoom;
            ZoomLbl.Text = imgFixingSettings.Zoom.ToString();
            rotation90 = imgFixingSettings.Rotation90;
            BlackWhiteChkBox.Checked = imgFixingSettings.BlackWhiteMode;
            CropAfterChkBox.Checked = imgFixingSettings.CropAfterChkBox;
            XAfterTxtBox.Text = imgFixingSettings.XAfter.ToString();
            YAfterTxtBox.Text = imgFixingSettings.YAfter.ToString();
            dXAfterTxtBox.Text = imgFixingSettings.DXAfter.ToString();
            dYAfterTxtBox.Text = imgFixingSettings.DYAfter.ToString();
            AutoReloadChkBox.Checked = AutoReloadSave;
        }
        //private ImgFixingSettings GetImgFixingSettings()
        //{
        //    DistorSettings distorSettings = new DistorSettings()
        //    {
        //        A = A,
        //        B = B,
        //        C = C,
        //        D = D,
        //        E = E,
        //        Sm11 = Sm11,
        //        Sm12 = Sm12,
        //        Sm13 = Sm13,
        //        Sm21 = Sm21,
        //        Sm22 = Sm22,
        //        Sm23 = Sm23,
        //        Sm31 = Sm31,
        //        Sm32 = Sm32,
        //        Sm33 = Sm33,
        //    };

        //    int Y = 0, X = 0, dY = 0, dX = 0;
        //    Int32.TryParse(XAfterTxtBox.Text, out X);
        //    Int32.TryParse(YAfterTxtBox.Text, out Y);
        //    Int32.TryParse(dYAfterTxtBox.Text, out dY);
        //    Int32.TryParse(dXAfterTxtBox.Text, out dX);
        //    XAfterTxtBox.Text = X.ToString();
        //    YAfterTxtBox.Text = Y.ToString();
        //    dYAfterTxtBox.Text = dY.ToString();
        //    dXAfterTxtBox.Text = dX.ToString();

        //    return new ImgFixingSettings
        //    {
        //        Dir = InputDirTxtBox.Text,
        //        File = InputFileTxtBox.Text,
        //        Rotation90 = rotation90,
        //        Zoom = Zoom,
        //        BlackWhiteMode = BlackWhiteChkBox.Checked,

        //        Distortion = DistChkBox.Checked,
        //        DistorSettings = distorSettings,

        //        CropAfterChkBox = CropAfterChkBox.Checked,
        //        XAfter = X,
        //        YAfter = Y,
        //        DXAfter = dX,
        //        DYAfter = dY
        //    };
        //}
        private void RBtnUpDn_Click(object sender, EventArgs e) { Rotation90(false); ZeroCropAfter(true); SetSm13Sm23(); }
        private void RBtnUp90_Click(object sender, EventArgs e) { Rotation90(true); ZeroCropAfter(true); SetSm13Sm23(); }
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
            ImgFixingSettings.DistorSettings.Sm21 = ImgFixingSettings.DistorSettings.Sm21 + N;
            ImgFixingSettings.DistorSettings.Sm12 = ImgFixingSettings.DistorSettings.Sm12 - N;
            Sm21TxtBox.Text = ImgFixingSettings.DistorSettings.Sm21.ToString();
            Sm12TxtBox.Text = ImgFixingSettings.DistorSettings.Sm12.ToString();
            if (AutoReloadChkBox.Checked) OpenCvReloadImg();
        }
        private void ShowGridСhckBox_CheckedChanged(object sender, EventArgs e)
        {
            ImgFixingSettings.ShowGrid = ShowGridСhckBox.Checked;
            if (AutoReloadChkBox.Checked) OpenCvReloadImg();
        }
        private void UpdateForm()
        {
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

            CropAfterChkBox.Checked = ImgFixingSettings.CropAfterChkBox;
            XAfterTxtBox.Text = ImgFixingSettings.XAfter.ToString();
            YAfterTxtBox.Text = ImgFixingSettings.YAfter.ToString();
            dXAfterTxtBox.Text = ImgFixingSettings.DXAfter.ToString();
            dYAfterTxtBox.Text = ImgFixingSettings.DYAfter.ToString();

            AutoReloadChkBox.Checked = ImgFixingSettings.AutoReload;
            ShowGridСhckBox.Checked = ImgFixingSettings.ShowGrid;
        }

        private void UpdateSettings()
        {
            ImgFixingSettings.BlackWhiteMode = BlackWhiteChkBox.Checked;

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
            int.TryParse(XAfterTxtBox.Text, out int param);
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

        private void ApplyBtn_Click_1(object sender, EventArgs e)
        {
            UpdateSettings();
            FixFrames.ChangeSettings(ImgFixingSettings);
            OpenCvReloadImg();
        }

        // private void OpenCvReloadImg()=>pictureBox1.BackgroundImage = MatToBitmap(EditImg());
        private void OpenCvReloadImg()
        {
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
            var bitmapFrame  = FixFrames.EditImg(ImgFixingSettings, file);
            MidleWidthPoint = bitmapFrame.Width / 2;
            MidleHeightPoint = bitmapFrame.Height / 2;
            pictureBox1.BackgroundImage = bitmapFrame;
        }

        public string GetImgFixingPlan() => ImgFixingPlan;
    }
}