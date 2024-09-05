using ImageMagick;
using NewImgFixingLib.Models;
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

namespace NewImgFixingLib
{
    public partial class ImgFixingForm : Form
    {
        private int Xdn = 0, Ydn = 0, Xup = 0, Yup = 0;
        private double A = -0.13, B = 0.39, C = 0.08, D = 0, E = 0,  RotationAngle = 0;
        private double Sm11 = 1261, Sm12 = 0.0, Sm13 = 9.4;
        private double Sm21 = 0.5, Sm22 = 1217, Sm23 = 5.9;
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
                File.WriteAllBytes(outputFileNumber, EditImg(file.FullName).ToByteArray());
               // Image Img = Image.FromFile(outputFileNumber);
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
                File.WriteAllBytes(outputFileNumber, EditImg(fileList[i].FullName).ToByteArray());
                //Image Img = Image.FromFile(outputFileNumber);
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
            string file = fileEdit.DirFile(InputDirTxtBox.Text, InputFileTxtBox.Text);
            if (!File.Exists(file))
            {
                RezultRTB.Text = "Err ReloadImg.File " + file + " не найден!!!";
                return;
            }

            //DistortMethod distortMethod = new DistortMethod();
            //if (DistortionMetodComBox.SelectedItem == null ||
            //    (DistortMethod)DistortionMetodComBox.SelectedItem == DistortMethod.Undefined ||
            //    (DistortMethod)DistortionMetodComBox.SelectedItem == DistortMethod.Sentinel ||
            //    (DistortMethod)DistortionMetodComBox.SelectedItem == DistortMethod.Polynomial ||
            //    (DistortMethod)DistortionMetodComBox.SelectedItem == DistortMethod.Perspective ||
            //    (DistortMethod)DistortionMetodComBox.SelectedItem == DistortMethod.Arc) return;

            //distortMethod = (DistortMethod)DistortionMetodComBox.SelectedItem;

            MagickImage magickImage = EditImg(file);
            var imageData = magickImage.ToByteArray();
            
            using (var ms = new MemoryStream(imageData))
            {
                Bitmap MyImage = new Bitmap(ms);
                pictureBox1.BackgroundImage = MyImage;
            }
        }

        private void ATxtBox_TextChanged(object sender, EventArgs e)
        {
            A = Math.Round(Convert.ToDouble(ATxtBox.Text), 2);
            ATxtBox.Text = A.ToString();
            if (AutoReloadChkBox.Checked) OpenCvReloadImg();
        }
        private void BTxtBox_TextChanged(object sender, EventArgs e)
        {
            B = Math.Round(Convert.ToDouble(BTxtBox.Text), 2);
            BTxtBox.Text = B.ToString();
            if (AutoReloadChkBox.Checked) OpenCvReloadImg();
        }
        private void CTxtBox_TextChanged(object sender, EventArgs e)
        {
            C = Convert.ToDouble(CTxtBox.Text);
            if (AutoReloadChkBox.Checked) OpenCvReloadImg();
        }
        private void DTxtBox_TextChanged(object sender, EventArgs e)
        {
            D = Convert.ToDouble(DTxtBox.Text);
            if (AutoReloadChkBox.Checked) OpenCvReloadImg();
        }
        private void ETxtBox_TextChanged(object sender, EventArgs e)
        {
            E = Convert.ToDouble(ETxtBox.Text);
            if (AutoReloadChkBox.Checked) OpenCvReloadImg();
        }
        private void Sm11TxtBox_TextChanged(object sender, EventArgs e)
        {
            Sm11 = Math.Round(Convert.ToDouble(Sm11TxtBox.Text), 2);
            Sm11TxtBox.Text = Sm11.ToString();
            if (AutoReloadChkBox.Checked) OpenCvReloadImg();
        }
        private void Sm12TxtBox_TextChanged(object sender, EventArgs e)
        {
            Sm12 = Math.Round(Convert.ToDouble(Sm12TxtBox.Text), 2);
            Sm12TxtBox.Text = Sm12.ToString();
            if (AutoReloadChkBox.Checked) OpenCvReloadImg();
        }
        private void Sm13TxtBox_TextChanged(object sender, EventArgs e)
        {
            Sm13 = Math.Round(Convert.ToDouble(Sm13TxtBox.Text), 2);
            Sm13TxtBox.Text = Sm13.ToString();
            if (AutoReloadChkBox.Checked) OpenCvReloadImg();
        }


