using ImgAssemblingLib.AditionalForms;
using NLog;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WinFormsApp1.Enum;

namespace ImgAssemblingLib.Models
{
    public class Assembling
    {
        public Bitmap[] BitmapData { get; set; }
        private AssemblyPlan AssemblyPlan { get; set; }
        private FileEdit fileEdit = new FileEdit();
        private string SavedFileName { get; set; } = string.Empty;
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
        public bool SaveImgFixingRezultToFile { get; set; } = false;
        public bool IsErr { get; set; } = false;
        public string ErrText { get; set; } = string.Empty;
        public EnumErrCode ErrCode { get; set; }
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

        public Assembling(AssemblyPlan assemblyPlan, object param)
        {
            AssemblyPlan = assemblyPlan;
            _context = (SynchronizationContext)param;
            logger = LogManager.GetCurrentClassLogger();
        }
        
        public Assembling(AssemblyPlan assemblyPlan, Bitmap[] bitmapData, object param)
        {
            BitmapData = bitmapData;
            AssemblyPlan = assemblyPlan;
            _context = (SynchronizationContext)param;
            logger = LogManager.GetCurrentClassLogger();
        }

        public void ChangeAssemblyPlan(AssemblyPlan assemblyPlan) => AssemblyPlan = assemblyPlan;
        public Mat GetRezultImg()
        {
            if (RezultImg == null)
            {
                SetErr("Err RezultImg == null!!!");
                return new Mat();
            }
            else return RezultImg;
        }

        public string GetSavedFileName() => SavedFileName;
        public void SetRezultImg(Mat rezultImg) => RezultImg = rezultImg;
        private bool SetErr(string err)
        {
            ErrText = err;
            return false;
        }

        public bool CheckPlane()
        {
            if (AssemblyPlan == null) return SetErr("Err Assembling.AssemblyPlan = null!!!");
            if (AssemblyPlan.BitMap)
            {
                if (BitmapData==null || BitmapData.Length == 0) return SetErr("Err Assembling.BitmapData = null || = 0!!!");
            }
            else
            {
                if (string.IsNullOrEmpty(AssemblyPlan.WorkingDirectory)) return SetErr("Err " + AssemblyPlan.WorkingDirectory + " IsNullOrEmpty!!!");
            }
            
            return true;
        }

        public async Task<FinalResult> TryAssemble()
        {
            try
            {
                await StartAssembling();
                return new FinalResult()
                {
                    Speed = AssemblyPlan.Speed,
                    MatRezult = RezultImg,
                    BitRezult = RezultImg == null ? null : BitmapConverter.ToBitmap(RezultImg),
                    AssemblyReport = GetReport()
                };
            }
            catch (Exception ex)
            {
                return new FinalResult() {
                    IsErr = IsErr,
                    ErrText = ErrText + "\nEx.Message " + ex.Message,
                    ErrList = ErrList
                };
            }
            finally
            {

            }
        }

