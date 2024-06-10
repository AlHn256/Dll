using ImageMagick;
using ImgFixingLib.Models;
using System.Text.Json;

namespace ImgFixingLib
{
    public partial class ImgFixingForm : Form
    {
        private static int fileNumber = 0;
        private int Xdn = 0, Ydn = 0, Xup = 0, Yup = 0;
        private decimal A = 0.07m, B = -0.16m, C = -0.32m, D = 1.54m, RotationAngle = 0;
        private FileEdit fileEdit = new FileEdit(["*.jpeg", "*.jpg", "*.png", "*.bmp"]);
        private const string imgDefoltFixingFile = "imgFixingSettings.fip";
        private string imgFixingFile = imgDefoltFixingFile;
        public event Action<int> ProcessChanged;
        public event Action<string> TextChanged;
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
            DistortionMetodComBox.Enabled = false;
        }

        public ImgFixingForm(string imgFixingPlan, string directory, bool fileLoad = false)
        {
            InitializeComponent();
            if (!string.IsNullOrEmpty(imgFixingPlan)) imgFixingFile = imgFixingPlan;
            TryReadSettings(fileLoad);
            InputDirTxtBox.Text = directory;
        }

        public void OnProgressChanged(object i)
        {
            if (ProcessChanged != null) ProcessChanged((int)i);
        }
        public void OnTextChanged(object txt)
        {
            if (TextChanged != null) TextChanged((string)txt);
        }

