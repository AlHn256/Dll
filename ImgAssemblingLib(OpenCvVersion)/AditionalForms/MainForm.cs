using ImgAssemblingLibOpenCV.Models;
using NLog;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using OpenCvSharp.XFeatures2D;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImgAssemblingLibOpenCV.AditionalForms
{
    public partial class MainForm : Form
    {
        private AssemblyPlan assemblyPlan { get; set; }
        private Assembling Assembling { get; set; }
        private static Logger logger = LogManager.GetCurrentClassLogger(); //логи
        private FileEdit fileEdit;
        private bool SelectSearchArea { get; set; } = false;
        private float MinHeight = 0, MaxHeight = 0, MinWight = 0, MaxWight = 0;
        private decimal Zoom { get; set; } = 1.0m;
        private int Delta = 0, deltaStep = 20, Xdn = 0, Ydn = 0, Xup = 0, Yup = 0;
        private int FirstFileNumber = 0, SecondFileNumber = 0;
        private List<string> FileList = new List<string>();
        private string SecondFile = string.Empty, FirstFile = string.Empty, console = string.Empty;
        private string[] fileFilter = new string[] {"*.jpeg", "*.jpg", "*.png", "*.bmp"};
        private object _context;

        public MainForm()
        {
            InitializeComponent();
            this.Load += Loading;
        }
        //public MainForm(AssemblyPlan assemblyPlan)
        //{
        //    InitializeComponent();
        //    this.assemblyPlan = assemblyPlan;
        //}
        private void Loading(object sender, EventArgs e)
        {
            logger.Info("Programm starting");

            this.picBox_Display.MouseDown += picBox_Display_MouseDown;
            this.picBox_Display.MouseUp += picBox_Display_MouseUp;
            this.picBox_Display.MouseWheel += panel1_MouseWheel;
            this.FormClosing += Form1_FormClosing;
            this.DragDrop += WindowsForm_DragDrop;
            this.DragEnter += WindowsForm_DragEnter;
            ((ISupportInitialize)picBox_Display).EndInit();

            if (SynchronizationContext.Current != null) _context = SynchronizationContext.Current;
            else _context = new SynchronizationContext();
            
            Assembling = new Assembling(_context);
            Assembling.ProcessChanged += worker_ProcessChang;
            Assembling.TextChanged += worker_TextChang;
            Assembling.UpdateImg += worker_UpdateImg;
            Assembling.RTBAddInfo += rtbText_AddInfo;
            Assembling.RTBUpDateInfo += rtbText_UpDateInfo;

            fileEdit = new FileEdit(fileFilter);
            FormSettings formSettings = new FormSettings();
            if (fileEdit.AutoLoade(out formSettings))
            {
                
                if (formSettings.File1 != null) { FirstFile = formSettings.File1; FileDirTxtBox.Text = formSettings.File1; }
                if (formSettings.File2 != null) SecondFile = formSettings.File2;
                if (formSettings.Zoom != 0) Zoom = formSettings.Zoom;
                if (formSettings.AssemblyPlan != null)
                {
                    assemblyPlan = formSettings.AssemblyPlan;
                    if (formSettings.AssemblyPlan.Percent) { label5.Visible = true; label6.Visible = true; }
                    else { label5.Visible = false; label6.Visible = false; }
                    FromTxtBox.Text = formSettings.AssemblyPlan.From.ToString();
                    ToTxtBox.Text = formSettings.AssemblyPlan.To.ToString();
                    PeriodTxtBox.Text = formSettings.AssemblyPlan.Period.ToString();
                }
            }
            else
            {
                RTB.Text = fileEdit.ErrText;
                if (formSettings == null) formSettings = new FormSettings();
                assemblyPlan = new AssemblyPlan();
            }
            GetImgFiles(new string[] { FirstFile });
            ShowLoadeImgs();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            logger.Info("Closing programm");
            SaveSettings();
        }
        private bool SaveSettings()
        {
            FormSettings formSettings = new FormSettings()
            {
                File1 = FileDirTxtBox.Text,
                File2 = SecondFile,
                Zoom = Zoom,
                AssemblyPlan = assemblyPlan
            };
            fileEdit.AutoSave(formSettings);
            return true;
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            KeyDown(keyData);
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private async void KeyDown(Keys keyData)
        {
            if (keyData == Keys.A)
            {
                FromTxtBox.Text = "0";
                ToTxtBox.Text = "100";
                //await StartAssembling(true);
            }
            else if (keyData == Keys.Oem6 || keyData == Keys.Oem4)
            {
                if (keyData == Keys.Oem6) { Delta += deltaStep; await StartAssembling(true); }
                else if (keyData == Keys.Oem4) { Delta -= deltaStep; await StartAssembling(true); }
            }
            else if (keyData == Keys.Up || keyData == Keys.Down || keyData == Keys.K || keyData == Keys.Oem1 || keyData == Keys.OemPeriod || keyData == Keys.Oemcomma || keyData == Keys.Left || keyData == Keys.Right)
            {
                if (FileList.Count == 0) { RTB.Text = "Err KeyDown.FileList=0!!!"; return; }
                else if (keyData == Keys.Oemcomma) SecondFileNumber--;
                else if (keyData == Keys.OemPeriod) SecondFileNumber++;
                else if (keyData == Keys.K) FirstFileNumber--;
                else if (keyData == Keys.Oem1) FirstFileNumber++;
                else if (keyData == Keys.Left || keyData == Keys.Up)
                {
                    FirstFileNumber--;
                    SecondFileNumber--;
                }
                else if (keyData == Keys.Right || keyData == Keys.Down)
                {
                    FirstFileNumber++;
                    SecondFileNumber++;
                }

                if (FirstFileNumber < 0) FirstFileNumber = 0;
                if (SecondFileNumber < 0) SecondFileNumber = 0;
                if (FirstFileNumber > FileList.Count - 1) FirstFileNumber = FileList.Count - 1;
                if (SecondFileNumber > FileList.Count - 1) SecondFileNumber = FileList.Count - 1;
                if (SecondFileNumber < FirstFileNumber) SecondFileNumber = FirstFileNumber;

                FirstFile = FileList[FirstFileNumber];
                SecondFile = FileList[SecondFileNumber];

                RTB.Text = Path.GetFileName(FirstFile) + " - " + Path.GetFileName(SecondFile) + "\n";
                if (keyData == Keys.Up || keyData == Keys.Down)JoinImgs();
                else  ShowPoints();
            }
            else
            {
                console += keyData;
                if(console.Length>5) console= console.Substring(console.Length-5);
            }
        }

        private void TestBtn_Click(object sender, EventArgs e) => MatchImgs();
        private void MatchImgs()
        {
            Mat matSrc = new Mat(FileDirTxtBox.Text);
            Mat matTo = new Mat(SecondFile);

            picBox_Display.Image = BitmapConverter.ToBitmap(MatchPicBySift(matSrc, matTo));
        }
        private void Test2Btn_Click(object sender, EventArgs e)
        {
            Mat matSrc = new Mat(FileDirTxtBox.Text);
            Mat matTo = new Mat(SecondFile);
            int PointNumber = 45;
            Int32.TryParse(PeriodTxtBox.Text, out PointNumber);
            picBox_Display.Image = BitmapConverter.ToBitmap(MatchPicBySurf(matSrc, matTo, PointNumber));
        }

        void WindowsForm_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }
        void WindowsForm_DragDrop(object sender, DragEventArgs e)
        {
            Assembling.SetRezultImg(null);
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            GetImgFiles(files);
            ShowLoadeImgs();
        }
        protected void GetImgFiles(string[] files)
        {
            bool IsFirst = true;
            string serchingDir = string.Empty;
            if (files == null || files.Length == 0) return;
            if (files.Length == 1 && !string.IsNullOrEmpty(files[0]))
            {
                serchingDir = files[0];
                if (!fileEdit.ChkFileDir(serchingDir))
                {
                    RTB.Text = "Err File\\Dir not found!!!";
                    return ;
                }

                if (fileEdit.IsDirectory(serchingDir))
                {
                    FileInfo[] fileInfo = fileEdit.SearchFiles(serchingDir);
                    if (fileInfo.Length > 1)
                    {
                        FileDirTxtBox.Text = fileInfo[0].FullName;
                        SecondFile = fileInfo[1].FullName;
                    }
                    else if (fileInfo.Length == 1)
                    {
                        if (IsFirst) FileDirTxtBox.Text = fileInfo[0].FullName;
                        else SecondFile = fileInfo[0].FullName;
                        IsFirst = !IsFirst;
                    }
                }
                else
                {
                    serchingDir = Path.GetDirectoryName(serchingDir);
                    if (IsFirst) FileDirTxtBox.Text = serchingDir;
                    else SecondFile = serchingDir;
                    IsFirst = !IsFirst;
                }
            }
            if (files.Length > 1)
            {
                serchingDir = Path.GetDirectoryName(files[0]);
                FileDirTxtBox.Text = files[0];
                FirstFile = files[0];
                SecondFile = files[1];
            }

            if(!string.IsNullOrEmpty(serchingDir))LoadFileList(serchingDir);

            if (!string.IsNullOrEmpty(FileDirTxtBox.Text))
            {
                if (fileEdit.IsDirectory(FileDirTxtBox.Text)) assemblyPlan.WorkingDirectory = FileDirTxtBox.Text;
                else assemblyPlan.WorkingDirectory = Path.GetDirectoryName(FileDirTxtBox.Text);

                if (assemblyPlan.FixImg)
                {
                    string FixingImgDirectory = assemblyPlan.WorkingDirectory + "AutoOut";
                    assemblyPlan.FixingImgDirectory = FixingImgDirectory;
                    assemblyPlan.StitchingDirectory = FixingImgDirectory;
                }
                else assemblyPlan.StitchingDirectory = assemblyPlan.WorkingDirectory;
            }
        }
        private bool LoadFileList(string serchingDir)
        {
            if (!string.IsNullOrEmpty(serchingDir))
            {
                var list = fileEdit.SearchFiles(serchingDir);
                FileList = list.Select(f => f.FullName).ToList();
                if (FileList.Count > 1)
                {
                    for (int i = 0; i < FileList.Count; i++)
                    {
                        if (FileList[i] == FileDirTxtBox.Text)
                        {
                            FirstFileNumber = i;
                            if (i + 1 != FileList.Count) SecondFileNumber = i + 1;
                            else
                            {
                                FirstFileNumber = i - 1;
                                SecondFileNumber = i;
                            }
                        }
                    }
                }
            }
            return true;
        }

        public Point2d Point2fToPoint2d(Point2f point) => new Point2d((double)point.X, (double)point.Y);
        public Mat MatchPicBySift(Mat matSrc, Mat matTo)
        {
            using (Mat matSrcRet = new Mat())
            using (Mat matToRet = new Mat())
            {
                KeyPoint[] keyPointsSrc, keyPointsTo;
                using (var sift = OpenCvSharp.Features2D.SIFT.Create())
                {
                    sift.DetectAndCompute(matSrc, null, out keyPointsSrc, matSrcRet);
                    sift.DetectAndCompute(matTo, null, out keyPointsTo, matToRet);
                }
                using (var bfMatcher = new BFMatcher())
                {
                    var matches = bfMatcher.KnnMatch(matSrcRet, matToRet, k: 2);
                    var pointsSrc = new List<Point2f>();
                    var pointsDst = new List<Point2f>();
                    var goodMatches = new List<DMatch>();
                    foreach (DMatch[] items in matches.Where(x => x.Length > 1))
                    {
                        if (items[0].Distance < 0.5 * items[1].Distance)
                        {
                            pointsSrc.Add(keyPointsSrc[items[0].QueryIdx].Pt);
                            pointsDst.Add(keyPointsTo[items[0].TrainIdx].Pt);
                            goodMatches.Add(items[0]);
                            RTB.Text += $"{keyPointsSrc[items[0].QueryIdx].Pt.X}, {keyPointsSrc[items[0].QueryIdx].Pt.Y}\n";
                        }
                    }

                    var outMat = new Mat();
                    //Алгоритм RANSAC фильтрует совпадающие результаты
                    var pSrc = pointsSrc.ConvertAll(Point2fToPoint2d);
                    var pDst = pointsDst.ConvertAll(Point2fToPoint2d);
                    var outMask = new Mat();
                    // Если исходный результат сопоставления пуст, пропустите шаг фильтрации
                    if (pSrc.Count > 0 && pDst.Count > 0)
                        Cv2.FindHomography(pSrc, pDst, HomographyMethods.Ransac, mask: outMask);
                    // Применять фильтрацию только в том случае, если количество совпадающих точек, обработанных RANSAC, превышает 10. В противном случае используйте исходные результаты совпадающих точек (если точек совпадения слишком мало, после обработки RANSAC вы можете получить результат 0 совпадающих точек) .
                    if (outMask.Rows > 10)
                    {
                        byte[] maskBytes = new byte[outMask.Rows * outMask.Cols];
                        outMask.GetArray(out maskBytes);
                        Cv2.DrawMatches(matSrc, keyPointsSrc, matTo, keyPointsTo, goodMatches, outMat, matchesMask: maskBytes, flags: DrawMatchesFlags.NotDrawSinglePoints);
                    }
                    else
                        Cv2.DrawMatches(matSrc, keyPointsSrc, matTo, keyPointsTo, goodMatches, outMat, flags: DrawMatchesFlags.NotDrawSinglePoints);

                    Cv2.Resize(outMat, outMat, new OpenCvSharp.Size((int)(outMat.Width * Zoom), (int)(outMat.Height * Zoom)));
                    return outMat;
                }
            }
        }
        public Mat MatchPicBySurf(Mat matSrc, Mat matTo, double threshold = 40)
        {
            using (Mat matSrcRet = new Mat())
            using (Mat matToRet = new Mat())
            {
                KeyPoint[] keyPointsSrc, keyPointsTo;
                using (var surf = SURF.Create(threshold, 4, 3, true, true))
                {
                    surf.DetectAndCompute(matSrc, null, out keyPointsSrc, matSrcRet);
                    surf.DetectAndCompute(matTo, null, out keyPointsTo, matToRet);
                }

                using (var flnMatcher = new FlannBasedMatcher())
                {
                    var matches = flnMatcher.Match(matSrcRet, matToRet);
                    // Находим минимальное и максимальное расстояние
                    double minDistance = 1000;// Обратное приближение
                    double maxDistance = 0;
                    for (int i = 0; i < matSrcRet.Rows; i++)
                    {
                        double distance = matches[i].Distance;
                        if (distance > maxDistance) maxDistance = distance;
                        if (distance < minDistance) minDistance = distance;
                    }
                    RTB.Text = $"max distance : {maxDistance}\n";
                    RTB.Text += $"min distance : {minDistance}";

                    var pointsSrc = new List<Point2f>();
                    var pointsDst = new List<Point2f>();
                    //Выбираем лучшие точки соответствия
                    var goodMatches = new List<DMatch>();
                    for (int i = 0; i < matSrcRet.Rows; i++)
                    {
                        double distance = matches[i].Distance;
                        var sdfsdf = Math.Max(minDistance * 2, 0.02);
                        if (distance < Math.Max(minDistance * 2, 0.02))
                        {
                            pointsSrc.Add(keyPointsSrc[matches[i].QueryIdx].Pt);
                            pointsDst.Add(keyPointsTo[matches[i].TrainIdx].Pt);
                            //Если расстояние меньше диапазона, вставляем новый DMatch
                            goodMatches.Add(matches[i]);
                        }
                    }
                    var outMat = new Mat();

                    //Алгоритм RANSAC фильтрует совпадающие результаты
                    var pSrc = pointsSrc.ConvertAll(Point2fToPoint2d);
                    var pDst = pointsDst.ConvertAll(Point2fToPoint2d);
                    var outMask = new Mat();
                    if (pSrc != null && pDst != null)
                    {
                        // Если исходный результат сопоставления пуст, пропустите шаг фильтрации
                        if (pSrc.Count > 3 && pDst.Count > 3)
                            Cv2.FindHomography(pSrc, pDst, HomographyMethods.Ransac, mask: outMask);
                        // Применять фильтрацию только в том случае, если количество совпадающих точек, обработанных RANSAC, превышает 10. В противном случае используйте исходные результаты совпадающих точек (если точек совпадения слишком мало, после обработки RANSAC вы можете получить результат 0 совпадающих точек) .
                        if (outMask.Rows > 100)
                        {
                            byte[] maskBytes = new byte[outMask.Rows * outMask.Cols];
                            outMask.GetArray(out maskBytes);
                            Cv2.DrawMatches(matSrc, keyPointsSrc, matTo, keyPointsTo, goodMatches, outMat, matchesMask: maskBytes, flags: DrawMatchesFlags.NotDrawSinglePoints);
                        }
                        else
                            Cv2.DrawMatches(matSrc, keyPointsSrc, matTo, keyPointsTo, goodMatches, outMat, flags: DrawMatchesFlags.NotDrawSinglePoints);
                    }

                    Cv2.Resize(outMat, outMat, new OpenCvSharp.Size((int)(outMat.Width * Zoom), (int)(outMat.Height * Zoom)));
                    return outMat;
                }
            }
        }

        private void panel1_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0) ZoomIn();
            else ZoomOut();
        }
        private void ZoomOut()
        {
            if (Zoom < 0.07m) Zoom -= 0.005m;
            else if (Zoom < 0.3m) Zoom -= 0.04m;
            else Zoom -= 0.2m;
            if (Zoom < 0.01m) Zoom = 0.01m;

            ZoomLabel.Text = Zoom.ToString();
            var rezultImg = Assembling.GetRezultImg();

            if (rezultImg != null)
            {
                if (rezultImg.Width != 0 && rezultImg.Height != 0)
                {
                    Mat rezult = new Mat();
                    Cv2.Resize(rezultImg, rezult, new OpenCvSharp.Size((int)(rezultImg.Width * Zoom), (int)(rezultImg.Height * Zoom)));
                    picBox_Display.Image = BitmapConverter.ToBitmap(rezult);
                }
                else ShowLoadeImgs();
            }
            else ShowLoadeImgs();
        }
        private void ZoomIn()
        {
            Zoom += 0.2m;
            ZoomLabel.Text = Zoom.ToString();
            var rezultImg = Assembling.GetRezultImg();
            if (rezultImg != null)
            {
                if (rezultImg.Width != 0 && rezultImg.Height != 0)
                {
                    Mat rezult = new Mat();
                    Cv2.Resize(rezultImg, rezult, new OpenCvSharp.Size((int)(rezultImg.Width * Zoom), (int)(rezultImg.Height * Zoom)));
                    picBox_Display.Image = BitmapConverter.ToBitmap(rezult);
                }
                else ShowLoadeImgs();
            }
            else ShowLoadeImgs();
        }

        private void picBox_Display_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Xdn = e.X;
                Ydn = e.Y;
                RTB.Text = "Dn X " + Xdn + " Y " + Ydn;
            }
            else
            {
                SelectSearchArea = false;
                ShowLoadeImgs();
            }
        }
        private void picBox_Display_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Xup = e.X;
                Yup = e.Y;
                //RTB.Text += " Up X " + Xup + " Y " + Yup + "\n";

                int dX = Math.Abs(Xdn - Xup);
                int dY = Math.Abs(Ydn - Yup);
                var Delta = Math.Sqrt(dY * dY + dX * dX);



                SelectSearchArea = true;
                //MinHeight = (int)(Ydn/Zoom); MaxHeight = (int)(Yup / Zoom); MinWight = (int)(Xdn / Zoom); MaxWight = (int)(Xup/ Zoom);

                MinWight = (int)(Xdn / Zoom); MinHeight = (int)(Ydn / Zoom);
                MaxWight = (int)(Xup / Zoom); MaxHeight = (int)(Yup / Zoom);

                ShowLoadeImgs();
            }
        }
        private void ShowLoadeImgs()
        {
            //FirstFile = string.Empty;
            //if (!File.Exists(File1TxtBox.Text))
            //{
            //    if(Directory.Exists(File1TxtBox.Text))
            //    {
            //        var fileInfo = fileEdit.SearchFiles(File1TxtBox.Text);
            //        if (fileInfo[0] != null && fileInfo.Length > 1)
            //        {
            //            FirstFile = fileInfo[0].FullName;
            //            SecondFile = fileInfo[1].FullName;
            //        }
            //        else FirstFile = string.Empty;
            //    }
            //}
            //else FirstFile = File1TxtBox.Text;

            if (string.IsNullOrEmpty(FirstFile) && string.IsNullOrEmpty(FileDirTxtBox.Text))
            {
                RTB.Text = "Err необходимо добавить каталог поиска!!!";
                return;
            }
            if (string.IsNullOrEmpty(FirstFile)) FirstFile = FileDirTxtBox.Text;
            if (File.Exists(FirstFile) && File.Exists(SecondFile))
            {
                // RTB.Text = "Files loading:\n" + File1TxtBox.Text + "\n" + SecondFile;
                //FirstFile = File1TxtBox.Text;

                Mat srcImg1 = new Mat(FirstFile);
                Mat srcImg2 = new Mat(SecondFile);
                bool IsGorizontal = true;

                if (!IsGorizontal)
                {
                    if (srcImg1.Width != srcImg2.Width)
                    {
                        RTB.Text = "Err Ширина изображения несовместима!!!";
                        return;
                    }
                }
                else
                {
                    if (srcImg1.Height != srcImg2.Height)
                    {
                        RTB.Text = "Err Высота изображения несовместима!!!";
                        return;
                    }
                }
                picBox_Display.BackgroundImage = null;

                Mat result = new Mat();
                if (!IsGorizontal) Cv2.VConcat(srcImg1, srcImg2, result);
                else Cv2.HConcat(srcImg1, srcImg2, result);

                if (SelectSearchArea)
                {
                    if (MinWight > MaxWight) (MinWight, MaxWight) = (MaxWight, MinWight);
                    if (MinHeight > MaxHeight) (MinHeight, MaxHeight) = (MaxHeight, MinHeight);
                    if (MinHeight < 0) MinHeight = 0;
                    if (MinWight < 0) MinWight = 0;
                    if (MaxWight > srcImg1.Width - 1 && MinWight > srcImg1.Width - 1)
                    {
                        MaxWight = MaxWight - srcImg1.Width;
                        MinWight = MinWight - srcImg1.Width;
                    }
                    if (MaxWight > srcImg1.Width - 1) MaxWight = srcImg1.Width - 1;
                    if (MaxHeight > srcImg1.Height - 1) MaxHeight = srcImg1.Height - 1;
                    RTB.Text = "Устновленна область поиска точек:\n MaxHeight " + MaxHeight + " MinHeight " + MinHeight + "\n";
                    RTB.Text += "MaxWight " + MaxWight + " MinWight " + MinWight + "\n";
                    RTB.Text += "dX " + Math.Abs(Xup - Xdn) + " dY " + Math.Abs(Yup - Ydn) + "\n";
                    Cv2.Rectangle(result, new OpenCvSharp.Point((int)MinWight, (int)MinHeight), new OpenCvSharp.Point((int)MaxWight, (int)MaxHeight), Scalar.Red, 2);
                }
                else RTB.Text = "Область поиска точек отключена";

                Cv2.Resize(result, result, new OpenCvSharp.Size((int)(result.Width * Zoom), (int)(result.Height * Zoom)));
                picBox_Display.Image = BitmapConverter.ToBitmap(result);
            }
            else
            {
                RTB.Text = string.Empty;
                if (!File.Exists(FileDirTxtBox.Text)) RTB.Text = "First File :" + FileDirTxtBox.Text + " не найден!!!\n";
                if (!File.Exists(SecondFile)) RTB.Text += "Second File :" + SecondFile + " не найден!!!\n";
            }
        }
        private void ShowPointsBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(FirstFile)) FirstFile = FileDirTxtBox.Text;
            RTB.Text = string.Empty;
            ShowPoints();
        }
        public void ShowPoints()
        {
            if (string.IsNullOrEmpty(FirstFile) || string.IsNullOrEmpty(SecondFile)) return;

            AssemblyPlan assemblyPln = new AssemblyPlan();
            assemblyPln.StitchingDirectory = FirstFile;
            assemblyPln.AdditionalFilter = false;
            assemblyPln.SelectSearchArea = SelectSearchArea;
            assemblyPln.MinHeight = MinHeight;
            assemblyPln.MaxHeight = MaxHeight;
            assemblyPln.MinWight = MinWight;
            assemblyPln.MaxWight = MaxWight;

            StitchingBlock stitchingBlock = new StitchingBlock(assemblyPln);
            stitchingBlock.TextChanged += rtbText_AddInfo;
            stitchingBlock.ChangImg += worker_UpdateImg;
            stitchingBlock.AllPointsChkBox = AllPointsChkBox.Checked;
            stitchingBlock.GetVectorList(FirstFile, SecondFile, true);
            if (stitchingBlock.IsErr) RTB.Text += stitchingBlock.ErrText;
        }
        private async void JoinImgs(object sender, EventArgs e) => await JoinImgs();
        private async Task<bool> JoinImgs()
        {
            if (string.IsNullOrEmpty(FirstFile) || string.IsNullOrEmpty(SecondFile))
            {
                RTB.Text += "Err JoinImgs.FirstFile || SecondFile IsNullOrEmpty!!!";
                return false;
            }
            if (!File.Exists(FirstFile) )
            {
                RTB.Text += "Err JoinImgs.FirstFile "+ FirstFile + " NotExists!!!";
                return false;
            }
            if (!File.Exists(SecondFile))
            {
                RTB.Text += "Err JoinImgs.SecondFile " + SecondFile + " NotExists!!!";
                return false;
            }

            if (assemblyPlan == null) assemblyPlan = new AssemblyPlan();
            LoadBoders();
            assemblyPlan.BitMap = true;
            assemblyPlan.Stitch = true;
            assemblyPlan.FixImg = false;
            assemblyPlan.SaveImgFixingRezultToFile = true;

            RTB.Text = "Join Imgs\n";
            
            Assembling.BitmapData = new Bitmap[] { new Bitmap(FirstFile), new Bitmap(SecondFile) };
            Assembling.ChangeAssemblyPlan(assemblyPlan);

            //Bitmap[] dataArray = new Bitmap[] { new Bitmap(FirstFile), new Bitmap(SecondFile) };
            //Assembling assembling;
            //assembling = new Assembling(assemblyPlan, dataArray, _context);
            //assembling.SaveImgFixingRezultToFile = true;
            //assembling.UpdateImg += worker_UpdateImg;
            //assembling.RTBAddInfo += rtbText_AddInfo;

            if (await Assembling.StartAssembling())// Запуск сборки изображения
            {
                RTB.Text += "Assembling is finished!";
                return true;
            }
            else
            {
                RTB.Text += Assembling.ErrText;
                return false;
            }
        }
        private void worker_ProcessChang(int progress)
        {
            if (progress < 0) progressBar.Value = 0;
            else if (progress > 100) progressBar.Value = 100;
            else progressBar.Value = progress;
        }
        private void worker_TextChang(string text) => progressBarLabel.Text = text;
        private void rtbText_AddInfo(string text)=>RTB.Text += text;
        private void rtbText_UpDateInfo(string text) => RTB.Text = text;
        private void worker_UpdateImg(Mat img)
        {
            if (img.Width == 0 && img.Height == 0)
            {
                picBox_Display.Image = null;
                return;
            }

            if (img.Height * Zoom > picBox_Display.Height)
            {
                Zoom = Math.Round((decimal)(picBox_Display.Height * 0.9) / (decimal)img.Height, 2);
                if (Zoom < 0.1m) Zoom = 0.1m;
                ZoomLabel.Text = Zoom.ToString();
            }

            if (img.Width * Zoom > picBox_Display.Width)
            {
                Zoom = Math.Round((decimal)(picBox_Display.Width * 0.9) / (decimal)img.Width, 2);
                if (Zoom < 0.1m) Zoom = 0.1m;
                ZoomLabel.Text = Zoom.ToString();
            }

            Mat rezult = new Mat();

            if (SelectSearchArea) Cv2.Rectangle(img, new OpenCvSharp.Point((int)MinWight, (int)MinHeight), new OpenCvSharp.Point((int)MaxWight, (int)MaxHeight), Scalar.Red, 2);

            Cv2.Resize(img, rezult, new OpenCvSharp.Size((int)(img.Width * Zoom), (int)(img.Height * Zoom)));
            picBox_Display.Image = BitmapConverter.ToBitmap(rezult);
        }

        private void StopBtn_Click(object sender, EventArgs e)
        {
            StitchingBlock.StopProcess = true;
            ImgFixingForm.StopProcess = true;
        }
        private void SaveThisImgBtn_Click(object sender, EventArgs e) => SaveImg(false);
        private void SaveBtn_Click(object sender, EventArgs e) => SaveImg();
        private bool SaveImg(bool usinOSV = true)
        {
            fileEdit.ClearInformation();
            string fileName = string.Empty;
            if (usinOSV)
            {
                Mat rezultImg = Assembling.GetRezultImg();
                fileName = fileEdit.SaveImg(rezultImg);
            }
            else fileName = fileEdit.SaveImg(null, picBox_Display.Image);

            if (string.IsNullOrEmpty(fileName))
            {
                RTB.Text = fileEdit.ErrText + "\n";
                return false;
            }
            else
            {
                RTB.Text = fileEdit.TextMessag + "\n";
                return true;
            }
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e) => this.Close();
        private void deleteResultesToolStripMenuItem_Click(object sender, EventArgs e) => fileEdit.DeleteResultes("Result");
        private void deletePlanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StitchingBlock stitchingBlock = new StitchingBlock(FileDirTxtBox.Text, false);
            if (stitchingBlock.DeletPlan()) RTB.Text = "Plan Deleted!\n";
        }
        private async void deleteFileCopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(FileDirTxtBox.Text)) { RTB.Text = "Err File1TxtBox.Text IsNullOrEmpty!!!"; return; }
            if (await fileEdit.FindCopyAndDel(Path.GetDirectoryName(FileDirTxtBox.Text))) RTB.Text = fileEdit.TextMessag;
            else RTB.Text = fileEdit.ErrText;
            fileEdit.ClearInformation();
        }
        private void FileNameFixingToolStripMenuItemClick(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(FileDirTxtBox.Text)) RTB.Text = "Err File1TxtBox.Text IsNullOrEmpty!!!";
            {
                string dir = Path.GetDirectoryName(FileDirTxtBox.Text);
                if (fileEdit.CheckFileName(dir)) fileEdit.FixFileName(dir);
            }
        }
        private void TestImgFixingBtn_Click(object sender, EventArgs e)
        {
            if (assemblyPlan == null) assemblyPlan = new AssemblyPlan();
            if (fileEdit.IsDirectory(FileDirTxtBox.Text)) assemblyPlan.WorkingDirectory = FileDirTxtBox.Text;
            else assemblyPlan.WorkingDirectory = Path.GetDirectoryName(FileDirTxtBox.Text);

            EditingStitchingPlan editingStitchingPlan = new EditingStitchingPlan(assemblyPlan);
            editingStitchingPlan.ShowDialog();
            if (editingStitchingPlan.PlanIsUpDate)
            {
                assemblyPlan = editingStitchingPlan.GetAssemblingPlan();
                if (assemblyPlan == null) return;

                if (assemblyPlan.DefaultParameters)
                {
                    label5.Visible = true;
                    label6.Visible = true;
                    assemblyPlan.From = 0;
                    assemblyPlan.To = 100;
                    assemblyPlan.Period = 1;
                    assemblyPlan.Delta = 0;
                }
                else if (assemblyPlan.Percent)
                {
                    label5.Visible = true;
                    label6.Visible = true;
                    if (assemblyPlan.From < 0 || assemblyPlan.To < 0 || assemblyPlan.From > 100 || assemblyPlan.To > 100 || assemblyPlan.From > assemblyPlan.To)
                    {
                        assemblyPlan.From = 0;
                        assemblyPlan.To = 100;
                    }
                }
                else
                {
                    label5.Visible = false;
                    label6.Visible = false;
                }

                PeriodTxtBox.Text = assemblyPlan.Period.ToString();
                FileDirTxtBox.Text = assemblyPlan.WorkingDirectory;
                Delta = assemblyPlan.Delta;
                FromTxtBox.Text = assemblyPlan.From.ToString();
                ToTxtBox.Text = assemblyPlan.To.ToString();
                UseBitmapChckBox.Checked = assemblyPlan.BitMap;
                SaveSettings();
            }
        }
        private void LoadBoders()
        {
            if (assemblyPlan == null) return;
            if (assemblyPlan.Percent) CheckPercents();
            UpDateFrom();
            UpDateTo();
            UpDatePeriod();

            assemblyPlan.SelectSearchArea = SelectSearchArea;
            if (assemblyPlan.SelectSearchArea)
            {
                assemblyPlan.MinHeight = MinHeight;
                assemblyPlan.MaxHeight = MaxHeight;
                assemblyPlan.MinWight = MinWight;
                assemblyPlan.MaxWight = MaxWight;
            }
            assemblyPlan.Delta = Delta;
        }
        private bool CheckPercents()
        {
            if (assemblyPlan == null) return false;
            int from = 0, to = 100;
            Int32.TryParse(FromTxtBox.Text, out from);
            Int32.TryParse(ToTxtBox.Text, out to);
            if (from < 0 || to < 0 || from > 100 || to > 100 || from > to)
            {
                FromTxtBox.Text = "0";
                ToTxtBox.Text = "100";
                assemblyPlan.From = 0;
                assemblyPlan.From = 100;
            }
            return true;
        }
        private void UpDateFrom()
        {
            if (assemblyPlan == null) return;
            //assemblyPlan.DefaultParameters = false;
            int from = 0;
            Int32.TryParse(FromTxtBox.Text, out from);
            FromTxtBox.Text = from.ToString();
            assemblyPlan.From = from;
        }
        private void UpDateTo()
        {
            if (assemblyPlan == null) return;
            //assemblyPlan.DefaultParameters = false;
            int to = 100;
            Int32.TryParse(ToTxtBox.Text, out to);
            ToTxtBox.Text = to.ToString();
            assemblyPlan.To = to;
        }
        private void label6_Click(object sender, EventArgs e) => PersentInvok();
        private void label5_Click(object sender, EventArgs e) => PersentInvok();
        private void MainForm_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.X > 550 && e.X < 566 && e.Y > -15 && e.Y < 80) PersentInvok();
        }
        private void PersentInvok()
        {
            if (assemblyPlan != null)
            {
                assemblyPlan.Percent = !assemblyPlan.Percent;
                label5.Visible = assemblyPlan.Percent;
                label6.Visible = assemblyPlan.Percent;
            }
        }
        private void ImgFix()
        {
            ImgFixingForm imgFixingForm = new ImgFixingForm(Path.GetDirectoryName(FileDirTxtBox.Text));
            imgFixingForm.ShowDialog();
            string ImgFixingPlan = imgFixingForm.GetImgFixingPlan();
            if (!string.IsNullOrEmpty(ImgFixingPlan)) assemblyPlan.ImgFixingPlan = ImgFixingPlan;
        }
        private void button1_Click(object sender, EventArgs e) => ImgFix();
        private void imgFixingToolStripMenuItem_Click(object sender, EventArgs e) => ImgFix();
        private void UseBitmapChckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (assemblyPlan != null) assemblyPlan.BitMap = UseBitmapChckBox.Checked;
        }

        private void UpDatePeriod()
        {
            if (assemblyPlan == null) return;
            //assemblyPlan.DefaultParameters = false;
            int period = 1;
            Int32.TryParse(PeriodTxtBox.Text, out period);
            PeriodTxtBox.Text = period.ToString();
            assemblyPlan.Period = period;
        }
        private void FromTxtBox_TextChanged(object sender, EventArgs e) => UpDateFrom();
        private void ToTxtBox_TextChanged(object sender, EventArgs e) => UpDateTo();
        private void PeriodTxtBox_TextChanged(object sender, EventArgs e) => UpDatePeriod();
        private async void StartAssembling(object sender, EventArgs e) => await StartAssembling(true);
        private async Task<bool> StartAssembling(bool loadBoders = false)
        {
            if (string.IsNullOrEmpty(FileDirTxtBox.Text))
            {
                RTB.Text = "Err File1TxtBox.Text IsNullOrEmpty!!!";
                return false;
            }

            if (assemblyPlan == null) assemblyPlan = new AssemblyPlan();

            if (loadBoders) LoadBoders();
            else if (!fileEdit.IsDirectory(FileDirTxtBox.Text)) assemblyPlan.WorkingDirectory = Path.GetDirectoryName(FileDirTxtBox.Text);
            else assemblyPlan.WorkingDirectory = FileDirTxtBox.Text;
            //assemblyPlan.StitchingDirectory = assemblyPlan.WorkingDirectory;
            //assemblyPlan.BitMap = false;
            //assemblyPlan.ShowAssemblingFile = true;

            if (assemblyPlan.BitMap)
            {
                FileInfo[] fileList = fileEdit.SearchFiles(assemblyPlan.WorkingDirectory);
                if (fileList.Length == 0) return false;
                Bitmap[] dataArray = fileList.Select(x => { return new Bitmap(x.FullName); }).ToArray();
                Assembling.BitmapData = dataArray;
                //Assembling.SaveImgFixingRezultToFile = SavingImgWBitmapChckBox.Checked;
            }

            if (assemblyPlan.SpeedCounting) Assembling.CalculationSpeedDespiteErrors = true;

            Assembling.ChangeAssemblyPlan(assemblyPlan);

            FinalResult ruzult = await Assembling.TryAssemble();
            if (ruzult.IsErr)
            {
                RTB.Text += Assembling.ErrText;
                logger.Info(Assembling.ErrText);
            }
            else
            {
                RTB.Text += "Assembling is finished!";
                logger.Info("Assembling is finished!");
            }

            return !ruzult.IsErr;
        }
        private async void GetSpeedBtn_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(FileDirTxtBox.Text))
            {
                RTB.Text = "ERR GetSpeedBtn.FileDirTxtBox.Text IsNullOrEmpty  !!!";
                return;
            }
            if(assemblyPlan==null) assemblyPlan= new AssemblyPlan();

            RTB.Text = string.Empty;
            LoadBoders();
            assemblyPlan.WorkingDirectory = FileDirTxtBox.Text;
            assemblyPlan.DelFileCopy = false;
            assemblyPlan.FixImg = false;
            assemblyPlan.StitchingDirectory = FileDirTxtBox.Text;
            assemblyPlan.DefaultParameters = true;
            assemblyPlan.FindKeyPoints = true;
            assemblyPlan.ChekStitchPlan = true;
            assemblyPlan.SpeedCounting = true;
            assemblyPlan.Stitch = false;
            Assembling.ChangeAssemblyPlan(assemblyPlan);
            Assembling.CalculationSpeedDespiteErrors = true;
            await Assembling.StartAssembling();
        }

        private class SpeedStat
        {
            public int Fr { get; set; }
            public int To { get; set; }
            public int Points { get; set; }
            public double Speed { get; set; }
            public SpeedStat(int fr, int to, double speed)
            {
                Fr = fr;
                To = to;
                Points = to - fr;
                Speed = speed;
            }
        }
        private async void Random_Click(object sender, EventArgs e)
        {
            if (FileList.Count == 0) LoadFileList(FileDirTxtBox.Text);
            if (FileList.Count != 0)
            {
                LoadBoders();
                assemblyPlan.WorkingDirectory = FileDirTxtBox.Text;
                assemblyPlan.From = 0;
                assemblyPlan.To = 100;
                assemblyPlan.Percent = true;
                assemblyPlan.DefaultParameters = true;
                assemblyPlan.DelFileCopy = false;
                assemblyPlan.FixImg = false;
                assemblyPlan.StitchingDirectory = FileDirTxtBox.Text;
                assemblyPlan.FindKeyPoints = true;
                assemblyPlan.ChekStitchPlan = true;
                assemblyPlan.SpeedCounting = false;
                assemblyPlan.Stitch = false;
                Assembling.ChangeAssemblyPlan(assemblyPlan);
                await Assembling.StartAssembling();

                List<SpeedStat> speedStatsList = new List<SpeedStat>();
                Random random = new Random();
                string Text = string.Empty;
                for (int i = 0; i < 25; i++)
                {
                    int fr = random.Next(FileList.Count - 25), to = fr + random.Next(10, 24);
                    LoadBoders();
                    assemblyPlan.Percent = false;
                    assemblyPlan.From = fr;
                    assemblyPlan.To = to;
                    assemblyPlan.FixImg = false;
                    assemblyPlan.FindKeyPoints = true;
                    assemblyPlan.ChekStitchPlan = true;
                    assemblyPlan.SpeedCounting = true;
                    assemblyPlan.Stitch = false;
                    Assembling.ChangeAssemblyPlan(assemblyPlan);
                    Assembling.CalculationSpeedDespiteErrors = false;
                    await Assembling.StartAssembling();
                    double Speed = Assembling.GetSpeed();
                    speedStatsList.Add(new SpeedStat(fr, to, Speed));
                    Text += "Speed " + Speed + "  Fr " + fr + " To " + to + " ( " + (to - fr) + " )\n";
                }
                RTB.Text = Text;
            }
        }

        private void OpenDirDtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(FileDirTxtBox.Text)) return;

            if(!fileEdit.OpenFileDir(FileDirTxtBox.Text)) RTB.Text = fileEdit.ErrText;
        }
    }
}