        public async Task<bool> StartAssembling()
        {
            bool contectIsOn = _context == null ? false : true;
            if (contectIsOn) _context.Send(OnRTBUpDateInfo, "Start Assembling\n");
            logger.Info("\nStart Assembling");

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            TimeSpan ts = stopwatch.Elapsed;
            TimeSpan tSum = TimeSpan.Zero;
            
            bool workingWBitmap = (AssemblyPlan.BitMap && BitmapData != null && BitmapData.Length != 0) ? true : false;
            if (workingWBitmap)
            {
                logger.Info("Working with Bitmap. BitmapData - " + BitmapData.Length);
                if (contectIsOn) _context.Send(OnRTBAddInfo, "   Working with Bitmap. BitmapData - " + BitmapData.Length+"\n");
            }
            else
            {
                logger.Info("Working with Directory");
                if (contectIsOn) _context.Send(OnRTBAddInfo, "   Working with Directory");
            }
            logger.Info("Delta = " + AssemblyPlan.Delta);
            if (contectIsOn) _context.Send(OnRTBAddInfo, "   Delta = " + AssemblyPlan.Delta);

            if (!CheckPlane()) return false;

            if (!workingWBitmap)
            {
                if (contectIsOn) _context.Send(OnRTBAddInfo, "   Start File Name Checking ");
                logger.Info("   Start File Name Checking");
                if (AssemblyPlan.FileNameCheck)
                {
                    var check = fileEdit.CheckFileName(AssemblyPlan.WorkingDirectory);
                    if (check) { SendFinished(); AssemblyPlan.FileNameCheckRezult = "Выполнено."; }
                    else { SendErr("Ошибка при проверке файлов в папке " + AssemblyPlan.WorkingDirectory); AssemblyPlan.FileNameCheckRezult = ErrText; }
                }
                else { SendSkipped(); AssemblyPlan.FileNameCheckRezult = "Этап пропущен!!!"; }
                SendTime("   Time ", ts);
                stopwatch.Restart();

                if (contectIsOn) _context.Send(OnRTBAddInfo, "   File Name Fixing ");
                logger.Info("File Name Fixing");
                if (AssemblyPlan.FileNameFixing)
                {
                    if (fileEdit.FixFileName(AssemblyPlan.WorkingDirectory)) { SendFinished(); AssemblyPlan.FileNameFixingRezult = "Выполнено."; }
                    else { SendErr(" "); AssemblyPlan.FileNameFixingRezult = "Ошибка!!!"; }
                }
                else { SendSkipped(); AssemblyPlan.FileNameFixingRezult = "Этап пропущен!!!"; }
                ts = stopwatch.Elapsed;
                tSum += ts;
                SendTime("  Time ", ts);
                stopwatch.Restart();

                if (contectIsOn) _context.Send(OnRTBAddInfo, "   Del File Copy ");
                logger.Info("Del File Copy");
                if (AssemblyPlan.DelFileCopy)
                {
                    if (await fileEdit.FindCopyAndDel(AssemblyPlan.WorkingDirectory))
                    {
                        if (contectIsOn) _context.Send(OnRTBAddInfo, " - " + fileEdit.TextMessag + "\n");
                        logger.Info(fileEdit.TextMessag);
                        AssemblyPlan.DelFileCopyRezult = fileEdit.TextMessag;
                    }
                    else { SendErr(fileEdit.ErrText + "\n"); AssemblyPlan.DelFileCopyRezult = fileEdit.ErrText; }
                    fileEdit.ClearInformation();
                }
                else { SendSkipped(); AssemblyPlan.DelFileCopyRezult = "Этап пропущен!!!"; }
                ts = stopwatch.Elapsed;
                tSum += ts;
                SendTime("  Time ", ts);
                stopwatch.Restart();
            }

            if (contectIsOn) _context.Send(OnRTBAddInfo, "   Img Fixing ");
            logger.Info("Img Fixing"); // Исправление кадров по загруженной инструкции
            if (AssemblyPlan.FixImg)
            {
                if (workingWBitmap)
                {
                    logger.Info("   Starting Img Fixing using " + AssemblyPlan.ImgFixingPlan + " plan ");
                    if (contectIsOn) _context.Send(OnRTBAddInfo, "\n     Starting Img Fixing using " + AssemblyPlan.ImgFixingPlan + " plan ");

                    ImgFixingForm imgFixingForm = new ImgFixingForm(AssemblyPlan.ImgFixingPlan, SaveImgFixingRezultToFile, AssemblyPlan.FixingImgDirectory);
                    if (contectIsOn)
                    {
                        imgFixingForm.ProcessChanged += worker_ProcessChang;
                        imgFixingForm.TextChanged += worker_TextChang;
                        await Task.Run(() => { BitmapData = imgFixingForm.FixImges(_context, BitmapData); });
                    }
                    else BitmapData = imgFixingForm.FixImges(null, BitmapData);

                    if (imgFixingForm.IsErr || BitmapData.Length == 0)
                    {
                        SendErr(" "); AssemblyPlan.FixImgRezult = "Не выполнено из-за ошибки!!!";
                        if (imgFixingForm.IsErr)
                        {
                            AssemblyPlan.FixImgRezult = imgFixingForm.ErrText;
                            return SetErr(imgFixingForm.ErrText);
                        }
                        if (BitmapData.Length == 0)
                        {
                            AssemblyPlan.FixImgRezult = "Err StartAssembling.BitmapData.Length = 0!!!";
                            return SetErr("Err StartAssembling.BitmapData.Length = 0!!!");
                        }
                    }
                    else { SendFinished(); AssemblyPlan.FixImgRezult = "Выполнено."; }
                }
                else
                {
                    string ImgFixingDir = string.Empty;
                    if (string.IsNullOrEmpty(AssemblyPlan.FixingImgDirectory)) ImgFixingDir = AssemblyPlan.WorkingDirectory + "AutoOut";
                    else ImgFixingDir = AssemblyPlan.FixingImgDirectory;

                    ImgFixingForm imgFixingForm = new ImgFixingForm(AssemblyPlan.ImgFixingPlan, AssemblyPlan.WorkingDirectory, false);
                    if (string.IsNullOrEmpty(AssemblyPlan.ImgFixingPlan)) AssemblyPlan.ImgFixingPlan = imgFixingForm.GetImgFixingPlan();
                    if (contectIsOn) _context.Send(OnRTBAddInfo, " Checking old files ");
                    logger.Info("Checking old files");
                    if (AssemblyPlan.ChekFixImg && imgFixingForm.CheckFixingImg(ImgFixingDir)) // Провереряем существуют ли уже исправленные кадры
                    {
                        AssemblyPlan.StitchingDirectory = ImgFixingDir;
                        if (contectIsOn) _context.Send(OnRTBAddInfo, " - Using old files\n");
                        logger.Info("Using old files");
                        AssemblyPlan.ChekFixImgRezult = "Выполнено.";
                        AssemblyPlan.FixImgRezult = "Пропущено т.к. уже есть исправленные файлы.";
                    }
                    else
                    {
                        if (AssemblyPlan.ChekFixImg)
                        {
                            if (contectIsOn) _context.Send(OnRTBAddInfo, " - Old files not founded ");
                            logger.Info("Old files not founded");
                            AssemblyPlan.ChekFixImgRezult = "Исправленные файлы не найдены!!!";
                        }

                        imgFixingForm.ProcessChanged += worker_ProcessChang;
                        imgFixingForm.TextChanged += worker_TextChang;

                        logger.Info("   Starting Img Fixing using " + AssemblyPlan.ImgFixingPlan + " plan ");
                        if (contectIsOn) _context.Send(OnRTBAddInfo, "\n     Starting Img Fixing using " + AssemblyPlan.ImgFixingPlan + " plan ");
                        await Task.Run(() => { imgFixingForm.FixImges(_context, ImgFixingDir); });

                        if (imgFixingForm.IsErr) { SendFinished(); AssemblyPlan.FixImgRezult = "Выполнено."; AssemblyPlan.StitchingDirectory = ImgFixingDir; }
                        else { SendErr(imgFixingForm.ErrText); AssemblyPlan.FixImgRezult =  imgFixingForm.ErrText; }
                    }
                }
            }
            else {SendSkipped();AssemblyPlan.FixImgRezult = "Этап пропущен!!!";AssemblyPlan.ChekFixImgRezult = "Этап пропущен!!!";}
            ts = stopwatch.Elapsed;
            tSum += ts;
            SendTime("  Time ", ts);
            stopwatch.Restart();

            if (contectIsOn) _context.Send(OnRTBAddInfo, "   Find Key Points ");
            logger.Info("Find Key Points"); // Поиск ключевых точек
            if (AssemblyPlan.FindKeyPoints)
            {
                if (await FindKeyPoints()) { SendFinished(); AssemblyPlan.FindKeyPointsRezult = "Выполнено."; }
                else { SendErr(ErrText); AssemblyPlan.FindKeyPointsRezult = ErrText; }
            }
            else { SendSkipped(); AssemblyPlan.FindKeyPointsRezult = "Этап пропущен!!!"; }
            ts = stopwatch.Elapsed;
            tSum += ts;
            SendTime("  Time ", ts);
            stopwatch.Stop();

            if (contectIsOn) _context.Send(OnRTBAddInfo, "   Get Speed \n");
            logger.Info("Get Speed");
            if (AssemblyPlan.SpeedCounting) // Подсчет скорости
            {
                // SpeedСounter speedСounter = new(stitchingBlock.GetSelectedFiles(), AssemblyPlan);
                SpeedСounter speedСounter = new SpeedСounter(stitchingBlock.GetSelectedFiles(), AssemblyPlan.MillimetersInPixel, AssemblyPlan.TimePerFrame);
                AssemblyPlan.Speed = speedСounter.GetSpeedByPoints(CalculationSpeedDespiteErrors);
                // AssemblyPlan.Speed = speedСounter.GetSpeedByPoints(true);
                var avSpeedList = speedСounter.GetSpeedListByPoints(10);
                double avSp = 0;
                if (avSpeedList.Count > 1)avSp = avSpeedList.Sum(x => x.Sp) / avSpeedList.Count();
                if (contectIsOn)
                {
                    if (AssemblyPlan.Speed != -1) _context.Send(OnRTBAddInfo, "   Скорость ~ " + AssemblyPlan.Speed.ToString() + " Км/ч\n");
                    else { SendErr("Скорость неопределена!!!"); AssemblyPlan.SpeedCountingRezults = ErrText; }
                }
            }
            else { SendSkipped(); AssemblyPlan.SpeedCountingRezults = "Этап пропущен!!!"; }
            ts = stopwatch.Elapsed;
            tSum += ts;
            SendTime("  Time ", ts);
            stopwatch.Stop();

            if (contectIsOn) _context.Send(OnRTBAddInfo, "   Image Assembling ");
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
            if (contectIsOn) _context.Send(OnRTBAddInfo, "   Saving Rezult ");
            logger.Info("Saving Rezult ");
            if (AssemblyPlan.SaveRezults)
            {
                fileEdit.ClearInformation();
                SavedFileName = fileEdit.SaveImg(RezultImg);
                if (!string.IsNullOrEmpty(SavedFileName)){
                    SendFinished(fileEdit.TextMessag);AssemblyPlan.RezultOfSavingRezults = fileEdit.TextMessag;
                    if (AssemblyPlan.ShowAssemblingFile)
                        fileEdit.OpenFileDir(SavedFileName);
                }

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
            if (_context!=null) _context.Send(OnRTBAddInfo, text + String.Format("{0:00}:{1:00}:{2:00}", ts.Minutes, ts.Seconds, ts.Milliseconds / 10) + "\n");
            logger.Info(text + String.Format("{0:00}:{1:00}:{2:00}", ts.Minutes, ts.Seconds, ts.Milliseconds / 10));
        }
        private void SendFinished(string text = null)
        {
            if (string.IsNullOrEmpty(text))
            {
                if (_context != null) _context.Send(OnRTBAddInfo, " - Finished\n");
                logger.Info("Finished");
            }
            else
            {
                if (_context != null) _context.Send(OnRTBAddInfo, " - Finished " + text );
                logger.Info("Finished " + text);
            }
        }
        private void SendSkipped()
        {
            if (_context!=null) _context.Send(OnRTBAddInfo, " -  Skipped\n");
            logger.Info("Skipped");
        }
        private void SendErr(string err)
        {
            SetErr("Err "+err+"!!!\n");
            if (_context != null) _context.Send(OnRTBAddInfo, ErrText);
            logger.Error(ErrText);
        }
        private async Task<bool> FindKeyPoints()
        {
            if (AssemblyPlan.BitMap) stitchingBlock = new StitchingBlock(BitmapData);
            else stitchingBlock = new StitchingBlock(AssemblyPlan);
            stitchingBlock.ProcessChanged += worker_ProcessChang;
            stitchingBlock.TextChanged += worker_TextChang;

            if (AssemblyPlan.BitMap) await Task.Run(() => { stitchingBlock.FindKeyPoints(_context); });
            else
            {
                bool tryReadMapPlan = false;
                if (stitchingBlock.IsErr) return SetErr(stitchingBlock.GetErrText());
                if (AssemblyPlan.ChekStitchPlan) tryReadMapPlan = await stitchingBlock.TryReadMapPlan(AssemblyPlan.From, AssemblyPlan.To); // Если включенно пробуем найти старый план сборки
                if (!tryReadMapPlan)
                {
                    await Task.Run(() => { stitchingBlock.FindKeyPoints(_context); }); // Если плана нет, запускаем создние нового
                    if (AssemblyPlan.DefaultParameters || (AssemblyPlan.Percent && AssemblyPlan.From == 0 && AssemblyPlan.To == 100)) await stitchingBlock.TrySaveMapPlan();
                }
            }

            var areasForDelet = stitchingBlock.FindeBlockForDelet();
            if (areasForDelet.Count > 0)
            {
                stitchingBlock.DeletAreas(areasForDelet);
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

                if(_context!=null) _context.Send(OnImgUpdate, RezultImg);
                if (RezultImg.Width == 0 && RezultImg.Height == 0) return SetErr(stitchingBlock.GetErrText());
                return true;
            }
            else return SetErr("Err StitchingBlock = null !!!");
        }
        private string GetReport()
        {
            string Report = string.Empty;

            return Report;
        }
    }
}