        void WindowsForm_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }
        void WindowsForm_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files == null) return;
            if (files.Length == 0) return;
            if (files.Length == 1) InputDirTxtBox.Text = Path.GetDirectoryName(files[0]);
            InputFileTxtBox.Text = Path.GetFileName(files[0]);
        }

        private void OnLoad(object? sender, EventArgs e)
        {
            ATxtBox.Text = A.ToString();
            BTxtBox.Text = B.ToString();
            CTxtBox.Text = C.ToString();
            DTxtBox.Text = D.ToString();
            RotValTxtBox.Text = RotationAngle.ToString();

            var distortionMetods = Enum.GetValues(typeof(DistortMethod));
            DistortionMetodComBox.DataSource = distortionMetods;
            DistortionMetodComBox.SelectedItem = DistortMethod.Barrel;

            return;
        }
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
            DistortMethod distortMethod = (DistortMethod)DistortionMetodComBox.SelectedItem;
            FileInfo[] fileList = fileEdit.SearchFiles(InputDirTxtBox.Text);
            foreach (var file in fileList)
            {
                string outputFileNumber = outputDir + "\\" + file.Name;
                File.WriteAllBytes(outputFileNumber, EditImg(file.FullName, distortMethod).ToByteArray());
                Image Img = Image.FromFile(outputFileNumber);
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

            DistortMethod distortMethod = (DistortMethod)DistortionMetodComBox.SelectedItem;
            FileInfo[] fileList = fileEdit.SearchFiles(InputDirTxtBox.Text);
            for (int i = 0; i < fileList.Count(); i++)
            {
                string outputFileNumber = outputDir + "\\" + fileList[i].Name;
                File.WriteAllBytes(outputFileNumber, EditImg(fileList[i].FullName, distortMethod).ToByteArray());
                Image Img = Image.FromFile(outputFileNumber);
                context.Send(OnProgressChanged, i * 100 / fileList.Count());
                context.Send(OnTextChanged, "Imges Fixing " + i * 100 / fileList.Count() + " %");
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
            for (int i = 0; i < fileList.Count(); i++)
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

            DistortMethod distortMethod = new DistortMethod();
            if (DistortionMetodComBox.SelectedItem == null ||
                (DistortMethod)DistortionMetodComBox.SelectedItem == DistortMethod.Undefined ||
                (DistortMethod)DistortionMetodComBox.SelectedItem == DistortMethod.Sentinel ||
                (DistortMethod)DistortionMetodComBox.SelectedItem == DistortMethod.Polynomial ||
                (DistortMethod)DistortionMetodComBox.SelectedItem == DistortMethod.Perspective ||
                (DistortMethod)DistortionMetodComBox.SelectedItem == DistortMethod.Arc) return;

            distortMethod = (DistortMethod)DistortionMetodComBox.SelectedItem;

            MagickImage magickImage = EditImg(file, distortMethod);
            var imageData = magickImage.ToByteArray();
            Bitmap MyImage;
            using (var ms = new MemoryStream(imageData))
            {
                MyImage = new Bitmap(ms);
                pictureBox1.BackgroundImage = (Image)MyImage;
            }
        }
        private void ABtnUp_Click(object sender, EventArgs e)
        {
            A = Convert.ToDecimal(ATxtBox.Text);
            A += 0.01m;
            ATxtBox.Text = A.ToString();
            ReloadImg();
        }

        private void ABtnDn_Click(object sender, EventArgs e)
        {
            A = Convert.ToDecimal(ATxtBox.Text);
            A -= 0.01m;
            ATxtBox.Text = A.ToString();
            //ReloadImg();
        }

        private void BBtnUp_Click(object sender, EventArgs e)
        {
            B = Convert.ToDecimal(BTxtBox.Text);
            B += 0.01m;
            BTxtBox.Text = B.ToString();
            //ReloadImg();
        }

        private void BBtnDn_Click(object sender, EventArgs e)
        {
            B = Convert.ToDecimal(BTxtBox.Text);
            B -= 0.01m;
            BTxtBox.Text = B.ToString();
            //ReloadImg();
        }
        private void CBtnUp_Click(object sender, EventArgs e)
        {
            C = Convert.ToDecimal(CTxtBox.Text);
            C += 0.01m;
            CTxtBox.Text = C.ToString();
            //ReloadImg();
        }

        private void CBtnDn_Click(object sender, EventArgs e)
        {
            C = Convert.ToDecimal(CTxtBox.Text);
            C -= 0.01m;
            CTxtBox.Text = C.ToString();
            //ReloadImg();
        }
        private void DBtnUp_Click(object sender, EventArgs e)
        {
            D = Convert.ToDecimal(DTxtBox.Text);
            D += 0.01m;
            DTxtBox.Text = D.ToString();
            //ReloadImg();
        }

        private void DBtnDn_Click(object sender, EventArgs e)
        {
            D = Convert.ToDecimal(DTxtBox.Text);
            D -= 0.01m;
            DTxtBox.Text = D.ToString();
            //ReloadImg();
        }
        private void RBtnUp001_Click(object sender, EventArgs e)
        {
            Decimal.TryParse(RotValTxtBox.Text, out RotationAngle);
            RotationAngle += 0.01m;
            RotValTxtBox.Text = RotationAngle.ToString();
            //ReloadImg();
        }

        private void RBtnDn001_Click(object sender, EventArgs e)
        {
            Decimal.TryParse(RotValTxtBox.Text, out RotationAngle);
            RotationAngle -= 0.01m;
            RotValTxtBox.Text = RotationAngle.ToString();
            //ReloadImg();
        }
        private void RBtnUp01_Click(object sender, EventArgs e)
        {
            Decimal.TryParse(RotValTxtBox.Text, out RotationAngle);
            RotationAngle += 0.1m;
            RotValTxtBox.Text = RotationAngle.ToString();
            ReloadImg();
        }
        private void RBtnDn01_Click(object sender, EventArgs e)
        {
            Decimal.TryParse(RotValTxtBox.Text, out RotationAngle);
            RotationAngle -= 0.1m;
            RotValTxtBox.Text = RotationAngle.ToString();
            //ReloadImg();
        }
        private void DTxtBox_TextChanged(object sender, EventArgs e)
        {
            D = Convert.ToDecimal(DTxtBox.Text);
            //ReloadImg();
        }

        private void CTxtBox_TextChanged(object sender, EventArgs e)
        {
            C = Convert.ToDecimal(CTxtBox.Text);
            //ReloadImg();
        }

        private void BTxtBox_TextChanged(object sender, EventArgs e)
        {
            B = Convert.ToDecimal(BTxtBox.Text);
            //ReloadImg();
        }

        private void ATxtBox_TextChanged(object sender, EventArgs e)
        {
            A = Convert.ToDecimal(ATxtBox.Text);
            //ReloadImg();
        }
        private void RotValTxtBox_TextChanged(object sender, EventArgs e)
        {
            Decimal.TryParse(RotValTxtBox.Text, out RotationAngle);
            ReloadImg();
        }

        private void DistortionMetodComBox_SelectedIndexChanged(object sender, EventArgs e) => ReloadImg();
        private void ApplyBtn_Click(object sender, EventArgs e) => ReloadImg();
        private void label5_Click(object sender, EventArgs e) => CropBeforeChkBox.Checked = !CropBeforeChkBox.Checked;
        private void label3_Click(object sender, EventArgs e) => RotationChkBox.Checked = !RotationChkBox.Checked;
        private void DistortionMetodLabel_Click(object sender, EventArgs e) => DistortionChkBox.Checked = !DistortionChkBox.Checked;

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
            if (files.Length > 0)
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
            A = 0;
            B = 0;
            C = 0;
            D = 1;
            ATxtBox.Text = "0";
            BTxtBox.Text = "0";
            CTxtBox.Text = "0";
            DTxtBox.Text = "1";
            ReloadImg();
        }

        private bool SetImgFixingSettings(ImgFixingSettings? imgFixingSettings, bool fileLoad)
        {
            if (imgFixingSettings == null) return false;

            CropBeforeChkBox.Checked = imgFixingSettings.CropBeforeChkBox;
            CropAfterChkBox.Checked = imgFixingSettings.CropAfterChkBox;
            XBeforeTxtBox.Text = imgFixingSettings.XBefore.ToString();
            XAfterTxtBox.Text = imgFixingSettings.XAfter.ToString();
            YBeforeTxtBox.Text = imgFixingSettings.YBefore.ToString();
            YAfterTxtBox.Text = imgFixingSettings.YAfter.ToString();
            HeightBeforeTxtBox.Text = imgFixingSettings.HeightBefore.ToString();
            HeightAfterTxtBox.Text = imgFixingSettings.HeightAfter.ToString();
            WidthAfterTxtBox.Text = imgFixingSettings.WidthAfter.ToString();
            WidthBeforeTxtBox.Text = imgFixingSettings.WidthBefore.ToString();
            RotationChkBox.Checked = imgFixingSettings.Rotation;
            RotValTxtBox.Text = imgFixingSettings.RotationAngle.ToString();
            DistortionChkBox.Checked = imgFixingSettings.Distortion;
            var distortionMetods = Enum.GetValues(typeof(DistortMethod));
            DistortionMetodComBox.DataSource = distortionMetods;
            DistortionMetodComBox.SelectedIndex = DistortionMetodComBox.FindString(imgFixingSettings.DistortMethod.ToString());
            ATxtBox.Text = imgFixingSettings.A.ToString();
            BTxtBox.Text = imgFixingSettings.B.ToString();
            CTxtBox.Text = imgFixingSettings.C.ToString();
            DTxtBox.Text = imgFixingSettings.D.ToString();
            if (fileLoad)
            {
                InputDirTxtBox.Text = imgFixingSettings.Dir;
                InputFileTxtBox.Text = imgFixingSettings.File;
            }
            return true;
        }
        private ImgFixingSettings GetImgFixingSettings()
        {
            int X = 0, Y = 0, HeightPercent = 100, WidthPercent = 100;
            Int32.TryParse(XBeforeTxtBox.Text, out X);
            Int32.TryParse(YBeforeTxtBox.Text, out Y);
            Int32.TryParse(HeightBeforeTxtBox.Text, out HeightPercent);
            Int32.TryParse(WidthBeforeTxtBox.Text, out WidthPercent);

            int Xa = 0, Ya = 0, HeightAPercent = 100, WidthAPercent = 100;
            Int32.TryParse(XAfterTxtBox.Text, out Xa);
            Int32.TryParse(YAfterTxtBox.Text, out Ya);
            Int32.TryParse(HeightAfterTxtBox.Text, out HeightAPercent);
            Int32.TryParse(WidthAfterTxtBox.Text, out WidthAPercent);

            return new ImgFixingSettings
            {
                Dir = InputDirTxtBox.Text,
                File = InputFileTxtBox.Text,
                CropBeforeChkBox = CropBeforeChkBox.Checked,
                XBefore = X,
                YBefore = Y,
                HeightBefore = HeightPercent,
                WidthBefore = WidthPercent,
                Rotation = RotationChkBox.Checked,
                RotationAngle = RotationAngle,
                Distortion = DistortionChkBox.Checked,
                DistortMethod = (DistortMethod)DistortionMetodComBox.SelectedItem,
                A = A,
                B = B,
                C = C,
                D = D,
                CropAfterChkBox = CropAfterChkBox.Checked,
                XAfter = Xa,
                YAfter = Ya,
                HeightAfter = HeightAPercent,
                WidthAfter = WidthAPercent
            };
        }

        private MagickImage EditImg(string InputFile, DistortMethod distortMethod)
        {
            MagickImage image = new MagickImage(InputFile);

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
            if (DistortionChkBox.Checked)
            {
                if (distortMethod != DistortMethod.Undefined && distortMethod != DistortMethod.Sentinel && distortMethod != DistortMethod.Polynomial && distortMethod != DistortMethod.Perspective && distortMethod != DistortMethod.Arc) image.Distort(distortMethod, [(double)A, (double)B, (double)C, (double)D]);
                // if (distortMethod == DistortMethod.Perspective) image.Distort(DistortMethod.Perspective, new double[] { 0, 0, 20, 60, 90, 0, 70, 63, 0, 90, 5, 83, 90, 90, 85, 88 });
                if (distortMethod == DistortMethod.Perspective) image.Distort(DistortMethod.Perspective, new double[] { 0.0, 20.60, 90.0, 70.63, 0.90, 5.83, 90.90, 85.88 });
                if (distortMethod == DistortMethod.Arc) image.Distort(DistortMethod.Arc, 360);
            }

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
                IDrawable[] drawables = 
                            [
                                new DrawableFillColor(MagickColors.Red),
                                new DrawableLine(80 * image.Width / 100, 0, 80 * image.Width / 100, image.Height),
                                new DrawableLine(20 * image.Width / 100, 0, 20 * image.Width / 100, image.Height),
                                new DrawableLine(0, 80 * image.Height / 100, image.Width, 80 * image.Height / 100),
                                new DrawableLine(0, 20 * image.Height / 100, image.Width, 20 * image.Height / 100)
                            ];

                image.Draw(drawables);
            }

            return image;
        }

        private async void ImgFixingForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            //await TrySaveSettings();
        }

        private async void ImgFixingForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //await TrySaveSettings();
        }
        public string GetImgFixingPlan() => imgFixingFile;
        private async void SaveSettingsBtn_Click(object sender, EventArgs e) => await TrySaveSettings();
        private void LoadSettingsBtn_Click(object sender, EventArgs e) => TryReadSettings();
        private void SaveAsBtn_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = fileEdit.GetDefoltDirectory();
            saveFileDialog.Filter = "Fixing img plan (*.fip)|*.fip|All files(*.*)|*.*";
            saveFileDialog.FilterIndex = 1;
            if (saveFileDialog.ShowDialog() == DialogResult.Cancel)
                return;

            string filename = saveFileDialog.FileName;
            fileEdit.SaveJson(filename, GetImgFixingSettings());
            RezultRTB.Text = "Settings save in " + filename;
        }

        public async Task TrySaveSettings()
        {
            fileEdit.SaveJson(imgFixingFile, GetImgFixingSettings());
            RezultRTB.Text = "Settings save in " + fileEdit.GetDefoltDirectory() + imgFixingFile;
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
                        string? jsonString = sr.ReadToEnd();
                        if (jsonString != null)
                        {
                            ImgFixingSettings? imgFixingSettings = JsonSerializer.Deserialize<ImgFixingSettings>(jsonString);
                            return SetImgFixingSettings(imgFixingSettings, fileLoad);
                        }
                    }
                }
                catch (IOException e)
                {
                    RezultRTB.Text = "The file could not be read: " + e.Message + "!!!\n";
                }
            }
            else RezultRTB.Text = "Err TryReadSettings.файл загрузки не найден!!!\n";
            return false;
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
                    ReloadImg();
                }
            }
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)=>ReloadImg();
    }
}