using OpenCvSharp;
using WinFormsApp1.Enum;
using System.Collections.Generic;
using System;
using System.IO;
using System.Threading;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;

namespace ImgAssemblingLib.Models
{
    public class StitchingBlock
    {
        private bool WorkingWBitmap { get; set; } = true; // Работа с блоком битмапов, а не с файлами
        private double[] Precision = new double[] { 0.01, 0.05, 0.1, 0.2, 0.3, 0.5, 0.75, 1.0, 1.5, 2.0, 5, 10, 20, 50, 100, 200, 300 };// Таблица точности ключевых точек
        private int minShift = 10; // минимально смещение картинок которое допускается из за погрешности определения ключевых точек
        private const int MinSufficientNumberOfPoints = 5; // минимальное и максимальное необходимое количество ключевых точек 
        private bool averageShift { get; set; } = false; // Вычисляемое смещение заменяется на среднее(иногда бывает полезно нпример для того что бы избавится от столбов)
        private bool Percent { get; set; } = false;
        private int From { get; set; } = 0;
        private int To { get; set; } = 100;
        private bool SelectSearchArea { get; set; } = false;
        private float MinHeight { get; set; } = 735;
        private float MaxHeight { get; set; } = 968;
        private float MinWight { get; set; } = 0;
        private float MaxWight { get; set; } = 0;
        public EnumDirection? Direction { get; set; }
        private List<SelectedFiles> SelectedFiles { get; set; }
        public string StitchingInfo { get; set; } = string.Empty;
        public string MainDir { get; set; }
        public bool IsErr { get; set; } = false;
        private EnumErrCode ErrCode { get; set; } = EnumErrCode.NoErr;
        public string ErrText { get; set; } = string.Empty;
        private List<string> ErrList = new List<string>();
        public static bool StopProcess { get; set; } = false;
        public bool AllPointsChkBox { get; set; } = false;
        public bool AdditionalFilter { get; set; } = false;
        public event Action<int> ProcessChanged;
        public event Action<string> TextChanged;
        public event Action<Mat> ChangImg;
        public DrawMatchesFlags drawMatchesFlags { get; set; } = DrawMatchesFlags.NotDrawSinglePoints;
        private FileEdit fileEdit { get; set; } = new FileEdit();

        //public StitchingBlock(AssemblyPlan assemblyPlan, Bitmap[] bitMapArray)
        public StitchingBlock(Bitmap[] bitMapArray)
        {
            if (bitMapArray == null || bitMapArray.Length == 0)
            {
                SetErr("Err StitchingBlock.bitMapArray = null || bitMapArray.Length = 0!!!");
                return;
            }

            if (SelectedFiles == null) SelectedFiles = new List<SelectedFiles>();
            else SelectedFiles.Clear();

            int i = 0, Width = 0, Height = 0;
            for (i = From; i < bitMapArray.Length; i++)
            {
                Mat mat = OpenCvSharp.Extensions.BitmapConverter.ToMat(bitMapArray[i]);
                if (i == 0)
                {
                    Width = mat.Width;
                    Height = mat.Height;
                }
                else if (mat.Width == 0 || mat.Height == 0)
                {
                    SetErr("Err GetVectorList.один из параметров (Width\\Height) нулевой!!!\n", EnumErrCode.ZeroFile);
                    return;
                }
                else if (mat.Width != Width || mat.Height != Height)
                {
                    SetErr("Err StitchingBlock.не совпадают параметры в одном из блоков mat.Width != Width || mat.Height != Height",EnumErrCode.ParametersDontMatch);
                    return;
                }

                SelectedFiles.Add(new SelectedFiles() { Id = i, Mat = mat });
            }
        }
        public StitchingBlock(AssemblyPlan assemblyPlan):this(assemblyPlan.StitchingDirectory, assemblyPlan.AdditionalFilter, assemblyPlan.Percent, assemblyPlan.From, assemblyPlan.To, assemblyPlan.Period, assemblyPlan.SelectSearchArea, assemblyPlan.MinHeight, assemblyPlan.MaxHeight, assemblyPlan.MinWight, assemblyPlan.MaxWight)
        { }
        public StitchingBlock(string file, bool additionalFilter,bool percent = true, int from = 0, int to = 100, int period = 1, bool selectSearchArea = false,float minHeight =0,float maxHeight = 0,float minWight=0,float maxWight=0)
        {
            if (string.IsNullOrEmpty(file))
            {
                SetErr("Err StitchingBlock.string.IsNullOrEmpty(File1TxtBox.Text)!!!");
                return;
            }
            if (!fileEdit.ChkFileDir(file))
            {
                SetErr("Err StitchingBlock. "+ file + " not Exists!!!");
                return;
            }

            if (fileEdit.IsDirectory(file)) MainDir = file;
            else MainDir = Path.GetDirectoryName(file);

            if (string.IsNullOrEmpty(MainDir))
            {
                SetErr("Err StitchingBlock.string.IsNullOrEmpty(MainDir)!!!");
                return;
            }
            FileInfo[] fileList = fileEdit.SearchFiles(MainDir);
            fileList = fileList.Where(f => f.Name.IndexOf("Result") == -1).ToArray();
            if (fileList.Length == 0)
            {
                SetErr("Err StitchingBlock.файлы в директории: " + MainDir + " не найдены!!!");
                return;
            }

            WorkingWBitmap = false;
            Percent = percent;
            From = from; 
            To = to;

            SelectSearchArea = selectSearchArea;
            MinHeight = minHeight;
            MaxHeight = maxHeight;
            MinWight = minWight;
            MaxWight = maxWight;

            if (From < 0) From = 0;

            if (Percent)
            {
                if (To < From) { From = 0; To = 100; }
                if (To > 100) To = 100;
                From = fileList.Length * From / 100;
                To = fileList.Length * To / 100;
            }
            else
            {
                if (To < From) To = From + 2;
                if (To > fileList.Length) To = fileList.Length;
                if (From > fileList.Length - 1) From = fileList.Length - 2;
                
            }

            if (SelectedFiles == null) SelectedFiles = new List<SelectedFiles>();
            else SelectedFiles.Clear();

            int i = 0;
            for (i = From; i < To; i = i + period)
                SelectedFiles.Add(new SelectedFiles(){Id = i,FullName = fileList[i].FullName});


            //Если нет последнего файла, добавляем его
            if(period>1 && (To==100 || To == fileList.Length) && SelectedFiles[SelectedFiles.Count - 1].FullName != fileList[fileList.Length - 1].FullName)
                SelectedFiles.Add(new SelectedFiles(){Id = i,FullName = fileList[fileList.Length - 1].FullName});

            AdditionalFilter = additionalFilter;
        }

        public void OnChangedImg(object i)
        {
            if (ChangImg != null) ChangImg((Mat)i);
        }