        private void Sm21TxtBox_TextChanged(object sender, EventArgs e)
        {
            Sm21 = Math.Round(Convert.ToDouble(Sm21TxtBox.Text), 2);
            Sm21TxtBox.Text = Sm21.ToString();
            if (AutoReloadChkBox.Checked) OpenCvReloadImg();
        }

        private void Sm22TxtBox_TextChanged(object sender, EventArgs e)
        {
            Sm22 = Math.Round(Convert.ToDouble(Sm22TxtBox.Text), 2);
            Sm22TxtBox.Text = Sm22.ToString();
            if (AutoReloadChkBox.Checked) OpenCvReloadImg();
        }

        private void Sm23TxtBox_TextChanged(object sender, EventArgs e)
        {
            Sm23 = Math.Round(Convert.ToDouble(Sm23TxtBox.Text), 2);
            Sm23TxtBox.Text = Sm23.ToString();
            if (AutoReloadChkBox.Checked) OpenCvReloadImg();
        }

        private void Sm31TxtBox_TextChanged(object sender, EventArgs e)
        {
            Sm31 = Math.Round(Convert.ToDouble(Sm31TxtBox.Text), 2);
            Sm31TxtBox.Text = Sm31.ToString();
            if (AutoReloadChkBox.Checked) OpenCvReloadImg();
        }

        private void Sm32TxtBox_TextChanged(object sender, EventArgs e)
        {
            Sm32 = Math.Round(Convert.ToDouble(Sm32TxtBox.Text), 2);
            Sm32TxtBox.Text = Sm32.ToString();
            if (AutoReloadChkBox.Checked) OpenCvReloadImg();
        }

        private void Sm33TxtBox_TextChanged(object sender, EventArgs e)
        {
            Sm33 = Math.Round(Convert.ToDouble(Sm33TxtBox.Text), 2);
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
        //private void RBtnUp001_Click(object sender, EventArgs e)
        //{
        //    Decimal.TryParse(RotValTxtBox.Text, out RotationAngle);
        //    RotationAngle += 0.01m;
        //    RotValTxtBox.Text = RotationAngle.ToString();
        //    if(AutoReloadChkBox.Checked)OpenCvReloadImg();
        //}

        //private void RBtnDn001_Click(object sender, EventArgs e)
        //{
        //    Decimal.TryParse(RotValTxtBox.Text, out RotationAngle);
        //    RotationAngle -= 0.01m;
        //    RotValTxtBox.Text = RotationAngle.ToString();
        //    if(AutoReloadChkBox.Checked)OpenCvReloadImg();
        //}
        //private void RBtnUp01_Click(object sender, EventArgs e)
        //{
        //    Decimal.TryParse(RotValTxtBox.Text, out RotationAngle);
        //    RotationAngle += 0.1;
        //    RotValTxtBox.Text = RotationAngle.ToString();
        //    if(AutoReloadChkBox.Checked)OpenCvReloadImg();
        //}
        //private void RBtnDn01_Click(object sender, EventArgs e)
        //{
        //    Decimal.TryParse(RotValTxtBox.Text, out RotationAngle);
        //    RotationAngle -= 0.1;
        //    RotValTxtBox.Text = RotationAngle.ToString();
        //    if(AutoReloadChkBox.Checked)OpenCvReloadImg();
        //}



        //private void RotValTxtBox_TextChanged(object sender, EventArgs e)
        //{
        //    Decimal.TryParse(RotValTxtBox.Text, out RotationAngle);
        //    if(AutoReloadChkBox.Checked)OpenCvReloadImg();
        //}

        private void DistortionMetodComBox_SelectedIndexChanged(object sender, EventArgs e) => ReloadImg();
        private void ApplyBtn_Click(object sender, EventArgs e) => ReloadImg();
        private void label5_Click(object sender, EventArgs e) => CropBeforeChkBox.Checked = !CropBeforeChkBox.Checked;
        private void label3_Click(object sender, EventArgs e) => RotationChkBox.Checked = !RotationChkBox.Checked;
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
        private void InputDirTxtBox_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(InputDirTxtBox.Text)) return;
            var files = fileEdit.SearchFiles(InputDirTxtBox.Text);
            if (files[0]!=null)
            {
                InputFileTxtBox.Text = files[0].Name;
                pictureBox1.BackgroundImage = Image.FromFile(files[0].FullName);
            }

