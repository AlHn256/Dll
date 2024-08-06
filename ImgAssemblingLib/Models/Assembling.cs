using ImgAssemblingLib.AditionalForms;
using NLog;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ImgAssemblingLib.Models
{
    public class Assembling
    {
        private AssemblyPlan AssemblyPlan { get; set; }
        private FileEdit fileEdit = new FileEdit();
        private Mat RezultImg { get; set; }
        private SynchronizationContext _context;
        private StitchingBlock stitchingBlock { get; set; }
        public event Action<int> ProcessChanged;
        public event Action<string> TextChanged;
        public event Action<string> RTBUpDateInfo;
        public event Action<string> RTBAddInfo;
        public event Action<Mat> UpdateImg;
        public string StitchingInfo { get; set; }
        public bool StopExecution { get; set; } = false;
        public bool CalculationSpeedDespiteErrors { get; set; } = false;
        public bool IsErr { get; set; } = false;
        public string ErrText { get; set; } = string.Empty;
        public List<string> ErrList { get; set; } = new List<string>();

        private void worker_ProcessChang(int progress) => _context.Send(OnProgressChanged, progress);
        private void worker_TextChang(string text) => _context.Send(OnTextChanged, text);

        public void OnImgUpdate(object img)
        {
            if (UpdateImg != null) UpdateImg((Mat)img);
        }
        public void OnProgressChanged(object i)
        {
            if (ProcessChanged != null) ProcessChanged((int)i);
        }
        public void OnTextChanged(object txt)
        {
            if (TextChanged != null) TextChanged((string)txt);
        }
        public void OnRTBUpDateInfo(object txt)
        {
            if (RTBUpDateInfo != null) RTBUpDateInfo((string)txt);
        }
        public void OnRTBAddInfo(object txt)
        {
            if (RTBAddInfo != null) RTBAddInfo((string)txt);
        }

        private static Logger logger;
        public Assembling(object param)
        {
            _context = (SynchronizationContext)param;
            logger = LogManager.GetCurrentClassLogger();
        }
        public void ChangeAssemblyPlan(AssemblyPlan assemblyPlan)=>AssemblyPlan = assemblyPlan;
        public Mat GetRezultImg()
        {
            if (RezultImg == null)
            {
                SetErr("Err RezultImg == null!!!");
                return new Mat();
            }
            else return RezultImg;
        }
        public void SetRezultImg(Mat rezultImg) => RezultImg = rezultImg;
        private bool SetErr(string err)
        {
            ErrText = err;
            return false;
        }

        public bool CheckPlane()
        {
            if (AssemblyPlan == null) return SetErr("Err AssemblyPlan=null!!!");
            if (string.IsNullOrEmpty(AssemblyPlan.WorkingDirectory)) return SetErr("Err " + AssemblyPlan.WorkingDirectory + " IsNullOrEmpty!!!");
            return true;
        }

        public async Task<bool> StartAssembling()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            TimeSpan ts = stopwatch.Elapsed;
            TimeSpan tSum = TimeSpan.Zero;

            _context.Send(OnRTBAddInfo, "   Delta = " + AssemblyPlan.Delta + "\n   Start File Name Checking ");
            logger.Info("Delta = " + AssemblyPlan.Delta + "   Start File Name Checking");
            if (AssemblyPlan.FileNameCheck)
            {
                var check = fileEdit.CheckFileName(AssemblyPlan.WorkingDirectory);
                if (check){SendFinished();AssemblyPlan.FileNameCheckRezult = "Выполнено.";}
                else { SendErr("Ошибка при проверке файлов в папке " + AssemblyPlan.WorkingDirectory); AssemblyPlan.FileNameCheckRezult = ErrText; }
            }
            else {SendSkipped();AssemblyPlan.FileNameCheckRezult = "Этап пропущен!!!";}
            SendTime("   Time ", ts);
            stopwatch.Restart();

            _context.Send(OnRTBAddInfo, "   File Name Fixing ");
            logger.Info("File Name Fixing");
            if (AssemblyPlan.FileNameFixing)
            {
                if (fileEdit.FixFileName(AssemblyPlan.WorkingDirectory)){SendFinished(); AssemblyPlan.FileNameFixingRezult = "Выполнено.";}
                else { SendErr(" "); AssemblyPlan.FileNameFixingRezult = "Ошибка!!!"; }
            }
            else {SendSkipped();AssemblyPlan.FileNameFixingRezult = "Этап пропущен!!!";}
            ts = stopwatch.Elapsed;
            tSum += ts;
            SendTime("  Time ", ts);
            stopwatch.Restart();

            _context.Send(OnRTBAddInfo, "   Del File Copy ");
            logger.Info("Del File Copy");
            if (AssemblyPlan.DelFileCopy)
            {
                if (await fileEdit.FindCopyAndDel(AssemblyPlan.WorkingDirectory))
                {
                    _context.Send(OnRTBAddInfo, " - " + fileEdit.TextMessag + "\n");
                    logger.Info(fileEdit.TextMessag);
                    AssemblyPlan.DelFileCopyRezult = fileEdit.TextMessag;
                }
                else { SendErr(fileEdit.ErrText + "\n"); AssemblyPlan.DelFileCopyRezult = fileEdit.ErrText; }
                fileEdit.ClearInformation();
            }
            else {SendSkipped();AssemblyPlan.DelFileCopyRezult = "Этап пропущен!!!";}
            ts = stopwatch.Elapsed;
            tSum += ts;
            SendTime("  Time ", ts);
            stopwatch.Restart();

            _context.Send(OnRTBAddInfo, "   Img Fixing ");
            logger.Info("Img Fixing"); // Исправление кадров по загруженной инструкции
            if (AssemblyPlan.FixImg)
            {
                string ImgFixingDir = string.Empty;
                if (string.IsNullOrEmpty(AssemblyPlan.FixingImgDirectory)) ImgFixingDir = AssemblyPlan.WorkingDirectory + "AutoOut";
                else ImgFixingDir = AssemblyPlan.FixingImgDirectory;

                ImgFixingForm distortionTest = new ImgFixingForm(AssemblyPlan.ImgFixingPlan, AssemblyPlan.WorkingDirectory, false);
                if (string.IsNullOrEmpty(AssemblyPlan.ImgFixingPlan)) AssemblyPlan.ImgFixingPlan = distortionTest.GetImgFixingPlan();
                _context.Send(OnRTBAddInfo, " Checking old files ");
                logger.Info("Checking old files");
                if (AssemblyPlan.ChekFixImg && distortionTest.CheckFixigImg(ImgFixingDir)) // Провереряем существуют ли уже исправленные кадры
                {
                    AssemblyPlan.StitchingDirectory = ImgFixingDir;
                    _context.Send(OnRTBAddInfo, " - Using old files\n");
                    logger.Info("Using old files");
                    AssemblyPlan.ChekFixImgRezult = "Выполнено.";
                    AssemblyPlan.FixImgRezult = "Пропущено т.к. уже есть исправленные файлы.";
                }
                else
                {
                    if (AssemblyPlan.ChekFixImg)
                    {
                        _context.Send(OnRTBAddInfo, " - Old files not founded ");
                        logger.Info("Old files not founded");
                        AssemblyPlan.ChekFixImgRezult = "Исправленные файлы не найдены!!!";
                    }

                    distortionTest.ProcessChanged += worker_ProcessChang;
                    distortionTest.TextChanged += worker_TextChang;
                    bool checkFixinImg = false;

                    logger.Info("   Starting Img Fixing using " + AssemblyPlan.ImgFixingPlan + " plan ");
                    _context.Send(OnRTBAddInfo, "\n     Starting Img Fixing using " + AssemblyPlan.ImgFixingPlan + " plan ");
                    await Task.Run(() => { checkFixinImg = distortionTest.FixImges(_context, ImgFixingDir); });

                    if (checkFixinImg){SendFinished(); AssemblyPlan.FixImgRezult = "Выполнено."; AssemblyPlan.StitchingDirectory = ImgFixingDir;}
                    else { SendErr(" "); AssemblyPlan.FixImgRezult = "Не выполнено из-за ошибки!!!"; }
                }
            }
            else {SendSkipped();AssemblyPlan.FixImgRezult = "Этап пропущен!!!";AssemblyPlan.ChekFixImgRezult = "Этап пропущен!!!";}
            ts = stopwatch.Elapsed;
            tSum += ts;
            SendTime("  Time ", ts);
            stopwatch.Restart();
            
            _context.Send(OnRTBAddInfo, "   Find Key Points ");
            logger.Info("Find Key Points"); // Поиск ключевых точек

            if (AssemblyPlan.FindKeyPoints) 
            {
                if (await FindKeyPoints()){SendFinished(); AssemblyPlan.FindKeyPointsRezult = "Выполнено.";}
                else {SendErr(ErrText);AssemblyPlan.FindKeyPointsRezult = ErrText;}
            }
            else { SendSkipped(); AssemblyPlan.StitchRezult = "Этап пропущен!!!"; }
            ts = stopwatch.Elapsed;
            tSum += ts;
            SendTime("  Time ", ts);
            stopwatch.Stop();
            
            _context.Send(OnRTBAddInfo, "   Get Speed \n");
            logger.Info("Get Speed");
            if (AssemblyPlan.SpeedCounting) // Подсчет скорости
            {
                //SpeedСounter speedСounter = new(stitchingBlock.GetSelectedFiles(), AssemblyPlan);
                SpeedСounter speedСounter = new SpeedСounter(stitchingBlock.GetSelectedFiles(), AssemblyPlan.MillimetersInPixel, AssemblyPlan.TimePerFrame);
                AssemblyPlan.Speed = speedСounter.GetSpeedByPoints(CalculationSpeedDespiteErrors);
               // AssemblyPlan.Speed = speedСounter.GetSpeedByPoints(true);
                var avSpeedList = speedСounter.GetSpeedListByPoints(10);
                double avSp = 0;
                if (avSpeedList.Count > 1)avSp = avSpeedList.Sum(x => x.Sp) / avSpeedList.Count();

                if (AssemblyPlan.Speed != -1) _context.Send(OnRTBAddInfo, "   Скорость ~ " + AssemblyPlan.Speed.ToString() + " Км/ч\n");
                else { SendErr("Скорость неопределена!!!"); AssemblyPlan.SpeedCountingRezults = ErrText; }
            }
            else { SendSkipped(); AssemblyPlan.SpeedCountingRezults = "Этап пропущен!!!"; }
            ts = stopwatch.Elapsed;
            tSum += ts;
            SendTime("  Time ", ts);
            stopwatch.Stop();

            _context.Send(OnRTBAddInfo, "   Image Assembling ");
            logger.Info("Image Assembling");
            if (AssemblyPlan.Stitch) // Запуск сборки изображения из нескольких кадров
            {
                if (await StitchImgs()) { SendFinished(); AssemblyPlan.StitchRezult = "Выполнено."; }
                else {SendErr(ErrText); AssemblyPlan.StitchRezult = ErrText; }
            }
            else {SendSkipped(); AssemblyPlan.StitchRezult = "Этап пропущен!!!"; }
            ts = stopwatch.Elapsed;
            tSum += ts;
            SendTime("  Time ", ts);
            stopwatch.Stop();

            // Сохранение итогового изображения
            _context.Send(OnRTBAddInfo, "   Saving Rezult ");
            logger.Info("Saving Rezult ");
            if (AssemblyPlan.SaveRezults)
            {
                fileEdit.ClearInformation();
                if (fileEdit.SaveImg(RezultImg)){SendFinished(fileEdit.TextMessag);AssemblyPlan.RezultOfSavingRezults = fileEdit.TextMessag; }
                else { SendErr(fileEdit.ErrText); AssemblyPlan.StitchRezult = ErrText; }
            }
            else { SendSkipped(); AssemblyPlan.RezultOfSavingRezults = "Этап пропущен!!!"; }
            ts = stopwatch.Elapsed;
            tSum += ts;
            SendTime("  Time ", ts);
            SendTime("  AllTime ", tSum);
            stopwatch.Stop();

            return !IsErr;
        }

        public double GetSpeed() => AssemblyPlan.Speed;

        private void SendTime(string text, TimeSpan ts)
        {
            _context.Send(OnRTBAddInfo, text + String.Format("{0:00}:{1:00}:{2:00}", ts.Minutes, ts.Seconds, ts.Milliseconds / 10) + "\n");
            logger.Info(text + String.Format("{0:00}:{1:00}:{2:00}", ts.Minutes, ts.Seconds, ts.Milliseconds / 10));
        }
        private void SendFinished(string text = null)
        {
            if (string.IsNullOrEmpty(text))
            {
                _context.Send(OnRTBAddInfo, " - Finished\n");
                logger.Info("Finished");
            }
            else
            {
                _context.Send(OnRTBAddInfo, " - Finished " + text );
                logger.Info("Finished " + text);
            }
        }
        private void SendSkipped()
        {
            _context.Send(OnRTBAddInfo, " -  Skipped\n");
            logger.Info("Skipped");
        }
        private void SendErr(string err)
        {
            SetErr("Err "+err+"!!!\n");
            _context.Send(OnRTBAddInfo, ErrText);
            logger.Error(ErrText);
        }

        private async Task<bool> FindKeyPoints()
        {
            //stitchingBlock = new StitchingBlock(AssemblyPlan.StitchingDirectory, AssemblyPlan.AdditionalFilter,  AssemblyPlan.Percent, AssemblyPlan.From, AssemblyPlan.To, AssemblyPlan.Period);
            stitchingBlock = new StitchingBlock(AssemblyPlan);
            stitchingBlock.ProcessChanged += worker_ProcessChang;
            stitchingBlock.TextChanged += worker_TextChang;

            bool tryReadMapPlan = false;
            if (stitchingBlock.IsErr) return SetErr(stitchingBlock.GetErrText());
            if (AssemblyPlan.ChekStitchPlan) tryReadMapPlan = await stitchingBlock.TryReadMapPlan(AssemblyPlan.From, AssemblyPlan.To); // Если включенно пробуем найти старый план сборки
            if (!tryReadMapPlan)
            {
                await Task.Run(() => { stitchingBlock.FindKeyPoints(_context); }); // Если плана нет, запускаем создние нового
                if (AssemblyPlan.DefaultParameters || (AssemblyPlan.Percent && AssemblyPlan.From == 0&& AssemblyPlan.To == 100)) await stitchingBlock.TrySaveMapPlan();
            }

            //stitchingBlock.CheckErr();
            //if (stitchingBlock.IsErr)
            //{
            //    SetErr(stitchingBlock.ErrText);
            //    var sdf = stitchingBlock.GetErrText();
            //    bool fixinErr = stitchingBlock.CheckAndFixErr(_context);
            //    if (!fixinErr) return SetErr(stitchingBlock.ErrText);
            //}

            
            return true;
        }
        private async Task<bool> StitchImgs()
        {
            if (stitchingBlock != null)
            {
                RezultImg = new Mat();
                //await Task.Run(() => { RezultImg = stitchingBlock.Stitch(_context, AssemblyPlan.From, AssemblyPlan.To, AssemblyPlan.Delta); });
                await Task.Run(() => { RezultImg = stitchingBlock.Stitch(_context, AssemblyPlan.Delta); });

                _context.Send(OnImgUpdate, RezultImg);
                if (RezultImg.Width == 0 && RezultImg.Height == 0) return SetErr(stitchingBlock.GetErrText());

                return true;
            }
            else return SetErr("Err StitchingBlock = null !!!");
        }
    }
}