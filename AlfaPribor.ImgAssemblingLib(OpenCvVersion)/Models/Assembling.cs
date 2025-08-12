using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using WinFormsApp1.Enum;

namespace ImgAssemblingLibOpenCV.Models
{
    public class Assembling
    {
        #region Fields
        public int AssemblingId { get; set; }
        private SynchronizationContext _context;
        private FileEdit fileEdit = new FileEdit();
        public Bitmap[] BitmapData { get; set; }
        public AssemblyPlan AssemblyPlan { get; set; }
        private string SavedFileName { get; set; } = string.Empty;
        private Mat RezultImg { get; set; }
        private StitchingBlock stitchingBlock { get; set; }
        public event Action<int> ProcessChanged;
        public event Action<string> TextChanged;
        public event Action<string> RTBUpDateInfo;
        public event Action<string> RTBAddInfo;
        public event Action<Mat> UpdateImg;
        public event Action<string> AddLogEvent;
        public event Action<string> AddErrEvent;
        public bool CalculationSpeedDespiteErrors { get; set; } = false;
        public bool IsErr { get; set; } = false;
        public bool IsCriticalErr { get; set; } = false;
        public string ErrText { get; set; } = string.Empty;
        public EnumErrCode ErrCode { get; set; }
        public List<string> ErrList { get; set; } = new List<string>();
        public FinalResult FinalRezult { get; set; }
        #endregion

        #region Дополнительные функции для взаимодействия с винформами

        public void TriggerAddLog(object txt) => AddLogEvent?.Invoke((string)txt);
        public void TriggerAddErr(object txt) => AddLogEvent?.Invoke((string)txt);
        private void worker_ProcessChang(int progress) => _context?.Send(OnProgressChanged, progress);
        private void worker_TextChang(string text) => _context?.Send(OnTextChanged, text);
        public void OnImgUpdate(object img) => UpdateImg?.Invoke((Mat)img);
        public void OnProgressChanged(object i) => ProcessChanged?.Invoke((int)i);
        public void OnTextChanged(object txt) => TextChanged?.Invoke((string)txt);
        public void OnRTBUpDateInfo(object txt) => RTBUpDateInfo?.Invoke((string)txt);
        public void OnRTBAddInfo(object txt) => RTBAddInfo?.Invoke((string)txt);

        public Assembling()
        {
            AssemblyPlan = new AssemblyPlan();
        }

        public Assembling(object param)
        {
            _context = (SynchronizationContext)param;
        }
        public Assembling(string assemlingFile)
        {
            LoadAssemblyPlan(assemlingFile);
        }
        #endregion
        public Assembling(AssemblyPlan assemblyPlan, Bitmap[] bitmapData, object param)
        {
            BitmapData = bitmapData;
            AssemblyPlan = assemblyPlan;
            _context = (SynchronizationContext)param;
        }
        /// <summary>Загрузка плана сборки</summary>
        /// <param name="file">Путь к файлу параметров</param>
        private AssemblyPlan LoadAssemblyPlan(string file)
        {
            AssemblyPlan assemblyPlan;
            fileEdit.LoadeJson(file, out assemblyPlan);
            if (assemblyPlan != null) AssemblyPlan = assemblyPlan;
            return assemblyPlan;
        }
        /// <summary> Замена плана сборки</summary>
        /// <param name="assemblyPlan">Новый план</param>
        public void ChangeAssemblyPlan(AssemblyPlan assemblyPlan)
        {
            ClearAll();
            if (assemblyPlan == null) SetErr("AssemblyPlan is null");
            AssemblyPlan = assemblyPlan;
        }

        public void DelRezultImg(Mat rezultImg) => RezultImg = rezultImg;
        public Mat GetRezultImg() => RezultImg;
        public string GetRezultFileName() => SavedFileName;
        /// <summary>  Запись ошибки</summary>
        private bool SetErr(string err)
        {
            TriggerAddErr(AssemblingId + " " + err);
            IsErr = true;
            ErrText = err;
            ErrList.Add(err);
            return false;
        }
        /// <summary> Запись критической ошибки </summary>
        private bool SetCriticalErr(string err)
        {
            TriggerAddErr(AssemblingId + " Critical" + err);
            IsErr = true;
            IsCriticalErr = true;
            ErrText = err;
            ErrList.Add(err);
            return false;
        }
        /// <summary> Предварительная проверка плана сборки</summary>
        public bool CheckPlane()
        {
            if (AssemblyPlan == null) return SetCriticalErr("Err Assembling.AssemblyPlan = null!!!");
            if (AssemblyPlan.BitMap)
            {
                if (BitmapData == null || BitmapData.Length == 0) return SetCriticalErr("Err Assembling.BitmapData = null || = 0!!!");
            }
            else
            {
                if (string.IsNullOrEmpty(AssemblyPlan.WorkingDirectory)) return SetCriticalErr("Err CheckPlane.WorkingDirectory Is Null Or Empty!!!");
                if (string.IsNullOrEmpty(AssemblyPlan.StitchingDirectory)) return SetCriticalErr("Err CheckPlane.StitchingDirectory Is Null Or Empty!!!");

                if (!Directory.Exists(AssemblyPlan.WorkingDirectory)) return SetCriticalErr($"Err {AssemblyPlan.WorkingDirectory} !Exists!!!");
                if (!Directory.Exists(AssemblyPlan.StitchingDirectory)) return SetCriticalErr($"Err {AssemblyPlan.StitchingDirectory} !Exists!!!");
            }

            if (AssemblyPlan.FixImg)
            {
                if (string.IsNullOrEmpty(AssemblyPlan.ImgFixingPlan)) return SetCriticalErr("Err AssemblyPlan.ImgFixingPlan Is Null Or Empty!!!");
                if (!File.Exists(AssemblyPlan.ImgFixingPlan)) return SetCriticalErr($"Err {AssemblyPlan.ImgFixingPlan} !Exists !!!");
            }

            if (AssemblyPlan.ShowRezult == true && AssemblyPlan.SaveRezult == false)
            {
                AssemblyPlan.SaveRezult = true;
                SetErr($"Err CheckPlane.ShowRezult can't be without SaveRezult.\n SaveRezult is On!");
            }

            if (AssemblyPlan.SaveRezult == true && AssemblyPlan.Stitch == false)
            {
                AssemblyPlan.Stitch = true;
                SetErr($"Err CheckPlane.SaveRezult can't be without Stitch.\n Stitch  is On!");
            }

            if (AssemblyPlan.SpeedCounting == true && AssemblyPlan.Stitch == false)
            {
                AssemblyPlan.Stitch = true;
                SetErr($"Err CheckPlane.SpeedCounting can't be without Stitch.\n Stitch  is On!");
            }
            return true;
        }