        public void OnProgressChanged(object i)
        {
            if (ProcessChanged != null) ProcessChanged((int)i);
        }
        public void OnTextChanged(object txt)
        {
            if (TextChanged != null) TextChanged((string)txt);
        }
        public List<SelectedFiles> GetSelectedFiles() => SelectedFiles;

        public void FindKeyPoints(object param)
        {
            StopProcess = false;
            List<EnumDirection?> directionsList = new List<EnumDirection?>();
            SynchronizationContext context = (SynchronizationContext)param;
            bool contextIsOn = context == null? false:true;
            SelectedFiles firstSelectedFiles = new SelectedFiles(), secondSelectedFiles = new SelectedFiles();
            int copy = -1;

            for (int i = 0; i < SelectedFiles.Count; i++)
            {
                EraseError();
                if (StopProcess)
                {
                    if(contextIsOn) context.Send(OnTextChanged, "Stop Process");
                    return;
                }

                if (i != 0)
                {
                    if (copy == -1)firstSelectedFiles = SelectedFiles[i - 1];

                    secondSelectedFiles = SelectedFiles[i];
                    firstSelectedFiles.StitchingFile = secondSelectedFiles.FullName;
                    if(WorkingWBitmap) firstSelectedFiles.Hint = "Stitching Imgs " + firstSelectedFiles.Id + " - " + secondSelectedFiles.Id;
                    else firstSelectedFiles.Hint = "Stitching Imgs " + Path.GetFileNameWithoutExtension(firstSelectedFiles.FullName) + " - " + Path.GetFileName(secondSelectedFiles.FullName);

                    // Получаем список векторов по ключевым точкам
                    List<Vector> VectorList = new List <Vector>();
                    if(WorkingWBitmap) VectorList = GetVectorList(SelectedFiles[i - 1].Mat, SelectedFiles[i].Mat);
                    else VectorList = GetVectorList(firstSelectedFiles.FullName, secondSelectedFiles.FullName);

                    if (IsErr || VectorList.Count == 0)
                    {
                        copy = i - 1;
                        secondSelectedFiles.IsErr = true;
                        secondSelectedFiles.ErrCode = ErrCode;
                        if (ErrCode == EnumErrCode.Copy)secondSelectedFiles.ErrText = "COPY " + firstSelectedFiles.FullName + " - " + secondSelectedFiles.FullName;
                        else if (!string.IsNullOrEmpty(ErrText)) secondSelectedFiles.ErrText = ErrText;
                        else secondSelectedFiles.ErrText = "Err Подходящие точки не найдены!!!";
                    }
                    else
                    {
                        VectorInfo vectorInfo = GetAverages(VectorList);
                        firstSelectedFiles.VectorList = VectorList;
                        firstSelectedFiles.AverageXShift = vectorInfo.AverageXShift;
                        firstSelectedFiles.AverageYShift = vectorInfo.AverageYShift;
                        // firstSelectedFiles.AverageShift = Math.Abs(vectorInfo.AverageYShift) < Math.Abs(vectorInfo.AverageXShift) ? Math.Round(vectorInfo.AverageXShift) : Math.Round(vectorInfo.AverageYShift);
                        firstSelectedFiles.AverageShift = Math.Abs(vectorInfo.AverageYShift) < Math.Abs(vectorInfo.AverageXShift) ? vectorInfo.AverageXShift : vectorInfo.AverageYShift;
                        firstSelectedFiles.Direction = vectorInfo.Direction;
                        if (copy != -1)  copy = -1;
                        directionsList.Add(vectorInfo.Direction);
                    }
                }
                if (contextIsOn)
                {
                    context.Send(OnProgressChanged, i * 100 / SelectedFiles.Count);
                    context.Send(OnTextChanged, "Finding Key Points " + i * 100 / SelectedFiles.Count + " %");
                }
            }

            Direction = GetAverageDirection(SelectedFiles.Where(x=>x.ErrCode!=EnumErrCode.Copy).Select(x => x.Direction).ToList());
            if (contextIsOn)
            {
                context.Send(OnProgressChanged, 100);
                context.Send(OnTextChanged, "100 %");
            }
        }
        public List<Vector> GetVectorList(Mat matSrc, Mat matTo, bool makeFotoRezult = false)
        {

            List<Vector> goodPointList = new List<Vector>();
            List<DMatch> goodMatches = new List<DMatch>();

            KeyPoint[] keyPointsSrc, keyPointsTo;
            using (Mat matSrcRet = new Mat())
            using (Mat matToRet = new Mat())
            {
                using (var sift = OpenCvSharp.Features2D.SIFT.Create())
                {
                    sift.DetectAndCompute(matSrc, null, out keyPointsSrc, matSrcRet);
                    sift.DetectAndCompute(matTo, null, out keyPointsTo, matToRet);
                }

                using (var bfMatcher = new BFMatcher())
                {
                    var matches = bfMatcher.KnnMatch(matSrcRet, matToRet, k: 2);

                    if (matches.Length == 0)
                    {
                        SetErr("Err StitchImgsByPointsImgs.Length = 0 !!!", EnumErrCode.Err);
                        return goodPointList;
                    }

                    if (matches[0].Length == 2) goodPointList = GetVectors(matches, keyPointsSrc, keyPointsTo, out goodMatches);
                    else
                    {
                        SetErr("Err StitchImgsByPointsImgs.matches[0].Length != 2 !!!", EnumErrCode.Err);
                        return goodPointList;
                    }
                }

                if (goodPointList.Count == 0)
                {
                    if (string.IsNullOrEmpty(ErrText)) SetErr("Err StitchImgsByPointsImgs.ключевые точки не найдены!!!", EnumErrCode.PointsNotFound);
                    return goodPointList;
                }

                //foreach (var elem in goodPointList) elem.GetDirection();
                if (makeFotoRezult)// Если нужно создаем изображение с найденными точками
                {
                    VectorInfo vectorInfo = GetAverages(goodPointList);
                    goodMatches = goodMatches.Where(x => goodPointList.Any(y => y.MatchesId == x.QueryIdx)).ToList();

                    string text = goodPointList.Count + " points found " +
                        (Math.Abs(vectorInfo.AverageYShift) < Math.Abs(vectorInfo.AverageXShift) ? Math.Round(vectorInfo.AverageXShift, 2) : Math.Round(vectorInfo.AverageYShift, 2)).ToString() +
                          " " + vectorInfo.Direction + " Shift \n";

                    int i = 0;
                    foreach (Vector point in goodPointList) text += "P " + i++ + " Sh " + Math.Round(point.Delta, 2) + " " + point.Direction + " Shift " + point.Delta +
                         " Identity " + Math.Round(point.Identity, 2) + " CoDirection " + Math.Round(point.CoDirection, 2) + " dX " + Math.Round(point.dX, 2) + " dY " + Math.Round(point.Yfr, 2) + "\n";

                    OnTextChanged(text);
                    Mat RezultImg = new Mat();
                    Cv2.DrawMatches(matSrc, keyPointsSrc, matTo, keyPointsTo, goodMatches, RezultImg, flags: drawMatchesFlags);
                    OnChangedImg(RezultImg);
                }
            }
            return goodPointList;
        }

