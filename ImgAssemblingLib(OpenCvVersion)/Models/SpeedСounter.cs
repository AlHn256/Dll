using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ImgAssemblingLibOpenCV.Models
{

    //// E:\ImageArchive\3179_1_0AutoOut
    //double MM = 5.74; // Количество мм в одном пикселе
    //double MSek = 41.54; // Милисекунд в одном кадре
    // Скорость 11,898187509741113 Км/ч
    // Скорость 11,655573501941562 Км/ч

    // E:\ImageArchive\Speed TestAutoOut
    // V ~ 20,63886792 Км.ч
    //double MM = 4.82; 
    //double MSek = 40;
    // Скорость 18,21987761393682 Км/ч
    // Скорость(без исправления дист ) 17,02270045503279 Км/ч

    // E:\ImageArchive\13507_31_0_new
    // V ~ 8,359064776
    //double MM = 4.94; 
    //double MSek = 52.18; 
    // Скорость 10,435332724722155 Км/ч
    //internal async Task<double> GetSpeedByPoints(List<string> fileList, object param)

    // E:\ImageArchive\1,2
    // V ~ 22,43
    //double MM = 4.94; 
    //double MM = 5.177777778; // Side
    //double MM = 2.780; // Up
    //double MSek = 40;
    // Up
    // Скорость ~24,46768234693549 Км/ч
    // Скорость ~26,003103708787716 Км/ч
    // Скорость ~16,09715943877335 Км/ч
    // Скорость ~25,309687609886716 Км/ч
    // Скорость ~22,990210089861293 Км/ч

    //Side
    //V~ 20,9700 Км/ч
    //Скорость ~19,22345397269831 Км/ч
    //Скорость ~ 20,076215951478865 Км/ч
    //21,442355671712686 Км/ч
    //22.8939269741532
    // MM 5,51
    // MSek = 40;


    //N98
    //V ~
    //MM 2.911
    //MSek 50,23

    //Контf30 6058
    //Контf40 12192
    //Контf45 13710
    // вл11 электровоз   16440
    // ВЛ65 электропоезд 22500
    // тэм7		         21500
    // Колесо ГрВ		 1050
    // Колесн ПарВагонна 950


    //1_2
    //V ~ 7,181033915
    //MM 10,62807018
    //MSek 50,616667
    //Скорость ~ 6,043630507197215 Км/ч
    //Скорость ~ 6,709994650599466 Км/ч - через выделенную область


    internal class SpeedСounter
    {
        private List<SelectedFiles> SelectedFiles { get; set; }
        public double MmInPixel { get; set; } = 5.5; // Количество мм в одном пикселе
        public double MSekPerFrame { get; set; } = 40; // Милисекунд в одном кадре
        private int MaxPointChainWithoutErrors { get; set; } = 0; // Максимальная цепочка точек без ошибок
        public bool IsErr { get; set; } = false;
        public string ErrText { get; set; } = string.Empty;
        public List<string> ErrList { get; set; } = new List<string>();

        public SpeedСounter(List<SelectedFiles> selectedFiles)
        {
            if (selectedFiles == null)
            {
                SetErr("Err selectedFiles == null !!!");
                return;
            }
            SelectedFiles = selectedFiles;
            if (SelectedFiles.Any(x => x.IsErr)) IsErr = true;

            int nErr = 0, counter =0,fr = 0, to = 0;
            for (int i = 0; i< SelectedFiles.Count; i++)
            {
                if (SelectedFiles[i].IsErr)
                {
                    SetErr(SelectedFiles[i].ErrText);
                    if (nErr < counter) nErr = counter;
                    counter = 0;
                }
                else counter++;
            }

            MaxPointChainWithoutErrors = nErr > counter ? nErr: counter;
        }

        public SpeedСounter(List<SelectedFiles> selectedFiles, double millimetersInPixel, double timePerFrame) : this(selectedFiles)
        {
            MmInPixel = millimetersInPixel;
            MSekPerFrame = timePerFrame;
        }

        private bool SetErr(string err)
        {
            IsErr = true;
            ErrText = err;
            ErrList.Add(err);
            return false;
        }
        public double GetSpeedExperiment(int fr, int to)
        {

            List<Cadr> cadrList = new List<Cadr>();
            if (SelectedFiles.Count == 0)
            {
                SetErr("SelectedFiles.Count = 0");
                return -1;
            }
            List<SelectedFiles> NewFileList = SelectedFiles.Skip(fr).Take(to - fr).ToList();

            if (NewFileList.Count < 2)
            {
                SetErr("NewFileList.Count = 0");
                return -1;
            }

            if (NewFileList.Count == 2)
            {
                var AvSh = (NewFileList[0].AverageShift + NewFileList[1].AverageShift);
                return Math.Abs(1.8 * MmInPixel * (NewFileList[0].AverageShift + NewFileList[1].AverageShift) / MSekPerFrame);
            }

            for (int i = 0; i < NewFileList.Count - 1; i++)
            {
                Cadr cadr = new Cadr();
                string file1 = Path.GetFileNameWithoutExtension(NewFileList[i].FullName);
                string file2 = Path.GetFileNameWithoutExtension(NewFileList[i].StitchingFile);
                int n1 = 0, n2 = 0;
                Int32.TryParse(file1, out n1);
                Int32.TryParse(file2, out n2);
                cadr.FileName = file1 + " - " + file2;
                int numberOfCadr = (n1 == 0 || n2 == 0) ? 1 : n2 - n1;
                cadr.Shift = NewFileList[i].AverageShift / numberOfCadr;
                cadr.NOfCadr = 1;
                cadrList.Add(cadr);
            }

            // Проверка на замедление/остановку
            List<double> MidlAverageShift = new List<double>();
            var avSift = cadrList.Select(x => x.Shift).ToList();
            int N = 10;
            if (cadrList.Count < (N + 5))
            {
                if (cadrList.Count < 5)
                {
                    var sum = cadrList.Sum(x => x.Shift);
                    foreach (var x in cadrList) MidlAverageShift.Add(sum / cadrList.Count);
                }
                else MidlAverageShift = LineAveraging(avSift, 3);
            }
            else
            {
                MidlAverageShift = LineAveraging(avSift, N);
                MidlAverageShift = LineAveraging(MidlAverageShift, N);
                MidlAverageShift = LineAveraging(MidlAverageShift, N);
            }

            //var pointsList = cadrList.Select(x => x.Shift).ToList();
            double avSpeed = 3.6 * MmInPixel * cadrList.Sum(x => x.Shift) / (cadrList.Count - 1) / MSekPerFrame;
            double avSpeed2 = 3.6 * MmInPixel * MidlAverageShift.Sum() / (cadrList.Count - 1) / MSekPerFrame;

            return Math.Abs(avSpeed);
        }

        public List<StatisticList> GetSpeedListByPoints(int n)
        {
            List<StatisticList> SpList = new List<StatisticList>();
            if (SelectedFiles == null)
            {
                SetErr("Err SelectedFiles == null!!!");
                return SpList;
            }
            if(SelectedFiles.Count < 2 || SelectedFiles.Count< n*2) return SpList; // Нет смысла считать среднюю скорость для 1 элемента

            if (SelectedFiles.Count == 2)
            {
                SpList.Add(new StatisticList(0, 1, 1.8 * MmInPixel * (SelectedFiles[0].AverageShift + SelectedFiles[1].AverageShift) / MSekPerFrame));
                return SpList;
            }

            double N = n;
            double D = SelectedFiles.Count / N;
            for (double i = 0; i < N; i++)
            {
                int fr = (int)(i * D);
                int to = (int)((i + 1) * D);
                SpList.Add(new StatisticList(fr, to, GetSpeedExperiment(fr, to)));
            }

            return SpList;
        }
        public double GetSpeedByPoints(bool CalculationSpeedDespiteErrors = true)
        {
            if(!CalculationSpeedDespiteErrors)
            {
                if (IsErr)
                {
                    if(MaxPointChainWithoutErrors>7)
                    {

                    }
                    return -1;
                }
            }
            List<Cadr> cadrList = new List<Cadr>();
            if (SelectedFiles.Count == 0)
            {
                SetErr("SelectedFiles.Count == 0");
                return -1;
            }

            if (SelectedFiles.Count == 1)return  MmInPixel * 3.6 * SelectedFiles[0].AverageShift / MSekPerFrame;

            // Если элементов меньше 11 просто считаем среднюю
            if (SelectedFiles.Count <= 10) return MmInPixel * 3.6 * SelectedFiles.Sum(x => x.AverageShift)/ SelectedFiles.Count / MSekPerFrame;
            // MmInPixel * 3.6 * pointsList.Sum() / (pointsList.Count - 1) / MSekPerFrame;
            // 
            for (int i = 0; i < SelectedFiles.Count - 1; i++)
            {
                Cadr cadr = new Cadr();
                string file1 = Path.GetFileNameWithoutExtension(SelectedFiles[i].FullName);
                string file2 = Path.GetFileNameWithoutExtension(SelectedFiles[i].StitchingFile);
                int n1 = 0, n2 = 0;
                Int32.TryParse(file1, out n1);
                Int32.TryParse(file2, out n2);
                cadr.FileName = file1 + " - " + file2;
                int numberOfCadr = (n1 == 0 || n2 == 0) ? 1 : n2 - n1;
                cadr.Shift = SelectedFiles[i].AverageShift / numberOfCadr;
                cadr.NOfCadr = 1;
                cadrList.Add(cadr);
            }

            List<double> MidlAverageShift = new List<double>();
            List<double> ShiftList = cadrList.Select(x => x.Shift).ToList();
            for (int i = 0; i < 2; i++)
            {
                if (i == 0) MidlAverageShift = LineAveraging(ShiftList);
                else MidlAverageShift = LineAveraging(MidlAverageShift);
            }
            bool isSpeedConst = IsSpeedConst(MidlAverageShift);

            //var pointsList = cadrList.Select(x => x.Shift).ToList();
            double avShift = cadrList.Select(x => x.Shift).Sum()/cadrList.Count;
            double Speed = MmInPixel * 3.6 * avShift / MSekPerFrame;

            return Math.Abs(Speed);
        }

        private bool IsSpeedConst(List<Double> MidlAverageShift)
        {
            var avSpeed = MidlAverageShift.Sum() / (MidlAverageShift.Count - 1);
            bool fl = false;
            foreach (var elem in MidlAverageShift)
            {
                if (Math.Abs(avSpeed - elem) * 100 / avSpeed > 20)
                {
                    fl = true;
                    break;
                }
            }

            MNK mNK = new MNK(MidlAverageShift);
            var a = mNK.A;
            var b = mNK.B;

            return fl;
        }
        private List<double> LineAveraging(List<double> line, int N = 10)
        {
            if (N > line.Count - 1) N = line.Count - 1;
            if (N < 2) N = 2;
            bool isEven = (N % 2) == 0 ? true : false;
            List<double> result = new List<double>();
            Queue<double> queue = new Queue<double>();
            int n = 0;
            bool deque = false;

            while (true)
            {
                if (n < line.Count) queue.Enqueue(line[n++]);
                if (queue.Count > N && deque != true) deque = true;
                if (deque) queue.Dequeue();
                if (queue.Count == 0 || result.Count == line.Count) break;
                if (isEven)
                {
                    if (queue.Count >= N / 2) result.Add(queue.Sum() / queue.Count);
                }
                else
                {
                    if (queue.Count >= N / 2 + 1 || (result.Count == N - 1 && N == line.Count)) result.Add(queue.Sum() / queue.Count);
                }
            }
            return result;
        }
    }
}
