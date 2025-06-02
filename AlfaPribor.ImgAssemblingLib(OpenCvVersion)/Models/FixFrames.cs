using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;

namespace ImgAssemblingLibOpenCV.Models
{
    public class FixFrames
    {
        private const string imgDefoltFixingFile = "imgFixingSettings.oip";
        private string ImgFixingPlan = imgDefoltFixingFile; // Файл с настройками коректировки
        public ImgFixingSettings ImgFixingSettings = new ImgFixingSettings();  // Настройки для коректировки кадров
        private FileEdit fileEdit = new FileEdit(new string[] { "*.jpeg", "*.jpg", "*.png", "*.bmp" });
        public event Action<int> ProcessChanged;
        public event Action<string> TextChanged;
        public bool IsErr { get; set; } = false; // Флаг ошибки
        public List<string> ErrList { get; set; } = new List<string>(); // Список ошибок
        public string ErrText { get; set; } = string.Empty; // Текст последней ошибки
        private Mat FixingFrame1 { get; set; } // Фрагмент первого кадра для склейки
        private Mat FixingFrame2 { get; set; } // Фрагмент второго кадра для склейки
        private List<string> FileList = new List<string>(); // Список кадров
        private Mat Rezult { get; set; }
        private bool SaveRezultToFile; // Метка сохранения результатов
        private string SavingRezultDir; // Папка для сохранения результатов

        public FixFrames()
        {
        }

        /// <summary>
        /// Конструктор для работы с Bitmapами
        /// </summary>
        /// <param name="imgFixingPlan">Файл с планом корректировки</param>
        /// <param name="saveRezultToFile">Для проверки если нужно записать результаты в папку</param>
        /// <param name="fixingImgDirectory">Папка для реультатов</param>
        public FixFrames(string imgFixingPlan, bool saveRezultToFile = false, string fixingImgDirectory = "")
        {
            if (!string.IsNullOrEmpty(imgFixingPlan))
            {
                ImgFixingPlan = imgFixingPlan;
                TryReadSettings();
                SaveRezultToFile = saveRezultToFile;
                SavingRezultDir = fixingImgDirectory;
            }
        }

        ///// <summary>
        ///// Конструктор для склейки изображений из директории
        ///// </summary>
        ///// <param name="imgFixingSettings">Настройки по которым коректируктся фрагмент</param>
        ///// <param name="dir">Директория с кадрами</param>
        //public FixFrames(ImgFixingSettings imgFixingSettings, string directory)
        //{
        //    ImgFixingSettings = imgFixingSettings;
        //    TryReadSettings();
        //    LoadImg(directory);
        //}

        /// <summary>
        /// Конструктор для работы с дополнительным окном
        /// </summary>
        /// <param name="imgFixingPlan"></param>
        /// <param name="directory"></param>
        public FixFrames(string imgFixingPlan, string directory)
        {

            if (!string.IsNullOrEmpty(imgFixingPlan)) ImgFixingPlan = imgFixingPlan;
            if (!string.IsNullOrEmpty(ImgFixingPlan)) TryReadSettings();
            LoadFileList(directory);
        }

        /// <summary>
        /// Конструктор для склейки дух изображений
        /// </summary>
        /// <param name="imgFixingPlan">Файл с планом корректировки</param>
        /// <param name="file">Фрагмент для корректировки</param>
        /// <param name="parm">Параметр нужен что бы отличать конструкторы</param>
        public FixFrames(string file1, string file2, bool parm)
        {
            FixingFrame1 = new Mat(file1);
            FixingFrame2 = new Mat(file2);
        }

        public FixFrames(Mat file1, Mat file2)
        {
            FixingFrame1 = file1;
            FixingFrame2 = file2;
        }

        /// <summary>
        /// Загрузка списка кадров из папки
        /// </summary>
        /// <param name="serchingDir"></param>
        /// <returns></returns>
        private bool LoadFileList(string serchingDir)
        {
            if (string.IsNullOrEmpty(serchingDir))return false;
            var list = fileEdit.SearchFiles(serchingDir);
            FileList = list.Select(f => f.FullName).ToList();
            return true;
        }
        /// <summary>
        /// Загрузка одного кадра из файла
        /// </summary>
        /// <param name="file">Файл с кадром</param>
        /// <returns></returns>
        public bool LoadImg(string file)
        {
            if (string.IsNullOrEmpty(file)) return SetErr("File IsNullOrEmpty");
            if (!File.Exists(file)) return SetErr("File: " + file + " не найден");
            FixingFrame1 = EditImg(Cv2.ImRead(file));

            return ChkFixingFrame1();
        }

