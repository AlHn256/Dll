using ImgAssemblingLibOpenCV.AditionalForms;
using ImgAssemblingLibOpenCV.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StartTestProject
{
    public partial class AutoTest : Form
    {
        private object _context;
        private FileEdit fileEdit;
        private Assembling assembling;
        public static bool StopProcess { get; set; } = false;
        public AutoTest()
        {
            InitializeComponent();
            if (SynchronizationContext.Current != null) _context = SynchronizationContext.Current;
            else _context = new SynchronizationContext();
            fileEdit = new FileEdit();
            assembling = new Assembling(_context);
            SetDefoltBtnColor();
            HideProgressBar();
        }


        private void button2_Click(object sender, EventArgs e)
        {
            DelBtn();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddBtn();
        }

        private int BtnId = 0;
        private void AddBtn()
        {
            Button newBtn = new Button();
            newBtn.Location = new Point(80, 12 + 43 * BtnId);
            newBtn.Name = "FileTestBtn" + BtnId;
            newBtn.TabIndex = BtnId++;
            newBtn.Size = new Size(75, 37);
            //newBtn.Text = Name;
            newBtn.UseVisualStyleBackColor = true;
            newBtn.Click += new EventHandler(this.ShowTestInfo);
            Controls.Add(newBtn);


            newBtn.Location = new Point(161, 12 + 43 * BtnId);
            newBtn.Name = "ImgTestBtn" + BtnId;
            newBtn.TabIndex = BtnId++;
            newBtn.Click += new EventHandler(this.ShowTestInfo);
            Controls.Add(newBtn);
        }

        private void DelBtn()=>DelBtn(BtnId--);
        private void DelBtn(int id)
        {
            DelBtn("ImgTestBtn" + id);
            DelBtn("FileTestBtn" + id);
        }

        private void DelBtn(string Name)=>Controls.RemoveByKey(Name);
        
        private void ShowTestInfo(object sender, EventArgs e)
        {

        }

        private class Plan
        {
            public int Id {  get; set; } = 0;
            public string AssemblingFile {  get; set; }
            public string ImgFixingFile { get; set; }
            public bool FileTestPassed { get; set; } = false;
            public Bitmap FileRezult { get; set; } 
            public bool ImgTestPassed { get; set; } = false;
            public Bitmap ImgRezult { get; set; }
            public int ISpeedRezult { get; set; }
            public int FSpeedRezult { get; set; }
            public Plan(int id , string assemblingFile)
            {
                Id=id;
                AssemblingFile = assemblingFile;
            }
            public bool IsErr { get; set; } = false;
            public bool IsCriticalErr { get; set; } = false;
            public string ErrText { get; set; } = string.Empty;
            public List<string> ErrList { get; set; } = new List<string>();
            public bool SetErr(string err)
            {
                IsErr = true;
                ErrText = err;
                return false;
            }
            public bool SetCriticalErr(string err)
            {
                IsErr = true;
                IsCriticalErr = true;
                ErrText = err;
                return false;
            }
        }
        static IEnumerable<Plan> Get()
        {
            int i = 0;
            //yield return new Plan(i++, "Test.oip");
            //yield return new Plan(i++,"16RC.asp");
            //yield return new Plan(i++, "4.asp");
            //yield return new Plan(i++,"336_5_1.asp");
            yield return new Plan(i++, "3179_3_0");
            yield return new Plan(i++, "Err.oip");
            yield return new Plan(i++, "BitMap.asp");
            yield return new Plan(i++, "619.asp");
        }

        private async void PuskBtn_Click(object sender, EventArgs e)
        {
            SetDefoltBtnColor();
            if (PuskBtn.Text == "Пуск")
            {
                PuskBtn.Text = "Стоп";
                StopProcess = false;
                await StartTest();
                PuskBtn.Text = "Пуск";
            }
            else
            {
                Stop();
                PuskBtn.Text = "Пуск";
            }
        }

        private void SetDefoltBtnColor()
        {
            Color color = Color.FromArgb(240, 240, 240);
            FileTest1Btn.BackColor = color;
            ImgTest1Btn.BackColor = color;
            FileTest2Btn.BackColor =color;
            ImgTest2Btn.BackColor = color;
            FileTest3Btn.BackColor =color;
            ImgTest3Btn.BackColor = color;
            FileTest4Btn.BackColor =color;
            ImgTest4Btn.BackColor = color;
        }
        private void Stop()
        {
            StopProcess = true;
            StitchingBlock.StopProcess = true;
            ImgFixingForm.StopProcess = true;
        }

        private class temp
        {
            public int I { get; set; }
            public int Progress { get; set; }
        }
        List<temp> temps = new List<temp>();
        private async Task<bool> StartTest()
        {
            temps.Clear();
            var plansList = Get();
            FinalResult fileFinalResult = new FinalResult();
            FinalResult imgFinalResult = new FinalResult();
            int i = 0;
            assembling.ProcessChanged += CurrentProcessChang;
            assembling.TextChanged += CurrentProgressLbChang;
            AssemblyPlan assemblyPlan;
            ShowProgressBar();

            foreach (var plan in plansList)
            {
                try
                {
                    if (StopProcess)
                    {
                        fileFinalResult.SetCriticalErr("Тест остановлен пользователем!");
                        imgFinalResult.SetCriticalErr("Тест остановлен пользователем!");
                        break;
                    }
                    int progress = i++ * 100 / plansList.Count() / 2;
                    temps.Add(new temp() { I = i, Progress = progress });
                    SetGeneralProgressBar(progress);

                    if (string.IsNullOrEmpty(plan.AssemblingFile))
                    {
                        fileFinalResult.SetCriticalErr("Err plan.AssemblingFile IsNullOrEmpty!!!");
                        imgFinalResult.SetCriticalErr("Err plan.AssemblingFile IsNullOrEmpty!!!");
                        i++;
                        continue;
                    }

                    if (!File.Exists(plan.AssemblingFile))
                    {
                        fileFinalResult.SetCriticalErr($"Err file {plan.AssemblingFile} does not exist!!!");
                        imgFinalResult.SetCriticalErr($"Err file {plan.AssemblingFile} does not exist!!!");
                        i++;
                        continue;
                    }

                    fileEdit.LoadeJson(plan.AssemblingFile, out assemblyPlan); // Загружаем план сборки
                    assemblyPlan.ResetRezults();
                    assembling.ClearAll();
                    assembling.ChangeAssemblyPlan(assemblyPlan);
                    if (assemblyPlan == null)
                    {
                        fileFinalResult.SetCriticalErr($"Err assemblyPlan = null !!!");
                        imgFinalResult.SetCriticalErr($"Err assemblyPlan = null !!!");
                        i++;
                        continue;
                    }

                    if (!string.IsNullOrEmpty(assemblyPlan.StitchingDirectory) && assemblyPlan.FixImg && Directory.Exists(assemblyPlan.StitchingDirectory))
                    {
                        if (!fileEdit.DelAll(assemblyPlan.StitchingDirectory)) plan.SetErr("Err StitchingDirectory не удален!");
                    }

                    assemblyPlan.SaveRezults = true;
                    assemblyPlan.ShowAssemblingFile = true;
                    assemblyPlan.BitMap = false;
                    assemblyPlan.Speed = 0;
                    fileFinalResult = await assembling.TryAssemble(); 

                    // Тоже саме только с помощью масива битмапов
                    assemblyPlan.BitMap = true;
                    assemblyPlan.ResetRezults();
                    assembling.ClearAll();
                    assembling.BitmapData = LoadeBitmap(assemblyPlan.WorkingDirectory);
                    assembling.ChangeAssemblyPlan(assemblyPlan);


                    imgFinalResult = await assembling.TryAssemble(); 
                    progress = i++* 100 / plansList.Count() / 2;
                    temps.Add(new temp() { I = i, Progress = progress });
                    SetGeneralProgressBar(progress);
                }
                catch (Exception ex)
                {
                    plan.SetCriticalErr($"Err {ex.Message} !!!");
                    if (i % 2 == 0)
                    {
                        imgFinalResult.ErrText = ex.ToString();
                        imgFinalResult.IsErr = true;
                    }
                    else
                    {
                        fileFinalResult.ErrText = ex.ToString();
                        fileFinalResult.IsErr = true;
                    }
                }
                finally
                {
                    int X = i / 2;

                    Color btnFileColor = fileFinalResult.GetColor();
                    Color btnImgColor = imgFinalResult.GetColor();

                    switch (X)
                    {
                        case 1:
                            FileTest1Btn.BackColor = btnFileColor;
                            ImgTest1Btn.BackColor = btnImgColor;
                            break;
                        case 2:
                            FileTest2Btn.BackColor = btnFileColor;
                            ImgTest2Btn.BackColor = btnImgColor;
                            break;
                        case 3:
                            FileTest3Btn.BackColor = btnFileColor;
                            ImgTest3Btn.BackColor = btnImgColor;
                            break;
                        case 4:
                            FileTest4Btn.BackColor = btnFileColor;
                            ImgTest4Btn.BackColor = btnImgColor;
                            break;
                    }

                    fileFinalResult.Clear();
                    imgFinalResult.Clear();

                    
                    //progressBar.Visible = false;
                    //progressBarLabel.Visible = false;
                }
            }
            HideProgressBar();
            SetGeneralProgressBar(100);
            return true;
        }

        private void ShowProgressBar()
        {
            GeneralProgressBar.Visible = true;
            GeneralProgressLb.Visible = true;
            CurentProgressBar.Visible = true;
            CurrentProgressLb.Visible = true;
        }

        private void HideProgressBar()
        {
            CurrentProgressLb.Visible = false;
            GeneralProgressLb.Visible = false;
            GeneralProgressBar.Visible =false;
            CurentProgressBar.Visible = false;
        }

        private void SetGeneralProgressBar(int value)
        {
            GeneralProgressBar.Value = value;
            GeneralProgressLb.Text = value + " %";
        }
        private Bitmap[] LoadeBitmap(string file, int N = 0)
        {
            FileInfo[] fileList;
            if (N > 0) fileList = fileEdit.SearchFiles(file).Take(N).ToArray();
            else fileList = fileEdit.SearchFiles(file);

            if (fileList.Length == 0) return new Bitmap[] { };
            return fileList.Select(x => { return new Bitmap(x.FullName); }).ToArray();
        }
        private void CurrentProcessChang(int progress)
        {
            if (progress < 0) CurentProgressBar.Value = 0;
            else if (progress > 100) CurentProgressBar.Value = 100;
            else CurentProgressBar.Value = progress;
        }
        private void CurrentProgressLbChang(string text) => CurrentProgressLb.Text = text;

       
    }
}