        public List<Vector> GetVectorList(string file1, string file2, bool makeFotoRezult = false)
        {
            using (Mat matSrc = new Mat(file1))
            using (Mat matTo = new Mat(file2))
            {
                List<Vector> goodPointList = new List<Vector>();
                List<DMatch> goodMatches = new List<DMatch>();
                //Mat matSrc = Cv2.ImRead(file1, ImreadModes.Grayscale);
                //Mat matTo = Cv2.ImRead(file2, ImreadModes.Grayscale);
                if (matSrc.Width == 0 || matSrc.Height == 0 || matTo.Width == 0 || matTo.Height == 0)
                {
                    SetErr("Err GetVectorList.один из фалов " + file1 + " или " + file2 + " не найден!!!\n", EnumErrCode.FileNotFound);
                    return goodPointList;
                }
            if (matSrc.Width != matTo.Width || matSrc.Height != matTo.Height)
            {
                SetErr("Err GetVectorList.один из парамеров фалов " + file1 + " или " + file2 + " не совпадает!!!\n", EnumErrCode.Err);
                return goodPointList;
            }
            KeyPoint[] keyPointsSrc, keyPointsTo;
                using (Mat matSrcRet = new Mat())
                using (Mat matToRet = new Mat())
                {
                    using (var sift = OpenCvSharp.Features2D.SIFT.Create())
                    {
                        sift.DetectAndCompute(matSrc, null, out keyPointsSrc, matSrcRet);
                        sift.DetectAndCompute(matTo, null, out keyPointsTo, matToRet);
                    }

                    using (var bfMatcher = new BFMatcher())
                    {
                        var matches = bfMatcher.KnnMatch(matSrcRet, matToRet, k: 2);

                        if (matches.Length == 0)
                        {
                            SetErr("Err StitchImgsByPointsImgs.Length = 0 !!!", EnumErrCode.Err);
                            return goodPointList;
                        }

                        if (matches[0].Length == 2) goodPointList = GetVectors(matches, keyPointsSrc, keyPointsTo, out goodMatches);
                        else
                        {
                            SetErr("Err StitchImgsByPointsImgs.matches[0].Length != 2 !!!", EnumErrCode.Err);
                            return goodPointList;
                        }
                    }

                    if (goodPointList.Count == 0)
                    {
                        if (string.IsNullOrEmpty(ErrText)) SetErr("Err StitchImgsByPointsImgs.ключевые точки не найдены!!!", EnumErrCode.PointsNotFound);
                        return goodPointList;
                    }

                    //foreach (var elem in goodPointList) elem.GetDirection();

                    if (makeFotoRezult)// Если нужно создаем изображение с найденными точками
                    {
                        VectorInfo vectorInfo = GetAverages(goodPointList);
                        goodMatches = goodMatches.Where(x => goodPointList.Any(y => y.MatchesId == x.QueryIdx)).ToList();

                        string text = goodPointList.Count + " points found " +
                            (Math.Abs(vectorInfo.AverageYShift) < Math.Abs(vectorInfo.AverageXShift) ? Math.Round(vectorInfo.AverageXShift, 2) : Math.Round(vectorInfo.AverageYShift, 2)).ToString() +
                              " " + vectorInfo.Direction + " Shift \n";

                        int i = 0;
                        foreach (Vector point in goodPointList) text += "P " + i++ + " Sh " + Math.Round(point.Delta, 2) + " " + point.Direction + " Shift " + point.Delta +
                             " Identity " + Math.Round(point.Identity, 2) + " CoDirection " + Math.Round(point.CoDirection, 2) + " dX " + Math.Round(point.dX, 2) + " dY " + Math.Round(point.Yfr, 2) + "\n";

                        OnTextChanged(text);
                        Mat RezultImg = new Mat();
                        Cv2.DrawMatches(matSrc, keyPointsSrc, matTo, keyPointsTo, goodMatches, RezultImg, flags: drawMatchesFlags);
                        OnChangedImg(RezultImg);
                    }
                }
                return goodPointList;
            }
        }

