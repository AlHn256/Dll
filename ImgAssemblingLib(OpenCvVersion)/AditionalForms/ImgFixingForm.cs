using ImgAssemblingLibOpenCV.Models;
using ImgFixingLibOpenCvVersion.Models;
using Newtonsoft.Json;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace ImgAssemblingLibOpenCV.AditionalForms
{
    public partial class ImgFixingForm : Form
    {
        private int rotation90 = 0;
        private double A = -0.13, B = 0.39, C = 0.08, D = 0, E = 0, Zoom = 1;
        private double Sm11 = 1500, Sm12 = 0.0, Sm13 = 0.0;
        private double Sm21 = 0.0, Sm22 = 1500, Sm23 = 0.0;
        private double Sm31 = 0.0, Sm32 = 0.0, Sm33 = 1.0;
        private FileEdit fileEdit = new FileEdit(new string[] { "*.jpeg", "*.jpg", "*.png", "*.bmp" });
        private const string imgDefoltFixingFile = "imgFixingSettings.oip";
        private string imgFixingFile = imgDefoltFixingFile;
        public event Action<int> ProcessChanged;
        public event Action<string> TextChanged;
        public bool IsErr { get; set; } = false;
        public static bool StopProcess = false;
        public string ErrText { get; set; } = string.Empty;

        public ImgFixingForm(string directory)
        {
            InitializeComponent();

            InputDirTxtBox.Text = directory;
            Load += OnLoad;
        }
        public ImgFixingForm(string imgFixingPlan, string directory, bool fileLoad = false)
        {
            InitializeComponent();
            if (!string.IsNullOrEmpty(imgFixingPlan)) imgFixingFile = imgFixingPlan;
            TryReadSettings(fileLoad);
            InputDirTxtBox.Text = directory;
        }
        public ImgFixingForm(string imgFixingPlan, bool test = false)
        {
            InitializeComponent();
            if (!string.IsNullOrEmpty(imgFixingPlan)) imgFixingFile = imgFixingPlan;
            TryReadSettings(false);
        }
        public ImgFixingForm(string imgFixingPlan, bool saveRezultToFile = false, string fixingImgDirectory = "")
        {
            InitializeComponent();
            if (!string.IsNullOrEmpty(imgFixingPlan))
            {
                imgFixingFile = imgFixingPlan;
                TryReadSettings(false);
                //SaveRezultToFile = saveRezultToFile;
                //SavingRezultDir = fixingImgDirectory;
            }
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
            InputFileTxtBox.Text = Path.GetFileName(files[0]);
        }
        private void OnLoad(object sender, EventArgs e)
        {
            AllowDrop = true;
            DragEnter += new DragEventHandler(WindowsForm_DragEnter);
            DragDrop += new DragEventHandler(WindowsForm_DragDrop);
            pictureBox1.AllowDrop = true;
            pictureBox1.DragEnter += new DragEventHandler(WindowsForm_DragEnter);
            pictureBox1.DragDrop += new DragEventHandler(WindowsForm_DragDrop);
            ZoomLbl.Text = Zoom.ToString();
            this.MouseWheel += panel1_MouseWheel;

            ATxtBox.Text = A.ToString();
            BTxtBox.Text = B.ToString();
            CTxtBox.Text = C.ToString();
            DTxtBox.Text = D.ToString();
            ETxtBox.Text = E.ToString();
            Sm11TxtBox.Text = Sm11.ToString();
            Sm12TxtBox.Text = Sm12.ToString();
            Sm13TxtBox.Text = Sm13.ToString();
            Sm21TxtBox.Text = Sm21.ToString();
            Sm22TxtBox.Text = Sm22.ToString();
            Sm23TxtBox.Text = Sm23.ToString();
            Sm31TxtBox.Text = Sm31.ToString();
            Sm32TxtBox.Text = Sm32.ToString();
            Sm33TxtBox.Text = Sm33.ToString();

            BlockOn();
        }
        protected void BlockOn()
        {
            Sm11TxtBox.Enabled = false;
            Sm12TxtBox.Enabled = false;
            Sm13TxtBox.Enabled = false;
            Sm21TxtBox.Enabled = false;
            Sm22TxtBox.Enabled = false;
            Sm23TxtBox.Enabled = false;
            Sm31TxtBox.Enabled = false;
            Sm32TxtBox.Enabled = false;
            Sm33TxtBox.Enabled = false;
            //ATxtBox.Enabled = false;
            BBtnDn.Enabled = false;
            BBtnUp.Enabled = false;
            BTxtBox.Enabled = false;
            CBtnDn.Enabled = false;
            CBtnUp.Enabled = false;
            CTxtBox.Enabled = false;
            DBtnDn.Enabled = false;
            DBtnUp.Enabled = false;
            DTxtBox.Enabled = false;
            //ETxtBox.Enabled = false;
        }
        private void UnBlock()
        {
            Sm11TxtBox.Enabled = true;
            Sm12TxtBox.Enabled = true;
            Sm13TxtBox.Enabled = true;
            Sm21TxtBox.Enabled = true;
            Sm22TxtBox.Enabled = true;
            Sm23TxtBox.Enabled = true;
            Sm31TxtBox.Enabled = true;
            Sm32TxtBox.Enabled = true;
            Sm33TxtBox.Enabled = true;
            //ATxtBox.Enabled = false;
            BBtnDn.Enabled = true;
            BBtnUp.Enabled = true;
            BTxtBox.Enabled = true;
            CBtnDn.Enabled = true;
            CBtnUp.Enabled = true;
            CTxtBox.Enabled = true;
            DBtnDn.Enabled = true;
            DBtnUp.Enabled = true;
            DTxtBox.Enabled = true;
            //ETxtBox.Enabled = false;
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            KeyDown(keyData);
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private new void KeyDown(Keys keyData)
        {
            if (keyData == Keys.Z) DistZero();
            else if (keyData == Keys.NumPad7) ChangeA(false); 
            else if (keyData == Keys.NumPad9) ChangeA(true);
            else if (keyData == Keys.NumPad8) ChangeC(true);
            else if (keyData == Keys.NumPad2) ChangeC(false);
            else if (keyData == Keys.NumPad4) ChangeD(true);
            else if (keyData == Keys.NumPad6) ChangeD(false);
            else if (keyData == Keys.NumPad1) Rotation(-10);
            else if (keyData == Keys.NumPad3) Rotation(10);
            else if (keyData == Keys.Add) ChangZoom(false);
            else if (keyData == Keys.Subtract) ChangZoom(true);
        }
        private void ABtnUp_Click(object sender, EventArgs e) => ChangeA(true);
        private void ABtnDn_Click(object sender, EventArgs e) => ChangeA(false);
        private void ChangeA(bool increase)
        {
            double.TryParse(ATxtBox.Text, out A);
            if (increase) A = Math.Round(A + 0.01, 2);
            else A = Math.Round(A - 0.01, 2);
            ATxtBox.Text = A.ToString();
            if (AutoReloadChkBox.Checked) OpenCvReloadImg();
        }
        private void CBtnUp_Click(object sender, EventArgs e) => ChangeC(true);
        private void CBtnDn_Click(object sender, EventArgs e) => ChangeC(false);
        private void ChangeC(bool increase)
        {
            double.TryParse(CTxtBox.Text, out C);
            if (increase) C = Math.Round(C + 0.01, 2);
            else C = Math.Round(C - 0.01, 2);
            CTxtBox.Text = C.ToString();
            if (AutoReloadChkBox.Checked) OpenCvReloadImg();
        }
        private void DBtnUp_Click(object sender, EventArgs e) => ChangeD(true);
        private void DBtnDn_Click(object sender, EventArgs e) => ChangeD(false);
        private void ChangeD(bool increase)
        {
            double.TryParse(DTxtBox.Text, out D);
            if (increase) D = Math.Round(D + 0.01, 2);
            else D = Math.Round(D - 0.01, 2);
            DTxtBox.Text = D.ToString();
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
                string outputFileNumber = InputDirTxtBox.Text + "\\" + file.Name;
                EditImg(file.FullName).SaveImage(outputFileNumber);
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
                string outputFileNumber = outputDir + "\\" + fileList[i].Name;
                EditImg(fileList[i].FullName).SaveImage(outputFileNumber);
                context.Send(OnProgressChanged, i * 100 / fileList.Length);
                context.Send(OnTextChanged, "Imges Fixing " + i * 100 / fileList.Length + " %");
            }

            context.Send(OnProgressChanged, 100);
            context.Send(OnTextChanged, "Imges Fixing 100 %");
            return true;
        }
        public Bitmap[] FixImges(object param, Bitmap[] dataArray)
        {
            if (dataArray == null || dataArray.Length == 0)
            {
                SetErr("ERR FixImges.fileList == null !!!");
                return null;
            }

            SynchronizationContext context = (SynchronizationContext)param;
            ShowGridСhckBox.Checked = false;

            List<Bitmap> bitMapList = new List<Bitmap>();
            for (int i = 0; i < dataArray.Length; i++)
            {
                bitMapList.Add(EditImg(dataArray[i]));
                context.Send(OnProgressChanged, i * 100 / dataArray.Length);
                context.Send(OnTextChanged, "Imges Fixing " + i * 100 / dataArray.Length + " %");
            }

            context.Send(OnProgressChanged, 100);
            context.Send(OnTextChanged, "Imges Fixing 100 %");

            return bitMapList.ToArray();
        }
        public bool CheckFixingImg(string imgFixingDir = "")
        {
            // ??todo перенести это в fileEdit
            if (string.IsNullOrEmpty(imgFixingDir)) imgFixingDir = OutputDirTxtBox.Text;
            if (!Directory.Exists(imgFixingDir)) return false;

            FileInfo[] fileList = fileEdit.SearchFiles(InputDirTxtBox.Text);
            for (int i = 0; i < fileList.Length; i++)
                if (!File.Exists(imgFixingDir + "\\" + fileList[i].Name)) return false;
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
            if (increase)
            {
                if (Zoom < 1.04) Zoom = 1.05;
                else Zoom += 0.01;
            }
            else
            {
                Zoom -= 0.01;
                if (Zoom < 1.05) Zoom = 1;
            }
            ZoomLbl.Text = Zoom.ToString();
            ZeroCropAfter(false);
            SetSm13Sm23();
            if (AutoReloadChkBox.Checked) OpenCvReloadImg();
        }
        private void InputDirTxtBox_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(InputDirTxtBox.Text)) return;
            var files = fileEdit.SearchFiles(InputDirTxtBox.Text);
            if (files[0] != null)
            {
                InputFileTxtBox.Text = files[0].Name;
                pictureBox1.BackgroundImage = Image.FromFile(files[0].FullName);
            }
            OutputDirTxtBox.Text = InputDirTxtBox.Text.FirstOf('\\') + "\\" + InputDirTxtBox.Text.LastOf('\\') + "Out";
            if (AutoReloadChkBox.Checked) OpenCvReloadImg();
        }
        private void InputFileTxtBox_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(InputDirTxtBox.Text) || string.IsNullOrEmpty(InputFileTxtBox.Text)) return;

            //string file = fileEdit.DirFile(InputDirTxtBox.Text, InputFileTxtBox.Text);
            if (AutoReloadChkBox.Checked) OpenCvReloadImg();
        }
        private void ATxtBox_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ATxtBox.Text)) return;
            double.TryParse(ATxtBox.Text, out A);
            A = Math.Round(A, 2);
            if (AutoReloadChkBox.Checked) OpenCvReloadImg();
        }
        private void BTxtBox_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(BTxtBox.Text)) return;
            double.TryParse(BTxtBox.Text, out B);
            B = Math.Round(B, 2);
            if (AutoReloadChkBox.Checked) OpenCvReloadImg();
        }
        private void CTxtBox_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(CTxtBox.Text)) return;
            double.TryParse(CTxtBox.Text, out C);
            C = Math.Round(C, 2);
            if (AutoReloadChkBox.Checked) OpenCvReloadImg();
        }
        private void DTxtBox_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(DTxtBox.Text)) return;
            double.TryParse(DTxtBox.Text, out D);
            D = Math.Round(D, 2);
            if (AutoReloadChkBox.Checked) OpenCvReloadImg();
        }
        private void ETxtBox_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ETxtBox.Text)) return;
            double.TryParse(ETxtBox.Text, out E);
            E = Math.Round(E, 2);
            if (AutoReloadChkBox.Checked) OpenCvReloadImg();
        }
        private void Sm11TxtBox_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Sm11TxtBox.Text)) return;
            double.TryParse(Sm11TxtBox.Text, out Sm11);
            Sm11 = Math.Round(Sm11, 3);
            Sm11TxtBox.Text = Sm11.ToString();
            if (AutoReloadChkBox.Checked) OpenCvReloadImg();
        }
        private void Sm12TxtBox_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Sm12TxtBox.Text)) return;
            Double.TryParse(Sm12TxtBox.Text, out Sm12);
            Sm12 = Math.Round(Sm12, 3);
            Sm12TxtBox.Text = Sm12.ToString();
            if (AutoReloadChkBox.Checked) OpenCvReloadImg();
        }
        private void Sm13TxtBox_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Sm13TxtBox.Text)) return;
            Double.TryParse(Sm13TxtBox.Text, out Sm13);
            //Sm13 = Math.Round(Convert.ToDouble(Sm13TxtBox.Text), 3);
            Sm13TxtBox.Text = Sm13.ToString();
            if (AutoReloadChkBox.Checked) OpenCvReloadImg();
        }
        private void Sm21TxtBox_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Sm21TxtBox.Text)) return;
            Double.TryParse(Sm21TxtBox.Text, out Sm21);
            //Sm21 = Math.Round(Convert.ToDouble(Sm21TxtBox.Text), 3);
            Sm21TxtBox.Text = Sm21.ToString();
            if (AutoReloadChkBox.Checked) OpenCvReloadImg();
        }

        private void Sm22TxtBox_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Sm22TxtBox.Text)) return;
            Double.TryParse(Sm22TxtBox.Text, out Sm22);
            //Sm22 = Math.Round(Convert.ToDouble(Sm22TxtBox.Text), 3);
            Sm22TxtBox.Text = Sm22.ToString();
            if (AutoReloadChkBox.Checked) OpenCvReloadImg();
        }

        private void Sm23TxtBox_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Sm23TxtBox.Text)) return;
            Double.TryParse(Sm23TxtBox.Text, out Sm23);
            //Sm23 = Math.Round(Convert.ToDouble(Sm23TxtBox.Text), 3);
            Sm23TxtBox.Text = Sm23.ToString();
            if (AutoReloadChkBox.Checked) OpenCvReloadImg();
        }

        private void Sm31TxtBox_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Sm31TxtBox.Text)) return;
            Double.TryParse(Sm31TxtBox.Text, out Sm31);
            //Sm31 = Math.Round(Convert.ToDouble(Sm31TxtBox.Text), 3);
            Sm31TxtBox.Text = Sm31.ToString();
            if (AutoReloadChkBox.Checked) OpenCvReloadImg();
        }

        private void Sm32TxtBox_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Sm32TxtBox.Text)) return;
            Double.TryParse(Sm32TxtBox.Text, out Sm32);
            //Sm32 = Math.Round(Convert.ToDouble(Sm32TxtBox.Text), 3);
            Sm32TxtBox.Text = Sm32.ToString();
            if (AutoReloadChkBox.Checked) OpenCvReloadImg();
        }

        private void Sm33TxtBox_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Sm33TxtBox.Text)) return;
            Double.TryParse(Sm33TxtBox.Text, out Sm33);
            //Sm33 = Math.Round(Convert.ToDouble(Sm33TxtBox.Text), 3);
            Sm33TxtBox.Text = Sm33.ToString();
            if (AutoReloadChkBox.Checked) OpenCvReloadImg();
        }

        private void BBtnUp_Click(object sender, EventArgs e)
        {
            double.TryParse(BTxtBox.Text, out B);
            B = Math.Round(B + 0.01, 2);
            BTxtBox.Text = B.ToString();
            if (AutoReloadChkBox.Checked) OpenCvReloadImg();
        }
        private void BBtnDn_Click(object sender, EventArgs e)
        {
            double.TryParse(BTxtBox.Text, out B);
            B = Math.Round(B - 0.01, 2);
            BTxtBox.Text = B.ToString();
            if (AutoReloadChkBox.Checked) OpenCvReloadImg();
        }
        private void EBtnUp_Click(object sender, EventArgs e)
        {
            double.TryParse(ETxtBox.Text, out E);
            E = Math.Round(E + 0.01, 2);
            ETxtBox.Text = E.ToString();
            if (AutoReloadChkBox.Checked) OpenCvReloadImg();
        }
        private void EBtnDn_Click(object sender, EventArgs e)
        {
            double.TryParse(ETxtBox.Text, out E);
            E = Math.Round(E - 0.01, 2);
            ETxtBox.Text = E.ToString();
            if (AutoReloadChkBox.Checked) OpenCvReloadImg();
        }
        private void DistZeroBtn_Click(object sender, EventArgs e) => DistZero();
        private void DistZero()
            {
            bool AutoReloadSave = AutoReloadChkBox.Checked;
            AutoReloadChkBox.Checked = false;
            rotation90 = 0;

            A = 0;
            B = 0;
            C = 0;
            D = 0;
            E = 0;

            ATxtBox.Text = "0";
            BTxtBox.Text = "0";
            CTxtBox.Text = "0";
            DTxtBox.Text = "0";
            ETxtBox.Text = "0";

            Zoom = 1;
            ZoomLbl.Text = "1";
            SetSm13Sm23();

            Sm11TxtBox.Text = "1500"; Sm12TxtBox.Text = "0";
            Sm21TxtBox.Text = "0"; Sm22TxtBox.Text = "1500";
            Sm31TxtBox.Text = "0"; Sm32TxtBox.Text = "0"; Sm33TxtBox.Text = "1";

            AutoReloadChkBox.Checked = AutoReloadSave;
            ZeroCropAfter(true);
        }

        private void SetSm13Sm23()
        {
            string file = string.Empty;
            if (!string.IsNullOrEmpty(InputDirTxtBox.Text) && !string.IsNullOrEmpty(InputFileTxtBox.Text))
                file = fileEdit.DirFile(InputDirTxtBox.Text, InputFileTxtBox.Text);
            if (!File.Exists(file))
            {
                Sm13TxtBox.Text = "0";
                Sm23TxtBox.Text = "0";
            }
            else
            {
                Mat mat = new Mat(file);
                Sm13TxtBox.Text = (Zoom * mat.Width / 2).ToString();
                Sm23TxtBox.Text = (Zoom * mat.Height / 2).ToString();
            }
        }
        private void ZeroCropAfterBtn_Click(object sender, EventArgs e) => ZeroCropAfter(true);
        private void ZeroCropAfter(bool ReloadImg = false)
        {
            bool AutoReloadSave = AutoReloadChkBox.Checked;
            AutoReloadChkBox.Checked = false;

            XAfterTxtBox.Text = "0";
            YAfterTxtBox.Text = "0";
            dXAfterTxtBox.Text = "0";
            dYAfterTxtBox.Text = "0";

            AutoReloadChkBox.Checked = AutoReloadSave;
            if(ReloadImg) OpenCvReloadImg();
        }
        private async void SaveAsBtn_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = fileEdit.GetDefoltDirectory();
            saveFileDialog.Filter = "Fixing img plan (*.oip)|*.oip|All files(*.*)|*.*";
            saveFileDialog.FilterIndex = 1;
            if (saveFileDialog.ShowDialog() == DialogResult.Cancel) return;
            if (await fileEdit.SaveJsonAsync(saveFileDialog.FileName, GetImgFixingSettings())) RezultRTB.Text = "Settings save in " + saveFileDialog.FileName;
            else RezultRTB.Text = fileEdit.ErrText;
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
                    imgFixingFile = openFileDialog.FileName;
                    TryReadSettings(false);
                    OpenCvReloadImg();
                }
            }
        }
        public bool TryReadSettings(bool fileLoad = false)
        {
            if (File.Exists(imgFixingFile))
            {
                try
                {
                    // Open the text file using a stream reader.
                    using (var sr = new StreamReader(imgFixingFile))
                    {
                        // Read the stream as a string, and write the string to the console.
                        string jsonString = sr.ReadToEnd();
                        if (jsonString != null)
                        {
                            ImgFixingSettings imgFixingSettings = JsonConvert.DeserializeObject<ImgFixingSettings>(jsonString);
                            return SetImgFixingSettings(imgFixingSettings);
                        }
                    }
                }
                catch (IOException e)
                {
                    return SetErr("The file could not be read: " + e.Message + "!!!\n");
                }
            }
            else return SetErr("Err TryReadSettings.файл загрузки не найден!!!\n Загруженны настройки поумолчанию.");
            return false;
        }
        private bool SetImgFixingSettings(ImgFixingSettings imgFixingSettings)
        {
            bool AutoReloadSave = AutoReloadChkBox.Checked;
            AutoReloadChkBox.Checked = false;

            DistChkBox.Checked = imgFixingSettings.Distortion;
            DistorSettings distorSettings = imgFixingSettings.DistorSettings;

            Zoom = imgFixingSettings.Zoom;
            ZoomLbl.Text = Zoom.ToString();
            rotation90 = imgFixingSettings.Rotation90;

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

            CropAfterChkBox.Checked = imgFixingSettings.CropAfterChkBox;
            XAfterTxtBox.Text = imgFixingSettings.XAfter.ToString();
            YAfterTxtBox.Text = imgFixingSettings.YAfter.ToString();
            dXAfterTxtBox.Text = imgFixingSettings.DXAfter.ToString();
            dYAfterTxtBox.Text = imgFixingSettings.DYAfter.ToString();

            AutoReloadChkBox.Checked = AutoReloadSave;
            return true;
        }
        private ImgFixingSettings GetImgFixingSettings()
        {
            DistorSettings distorSettings = new DistorSettings()
            {
                A = A,
                B = B,
                C = C,
                D = D,
                E = E,
                Sm11 = Sm11,
                Sm12 = Sm12,
                Sm13 = Sm13,
                Sm21 = Sm21,
                Sm22 = Sm22,
                Sm23 = Sm23,
                Sm31 = Sm31,
                Sm32 = Sm32,
                Sm33 = Sm33,
            };

            int Y = 0, X = 0, dY = 0, dX = 0;
            Int32.TryParse(XAfterTxtBox.Text, out X);
            Int32.TryParse(YAfterTxtBox.Text, out Y);
            Int32.TryParse(dYAfterTxtBox.Text, out dY);
            Int32.TryParse(dXAfterTxtBox.Text, out dX);
            XAfterTxtBox.Text = X.ToString();
            YAfterTxtBox.Text = Y.ToString();
            dYAfterTxtBox.Text = dY.ToString();
            dXAfterTxtBox.Text = dX.ToString();

            return new ImgFixingSettings
            {
                Dir = InputDirTxtBox.Text,
                File = InputFileTxtBox.Text,
                Rotation90 = rotation90,
                Zoom = Zoom,
                //Rotation = RotationChkBox.Checked,
                //CropBeforeChkBox = CropBeforeChkBox.Checked,

                Distortion = DistChkBox.Checked,
                DistorSettings = distorSettings,

                CropAfterChkBox = CropAfterChkBox.Checked,
                XAfter = X,
                YAfter = Y,
                DXAfter = dX,
                DYAfter = dY
            };
        }

        public static byte[] BitmapToByte(string path, Image img, int quality)
        {
            var JpegCodecInfo = ImageCodecInfo.GetImageEncoders().Where(x => x.FormatDescription == "JPEG").First();
            ImageCodecInfo jpegCodec = JpegCodecInfo;
            EncoderParameters encoderParams = new EncoderParameters(1);
            EncoderParameter qualityParam = new EncoderParameter(Encoder.Quality, quality);
            encoderParams.Param[0] = qualityParam;
            MemoryStream mss = new MemoryStream();
            FileStream fs = new FileStream(path, FileMode.Create, FileAccess.ReadWrite);

            img.Save(mss, jpegCodec, encoderParams);
            byte[] matriz = mss.ToArray();
            fs.Write(matriz, 0, matriz.Length);

            mss.Close();
            fs.Close();
            return matriz;
        }
        private void RBtnUpDn_Click(object sender, EventArgs e){Rotation90(false);ZeroCropAfter(true);}
        private void RBtnUp90_Click(object sender, EventArgs e) {Rotation90(true);ZeroCropAfter(true);}
        private void RBtnUp001_Click(object sender, EventArgs e) => Rotation(100);
        private void RBtnDn001_Click(object sender, EventArgs e) => Rotation(-100);
        private void RBtnUp01_Click(object sender, EventArgs e) => Rotation(10);
        private void RBtnDn01_Click(object sender, EventArgs e) => Rotation(-10);

        private void RezultRTB_TextChanged(object sender, EventArgs e)
        {
            if (RezultRTB.Text.IndexOf("unblok") != -1|| RezultRTB.Text.IndexOf("Unblok") != -1) UnBlock();
        }

        private void Rotation90(bool direction)
        {
            if (direction) rotation90++;
            else rotation90--;
            if (rotation90 < 0) rotation90 = 3;
            if (rotation90 > 3) rotation90 = 0;
            if (AutoReloadChkBox.Checked) OpenCvReloadImg();
        }
        private void Rotation(int N)
        {
            double.TryParse(Sm21TxtBox.Text, out Sm21);
            Sm21 += N;
            Sm12 -= N;
            Sm21TxtBox.Text = Sm21.ToString();
            Sm12TxtBox.Text = Sm12.ToString();
            if (AutoReloadChkBox.Checked) OpenCvReloadImg();
        }
        private void ShowGridСhckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (AutoReloadChkBox.Checked) OpenCvReloadImg();
        }
        private void ApplyBtn_Click_1(object sender, EventArgs e) => OpenCvReloadImg();
        private void OpenCvReloadImg()=>pictureBox1.BackgroundImage = MatToBitmap(EditImg());
        private Mat EditImg(string file = "")
        {
            if (string.IsNullOrEmpty(file) && !string.IsNullOrEmpty(InputDirTxtBox.Text) && !string.IsNullOrEmpty(InputFileTxtBox.Text))
                file = fileEdit.DirFile(InputDirTxtBox.Text, InputFileTxtBox.Text);
            if (!File.Exists(file))
            {
                SetErr("Err File: " + file + " не найден!!!");
                return new Mat();
            }
            return EditImg(Cv2.ImRead(file));
        }

        private Bitmap EditImg(Bitmap bitmap)
        {
            if (bitmap == null)
            {
                SetErr("Err EditImg.bitmap == null !!!");
                return null;
            }
            if(bitmap.Width == 0 || bitmap.Height == 0)
            {
                SetErr("Err bitmap.Width ==0 || bitmap.Height == 0 !!!");
                return null;
            }
            return MatToBitmap(EditImg(BitmapConverter.ToMat(bitmap)));
        }
        private Mat EditImg(Mat img)
        {
            if (Zoom != 0)
            {
                Mat blackImg = new Mat("Black.jpg");
                int Width = img.Width, Height = img.Height, x1 = (int)(Width * (Zoom - 1) / 2), y1 = (int)(Height * (Zoom - 1) / 2); 
                Cv2.Resize(blackImg, blackImg, new OpenCvSharp.Size(Width * Zoom, Height * Zoom));
                Mat roi = blackImg[y1, y1 + Height, x1, x1 + Width];
                Cv2.Resize(img, img, new OpenCvSharp.Size(roi.Width, roi.Height));
                Cv2.CopyTo(img, roi);
                img = blackImg;
            }

            if (rotation90 == 1) Cv2.Rotate(img, img, RotateFlags.Rotate90Clockwise);
            else if (rotation90 == 2) Cv2.Rotate(img, img, RotateFlags.Rotate180);
            else if (rotation90 == 3) Cv2.Rotate(img, img, RotateFlags.Rotate90Counterclockwise);

            Mat rezult = new Mat();
            if (DistChkBox.Checked)
            {
                double[] distCoeffs = new double[] { A, B, C, D, E };
                InputArray _cameraMatrix = InputArray.Create<double>(new double[,]
                    {
                        { Sm11, Sm12, Sm13 },
                        { Sm21, Sm22, Sm23 },
                        { Sm31, Sm32, Sm33 }
                    });
                InputArray _distCoeffs = InputArray.Create<double>(distCoeffs);
                Cv2.Undistort(img, rezult, _cameraMatrix, _distCoeffs);

                double[] array_ = (double[])distCoeffs.Clone();
                RezultRTB.Text = $"k1:{array_[0]};\n k2:{array_[1]}; \n k3:{array_[4]}; \n p1:{array_[3]}; \n p2:{array_[2]};";
            }
            else rezult = img;

            if (CropAfterChkBox.Checked)
            {
                if (string.IsNullOrEmpty(XAfterTxtBox.Text)) dYAfterTxtBox.Text = "0";
                if (string.IsNullOrEmpty(YAfterTxtBox.Text)) dYAfterTxtBox.Text = "0";
                if (string.IsNullOrEmpty(dXAfterTxtBox.Text)) dYAfterTxtBox.Text = rezult.Width.ToString();
                if (string.IsNullOrEmpty(dXAfterTxtBox.Text)) dYAfterTxtBox.Text = rezult.Width.ToString();

                int Y = 0, X = 0, dY = 0, dX = 0;
                Int32.TryParse(XAfterTxtBox.Text, out X);
                Int32.TryParse(YAfterTxtBox.Text, out Y);
                if (Y < 0) Y = 0; if (X < 0) X = 0;
                if (Y > rezult.Width) Y = rezult.Width / 2;
                if (X > rezult.Height) X = rezult.Height / 2;
                Int32.TryParse(dYAfterTxtBox.Text, out dY);
                Int32.TryParse(dXAfterTxtBox.Text, out dX);

                if (dY <= 0 || Y + dY > rezult.Height) dY = rezult.Height - Y;
                if (dX <= 0 || X + dX > rezult.Width) dX = rezult.Width - X;
                XAfterTxtBox.Text = X.ToString();
                YAfterTxtBox.Text = Y.ToString();
                dYAfterTxtBox.Text = dY.ToString();
                dXAfterTxtBox.Text = dX.ToString();

                Rect rect;
                if (dY != 0 || dX != 0) rect = new Rect(X, Y, dX, dY);
                else rect = new Rect(X, Y, rezult.Width - X, rezult.Height - Y);
                rezult = new Mat(rezult, rect);
            }

            if (ShowGridСhckBox.Checked)
            {
                int n = 6;
                Cv2.Line(rezult, rezult.Width / n, 0, rezult.Width / n, rezult.Height, Scalar.Red, 1);
                Cv2.Line(rezult, rezult.Width - rezult.Width / n, 0, rezult.Width - rezult.Width / n, rezult.Height, Scalar.Red, 1);
                Cv2.Line(rezult, 0, rezult.Height / n, rezult.Width, rezult.Height / n, Scalar.Red, 1);
                Cv2.Line(rezult, 0, rezult.Height - rezult.Height / n, rezult.Width, rezult.Height - rezult.Height / n, Scalar.Red, 1);
            }
            return rezult;
        }
        private Bitmap MatToBitmap(Mat mat)
        {
            if (mat.Width == 0 && mat.Height == 0) return null;
            else return BitmapConverter.ToBitmap(mat);
        }
        public string GetImgFixingPlan() => imgFixingFile;
        private bool SetErr(string errText)
        {
            IsErr = true;
            ErrText = errText;
            RezultRTB.Text = ErrText;
            return false;
        }
    }
}