        public void ChangeFirstFrame(string file1) => FixingFrame1 = new Mat(file1);
        public void ChangeSecondFrame(string file2) => FixingFrame2 = new Mat(file2);
        public void ChangeFrames(string file1, string file2)
        {
            FixingFrame1 = new Mat(file1);
            FixingFrame2 = new Mat(file2);
        }
        public void ChangeSettings(ImgFixingSettings imgFixingSettings) => ImgFixingSettings = imgFixingSettings;

        /// <summary>
        /// Склейка двух изображений для DebugForm
        /// </summary>
        /// <returns></returns>
        public bool StitchTwoImg(bool useEdit = false)
        {
            if (useEdit)
            {
                FixingFrame1 = EditImg(FixingFrame1);
                FixingFrame2 = EditImg(FixingFrame2);
            }
            

            bool IsGorizontal = true;
            if (Rezult == null) Rezult = new Mat();
            if (IsGorizontal && FixingFrame1.Height != FixingFrame2.Height) return SetErr("Высота изображения несовместима");
            if (!IsGorizontal && FixingFrame1.Width != FixingFrame2.Width) return SetErr("Ширина изображения несовместима");

            if(FixingFrame1 ==null) FixingFrame1 = new Mat();
            if (!IsGorizontal) Cv2.VConcat(FixingFrame1, FixingFrame2, Rezult);
            else Cv2.HConcat(FixingFrame1, FixingFrame2, Rezult);

            //if (SelectSearchArea)
            //{
            //    if (MinWight > MaxWight) (MinWight, MaxWight) = (MaxWight, MinWight);
            //    if (MinHeight > MaxHeight) (MinHeight, MaxHeight) = (MaxHeight, MinHeight);
            //    if (MinHeight < 0) MinHeight = 0;
            //    if (MinWight < 0) MinWight = 0;
            //    if (MaxWight > FixingFrame1.Width - 1 && MinWight > FixingFrame1.Width - 1)
            //    {
            //        MaxWight = MaxWight - FixingFrame1.Width;
            //        MinWight = MinWight - FixingFrame1.Width;
            //    }
            //    if (MaxWight > FixingFrame1.Width - 1) MaxWight = FixingFrame1.Width - 1;
            //    if (MaxHeight > FixingFrame1.Height - 1) MaxHeight = FixingFrame1.Height - 1;
            //    RTB.Text = "Устновленна область поиска точек:\n MaxHeight " + MaxHeight + " MinHeight " + MinHeight + "\n";
            //    RTB.Text += "MaxWight " + MaxWight + " MinWight " + MinWight + "\n";
            //    RTB.Text += "dX " + Math.Abs(Xup - Xdn) + " dY " + Math.Abs(Yup - Ydn) + "\n";
            //    Cv2.Rectangle(result, new OpenCvSharp.Point((int)MinWight, (int)MinHeight), new OpenCvSharp.Point((int)MaxWight, (int)MaxHeight), Scalar.Red, 2);
            //}
            //else RTB.Text = "Область поиска точек отключена";
           
            //if (ImgFixingSettings.Zoom == 0) ImgFixingSettings.Zoom = 1;
            //Cv2.Resize(Rezult, Rezult, new OpenCvSharp.Size((int)(Rezult.Width * ImgFixingSettings.Zoom), (int)(Rezult.Height * ImgFixingSettings.Zoom)));
            return true;
        }