        public bool CheckAndFixErr(object param)
        {
            SynchronizationContext context = (SynchronizationContext)param;
            if (Direction == null)
            {
                EraseError();
                RemovingErrRecords(context);
                if (Direction == null) return SetErr("Err Direction == null!!!\n Основное напрвление движения определить не удалось!");
            }
            //AddStitchingInfo();
            if (!CheckErr()) // Проверка списка на ошибки и их количество
            {
                EraseError();
                context.Send(OnProgressChanged, 0);
                RemovingErrRecords(context);
            }

            return CheckErr();
        }
        public bool CheckErr()
        {
            if (Direction == null) return SetErr("Err Direction == null!!!\n Напрвление движения не определенно!");
            if (SelectedFiles.Count < 15) return true; // Если кадров мало то проверки на ошибки бесполезны
            // Пересоздаем временный список без последнего элемента, т.к. в посленем элементе инфа всегда не заполненая 
            var SelectedFilesTempList = SelectedFiles.Take(SelectedFiles.Count - 1).ToList();

            // Проверка на замедление/остановку
            List<double> MidlAverageShift = new List<double>();
            if (CheckForDecelerationAndStop(SelectedFilesTempList)> 50) // количество точек в процентах находящихся ниже минимальной планки погрешности
            {
                //var avSift = SelectedFiles.Select(x => x.AverageShift).ToList();
                //for (int i = 0; i<4; i++)
                //{
                //    if(i==0) MidlAverageShift = LineAveraging(avSift);
                //    else MidlAverageShift = LineAveraging(MidlAverageShift);
                //}
            }
            if(MidlAverageShift.Count>0)
            {
                // проверка на пересечение с 0 или пробуем определить приблизительный номер кадра остановки поезда
                int nOfFirstPoint = 0;
                int CrossZero = CheckForCrossZero(MidlAverageShift, out nOfFirstPoint);
                if(CrossZero>0 && nOfFirstPoint>0)
                {
                    if (nOfFirstPoint > 20) nOfFirstPoint -= 20; // Отступаем на несколько точек назад или на начало
                    else nOfFirstPoint = 0;
                    List<EnumDirection?> directionsList = new List<EnumDirection?>();

                    foreach (var item in SelectedFilesTempList)item.Hint = string.Empty;

                    for (int i = nOfFirstPoint+3; i< SelectedFilesTempList.Count-2; i++)
                    {
                        List<Vector> VectorList = GetVectorList(SelectedFilesTempList[nOfFirstPoint].FullName, SelectedFilesTempList[i].FullName);
                        if (VectorList.Count > 0)
                        {
                            VectorInfo vectorInfo = GetAverages(VectorList);
                            SelectedFilesTempList[i].VectorList = VectorList;
                            SelectedFilesTempList[i].AverageXShift = vectorInfo.AverageXShift;
                            SelectedFilesTempList[i].AverageYShift = vectorInfo.AverageYShift;
                            SelectedFilesTempList[i].AverageShift = Math.Abs(vectorInfo.AverageYShift) < Math.Abs(vectorInfo.AverageXShift) ? vectorInfo.AverageXShift : vectorInfo.AverageYShift;
                            SelectedFilesTempList[i].Direction = vectorInfo.Direction;
                            SelectedFilesTempList[i].Hint = " S of file" + SelectedFilesTempList[nOfFirstPoint].FullName + " - " + SelectedFilesTempList[i].FullName;
                        }
                        else
                        {
                            SelectedFilesTempList[i].AverageShift = 0;
                            SelectedFilesTempList[i].AverageXShift = 0;
                            SelectedFilesTempList[i].AverageYShift = 0;
                            SelectedFilesTempList[i].Direction = null;
                            SelectedFilesTempList[i].Hint = "VectorList.Count = 0!!!" + SelectedFilesTempList[nOfFirstPoint].FullName + " - " + SelectedFilesTempList[i].FullName; ;
                        }
                    }
                }
            }

            // Если есть векторара с разными напрвлениями то проверяем их количество
            if (SelectedFilesTempList.Any(x => x.Direction != Direction))
            {
                int x = 0, y = 0, maxY = 0;
                foreach (var elem in SelectedFilesTempList)
                {
                    if (elem.Direction != Direction)
                    {
                        x++;
                        y++;
                    }
                    else
                    {
                        if (maxY < y) maxY = y;
                        y = 0;
                    }
                }

                var N = x * 100 / SelectedFilesTempList.Count;

                return SetErr("Err Не все напрвления движения одинаковы!!!");
            }
            else
            {
                // Проверка на отсутсвие ключевых точек
                if (SelectedFilesTempList.Any(x => string.IsNullOrEmpty(x.StitchingFile)))
                    return SetErr("Err Есть пропуски в разделе StitchingFile!!!");
                else return true;
            }
        }

        private int CheckForCrossZero(List<double> midlAverageShift,out int nOfFirstPoint)
        {
            nOfFirstPoint = 0;
            if (midlAverageShift.Count == 0) return 0;
            int NOfZeroCrossing = 0; // Колличество пересечений с 0
            double prevPoint = 0;

            //foreach(var midl in midlAverageShift
            for (int i = 0; i< midlAverageShift.Count; i++)
            {
                if (prevPoint < 0 && midlAverageShift[i] > 0 || prevPoint > 0 && midlAverageShift[i] < 0)
                {
                    NOfZeroCrossing++;
                    if (nOfFirstPoint == 0) nOfFirstPoint = i;
                }
                prevPoint = midlAverageShift[i];
            }

            return NOfZeroCrossing;
        }

        private int CheckForDecelerationAndStop(List<SelectedFiles> SelectedFilesTempList)
        {
            int conter = 0;
            List<int> intList = new List<int>();
            List<double> AverageShift = new List<double>();
            List<double> AverageShiftAbs = new List<double>();
            foreach (var elem in SelectedFilesTempList)
            {
                if (Math.Abs(elem.AverageShift) <= 2 * minShift)
                {
                    intList.Add(elem.Id);
                    AverageShift.Add(elem.AverageShift);
                    AverageShiftAbs.Add(Math.Abs(elem.AverageShift));
                    conter++;
                }
            }
            int rezult = conter * 100 / SelectedFilesTempList.Count();
            return rezult;
        }

        private bool CheckForReplacement(int i, int j, out VectorInfo vectorInfo)
        {
            vectorInfo = new VectorInfo();

            if (i >= 1 && j>=0 && i>j && i < SelectedFiles.Count - 1 && j < SelectedFiles.Count - 2)
            {
                List<Vector> VectorList = GetVectorList(SelectedFiles[j].FullName, SelectedFiles[i].FullName);
                if (VectorList.Count == 0) return false;
                vectorInfo = GetAverages(VectorList);
                if (vectorInfo.Direction == Direction)return true;
            }
            return false;
        }

        private void RemovingErrRecords(SynchronizationContext context)
        {
            if (Direction == null)
            {
                int fr = -1, to = -1;
                for (int i = SelectedFiles.Count - 1; i > 0; i--)
                {
                    if (SelectedFiles[i].Direction == null)
                    {
                        if (to == -1) to = i;
                    }
                    else if (to != -1) fr = i;
                    
                    if (fr != -1)
                    {
                        to = to - fr;
                        SelectedFiles.RemoveRange(fr, to);
                        fr = -1;
                        to = -1;
                    }
                }

                Direction = GetAverageDirection(SelectedFiles.Select(x => x.Direction).ToList());
                return;
            }

            for (int i = SelectedFiles.Count - 2; i > -1; i--)
            {
                if (SelectedFiles[i].Direction != Direction || SelectedFiles[i].AverageShift == 0)
                {
                    if (i == SelectedFiles.Count - 2 || i == 0)SelectedFiles.RemoveAt(i); //Если точки крайние то просто их удаляем
                    else
                    {
                        List<VectorInfo> VectorListInfo = new List<VectorInfo>();
                        VectorInfo vectorInfo = new VectorInfo();
                        int k = -1;
                        int j = 0;
                        for (j = i - 1; j >= 0; j--)
                        {
                            bool rezult = CheckForReplacement(i + 1, j, out vectorInfo);
                            VectorListInfo.Add(vectorInfo);
                            if (rezult)
                            {
                                k = j;
                                break;
                            }
                        }

                        if (k == -1)
                        {
                            // удаляем все от 0 до i
                            SelectedFiles = SelectedFiles.Skip(i + 1).ToList();
                            break;
                        }
                        else
                        {
                            SelectedFiles[k].StitchingFile = SelectedFiles[i + 1].FullName;
                            SelectedFiles[k].AverageXShift = vectorInfo.AverageXShift;
                            SelectedFiles[k].AverageYShift = vectorInfo.AverageYShift;
                            SelectedFiles[k].AverageShift = Math.Abs(vectorInfo.AverageYShift) < Math.Abs(vectorInfo.AverageXShift) ? vectorInfo.AverageXShift : vectorInfo.AverageYShift;
                            SelectedFiles[k].Direction = vectorInfo.Direction;
                            SelectedFiles[k].Hint = "Stitching Imgs " + Path.GetFileNameWithoutExtension(SelectedFiles[k].FullName) + " - " + Path.GetFileName(SelectedFiles[i + 1].FullName);
                            SelectedFiles.RemoveAt(i);
                        }
                    }
                }
            }
        }
        