            OutputDirTxtBox.Text = InputDirTxtBox.Text.FirstOf('\\') + "\\" + InputDirTxtBox.Text.LastOf('\\') + "Out";
        }
        private void InputFileTxtBox_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(InputFileTxtBox.Text)) return;

            string file = fileEdit.DirFile(InputDirTxtBox.Text, InputFileTxtBox.Text);
            if (File.Exists(file)) pictureBox1.BackgroundImage = Image.FromFile(file);
        }
        private void DistZeroBtn_Click(object sender, EventArgs e)
        {
            bool AutoReloadSave = AutoReloadChkBox.Checked;
            AutoReloadChkBox.Checked = false;

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

            Sm11TxtBox.Text = "1261"; Sm12TxtBox.Text = "0"; Sm13TxtBox.Text = "9,4";
            Sm21TxtBox.Text = "0,5";  Sm22TxtBox.Text = "1217"; Sm23TxtBox.Text = "5,9";
            Sm31TxtBox.Text = "0";  Sm32TxtBox.Text = "0";  Sm33TxtBox.Text = "1";

            AutoReloadChkBox.Checked = AutoReloadSave;
            OpenCvReloadImg();
        }

        private bool SetImgFixingSettings(DistorSettings distorSettings, bool fileLoad)
        {
            A = distorSettings.A;
            ATxtBox.Text = distorSettings.A.ToString();
            B = distorSettings.B;
            BTxtBox.Text = distorSettings.B.ToString();
            C = distorSettings.C;
            CTxtBox.Text = distorSettings.C.ToString();
            D = distorSettings.D;
            DTxtBox.Text = distorSettings.D.ToString();
            E = distorSettings.E;
            ETxtBox.Text = distorSettings.E.ToString();
            Sm11 = distorSettings.Sm11;
            Sm12 = distorSettings.Sm12;
            Sm13 = distorSettings.Sm13;
            Sm21 = distorSettings.Sm21;
            Sm22 = distorSettings.Sm22;
            Sm23 = distorSettings.Sm23;
            Sm31 = distorSettings.Sm31;
            Sm32 = distorSettings.Sm32;
            Sm33 = distorSettings.Sm33;



            //if (imgFixingSettings == null) return false;
            //CropBeforeChkBox.Checked = imgFixingSettings.CropBeforeChkBox;
            //CropAfterChkBox.Checked = imgFixingSettings.CropAfterChkBox;
            //XBeforeTxtBox.Text = imgFixingSettings.XBefore.ToString();
            //XAfterTxtBox.Text = imgFixingSettings.XAfter.ToString();
            //YBeforeTxtBox.Text = imgFixingSettings.YBefore.ToString();
            //YAfterTxtBox.Text = imgFixingSettings.YAfter.ToString();
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
        private DistorSettings GetImgFixingSettings()
        {
            return new DistorSettings()
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

        //int X = 0, Y = 0, HeightPercent = 100, WidthPercent = 100;
        //    Int32.TryParse(XBeforeTxtBox.Text, out X);
        //    Int32.TryParse(YBeforeTxtBox.Text, out Y);
        //    Int32.TryParse(HeightBeforeTxtBox.Text, out HeightPercent);
        //    Int32.TryParse(WidthBeforeTxtBox.Text, out WidthPercent);

        //    int Xa = 0, Ya = 0, HeightAPercent = 100, WidthAPercent = 100;
        //    Int32.TryParse(XAfterTxtBox.Text, out Xa);
        //    Int32.TryParse(YAfterTxtBox.Text, out Ya);
        //    Int32.TryParse(HeightAfterTxtBox.Text, out HeightAPercent);
        //    Int32.TryParse(WidthAfterTxtBox.Text, out WidthAPercent);

        //    return new ImgFixingSettings
        //    {
        //        Dir = InputDirTxtBox.Text,
        //        File = InputFileTxtBox.Text,
        //        CropBeforeChkBox = CropBeforeChkBox.Checked,
        //        XBefore = X,
        //        YBefore = Y,
        //        HeightBefore = HeightPercent,
        //        WidthBefore = WidthPercent,
        //        Rotation = RotationChkBox.Checked,
        //        A = A,
        //        B = B,
        //        C = C,
        //        D = D,
        //        CropAfterChkBox = CropAfterChkBox.Checked,
        //        XAfter = Xa,
        //        YAfter = Ya,
        //        HeightAfter = HeightAPercent,
        //        WidthAfter = WidthAPercent
        //    };
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

        public Bitmap[] FixImgArray(Bitmap[] dataArray)
        {
            var DataArray = dataArray.Select(x => { return new MagickImage(BitmapToByte("Test.jpg",x, 99)); }).ToArray();
            if(DataArray.Length == 0)return new Bitmap[0];
            DataArray = FixImgArray(DataArray);

            return DataArray.Select(x => MagickImageToBitMap(x)).ToArray();
        }
        private async void SaveAsBtn_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = fileEdit.GetDefoltDirectory();
            saveFileDialog.Filter = "Fixing img plan (*.fip)|*.fip|All files(*.*)|*.*";
            saveFileDialog.FilterIndex = 1;
            if (saveFileDialog.ShowDialog() == DialogResult.Cancel)
                return;

            if (await fileEdit.SaveJsonAsync(saveFileDialog.FileName, GetImgFixingSettings())) RezultRTB.Text = "Settings save in " + saveFileDialog.FileName;
            else RezultRTB.Text = fileEdit.ErrText;
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

        public MagickImage[] FixImgArray(MagickImage[] DataArray)
        {
            if (DataArray == null && BitmapArray == null) return new MagickImage[0];
            if (DataArray.Length == 0 && BitmapArray.Length == 0) return new MagickImage[0];

            return DataArray.Select(x => EditImg(x)).ToArray();
        }
        private MagickImage EditImg(string InputFile)
        {
            return EditImg(new MagickImage(InputFile));
        }
        private MagickImage EditImg(MagickImage image)
        {
            DistortMethod distortMethod = DistortMethod.Barrel;

            if (CropBeforeChkBox.Checked)
            {
                int X = 0, Y = 0, HeightPercent = 100, WidthPercent = 100;
                Int32.TryParse(XBeforeTxtBox.Text, out X);
                Int32.TryParse(YBeforeTxtBox.Text, out Y);
                Int32.TryParse(HeightBeforeTxtBox.Text, out HeightPercent);
                Int32.TryParse(WidthBeforeTxtBox.Text, out WidthPercent);

                MagickGeometry geometry = new MagickGeometry();
                geometry.Width = image.Width * WidthPercent / 100;
                geometry.Height = image.Height * HeightPercent / 100;
                geometry.X = X;
                geometry.Y = Y;
                image.Crop(geometry);
            }

            if (RotationAngle != 0 && RotationChkBox.Checked) image.Rotate((double)RotationAngle);
            //if (DistortionChkBox.Checked)
            //{
            //    if (distortMethod != DistortMethod.Undefined && distortMethod != DistortMethod.Sentinel && distortMethod != DistortMethod.Polynomial && distortMethod != DistortMethod.Perspective && distortMethod != DistortMethod.Arc) image.Distort(distortMethod, new double[] { (double)A, (double)B, (double)C, (double)D });
            //    // if (distortMethod == DistortMethod.Perspective) image.Distort(DistortMethod.Perspective, new double[] { 0, 0, 20, 60, 90, 0, 70, 63, 0, 90, 5, 83, 90, 90, 85, 88 });
            //    if (distortMethod == DistortMethod.Perspective) image.Distort(DistortMethod.Perspective, new double[] { 0.0, 20.60, 90.0, 70.63, 0.90, 5.83, 90.90, 85.88 });
            //    if (distortMethod == DistortMethod.Arc) image.Distort(DistortMethod.Arc, 360);
            //}

            if (CropAfterChkBox.Checked)
            {
                int X = 0, Y = 0, HeightPercent = 100, WidthPercent = 100;
                Int32.TryParse(XAfterTxtBox.Text, out X);
                Int32.TryParse(YAfterTxtBox.Text, out Y);
                Int32.TryParse(HeightAfterTxtBox.Text, out HeightPercent);
                Int32.TryParse(WidthAfterTxtBox.Text, out WidthPercent);

                MagickGeometry geometry = new MagickGeometry();
                geometry.Width = image.Width * WidthPercent / 100;
                geometry.Height = image.Height * HeightPercent / 100;
                geometry.X = X;
                geometry.Y = Y;
                image.Crop(geometry);
            }

            if (ShowGridСhckBox.Checked)
            {
                IDrawable[] drawables = new IDrawable[] {
                    new DrawableFillColor(MagickColors.Red),
                    new DrawableLine(80 * image.Width / 100, 0, 80 * image.Width / 100, image.Height),
                    new DrawableLine(20 * image.Width / 100, 0, 20 * image.Width / 100, image.Height),
                    new DrawableLine(0, 80 * image.Height / 100, image.Width, 80 * image.Height / 100),
                    new DrawableLine(0, 20 * image.Height / 100, image.Width, 20 * image.Height / 100)
                };

                image.Draw(drawables);
            }

            return image;
        }

        private void ApplyBtn_Click_1(object sender, EventArgs e)=>OpenCvReloadImg();

        private void OpenCvReloadImg()
        {

            string file = fileEdit.DirFile(InputDirTxtBox.Text, InputFileTxtBox.Text);
            if (!File.Exists(file))
            {
                RezultRTB.Text = "Err ReloadImg.File " + file + " не найден!!!";
                return;
            }

            //OpenCvSharp.Size board_size = new OpenCvSharp.Size(9, 6);

            /// <summary>
            /// Фактический размер каждой шахматной доски на калибровочной доске
            /// </summary>
            //OpenCvSharp.Size square_size = new OpenCvSharp.Size(10, 10);
            /// <summary>
            /// Матрица параметров в камере
            /// </summary>

            Mat cameraMatrix = new Mat(3, 3, MatType.CV_32FC1, Scalar.All(0));
            /// <summary>
            /// 5 коэффициентов искажения камеры: k1,k2,p1,p2,k3
            /// </summary>
            Mat distCoeffs = new Mat(1, 5, MatType.CV_32FC1, Scalar.All(0));

            //double[,] cameraMatrix_ = new double[,]
            //{
            //    { 0, 0, 1151.7 },
            //    { 0, 985.6, 0 },
            //    { 985.6, 0, 1153.4 }
            //};


            //camera_matrix = np.array(
            //      [[1.26125746e+03, 0.00000000e+00, 9.40592038e+02],
            //      [0.00000000e+00, 1.21705719e+03, 5.96848905e+02],
            //      [0.00000000e+00, 0.00000000e+00, 1.00000000e+00]]);
            //dist_coefs = np.array([-3.18345478e+01, 7.26874187e+02, -1.20480816e-01, 9.43789095e-02, 5.28916586e-01]);


            //K.put(0, 0, 1151.7, 0, 985.6, 0, 1153.4, 573.004, 0, 0, 1);
            //double[] cameraMatrix_ = new double[] { 0, 0, 1151.7, 0, 985.6, 0, 1153.4, 573.004, 0, 0, 1 };

            //double[] distCoeffs_ = new double[] { distCoeffs.Get<double>(0), distCoeffs.Get<double>(1), distCoeffs.Get<double>(2), distCoeffs.Get<double>(3), distCoeffs.Get<double>(4) };
            //D.put(0, 0, -0.42, 0.2196, 0.0038, 0.013, -0.06);
            //double[] distCoeffs_ = new double[] { -3.18345478e+01, 7.26874187e+02, -1.20480816e-01, 9.43789095e-02, 5.28916586e-01 };

            Mat img = Cv2.ImRead(file);
            //Mat img = Cv2.ImRead(@"E:\Work\Exampels\14\40.bmp");
            Mat correct = new Mat();

            double[,] cameraMatrix_ = new double[,]
                {
                    { Sm11, Sm12, Sm13},
                    { Sm21, Sm22, Sm23 },
                    { Sm31, Sm32, Sm33 }
                };

            double[] distCoeffs_ = new double[] { Math.Round(A, 2), Math.Round(B, 2), Math.Round(C, 2), Math.Round(D, 2), Math.Round(E, 2) };

            InputArray _cameraMatrix = InputArray.Create<double>(cameraMatrix_);
            InputArray _distCoeffs = InputArray.Create<double>(distCoeffs_);

            //Cv2.Undistort(img, correct, K, D);
            Cv2.Undistort(img, correct, _cameraMatrix, _distCoeffs);
            //Cv2.Undistort(img, correct, 10 , null, _cameraMatrix);


            //Cv2.Resize(undistorted, undistortedScaled, Size(0, 0), scaleX, scaleY);

            //pictureBox1.Image = null;
            pictureBox1.BackgroundImage = MatToBitmap(correct);

            //       Point3f[][] obj_points = new Point3f[calibFrames.Count][];
            //       Point2f[][] img_points = new Point2f[calibFrames.Count][];
            //       int index = 0;
            //       foreach (CalibFrame frame in calibFrames)
            //       {
            //           img_points[index] = frame.Corners;
            //           obj_points[index] = new Point3f[board_size.Width * board_size.Height];

            //           int index1 = 0;
            //           for (float y = 0; y < board_size.Height; y++)
            //               for (float x = 0; x < board_size.Width; x++)
            //               {
            //                   Point3f p = new Point3f(x * square_size.Width, y * square_size.Height, 0);
            //                   obj_points[index][index1] = p;
            //                   index1++;
            //               }

            //           /*for (int i = 0; i < obj_points[index].Length; i++)
            //obj_points[index][i] = new Point3f(i * (float)10, i % (float)10, 0);*/
            //           index++;
            //       }

            //double error = Cv2.CalibrateCamera(obj_points, img_points, calibFrames.First().Gray.Size(), cameraMatrix_, distCoeffs_, out var revecs, out var tvecs);

            double[] array_ = (double[])distCoeffs_.Clone();
            string params_ = $"k1:{array_[0]};\n k2:{array_[1]}; \n k3:{array_[2]}; \n p1:{array_[3]}; \n p2:{array_[4]};";
            RezultRTB.Text = params_;

            //InputArray _cameraMatrix = InputArray.Create<double>(cameraMatrix_);
            //InputArray _distCoeffs = InputArray.Create<double>(distCoeffs_);

            //Cv2.Undistort(img, correct, _cameraMatrix, _distCoeffs);

            //Cv2.Rema

            //pictureBox1.Image = null;
            //pictureBox1.Image = MatToBitmap(correct);

            //img_objects = corners
            //Mat[] tvecMat = new Mat[] { };
            //Mat[] rvecsMat = new Mat[] { };

            //OpenCvSharp.Size imageSize = new OpenCvSharp.Size(0,0);

            //int index = 0;
            //foreach (CalibFrame frame in calibFrames)
            //{
            //	obj_points[index] = new Mat(frame.Gray.Rows, frame.Gray.Cols, frame.Gray.Type(), Scalar.All(0));

            //	img_points[index] = new Mat(frame.Corners.Count(), 2, MatType.CV_32FC1);

            //	index++;

            //	imageSize = new OpenCvSharp.Size(frame.Gray.Width, frame.Gray.Height);
            //}
            //Cv2.CalibrateCamera(obj_points, img_points, imageSize, cameraMatrix, distCoeffs, out tvecMat, out rvecsMat);

        }

        Bitmap MatToBitmap(Mat mat)
        {
            //mat.ConvertTo(mat, MatType.CV_8U);
            return BitmapConverter.ToBitmap(mat);
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
                            DistorSettings imgFixingSettings = JsonConvert.DeserializeObject<DistorSettings>(jsonString);
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