        public Bitmap GetRezult() => BitmapConverter.ToBitmap(Rezult);
        private bool SetErr(string errText)
        {
            ErrText = "Err "+errText+"!!!";
            if (IsErr) ErrList.Add(ErrText);
            IsErr = true;
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
        /// <summary>
        /// Загрузка настроек коректировки
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
                }
                catch (IOException e)
                {
                    SetErr("The file could not be read: " + e.Message + "!!!\n");
                }
            }
            else SetErr("Err TryReadSettings.файл загрузки не найден!!!\n Загруженны настройки поумолчанию.");
        }

        /// <summary>
        /// Исправление дисторсии и сохранение кадров в отдельную папку
        /// </summary>
        /// <param name="param">Параметр синхронизации</param>
        /// <param name="outputDir">Папка для сохранения</param>
        /// <returns></returns>
        public bool FixImges(object param, string outputDir = "")
        {
            SynchronizationContext context = (SynchronizationContext)param;
            if(ImgFixingSettings == null) return false;
            
            ImgFixingSettings.ShowGrid = false;
            if (string.IsNullOrEmpty(outputDir))
            {
                if (!Directory.Exists(SavingRezultDir)) return false;
                SavingRezultDir = outputDir;
            }

            if (FileList == null) return SetErr("FixImges.FileList == null");
            if (FileList.Count == 0) return SetErr("FileList.Count == 0");
            if(!fileEdit.ChkDir(outputDir)) return SetErr("Папка для сохранения результатов"+ outputDir+" не создана");

            int FileListCount = FileList.Count();
            for (int i = 0; i < FileListCount; i++)
            {
                string outputFile = outputDir + Path.DirectorySeparatorChar + Path.GetFileName(FileList[i]);
                EditImg(FileList[i]).Save(outputFile);
                context.Send(OnProgressChanged, i * 100 / FileListCount);
                context.Send(OnTextChanged, "Imges Fixing " + i * 100 / FileListCount + " %");
            }

            context.Send(OnProgressChanged, 100);
            context.Send(OnTextChanged, "Imges Fixing 100 %");
            return IsErr;
        }

        /// <summary>
        /// Коректировка кадров из массива
        /// </summary>
        /// <param name="param">Контекст синхронизации</param>
        /// <param name="dataArray">Массив кадров для коректировки</param>
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
            
            bool fileSaving = false;
            if (SaveRezultToFile)
            {
                if (fileEdit.ChkDir(SavingRezultDir)) fileSaving = true;
                else SetErr($"ERR FixImges.!Directory.Exists({SavingRezultDir}) !!!");
            }

            List<Bitmap> bitMapList = new List<Bitmap>();
            for (int i = 0; i < dataArray.Length; i++)
            {
                Bitmap img = EditImg(dataArray[i]);
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

        public Bitmap EditImg(Bitmap bitmap)
        {
            if (bitmap == null)
            {
                SetErr("Err EditImg.bitmap == null !!!");
                return null;
            }
            if (bitmap.Width == 0 || bitmap.Height == 0)
            {
                SetErr("Err bitmap.Width ==0 || bitmap.Height == 0 !!!");
                return null;
            }
            return MatToBitmap(EditImg(BitmapConverter.ToMat(bitmap)));
        }

        public Bitmap EditImg(ImgFixingSettings imgFixingSettings, string file = "")
        {
            ImgFixingSettings = imgFixingSettings;
            return EditImg(file);
        }
        public Bitmap EditImg(string file = "")
        {
            if (!File.Exists(file))
            {
                SetErr("File: " + file + " не найден");
                return null;
            }
            var mat = EditImg(Cv2.ImRead(file));
            return MatToBitmap(mat);
        }

        public Bitmap EditImg()
        {
            if (FixingFrame1 == null)
            {
                SetErr("FixingFrame1 = null");
                return null;
            }
            
            return MatToBitmap(EditImg(FixingFrame1));
        }

        /// <summary>
        /// Проверка границ перед обрезкой кадра
        /// </summary>
        /// <param name="img"></param>
        private void ChkBorder(Mat img)
        {
            if (img.Width == 0 || img.Height == 0) return;
            int Y = ImgFixingSettings.YBefor, X = ImgFixingSettings.XBefor, dY = ImgFixingSettings.DYBefor, dX = ImgFixingSettings.DXBefor;

            if (Y < 0) Y = 0;
            if (X < 0) X = 0;
            if (Y > img.Height - 1) Y = img.Height / 2;
            if (X > img.Width - 1) X = img.Width / 2;

            if (dY <= 0) dY = img.Height;
            if (dX <= 0) dX = img.Width;
            if (Y + dY > img.Height) dY = img.Height - Y;
            if (X + dX > img.Width) dX = img.Width - X;

            ImgFixingSettings.YBefor = Y;
            ImgFixingSettings.XBefor = X;
            ImgFixingSettings.DYBefor = dY;
            ImgFixingSettings.DXBefor = dX;
        }

        /// <summary>
        /// Редактирование изображения в зависимости от настроенных параметров
        /// </summary>
        /// <param name="img">Картинка для редактировани</param>
        /// <returns></returns>
        private Mat EditImg(Mat img)
        {
            if (ImgFixingSettings.CropBeforChkBox && (img.Width!= (ImgFixingSettings.DXBefor - ImgFixingSettings.XBefor )|| img.Height != (ImgFixingSettings.DYBefor - ImgFixingSettings.DYBefor)))
            {
                ChkBorder(img);
                Rect rect;
                if (ImgFixingSettings.DYBefor != 0 || ImgFixingSettings.DXBefor != 0) rect = new Rect(ImgFixingSettings.XBefor, ImgFixingSettings.YBefor, ImgFixingSettings.DXBefor, ImgFixingSettings.DYBefor);
                else rect = new Rect(ImgFixingSettings.XBefor, ImgFixingSettings.YBefor, img.Width - ImgFixingSettings.XBefor, img.Height - ImgFixingSettings.YBefor);
                Mat newImg = new Mat(img, rect);
                img = newImg;
            }

            if (ImgFixingSettings.Diminish!=1) Cv2.Resize(img, img, new OpenCvSharp.Size(img.Width / ImgFixingSettings.Diminish, img.Height / ImgFixingSettings.Diminish));
            if (ImgFixingSettings.Zoom > 1)
            {
                int Width = img.Width, Height = img.Height, x1 = (int)(Width * (ImgFixingSettings.Zoom - 1) / 2), y1 = (int)(Height * (ImgFixingSettings.Zoom - 1) / 2);
                Mat blackImg = new Mat((int)(Height * ImgFixingSettings.Zoom), (int)(Width * ImgFixingSettings.Zoom), MatType.CV_8UC3, new Scalar(0, 0, 0));

                Mat roi = blackImg[y1, y1 + Height, x1, x1 + Width];
                Cv2.Resize(img, img, new OpenCvSharp.Size(roi.Width, roi.Height));
                Cv2.CopyTo(img, roi);
                img = blackImg;
            }

            if (ImgFixingSettings.BlackWhiteMode) Cv2.CvtColor(img, img, ColorConversionCodes.BGR2GRAY);
            if (ImgFixingSettings.Rotation90 == 1) Cv2.Rotate(img, img, RotateFlags.Rotate90Clockwise);
            else if (ImgFixingSettings.Rotation90 == 2) Cv2.Rotate(img, img, RotateFlags.Rotate180);
            else if (ImgFixingSettings.Rotation90 == 3) Cv2.Rotate(img, img, RotateFlags.Rotate90Counterclockwise);

            if (Rezult == null) Rezult = new Mat();
            if (ImgFixingSettings.Distortion)
            {
                double[] distCoeffs = new double[] { ImgFixingSettings.DistorSettings.A, ImgFixingSettings.DistorSettings.B, ImgFixingSettings.DistorSettings.C, ImgFixingSettings.DistorSettings.D, ImgFixingSettings.DistorSettings.E };
                InputArray _cameraMatrix = InputArray.Create<double>(new double[,]
                    {
                        { ImgFixingSettings.DistorSettings.Sm11, ImgFixingSettings.DistorSettings.Sm12, ImgFixingSettings.DistorSettings.Sm13 },
                        { ImgFixingSettings.DistorSettings.Sm21, ImgFixingSettings.DistorSettings.Sm22, ImgFixingSettings.DistorSettings.Sm23 },
                        { ImgFixingSettings.DistorSettings.Sm31, ImgFixingSettings.DistorSettings.Sm32, ImgFixingSettings.DistorSettings.Sm33 }
                    });
                InputArray _distCoeffs = InputArray.Create<double>(distCoeffs);
                Cv2.Undistort(img, Rezult, _cameraMatrix, _distCoeffs);

                //double[] array_ = (double[])distCoeffs.Clone();
                //RezultRTB.Text = $"k1:{array_[0]};\n k2:{array_[1]}; \n k3:{array_[4]}; \n p1:{array_[3]}; \n p2:{array_[2]};";
            }
            else Rezult = img;

            if (ImgFixingSettings.CropAfterChkBox)
            {
                int Y = ImgFixingSettings.YAfter, X = ImgFixingSettings.XAfter, dY = ImgFixingSettings.DYAfter, dX = ImgFixingSettings.DXAfter;
                if (Y < 0) Y = 0; if (X < 0) X = 0;
                if (Y > Rezult.Width) Y = Rezult.Width / 2;
                if (X > Rezult.Height) X = Rezult.Height / 2;

                if (dY <= 0 || Y + dY > Rezult.Height) dY = Rezult.Height - Y;
                if (dX <= 0 || X + dX > Rezult.Width) dX = Rezult.Width - X;
                ImgFixingSettings.YAfter = Y;
                ImgFixingSettings.XAfter = X;
                ImgFixingSettings.DYAfter = dY;
                ImgFixingSettings.DXAfter = dX;

                Rect rect;
                if (dY != 0 || dX != 0) rect = new Rect(X, Y, dX, dY);
                else rect = new Rect(X, Y, Rezult.Width - X, Rezult.Height - Y);
                Rezult = new Mat(Rezult, rect);
            }

            if (ImgFixingSettings.ShowGrid)
            {
                int n = 6;
                Cv2.Line(Rezult, Rezult.Width / n, 0, Rezult.Width / n, Rezult.Height, Scalar.Red, 1);
                Cv2.Line(Rezult, Rezult.Width - Rezult.Width / n, 0, Rezult.Width - Rezult.Width / n, Rezult.Height, Scalar.Red, 1);
                Cv2.Line(Rezult, 0, Rezult.Height / n, Rezult.Width, Rezult.Height / n, Scalar.Red, 1);
                Cv2.Line(Rezult, 0, Rezult.Height - Rezult.Height / n, Rezult.Width, Rezult.Height - Rezult.Height / n, Scalar.Red, 1);
            }

            return Rezult;
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
                    var TempArray = matches.Where(x=>x.Length > 1).ToArray();

                    // foreach (OpenCvSharp.DMatch[] items in matches.Where(x => x.Length > 1))
                    foreach (OpenCvSharp.DMatch[] items in TempArray)
                    {
                        if (items[0].Distance < 0.5 * items[1].Distance)
                        {
                            pointsSrc.Add(keyPointsSrc[items[0].QueryIdx].Pt);
                            pointsDst.Add(keyPointsTo[items[0].TrainIdx].Pt);
                            goodMatches.Add(items[0]);
                           // RTB.Text += $"{keyPointsSrc[items[0].QueryIdx].Pt.X}, {keyPointsSrc[items[0].QueryIdx].Pt.Y}\n";
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

                    Cv2.Resize(outMat, outMat, new OpenCvSharp.Size((int)(outMat.Width * ImgFixingSettings.Zoom), (int)(outMat.Height * ImgFixingSettings.Zoom)));
                    return outMat;
                }
            }
        }

        //public bool DawnScal(string href, double multiple)
        //{
        //    if(string.IsNullOrEmpty(href)) return false;
        //    if(fileEdit.IsDirectory(href))
        //    {
        //        if (!Directory.Exists(href)) return false;
        //        FileInfo[] fileList = fileEdit.SearchFiles(href);
        //        DawnScal(fileList[0], multiple);
        //    }
        //    else return DawnScal(new FileInfo(href), multiple);

        //    return true;
        //}

        //public bool DawnScal(FileInfo file, double multiple)
        //{
        //    if (!File.Exists(file.FullName)) return false;
        //    Mat img = new Mat(file.FullName);
        //    Cv2.Resize(img, img, new OpenCvSharp.Size(img.Width / multiple, img.Height / multiple));
        //    string dir = Path.GetDirectoryName(file.FullName);
        //    var dir3 = file.Directory;
        //    string dir2 = file.Directory.Name;
            
        //    string newhref = dir + Path.DirectorySeparatorChar + Path.GetFileNameWithoutExtension(file.FullName) + ".x" + multiple.ToString() + file.Extension;
        //    img.SaveImage(newhref);
        //    return true;
        //}

        private Bitmap MatToBitmap(Mat mat)
        {
            if (mat.Width == 0 && mat.Height == 0) return null;
            else return BitmapConverter.ToBitmap(mat);
        }

        private bool ChkFixingFrame1()
        {
            if (FixingFrame1 == null) return SetErr("FixingFrame1 = null");
            if (FixingFrame1.Width == 0 || FixingFrame1.Height == 0) return SetErr("FixingFrame1.Width = 0 || FixingFrame1.Height = 0");

            return true;
        }

        public string GetImgFixingPlan() => ImgFixingPlan;
    }
}