        int X = 0;
        internal Mat Stitch(object param, int Delta = 0)// Сборка кадров в один
        {
            Mat rezult = new Mat();
            if (SelectedFiles == null || SelectedFiles.Count == 0)
            {
                SetErr("Err Stitch.найденных для сборки изображений нет!!!");
                return rezult;
            }

            AddStitchingInfo();
            SelectedFiles = SelectedFiles.Where(x => !x.IsErr).ToList(); // Удаляем все ошибочные сектора
            if (Direction == EnumDirection.Right || Direction == EnumDirection.Down) InvertDirection(); // Если направление движения правое то делаем инверсию SelectedFiles

            SynchronizationContext context = (SynchronizationContext)param;
            // Сборка картинки по частям
            for (int i = 0; i < SelectedFiles.Count - 1; i++)
            {
                X = i;
                //if(i == SelectedFiles.Count - 2 && WorkingWBitmap)
                //{
                //    break;
                //}
                if (SelectedFiles[i].IsErr) continue;
                if (context != null) context.Send(OnProgressChanged, i * 100 / SelectedFiles.Count);
                if (context != null) context.Send(OnTextChanged, "Frame Union " + i * 100 / SelectedFiles.Count + " %");

                if (SelectedFiles.Count == 2) // Вариант на случай если нужно собрать только 2 картинки
                {
                    int shift =(int)Math.Round(SelectedFiles[0].AverageShift);
                    Mat Img1 = new Mat(SelectedFiles[0].FullName);
                    Mat Img2 = new Mat(SelectedFiles[1].FullName);
                    if (Direction == EnumDirection.Down || Direction == EnumDirection.Up)
                    {
                        int d1 = Img1.Height / 2 + shift / 2 + Delta - 1;
                        int d2 = Img2.Height / 2 - shift / 2 + Delta;
                        int d3 = Img2.Height / 2 + shift / 2 - Delta - 1;

                        if (d1 >= 0 && d1 < Img1.Height - 1 && d2 >= 0 && d2 < Img2.Height - 1)
                        {
                            Rect rect1 = new Rect(0, 0, Img1.Width - 1, d1);
                            Mat dstroi1 = new Mat(Img1, rect1);
                            Rect rect2 = new Rect(0, d2, Img2.Width - 1, d3);
                            Mat dstroi2 = new Mat(Img2, rect2);
                            Cv2.VConcat(dstroi1, dstroi2, rezult);
                        }
                    }
                    else
                    {
                        int d1 = Img1.Width / 2 + shift / 2 + Delta - 1;
                        int d2 = Img2.Width / 2 - shift / 2 + Delta;
                        int d3 = Img2.Width / 2 + shift / 2 - Delta - 1;
                        if (d1 >= 0 && d1 < Img1.Width - 1 && d2 >= 0 && d2 < Img2.Width - 1)
                        {
                            Rect rect1 = new Rect(0, 0, d1, Img1.Height - 1);
                            Mat dstroi1 = new Mat(Img1, rect1);
                            Rect rect2 = new Rect(d2, 0, d3, Img2.Height - 1);
                            Mat dstroi2 = new Mat(Img2, rect2);
                            Cv2.HConcat(dstroi1, dstroi2, rezult);
                        }
                    }
                }
                else
                {
                    double shift = averageShift ? SelectedFiles[0].AverageShift : SelectedFiles[i].AverageShift;

                    if (WorkingWBitmap)
                    {
                        if (i == 0) rezult = JoinImg(SelectedFiles[i].Mat, SelectedFiles[i + 1].Mat, (int)Math.Round(shift), EnumFramePosition.First, Delta);
                        else if (i == SelectedFiles.Count - 2) rezult = JoinImg(rezult, SelectedFiles[i + 1].Mat, (int)Math.Round(shift), EnumFramePosition.Last, Delta);
                        else rezult = JoinImg(rezult, SelectedFiles[i + 1].Mat, (int)Math.Round(shift), EnumFramePosition.Midle, Delta);
                    }
                    else
                    {
                        if (i == 0) rezult = JoinImg(SelectedFiles[i].FullName, SelectedFiles[i + 1].FullName, (int)Math.Round(shift), EnumFramePosition.First, Delta);
                        else if (i == SelectedFiles.Count - 2) rezult = JoinImg(rezult, SelectedFiles[i + 1].FullName, (int)Math.Round(shift), EnumFramePosition.Last, Delta);
                        else rezult = JoinImg(rezult, SelectedFiles[i + 1].FullName, (int)Math.Round(shift), EnumFramePosition.Midle, Delta);
                    }
                }
                if (rezult.Width == 0 || rezult.Height == 0)
                {
                    SetErr("Итоговая картинка не собрана. Ошибка на "+i+ " кадре!!!");
                    break;
                }
            }

            if (context != null) context.Send(OnProgressChanged, 100);
            if (context != null) context.Send(OnTextChanged, "100 %");
            AddErrStitchingInfo();
            return rezult;
        }

        private void InvertDirection()
        {
            if (Direction == EnumDirection.Right) Direction = EnumDirection.Left;
            if (Direction == EnumDirection.Down) Direction = EnumDirection.Up;
            List<SelectedFiles> TempSelectedFiles = new List<SelectedFiles>();

            int y = 0;
            for (int i = SelectedFiles.Count - 1; i > -1; i--)
            {
                if (i == 0)
                {
                    TempSelectedFiles.Add(new SelectedFiles()
                    {
                        Id = y++,
                        FullName = SelectedFiles[0].FullName,
                        Mat = SelectedFiles[0].Mat,
                    });
                }
                else
                {
                    SelectedFiles tempSf;
                    if (SelectedFiles[i].IsErr)
                    {
                        tempSf = new SelectedFiles(SelectedFiles[i]);
                        tempSf.StitchingFile = SelectedFiles[i - 1].FullName;
                    }
                    else
                    {
                        if (!SelectedFiles[i - 1].IsErr)
                        {
                            tempSf = new SelectedFiles(SelectedFiles[i - 1]);
                            tempSf.StitchingFile = SelectedFiles[i - 1].FullName;
                            tempSf.Mat = SelectedFiles[i].Mat;
                        }
                        else
                        {
                            int z = i - 1;
                            while (!SelectedFiles[z--].IsErr)
                                if (z == 0) break;

                            tempSf = new SelectedFiles(SelectedFiles[z]);
                            tempSf.Mat = SelectedFiles[z].Mat;
                            tempSf.StitchingFile = SelectedFiles[i - 1].FullName;
                            //tempSf.StitchingFile = SelectedFiles[z].FullName;
                            tempSf.VectorList = SelectedFiles[z].VectorList;
                            tempSf.Direction = SelectedFiles[z].Direction;
                            tempSf.Hint = SelectedFiles[z].Hint;
                            tempSf.AverageXShift = SelectedFiles[z].AverageXShift;
                            tempSf.AverageYShift = SelectedFiles[z].AverageYShift;
                            tempSf.AverageShift = SelectedFiles[z].AverageShift;
                            tempSf.IsErr = SelectedFiles[z].IsErr;
                            tempSf.ErrCode = SelectedFiles[z].ErrCode;
                            tempSf.ErrText = SelectedFiles[z].ErrText;
                        }

                        tempSf.Id = y++;
                        tempSf.FullName = SelectedFiles[i].FullName;
                        tempSf.Direction = Direction;
                    }
                    TempSelectedFiles.Add(tempSf);
                }
            }

            // ??ToDo Вырезать все ошибочные элементы в начале и конце
            SelectedFiles = TempSelectedFiles;
        }
        