        /// <summary>
        /// Сборка кадров с подробной записью результатов (Асинхронная версия)
        /// </summary>
        /// <returns>FinalResult Подробный отчет о результатах сборки</returns>
        public async Task<FinalResult> TryAssembleAsync()
        {
            try
            {
                await StartAssemblingAsync();
                if (RezultImg == null)
                {
                    return FinalRezult = new FinalResult()
                    {
                        Speed = 0,
                        MatRezult = null,
                        BitRezult = null,
                        IsErr = IsErr,
                        IsCriticalErr = IsCriticalErr,
                        ErrText = ErrText,
                        ErrList = ErrList
                    };
                }
                else return FinalRezult = new FinalResult()
                {
                    Speed = AssemblyPlan.Speed,
                    MatRezult = RezultImg,
                    BitRezult = (RezultImg.Width != 0 && RezultImg.Height != 0) ? BitmapConverter.ToBitmap(RezultImg) : null,
                    IsErr = IsErr,
                    IsCriticalErr = IsCriticalErr,
                    ErrText = ErrText,
                    ErrList = ErrList
                };
            }
            catch (Exception ex)
            {
                SetCriticalErr(ex.Message);
                return FinalRezult = new FinalResult()
                {
                    IsErr = IsErr,
                    IsCriticalErr = IsCriticalErr,
                    ErrText = ErrText + "Ex.Message " + ex.Message,
                    ErrList = ErrList
                };
            }
        }
        /// <summary>  Запуск сборки изображения </summary>
        public bool StartAssembling()
        {
            FinalRezult = new FinalResult();
            bool contectIsOn = _context == null ? false : true;
            if (contectIsOn) _context.Send(OnRTBAddInfo, "Start Assembling\n");
            TriggerAddLog("/n" + AssemblingId + " Start Assembling");

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            TimeSpan ts = stopwatch.Elapsed;
            TimeSpan tSum = TimeSpan.Zero;

            // Проверка плана сборки на ошибки
            if (!CheckPlane()) return SetCriticalErr("Err CheckPlane not pass!!!");

            if (AssemblyPlan.BitMap)
            {
                TriggerAddLog(AssemblingId + " Working with Bitmap. BitmapData - " + BitmapData.Length);
                if (contectIsOn) _context.Send(OnRTBAddInfo, "   Working with Bitmap. BitmapData - " + BitmapData.Length + "\n");
            }
            else
            {
                TriggerAddLog(AssemblingId + " Working with Directory");
                if (contectIsOn) _context.Send(OnRTBAddInfo, "   Working with Directory");
            }
            TriggerAddLog(AssemblingId + " Delta = " + AssemblyPlan.Delta);
            if (contectIsOn) _context.Send(OnRTBAddInfo, "   Delta = " + AssemblyPlan.Delta);

            if (!AssemblyPlan.BitMap)
            {
                if (contectIsOn) _context.Send(OnRTBAddInfo, "   File Name Fixing ");
                TriggerAddLog(AssemblingId + " File Name Fixing");
                if (AssemblyPlan.FileNameFixing)
                {
                    if (fileEdit.FixFileName(AssemblyPlan.WorkingDirectory)) { SendFinished(); FinalRezult.FileNameFixingRezult = "Выполнено."; }
                    else { SendErr(" "); FinalRezult.FileNameFixingRezult = "Ошибка!!!"; }
                }
                else { SendSkipped(); FinalRezult.FileNameFixingRezult = "Этап пропущен!!!"; }
                ts = stopwatch.Elapsed;
                tSum += ts;
                SendTime(" Time ", ts);
                stopwatch.Restart();

                if (contectIsOn) _context.Send(OnRTBAddInfo, "   Del File Copy ");
                TriggerAddLog(AssemblingId + " Del File Copy");
                if (AssemblyPlan.DelFileCopy)
                {
                    if (fileEdit.FindCopyAndDel(AssemblyPlan.WorkingDirectory))
                    {
                        if (contectIsOn) _context.Send(OnRTBAddInfo, " - " + fileEdit.TextMessag + "\n");
                        TriggerAddLog(fileEdit.TextMessag);
                        FinalRezult.DelFileCopyRezult = fileEdit.TextMessag;
                    }
                    else { SendErr(fileEdit.ErrText + "\n"); FinalRezult.DelFileCopyRezult = fileEdit.ErrText; }
                    fileEdit.ClearInformation();
                }
                else { SendSkipped(); FinalRezult.DelFileCopyRezult = "Этап пропущен!!!"; }
                ts = stopwatch.Elapsed;
                tSum += ts;
                SendTime(" Time ", ts); 
                stopwatch.Restart();
            }

            if (contectIsOn) _context.Send(OnRTBAddInfo, "   Img Fixing ");
            // Исправление кадров по загруженной инструкции
            TriggerAddLog(AssemblingId + " Img Fixing");
            if (AssemblyPlan.FixImg)
            {
                if (AssemblyPlan.BitMap) // Работа с массивом кадров или с кадрами из директории
                {
                    TriggerAddLog(AssemblingId + " Starting Img Fixing using " + AssemblyPlan.ImgFixingPlan + " plan ");
                    if (contectIsOn) _context.Send(OnRTBAddInfo, "\n     Starting Img Fixing using " + AssemblyPlan.ImgFixingPlan + " plan ");

                    FixFrames FixFrames = new FixFrames(AssemblyPlan.ImgFixingPlan, AssemblyPlan.SaveImgFixingRezultToFile, AssemblyPlan.FixingImgDirectory);
                    if (contectIsOn)
                    {
                        FixFrames.ProcessChanged += worker_ProcessChang;
                        FixFrames.TextChanged += worker_TextChang;
                        BitmapData = FixFrames.FixImges(_context, BitmapData);
                    }
                    else BitmapData = FixFrames.FixImges(null, BitmapData);

                    if (FixFrames.IsErr || BitmapData.Length == 0)
                    {
                        SendErr(" "); FinalRezult.FixImgRezult = "Не выполнено из-за ошибки!!!";
                        if (FixFrames.IsErr)
                        {
                            FinalRezult.FixImgRezult = FixFrames.ErrText;
                            return SetErr(FixFrames.ErrText);
                        }
                        if (BitmapData.Length == 0)
                        {
                            FinalRezult.FixImgRezult = "Err StartAssembling.BitmapData.Length = 0!!!";
                            return SetErr("Err StartAssembling.BitmapData.Length = 0!!!");
                        }
                    }
                    else { SendFinished(); FinalRezult.FixImgRezult = "Выполнено."; }
                }
                else
                {
                    string ImgFixingDir = string.Empty;
                    if (string.IsNullOrEmpty(AssemblyPlan.FixingImgDirectory)) ImgFixingDir = AssemblyPlan.WorkingDirectory + "AutoOut";
                    else ImgFixingDir = AssemblyPlan.FixingImgDirectory;

                    if (contectIsOn) _context.Send(OnRTBAddInfo, " Checking old files ");
                    TriggerAddLog(AssemblingId + " Checking old files");
                    if (AssemblyPlan.ChekFixImg && CheckFixingImg(AssemblyPlan.WorkingDirectory, ImgFixingDir)) // Провереряем существуют ли уже исправленные кадры
                    {
                        AssemblyPlan.StitchingDirectory = ImgFixingDir;
                        if (contectIsOn) _context.Send(OnRTBAddInfo, " - Using old files\n");
                        TriggerAddLog(AssemblingId + " Using old files");
                        FinalRezult.ChekFixImgRezult = "Выполнено.";
                        FinalRezult.FixImgRezult = "Пропущено т.к. уже есть исправленные файлы.";
                    }
                    else
                    {
                        if (AssemblyPlan.ChekFixImg)
                        {
                            if (contectIsOn) _context.Send(OnRTBAddInfo, " - Old files not founded ");
                            TriggerAddLog(AssemblingId + " Old files not founded");
                            FinalRezult.ChekFixImgRezult = "Исправленные файлы не найдены!!!";
                        }

                        //ImgFixingForm imgFixingForm = new ImgFixingForm(AssemblyPlan.ImgFixingPlan, AssemblyPlan.WorkingDirectory);
                        FixFrames FixFrames = new FixFrames(AssemblyPlan.ImgFixingPlan, AssemblyPlan.WorkingDirectory);
                        //if (string.IsNullOrEmpty(AssemblyPlan.ImgFixingPlan)) AssemblyPlan.ImgFixingPlan = FixFrames.GetImgFixingPlan();

                        FixFrames.ProcessChanged += worker_ProcessChang;
                        FixFrames.TextChanged += worker_TextChang;
                        TriggerAddLog(AssemblingId + "   Starting Img Fixing using " + AssemblyPlan.ImgFixingPlan + " plan ");
                        if (contectIsOn) _context.Send(OnRTBAddInfo, "\n     Starting Img Fixing using " + AssemblyPlan.ImgFixingPlan + " plan ");

                        if (FixFrames.FixImges(_context, ImgFixingDir)) { SendErr(FixFrames.ErrText); FinalRezult.FixImgRezult = FixFrames.ErrText; }
                        else { SendFinished(); FinalRezult.FixImgRezult = "Выполнено."; AssemblyPlan.StitchingDirectory = ImgFixingDir; }
                    }
                }
            }
            else { SendSkipped(); FinalRezult.FixImgRezult = "Этап пропущен!!!"; FinalRezult.ChekFixImgRezult = "Этап пропущен!!!"; }
            ts = stopwatch.Elapsed;
            tSum += ts;
            SendTime(" Time ", ts);
            stopwatch.Restart();

            if (contectIsOn) _context.Send(OnRTBAddInfo, "   Find Key Points ");
            TriggerAddLog(AssemblingId + " Find Key Points");
            if (AssemblyPlan.FindKeyPoints) // Поиск ключевых точек
            {
                if (FindKeyPoints()) { SendFinished(); FinalRezult.FindKeyPointsRezult = "Выполнено."; }
                else
                {
                    SendErr(ErrText); FinalRezult.FindKeyPointsRezult = ErrText;
                    if (IsCriticalErr) return false;
                }
            }
            else { SendSkipped(); FinalRezult.FindKeyPointsRezult = "Этап пропущен!!!"; }
            ts = stopwatch.Elapsed;
            tSum += ts;
            SendTime(" Time ", ts);
            stopwatch.Restart();

            if (contectIsOn) _context.Send(OnRTBAddInfo, "   Get Speed \n");
            TriggerAddLog(AssemblingId + " Get Speed");
            if (AssemblyPlan.SpeedCounting) // Подсчет скорости
            {
                SpeedСounter speedСounter = new SpeedСounter(stitchingBlock.GetSelectedFiles(), AssemblyPlan.MillimetersInPixel, AssemblyPlan.TimePerFrame);
                AssemblyPlan.Speed = speedСounter.GetSpeedByPoints(CalculationSpeedDespiteErrors);
                //var avSpeedList = speedСounter.GetSpeedListByPoints(10);
                //double avSp = 0;
                //if (avSpeedList.Count > 1)avSp = avSpeedList.Sum(x => x.Sp) / avSpeedList.Count();
                if (contectIsOn)
                {
                    if (AssemblyPlan.Speed != -1) _context.Send(OnRTBAddInfo, "   Скорость ~ " + AssemblyPlan.Speed.ToString() + " Км/ч\n");
                    else { SendErr("Скорость неопределена!!!"); FinalRezult.SpeedCountingRezults = ErrText; }
                }
            }
            else { SendSkipped(); FinalRezult.SpeedCountingRezults = "Этап пропущен!!!"; }
            ts = stopwatch.Elapsed;
            tSum += ts;
            SendTime(" Time ", ts);
            stopwatch.Restart();

            if (contectIsOn) _context.Send(OnRTBAddInfo, "   Image Assembling ");
            TriggerAddLog(AssemblingId + " Image Assembling");
            if (AssemblyPlan.Stitch) // Запуск сборки изображения из нескольких кадров
            {
                if (StitchImgs()) { SendFinished(); FinalRezult.StitchRezult = "Выполнено."; }
                else { SendErr(ErrText); FinalRezult.StitchRezult = ErrText; }
            }
            else { SendSkipped(); FinalRezult.StitchRezult = "Этап пропущен!!!"; }
            ts = stopwatch.Elapsed;
            tSum += ts;
            SendTime(" Time ", ts);
            stopwatch.Restart();

            if (contectIsOn) _context.Send(OnRTBAddInfo, "   Saving Rezult ");
            TriggerAddLog(AssemblingId + " Saving Rezult");
            if (AssemblyPlan.SaveRezult) // Сохранение итогового изображения
            {
                fileEdit.ClearInformation();
                SavedFileName = fileEdit.SaveImg(RezultImg);
                if (!string.IsNullOrEmpty(SavedFileName))
                {
                    SendFinished(fileEdit.TextMessag); FinalRezult.RezultOfSavingRezults = fileEdit.TextMessag;
                    if (AssemblyPlan.ShowRezult)
                        fileEdit.OpenFileDir(SavedFileName);
                }

                else { SendErr(fileEdit.ErrText); FinalRezult.StitchRezult = ErrText; }
            }
            else { SendSkipped(); FinalRezult.RezultOfSavingRezults = "Этап пропущен!!!"; }
            ts = stopwatch.Elapsed;
            tSum += ts;
            SendTime(" Time ", ts);
            SendTime(" AllTime ", tSum);

            TriggerAddLog(AssemblingId + " Assembling Finished\n");

            stopwatch.Restart();
            return !IsErr;
        }
        /// <summary>  Запуск сборки изображения (асинхронная версия) </summary>
        public async Task<bool> StartAssemblingAsync()
        {
            FinalRezult = new FinalResult();
            bool contectIsOn = _context == null ? false : true;
            if (contectIsOn) _context.Send(OnRTBAddInfo, "Start Assembling\n");
            TriggerAddLog("/n" + AssemblingId + " Start Assembling");

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            TimeSpan ts = stopwatch.Elapsed;
            TimeSpan tSum = TimeSpan.Zero;

            // Проверка плана сборки на ошибки
            if (!CheckPlane()) return SetCriticalErr("Err CheckPlane not pass!!!");

            if (AssemblyPlan.BitMap)
            {
                TriggerAddLog(AssemblingId + " Working with Bitmap. BitmapData - " + BitmapData.Length);
                if (contectIsOn) _context.Send(OnRTBAddInfo, "   Working with Bitmap. BitmapData - " + BitmapData.Length + "\n");
            }
            else
            {
                TriggerAddLog(AssemblingId + " Working with Directory");
                if (contectIsOn) _context.Send(OnRTBAddInfo, "   Working with Directory");
            }
            TriggerAddLog(AssemblingId + " Delta = " + AssemblyPlan.Delta);
            if (contectIsOn) _context.Send(OnRTBAddInfo, "   Delta = " + AssemblyPlan.Delta);

            if (!AssemblyPlan.BitMap)
            {
                if (contectIsOn) _context.Send(OnRTBAddInfo, "   File Name Fixing ");
                TriggerAddLog(AssemblingId + " File Name Fixing");
                if (AssemblyPlan.FileNameFixing)
                {
                    if (fileEdit.FixFileName(AssemblyPlan.WorkingDirectory)) { SendFinished(); FinalRezult.FileNameFixingRezult = "Выполнено."; }
                    else { SendErr(" "); FinalRezult.FileNameFixingRezult = "Ошибка!!!"; }
                }
                else { SendSkipped(); FinalRezult.FileNameFixingRezult = "Этап пропущен!!!"; }
                ts = stopwatch.Elapsed;
                tSum += ts;
                SendTime(" Time ", ts);
                stopwatch.Restart();

                if (contectIsOn) _context.Send(OnRTBAddInfo, "   Del File Copy ");
                TriggerAddLog(AssemblingId + " Del File Copy");
                if (AssemblyPlan.DelFileCopy)
                {
                    if (await fileEdit.FindCopyAndDelAsync(AssemblyPlan.WorkingDirectory))
                    {
                        if (contectIsOn) _context.Send(OnRTBAddInfo, " - " + fileEdit.TextMessag + "\n");
                        TriggerAddLog(fileEdit.TextMessag);
                        FinalRezult.DelFileCopyRezult = fileEdit.TextMessag;
                    }
                    else { SendErr(fileEdit.ErrText + "\n"); FinalRezult.DelFileCopyRezult = fileEdit.ErrText; }
                    fileEdit.ClearInformation();
                }
                else { SendSkipped(); FinalRezult.DelFileCopyRezult = "Этап пропущен!!!"; }
                ts = stopwatch.Elapsed;
                tSum += ts;
                SendTime(" Time ", ts);
                stopwatch.Restart();
            }

            if (contectIsOn) _context.Send(OnRTBAddInfo, "   Img Fixing ");
            // Исправление кадров по загруженной инструкции
            TriggerAddLog(AssemblingId + " Img Fixing");
            if (AssemblyPlan.FixImg)
            {
                if (AssemblyPlan.BitMap)
                {
                    TriggerAddLog(AssemblingId + " Starting Img Fixing using " + AssemblyPlan.ImgFixingPlan + " plan ");
                    if (contectIsOn) _context.Send(OnRTBAddInfo, "\n     Starting Img Fixing using " + AssemblyPlan.ImgFixingPlan + " plan ");

                    //ImgFixingForm imgFixingForm = new ImgFixingForm(AssemblyPlan.ImgFixingPlan, AssemblyPlan.SaveImgFixingRezultToFile, AssemblyPlan.FixingImgDirectory);
                    FixFrames FixFrames = new FixFrames(AssemblyPlan.ImgFixingPlan, AssemblyPlan.SaveImgFixingRezultToFile, AssemblyPlan.FixingImgDirectory);
                    if (contectIsOn)
                    {
                        FixFrames.ProcessChanged += worker_ProcessChang;
                        FixFrames.TextChanged += worker_TextChang;
                        await Task.Run(() => { BitmapData = FixFrames.FixImges(_context, BitmapData); });
                    }
                    else BitmapData = FixFrames.FixImges(null, BitmapData);

                    if (FixFrames.IsErr || BitmapData.Length == 0)
                    {
                        SendErr(" "); FinalRezult.FixImgRezult = "Не выполнено из-за ошибки!!!";
                        if (FixFrames.IsErr)
                        {
                            FinalRezult.FixImgRezult = FixFrames.ErrText;
                            return SetErr(FixFrames.ErrText);
                        }
                        if (BitmapData.Length == 0)
                        {
                            FinalRezult.FixImgRezult = "Err StartAssembling.BitmapData.Length = 0!!!";
                            return SetErr("Err StartAssembling.BitmapData.Length = 0!!!");
                        }
                    }
                    else { SendFinished(); FinalRezult.FixImgRezult = "Выполнено."; }
                }
                else
                {
                    string ImgFixingDir = string.Empty;
                    if (string.IsNullOrEmpty(AssemblyPlan.FixingImgDirectory)) ImgFixingDir = AssemblyPlan.WorkingDirectory + "AutoOut";
                    else ImgFixingDir = AssemblyPlan.FixingImgDirectory;

                    //FixFrames FixFrames = new FixFrames(AssemblyPlan.ImgFixingPlan, AssemblyPlan.WorkingDirectory);
                    //if (string.IsNullOrEmpty(AssemblyPlan.ImgFixingPlan)) AssemblyPlan.ImgFixingPlan = FixFrames.GetImgFixingPlan();

                    if (contectIsOn) _context.Send(OnRTBAddInfo, " Checking old files ");
                    TriggerAddLog(AssemblingId + " Checking old files");
                    if (AssemblyPlan.ChekFixImg && CheckFixingImg(AssemblyPlan.WorkingDirectory, ImgFixingDir)) // Провереряем существуют ли уже исправленные кадры
                    {
                        AssemblyPlan.StitchingDirectory = ImgFixingDir;
                        if (contectIsOn) _context.Send(OnRTBAddInfo, " - Using old files\n");
                        TriggerAddLog(AssemblingId + " Using old files");
                        FinalRezult.ChekFixImgRezult = "Выполнено.";
                        FinalRezult.FixImgRezult = "Пропущено т.к. уже есть исправленные файлы.";
                    }
                    else
                    {
                        if (AssemblyPlan.ChekFixImg)
                        {
                            if (contectIsOn) _context.Send(OnRTBAddInfo, " - Old files not founded ");
                            TriggerAddLog(AssemblingId + " Old files not founded");
                            FinalRezult.ChekFixImgRezult = "Исправленные файлы не найдены!!!";
                        }

                        FixFrames FixFrames = new FixFrames(AssemblyPlan.ImgFixingPlan, AssemblyPlan.WorkingDirectory);
                        //if (string.IsNullOrEmpty(AssemblyPlan.ImgFixingPlan)) AssemblyPlan.ImgFixingPlan = FixFrames.GetImgFixingPlan();

                        FixFrames.ProcessChanged += worker_ProcessChang;
                        FixFrames.TextChanged += worker_TextChang;
                        TriggerAddLog(AssemblingId + "   Starting Img Fixing using " + AssemblyPlan.ImgFixingPlan + " plan ");
                        if (contectIsOn) _context.Send(OnRTBAddInfo, "\n     Starting Img Fixing using " + AssemblyPlan.ImgFixingPlan + " plan ");

                        await Task.Run(() =>
                        {
                            if (FixFrames.FixImges(_context, ImgFixingDir)) { SendErr(FixFrames.ErrText); FinalRezult.FixImgRezult = FixFrames.ErrText; }
                            else { SendFinished(); FinalRezult.FixImgRezult = "Выполнено."; AssemblyPlan.StitchingDirectory = ImgFixingDir; }
                        });
                    }
                }
            }
            else { SendSkipped(); FinalRezult.FixImgRezult = "Этап пропущен!!!"; FinalRezult.ChekFixImgRezult = "Этап пропущен!!!"; }
            ts = stopwatch.Elapsed;
            tSum += ts;
            SendTime(" Time ", ts);
            stopwatch.Restart();

            if (contectIsOn) _context.Send(OnRTBAddInfo, "   Find Key Points ");
            TriggerAddLog(AssemblingId + " Find Key Points");
            if (AssemblyPlan.FindKeyPoints) // Поиск ключевых точек
            {
                if (await FindKeyPointsAsync()) { SendFinished(); FinalRezult.FindKeyPointsRezult = "Выполнено."; }
                else
                {
                    SendErr(ErrText); FinalRezult.FindKeyPointsRezult = ErrText;
                    if (IsCriticalErr) return false;
                }
            }
            else { SendSkipped(); FinalRezult.FindKeyPointsRezult = "Этап пропущен!!!"; }
            ts = stopwatch.Elapsed;
            tSum += ts;
            SendTime(" Time ", ts);
            stopwatch.Restart();

            if (contectIsOn) _context.Send(OnRTBAddInfo, "   Get Speed \n");
            TriggerAddLog(AssemblingId + " Get Speed");
            if (AssemblyPlan.SpeedCounting) // Подсчет скорости
            {
                SpeedСounter speedСounter = new SpeedСounter(stitchingBlock.GetSelectedFiles(), AssemblyPlan.MillimetersInPixel, AssemblyPlan.TimePerFrame);
                AssemblyPlan.Speed = speedСounter.GetSpeedByPoints(CalculationSpeedDespiteErrors);
                //var avSpeedList = speedСounter.GetSpeedListByPoints(10);
                //double avSp = 0;
                //if (avSpeedList.Count > 1)avSp = avSpeedList.Sum(x => x.Sp) / avSpeedList.Count();
                if (contectIsOn)
                {
                    if (AssemblyPlan.Speed != -1) _context.Send(OnRTBAddInfo, "   Скорость ~ " + AssemblyPlan.Speed.ToString() + " Км/ч\n");
                    else { SendErr("Скорость неопределена!!!"); FinalRezult.SpeedCountingRezults = ErrText; }
                }
            }
            else { SendSkipped(); FinalRezult.SpeedCountingRezults = "Этап пропущен!!!"; }
            ts = stopwatch.Elapsed;
            tSum += ts;
            SendTime(" Time ", ts);
            stopwatch.Restart();

            if (contectIsOn) _context.Send(OnRTBAddInfo, "   Image Assembling ");
            TriggerAddLog(AssemblingId + " Image Assembling");
            if (AssemblyPlan.Stitch) // Запуск сборки изображения из нескольких кадров
            {
                if (await StitchImgsAsync()) { SendFinished(); FinalRezult.StitchRezult = "Выполнено."; }
                else { SendErr(ErrText); FinalRezult.StitchRezult = ErrText; }
            }
            else { SendSkipped(); FinalRezult.StitchRezult = "Этап пропущен!!!"; }
            ts = stopwatch.Elapsed;
            tSum += ts;
            SendTime(" Time ", ts);
            stopwatch.Restart();

            if (contectIsOn) _context.Send(OnRTBAddInfo, "   Saving Rezult ");
            TriggerAddLog(AssemblingId + " Saving Rezult");
            if (AssemblyPlan.SaveRezult) // Сохранение итогового изображения
            {
                fileEdit.ClearInformation();
                SavedFileName = fileEdit.SaveImg(RezultImg);
                if (!string.IsNullOrEmpty(SavedFileName))
                {
                    SendFinished(fileEdit.TextMessag); FinalRezult.RezultOfSavingRezults = fileEdit.TextMessag;
                    if (AssemblyPlan.ShowRezult)
                        fileEdit.OpenFileDir(SavedFileName);
                }

                else { SendErr(fileEdit.ErrText); FinalRezult.StitchRezult = ErrText; }
            }
            else { SendSkipped(); FinalRezult.RezultOfSavingRezults = "Этап пропущен!!!"; }
            ts = stopwatch.Elapsed;
            tSum += ts;
            SendTime(" Time ", ts);
            SendTime(" AllTime ", tSum);

            TriggerAddLog(AssemblingId + " Assembling Finished\n");

            stopwatch.Restart();
            return !IsErr;
        }
        /// <summary>  Запрос скорости </summary>
        public double GetSpeed() => AssemblyPlan.Speed;
        /// <summary> Сообщения о затраченном времени на операцию</summary>
        private void SendTime(string text, TimeSpan ts)
        {
            string txt = String.Format("{0:00}:{1:00}:{2:00}", ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
            if (_context != null) _context.Send(OnRTBAddInfo, text + txt + "\n");
            TriggerAddLog(AssemblingId + txt);
        }
        /// <summary> Сообщения о пропуске этапа </summary>
        private void SendSkipped()
        {
            if (_context != null) _context.Send(OnRTBAddInfo, " -  Skipped\n");
            TriggerAddLog(AssemblingId + " Skipped");
        }
        /// <summary> Отправка сообщения об ошибке </summary>
        private void SendErr(string err)
        {
            SetErr(err);
            if (_context != null) _context.Send(OnRTBAddInfo, ErrText);
            TriggerAddLog(AssemblingId + ErrText);
        }
        /// <summary>
        /// Отправка сообщения об оконцании операции
        /// </summary>
        /// <param name="text">Дополнительная информация</param>
        private void SendFinished(string text = null)
        {
            if (string.IsNullOrEmpty(text))
            {
                if (_context != null) _context.Send(OnRTBAddInfo, " - Finished\n");
                TriggerAddLog(AssemblingId + " Finished");
            }
            else
            {
                if (_context != null) _context.Send(OnRTBAddInfo, " - Finished " + text);
                TriggerAddLog(AssemblingId + " Finished " + text);
            }
        }
        /// <summary> Поиск ключевых точек  </summary>
        private bool FindKeyPoints()
        {
            if (AssemblyPlan.BitMap) stitchingBlock = new StitchingBlock(BitmapData, AssemblyPlan.Period);
            else stitchingBlock = new StitchingBlock(AssemblyPlan);
            stitchingBlock.ProcessChanged += worker_ProcessChang;
            stitchingBlock.TextChanged += worker_TextChang;
            stitchingBlock.NewEngin = AssemblyPlan.NewEngin;

            if (AssemblyPlan.BitMap) stitchingBlock.FindKeyPoints(_context);
            else
            {
                bool tryReadMapPlan = false;
                if (stitchingBlock.IsErr) return SetErr(stitchingBlock.GetErrText());
                if (AssemblyPlan.ChekStitchPlan) tryReadMapPlan = stitchingBlock.TryReadMapPlan(AssemblyPlan.From, AssemblyPlan.To); // Если включенно пробуем найти старый план сборки
                if (!tryReadMapPlan)
                {
                    stitchingBlock.FindKeyPoints(_context);  // Если плана нет, запускаем создние нового
                    if (AssemblyPlan.DefaultParameters || (AssemblyPlan.Percent && AssemblyPlan.From == 0 && AssemblyPlan.To == 100))stitchingBlock.TrySaveMapPlan();
                }
            }
            if (stitchingBlock.Direction == null) return SetCriticalErr("CriticalErr Direction not detect !!!");

            var areasForDelet = stitchingBlock.FindeBlockForDelet();// Обнаружение и удаление областей с большим количеством ошибок
            if (areasForDelet.Count > 0) stitchingBlock.DeletAreas(areasForDelet);

            return true;
        }

        /// <summary> Поиск ключевых точек (Асинхронная версия) </summary>
        private async Task<bool> FindKeyPointsAsync()
        {
            if (AssemblyPlan.BitMap) stitchingBlock = new StitchingBlock(BitmapData,AssemblyPlan.Period);
            else stitchingBlock = new StitchingBlock(AssemblyPlan);
            stitchingBlock.ProcessChanged += worker_ProcessChang;
            stitchingBlock.TextChanged += worker_TextChang;
            stitchingBlock.NewEngin = AssemblyPlan.NewEngin;

            if (AssemblyPlan.BitMap) await Task.Run(() => { stitchingBlock.FindKeyPoints(_context); });
            else
            {
                bool tryReadMapPlan = false;
                if (stitchingBlock.IsErr) return SetErr(stitchingBlock.GetErrText());
                if (AssemblyPlan.ChekStitchPlan) tryReadMapPlan = stitchingBlock.TryReadMapPlan(AssemblyPlan.From, AssemblyPlan.To); // Если включенно пробуем найти старый план сборки
                if (!tryReadMapPlan)
                {
                    await Task.Run(() => { stitchingBlock.FindKeyPoints(_context); }); // Если плана нет, запускаем создние нового
                    if (AssemblyPlan.DefaultParameters || (AssemblyPlan.Percent && AssemblyPlan.From == 0 && AssemblyPlan.To == 100)) stitchingBlock.TrySaveMapPlan();
                }
            }
            if (stitchingBlock.Direction == null) return SetCriticalErr("CriticalErr Direction not detect !!!");

            var areasForDelet = stitchingBlock.FindeBlockForDelet();// Обнаружение и удаление областей с большим количеством ошибок
            if (areasForDelet.Count > 0) stitchingBlock.DeletAreas(areasForDelet);

            return true;
        }

        /// <summary> Сборка изображения по ранее найденным ключевым точкам</summary>
        private bool StitchImgs()
        {
            if (stitchingBlock != null)
            {
                RezultImg = new Mat();
                RezultImg = stitchingBlock.Stitch(_context, AssemblyPlan.Delta);
                FinalRezult.MatRezult = RezultImg;
                FinalRezult.BitRezult = BitmapConverter.ToBitmap(RezultImg);
                if (_context != null) _context.Send(OnImgUpdate, RezultImg);
                if (RezultImg.Width == 0 && RezultImg.Height == 0) return SetErr(stitchingBlock.GetErrText());
                if (stitchingBlock.IsErr && _context != null) _context.Send(OnRTBAddInfo, stitchingBlock.ErrText);
                return true;
            }
            else return SetErr("Err StitchingBlock = null !!!");
        }

        /// <summary> Сборка изображения по ранее найденным ключевым точкам 
        /// (Асинхронная версия)
        /// </summary>
        private async Task<bool> StitchImgsAsync()
        {
            if (stitchingBlock != null)
            {
                RezultImg = new Mat();
                await Task.Run(() => { RezultImg = stitchingBlock.Stitch(_context, AssemblyPlan.Delta);});
                if (_context != null) _context.Send(OnImgUpdate, RezultImg);
                if (RezultImg.Width == 0 && RezultImg.Height == 0) return SetErr(stitchingBlock.GetErrText());
                if (stitchingBlock.IsErr && _context != null) _context.Send(OnRTBAddInfo, stitchingBlock.ErrText);
                FinalRezult.MatRezult = RezultImg;
                FinalRezult.BitRezult = BitmapConverter.ToBitmap(RezultImg);
                return true;
            }
            else return SetErr("Err StitchingBlock = null !!!");
        }


        public bool CheckFixingImg(string inputDir, string outputDir)
        {
            if (string.IsNullOrEmpty(outputDir) || string.IsNullOrEmpty(inputDir)) return false;
            if (!Directory.Exists(inputDir) || !Directory.Exists(outputDir)) return false;

            FileInfo[] inputFileList = fileEdit.SearchFiles(inputDir);
            FileInfo[] outputFileList = fileEdit.SearchFiles(outputDir);
            if (inputFileList.Length != outputFileList.Length) return false;
            for (int i = 0; i < inputFileList.Length; i++)
                if (!File.Exists(outputDir + Path.DirectorySeparatorChar + inputFileList[i].Name)) return false;
            return true;
        }

        /// <summary> Очистка всех записей с предыдущей сборки</summary>
        public void ClearAll()
        {
            BitmapData = null;
            AssemblyPlan = null;
            SavedFileName = string.Empty;
            RezultImg = null;
            IsErr = false;
            IsCriticalErr = false;
            ErrText = string.Empty;
            EnumErrCode ErrCode = EnumErrCode.NoErr;
            ErrList.Clear();
            stitchingBlock = null;
        }
        /// <summary> Очистка всех записей об ошибках</summary>
        public void ClearErr()
        {
            IsErr = false;
            IsCriticalErr = false;
            ErrText = string.Empty;
            EnumErrCode ErrCode = EnumErrCode.NoErr;
            ErrList.Clear();
        }
    }
}