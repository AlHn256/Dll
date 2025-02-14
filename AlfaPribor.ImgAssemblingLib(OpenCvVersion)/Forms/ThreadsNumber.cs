using ImgAssemblingLibOpenCV.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImgAssemblingLibOpenCV.AditionalForms
{
    public partial class ThreadsNumber : Form
    {
        public int Threads { get; set; } = 1;
        public string DefoltDirectory { get; set; } = string.Empty;
        private FileEdit fileEdit;
        public bool IsErr { get; set; } = false;
        public string ErrText { get; set; } = string.Empty;
        public List<string> ErrList { get; set; }

        public ThreadsNumber(int threads = 1)
        {
            InitializeComponent();

            Threads = threads;
            RTB.Text = threads.ToString();

            fileEdit = new FileEdit();
            ErrList = new List<string>();
            DefoltDirectory = GetDefDirectoty();
            SettingsDirTxtBox.Text = DefoltDirectory;
            CheckFiles();
            RedrawBtns();
        }
        private bool SetErr (string err)
        {
            if(IsErr)ErrList.Add(err);
            ErrText = err;
            IsErr = true;
            return false;
        }
        private string GetDefoltImgFixingPlan(int planNumber) => GetDefDirectoty() + Path.DirectorySeparatorChar + "DefImgFixingPlan" + planNumber.ToString("D2") + ".oip";
        public string GetDefAssemblingPlanN(int planNumber) => GetDefDirectoty() + Path.DirectorySeparatorChar + "DefAssemlingPlan" + planNumber.ToString("D2") + ".asp";
        public string GetDefDirectoty()
        {
            if (string.IsNullOrEmpty(DefoltDirectory)) return Path.GetDirectoryName(fileEdit.GetDefoltDirectory());
            else return DefoltDirectory;
        }
        private bool RecreateFiles()
        {
            if (CheckDir())
            {
                //SettingsDirTxtBox.Text = GetDefDirectoty();
                if (CheckFiles()) RedrawBtns();
                else return ShowErr();
            }
            else return ShowErr();
            return true;
        }

        private bool CheckDir()
        {
            if (!fileEdit.ChkDir(DefoltDirectory))
            {
                IsErr = true;
                ErrText = "Не удалось открыть папку : " + DefoltDirectory;
            }
            return true;
        }

        private bool CheckFiles()
        {
            for (int i = 0; i < Threads; i++)
            {
                string file = GetDefAssemblingPlanN(i);
                if (!fileEdit.ChkFile(file)) return SetErr(file + " не создан");

                if (new FileInfo(file).Length == 0)
                {
                    AssemblyPlan assemblyPlan = new AssemblyPlan();
                    // !!! Временно вклчена имитацияполучения потока изображения с камеры или массива кадров !!!
                    if (i < 4) assemblyPlan.WorkingDirectory = "E:\\ImageArchive\\3156_1_" + i.ToString();
                    else if (i == 4) assemblyPlan.WorkingDirectory = "E:\\ImageArchive\\4";
                    else if (i > 4)
                    {
                        assemblyPlan.WorkingDirectory = "E:\\ImageArchive\\3156_6_" + (i - 5).ToString();

                        if (i == 10) assemblyPlan.WorkingDirectory = "E:\\ImageArchive\\941_1_1";
                        if (i == 11) assemblyPlan.WorkingDirectory = "E:\\ImageArchive\\941_1_1AutoOut";
                        if (i == 12) assemblyPlan.WorkingDirectory = "E:\\ImageArchive\\941_1_1JPG";
                        if (i == 13) assemblyPlan.WorkingDirectory = "E:\\ImageArchive\\941_1_1AutoJPG";
                    }

                    assemblyPlan.StitchingDirectory = assemblyPlan.WorkingDirectory;
                    fileEdit.SaveJson(file, assemblyPlan);
                }
            }
            return true;
        }

        private void DelBtn_Click(object sender, EventArgs e) => ChangThreads(false);
        private void ChangThreads(bool increase)
        {
            if (increase) Threads++;
            else Threads--;
            if (Threads < 1) Threads = 1;

            if (RecreateFiles()) RedrawBtns();
        }

        private void AddBtn_Click(object sender, EventArgs e) => ChangThreads(true);
        private void OpenSettingsDirBtn_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog FBD = new FolderBrowserDialog();
            FBD.SelectedPath = DefoltDirectory;
            if (FBD.ShowDialog() == DialogResult.OK)
            {
                DefoltDirectory = FBD.SelectedPath;
                SettingsDirTxtBox.Text = FBD.SelectedPath;
                CheckFiles();
            }
        }

        private void RedrawBtns()
        {
            RTB.Text = Threads.ToString();
            DelAllBtn();
            for (int i = 0; i < Threads; i++)
            {
                string defAssemblingPlanPath = GetDefAssemblingPlanN(i);
                if (!fileEdit.ChkFile(defAssemblingPlanPath))
                {
                    SetErr(defAssemblingPlanPath + " не создан");
                }

                Label label = new Label();
                label.Location = new Point(12, 160 + 25 * i);
                label.Name = "labelBox" + i;
                label.Size = new Size(220, 20);
                label.TabIndex = i;
                label.Text = Path.GetFileName(defAssemblingPlanPath);
                Controls.Add(label);

                Button btn = new Button();
                btn.Location = new Point(240, 157 + 25 * i);
                btn.Name = "EditBtn" + i;
                btn.Size = new Size(90, 22);
                btn.Text = "Исправить план";
                btn.Click += new EventHandler(this.EditPlan);
                Controls.Add(btn);

                Button btn2 = new Button();
                btn2.Location = new Point(330, 157 + 25 * i);
                btn2.Name = "StartBtn" + i;
                btn2.Size = new Size(90, 22);
                btn2.Text = "Собрать";
                btn2.Click += new EventHandler(this.StartAssemling);
                Controls.Add(btn2);
            }
        }

        private async void StartAssemling(object sender, EventArgs e)
        {
            ShowMessage(string.Empty, false);
            if (sender is Button)
            {
                Button btn = (Button)sender;
                if (btn.Name.IndexOf("StartBtn") == 0)
                {
                    string AssemlingFile = GetDefAssemblingPlanN(GetBtnNumber(btn, "StartBtn"));
                    if (!await StartAssembling(AssemlingFile)) ShowErr();
                }
            }
        }
        private async Task<bool> StartAssembling(string AssemlingFile)
        {
            try
            {
                Assembling assembling = new Assembling(AssemlingFile);
                // !!!Временно для загрузи изображений подгружется массив кадров из папки
                //assembling.BitmapData = GetData(assembling.W)
                if(string.IsNullOrEmpty(assembling.AssemblyPlan.WorkingDirectory)) return false;
                assembling.BitmapData = LoadeBitmap(assembling.AssemblyPlan.WorkingDirectory);
                if (assembling.BitmapData.Length == 0) return SetErr("Массив с кадрами пуст");
                
                assembling.CheckPlane();
                if (assembling.IsErr) return SetErr(assembling.ErrText);

                FinalResult finalResult = await assembling.TryAssemble();
                if (finalResult.IsErr) return SetErr(assembling.ErrText);
            }
            catch (Exception ex) {return SetErr(ex.Message); }
            return true;
        }

        private Bitmap[] LoadeBitmap(string file, int N = 0)
        {
            FileInfo[] fileList;
            if (N > 0) fileList = fileEdit.SearchFiles(file).Take(N).ToArray();
            else fileList = fileEdit.SearchFiles(file);

            if (fileList.Length == 0) return new Bitmap[] { };
            return fileList.Select(x => { return new Bitmap(x.FullName); }).ToArray();
        }

        private void EditPlan(object sender, EventArgs e)
        {
            if (sender is Button)
            {
                Button btn = (Button)sender;
                int PlanNumber = GetBtnNumber(btn, "EditBtn");
                if (PlanNumber == -1) return;

                EditingStitchingPlan editingStitchingPlan = new EditingStitchingPlan(GetDefAssemblingPlanN(PlanNumber), GetDefoltImgFixingPlan(PlanNumber));
                //editingStitchingPlan.DefoltImgFixingPlan = GetDefoltImgFixingPlan(PlanNumber);
                editingStitchingPlan.Show();
            }
        }

        private int GetBtnNumber(Button btn, string serchText)
        {
            int PlanNumber = -1;
            if (btn.Name.IndexOf(serchText) == -1) return PlanNumber;
            string number = btn.Name.Substring(serchText.Length);
            int.TryParse(number, out PlanNumber);
            return PlanNumber;
        }

        private void DelAllBtn()
        {
            List<Control> controlList = new List<Control>();
            foreach (Control c in this.Controls) controlList.Add(c);
            foreach (Control c in controlList)
                if (c is Label || c is Button)
                {
                    if (c is Label)
                    {
                        Label label = (Label)c;
                        if (label.Name != "FirstLabel") this.Controls.Remove(c);
                    }

                    if (c is Button)
                    {
                        Button btn = (Button)c;
                        if (btn.Name.IndexOf("EditBtn") == 0 || btn.Name.IndexOf("StartBtn") == 0) this.Controls.Remove(c);
                    }
                }
        }
        private bool ShowErr(string errText = null) => ShowMessage(string.IsNullOrEmpty(errText) ? ErrText : errText, true);
        private bool ShowMessage(string errText = null, bool isErr = false)
        {
            Label label;
            string text = string.Empty;
            if (isErr) text = string.IsNullOrEmpty(errText) ? "Err :" + ErrText + "!!!" : "Err :" + errText + "!!!";
            else text = string.IsNullOrEmpty(errText) ? ErrText : errText;

            List<Control> controlList = new List<Control>();
            foreach (Control c in this.Controls) controlList.Add(c);
            foreach (Control c in controlList)
                if (c is Label)
                {
                    label = (Label)c;
                    if (label.Name == "ErrLab")
                    {
                        label.Text = text;
                        return true;
                    }
                }

            label = new Label();
            label.Location = new Point(12, 95);
            label.Name = "ErrLab";
            label.Size = new Size(400, 20);
            label.Text = text;
            Controls.Add(label);
            return false;
        }
    }
}