        public List<Vector> GetVectors(DMatch[][] matches, KeyPoint[] keyPointsSrc, KeyPoint[] keyPointsTo, out List<DMatch> goodMatches)
        {
            goodMatches = new List<DMatch>();
            List<Vector> goodPointList = new List<Vector>();
            if (matches == null) return goodPointList;
            int i = 0, n = 0;

            if (SelectSearchArea && matches.Length>30) // Отсееваем точки если выбран определенный сектор
            {
                double minHeight = MinHeight , maxHeight = MaxHeight , delta = Math.Abs(MinHeight - MaxHeight);
                var matches2 = matches.Where(x => keyPointsSrc[x[0].QueryIdx].Pt.Y > minHeight && keyPointsSrc[x[0].QueryIdx].Pt.Y < maxHeight && keyPointsTo[x[0].TrainIdx].Pt.Y > minHeight - 5 && keyPointsTo[x[0].TrainIdx].Pt.Y < maxHeight + 5).ToArray();

                if (delta < 10) delta = 10;
                minHeight -= delta / 2; maxHeight += delta / 2;
                while (matches2.Length <20)
                {
                    if (n++ > 10) break;
                    if (minHeight < 0) minHeight = 0;
                    //if (maxHeight > MaxHeight) maxHeight = MaxHeight;
                    matches2 = matches.Where(x => keyPointsSrc[x[0].QueryIdx].Pt.Y > minHeight && keyPointsSrc[x[0].QueryIdx].Pt.Y < maxHeight && keyPointsTo[x[0].TrainIdx].Pt.Y > minHeight - 5 && keyPointsTo[x[0].TrainIdx].Pt.Y < maxHeight + 5).ToArray();
                    minHeight -= delta / 2; maxHeight += delta / 2;
                }
                if(matches2.Length>=20) matches= matches2;
            }

            if (matches.Length == 0)
            {
                SetErr("Не найдены подходящие точки", EnumErrCode.PointsNotFound);
                return goodPointList;
            }

            for (i = 0; i < Precision.Length; i++)
            {
                goodMatches = new List<DMatch>();
                goodPointList = new List<Vector>();
                int SamePoints = 0;
                for (int j = 0; j < matches.Length; j++)
                {
                    var match = matches[j][0];
                    if (matches[j][0].Distance < Precision[i] * matches[j][1].Distance)
                    {
                        Vector vector = new Vector(match.QueryIdx, keyPointsSrc[match.QueryIdx].Pt.X, keyPointsSrc[match.QueryIdx].Pt.Y, keyPointsTo[match.TrainIdx].Pt.X, keyPointsTo[match.TrainIdx].Pt.Y);
                        if (AllPointsChkBox || !vector.isSamePoint)
                        {
                            goodPointList.Add(vector);
                            goodMatches.Add(matches[j][0]);
                        }
                        else if (vector.isSamePoint) SamePoints++;
                    }
                    //goodMatches = goodMatches.Where(x => goodPointList.Any(y => y.MatchesId == x.QueryIdx)).ToList();
                    //if (SelectSearchArea)
                    //{
                    //    if (matches[j][0].Distance < Precision[i] * matches[j][1].Distance && keyPointsSrc[match.QueryIdx].Pt.Y > MinHeight && keyPointsSrc[match.QueryIdx].Pt.Y < MaxHeight && keyPointsTo[match.TrainIdx].Pt.Y < MaxHeight + 5 && keyPointsTo[match.TrainIdx].Pt.Y > MinHeight - 5)
                    //    //if (matches[j][0].Distance < Precision[i] * matches[j][1].Distance && keyPointsSrc[match.QueryIdx].Pt.Y > MinHeight && keyPointsSrc[match.QueryIdx].Pt.Y < MaxHeight)
                    //    //if (matches[j][0].Distance < Precision[i] * matches[j][1].Distance && keyPointsSrc[match.QueryIdx].Pt.Y > MinHeight && keyPointsSrc[match.QueryIdx].Pt.Y < MaxHeight && keyPointsSrc[match.QueryIdx].Pt.X > MinWight && keyPointsSrc[match.QueryIdx].Pt.X < MaxWight)
                    //    {
                    //        Vector vector = new Vector(j, keyPointsSrc[match.QueryIdx].Pt.X, keyPointsSrc[match.QueryIdx].Pt.Y, keyPointsTo[match.TrainIdx].Pt.X, keyPointsTo[match.TrainIdx].Pt.Y);
                    //        if (AllPointsChkBox || !vector.isSamePoint)
                    //        {
                    //            goodPointList.Add(vector);
                    //            goodMatches.Add(matches[j][0]);
                    //        }
                    //        else if (vector.isSamePoint) SamePoints++;
                    //    }
                    //}
                    //else
                    //{
                    //    if (matches[j][0].Distance < Precision[i] * matches[j][1].Distance)
                    //    {
                    //        Vector vector = new Vector(j, keyPointsSrc[match.QueryIdx].Pt.X, keyPointsSrc[match.QueryIdx].Pt.Y, keyPointsTo[match.TrainIdx].Pt.X, keyPointsTo[match.TrainIdx].Pt.Y);
                    //        if (AllPointsChkBox || !vector.isSamePoint)
                    //        {
                    //            goodPointList.Add(vector);
                    //            goodMatches.Add(matches[j][0]);
                    //        }
                    //        else if (vector.isSamePoint) SamePoints++;
                    //    }
                    //}
                }

                if (SamePoints >= matches.Length * 0.9)
                {
                    SetErr("COPY", EnumErrCode.Copy);
                    return goodPointList;
                }
                if (matches.Length == goodPointList.Count) break;

                if (SelectSearchArea && goodPointList.Count > MinSufficientNumberOfPoints/2 && !AllPointsChkBox) break;
                if (goodPointList.Count > MinSufficientNumberOfPoints && !AllPointsChkBox) break;
               // if (goodPointList.Count > MaxSufficientNumberOfPoints && AllPointsChkBox) break;
            }

            PointFiltr pointFiltr = new PointFiltr(goodPointList);
            if (AdditionalFilter)
            {
                goodPointList = pointFiltr.PointFiltering();
                if (goodPointList.Count == 0)
                {
                    SetErr(pointFiltr.ErrText);
                    return goodPointList;
                }
            }

            if (goodPointList.Count >= 10)  goodPointList = pointFiltr.PointScreening();
            return goodPointList;
        }
        
