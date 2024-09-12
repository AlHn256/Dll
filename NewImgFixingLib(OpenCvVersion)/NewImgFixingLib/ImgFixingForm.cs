using ImageMagick;
using ImgFixingLibOpenCvVersion.Models;
using Newtonsoft.Json;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace ImgFixingLibOpenCvVersion
{
    public partial class ImgFixingForm : Form
    {
        private int Xdn = 0, Ydn = 0, Xup = 0, Yup = 0;
        private double A = -0.13, B = 0.39, C = 0.08, D = 0, E = 0,  RotationAngle = 0;
        private double Sm11 = 1500, Sm12 = 0.0, Sm13 = 9.4;
        private double Sm21 = 0.0, Sm22 = 1500, Sm23 = 5.9;
        private double Sm31 = 0.0, Sm32 = 0.0,  Sm33 = 1.0;
        private FileEdit fileEdit = new FileEdit(new string[] { "*.jpeg", "*.jpg", "*.png", "*.bmp" });
        private const string imgDefoltFixingFile = "imgFixingSettings.fip";
        private string imgFixingFile = imgDefoltFixingFile;
        public event Action<int> ProcessChanged;
        public event Action<string> TextChanged;
        private Bitmap[] BitmapArray { get; set; }
        private bool IsErr { get; set; } = false;
        private string ErrText { get; set; } = string.Empty;

        public ImgFixingForm(string directory)
        {
            InitializeComponent();

            InputDirTxtBox.Text = directory;
            Load += OnLoad;

            AllowDrop = true;
            DragEnter += new DragEventHandler(WindowsForm_DragEnter);
            DragDrop += new DragEventHandler(WindowsForm_DragDrop);
            pictureBox1.AllowDrop = true;
            pictureBox1.DragEnter += new DragEventHandler(WindowsForm_DragEnter);
            pictureBox1.DragDrop += new DragEventHandler(WindowsForm_DragDrop);
        }
        public ImgFixingForm(string imgFixingPlan, string directory, bool fileLoad = false)
        {
            InitializeComponent();
            if (!string.IsNullOrEmpty(imgFixingPlan)) imgFixingFile = imgFixingPlan;
            TryReadSettings(fileLoad);
            InputDirTxtBox.Text = directory;
        }
        
        public ImgFixingForm(string imgFixingPlan, bool test = false )
        {
            InitializeComponent();
            if (!string.IsNullOrEmpty(imgFixingPlan)) imgFixingFile = imgFixingPlan;
            TryReadSettings(false);
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
        private void ReloadParams()
        {
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
        }
        private void OnLoad(object sender, EventArgs e) => ReloadParams();
        private void CorrectFiles_Click(object sender, EventArgs e) => FixImges();
        public bool FixImges(string outputDir = "")
        {
            ShowGridСhckBox.Checked = false;
            if (string.IsNullOrEmpty(outputDir))
            {
                if (!Directory.Exists(InputDirTxtBox.Text)) return false;
                outputDir = OutputDirTxtBox.Text;
            }

            if (!fileEdit.ChkDir(outputDir)) return false;
            if (!Directory.Exists(InputDirTxtBox.Text)) return false;
            //DistortMethod distortMethod = (DistortMethod)DistortionMetodComBox.SelectedItem;
            FileInfo[] fileList = fileEdit.SearchFiles(InputDirTxtBox.Text);
            foreach (var file in fileList)
            {
                string outputFileNumber = outputDir + "\\" + file.Name;
                EditImg(file.FullName).SaveImage(outputFileNumber);
                //File.WriteAllBytes(outputFileNumber, EditImg(file.FullName).ToByteArray());
            }
            return true;
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
            // DistortMethod distortMethod = (DistortMethod)DistortionMetodComBox.SelectedItem;
            FileInfo[] fileList = fileEdit.SearchFiles(InputDirTxtBox.Text);
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

        private void ReloadImg()
        {
            //string file = fileEdit.DirFile(InputDirTxtBox.Text, InputFileTxtBox.Text);
            //if (!File.Exists(file))
            //{
            //    RezultRTB.Text = "Err ReloadImg.File " + file + " не найден!!!";
            //    return;
            //}

            ////DistortMethod distortMethod = new DistortMethod();
            ////if (DistortionMetodComBox.SelectedItem == null ||
            ////    (DistortMethod)DistortionMetodComBox.SelectedItem == DistortMethod.Undefined ||
            ////    (DistortMethod)DistortionMetodComBox.SelectedItem == DistortMethod.Sentinel ||
            ////    (DistortMethod)DistortionMetodComBox.SelectedItem == DistortMethod.Polynomial ||
            ////    (DistortMethod)DistortionMetodComBox.SelectedItem == DistortMethod.Perspective ||
            ////    (DistortMethod)DistortionMetodComBox.SelectedItem == DistortMethod.Arc) return;

            ////distortMethod = (DistortMethod)DistortionMetodComBox.SelectedItem;

            //MagickImage magickImage = EditImg(file);
            //var imageData = magickImage.ToByteArray();
            
            //using (var ms = new MemoryStream(imageData))
            //{
            //    Bitmap MyImage = new Bitmap(ms);
            //    pictureBox1.BackgroundImage = MyImage;
            //}
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
            C = Math.Round(C,2);
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
            Sm13 = Math.Round(Convert.ToDouble(Sm13TxtBox.Text), 3);
            Sm13TxtBox.Text = Sm13.ToString();
            if (AutoReloadChkBox.Checked) OpenCvReloadImg();
        }
        private void Sm21TxtBox_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Sm21TxtBox.Text)) return;
            Sm21 = Math.Round(Convert.ToDouble(Sm21TxtBox.Text), 3);
            Sm21TxtBox.Text = Sm21.ToString();
            if (AutoReloadChkBox.Checked) OpenCvReloadImg();
        }

        private void Sm22TxtBox_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Sm22TxtBox.Text)) return;
            Sm22 = Math.Round(Convert.ToDouble(Sm22TxtBox.Text), 3);
            Sm22TxtBox.Text = Sm22.ToString();
            if (AutoReloadChkBox.Checked) OpenCvReloadImg();
        }

        private void Sm23TxtBox_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Sm23TxtBox.Text)) return;
            Sm23 = Math.Round(Convert.ToDouble(Sm23TxtBox.Text), 3 );
            Sm23TxtBox.Text = Sm23.ToString();
            if (AutoReloadChkBox.Checked) OpenCvReloadImg();
        }

        private void Sm31TxtBox_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Sm31TxtBox.Text)) return;
            Sm31 = Math.Round(Convert.ToDouble(Sm31TxtBox.Text), 3);
            Sm31TxtBox.Text = Sm31.ToString();
            if (AutoReloadChkBox.Checked) OpenCvReloadImg();
        }

        private void Sm32TxtBox_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Sm32TxtBox.Text)) return;
            Sm32 = Math.Round(Convert.ToDouble(Sm32TxtBox.Text), 3);
            Sm32TxtBox.Text = Sm32.ToString();
            if (AutoReloadChkBox.Checked) OpenCvReloadImg();
        }

        private void Sm33TxtBox_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Sm33TxtBox.Text)) return;
            Sm33 = Math.Round(Convert.ToDouble(Sm33TxtBox.Text), 3);
            Sm33TxtBox.Text = Sm33.ToString();
            if (AutoReloadChkBox.Checked) OpenCvReloadImg();
        }
        private void ABtnUp_Click(object sender, EventArgs e)
        {
            A = Convert.ToDouble(ATxtBox.Text);
            A = Math.Round(A + 0.01, 2);
            ATxtBox.Text = A.ToString();
            if(AutoReloadChkBox.Checked)OpenCvReloadImg();
        }

        private void ABtnDn_Click(object sender, EventArgs e)
        {
            A = Convert.ToDouble(ATxtBox.Text);
            A = Math.Round(A - 0.01, 2);
            ATxtBox.Text = A.ToString();
            if(AutoReloadChkBox.Checked)OpenCvReloadImg();
        }

        private void BBtnUp_Click(object sender, EventArgs e)
        {
            B = Convert.ToDouble(BTxtBox.Text);
            B = Math.Round(B + 0.01, 2);
            BTxtBox.Text = B.ToString();
            if(AutoReloadChkBox.Checked)OpenCvReloadImg();
        }

        private void BBtnDn_Click(object sender, EventArgs e)
        {
            B = Convert.ToDouble(BTxtBox.Text);
            B = Math.Round(B - 0.01, 2);
            BTxtBox.Text = B.ToString();
            if(AutoReloadChkBox.Checked)OpenCvReloadImg();
        }
        private void CBtnUp_Click(object sender, EventArgs e)
        {
            C = Convert.ToDouble(CTxtBox.Text);
            C = Math.Round(C+0.01,2);
            CTxtBox.Text = C.ToString();
            if(AutoReloadChkBox.Checked)OpenCvReloadImg();
        }

        private void CBtnDn_Click(object sender, EventArgs e)
        {
            C = Convert.ToDouble(CTxtBox.Text);
            C = Math.Round(C - 0.01,2);
            CTxtBox.Text = C.ToString();
            if(AutoReloadChkBox.Checked)OpenCvReloadImg();
        }
        private void DBtnUp_Click(object sender, EventArgs e)
        {
            D = Convert.ToDouble(DTxtBox.Text);
            D = Math.Round(D + 0.01, 2);
            DTxtBox.Text = D.ToString();
            if(AutoReloadChkBox.Checked)OpenCvReloadImg();
        }

        private void DBtnDn_Click(object sender, EventArgs e)
        {
            D = Convert.ToDouble(DTxtBox.Text);
            D = Math.Round(D - 0.01, 2);
            DTxtBox.Text = D.ToString();
            if(AutoReloadChkBox.Checked)OpenCvReloadImg();
        }
        private void EBtnUp_Click(object sender, EventArgs e)
        {
            E = Convert.ToDouble(ETxtBox.Text);
            E = Math.Round(E + 0.01, 2);
            ETxtBox.Text = E.ToString();
            if(AutoReloadChkBox.Checked)OpenCvReloadImg();
        }
        private void EBtnDn_Click(object sender, EventArgs e)
        {
            E = Convert.ToDouble(ETxtBox.Text);
            E = Math.Round(E - 0.01, 2);
            ETxtBox.Text = E.ToString();
            if(AutoReloadChkBox.Checked)OpenCvReloadImg();
        }

        private void DistortionMetodComBox_SelectedIndexChanged(object sender, EventArgs e) => ReloadImg();
        private void ApplyBtn_Click(object sender, EventArgs e) => ReloadImg();
        private void label5_Click(object sender, EventArgs e) => CropBeforeChkBox.Checked = !CropBeforeChkBox.Checked;
        //private void label3_Click(object sender, EventArgs e) => RotationChkBox.Checked = !RotationChkBox.Checked;
        //private void DistortionMetodLabel_Click(object sender, EventArgs e) => DistortionChkBox.Checked = !DistortionChkBox.Checked;

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            Xdn = e.X;
            Ydn = e.Y;
            RezultRTB.Text = "Dn X " + Xdn + " Y " + Ydn + "\n";
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            Xup = e.X;
            Yup = e.Y;
            RezultRTB.Text += "Up X " + Xup + " Y " + Yup + "\n";
            RezultRTB.Text += "e.Delta " + e.Delta + "e.Button " + e.Button + "e.Clicks " + e.Clicks + "e.Location " + e.Location.ToString();
        }

        private void pictur(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            string textdir = string.Empty, Dir = string.Empty;
            foreach (string file in files)
            {
                FileAttributes attr = File.GetAttributes(file);
                if ((attr & FileAttributes.Directory) == FileAttributes.Directory) textdir = file;
                else textdir = Path.GetDirectoryName(file);
                Dir = textdir;
            }
        }
        private void DistZeroBtn_Click(object sender, EventArgs e)
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

                Sm13TxtBox.Text = (mat.Width / 2).ToString();
                Sm23TxtBox.Text = (mat.Height / 2).ToString();
            }

            Sm11TxtBox.Text = "1500"; Sm12TxtBox.Text = "0"; 
            Sm21TxtBox.Text = "0";  Sm22TxtBox.Text = "1500"; 
            Sm31TxtBox.Text = "0";  Sm32TxtBox.Text = "0";  Sm33TxtBox.Text = "1";

            AutoReloadChkBox.Checked = AutoReloadSave;
            ZeroCropAfter();
            OpenCvReloadImg();
        }
        private void ZeroCropAfter()
        {
            bool AutoReloadSave = AutoReloadChkBox.Checked;
            AutoReloadChkBox.Checked = false;

            XAfterTxtBox.Text = "0";
            YAfterTxtBox.Text = "0";
            dXAfterTxtBox.Text = "0";
            dYAfterTxtBox.Text = "0";

            AutoReloadChkBox.Checked = AutoReloadSave;
            OpenCvReloadImg();
        }
        private void ZeroCropAfterBtn_Click(object sender, EventArgs e)=>ZeroCropAfter();
        private bool SetImgFixingSettings(ImgFixingSettings imgFixingSettings , bool fileLoad)
        {
            bool AutoReloadSave = AutoReloadChkBox.Checked;
            AutoReloadChkBox.Checked = false;


            DistChkBox.Checked = imgFixingSettings.Distortion;
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

            CropAfterChkBox.Checked = imgFixingSettings.CropAfterChkBox;
            XAfterTxtBox.Text = imgFixingSettings.XAfter.ToString();
            YAfterTxtBox.Text = imgFixingSettings.YAfter.ToString();
            dXAfterTxtBox.Text = imgFixingSettings.DXAfter.ToString();
            dYAfterTxtBox.Text = imgFixingSettings.DYAfter.ToString();

            AutoReloadChkBox.Checked = AutoReloadSave;
            //A = distorSettings.A;
            //B = distorSettings.B;
            //C = distorSettings.C;
            //D = distorSettings.D;
            //E = distorSettings.E;
            //Sm11 = distorSettings.Sm11;
            //Sm12 = distorSettings.Sm12;
            //Sm13 = distorSettings.Sm13;
            //Sm21 = distorSettings.Sm21;
            //Sm22 = distorSettings.Sm22;
            //Sm23 = distorSettings.Sm23;
            //Sm31 = distorSettings.Sm31;
            //Sm32 = distorSettings.Sm32;
            //Sm33 = distorSettings.Sm33;

            //if (imgFixingSettings == null) return false;
            //CropBeforeChkBox.Checked = imgFixingSettings.CropBeforeChkBox;
            //XBeforeTxtBox.Text = imgFixingSettings.XBefore.ToString();
            //YBeforeTxtBox.Text = imgFixingSettings.YBefore.ToString();
            //HeightBeforeTxtBox.Text = imgFixingSettings.HeightBefore.ToString();
            //HeightAfterTxtBox.Text = imgFixingSettings.HeightAfter.ToString();
            //WidthAfterTxtBox.Text = imgFixingSettings.WidthAfter.ToString();
            //WidthBeforeTxtBox.Text = imgFixingSettings.WidthBefore.ToString();
            //RotationChkBox.Checked = imgFixingSettings.Rotation;
            //RotValTxtBox.Text = imgFixingSettings.RotationAngle.ToString();
            ////DistortionChkBox.Checked = imgFixingSettings.Distortion;
            //var distortionMetods = Enum.GetValues(typeof(DistortMethod));
            ////DistortionMetodComBox.DataSource = distortionMetods;
            ////DistortionMetodComBox.SelectedIndex = DistortionMetodComBox.FindString(imgFixingSettings.DistortMethod.ToString());
            //ATxtBox.Text = imgFixingSettings.A.ToString();
            //BTxtBox.Text = imgFixingSettings.B.ToString();
            //CTxtBox.Text = imgFixingSettings.C.ToString();
            //DTxtBox.Text = imgFixingSettings.D.ToString();
            //if (fileLoad)
            //{
            //    InputDirTxtBox.Text = imgFixingSettings.Dir;
            //    InputFileTxtBox.Text = imgFixingSettings.File;
            //}
            return true;
        }
        private ImgFixingSettings GetImgFixingSettings()
        {
            DistorSettings distorSettings =  new DistorSettings()
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
                //Rotation = RotationChkBox.Checked,
                CropBeforeChkBox = CropBeforeChkBox.Checked,

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

        //public static byte[] BitmapToByte(Image img, int quality)
        //{
        //    ImageCodecInfo jpegCodec = ImageCodecInfo.GetImageEncoders().Where(codec => codec.MimeType == "image/jpeg").First();
        //    EncoderParameters encoderParams = new EncoderParameters(1);
        //    encoderParams.Param[0] = new EncoderParameter(Encoder.Quality, quality);
        //    MemoryStream mss = new MemoryStream();
        //    img.Save(mss, jpegCodec, encoderParams);
        //    byte[] matriz = mss.ToArray();

        //    mss.Close();
        //    return matriz;
        //}

        private async void SaveAsBtn_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = fileEdit.GetDefoltDirectory();
            saveFileDialog.Filter = "Fixing img plan (*.fip)|*.fip|All files(*.*)|*.*";
            saveFileDialog.FilterIndex = 1;
            if (saveFileDialog.ShowDialog() == DialogResult.Cancel)return;
            if (await fileEdit.SaveJsonAsync(saveFileDialog.FileName, GetImgFixingSettings())) RezultRTB.Text = "Settings save in " + saveFileDialog.FileName;
            else RezultRTB.Text = fileEdit.ErrText;
        }
        private void RBtnUpDn_Click(object sender, EventArgs e)
        {
            Rotation90(false);
            ZeroCropAfter();
        }
        private void RBtnUp90_Click(object sender, EventArgs e)
        {
            Rotation90(true);
            ZeroCropAfter();
        }
        private void RBtnUp001_Click(object sender, EventArgs e)=>Rotation(100);
        private void RBtnDn001_Click(object sender, EventArgs e) => Rotation(-100);
        private void RBtnUp01_Click(object sender, EventArgs e) => Rotation(10);
        private void RBtnDn01_Click(object sender, EventArgs e) => Rotation(-10);

        private int rotation90 = 0;
        private void Rotation90(bool direction)
        {
            if (direction) rotation90++;
            else rotation90--;
            if(rotation90 < 0) rotation90 = 3;
            if (rotation90 > 3) rotation90 = 0;
            if (AutoReloadChkBox.Checked) OpenCvReloadImg();
        }
        private void Rotation(int N)
        {
            Sm21 = Convert.ToDouble(Sm21TxtBox.Text);
            Sm21 += N;
            Sm12 -= N;
            //RotValTxtBox.Text = Sm21.ToString();
            Sm21TxtBox.Text = Sm21.ToString();
            Sm12TxtBox.Text = Sm12.ToString();
            if (AutoReloadChkBox.Checked) OpenCvReloadImg();
        }
        private void ShowGridСhckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (AutoReloadChkBox.Checked) OpenCvReloadImg();
        }

        private void LoadFrBtn_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = fileEdit.GetDefoltDirectory();
                openFileDialog.Filter = "Fixing img plan (*.fip)|*.fip|All files(*.*)|*.*";
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
        private Bitmap MagickImageToBitMap(MagickImage magickImage)
        {

            // MagickImage magickImage = EditImg(Image);
            //var imageData = ;
            MemoryStream ms = new MemoryStream(magickImage.ToByteArray());
            return new Bitmap(ms);
        }

        public Bitmap[] FixImgArray(Bitmap[] dataArray)
        {
            //var DataArray = dataArray.Select(x => BitmapConverter.ToMat(x)).ToArray();
            //if (DataArray.Length == 0) return new Bitmap[0];
            //DataArray = FixImgArray(DataArray);

            //return DataArray.Select(x => MagickImageToBitMap(x)).ToArray();
            return dataArray;
        }
        public Mat[] FixImgArray(Mat[] DataArray)
        {
            if (DataArray == null && BitmapArray == null) return new Mat[0];
            if (DataArray.Length == 0 && BitmapArray.Length == 0) return new Mat[0];
            return DataArray.Select(x => EditImg(x)).ToArray();
        }
        private void ApplyBtn_Click_1(object sender, EventArgs e) => OpenCvReloadImg();
        private void OpenCvReloadImg()
        {
            pictureBox1.BackgroundImage = MatToBitmap(EditImg());
            //pictureBox1.Image = MatToBitmap(EditImg());
        }

        private Mat EditImg(string file = "")
        {
            if(string.IsNullOrEmpty(file) && !string.IsNullOrEmpty(InputDirTxtBox.Text) && !string.IsNullOrEmpty(InputFileTxtBox.Text)) 
                file = fileEdit.DirFile(InputDirTxtBox.Text, InputFileTxtBox.Text);
            if (!File.Exists(file))
            {
                SetErr("Err File: " + file + " не найден!!!");
                return new Mat();
            }
            return EditImg(Cv2.ImRead(file));
        }

        private Mat EditImg(Mat img)
        {
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
                RezultRTB.Text = $"k1:{array_[0]};\n k2:{array_[1]}; \n k3:{array_[2]}; \n p1:{array_[3]}; \n p2:{array_[4]};";
            }
            else rezult = img;


            //180度
            //Cv2.Rotate(Src_Images, SRC2, RotateFlags.Rotate180);
            //270度
            //Cv2.Rotate(Src_Images, SRC1, RotateFlags.Rotate90Counterclockwise);
      


            if (CropAfterChkBox.Checked)
            {
                if (string.IsNullOrEmpty(XAfterTxtBox.Text)) dYAfterTxtBox.Text = "0";
                if (string.IsNullOrEmpty(YAfterTxtBox.Text)) dYAfterTxtBox.Text = "0";
                if (string.IsNullOrEmpty(dXAfterTxtBox.Text)) dYAfterTxtBox.Text = rezult.Width.ToString();
                if (string.IsNullOrEmpty(dXAfterTxtBox.Text)) dYAfterTxtBox.Text = rezult.Width.ToString();

                int Y = 0, X = 0, dY = 0, dX = 0;
                Int32.TryParse(XAfterTxtBox.Text, out X);
                Int32.TryParse(YAfterTxtBox.Text, out Y);
                if (Y < 0) Y = 0; if(X<0) X = 0;
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
                if (dY!=0 || dX!=0) rect = new Rect(X, Y,dX, dY);
                else rect = new Rect( X, Y, rezult.Width - X, rezult.Height - Y);
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

            //DistortMethod distortMethod = DistortMethod.Barrel;
            //if (CropBeforeChkBox.Checked)
            //{
            //    int X = 0, Y = 0, HeightPercent = 100, WidthPercent = 100;
            //    Int32.TryParse(XBeforeTxtBox.Text, out X);
            //    Int32.TryParse(YBeforeTxtBox.Text, out Y);
            //    Int32.TryParse(HeightBeforeTxtBox.Text, out HeightPercent);
            //    Int32.TryParse(WidthBeforeTxtBox.Text, out WidthPercent);

            //    MagickGeometry geometry = new MagickGeometry();
            //    geometry.Width = image.Width * WidthPercent / 100;
            //    geometry.Height = image.Height * HeightPercent / 100;
            //    geometry.X = X;
            //    geometry.Y = Y;
            //    image.Crop(geometry);
            //}

            //if (RotationAngle != 0 && RotationChkBox.Checked) image.Rotate((double)RotationAngle);
            //if (DistortionChkBox.Checked)
            //{
            //    if (distortMethod != DistortMethod.Undefined && distortMethod != DistortMethod.Sentinel && distortMethod != DistortMethod.Polynomial && distortMethod != DistortMethod.Perspective && distortMethod != DistortMethod.Arc) image.Distort(distortMethod, new double[] { (double)A, (double)B, (double)C, (double)D });
            //    // if (distortMethod == DistortMethod.Perspective) image.Distort(DistortMethod.Perspective, new double[] { 0, 0, 20, 60, 90, 0, 70, 63, 0, 90, 5, 83, 90, 90, 85, 88 });
            //    if (distortMethod == DistortMethod.Perspective) image.Distort(DistortMethod.Perspective, new double[] { 0.0, 20.60, 90.0, 70.63, 0.90, 5.83, 90.90, 85.88 });
            //    if (distortMethod == DistortMethod.Arc) image.Distort(DistortMethod.Arc, 360);
            //}

            //if (CropAfterChkBox.Checked)
            //{
            //    int X = 0, Y = 0, HeightPercent = 100, WidthPercent = 100;
            //    Int32.TryParse(XAfterTxtBox.Text, out X);
            //    Int32.TryParse(YAfterTxtBox.Text, out Y);
            //    Int32.TryParse(dYAfterTxtBox.Text, out HeightPercent);
            //    Int32.TryParse(dXAfterTxtBox.Text, out WidthPercent);

            //    MagickGeometry geometry = new MagickGeometry();
            //    geometry.Width = image.Width * WidthPercent / 100;
            //    geometry.Height = image.Height * HeightPercent / 100;
            //    geometry.X = X;
            //    geometry.Y = Y;
            //    image.Crop(geometry);
            //}

            //if (ShowGridСhckBox.Checked)
            //{
            //    IDrawable[] drawables = new IDrawable[] {
            //        new DrawableFillColor(MagickColors.Red),
            //        new DrawableLine(80 * image.Width / 100, 0, 80 * image.Width / 100, image.Height),
            //        new DrawableLine(20 * image.Width / 100, 0, 20 * image.Width / 100, image.Height),
            //        new DrawableLine(0, 80 * image.Height / 100, image.Width, 80 * image.Height / 100),
            //        new DrawableLine(0, 20 * image.Height / 100, image.Width, 20 * image.Height / 100)
            //    };

            //    image.Draw(drawables);
            //}

            return rezult;
        }
        private Bitmap MatToBitmap(Mat mat)
        {
            if(mat.Width == 0 && mat.Height ==0) return null;
            else return BitmapConverter.ToBitmap(mat);
        }
        public string GetImgFixingPlan() => imgFixingFile;
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
                            return SetImgFixingSettings(imgFixingSettings, fileLoad);
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

        private bool SetErr(string errText)
        {
            IsErr = true;   
            ErrText = errText;
            RezultRTB.Text = ErrText;
            return false;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e) => ReloadImg();
        internal bool CheckFixigImg(string imgFixingDir = "")
        {
            // ??todo перенести это в fileEdit
            if (string.IsNullOrEmpty(imgFixingDir)) imgFixingDir = OutputDirTxtBox.Text;
            if (!Directory.Exists(imgFixingDir)) return false;

            FileInfo[] fileList = fileEdit.SearchFiles(InputDirTxtBox.Text);
            for (int i = 0; i < fileList.Count(); i++)
                if (!File.Exists(imgFixingDir + "\\" + fileList[i].Name)) return false;

            return true;
        }
    }
}