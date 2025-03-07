using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;

namespace ImgAssemblingLibOpenCV.Models
{
    public class FixFrames
    {
        private bool SaveRezultToFile;
        private string SavingRezultDir;
        public ImgFixingSettings ImgFixingSettings = new ImgFixingSettings();
        private const string imgDefoltFixingFile = "imgFixingSettings.oip";
        private string ImgFixingPlan = imgDefoltFixingFile;
        private FileEdit fileEdit = new FileEdit(new string[] { "*.jpeg", "*.jpg", "*.png", "*.bmp" });
        public event Action<int> ProcessChanged;
        public event Action<string> TextChanged;
        public bool IsErr { get; set; } = false;
        public List<string> ErrList { get; set; } = new List<string>();
        public string ErrText { get; set; } = string.Empty;
        private Mat OriginalFrame { get; set; }
        private Mat FixingFrame1 { get; set; }
        private Mat FixingFrame2 { get; set; }

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="imgFixingSettings">Настройки по которым коректируктся фрагмент</param>
        /// <param name="file">Фрагмент для корректировки</param>
        public FixFrames(ImgFixingSettings imgFixingSettings, string file)
        {
            ImgFixingSettings = imgFixingSettings;
            TryReadSettings();
            LoadImg(file);
        }

        /// <summary>
        /// Конструктор для склейки дух изображений
        /// </summary>
        /// <param name="imgFixingPlan">Файл с планом корректировки</param>
        /// <param name="file">Фрагмент для корректировки</param>
        public FixFrames(string file1, string file2)
        {
            FixingFrame1 = new Mat(file1);
            FixingFrame2 = new Mat(file2);
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
        public bool StitchTwoImg()
        {
            bool IsGorizontal = true;

            if (IsGorizontal && FixingFrame1.Height != FixingFrame2.Height) return SetErr("Высота изображения несовместима");
            if (!IsGorizontal && FixingFrame1.Width != FixingFrame2.Width) return SetErr("Ширина изображения несовместима");

            if(FixingFrame1 ==null) FixingFrame1 = new Mat();
            if (!IsGorizontal) Cv2.VConcat(FixingFrame1, FixingFrame2, FixingFrame1);
            else Cv2.HConcat(FixingFrame1, FixingFrame2, FixingFrame1);

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

            if (Rezult == null) Rezult = new Mat();
            if (ImgFixingSettings.Zoom == 0) ImgFixingSettings.Zoom = 1;
            Cv2.Resize(FixingFrame1, Rezult, new OpenCvSharp.Size((int)(FixingFrame1.Width * ImgFixingSettings.Zoom), (int)(FixingFrame1.Height * ImgFixingSettings.Zoom)));
            return true;
        }

        public Bitmap GetOriginalFrame() => BitmapConverter.ToBitmap(FixingFrame1);
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
                    //ImgFixingSettings imgFixingSettings;
                    fileEdit.LoadeJson(loadingFile, out ImgFixingSettings);
                    //SetImgFixingSettings(imgFixingSettings);
                }
                catch (IOException e)
                {
                    SetErr("The file could not be read: " + e.Message + "!!!\n");
                }
            }
            else SetErr("Err TryReadSettings.файл загрузки не найден!!!\n Загруженны настройки поумолчанию.");
        }

        //private void SetImgFixingSettingsWD(ImgFixingSettings imgFixingSettings) // для старых версий загрузка плана без настроек дисторсии
        //{
        //    ImgFixingSettings = imgFixingSettings;

        //    //bool AutoReloadSave = AutoReloadChkBox.Checked;
        //    //AutoReloadChkBox.Checked = false;
        //    //DistChkBox.Checked = false;

        //    //if (imgFixingSettings.Zoom < 1) imgFixingSettings.Zoom = 1;
        //    //Zoom = imgFixingSettings.Zoom;
        //    //ZoomLbl.Text = imgFixingSettings.Zoom.ToString();
        //    //rotation90 = imgFixingSettings.Rotation90;
        //    //BlackWhiteChkBox.Checked = imgFixingSettings.BlackWhiteMode;
        //    //CropAfterChkBox.Checked = imgFixingSettings.CropAfterChkBox;
        //    //XAfterTxtBox.Text = imgFixingSettings.XAfter.ToString();
        //    //YAfterTxtBox.Text = imgFixingSettings.YAfter.ToString();
        //    //dXAfterTxtBox.Text = imgFixingSettings.DXAfter.ToString();
        //    //dYAfterTxtBox.Text = imgFixingSettings.DYAfter.ToString();
        //    //AutoReloadChkBox.Checked = AutoReloadSave;
        //}
        private ImgFixingSettings GetImgFixingSettings() => ImgFixingSettings; // Получить текущие настройки
        
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
        //private Mat EditImg(string file = "")
        //{
        //    //if (string.IsNullOrEmpty(file) && !string.IsNullOrEmpty(InputDirTxtBox.Text) && !string.IsNullOrEmpty(InputFileTxtBox.Text))
        //    //    file = fileEdit.DirFile(InputDirTxtBox.Text, InputFileTxtBox.Text);
        //    if (!File.Exists(file))
        //    {
        //        SetErr("Err File: " + file + " не найден!!!");
        //        return new Mat();
        //    }
        //    return EditImg(Cv2.ImRead(file));
        //}

        /// <summary>
        /// Исправление дисторсии и сохранение кадров в отдельную папку
        /// </summary>
        /// <param name="param">Параметр синхронизации</param>
        /// <param name="outputDir">Папка для сохранения</param>
        /// <returns></returns>
        public bool FixImges(object param, string outputDir = "")
        {
            SynchronizationContext context = (SynchronizationContext)param;

            //ShowGridСhckBox.Checked = false;
            //if (string.IsNullOrEmpty(outputDir))
            //{
            //    if (!Directory.Exists(InputDirTxtBox.Text)) return false;
            //    outputDir = OutputDirTxtBox.Text;
            //}

            //if (!fileEdit.ChkDir(outputDir)) return false;
            //FileInfo[] fileList = fileEdit.SearchFiles(InputDirTxtBox.Text);

            //if (fileList == null) return SetErr("ERR FixImges.fileList == null !!!");
            //for (int i = 0; i < fileList.Length; i++)
            //{
            //    string outputFileNumber = outputDir + Path.DirectorySeparatorChar + fileList[i].Name;
            //    EditImg(fileList[i].FullName).Save(outputFileNumber);
            //    context.Send(OnProgressChanged, i * 100 / fileList.Length);
            //    context.Send(OnTextChanged, "Imges Fixing " + i * 100 / fileList.Length + " %");
            //}

            context.Send(OnProgressChanged, 100);
            context.Send(OnTextChanged, "Imges Fixing 100 %");
            return IsErr;
        }

        public Bitmap[] FixImges(object param, Bitmap[] dataArray)
        {
            if (dataArray == null || dataArray.Length == 0)
            {
                SetErr("ERR FixImges.fileList == null !!!");
                return null;
            }

            bool sinchoniztioIsOn = param == null ? false : true;
            SynchronizationContext context = (SynchronizationContext)param;
            //ShowGridСhckBox.Checked = false;
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

        public bool LoadImg(string file)
        {
            if (string.IsNullOrEmpty(file)) return SetErr("File IsNullOrEmpty");
            if (!File.Exists(file)) return SetErr("File: " + file + " не найден");
            FixingFrame1 = EditImg(Cv2.ImRead(file));

            return ChkFixingFrame1();
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
            var mat = EditImg(FixingFrame1);
            return MatToBitmap(mat);
        }

        public bool CheckFixingImg(string imgFixingDir = "")
        {
            return false;
            //if (string.IsNullOrEmpty(imgFixingDir)) imgFixingDir = OutputDirTxtBox.Text;
            //if (!Directory.Exists(imgFixingDir)) return false;

            //FileInfo[] fileList = fileEdit.SearchFiles(InputDirTxtBox.Text);
            //for (int i = 0; i < fileList.Length; i++)
            //    if (!File.Exists(imgFixingDir + Path.DirectorySeparatorChar + fileList[i].Name)) return false;
            //return true;
        }

        private Mat Rezult {  get; set; }   

        private Mat EditImg(Mat img)
        {
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
                //if (string.IsNullOrEmpty(XAfterTxtBox.Text)) dYAfterTxtBox.Text = "0";
                //if (string.IsNullOrEmpty(YAfterTxtBox.Text)) dYAfterTxtBox.Text = "0";
                //if (string.IsNullOrEmpty(dXAfterTxtBox.Text)) dYAfterTxtBox.Text = Rezult.Width.ToString();
                //if (string.IsNullOrEmpty(dXAfterTxtBox.Text)) dYAfterTxtBox.Text = Rezult.Width.ToString();

                //int Y = 0, X = 0, dY = 0, dX = 0;
                //Int32.TryParse(XAfterTxtBox.Text, out X);
                //Int32.TryParse(YAfterTxtBox.Text, out Y);
                //if (Y < 0) Y = 0; if (X < 0) X = 0;
                //if (Y > Rezult.Width) Y = Rezult.Width / 2;
                //if (X > Rezult.Height) X = Rezult.Height / 2;
                //Int32.TryParse(dYAfterTxtBox.Text, out dY);
                //Int32.TryParse(dXAfterTxtBox.Text, out dX);

                //if (dY <= 0 || Y + dY > Rezult.Height) dY = Rezult.Height - Y;
                //if (dX <= 0 || X + dX > Rezult.Width) dX = Rezult.Width - X;
                //XAfterTxtBox.Text = X.ToString();
                //YAfterTxtBox.Text = Y.ToString();
                //dYAfterTxtBox.Text = dY.ToString();
                //dXAfterTxtBox.Text = dX.ToString();

                int Y = ImgFixingSettings.YAfter, X = ImgFixingSettings.XAfter, dY = ImgFixingSettings.DYAfter, dX = ImgFixingSettings.DXAfter;
               // Int32.TryParse(XAfterTxtBox.Text, out X);
                //Int32.TryParse(YAfterTxtBox.Text, out Y);
                if (Y < 0) Y = 0; if (X < 0) X = 0;
                if (Y > Rezult.Width) Y = Rezult.Width / 2;
                if (X > Rezult.Height) X = Rezult.Height / 2;
                //Int32.TryParse(dYAfterTxtBox.Text, out dY);
                //Int32.TryParse(dXAfterTxtBox.Text, out dX);

                if (dY <= 0 || Y + dY > Rezult.Height) dY = Rezult.Height - Y;
                if (dX <= 0 || X + dX > Rezult.Width) dX = Rezult.Width - X;
                ImgFixingSettings.YAfter = X;
                ImgFixingSettings.XAfter = Y;
                ImgFixingSettings.DYAfter = dY;
                ImgFixingSettings.DXAfter = dX;

                Rect rect;
                if (dY != 0 || dX != 0) rect = new Rect(X, Y, dX, dY);
                else rect = new Rect(X, Y, Rezult.Width - X, Rezult.Height - Y);
                Rezult = new Mat(Rezult, rect);
            }

            //if (ShowGridСhckBox.Checked)
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