        private EnumDirection? GetAverageDirection(List<EnumDirection?> directionList)
        {
            if (directionList == null) return null;
            if (directionList.Count() == 0) return null;
            var existDirections = directionList.Distinct().ToArray();// Получаем все существующие направления движения
            List<DirectionCheck> DirectionCheckList = new List<DirectionCheck>();
            foreach (EnumDirection? direction in existDirections)
            {
                var amaunt = directionList.Count(x => x == direction);
                DirectionCheckList.Add(new DirectionCheck
                {
                    EnumDirection = direction,
                    Amount = amaunt
                });
            }

            return DirectionCheckList.Where(x => x.Amount == DirectionCheckList.Max(y => y.Amount)).Select(z => z.EnumDirection).FirstOrDefault();
        }
        class DirectionCheck
        {
            public EnumDirection? EnumDirection { get; set; }
            public int Amount { get; set; }
        }
        public VectorInfo GetAverages(List<Vector> VectorList)
        {
            double dXSum = VectorList.Sum(x=>Math.Abs(x.dX)), dYSum = VectorList.Sum(x => Math.Abs(x.dY));
            return new VectorInfo()
            {
                Direction = GetAverageDirection(VectorList.Select(x => (EnumDirection?)x.Direction).ToList()),
                AverageXShift = dXSum / VectorList.Count,
                AverageYShift = dYSum / VectorList.Count
            };
        }

        private bool SetErr(string err, EnumErrCode enumErrCode = EnumErrCode.NoErr)
        {
            ErrCode = enumErrCode;
            if (IsErr) ErrList.Add(err);
            else IsErr = true;
            ErrText = err;
            return false;
        }
        public string GetErrText()
        {
            if (!IsErr) return string.Empty;
            if (ErrList.Count == 0) return ErrText;

            string errTxt = ErrText;
            foreach (string elem in ErrList) errTxt += "\n" + elem;
            return errTxt;
        }

        private void EraseError()
        {
            IsErr = false;
            ErrCode = EnumErrCode.NoErr;
            ErrText = string.Empty;
        }
        private void EraseErrors()
        {
            ErrList.Clear();
            ErrText = string.Empty;
            IsErr = false;
        }
        private void AddStitchingInfo()
        {
            StitchingInfo = string.Empty;
            for (int i = 0; i < SelectedFiles.Count - 2; i++)
            {
                if (SelectedFiles[i].AverageShift == 0 || SelectedFiles[i].Direction != Direction) StitchingInfo += SelectedFiles[i].FullName + " ??  Shift " + Math.Round(SelectedFiles[i].AverageShift, 2) + " Direction " + SelectedFiles[i].Direction + "\n";
                else StitchingInfo += SelectedFiles[i].Hint + " " + Math.Round(SelectedFiles[i].AverageShift, 2) + " " + SelectedFiles[i].Direction + "\n";
            }
        }
        private void AddErrStitchingInfo()
        {
            if (ErrList.Count != 0)
            {
                StitchingInfo += "\n\nError info:\n";
                foreach (var info in ErrList) StitchingInfo += info + "\n";
            }
        }
        //private Mat JoinImg(string file1, string file2, int shift, EnumFramePosition enumFramePosition, int Delta = 0) => JoinImg(new Mat(file1), file2, shift, enumFramePosition, Delta);
        private Mat JoinImg(string file1, string file2, int shift, EnumFramePosition enumFramePosition, int Delta = 0)
        {
            return JoinImg(new Mat(file1), new Mat(file2), shift, enumFramePosition, Delta);
        }

        //private Mat JoinImg(string file1, Mat Img2, int shift, EnumFramePosition enumFramePosition, int Delta = 0)
        //{
        //    return JoinImg(new Mat(file1), Img2, shift, enumFramePosition, Delta);
        //}

        private Mat JoinImg(Mat Img1, string file2, int shift, EnumFramePosition enumFramePosition, int Delta = 0)
        {
            return JoinImg(Img1, new Mat(file2), shift, enumFramePosition, Delta);
        }

