using ImgAssemblingLibOpenCV.AditionalForms;
using ImgAssemblingLibOpenCV.Models;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StartTestProject
{
    public partial class TestingForm : Form
    {
        private FileEdit fileEdit = new FileEdit(new string[] { "*.jpeg", "*.jpg", "*.png", "*.bmp" });
        private string consol = string.Empty;
        private object _context;
        private BackgroundWorker worker;
        private PerformanceCounter cpuCounter;
        private PerformanceCounter ramCounter;
        public TestingForm()
        {
            InitializeComponent();
            progressBar.Visible = false;
            progressBarLabel.Visible = false;
            if (SynchronizationContext.Current != null) _context = SynchronizationContext.Current;
            else _context = new SynchronizationContext();

            cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            ramCounter = new PerformanceCounter("Memory", "Available MBytes");

            worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = true;

            worker.DoWork += new DoWorkEventHandler(worker_DoWork);
            worker.ProgressChanged += new ProgressChangedEventHandler(worker_ProgressChanged);
            //worker.RunWorkerCompleted +=
            //           new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);

            //Thread.Sleep(1000);
            //RezultLb.Text = "CPU "+ getCurrentCpuUsage()+ " RAM " + getAvailableRAM();
            worker.RunWorkerAsync();
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            consol += keyData;
            if (consol.Length > 5)
            {
                consol = consol.Substring(consol.Length - 5);
                if (consol.IndexOf("IMG") != -1){ ShowImgFixingForm(); consol = string.Empty; }
                if (consol.IndexOf("MAIN") != -1){ ShowMainForm(); consol = string.Empty; }
                if (consol.IndexOf("EDIT") != -1){ShowEditingStitchingForm(); consol = string.Empty; }
                if (consol.IndexOf("AUTO") != -1){ OpenAutoTest(); consol = string.Empty; }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void OpenAutoTest()=>new AutoTest().ShowDialog();
        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)=>CpuLb.Text = "CPU " + e.ProgressPercentage + "% RAM " + getAvailableRAM();
        public string getAvailableRAM()=> ramCounter.NextValue() + "MB";
        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                Thread.Sleep(800);
                worker.ReportProgress((int)cpuCounter.NextValue());
            }
        }
        private void ShowMainForm()
        {
            MainForm imgFixingForm = new MainForm();
            imgFixingForm.ShowDialog();
        }
        private void ShowEditingStitchingForm()
        {
            EditingStitchingPlan imgFixingForm = new EditingStitchingPlan(new AssemblyPlan());
            imgFixingForm.ShowDialog();
        }
        private void ShowImgFixingForm()
        {
            ImgFixingForm imgFixingForm = new ImgFixingForm("D:\\Work\\Exampels\\15");
            imgFixingForm.ShowDialog();
        }
        private void MainFormBtn_Click(object sender, EventArgs e)=>ShowMainForm();
        private void EditingStitchingPlanBtn_Click(object sender, EventArgs e)=>ShowEditingStitchingForm();
        //private string assemblingFile = "D:\\Work\\C#\\Dll\\ImgAssemblingLib\\StartTestProject\\bin\\Debug\\3179.asp";
        private string assemblingFile = "TestingPlans\\16RC.asp";

        // Пример сборки изображения с использованием только файла плана сборки
        private async void Exampl1Btn_Click(object sender, EventArgs e)
        {
            RezultLb.Text = string.Empty;

            AssemblyPlan assemblyPlan; 
            fileEdit.LoadeJson(assemblingFile, out assemblyPlan); // Загружаем план сборки
            if (assemblyPlan == null)
            {
                RezultLb.Text = "ERR Exampl1Btn_Click.файл сборки не найден!!!";
                return;
            }
            // Для имитации загружаем файлы из папки и создаем массив битмапов
            Bitmap[] dataArray = LoadeBitmap("E:\\ImageArchive\\3179_3_0", 40);
            // Bitmap[] dataArray = LoadeBitmap("E:\\ImageArchive\\3179_3_2");
            Assembling assembling = new Assembling(assemblyPlan, dataArray, null); 

            // Запускаем сборку
            ShowResult(await new Assembling(assemblyPlan, dataArray, null).TryAssemble());
        }
        
        // Пример без исправления изображений
        private async void Exampl2Btn_Click(object sender, EventArgs e)
        {
            RezultLb.Text = string.Empty;

            AssemblyPlan assemblyPlan;
            fileEdit.LoadeJson(assemblingFile, out assemblyPlan);
            assemblyPlan.FixImg = false; // Отключаем исправление изображений
            // Bitmap[] dataArray = LoadeBitmap("E:\\ImageArchive\\3162_25_3AutoOut", 40);
            //Bitmap[] dataArray = LoadeBitmap("D:\\Work\\Exampels\\20Up");
            Bitmap[] dataArray = LoadeBitmap("D:\\Work\\Exampels\\15AutoOut");

            ShowResult(await new Assembling(assemblyPlan, dataArray, null).TryAssemble());
        }

        // Пример с установкой другого плана корректировки изображения
        private async void Exampl3Btn_Click(object sender, EventArgs e)
        {
            RezultLb.Text = string.Empty;

            AssemblyPlan assemblyPlan;
            fileEdit.LoadeJson(assemblingFile, out assemblyPlan);
            // Заменяем план корректировки изображений
            assemblyPlan.ImgFixingPlan = "D:\\Work\\C#\\Dll\\ImgAssemblingLib(OpenCvVersion)\\StartTestProject\\bin\\Debug\\TestingPlans\\4.oip";
            Bitmap[] dataArray = LoadeBitmap("E:\\ImageArchive\\4", 27);
            //Bitmap[] dataArray = LoadeBitmap("D:\\Work\\Exampels\\16RC");

            ShowResult(await new Assembling(assemblyPlan, dataArray, null).TryAssemble());
        }

        // Пример настройки смещения полосы сборки относительно центра катинки (иногда помогает избавиться от повторяющихся объектов на заднем фоне вроде столбов)
        private async void Exampl4Btn_Click(object sender, EventArgs e)
        {
            RezultLb.Text = string.Empty;
            AssemblyPlan assemblyPlan;
            fileEdit.LoadeJson(assemblingFile, out assemblyPlan);
            // Заменяем план корректировки изображений
            assemblyPlan.ImgFixingPlan = "D:\\Work\\C#\\Dll\\ImgAssemblingLib(OpenCvVersion)\\StartTestProject\\bin\\Debug\\4.oip";
            assemblyPlan.Delta = -120;// Смещяем полсу
            Bitmap[] dataArray = LoadeBitmap("E:\\ImageArchive\\4", 29);

            ShowResult(await new Assembling(assemblyPlan, dataArray, null).TryAssemble());
        }

        // Пример только с подсчетом скорости (без сборки изображения)
        private async void Exampl5Btn_Click(object sender, EventArgs e)
        {
            RezultLb.Text = string.Empty;
            AssemblyPlan assemblyPlan;
            fileEdit.LoadeJson(assemblingFile, out assemblyPlan);
            assemblyPlan.ImgFixingPlan = "D:\\Work\\C#\\Dll\\ImgAssemblingLib(OpenCvVersion)\\StartTestProject\\bin\\Debug\\TestingPlans\\4.oip";
            assemblyPlan.SaveRezult = false;
            assemblyPlan.ShowRezult = false;
            assemblyPlan.MillimetersInPixel = 5.5; // Количество мм в одном пикселе
            assemblyPlan.TimePerFrame  = 40; // Миллисекунд в одном кадре
            Bitmap[] dataArray = LoadeBitmap("E:\\ImageArchive\\4").Skip(10).Take(10).ToArray();

            ShowResult(await new Assembling(assemblyPlan, dataArray, null).TryAssemble());
        }

        // Пример заполнения параметров без загрузки файла сбоки
        private async void Exampl6Btn_Click(object sender, EventArgs e)
        {
            RezultLb.Text = string.Empty;
            AssemblyPlan assemblyPlan = new AssemblyPlan()
            {
                BitMap = true, // Включаем работу с массивом битмапов вместо файлов
                DelFileCopy = false, // Отключение удаления копий изображений
                ImgFixingPlan = "D:\\Work\\C#\\Dll\\ImgAssemblingLib(OpenCvVersion)\\StartTestProject\\bin\\Debug\\4.oip",
                Delta = -120, // Смещение полосы склейки
                SaveRezult = true,
                ShowRezult = true,
                SpeedCounting = true, // Включение подсчета скорости
                MillimetersInPixel = 5.5, // Количество мм в одном пикселе
                TimePerFrame = 40, // Милисекунд в одном кадре
                SelectSearchArea = true, // Для большей точности можно задать область поиска ключевых точек
                MaxHeight = 1630, // Настройка области поиска ключевых точек
                MinHeight = 1533,
                MaxWight = 766,
                MinWight = 112
            };
            
            Bitmap[] dataArray = LoadeBitmap("E:\\ImageArchive\\4", 27);

            ShowResult(await new Assembling(assemblyPlan, dataArray, null).TryAssemble());
        }

        // Пример коррекции изображений без сборки
        private async void FixingImgsUsingDataArrayBtn_Click(object sender, EventArgs e)
        {
            RezultLb.Text = string.Empty;
            //Для имитации загружаем файлы из папки и создаем массив битмапов
            Bitmap[] dataArray = LoadeBitmap("D:\\Work\\Exampels\\16RC");
            ImgFixingForm imgFixingForm = new ImgFixingForm("16RC.oip", false); // Загружаем файл с параментрами корректировки изображений
            if (imgFixingForm.IsErr){RezultLb.Text = imgFixingForm.ErrText;return;}

            await Task.Run(() => { dataArray = imgFixingForm.FixImges(_context, dataArray); });

            if (dataArray == null || dataArray.Length == 0 || imgFixingForm.IsErr)
            {
                if (imgFixingForm.IsErr) { RezultLb.Text = imgFixingForm.ErrText; return; }
                else RezultLb.Text = "ERR dataArray == null || dataArray.Length == 0 !!!";
                RezultLb.Text = imgFixingForm.ErrText;
                return;
            }
            // Для проверки можно записать один файл из итогового массива
            dataArray[22].Save("test2022.jpg");
            fileEdit.OpenFileDir("test2022.jpg");
        }
        private async void KeypointsAreaBtn_Click(object sender, EventArgs e)
        {
            RezultLb.Text = string.Empty;
            AssemblyPlan assemblyPlan;
            fileEdit.LoadeJson(assemblingFile, out assemblyPlan);
            assemblyPlan.ImgFixingPlan = "D:\\Work\\C#\\Dll\\ImgAssemblingLib(OpenCvVersion)\\StartTestProject\\bin\\Debug\\TestingPlans\\4.oip";
            Bitmap[] dataArray = LoadeBitmap("E:\\ImageArchive\\4", 27);

            assemblyPlan.SelectSearchArea = true; // Для большей точности можно задать область поиска ключевых точек
            assemblyPlan.MaxHeight = 1630;
            assemblyPlan.MinHeight = 1533;
            assemblyPlan.MaxWight = 766;
            assemblyPlan.MinWight = 11;
            Assembling assembling = new Assembling(assemblyPlan, dataArray, null);
            if (!await assembling.StartAssembling()) RezultLb.Text = assembling.ErrText;
        }
        // Пример c прогрессбаром
        private async void Exampl8Btn_Click(object sender, EventArgs e)=>await StartAssembling();
        private async Task<bool> StartAssembling()
        {
            RezultLb.Text = string.Empty;
            progressBar.Visible = true;
            progressBarLabel.Visible = true;
            progressBarLabel.BringToFront();

            try
            {
                AssemblyPlan assemblyPlan;
                fileEdit.LoadeJson(assemblingFile, out assemblyPlan); // Загружаем план сборки
                if (assemblyPlan == null)
                {
                    RezultLb.Text = "ERR Exampl1Btn_Click.файл сборки не найден!!!";
                    return false;
                }
                // Для имитации загружаем файлы из папки и создаем массив битмапов
                Bitmap[] dataArray = LoadeBitmap("E:\\ImageArchive\\3179_3_0", 40);

                // Bitmap[] dataArray = LoadeBitmap("E:\\ImageArchive\\3179_3_2");
                Assembling assembling = new Assembling(assemblyPlan, dataArray, _context); // Запускаем сборку
                assembling.ProcessChanged += worker_ProcessChang;
                assembling.TextChanged += worker_TextChang;

                FinalResult finalResult = await assembling.TryAssemble();
                if (finalResult.IsErr) RezultLb.Text = finalResult.ErrText;
            }
            catch (Exception ex){RezultLb.Text = ex.Message;}
            finally {
                progressBar.Visible = false;
                progressBarLabel.Visible = false;
            }
            return true;
        }
        private void ShowResult(FinalResult finalResult)
        {
            if (finalResult.IsErr)
            {
                string Err = string.Empty;
                if(finalResult.ErrList!= null && finalResult.ErrList.Count > 0) 
                    foreach (var err in finalResult.ErrList) Err += err + "\n";
                else Err= finalResult.ErrText;
                RezultLb.Text = Err;
            }
            else RezultLb.Text = RezultLb.Text = "Speed " + String.Format("{0:0.##}", finalResult.Speed) + " Km\\H";
        }
        private void worker_ProcessChang(int progress)
        {
            if (progress < 0) progressBar.Value = 0;
            else if (progress > 100) progressBar.Value = 100;
            else progressBar.Value = progress;
        }
        private void worker_TextChang(string text) => progressBarLabel.Text = text;
        private Bitmap[] LoadeBitmap(string file, int N = 0)
        {
            FileInfo[] fileList;
            if (N > 0) fileList = fileEdit.SearchFiles(file).Take(N).ToArray();
            else fileList = fileEdit.SearchFiles(file);

            if (fileList.Length == 0) return new Bitmap[] { };
            return fileList.Select(x => { return new Bitmap(x.FullName); }).ToArray();
        }
    }
}
