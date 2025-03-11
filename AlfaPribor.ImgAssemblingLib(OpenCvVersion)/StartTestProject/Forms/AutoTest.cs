using ImgAssemblingLib.Models;
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
        private int BtnId = 0;
        private List<AutoTestPlan> PlansList { get; set; }
        public static bool StopProcess { get; set; } = false;
        public AutoTest()
        {
            InitializeComponent();
            if (SynchronizationContext.Current != null) _context = SynchronizationContext.Current;
            else _context = new SynchronizationContext();
            fileEdit = new FileEdit();
            assembling = new Assembling(_context);

            //Plans();
            Plans();
            SetDefoltBtnColor();
            HideProgressBar();
            AddBtns();
        }

        private void Plans()
        {
            int i = 0;
            if(PlansList == null) PlansList = new List<AutoTestPlan>();
            else PlansList.Clear();

            PlansList.Add(new AutoTestPlan(i++, "TestingPlans\\BitMap.asp"));
            PlansList.Add(new AutoTestPlan(i++, "TestingPlans\\3179_3_0.asp"));
            PlansList.Add(new AutoTestPlan(i++, "TestingPlans\\16RC.asp"));
            PlansList.Add(new AutoTestPlan(i++, "TestingPlans\\336_5_1.asp"));
            PlansList.Add(new AutoTestPlan(i++, "TestingPlans\\Err.asp"));
            PlansList.Add(new AutoTestPlan(i++, "TestingPlans\\4.asp"));
            //PlansList.Add(new AutoTestPlan(i++, "TestingPlans\\Test.asp"));// слишком долгий
            //PlansList.Add(new AutoTestPlan(i++, "TestingPlans\\619.asp"));// слишком долгий
        }

        static IEnumerable<AutoTestPlan> Plans2()
        {
            int i = 0;
            yield return new AutoTestPlan(i++, "TestingPlans\\BitMap.asp");
            yield return new AutoTestPlan(i++, "TestingPlans\\Err.asp");
            yield return new AutoTestPlan(i++, "TestingPlans\\4.asp");
            yield return new AutoTestPlan(i++, "TestingPlans\\Test.asp");
            //yield return new Plan(i++, "TestingPlans\\16RC.asp");
            //yield return new Plan(i++, "TestingPlans\\336_5_1.asp");
            //yield return new Plan(i++, "TestingPlans\\3179_3_0.asp");
            //yield return new Plan(i++, "TestingPlans\\619.asp");
        }
        private void AddBtns()
        {
            if (PlansList == null) return;
            if (PlansList.Count() == 0) return;

            //var plans = PlansList.ToArray();
            int N = PlansList.Count();
            var dsf = PlansList[0].AssemblingFile;
            for (int i = 0; i < N; i++)
            {
                AddText(Path.GetFileNameWithoutExtension(PlansList[i].AssemblingFile));
                AddBtn();
                BtnId++;
            }
        }
        private void AddText(string file)
        {
            Label NewText = new Label();
            NewText.AutoSize = true;
            NewText.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(204)));
            NewText.Location = new Point(5, 23+43* BtnId);
            NewText.Name = "Test" + BtnId;
            NewText.Size = new Size(54, 17);
            NewText.Text = file.Length>7? file.Substring(0,7) : file;
            Controls.Add(NewText);
        }
        string message = "Simple MessageBox";
        private void AddBtn()
        {
            Button newBtn = new Button();
            newBtn.Location = new Point(80, 12 + 43 * BtnId);
            newBtn.Name = "FileTestBtn" + BtnId;
            newBtn.Size = new Size(75, 37);
            newBtn.UseVisualStyleBackColor = true;
            newBtn.Click += new EventHandler(this.ShowTestInfo);
            Controls.Add(newBtn);

            newBtn = new Button();
            newBtn.Location = new Point(161, 12 + 43 * BtnId);
            newBtn.Name = "ImgTestBtn" + BtnId;
            newBtn.Size = new Size(75, 37);
            newBtn.UseVisualStyleBackColor = true;
            newBtn.Click += new EventHandler(this.ShowTestInfo);
            Controls.Add(newBtn);
        }
        private void DelBtn() => DelBtn(--BtnId);
        private void DelBtn(int id)
        {
            DelBtn("ImgTestBtn" + id);
            DelBtn("FileTestBtn" + id);
        }
        private void DelBtn(string Name)=>Controls.RemoveByKey(Name);
        private void ShowTestInfo(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            string btnName = btn.Name;
            bool isFileTypeBtn = btnName.IndexOf("FileTestBtn") != -1 ? true : false;
            int id = -1;
            string strId = isFileTypeBtn ? btnName.Substring("FileTestBtn".Length) : btnName.Substring("ImgTestBtn".Length);
            int.TryParse(strId, out id);

            if (id == -1) return;
            //ChangBtnColor(isFileTypeBtn, id, Color.FromArgb(0, 64, 0));

            if (PlansList != null && PlansList.Count > 0)
            {
                FinalResult finalResult;
                if(isFileTypeBtn) finalResult = PlansList[id].FileFinalResult;
                else finalResult = PlansList[id].ImgFinalResult;

               
                if (finalResult == null) return;
                if (finalResult.IsCriticalErr)
                {
                    string errText = string.Empty;
                    if (finalResult.ErrList.Count != 0) 
                        foreach (string strErr in finalResult.ErrList) errText += "\n" + strErr;
                    else errText = finalResult.IsCriticalErr ? "CriticalErr " + finalResult.ErrText : (finalResult.IsErr ? "Err " + finalResult.ErrText : string.Empty);
                    MessageBox.Show(errText);
                }
                else if (finalResult.IsErr)
                {
                    string errText = finalResult.ErrText;
                    MessageBox.Show(errText);
                    if(finalResult.BitRezult != null && !string.IsNullOrEmpty(finalResult.RezulFileLink))
                        fileEdit.OpenFileDir(finalResult.RezulFileLink);
                }
                else if (finalResult.BitRezult != null && !string.IsNullOrEmpty(finalResult.RezulFileLink))
                    fileEdit.OpenFileDir(finalResult.RezulFileLink);
                
            }
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
            for (int i = 0; i < BtnId; i++)
            {
                ChangBtnColor(true, i, Color.FromArgb(240, 240, 240));
                ChangBtnColor(false, i, Color.FromArgb(240, 240, 240));
            }
        }
        private void Stop()
        {
            StopProcess = true;
            StitchingBlock.StopProcess = true;
            ImgFixingForm.StopProcess = true;
        }

        //private class temp
        //{
        //    public int I { get; set; }
        //    public int Progress { get; set; }
        //}
        //List<temp> temps = new List<temp>();
        private async Task<bool> StartTest()
        {

            if (PlansList == null) return false;
            if (PlansList.Count() == 0) return false;

            int i = 0, plansNumber = PlansList.Count(), progressMultiplier = 100 / plansNumber / 2;
            FinalResult fileFinalResult = new FinalResult();
            FinalResult imgFinalResult = new FinalResult();
            assembling.ProcessChanged += CurrentProcessChang;
            assembling.TextChanged += CurrentProgressLbChang;
            AssemblyPlan assemblyPlan;
            ShowProgressBar();

            //foreach (var plan in PlansList)
            for (int y =0; y<PlansList.Count;y++)
            {
                try
                {
                    if (StopProcess)
                    {
                        fileFinalResult.SetCriticalErr("Тест остановлен пользователем!");
                        imgFinalResult.SetCriticalErr("Тест остановлен пользователем!");
                        break;
                    }
                    //progress = i++* 100 / plansNumber / 2;
                    int progress = i++ * progressMultiplier;
                    SetGeneralProgressBar(progress);

                    if (string.IsNullOrEmpty(PlansList[y].AssemblingFile))
                    {
                        fileFinalResult.SetCriticalErr("Err PlansList[y].AssemblingFile IsNullOrEmpty!!!");
                        imgFinalResult.SetCriticalErr("Err PlansList[y].AssemblingFile IsNullOrEmpty!!!");
                        i++;
                        continue;
                    }

                    if (!File.Exists(PlansList[y].AssemblingFile))
                    {
                        fileFinalResult.SetCriticalErr($"Err file {PlansList[y].AssemblingFile} does not exist!!!");
                        imgFinalResult.SetCriticalErr($"Err file {PlansList[y].AssemblingFile} does not exist!!!");
                        i++;
                        continue;
                    }

                    fileEdit.LoadeJson(PlansList[y].AssemblingFile, out assemblyPlan); // Загружаем план сборки
                    assemblyPlan.ResetRezults();
                    assembling.ClearAll();
                    
                    if (assemblyPlan == null)
                    {
                        fileFinalResult.SetCriticalErr($"Err assemblyPlan = null !!!");
                        imgFinalResult.SetCriticalErr($"Err assemblyPlan = null !!!");
                        i++;
                        continue;
                    }

                    if (!string.IsNullOrEmpty(assemblyPlan.StitchingDirectory) && assemblyPlan.FixImg && Directory.Exists(assemblyPlan.StitchingDirectory))
                    {
                        if (!fileEdit.DelAll(assemblyPlan.StitchingDirectory))
                        {
                            fileFinalResult.SetErr($"Err file {PlansList[y].AssemblingFile} does not exist!!!");
                            imgFinalResult.SetErr($"Err file {PlansList[y].AssemblingFile} does not exist!!!");
                        }
                    }

                    assemblyPlan.BitMap = false;
                    assemblyPlan.SaveRezult = true;
                    assemblyPlan.ShowRezult = false;
                    assembling.ChangeAssemblyPlan(assemblyPlan);
                    assembling.CalculationSpeedDespiteErrors = true;
                    fileFinalResult = await assembling.TryAssembleAsync();
                    
                    // Тоже саме только с помощью масива битмапов
                    assemblyPlan.BitMap = true;
                    assemblyPlan.ResetRezults();
                    assemblyPlan.ShowRezult = notShowRezultChkBox.Checked ? false: true;
                    
                    assembling.ClearAll();
                    assembling.BitmapData = LoadeBitmap(assemblyPlan.WorkingDirectory);
                    assembling.ChangeAssemblyPlan(assemblyPlan);
                    assembling.CalculationSpeedDespiteErrors = true;
                    imgFinalResult = await assembling.TryAssembleAsync();

                    //progress = i++* 100 / plansNumber / 2;
                    progress = i++ * progressMultiplier;
                    SetGeneralProgressBar(progress);
                }
                catch (Exception ex)
                {
                    if (i % 2 == 0) imgFinalResult.SetCriticalErr($"Err {ex.Message}!!!");
                    else fileFinalResult.SetCriticalErr($"Err {ex.Message} !!!");
                }
                finally
                {
                    if (fileFinalResult == null || imgFinalResult == null)
                    {

                    }

                    if(fileFinalResult.BitRezult!=null && fileFinalResult.BitRezult.Width!= 0 && fileFinalResult.BitRezult.Height != 0)
                    {
                        string file = "tempPic" + FileId++ + ".jpg";
                        fileFinalResult.BitRezult.Save(file);
                        fileFinalResult.RezulFileLink = file;
                    }
                    if(imgFinalResult.BitRezult!=null && imgFinalResult.BitRezult.Width!= 0 && imgFinalResult.BitRezult.Height != 0)
                    {
                        string file = "tempPic" + FileId++ + ".jpg";
                        imgFinalResult.BitRezult.Save(file);
                        imgFinalResult.RezulFileLink = file;
                    }

                    PlansList[y].AddFileFinalResult(fileFinalResult);
                    PlansList[y].AddImgFinalResult(imgFinalResult);

                    ChangBtnColor(true, i / 2 - 1, fileFinalResult.GetColor());
                    ChangBtnColor(false, i / 2 -1, imgFinalResult.GetColor());

                    fileFinalResult.Clear();
                    imgFinalResult.Clear();
                }
            }
            HideProgressBar();
            //SetGeneralProgressBar(100);
            return true;
        }

        private int FileId { get; set; } = 0;

        private void ChangBtnColor(bool type, int id, Color color)
        {
            string btnName = type ? "FileTestBtn" + id : "ImgTestBtn" + id;
            var btn = this.Controls.Find(btnName, true).FirstOrDefault();
            if (btn != null) btn.BackColor = color;
        }
        private void ShowProgressBar()
        {
            GeneralProgressBar.Visible = true;
            CurentProgressBar.Visible = true;
            CurentProgressBar.SendToBack();
            GeneralProgressLb.Visible = true;
            CurrentProgressLb.Visible = true;
            GeneralProgressLb.BringToFront();
            CurrentProgressLb.BringToFront();

            //GeneralProgress2Lb.Visible = true;
            //GeneralProgressBar.SendToBack();
            //GeneralProgress2Lb.BringToFront();
        }

        private void HideProgressBar()
        {
            GeneralProgressBar.Visible = false;
            CurentProgressBar.Visible = false;
            CurrentProgressLb.Visible = false;
            GeneralProgressLb.Visible = false;

            //GeneralProgress2Lb.Visible = false;
        }
        private void SetGeneralProgressBar(int value)
        {
            GeneralProgressBar.Value = value;
            GeneralProgressLb.Text = value + " %";
            //GeneralProgress2Lb.Text = value + " %";
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
        private void CurrentProgressLbChang(string text)
        {
            CurrentProgressLb.Text = text;
            //CurrentProgress2Lb.Text = text;
        }

        //private void button1_Click(object sender, EventArgs e)
        //{
        //    assembling = new Assembling(_context);
        //   // string rezult = assembling.JpegTest();
        //}
    }
}