        //private Mat JoinImg(Mat Img1, string file2, int shift, EnumFramePosition FramePosition, int Delta = 0)
        private Mat JoinImg(Mat Img1, Mat Img2, int shift, EnumFramePosition FramePosition, int Delta = 0)
        {
            shift = Math.Abs(shift);
            Mat rezult = new Mat();
            //if (!File.Exists(file2))
            //{
            //    SetErr("Err !File.Exists(file2) !!!");
            //    return rezult;
            //}
            //Mat Img2 = new Mat(file2);

            if (Img1.Width == 0 || Img1.Height == 0 || Img2.Width == 0 || Img2.Height == 0)
            {
                if (Img1.Width == 0) SetErr("Err Img1.Width == 0 !!!");
                if (Img1.Height == 0) SetErr("Err Img1.Height == 0 !!!");
                if (Img2.Width == 0) SetErr("Err Img2.Width == 0 !!!");
                if (Img2.Height == 0) SetErr("Err Img2.Height == 0 !!!");
                return rezult;
            }

            int h1 = Img1.Height / 2, h2 = Img2.Height / 2, w1 = Img1.Width / 2, w2 = Img2.Width / 2;
            if (Direction == EnumDirection.Up)
            {
                if (FramePosition == EnumFramePosition.First)
                {
                    int d1 = h1 + Delta - 1;
                    int d2 = h2 - shift + Delta;

                    if (d1 > 0 && d1 < Img1.Height - 1 && d2 > 0 && d2 < Img2.Height - 1)
                    {
                        Rect rect1 = new Rect(0, 0, Img1.Width - 1, d1);
                        Mat dstroi1 = new Mat(Img1, rect1);

                        Rect rect2 = new Rect(0, d2, Img2.Width - 1, shift);
                        Mat dstroi2 = new Mat(Img2, rect2);
                        Cv2.VConcat(dstroi1, dstroi2, rezult);
                    }
                    else SetErr("Err JoinImg.В одном из кадров превышена граница изображения!!!");
                }

                else if (FramePosition == EnumFramePosition.Midle)
                {
                    int d2 = h2 - shift + Delta;
                    if (d2 > 0 && d2 < Img2.Height - 1)
                    {
                        Rect rect2 = new Rect(0, d2, Img2.Width - 1, shift);
                        Mat dstroi2 = new Mat(Img2, rect2);
                        Cv2.VConcat(Img1, dstroi2, rezult);
                    }
                    else SetErr("Err JoinImg.В одном из кадров превышена граница изображения!!!");
                }

                else if (FramePosition == EnumFramePosition.Last)
                {
                    int d1 = h2 - shift + Delta;
                    int d2 = Img2.Height - h2 + shift - Delta - 1;

                    if (d1 > 0 && d1 < Img2.Height - 1 && d2 > 0 && d2 < Img2.Height - 1)
                    {
                        Rect rect2 = new Rect(0, d1, Img2.Width - 1, d2);
                        Mat dstroi2 = new Mat(Img2, rect2);
                        Cv2.VConcat(Img1, dstroi2, rezult);
                    }
                    else SetErr("Err JoinImg.В одном из кадров превышена граница изображения!!!");
                }
            }
            else if (Direction == EnumDirection.Left)
            {
                if (FramePosition == EnumFramePosition.First)
                {
                    int d1 = w1 + Delta - 1;
                    int d2 = w2 - shift + Delta;
                    if (d1 > 0 && d1 < Img1.Width - 1 && d2 > 0 && d2 < Img2.Width - 1)
                    {
                        Rect rect1 = new Rect(0, 0, d1, Img1.Height - 1);
                        Mat dstroi1 = new Mat(Img1, rect1);

                        Rect rect2 = new Rect(w2 - shift + Delta, 0, shift - 1, Img2.Height - 1);
                        Mat dstroi2 = new Mat(Img2, rect2);
                        Cv2.HConcat(dstroi1, dstroi2, rezult);
                    }
                    else SetErr("Err JoinImg.В одном из кадров превышена граница изображения!!!");
                }
                else if (FramePosition == EnumFramePosition.Midle)
                {
                    int d2 = w2 - shift + Delta;
                    int d3 = d2 + shift - 1;



                    if (d2 > 0 && d2 < Img2.Width - 1 && d3 > 0 && d3 < Img2.Width - 1)
                    {
                        Rect rect2 = new Rect(d2, 0, shift - 1, Img2.Height - 1);
                        Mat dstroi2 = new Mat(Img2, rect2);

                        if (X > 134)
                        {
                            Img1.SaveImage("1.bmp");
                            Img2.SaveImage("2.bmp");
                            dstroi2.SaveImage("3.bmp");
                        }

                        if (Img1.Height!= dstroi2.Height )
                        {
                            rect2 = new Rect(d2, 0, shift, Img2.Height);
                            dstroi2 = new Mat(Img2, rect2);
                        }

                        Cv2.HConcat(Img1, dstroi2, rezult);
                    }
                    else SetErr("Err JoinImg.В одном из кадров превышена граница изображения!!!");
                }
                else if (FramePosition == EnumFramePosition.Last)
                {
                    int d1 = w2 - shift + Delta - 1;
                    int d2 = Img2.Width - Delta - w2 + shift - 1;
                    if (d1 > 0 && d1 < Img2.Width - 1 && d2 > 0 && d2 < Img2.Width - 1)
                    {
                        Rect rect2 = new Rect(d1, 0, d2, Img2.Height - 1);
                        Mat dstroi2 = new Mat(Img2, rect2);
                        Cv2.HConcat(Img1, dstroi2, rezult);
                    }
                    else SetErr("Err JoinImg.В одном из кадров превышена граница изображения!!!");
                }
            }
            return rezult;
        }

        public async Task<bool> TryReadMapPlan(int from = 0, int to = 100)
        {
            StitchingPlan stitchingPlan = new StitchingPlan();
            try
            {fileEdit.LoadeJson(GetPlanName(), out stitchingPlan);}
            catch (IOException e)
            {SetErr("The file could not be read: " + e.Message + "!!!");}

            if(stitchingPlan == null) return false;

            if (MainDir == stitchingPlan.Dir && stitchingPlan.SelectedFiles != null)
            {
                if (from < 0 || to > stitchingPlan.SelectedFiles.Count - 1 || from > to) { from = 0; to = SelectedFiles.Count - 1; }

                if (from == 0 && to == 100) SelectedFiles = stitchingPlan.SelectedFiles;
                else SelectedFiles = stitchingPlan.SelectedFiles.Skip(from).Take(to - from).ToList();
                Direction = stitchingPlan.Direction;
                return true;
            }
            else return false;
        }
        public async Task TrySaveMapPlan()
        {
            if (!Directory.Exists(MainDir)) return;
            StitchingPlan stitchingPlan = new StitchingPlan(MainDir, SelectedFiles, Direction);
            string saveFile = GetPlanName();
            fileEdit.SaveJson(saveFile, stitchingPlan);
        }

        public bool DeletPlan()
        {
            string plan = GetPlanName();
            if (string.IsNullOrEmpty(plan)) return false;
            File.Delete(plan);
            if (!CheckPlan()) return true;
            else return false;
        }
        public bool CheckPlan()
        {
            if (File.Exists(GetPlanName())) return true;
            else return false;
        }

        private string GetPlanName()
        {
            string PlanName = string.Empty;

            FileInfo[] fileList = fileEdit.SearchFiles(MainDir);
            fileList = fileList.Where(f => f.Name.IndexOf("Result") == -1).ToArray();
            if (fileList.Length == 0)
            {
                SetErr("Err TryReadMapPlan.файлы в директории: " + MainDir + " не найдены!!!");
                return PlanName;
            }
            else PlanName = fileEdit.GetDefoltDirectory() + "Plan" + fileList.Length + ".spn";

            return PlanName;
        }
        //private List<double> LineAveraging(List<double> line, int N = 10)
        //{
        //    List<double> result = new List<double>();
        //    Queue<double> queue = new Queue<double>();
        //    for (int i = 0; i < (N - 1) / 2; i++) result.Add(0);

        //    foreach (var elem in line)
        //    {
        //        queue.Enqueue(elem);
        //        if (queue.Count == N + 1) queue.Dequeue();
        //        if (queue.Count == N) result.Add(queue.Sum() / N);
        //    }

        //    for (int i = 0; i < (N - 1) / 2; i++)
        //    {
        //        if ((N / 2 + 1) >= result.Count) result[i] = result[N / 2];
        //        else result[i] = result[N / 2 + 1];
        //    }
        //    while (result.Count < line.Count) result.Add(result[result.Count - 1]);
        //    return result;
        //}
    }
}