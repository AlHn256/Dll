using OpenCvSharp;
using WinFormsApp1.Enum;
using System.Collections.Generic;
using System;
using System.IO;
using System.Threading;
using System.Linq;
using System.Drawing;
using OpenCvSharp.Extensions;
using System.Diagnostics;

namespace ImgAssemblingLibOpenCV.Models
{
    public class StitchingBlock
    {
        #region params
        private bool WorkingWBitmap { get; set; } = true; // Работа с блоком битмапов, а не с файлами
        public bool NewEngin { get; set; } 
        private double[] Precision = new double[] { 0.01, 0.05, 0.1, 0.2, 0.3, 0.5, 0.75, 1.0, 1.5, 2.0, 5, 10, 20, 50, 100, 200, 300 };// Таблица точности ключевых точек
        private const int minShift = Vector.PixelError * 2; // минимально смещение картинок которое допускается из за погрешности
        private const int MinSufficientNumberOfPoints = 5; // минимальное необходимое количество ключевых точек 
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
        public event Action<Bitmap> ChangBitmapImg;
        public DrawMatchesFlags drawMatchesFlags { get; set; } = DrawMatchesFlags.NotDrawSinglePoints;
        private FileEdit fileEdit { get; set; } = new FileEdit();
        #endregion
        private bool SetErr(string err, EnumErrCode enumErrCode = EnumErrCode.NoErr)
        {
            ErrCode = enumErrCode;
            ErrList.Add(err);
            IsErr = true;
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
                Mat mat = BitmapConverter.ToMat(bitMapArray[i]);
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
        public StitchingBlock(Bitmap[] bitMapArray, int period)
        {
            if (bitMapArray == null || bitMapArray.Length == 0)
            {
                SetErr("Err StitchingBlock.bitMapArray = null || bitMapArray.Length = 0!!!");
                return;
            }

            if (SelectedFiles == null) SelectedFiles = new List<SelectedFiles>();
            else SelectedFiles.Clear();

            int i = 0, Width = 0, Height = 0;
            if (period < 1) period = 1;
            for (i = From; i < bitMapArray.Length; i = i + period)
            {
                Mat mat = BitmapConverter.ToMat(bitMapArray[i]);
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

            //Если нет последнего файла, добавляем его
            //if (period > 1 && (To == 100 || To == fileList.Length) && SelectedFiles[SelectedFiles.Count - 1].FullName != fileList[fileList.Length - 1].FullName)
            //    SelectedFiles.Add(new SelectedFiles() { Id = i, FullName = fileList[fileList.Length - 1].FullName });
        }
        public StitchingBlock(AssemblyPlan assemblyPlan):this(assemblyPlan.StitchingDirectory, assemblyPlan.AdditionalFilter, assemblyPlan.Percent, assemblyPlan.From, assemblyPlan.To, assemblyPlan.Period, assemblyPlan.SelectSearchArea, assemblyPlan.MinHeight, assemblyPlan.MaxHeight, assemblyPlan.MinWight, assemblyPlan.MaxWight){ }
        public StitchingBlock(AssemblyPlan assemblyPlan, bool newEngin) : this(assemblyPlan.StitchingDirectory, assemblyPlan.AdditionalFilter, assemblyPlan.Percent, assemblyPlan.From, assemblyPlan.To, assemblyPlan.Period, assemblyPlan.SelectSearchArea, assemblyPlan.MinHeight, assemblyPlan.MaxHeight, assemblyPlan.MinWight, assemblyPlan.MaxWight) { NewEngin = newEngin; }
        public StitchingBlock(string file, bool additionalFilter,bool percent = true, int from = 0, int to = 100, int period = 1, bool selectSearchArea = false,float minHeight =0,float maxHeight = 0,float minWight=0,float maxWight=0)
        {

            if (string.IsNullOrEmpty(file))
            {
                SetErr("Err StitchingBlock.string.IsNullOrEmpty(file)!!!");
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
            if (period < 1) period = 1;
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

        public void OnChangedBitmapImg(object i)
        {
            if (ChangBitmapImg != null) ChangBitmapImg((Bitmap)i);
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

        private bool СuttingOnChangDirection { get; set; } = false;
        private bool ResearchIsOn { get; set; } = true;






        //// Timing
        List<Timing> TimeList = new List<Timing>();
        private class Timing
        {
            public TimeSpan FSt { get; set; }
            public TimeSpan SSt { get; set; }
            public TimeSpan SSt01 { get; set; }
            public TimeSpan SSt02 { get; set; }
            public TimeSpan SSt03 { get; set; }
            public TimeSpan DSt {get;set;}
            public TimeSpan Sum {get;set;}
        }


        /// <summary>
        /// Поиск ключевых точек на кадрах
        /// </summary>
        /// <param name="param">Параметр синхронизации</param>
        public void FindKeyPoints(object param)
        {
            //// Timing
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            StopProcess = false;
            EraseError();
            List<EnumDirection?> directionsList = new List<EnumDirection?>();
            SynchronizationContext context = (SynchronizationContext)param;
            bool contextIsOn = context == null? false:true;
            SelectedFiles firstSelectedFiles = new SelectedFiles(), secondSelectedFiles = new SelectedFiles();
            int copy = -1, shiftErr = -1, shiftErrNumber = 0, notMatchDirectionsN = 0;
            bool minShiftDetectionIsON = false;

            //// Timing
            TimeSpan ts = stopwatch.Elapsed;
            TimeSpan tSum = TimeSpan.Zero;
            Timing timing = new Timing();

            for (int i = 0; i < SelectedFiles.Count; i++)
            {
                ////// Timing
                //TimeSpan ts = stopwatch.Elapsed;
                //TimeSpan tSum = TimeSpan.Zero;
                //Timing timing = new Timing();

                if (StopProcess)
                {
                    if (contextIsOn) context.Send(OnTextChanged, "Stop Process");
                    SetErr("Поиск ключевых точек приостановлен пользователем!");
                    return;
                }

                if (i == 0) continue;
                if (copy == -1  && shiftErr == -1) firstSelectedFiles = SelectedFiles[i - 1];
                secondSelectedFiles = SelectedFiles[i];

                firstSelectedFiles.StitchingFile = secondSelectedFiles.FullName;
                if (WorkingWBitmap) firstSelectedFiles.Hint = "Stitching Imgs " + firstSelectedFiles.Id + " - " + secondSelectedFiles.Id;
                else firstSelectedFiles.Hint = "Stitching Imgs " + Path.GetFileNameWithoutExtension(firstSelectedFiles.FullName) + " - " + Path.GetFileName(secondSelectedFiles.FullName);
                IsErr = false;



                ////// Timing
                //ts = stopwatch.Elapsed;
                //tSum += ts;
                //timing.FSt = ts;
                //stopwatch.Restart();
                 


                // Получаем список векторов по ключевым точкам
                List<Vector> VectorList = WorkingWBitmap ? GetVectorList(firstSelectedFiles.Mat, secondSelectedFiles.Mat) : GetVectorList(firstSelectedFiles.FullName, secondSelectedFiles.FullName);



                ////// Timing
                //ts = stopwatch.Elapsed;
                //tSum += ts;
                //timing.SSt = ts;
                //stopwatch.Restart();




                if (IsErr || VectorList.Count == 0)
                {
                    copy = i - 1;
                    secondSelectedFiles.IsErr = true;
                    secondSelectedFiles.ErrCode = ErrCode;
                    if (ErrCode == EnumErrCode.Copy)
                    {
                        secondSelectedFiles.ErrText = "COPY " + firstSelectedFiles.FullName + " - " + secondSelectedFiles.FullName;
                    }
                    else if (!string.IsNullOrEmpty(ErrText)) secondSelectedFiles.ErrText = ErrText;
                    else secondSelectedFiles.ErrText = "Err Подходящие точки не найдены!!!";
                }
                else
                {
                    VectorInfo vectorInfo = GetAverages(VectorList);
                    firstSelectedFiles.VectorList = VectorList;
                    firstSelectedFiles.AverageXShift = vectorInfo.AverageXShift;
                    firstSelectedFiles.AverageYShift = vectorInfo.AverageYShift;
                    firstSelectedFiles.AverageShift = Math.Abs(vectorInfo.AverageYShift) < Math.Abs(vectorInfo.AverageXShift) ? vectorInfo.AverageXShift : vectorInfo.AverageYShift;
                    if (firstSelectedFiles.AverageShift < minShift && minShiftDetectionIsON)
                    {
                        shiftErr = i;
                        shiftErrNumber++;
                        secondSelectedFiles.IsErr = true;
                        secondSelectedFiles.ErrCode = EnumErrCode.ShiftThreshold;
                        secondSelectedFiles.ErrText = "Err Shift " + firstSelectedFiles.FullName + " - " + secondSelectedFiles.FullName;
                    }
                    else
                    {
                        shiftErrNumber = 0;
                        shiftErr = -1;
                    }
                    firstSelectedFiles.Direction = vectorInfo.Direction;
                    if (copy != -1) copy = -1;

                    directionsList.Add(vectorInfo.Direction);
                    if (directionsList.Count>10 && СuttingOnChangDirection) // Проверка на изменение направления движения
                    {
                        var avDirection = GetAverageDirection(directionsList);
                        if (avDirection != vectorInfo.Direction && copy == -1)
                        {
                            secondSelectedFiles.IsErr = true;
                            secondSelectedFiles.ErrCode = EnumErrCode.WrongDirection;
                            secondSelectedFiles.ErrText = "Err Wrong Direction " + firstSelectedFiles.FullName + " - " + secondSelectedFiles.FullName;
                            notMatchDirectionsN++;

                            if(SelectedFiles[i-1].ErrCode == EnumErrCode.WrongDirection || SelectedFiles[i - 2].ErrCode == EnumErrCode.WrongDirection)
                            {
                                if (SelectedFiles[i - 2].ErrCode == EnumErrCode.WrongDirection)
                                {
                                    SelectedFiles = SelectedFiles.Take(i - 3).ToList();
                                }
                                else if (SelectedFiles[i - 1].ErrCode == EnumErrCode.WrongDirection )
                                {
                                    SelectedFiles = SelectedFiles.Take(i-2).ToList();
                                }
                                break;
                            }
                        }
                        else notMatchDirectionsN = 0;
                    }
                }
                if (contextIsOn && i%2 == 0)
                {
                    context.Send(OnProgressChanged, i * 100 / SelectedFiles.Count);
                    context.Send(OnTextChanged, "Finding Key Points " + i * 100 / SelectedFiles.Count + " %");
                }

                ////// Timing
                //ts = stopwatch.Elapsed;
                //tSum += ts;
                //timing.DSt = ts;
                //timing.Sum = tSum;
                //stopwatch.Restart();
                //TimeList.Add(timing);
            }

            //// Timing
            ts = stopwatch.Elapsed;

            Direction = GetAverageDirection(SelectedFiles.Where(x=>x.ErrCode!=EnumErrCode.Copy).Select(x => x.Direction).ToList());
            if (contextIsOn)
            {
                context.Send(OnProgressChanged, 100);
                context.Send(OnTextChanged, "100 %");
            }

            if(ResearchIsOn)StartResearch();
        }

        private void StartResearch()
        {
            ResearchLine  researchLine = new ResearchLine(SelectedFiles, Direction);
            bool research = researchLine.StartResearch();
            bool SlowMotion = researchLine.SlowMotion;
            bool ReverseMotion = researchLine.TrainStop;
            bool TrainStop = researchLine.ReverseMotion;
        }

        public class ResearchPoint
        {
            public double AverageShift { get; set; }
            public double LineAveraging { get; set; } = -1;
            public double Divergence { get; set; } = -1;
            public double AreaDivergence { get; set; } = -1;
            public double AreaDirection { get; set; } = -1;
            public double CoDirection { get; set; } = -1;
            public EnumDirection? Direction { get; set; }
            public bool SlowM { get; set; } = false;
            public bool Stop { get; set; } = false;
            public bool Revers { get; set; } = false;
            public double ReversPercent { get; set; } = -1;
            public double StopByShift { get; set; } = -1;
            public string FullName { get; set; }
            public string StatusInformation { get; set; }
        }

        public class ResearchLine
        {
            public bool IgnoreCopies { get; set; } = true; // Не учитывать копии кадров
            public EnumDirection? MainDirection { get; set; }
            private int minShift { get; set; } = 10; // минимально смещение картинок которое допускается из за погрешности определения ключевых точек
            private int PercentUnderMinShift { get; set; } = -1;
            public List<SelectedFiles> SelectedFiles { get; set; }
            public List<ResearchPoint> ResearchList { get; set; }
            public bool SlowMotion { get; set; } = false;
            public bool ReverseMotion { get; set; } = false;
            public bool TrainStop { get; set; } = false;
            public bool AreaForReExamination { get; set; } = false;
            public List<StopPoint> StopPoints { get; set; }
            public string Messag { get; set; }

            public ResearchLine(List<SelectedFiles> selectedFiles, EnumDirection? mainDirection, int minShift = 10)
            {
                MainDirection = mainDirection;
                this.minShift = minShift;
                SelectedFiles = selectedFiles;
                if (selectedFiles == null) return;
                if (selectedFiles.Count == 0) return;
                ResearchList = (from x in selectedFiles
                                select new ResearchPoint()
                                {FullName= x.FullName, AverageShift = x.AverageShift, Direction = x.Direction }).ToList();}
            public bool StartResearch()
            {
                if (ResearchList == null) return false;
                if (ResearchList.Count == 0) return false;

                PercentUnderMinShift = CheckForDecelerationAndStop(SelectedFiles);

                //List<double> doublList = ResearchList.Select(x=>x.AverageShift).ToList();
                SpeedСounter speedСounter = new SpeedСounter(SelectedFiles, 1, 1);
                var resultList = speedСounter.LineAveraging(ResearchList.Select(x => x.AverageShift).ToList());
                if (resultList.Count == ResearchList.Count)
                {
                    for (int i = 0; i < ResearchList.Count; i++)
                    {
                        ResearchList[i].LineAveraging = resultList[i];
                        ResearchList[i].Divergence =Math.Abs(ResearchList[i].AverageShift - resultList[i]);
                        if (IgnoreCopies && ResearchList[i].AverageShift == 0) ResearchList[i].AreaDivergence = -1;
                        else ResearchList[i].AreaDivergence = Math.Abs(ResearchList[i].AverageShift - resultList[i]) / resultList[i];
                    }
                }
                if (MainDirection == null) return false;
                GetDirectionLines();
                DetectAreas();
                return true;
            }

            private int CheckForDecelerationAndStop(List<SelectedFiles> SelectedFilesTempList) // проверка на замедление \ остановку поезда
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
            
            private bool GetDirectionLines()
            {
                int N = 10;
                if (ResearchList == null) return false;
                if (ResearchList.Count == 0) return false;
                //if (N > ResearchList.Count - 1) N = ResearchList.Count - 1;
                //if (N < 2) N = 2;
                //bool isEven = (N % 2) == 0 ? true : false;
               // List<double> result = new List<double>();
                Queue<EnumDirection?> queue = new Queue<EnumDirection?>();
                int n = -1,i=0;
                bool deque = false;

                while (true)
                {
                    //if (++n > ResearchList.Count - 1)
                    //{
                    //    break;
                    //}
                    //if (IgnoreCopies && ResearchList[n].AverageShift == 0) continue;
                    if (++n < ResearchList.Count && ResearchList[n].Direction!= null && ResearchList[n].Direction!= EnumDirection.Undefined) 
                        queue.Enqueue(ResearchList[n].Direction);
                    else if(n >= ResearchList.Count)queue.Dequeue();
                    
                    if (queue.Count > N )queue.Dequeue(); 
                    if (queue.Count == 0 || i >= ResearchList.Count)break;
                    if (queue.Count >= N / 2)
                    {
                        if (IgnoreCopies && ResearchList[i].AverageShift == 0)
                        {
                            ResearchList[i].CoDirection = -1;
                            ResearchList[i].AreaDirection = -1;
                        }
                        else
                        {
                            ResearchList[i].CoDirection = CountCoDirection(queue);
                            ResearchList[i].AreaDirection = CountAreaDirection(queue);
                        }
                        i++;
                    }
                }
                return true;
            }
            public class StopPoint
            {
                public string FullName { get; set; }
                public double averagDirection { get; set; }
            }
            private bool DetectAreas()
            {
                if(ResearchList.Count == 0) return false;

                for (int i = 0; i< ResearchList.Count; i++)
                {
                    if(IgnoreCopies && ResearchList[i].AverageShift == 0)
                    {
                        ResearchList[i].Revers=false;
                        ResearchList[i].Stop = false;
                        ResearchList[i].SlowM = false;
                        continue; 
                    }

                    bool Revers = (ResearchList[i].CoDirection < 0.3 && ResearchList[i].CoDirection!=-1) ? true : false,
                        Stop = (ResearchList[i].CoDirection > 0.3 && ResearchList[i].CoDirection < 0.8 && ResearchList[i].CoDirection != -1) ? true : false,
                        SlowM = ResearchList[i].LineAveraging < minShift * 1.5 ? true: false;

                    ResearchList[i].Revers = Revers;
                    ResearchList[i].Stop = Stop;
                    ResearchList[i].SlowM = SlowM;
                }

                var sgsdf = ResearchList.Where(x => x.SlowM).Count();
                var sgsdf2 = ResearchList.Where(x => x.SlowM || x.Stop).Count();

                PercentUnderMinShift = ResearchList.Where(x => x.SlowM).Count() * 100 / ResearchList.Count();
                var Dublicate = ResearchList.Where(x => x.SlowM || x.Stop).ToList();
                var persentForDublicate = ResearchList.Where(x => x.SlowM || x.Stop).Count() * 100 / ResearchList.Count();
                //CheckForDecelerationAndStop(SelectedFiles);

                var chanceOfRevers = ResearchList.Any(x => x.Revers);
                var dsf2 = ResearchList.Count(x => x.Revers);

                var st = ResearchList.Any(x => x.Stop);
                var dsf4 = ResearchList.Count(x => x.Stop);

                var slm = ResearchList.Any(x => x.SlowM );
                var dsf6 = ResearchList.Count(x => x.Stop);

                var zeroShift = ResearchList.Any(x => x.AverageShift == 0);
                var zsh = ResearchList.Count(x => x.AverageShift == 0);
                
                TrainStop = st;
                SlowMotion = slm;

                int N = 15;
                bool stopByShift = false;
                Queue<double> queue = new Queue<double>();
                if (zsh>5)
                {
                    for (int i = 0; i < ResearchList.Count; i++)
                    {
                        //if (ResearchList[i].CoDirection == -1) continue;
                        queue.Enqueue(ResearchList[i].AverageShift);
                        if (queue.Count > N) queue.Dequeue();
                        if (queue.Count >= N / 2)
                        {
                            ResearchList[i].StopByShift = queue.Sum() / queue.Count;
                            //var sdf = ResearchList[i].StopByShift - 0.5;
                            //tmps.Add(new StopPoint() { FullName = ResearchList[i].FullName, averagDirection = (ResearchList[i].ReversPercent - 0.5) });
                            if (queue.Count >= N - 1 && ResearchList[i].StopByShift < 0.1) stopByShift = true;
                        }
                    }
                }

                bool isRevers = false;
                queue.Clear();
                // List<StopPoint> tmps = new List<StopPoint>();
                if (chanceOfRevers)
                {
                    for (int i = 0; i < ResearchList.Count; i++)
                    {
                        if (ResearchList[i].CoDirection == -1)continue;
                        queue.Enqueue(ResearchList[i].CoDirection);
                        if(queue.Count>N)queue.Dequeue();
                        if (queue.Count >= N/2)
                        { 
                            ResearchList[i].ReversPercent = queue.Sum() / queue.Count - 0.5;
                            if (!isRevers && queue.Count >= N - 1 && ResearchList[i].ReversPercent < -0.3) isRevers = true;
                            //tmps.Add(new StopPoint() { FullName = ResearchList[i].FullName, averagDirection = (ResearchList[i].ReversPercent - 0.5) });
                            //if (!isRevers && queue.Count>=N-1 && ResearchList[i].ReversPercent < 0.2) isRevers = true;
                        }
                    }
                }

                ReverseMotion = isRevers;
                if (ReverseMotion)
                {
                    StopPoints = FindCrossZero();
                    //if (StopPoints.Count > 1) TrainStop = true;
                    //else if (StopPoints.Count == 1) TrainStop = true;
                    //else TrainStop = false;
                    TrainStop = true;
                    SlowMotion = true;
                    Messag = "Если есть реверс значит должны быть Stop и SlowM";
                }
                else if (!SlowMotion && TrainStop) Messag = "Скорее всего это не остановка поезда, а просто он закончился!";
                else if (!TrainStop && !SlowMotion && !TrainStop) Messag = "Все норм, обычный поезд!";

                for (int i = 0; i < ResearchList.Count; i++)
                {
                    if (ResearchList[i].Stop && ResearchList[i].SlowM) ResearchList[i].StatusInformation = "Замедленное движение или остановка";
                    else if (ResearchList[i].SlowM ) ResearchList[i].StatusInformation = "Замедленное движение";
                    else if (ResearchList[i].Stop) ResearchList[i].StatusInformation = "Предположительное место остановки";
                    if (ResearchList[i].StopByShift < 0.1 && ResearchList[i].StopByShift != -1) ResearchList[i].StatusInformation = "Остановка или какмера глючит(создает множество копий)";
                    if (ResearchList[i].Revers) ResearchList[i].StatusInformation = "Движение в обратном направлении " + GetOpositDirection(MainDirection);
                    if(!ResearchList[i].Stop && !ResearchList[i].SlowM && !ResearchList[i].Revers && (ResearchList[i].StopByShift > 0.1 || ResearchList[i].StopByShift == -1)) ResearchList[i].StatusInformation = "Движение по основному направлению " + MainDirection;
                }
                // List<StopPoint> stopPoints2 = FindCrossZero(tmps);
                return true;
            }
            private List<StopPoint> FindCrossZero()  // Получаем точки в которых происходит пересечение нуля
            {
                if(ResearchList.Count < 10) return new List<StopPoint>();
                List<StopPoint> rezultList = new List<StopPoint>();
                for (int i = 0; i < ResearchList.Count; i++)
                {
                    if (i < 2 || i > ResearchList.Count() - 3) continue;
                    if ((ResearchList[i - 2].ReversPercent < 0 && ResearchList[i - 1].ReversPercent < 0 && ResearchList[i].ReversPercent >= 0 && ResearchList[i + 1].ReversPercent > 0) ||
                        (ResearchList[i - 2].ReversPercent > 0 && ResearchList[i - 1].ReversPercent > 0 && ResearchList[i].ReversPercent <= 0 && ResearchList[i + 1].ReversPercent < 0))
                        rezultList.Add(new StopPoint() { FullName = ResearchList[i].FullName, averagDirection = ResearchList[i].ReversPercent});   
                }

                return rezultList;
            }
            private double CountAreaDirection(Queue<EnumDirection?> queue)
            {
                if (queue == null) return 0;
                if (queue.Count() == 0) return 0;
                var existDirections = queue.Distinct().ToArray();// Получаем все существующие направления движения
                if (existDirections.Length == 1) return 1;

                List<DirectionCheck> DirectionCheckList = new List<DirectionCheck>();
                foreach (EnumDirection? direction in existDirections)
                {
                    var amaunt = queue.Count(x => x == direction);
                    DirectionCheckList.Add(new DirectionCheck
                    {
                        EnumDirection = direction,
                        Amount = amaunt
                    });
                }
                var mainDirection =  DirectionCheckList.Where(x => x.Amount == DirectionCheckList.Max(y => y.Amount)).Select(z => z.EnumDirection).FirstOrDefault();

                return CountCoDirection(queue, mainDirection);
            }
            private double CountCoDirection(Queue<EnumDirection?> queue , EnumDirection? mainDirection = null )
            {
                if (mainDirection == null) mainDirection = MainDirection;
                double Sum = 0;
                foreach (EnumDirection? elem in queue)
                {
                    if (elem == null || OpositDirection(elem, mainDirection)) continue;
                    else if (elem == mainDirection) Sum++;
                    else Sum += 0.5;
                }

                return Sum / queue.Count;
            }
            private bool OpositDirection(EnumDirection? dir, EnumDirection? mainDirection= null )
            {
                if (dir == null) return false;
                if (mainDirection == null) mainDirection = MainDirection;
                if ((MainDirection == EnumDirection.Left && dir == EnumDirection.Right) ||
                    (MainDirection == EnumDirection.Right && dir == EnumDirection.Left) ||
                    (MainDirection == EnumDirection.Up && dir == EnumDirection.Down) ||
                    (MainDirection == EnumDirection.Down && dir == EnumDirection.Up)) return true;

                return false;
            }
            private EnumDirection? GetOpositDirection(EnumDirection? mainDirection)
            {
                if (mainDirection == null  || mainDirection == EnumDirection.Undefined) return mainDirection;
                if (MainDirection == EnumDirection.Left) return EnumDirection.Right;
                if (MainDirection == EnumDirection.Right) return EnumDirection.Left;
                if (MainDirection == EnumDirection.Up) return EnumDirection.Down;
                if (MainDirection == EnumDirection.Down) return EnumDirection.Up;
                return EnumDirection.Undefined;
            }
        }
        public List<AreaForDel> FindeBlockForDelet()
        {
            List<AreaForDel> areasForDelet = new List<AreaForDel>();
            if (SelectedFiles == null || SelectedFiles.Count < 15 || Direction == EnumDirection.Undefined) return areasForDelet;

            int[] Errors = FindErrs();
            var Area = HaosMeasur(Errors);
            int maxErrLvl = 4;
            areasForDelet = GetAreasForDel(Area, maxErrLvl);
            return JoinAreas(areasForDelet, SelectedFiles.Count);
        }
        public void DeletAreas(List<StitchingBlock.AreaForDel> areasForDelet)
        {
            if (areasForDelet.Count == 0 || SelectedFiles == null || SelectedFiles.Count == 0) return;
            for (int i = areasForDelet.Count - 1; i > -1; i--) SelectedFiles.RemoveRange(areasForDelet[i].From, areasForDelet[i].To - areasForDelet[i].From);
        }
        private int[] FindErrs(bool copyСounting = false )
        {
            int[] Errors = new int[SelectedFiles.Count];
            bool prevCopy = false;
            //double N = 0;
            List<tmp> tmpList = new List<tmp>();
            for (int i = 0; i < SelectedFiles.Count; i++)
            {
                tmp Tmp = new tmp(i);
                if (i == SelectedFiles.Count - 1)
                {
                    tmpList.Add(Tmp);
                    continue;
                }
                if (SelectedFiles[i].Direction != Direction && SelectedFiles[i].ErrCode != EnumErrCode.Copy && SelectedFiles[i].ErrCode != EnumErrCode.ShiftThreshold)
                {
                    Errors[i]++;
                    Tmp = new tmp(i, "Direction");
                }
                if (prevCopy && SelectedFiles[i].ErrCode == EnumErrCode.Copy && copyСounting)
                {
                    Errors[i]++;
                    Tmp.Add("DoublCopy");
                }
                else if (SelectedFiles[i].ErrCode == EnumErrCode.Copy) prevCopy = true;
                else prevCopy = false;

                if (SelectedFiles[i].IsErr && SelectedFiles[i].ErrCode != EnumErrCode.Copy && SelectedFiles[i].ErrCode != EnumErrCode.ShiftThreshold)
                {
                    Errors[i]++;
                    Tmp.Add("IsErr");
                }
                //if (SelectedFiles[i].AverageShift < 10 && SelectedFiles[i].ErrCode != EnumErrCode.Copy)
                //{
                //    Errors[i]++;
                //    Tmp.Add("AverageShift less errShift");
                //}

                //if (Errors[i] > 0) N++;
                tmpList.Add(Tmp);
            }
            return Errors;
        }
        public class tmp
        {
            public int I { get; set; }
            public int Err { get; set; }
            public List<string> ErrText { get; set; }
            public string ErrLine { get; set; }
            public tmp(int i)
            {
                I = i;
            }
            public tmp(int i, string errText)
            {
                I = i;
                Err = 1;
                ErrText = new List<string>() { errText };
                ErrLine = errText;
            }
            public void Add(string errText, int i = -1)
            {
                if (i != -1) I = i;
                Err++;
                if (ErrText == null) ErrText = new List<string> { errText };
                else ErrText.Add(errText);
                if (string.IsNullOrEmpty(ErrLine)) ErrLine = errText;
                else ErrLine += " " + errText;
            }
        }
        private List<AreaForDel> GetAreasForDel(int[] Area, int maxErrLvl)
        {
            List<AreaForDel> areasForDelet = new List<AreaForDel>();
            int fr = 0;
            bool isin = false;
            for (int i = 0; i < Area.Length; i++)
            {
                if (Area[i] >= maxErrLvl && !isin)
                {
                    fr = i - maxErrLvl;
                    if (fr < 0) fr = 0;
                    isin = true;
                }

                if (isin && Area[i] < maxErrLvl)
                {
                    isin = false;
                    int to = To = i + maxErrLvl;
                    if (to > Area.Length) to = Area.Length;
                    areasForDelet.Add(new AreaForDel(From = fr, To = to ));
                }

                if (isin && i == Area.Length - 1) areasForDelet.Add(new AreaForDel( From = fr, To = Area.Length));
            }
            return areasForDelet;
        }
        private List<AreaForDel> JoinAreas(List<AreaForDel> areasForDelet, int ArrLeng)
        {
            if (areasForDelet.Count < 2 || ArrLeng==0) return areasForDelet;
            int minimalGap = ArrLeng / 100 >15 ? 15: ArrLeng / 100;
            List<AreaForDel> newListAreaForDels = new List<AreaForDel>();
            AreaForDel newArea = new AreaForDel(-1,-1);
            for (int i = 1; i < areasForDelet.Count; i++)
            {
                if(newArea.From != -1)
                {
                    if (newArea.To > areasForDelet[i].From - minimalGap) newArea.To = areasForDelet[i].To;
                    else
                    {
                        newListAreaForDels.Add(newArea);
                        newArea = new AreaForDel(-1, -1);
                    }
                }
                else if (areasForDelet[i - 1].To > areasForDelet[i].From)
                {
                    newArea = new AreaForDel( areasForDelet[i - 1].From, areasForDelet[i].To );
                }
                else newListAreaForDels.Add(new AreaForDel(areasForDelet[i - 1].From, areasForDelet[i-1].To));
                if(i== areasForDelet.Count - 1 && newArea.From != -1) newListAreaForDels.Add(newArea);
            }

            return newListAreaForDels;
        }
        public class AreaForDel
        {
            public int From {get;set;} 
            public int To {get;set;} 
            public AreaForDel (int from, int to) {From = from; To = to;}
        }
        private int[] HaosMeasur(int[] errors)
        {
            int N = 10;
            if (N > errors.Length - 1) N = errors.Length - 1;
            if (N < 2) N = 2;
            List<int> result = new List<int>();
            Queue<int> queue = new Queue<int>();
            int n = 0;

            for (int i = 0; i < errors.Length; i++)
            {
                if (n < errors.Length) queue.Enqueue(errors[n++]);
                if (queue.Count > N )queue.Dequeue();
                result.Add(queue.Sum());
            }

            return result.ToArray();
        }

        /// <summary>
        /// Получаем список векторов смещения одной картинки относительно второй
        /// Текстовая версия т.к. есть проблемы у перегрузки
        /// </summary>
        /// <param name="file1">Картинка №1</param>
        /// <param name="file2">Картинка №2</param>
        /// <param name="makeFotoRezult">Определяет нужно ли создавать изображение с найденными точками</param>
        /// <returns></returns>
        public List<Vector> GetVectorListStringVersion(string file1, string file2, bool makeFotoRezult = false) => GetVectorList(new Mat(file1), new Mat(file2), makeFotoRezult);
        /// <summary>
        /// Получаем список векторов смещения одной картинки относительно второй
        /// </summary>
        /// <param name="file1">Картинка №1</param>
        /// <param name="file2">Картинка №2</param>
        /// <param name="makeFotoRezult">Определяет нужно ли создавать изображение с найденными точками</param>
        /// <returns></returns>
        public List<Vector> GetVectorList(string file1, string file2, bool makeFotoRezult = false) => GetVectorList(new Mat(file1), new Mat(file2), makeFotoRezult);

        public List<int> PressisionList =  new List<int>();

        /// <summary>
        /// Получаем список векторов смещения одной картинки относительно второй
        /// </summary>
        /// <param name="file1">Картинка №1</param>
        /// <param name="file2">Картинка №2</param>
        /// <param name="makeFotoRezult">Определяет нужно ли создавать изображение с найденными точками</param>
        /// <returns></returns>
        public List<Vector> GetVectorList(Mat matSrc, Mat matTo, bool makeFotoRezult = false)
        {
            // Timing
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            TimeSpan ts = stopwatch.Elapsed;
            TimeSpan tSum = TimeSpan.Zero;
            Timing timing = new Timing();

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

                    if (matches[0].Length == 2)
                    {
                        goodPointList = GetVectors(matches, keyPointsSrc, keyPointsTo, out goodMatches);
                        PressisionList.Add(Prezision);
                    }
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

                if (makeFotoRezult)// Если нужно создаем изображение с найденными точками
                {
                    VectorInfo vectorInfo = GetAverages(goodPointList);
                    goodMatches = goodMatches.Where(x => goodPointList.Any(y => y.MatchesId == x.QueryIdx)).ToList();

                    ts = stopwatch.Elapsed;
                    string text = goodPointList.Count + " points found " + (Math.Abs(vectorInfo.AverageYShift) < Math.Abs(vectorInfo.AverageXShift) ? Math.Round(vectorInfo.AverageXShift, 2) : Math.Round(vectorInfo.AverageYShift, 2)).ToString() +
                          " " + vectorInfo.Direction + " Shift Time " + String.Format("{0:00}:{1:00}", ts.Seconds, ts.Milliseconds / 10) + "\n";
                    int i = 0;
                    foreach (Vector point in goodPointList)text += "P " + i++ + " Sh " + Math.Round(point.Delta, 2) + " " + point.Direction + " Shift " + point.Delta +  " Identity " + Math.Round(point.Identity, 2) + " CoDirection " + Math.Round(point.CoDirection, 2) + " dX " + Math.Round(point.dX, 2) + " dY " + Math.Round(point.Yfr, 2) + "\n";

                    OnTextChanged(text);
                    Mat RezultImg = new Mat();
                    Cv2.DrawMatches(matSrc, keyPointsSrc, matTo, keyPointsTo, goodMatches, RezultImg, flags: drawMatchesFlags);
                    OnChangedBitmapImg(BitmapConverter.ToBitmap(RezultImg));
                }

                // Timing
                //ts = stopwatch.Elapsed;
                //tSum += ts;
                //timing.SSt03 = ts;
                //timing.Sum = tSum;
                //stopwatch.Restart();
                //TimeList.Add(timing);
            }

            return goodPointList;
        }

        /// <summary>
        /// Получение изображения пар ключевых точек с помощью ORB
        /// </summary>
        /// <param name="FilePath1"></param>
        /// <param name="FilePath2"></param>
        /// <returns></returns>
        //public Bitmap RunORB(string FilePath1, string FilePath2)
        //{
        //    var img1 = new Mat(FilePath1);
        //    var img2 = new Mat(FilePath2);
        //    using (var orb = ORB.Create(1500))
        //    using (var descriptor1 = new Mat())
        //    using (var descriptor2 = new Mat())
        //    {
        //        KeyPoint[] keyPoints1, keyPoints2;
        //        orb.DetectAndCompute(img1, null, out keyPoints1, descriptor1);
        //        orb.DetectAndCompute(img2, null, out keyPoints2, descriptor2);

        //        // Flann needs the descriptors to be of type CV_32F
        //        descriptor1.ConvertTo(descriptor1, MatType.CV_32F);
        //        descriptor2.ConvertTo(descriptor2, MatType.CV_32F);

        //        var matcher = new FlannBasedMatcher();
        //        DMatch[] matches = matcher.Match(descriptor1, descriptor2);

        //        using (Mat view = new Mat())
        //        //  using (var window = new Window())
        //        {
        //            Cv2.DrawMatches(img1, keyPoints1, img2, keyPoints2, matches, view);
        //            //window.ShowImage(view);
        //            Bitmap rezult = BitmapConverter.ToBitmap(view);
        //            Cv2.WaitKey();
        //            return rezult;
        //        }
        //    }
        //}

        internal Mat Stitch(object param, int Delta = 0)// Сборка кадров в один
        {
            StopProcess = false;
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

            // Проверка границ
            //int newDelta = CheckBorders(Delta);
            //if (newDelta != Delta)
            //{
            //    SetErr($"\nDelta пришлось поменять с {Delta} на {newDelta}");
            //    Delta = newDelta;
            //}

            // Сборка картинки по частям
            for (int i = 0; i < SelectedFiles.Count - 1; i++)
            {
                if (StopProcess)
                {
                    SetErr("Сборка кадров приостановлена пользователем!");
                    return rezult;
                }
                if (SelectedFiles[i].IsErr) continue;
                if (context != null) context.Send(OnProgressChanged, i * 100 / SelectedFiles.Count);
                if (context != null) context.Send(OnTextChanged, "Frame Union " + i * 100 / SelectedFiles.Count + " %");

                if (SelectedFiles.Count == 2) // Вариант на случай если нужно собрать только 2 картинки
                {
                    int shift =(int)Math.Round(SelectedFiles[0].AverageShift);
                    Mat Img1, Img2;

                    if (WorkingWBitmap)
                    {
                        Img1 = SelectedFiles[0].Mat;
                        Img2 = SelectedFiles[1].Mat;
                    }
                    else
                    {
                        if(string.IsNullOrEmpty(SelectedFiles[0].FullName) || string.IsNullOrEmpty(SelectedFiles[1].FullName))
                        {
                            SetErr("Err Stitch.One of SelectedFiles == 0!!!");
                            return rezult;
                        }
                        Img1 = new Mat(SelectedFiles[0].FullName);
                        Img2 = new Mat(SelectedFiles[1].FullName);
                    }

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
                    SetErr("Итоговая картинка не собрана (Width||Height == 0). Id последего кадра " + SelectedFiles[i].Id + " i - "+i+" !!!");
                    if (context != null) context.Send(OnProgressChanged, 100);
                    if (context != null) context.Send(OnTextChanged, "Err  rezultImg = 0. Ошибка на "+ SelectedFiles[i].Id+" кадре!!!");
                    return rezult;
                }
            }

            if (context != null) context.Send(OnProgressChanged, 100);
            if (context != null) context.Send(OnTextChanged, "100 %");
            AddErrStitchingInfo();
            return rezult;
        }
        private Mat JoinImg(string file1, string file2, int shift, EnumFramePosition enumFramePosition, int Delta = 0) => JoinImg(new Mat(file1), new Mat(file2), shift, enumFramePosition, Delta);
        private Mat JoinImg(Mat Img1, string file2, int shift, EnumFramePosition enumFramePosition, int Delta = 0) => JoinImg(Img1, new Mat(file2), shift, enumFramePosition, Delta);
        private Mat JoinImg(Mat Img1, Mat Img2, int shift, EnumFramePosition FramePosition, int Delta = 0)
        {
            shift = Math.Abs(shift);
            Mat rezult = new Mat();
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
                    int d2 = h2 + shift - Delta - 1;

                    if (d1 > 0 && d1 < Img2.Height - 1 && d2 > 0 && d2 < Img2.Height - 1)
                    {
                        Rect rect2 = new Rect(0, d1, Img2.Width - 1, d2);
                        Mat dstroi2 = new Mat(Img2, rect2);
                        Cv2.VConcat(Img1, dstroi2, rezult);
                    }
                    else
                    {
                        SetErr("Err JoinImg.В одном из кадров превышена граница изображения!!!");
                        return Img1;
                    }
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

                        if (Img1.Height != dstroi2.Height)
                        {
                            rect2 = new Rect(d2, 0, shift, Img2.Height);
                            dstroi2 = new Mat(Img2, rect2);
                        }

                        Cv2.HConcat(Img1, dstroi2, rezult);
                    }
                    else
                    {
                        SetErr("Err JoinImg.В одном из кадров превышена граница изображения!!!");
                        return Img1;
                    }
                }
                else if (FramePosition == EnumFramePosition.Last)
                {
                    int d1 = w2 - shift + Delta - 1;
                    int d2 = w2 + shift - Delta - 1;

                    if (d1 > 0 && d1 < Img2.Width - 1 && d2 > 0 && d2 < Img2.Width - 1)
                    {
                        Rect rect2 = new Rect(d1, 0, d2, Img2.Height - 1);
                        Mat dstroi2 = new Mat(Img2, rect2);
                        Cv2.HConcat(Img1, dstroi2, rezult);
                    }
                    else
                    {
                        SetErr("Err JoinImg.В одном из кадров превышена граница изображения!!!");
                        return Img1;
                    }
                }
            }
            return rezult;
        }
        private int CheckBorders(int Delta, bool recurs = true)
        {
            int DeltaSave = Delta;
            Mat imgFile = SelectedFiles[0].Mat;
            if(imgFile == null && !string.IsNullOrEmpty(SelectedFiles[0].FullName)) imgFile = new Mat(SelectedFiles[0].FullName);
            
            if(imgFile == null) return Delta;
            if(imgFile.Width == 0) return Delta;

            int newDelta = Delta;
            int w2 = imgFile.Width / 2;
            int lastElem = 0;
            if (Direction == EnumDirection.Left)
            {
                List<int> intsD2 = new List<int>();
                for(int i =0; i< SelectedFiles.Count; i++)
                {
                    if(i == SelectedFiles.Count-1)lastElem = w2 - (int)SelectedFiles[i].AverageShift + Delta - 1;   
                    else
                    {
                    int d2 = w2 - (int)SelectedFiles[i].AverageShift + Delta;
                    intsD2.Add(d2);
                    }
                }

                int minD2 = intsD2.Min();

                if (minD2 < 0 || lastElem < 0 || lastElem > w2)
                {
                    if (minD2 < 0) newDelta = -minD2 + 5;
                    //lastElem = w2 - (int)SelectedFiles[i].AverageShift + Delta - 1;   
                    if (lastElem < 0)
                    {
                        newDelta = -lastElem + 5;
                       // newDelta = lastElem = w2 - (int)SelectedFiles[SelectedFiles.Count - 1].AverageShift + Delta - 1;
                    }                    
                    if (lastElem > w2)
                    {
                        newDelta = - lastElem + 5;
                        
                        // newDelta = lastElem = w2 - (int)SelectedFiles[SelectedFiles.Count - 1].AverageShift + Delta - 1;
                    }

                    if (recurs)
                    {
                        int delta2 =  CheckBorders(newDelta, false);
                        if (newDelta != delta2) newDelta = DeltaSave;
                    }
                }
                //if (maxD2 > w2) newDelta = - minD2+5;
            }
            return newDelta;
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

            SelectedFiles = TempSelectedFiles;
        }

        public int Prezision = 0;


        /// <summary> Поиск соноправленных векторов по парным ключевым точкам </summary>
        /// <param name="matches">матрица парных точек</param>
        /// <param name="keyPointsSrc">Ключевые точки с первого кадра</param>
        /// <param name="keyPointsTo">Ключевые точки со второго кадра</param>
        /// <param name="goodMatches">Итоговый список векторов</param>
        /// <returns></returns>
        public List<Vector> GetVectors(DMatch[][] matches, KeyPoint[] keyPointsSrc, KeyPoint[] keyPointsTo, out List<DMatch> goodMatches)
        {
            goodMatches = new List<DMatch>();
            List<Vector> goodPointList = new List<Vector>();
            if (matches == null)
            {
                SetErr("Не найдены совпадающие ключевые точки", EnumErrCode.PointsNotFound);
                return goodPointList;
            }

            if (AllPointsChkBox)  // for all points
            {
                for (int j = 0; j < matches.Length; j++)
                {
                    var match = matches[j][0];
                    Vector vector = new Vector(match.QueryIdx, keyPointsSrc[match.QueryIdx].Pt.X, keyPointsSrc[match.QueryIdx].Pt.Y, keyPointsTo[match.TrainIdx].Pt.X, keyPointsTo[match.TrainIdx].Pt.Y);
                    goodPointList.Add(vector);
                    goodMatches.Add(matches[j][0]);
                }

                //for (int j = 0; j < matches.Length; j++)
                //{
                //    var match = matches[j][0];
                //    if (matches[j][0].Distance < 0.2 * matches[j][1].Distance)
                //    {
                //        goodPointList.Add(new Vector(match.QueryIdx, keyPointsSrc[match.QueryIdx].Pt.X, keyPointsSrc[match.QueryIdx].Pt.Y, keyPointsTo[match.TrainIdx].Pt.X, keyPointsTo[match.TrainIdx].Pt.Y));
                //        goodMatches.Add(matches[j][0]);
                //    }
                //}

                //var sdf1 = matches.Select(x => x[0]).ToList();
                //var sdf2 = matches.Select(x => x[1]).ToList();

                //var goodmatches = matches.Where(x => x[0].Distance < 0.2 * x[1].Distance).ToList();
                //var gdsf1 = goodmatches.Select(x => x[0]).ToList();
                //var gdsf2 = goodmatches.Select(x => x[1]).ToArray();



                //for (int j = 0; j < mingoodmatches.Length; j++)
                //{
                //    var match = mingoodmatches[j][0];

                //    Vector vector = new Vector(match.QueryIdx, keyPointsSrc[match.QueryIdx].Pt.X, keyPointsSrc[match.QueryIdx].Pt.Y, keyPointsTo[match.TrainIdx].Pt.X, keyPointsTo[match.TrainIdx].Pt.Y);
                //    goodPointList.Add(vector);
                //    goodMatches.Add(matches[j][0]);
                //}

                return goodPointList;
            }
            if (NewEngin)
            {
                //var mingoodmatches = matches.OrderBy(x => (x[0].Distance / x[1].Distance)).Take(30).ToArray();
                goodMatches = matches.OrderBy(x => (x[0].Distance / x[1].Distance)).Take(20).Select(x => x[0]).ToList();
                int isCopy = 0;
                int ErrorLimits = 0;

                foreach (var elem in goodMatches)
                {
                    Vector vector = new Vector(elem.QueryIdx, keyPointsSrc[elem.QueryIdx].Pt.X, keyPointsSrc[elem.QueryIdx].Pt.Y, keyPointsTo[elem.TrainIdx].Pt.X, keyPointsTo[elem.TrainIdx].Pt.Y);
                    if (vector.inErrorLimits) ErrorLimits++;
                    if (vector.isSamePoint)
                    {
                        isCopy++;
                        if (isCopy > 10)
                        {
                            goodMatches.Clear();
                            SetErr("COPY", EnumErrCode.Copy);
                            return new List<Vector>();
                        }
                        continue;
                    }

                    goodPointList.Add(vector);
                }
            }
            else
            {
                int i = 0, n = 0;
                if (SelectSearchArea && matches.Length > 30) // Отсееваем точки если выбран определенный сектор
                {
                    double minHeight = MinHeight, maxHeight = MaxHeight, delta = Math.Abs(MinHeight - MaxHeight);
                    var matches2 = matches.Where(x => keyPointsSrc[x[0].QueryIdx].Pt.Y > minHeight && keyPointsSrc[x[0].QueryIdx].Pt.Y < maxHeight && keyPointsTo[x[0].TrainIdx].Pt.Y > minHeight - 5 && keyPointsTo[x[0].TrainIdx].Pt.Y < maxHeight + 5).ToArray();

                    if (delta < 10) delta = 10;
                    minHeight -= delta / 2; maxHeight += delta / 2;
                    while (matches2.Length < 20)
                    {
                        if (n++ > 10) break;
                        if (minHeight < 0) minHeight = 0;
                        matches2 = matches.Where(x => keyPointsSrc[x[0].QueryIdx].Pt.Y > minHeight && keyPointsSrc[x[0].QueryIdx].Pt.Y < maxHeight && keyPointsTo[x[0].TrainIdx].Pt.Y > minHeight - 5 && keyPointsTo[x[0].TrainIdx].Pt.Y < maxHeight + 5).ToArray();
                        minHeight -= delta / 2; maxHeight += delta / 2;
                    }
                    if (matches2.Length >= 20) matches = matches2;
                }
                if (matches.Length == 0)
                {
                    SetErr("Не найдены подходящие точки", EnumErrCode.PointsNotFound);
                    return goodPointList;
                }

                double Precisious = 0;
                for (i = 0; i < Precision.Length; i++) // Понижая точность ищим достаточное колличество ключевых точек
                {
                    Precisious = Precision[i];
                    goodMatches = new List<DMatch>();
                    goodPointList = new List<Vector>();
                    int SamePoints = 0;
                    for (int j = 0; j < matches.Length; j++)
                    {
                        var match = matches[j][0];
                        if (matches[j][0].Distance < Precisious * matches[j][1].Distance)
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
                    if (SelectSearchArea && goodPointList.Count > MinSufficientNumberOfPoints / 2 && !AllPointsChkBox) break;
                    if (goodPointList.Count > MinSufficientNumberOfPoints && !AllPointsChkBox) break;

                    if(i==2)
                    {

                    }
                }

                Prezision = i;
            }

            PointFiltr pointFiltr = new PointFiltr(goodPointList);
            if (!AdditionalFilter)
            {
                goodPointList = pointFiltr.PointFiltering();
                if (goodPointList.Count == 0)
                {
                    SetErr(pointFiltr.ErrText);
                    return goodPointList;
                }
            }

            if (goodPointList.Count >= 10) goodPointList = pointFiltr.AdditionalFilter();
            return goodPointList;
        }

        private EnumDirection? GetAverageDirection(List<EnumDirection?> directionList)// Определяем среднее направления движения
        {
            if (directionList == null) return null;
            directionList = directionList.Where(x => x!=null).ToList();
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

        /// <summary>Пробуем прочитать старый план сборки если он есть</summary>
        /// <param name="from">Кадр с которого начать запись</param>
        /// <param name="to">последний кадр для записи</param>
        public bool TryReadMapPlan(int from = 0, int to = 100)
        {
            try
            {
                fileEdit.LoadeJson(GetPlanName(), out StitchingPlan stitchingPlan);
                if (stitchingPlan == null) return false;

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
            catch (IOException e)
            {
                SetErr("The file could not be read: " + e.Message + "!!!"); 
                return false;
            }
        }

        /// <summary>Сохраняем план сборки</summary>
        public void TrySaveMapPlan()
        {
            if (!Directory.Exists(MainDir)) return;
            StitchingPlan stitchingPlan = new StitchingPlan(MainDir, SelectedFiles, Direction);
            string saveFile = GetPlanName();
            fileEdit.SaveJson(saveFile, stitchingPlan, true);
        }
        /// <summary>Удаление плана сборки</summary>
        public bool DeletPlan()
        {
            string plan = GetPlanName();
            if (string.IsNullOrEmpty(plan)) return false;
            File.Delete(plan);
            if (!CheckPlan()) return true;
            else return false;
        }
        /// <summary>Проверка плана сборки</summary>
        public bool CheckPlan()
        {
            if (File.Exists(GetPlanName())) return true;
            else return false;
        }
        private string GetPlanName() // Получить имя файла для сохранения плана
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

        #region недоделоанное - проверка на ошибки, замедление, остановку ...
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
            if (CheckForDecelerationAndStop(SelectedFilesTempList) > 50) // количество точек в процентах находящихся ниже минимальной планки погрешности
            {
                //var avSift = SelectedFiles.Select(x => x.AverageShift).ToList();
                //for (int i = 0; i<4; i++)
                //{
                //    if(i==0) MidlAverageShift = LineAveraging(avSift);
                //    else MidlAverageShift = LineAveraging(MidlAverageShift);
                //}
            }
            if (MidlAverageShift.Count > 0)
            {
                // проверка на пересечение с 0 или пробуем определить приблизительный номер кадра остановки поезда
                int nOfFirstPoint = 0;
                int CrossZero = CheckForCrossZero(MidlAverageShift, out nOfFirstPoint);
                if (CrossZero > 0 && nOfFirstPoint > 0)
                {
                    if (nOfFirstPoint > 20) nOfFirstPoint -= 20; // Отступаем на несколько точек назад или на начало
                    else nOfFirstPoint = 0;
                    List<EnumDirection?> directionsList = new List<EnumDirection?>();

                    foreach (var item in SelectedFilesTempList) item.Hint = string.Empty;

                    for (int i = nOfFirstPoint + 3; i < SelectedFilesTempList.Count - 2; i++)
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
        private int CheckForCrossZero(List<double> midlAverageShift, out int nOfFirstPoint)
        {
            nOfFirstPoint = 0;
            if (midlAverageShift.Count == 0) return 0;
            int NOfZeroCrossing = 0; // Колличество пересечений с 0
            double prevPoint = 0;

            //foreach(var midl in midlAverageShift
            for (int i = 0; i < midlAverageShift.Count; i++)
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
        private int CheckForDecelerationAndStop(List<SelectedFiles> SelectedFilesTempList) // проверка на замедление \ остановку поезда
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

            if (i >= 1 && j >= 0 && i > j && i < SelectedFiles.Count - 1 && j < SelectedFiles.Count - 2)
            {
                List<Vector> VectorList = GetVectorList(SelectedFiles[j].FullName, SelectedFiles[i].FullName);
                if (VectorList.Count == 0) return false;
                vectorInfo = GetAverages(VectorList);
                if (vectorInfo.Direction == Direction) return true;
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
                    if (i == SelectedFiles.Count - 2 || i == 0) SelectedFiles.RemoveAt(i); //Если точки крайние то просто их удаляем
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
        #endregion

    }
}