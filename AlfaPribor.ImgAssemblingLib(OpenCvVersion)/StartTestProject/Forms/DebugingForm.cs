using AlfaPribor.Logs;
using ImgAssemblingLibOpenCV.Models;
using StartTestProject.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImgAssemblingLibOpenCV.AditionalForms
{
    public partial class DebugingForm : Form
    {
        private AssemblyPlan assemblyPlan { get; set; }
        private Assembling Assembling { get; set; }
        private RotateFileLogger _Log; //логи
        private FileEdit fileEdit;
        private bool SelectSearchArea { get; set; } = false;
        private float MinHeight = 0, MaxHeight = 0, MinWight = 0, MaxWight = 0;
        private double Zoom { get; set; } = 1.0;
        private int Delta = 0, deltaStep = 20, Xdn = 0, Ydn = 0, Xup = 0, Yup = 0;
        private int FirstFileNumber = 0, SecondFileNumber = 0;
        private List<string> FileList = new List<string>();
        private string SecondFile = string.Empty, FirstFile = string.Empty, console = string.Empty;
        private string[] fileFilter = new string[] {"*.jpeg", "*.jpg", "*.png", "*.bmp"};
        private string prevFirstFile = string.Empty, prevSecondFile = string.Empty;
        private object _context;
        private FixFrames FixingFrames;
        private Bitmap rezultImg {  get; set; }    

        public DebugingForm()
        {
            InitializeComponent();
            this.Load += Loading;
        }
        private void Loading(object sender, EventArgs e)
        {
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
            //Assembling.UpdateImg += worker_UpdateImg;
            Assembling.RTBAddInfo += rtbText_AddInfo;
            Assembling.RTBUpDateInfo += rtbText_UpDateInfo;
            fileEdit = new FileEdit(fileFilter);

            StartLoger();

            FormSettings formSettings = new FormSettings();
            if (fileEdit.AutoLoade(out formSettings,"MainForm"))
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

            if (fileEdit.IsDirectory(FirstFile))GetImgFiles(new string[] { FirstFile });
            LoadFileList(FirstFile);
            ShowLoadedImgs();
        }

        private void StartLoger()
        {
            EncodingInfo[] encodings = Encoding.GetEncodings();
            EncodingInfo info = encodings.First(item => item.DisplayName == "Кириллица (Windows)");
            _Log = new RotateFileLogger(10, 1048576, info.GetEncoding());
            string logDir = fileEdit.GetDefoltDirectory()+ "logs";
            if (!fileEdit.ChkDir(logDir))
            {
                RTB.Text += "\nErr Папка для логов не создана!!!";
                return;
            }
            _Log.FileName = logDir+ Path.DirectorySeparatorChar + "LogExample.log";
            _Log.AutoCreate = true;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)=>SaveSettings();
        private bool SaveSettings()
        {
            FormSettings formSettings = new FormSettings()
            {
                File1 = FileDirTxtBox.Text,
                File2 = SecondFile,
                Zoom = Zoom,
                AssemblyPlan = assemblyPlan
            };
            fileEdit.AutoSave(formSettings,"MainForm");
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
        private void TestBtn_Click(object sender, EventArgs e) 
        {
            //Mat matSrc = new Mat(FileDirTxtBox.Text);
            //Mat matTo = new Mat(SecondFile);
            //picBox_Display.Image = BitmapConverter.ToBitmap(MatchPicBySift(matSrc, matTo));
        }
        private void FileDirTxtBox_TextChanged(object sender, EventArgs e)
        {
            if(FileDirTxtBox.Text != FirstFile)GetImgFiles(new string[1] { FileDirTxtBox.Text });
        }
        void WindowsForm_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }
        void WindowsForm_DragDrop(object sender, DragEventArgs e)
        {
            GetImgFiles((string[])e.Data.GetData(DataFormats.FileDrop));
            ShowLoadedImgs();
        }
        protected bool GetImgFiles(string[] files)
        {
            if (files == null || files.Length == 0) return false;

            if (files.Length == 2)
            {
                FirstFile = files[0];
                SecondFile = files[1];
                LoadFileList(FirstFile);
            }
            else
            {
                if(string.IsNullOrEmpty(files[0])) return false;
                LoadFileList(files[0]);
                if (FileList.Count > 1)
                {
                    FirstFile = FileList[0];
                    SecondFile = FileList[1];
                }
            }

            if (!string.IsNullOrEmpty(FirstFile))
            {
                FileDirTxtBox.Text = FirstFile;
                assemblyPlan.WorkingDirectory = Path.GetDirectoryName(FirstFile);

                if (assemblyPlan.FixImg)
                {
                    string FixingImgDirectory = assemblyPlan.WorkingDirectory + "AutoOut";
                    assemblyPlan.FixingImgDirectory = FixingImgDirectory;
                    assemblyPlan.StitchingDirectory = FixingImgDirectory;
                }
                else assemblyPlan.StitchingDirectory = assemblyPlan.WorkingDirectory;
            }

            return true;
        }

        private bool LoadFileList(string serchingDir = null)
        {
            if (string.IsNullOrEmpty(serchingDir) && string.IsNullOrEmpty(FirstFile)) return false;
            if (string.IsNullOrEmpty(serchingDir)) serchingDir = FirstFile;

            if (!string.IsNullOrEmpty(serchingDir))
            {
                if (!fileEdit.IsDirectory(serchingDir)) serchingDir = Path.GetDirectoryName(serchingDir);

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

        //public Point2d Point2fToPoint2d(Point2f point) => new Point2d((double)point.X, (double)point.Y);
        //public Mat MatchPicBySift(Mat matSrc, Mat matTo)
        //{
        //    using (Mat matSrcRet = new Mat())
        //    using (Mat matToRet = new Mat())
        //    {
        //        KeyPoint[] keyPointsSrc, keyPointsTo;
        //        using (var sift = OpenCvSharp.Features2D.SIFT.Create())
        //        {
        //            sift.DetectAndCompute(matSrc, null, out keyPointsSrc, matSrcRet);
        //            sift.DetectAndCompute(matTo, null, out keyPointsTo, matToRet);
        //        }
        //        using (var bfMatcher = new BFMatcher())
        //        {
        //            var matches = bfMatcher.KnnMatch(matSrcRet, matToRet, k: 2);
        //            var pointsSrc = new List<Point2f>();
        //            var pointsDst = new List<Point2f>();
        //            var goodMatches = new List<DMatch>();
        //            foreach (DMatch[] items in matches.Where(x => x.Length > 1))
        //            {
        //                if (items[0].Distance < 0.5 * items[1].Distance)
        //                {
        //                    pointsSrc.Add(keyPointsSrc[items[0].QueryIdx].Pt);
        //                    pointsDst.Add(keyPointsTo[items[0].TrainIdx].Pt);
        //                    goodMatches.Add(items[0]);
        //                    RTB.Text += $"{keyPointsSrc[items[0].QueryIdx].Pt.X}, {keyPointsSrc[items[0].QueryIdx].Pt.Y}\n";
        //                }
        //            }

        //            var outMat = new Mat();
        //            //Алгоритм RANSAC фильтрует совпадающие результаты
        //            var pSrc = pointsSrc.ConvertAll(Point2fToPoint2d);
        //            var pDst = pointsDst.ConvertAll(Point2fToPoint2d);
        //            var outMask = new Mat();
        //            // Если исходный результат сопоставления пуст, пропустите шаг фильтрации
        //            if (pSrc.Count > 0 && pDst.Count > 0)
        //                Cv2.FindHomography(pSrc, pDst, HomographyMethods.Ransac, mask: outMask);
        //            // Применять фильтрацию только в том случае, если количество совпадающих точек, обработанных RANSAC, превышает 10. В противном случае используйте исходные результаты совпадающих точек (если точек совпадения слишком мало, после обработки RANSAC вы можете получить результат 0 совпадающих точек) .
        //            if (outMask.Rows > 10)
        //            {
        //                byte[] maskBytes = new byte[outMask.Rows * outMask.Cols];
        //                outMask.GetArray(out maskBytes);
        //                Cv2.DrawMatches(matSrc, keyPointsSrc, matTo, keyPointsTo, goodMatches, outMat, matchesMask: maskBytes, flags: DrawMatchesFlags.NotDrawSinglePoints);
        //            }
        //            else
        //                Cv2.DrawMatches(matSrc, keyPointsSrc, matTo, keyPointsTo, goodMatches, outMat, flags: DrawMatchesFlags.NotDrawSinglePoints);

        //            Cv2.Resize(outMat, outMat, new OpenCvSharp.Size((int)(outMat.Width * Zoom), (int)(outMat.Height * Zoom)));
        //            return outMat;
        //        }
        //    }
        //}
        //public Mat MatchPicBySurf(Mat matSrc, Mat matTo, double threshold = 40)
        //{
        //    using (Mat matSrcRet = new Mat())
        //    using (Mat matToRet = new Mat())
        //    {
        //        KeyPoint[] keyPointsSrc, keyPointsTo;
        //        using (var surf = SURF.Create(threshold, 4, 3, true, true))
        //        {
        //            surf.DetectAndCompute(matSrc, null, out keyPointsSrc, matSrcRet);
        //            surf.DetectAndCompute(matTo, null, out keyPointsTo, matToRet);
        //        }

        //        using (var flnMatcher = new FlannBasedMatcher())
        //        {
        //            var matches = flnMatcher.Match(matSrcRet, matToRet);
        //            // Находим минимальное и максимальное расстояние
        //            double minDistance = 1000;// Обратное приближение
        //            double maxDistance = 0;
        //            for (int i = 0; i < matSrcRet.Rows; i++)
        //            {
        //                double distance = matches[i].Distance;
        //                if (distance > maxDistance) maxDistance = distance;
        //                if (distance < minDistance) minDistance = distance;
        //            }
        //            RTB.Text = $"max distance : {maxDistance}\n";
        //            RTB.Text += $"min distance : {minDistance}";

        //            var pointsSrc = new List<Point2f>();
        //            var pointsDst = new List<Point2f>();
        //            //Выбираем лучшие точки соответствия
        //            var goodMatches = new List<DMatch>();
        //            for (int i = 0; i < matSrcRet.Rows; i++)
        //            {
        //                double distance = matches[i].Distance;
        //                var sdfsdf = Math.Max(minDistance * 2, 0.02);
        //                if (distance < Math.Max(minDistance * 2, 0.02))
        //                {
        //                    pointsSrc.Add(keyPointsSrc[matches[i].QueryIdx].Pt);
        //                    pointsDst.Add(keyPointsTo[matches[i].TrainIdx].Pt);
        //                    //Если расстояние меньше диапазона, вставляем новый DMatch
        //                    goodMatches.Add(matches[i]);
        //                }
        //            }
        //            var outMat = new Mat();

        //            //Алгоритм RANSAC фильтрует совпадающие результаты
        //            var pSrc = pointsSrc.ConvertAll(Point2fToPoint2d);
        //            var pDst = pointsDst.ConvertAll(Point2fToPoint2d);
        //            var outMask = new Mat();
        //            if (pSrc != null && pDst != null)
        //            {
        //                // Если исходный результат сопоставления пуст, пропустите шаг фильтрации
        //                if (pSrc.Count > 3 && pDst.Count > 3)
        //                    Cv2.FindHomography(pSrc, pDst, HomographyMethods.Ransac, mask: outMask);
        //                // Применять фильтрацию только в том случае, если количество совпадающих точек, обработанных RANSAC, превышает 10. В противном случае используйте исходные результаты совпадающих точек (если точек совпадения слишком мало, после обработки RANSAC вы можете получить результат 0 совпадающих точек) .
        //                if (outMask.Rows > 100)
        //                {
        //                    byte[] maskBytes = new byte[outMask.Rows * outMask.Cols];
        //                    outMask.GetArray(out maskBytes);
        //                    Cv2.DrawMatches(matSrc, keyPointsSrc, matTo, keyPointsTo, goodMatches, outMat, matchesMask: maskBytes, flags: DrawMatchesFlags.NotDrawSinglePoints);
        //                }
        //                else
        //                    Cv2.DrawMatches(matSrc, keyPointsSrc, matTo, keyPointsTo, goodMatches, outMat, flags: DrawMatchesFlags.NotDrawSinglePoints);
        //            }

        //            Cv2.Resize(outMat, outMat, new OpenCvSharp.Size((int)(outMat.Width * Zoom), (int)(outMat.Height * Zoom)));
        //            return outMat;
        //        }
        //    }
        //}
        private void panel1_MouseWheel(object sender, MouseEventArgs e)
        {
            //Zoom = 1;
            if (e.Delta > 0) zoom(true);
            else zoom(false);
        }

        private void zoom(bool zoomIn)
        {
            if (zoomIn)
            {
                Zoom += 0.2;
                ZoomLabel.Text = Zoom.ToString();
            }
            else
            {
                if (Zoom < 0.07) Zoom -= 0.005;
                else if (Zoom < 0.3) Zoom -= 0.04;
                else Zoom -= 0.2;
                if (Zoom < 0.01) Zoom = 0.01;
            }

            if (FixingFrames == null) return;
            if (FixingFrames.ImgFixingSettings == null) return;
            FixingFrames.ImgFixingSettings.Zoom = Zoom;
            ZoomLabel.Text = FixingFrames.ImgFixingSettings.Zoom.ToString();

            if (rezultImg != null)
            {
                if (rezultImg.Width != 0 && rezultImg.Height != 0)
                {
                    Size newSize = new Size((int)(rezultImg.Width * Zoom), (int)(rezultImg.Height * Zoom));
                    picBox_Display.Image = new Bitmap(rezultImg, newSize);
                    return;
                }
            }

           //ShowLoadedImgs();
        }

        private void picBox_Display_MouseDown(object sender, MouseEventArgs e)
        {
            //if (e.Button == MouseButtons.Right)
            //{
            //    Xdn = e.X;
            //    Ydn = e.Y;
            //    RTB.Text = "Dn X " + Xdn + " Y " + Ydn;
            //}
            //else
            //{
            //    SelectSearchArea = false;
            //    ShowLoadedImgs();
            //}
        }

        private void picBox_Display_MouseUp(object sender, MouseEventArgs e)
        {
            //if (e.Button == MouseButtons.Right)
            //{
            //    Xup = e.X;
            //    Yup = e.Y;
            //    //RTB.Text += " Up X " + Xup + " Y " + Yup + "\n";

            //    int dX = Math.Abs(Xdn - Xup);
            //    int dY = Math.Abs(Ydn - Yup);
            //    var Delta = Math.Sqrt(dY * dY + dX * dX);

            //    SelectSearchArea = true;
            //    MinWight = (int)(Xdn / Zoom); MinHeight = (int)(Ydn / Zoom);
            //    MaxWight = (int)(Xup / Zoom); MaxHeight = (int)(Yup / Zoom);

            //    ShowLoadedImgs();
            //}
        }
        
        private void ShowLoadedImgs()
        {
            if (string.IsNullOrEmpty(FirstFile) && string.IsNullOrEmpty(FileDirTxtBox.Text))
            {
                RTB.Text = "Err необходимо добавить каталог поиска!!!";
                return;
            }
            if (string.IsNullOrEmpty(FirstFile)) FirstFile = FileDirTxtBox.Text;

            if (File.Exists(FirstFile) && File.Exists(SecondFile))
            {
                if (prevFirstFile != FirstFile || prevSecondFile != SecondFile)
                {
                    if (FixingFrames != null) FixingFrames.ChangeFrames(FirstFile, SecondFile);
                    else FixingFrames = new FixFrames(FirstFile, SecondFile, false);
                }

                if (FixImgChckBox.Checked) FixingFrames.TryReadSettings(assemblyPlan.ImgFixingPlan);
                else FixingFrames.ChangeSettings(new ImgFixingSettings { Zoom = Zoom });
                prevFirstFile = FirstFile; 
                prevSecondFile = SecondFile;
                picBox_Display.Image = null;

                if (FixingFrames.StitchTwoImg(FixImgChckBox.Checked))
                {
                    var img = FixingFrames.GetRezult();
                    if(img!=null && img.Width !=0 && img.Height!=0) worker_UpdateImg(img);
                    picBox_Display.Image = FixingFrames.GetRezult();
                }
                else RTB.Text = FixingFrames.ErrText;
            }
            else
            {
                RTB.Text = string.Empty;
                if (!File.Exists(FileDirTxtBox.Text)) RTB.Text = "First File :" + FileDirTxtBox.Text + " не найден!!!\n";
                if (!File.Exists(SecondFile)) RTB.Text += "Second File :" + SecondFile + " не найден!!!\n";
            }
        }
        /// <summary> Показать ключевые точки </summary>
        private void ShowPointsBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(FirstFile)) FirstFile = FileDirTxtBox.Text;
            RTB.Text = string.Empty;
            ShowPoints();
        }

        /// <summary> Показать ключевые точки </summary>
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
            assemblyPln.FixImg = FixImgChckBox.Checked;

            StitchingBlock stitchingBlock = new StitchingBlock(assemblyPln);
            stitchingBlock.TextChanged += rtbText_AddInfo;
            stitchingBlock.ChangBitmapImg += worker_UpdateImg;
            stitchingBlock.AllPointsChkBox = AllPointsChkBox.Checked;
            stitchingBlock.GetVectorListStringVersion(FirstFile, SecondFile, true);
            if (stitchingBlock.IsErr) RTB.Text += stitchingBlock.ErrText;

            //if (AllPointsChkBox.Checked)
            //{
            //    rezultImg = stitchingBlock.RunORB(FirstFile, SecondFile);
            //    worker_UpdateImg();
            //}
            //else
            //{
            //    stitchingBlock.TextChanged += rtbText_AddInfo;
            //    stitchingBlock.ChangBitmapImg += worker_UpdateImg;
            //    stitchingBlock.AllPointsChkBox = AllPointsChkBox.Checked;
            //    stitchingBlock.GetVectorListStringVersion(FirstFile, SecondFile, true);
            //    if (stitchingBlock.IsErr) RTB.Text += stitchingBlock.ErrText;
            //}
        }

        /// <summary> Объединение двух кадров </summary>
        private async void JoinImgs(object sender, EventArgs e) => await JoinImgs();
        /// <summary> Объединение двух кадров </summary>
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

            RTB.Text = "Объединение кадров\n" + Path.GetFileName(FirstFile) + " - " + Path.GetFileName(SecondFile) + "\n";
            if (FirstFile == SecondFile)
            {
                RTB.Text += " - Copy!";
                return false;
            }

            if (assemblyPlan == null) assemblyPlan = new AssemblyPlan();
            LoadBorders();
            assemblyPlan.BitMap = true;
            assemblyPlan.Stitch = true;

            if (FixImgChckBox.Checked && !string.IsNullOrEmpty(assemblyPlan.ImgFixingPlan)) assemblyPlan.FixImg = true;
            else assemblyPlan.FixImg = false;

            assemblyPlan.ShowRezult = false;
            assemblyPlan.SaveImgFixingRezultToFile = true;
            PeriodTxtBox.Text = "1";
            Assembling.ChangeAssemblyPlan(assemblyPlan);
            Assembling.BitmapData = new Bitmap[] { new Bitmap(FirstFile), new Bitmap(SecondFile) };

            if (await Assembling.StartAssemblingAsync())// Запуск сборки изображения
            {
                worker_UpdateImg(Assembling.FinalRezult.BitRezult);
                RTB.Text += "Сборка завершена!";
                return true;
            }
            else
            {
                RTB.Text += Assembling.ErrText;
                return false;
            }
        }
        /// <summary>
        /// Установка progressBar
        /// </summary>
        /// <param name="progress">Новое значение progressBar</param>
        private void worker_ProcessChang(int progress)
        {
            if (progress < 0) progressBar.Value = 0;
            else if (progress > 100) progressBar.Value = 100;
            else progressBar.Value = progress;
        }
        private void worker_TextChang(string text) => progressBarLabel.Text = text;
        private void rtbText_AddInfo(string text)=>RTB.Text += text;
        private void rtbText_UpDateInfo(string text) => RTB.Text = text;
        /// <summary>
        /// обновление рисунка
        /// </summary>
        /// <param name="img">новое изображение</param>
        private void worker_UpdateImg(Bitmap img = null)
        {
            if (img == null && rezultImg == null) return;
            if (img == null) img = rezultImg;
            else rezultImg = img;
            if (Zoom > 5) Zoom = 1;
            Size newSize = new Size((int)(img.Width * Zoom), (int)(img.Height * Zoom));
            picBox_Display.Image = new Bitmap(img, newSize);
        }

        /// <summary> Остановка сборки </summary>
        private void StopBtn_Click(object sender, EventArgs e) => StopAssembling();
        private bool SaveImg(bool usinOSV = true)
        {
            fileEdit.ClearInformation();
            string fileName = string.Empty;

            // было
            //if (usinOSV)
            //{
            //    Mat rezultImg = Assembling.GetRezultImg();
            //    fileName = fileEdit.SaveImg(rezultImg);
            //}
            //else fileName = fileEdit.SaveImg(null, picBox_Display.Image);

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
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)=>this.Close();
        private void deleteResultesToolStripMenuItem_Click(object sender, EventArgs e) => fileEdit.DeleteResultes("Result");
        private void deletePlanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StitchingBlock stitchingBlock = new StitchingBlock(FileDirTxtBox.Text, false);
            if (stitchingBlock.DeletPlan()) RTB.Text = "Plan Deleted!\n";
        }
        private void deleteFileCopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileInfo[] fileList = fileEdit.SearchFiles(Path.GetDirectoryName(FileDirTxtBox.Text));
            foreach(FileInfo file in fileList)
            {
                Bitmap bmp = new Bitmap(file.FullName);
                string newFile = file.DirectoryName + Path.DirectorySeparatorChar + Path.GetFileNameWithoutExtension(file.Name)+ ".jpeg";
                bmp.Save(newFile, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
        }

        private void TestImgFixingBtn_Click(object sender, EventArgs e)
        {
            if (assemblyPlan == null) assemblyPlan = new AssemblyPlan();
            if (fileEdit.IsDirectory(FileDirTxtBox.Text)) assemblyPlan.WorkingDirectory = FileDirTxtBox.Text;
            else assemblyPlan.WorkingDirectory = Path.GetDirectoryName(FileDirTxtBox.Text);

            EditingStitchingPlan editingStitchingPlan = new EditingStitchingPlan(assemblyPlan);
            editingStitchingPlan.SetDefoltPlan(assemblyPlan.ImgFixingPlan);
            editingStitchingPlan.ShowDialog();
            if (editingStitchingPlan.PlanIsUpDate)
            {
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

        /// <summary> Загрузка границ сборки кадров </summary>
        private void LoadBorders()
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
        /// <summary> Проверка процентов </summary>
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
            Int32.TryParse(FromTxtBox.Text, out int from);
            FromTxtBox.Text = from.ToString();
            assemblyPlan.From = from;
        }
        private void UpDateTo()
        {
            if (assemblyPlan == null) return;
            int to = 100;
            Int32.TryParse(ToTxtBox.Text, out to);
            ToTxtBox.Text = to.ToString();
            assemblyPlan.To = to;
        }
        /// <summary> Включение\отклучение процентного счетчика </summary>
        private void label6_Click(object sender, EventArgs e) => PersentInvok();
        /// <summary> Включение\отклучение процентного счетчика  </summary>
        private void label5_Click(object sender, EventArgs e) => PersentInvok();
        /// <summary> Включение\отклучение процентного счетчика при нажатии в определенную область </summary>
        private void MainForm_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.X > 550 && e.X < 566 && e.Y > -15 && e.Y < 80) PersentInvok();
        }

        /// <summary> Сохранить результат </summary>
        private void SaveOriginalToolStripMenuItem_Click(object sender, EventArgs e) => SaveImg();
        /// <summary> Сохранить результат из окна </summary>
        private void SaveWindowImgToolStripMenuItem_Click(object sender, EventArgs e)=>SaveImg(false);

        private void FixImgChckBox_CheckedChanged(object sender, EventArgs e)
        {
            if(assemblyPlan==null) return;
            assemblyPlan.FixImg = FixImgChckBox.Checked;
        }

        /// <summary>
        /// Смена счетчика кадров с колличественного на процентный
        /// </summary>
        private void PersentInvok()
        {
            if (assemblyPlan != null)
            {
                assemblyPlan.Percent = !assemblyPlan.Percent;
                label5.Visible = assemblyPlan.Percent;
                label6.Visible = assemblyPlan.Percent;
            }
        }
        /// <summary> Показать все ключевые точки </summary>
        private void AllPointsChkBox_CheckedChanged(object sender, EventArgs e) => ShowPoints();
        private void UseBitmapChckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (assemblyPlan != null) assemblyPlan.BitMap = UseBitmapChckBox.Checked;
        }
        /// <summary> настройка переиода кадров </summary>
        private void UpDatePeriod()
        {
            if (assemblyPlan == null) return;
            Int32.TryParse(PeriodTxtBox.Text, out int period);
            if(period < 1) period = 1;
            PeriodTxtBox.Text = period.ToString();
            assemblyPlan.Period = period;
        }
        private void FromTxtBox_TextChanged(object sender, EventArgs e) => UpDateFrom();
        private void ToTxtBox_TextChanged(object sender, EventArgs e) => UpDateTo();
        private void PeriodTxtBox_TextChanged(object sender, EventArgs e) => UpDatePeriod();
        /// <summary>  Запуск сборки  </summary>
        private async void StartAssembling(object sender, EventArgs e) => await StartAssembling(true);
        /// <summary>  Запуск сборки  </summary>
        private async Task<bool> StartAssembling(bool loadBoders = false)
        {
            StopAssembling();
            RTB.Text = string.Empty;

            if (string.IsNullOrEmpty(FileDirTxtBox.Text))
            {
                RTB.Text = "Err File1TxtBox.Text IsNullOrEmpty!!!";
                return false;
            }

            if (assemblyPlan == null) assemblyPlan = new AssemblyPlan();
            if (loadBoders) LoadBorders();
            else if (!fileEdit.IsDirectory(FileDirTxtBox.Text)) assemblyPlan.WorkingDirectory = Path.GetDirectoryName(FileDirTxtBox.Text);
            else assemblyPlan.WorkingDirectory = FileDirTxtBox.Text;
            assemblyPlan.StitchingDirectory = assemblyPlan.WorkingDirectory;

            Assembling.ChangeAssemblyPlan(assemblyPlan);
            if (Assembling.AssemblyPlan.BitMap)
            {
                FileInfo[] fileList = fileEdit.SearchFiles(assemblyPlan.WorkingDirectory);
                if (fileList.Length == 0) return false;
                Bitmap[] dataArray = fileList.Select(x => { return new Bitmap(x.FullName); }).ToArray();
                Assembling.BitmapData = dataArray;
            }
            if (Assembling.AssemblyPlan.SpeedCounting) Assembling.CalculationSpeedDespiteErrors = true;

            await Assembling.TryAssembleAsync();
            if (Assembling.FinalRezult.IsErr)
            {
                RTB.Text += Assembling.ErrText;
                _Log.DebugPrint(Assembling.ErrText);
            }
            else
            {
                RTB.Text += "Сборка завершена!";
                _Log.DebugPrint("Сборка завершена!");
            }

            if (Assembling.FinalRezult.BitRezult == null) picBox_Display.Image = null;
            else worker_UpdateImg(Assembling.FinalRezult.BitRezult);
            return !Assembling.FinalRezult.IsErr;
        }
        /// <summary> Остановка сборки </summary>
        private void StopAssembling()
        {
            progressBar.Value = 0;
            progressBarLabel.Text = "0";
            StitchingBlock.StopProcess = false;
            ImgFixingForm.StopProcess = false;
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
            LoadBorders();
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
            await Assembling.StartAssemblingAsync();
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
                LoadBorders();
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
                await Assembling.StartAssemblingAsync();

                List<SpeedStat> speedStatsList = new List<SpeedStat>();
                Random random = new Random();
                string Text = string.Empty;
                for (int i = 0; i < 25; i++)
                {
                    int fr = random.Next(FileList.Count - 25), to = fr + random.Next(10, 24);
                    LoadBorders();
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
                    await Assembling.StartAssemblingAsync();
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
