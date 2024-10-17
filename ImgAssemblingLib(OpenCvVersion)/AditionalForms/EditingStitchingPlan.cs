using ImgAssemblingLibOpenCV.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImgAssemblingLibOpenCV.AditionalForms
{
    public partial class EditingStitchingPlan : Form
    {
        private AssemblyPlan AssemblyPlan;
        private bool PersentOnOff = true;
        private FileEdit fileEdit = new FileEdit();
        public bool PlanIsUpDate = true;
        private Object _context;
        public bool IsErr { get; set; } = false;
        public string ErrText { get; set; } = string.Empty;
        public List<string> ErrList { get; set; } = new List<string>();
        private bool SetErr(string err)
        {
            if (ErrText != null) ErrList.Add(err);
            ErrText = err;
            return false;
        }

        //private bool ClearErr()
        //{
        //    ErrText = string.Empty;
        //    ErrList.Clear();
        //    return false;
        //}
        //public EditingStitchingPlan(string assemblingFile)
        //{
        //    InitializeComponent();
        //    if (string.IsNullOrEmpty(assemblingFile))
        //    {
        //        SetErr("Err EditingStitchingPlan.AssemblingFile IsNullOrEmpty!!!");
        //        return;
        //    }
            
        //    if(!File.Exists(assemblingFile))
        //    {
        //        SetErr("Err EditingStitchingPlan.AssemblingFile: "+ assemblingFile + " not founded!!!");
        //        return;
        //    }
        //    LoadAssemblyPlan(assemblingFile);

        //    if(AssemblyPlan==null)
        //    {
        //        SetErr("Err EditingStitchingPlan.AssemblyPlan don't loaded!!!");
        //        return;
        //    }
        //    OpenResultChckBox.Checked = (AssemblyPlan.SaveRezults && AssemblyPlan.ShowAssemblingFile);
        //}
        public EditingStitchingPlan(AssemblyPlan assemblyPlan)
        {
            InitializeComponent();
            if (SynchronizationContext.Current != null) _context = SynchronizationContext.Current;
            else _context = new SynchronizationContext();

            if (assemblyPlan == null) AssemblyPlan = new AssemblyPlan();
            AssemblyPlan = assemblyPlan;
            LoadSettings();

            WorkingDirectoryTxtBox.AllowDrop = true;
            WorkingDirectoryTxtBox.DragDrop += WindowsForm_DragDrop;
            WorkingDirectoryTxtBox.DragEnter += WindowsForm_DragEnter;

            ImgFixingPlanTxtBox.AllowDrop = true;
            ImgFixingPlanTxtBox.DragDrop += ImgFixingPlanTxtBox_DragDrop;
            ImgFixingPlanTxtBox.DragEnter += ImgFixingPlanTxtBox_DragEnter;

            ToolTip toolTip1 = new ToolTip();
            toolTip1.SetToolTip(SavingImgWBitmapChckBox, "Принудительная запись исправленных изображений в файлы");
        }
        public AssemblyPlan GetAssemblingPlan() => AssemblyPlan;
        void WindowsForm_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }
        void WindowsForm_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (fileEdit.IsDirectory(files[0])) WorkingDirectoryTxtBox.Text = files[0];
            else WorkingDirectoryTxtBox.Text = Path.GetDirectoryName(files[0]);
        }
        void ImgFixingPlanTxtBox_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }
        void ImgFixingPlanTxtBox_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            var sdf = files[0];
            var ggs = Path.GetExtension(files[0]);
            files = files.Where(x => Path.GetExtension(x) == ".oip").ToArray();
            if (files.Length > 0) ImgFixingPlanTxtBox.Text = files[0];
            else ImgFixingPlanTxtBox.Text = string.Empty;
        }
        private void LoadSettings()
        {
            BitMapChckBox.Checked = AssemblyPlan.BitMap;
            if (BitMapChckBox.Checked)  SavingImgWBitmapChckBox.Checked = AssemblyPlan.SaveImgFixingRezultToFile;
            CheckBitMap();

            WorkingDirectoryTxtBox.Text = AssemblyPlan.WorkingDirectory;
            FixImgChckBox.Checked = AssemblyPlan.FixImg;
            FixingImgDirectoryTxtBox.Text = AssemblyPlan.FixingImgDirectory;

            ImgFixingPlanTxtBox.Text = AssemblyPlan.ImgFixingPlan;
            EnableFixImgPanel(FixImgChckBox.Checked);
            AutoChckBox.Checked = true;
            AutoChckBoxInvok();

            FindKeyPointsСhckBox.Checked = AssemblyPlan.FindKeyPoints;
            StitchСhckBox.Checked = AssemblyPlan.Stitch;
            StitchingDirectoryTxtBox.Text = AssemblyPlan.StitchingDirectory;
            ChekFixedImgsChckBox.Checked = AssemblyPlan.ChekFixImg;
            ChekStitchPlanСhckBox.Checked = AssemblyPlan.ChekStitchPlan;
            DefaultParametersCheckBox.Checked = AssemblyPlan.DefaultParameters;
            if(AssemblyPlan.DefaultParameters)
            {
                AssemblyPlan.Delta = 0;
                DeltaTxtBox.Text = "0";
                PeriodTxtBox.Text = "1";
                AssemblyPlan.Period = 1;
                FromTxtBox.Text = "0";
                AssemblyPlan.From = 0;
                ToTxtBox.Text = "100";
                AssemblyPlan.To = 100;
                PersentOnOff = true;
            }
            else
            {
                DeltaTxtBox.Text = AssemblyPlan.Delta.ToString();
                PeriodTxtBox.Text = AssemblyPlan.Period.ToString();
                FromTxtBox.Text = AssemblyPlan.From.ToString();
                ToTxtBox.Text = AssemblyPlan.To.ToString();
            }
            
            PersentInvok();
            AdditionalFilterChckBox.Checked = AssemblyPlan.AdditionalFilter;
            SaveResultChckBox.Checked = AssemblyPlan.SaveRezult;
            SpeedCountingСhckBox.Checked = AssemblyPlan.SpeedCounting;
            OpenResultChckBox.Checked = AssemblyPlan.ShowRezult;

            MillimetersInPixelTxtBox.Text = AssemblyPlan.MillimetersInPixel.ToString();
            TimePerFrameTxtBox.Text = AssemblyPlan.TimePerFrame.ToString();
        }
        private void UpdateAssemblyPlan()
        {
            AssemblyPlan.BitMap = BitMapChckBox.Checked;
            if(BitMapChckBox.Checked) AssemblyPlan.SaveImgFixingRezultToFile = SavingImgWBitmapChckBox.Checked;
            AssemblyPlan.WorkingDirectory = WorkingDirectoryTxtBox.Text;
            AssemblyPlan.FixImg = FixImgChckBox.Checked;
            AssemblyPlan.FixingImgDirectory = FixingImgDirectoryTxtBox.Text;
            AssemblyPlan.ImgFixingPlan = ImgFixingPlanTxtBox.Text;
            AssemblyPlan.ChekFixImg = ChekFixedImgsChckBox.Checked;

            AssemblyPlan.FindKeyPoints = FindKeyPointsСhckBox.Checked;
            AssemblyPlan.AdditionalFilter = AdditionalFilterChckBox.Checked;

            AssemblyPlan.DefaultParameters = DefaultParametersCheckBox.Checked;
            AssemblyPlan.Percent = label6.Visible;
            if (DefaultParametersCheckBox.Checked)
            {
                AssemblyPlan.Period = 1;
                AssemblyPlan.Delta = 0;
                AssemblyPlan.From = 0;
                AssemblyPlan.To = 100;
            }
            else
            {
                int period = 1, delta = 0, from = 0, to = 100;
                Int32.TryParse(PeriodTxtBox.Text, out period);
                Int32.TryParse(DeltaTxtBox.Text, out delta);
                Int32.TryParse(FromTxtBox.Text, out from);
                Int32.TryParse(ToTxtBox.Text, out to);
                AssemblyPlan.Period = period;
                AssemblyPlan.Delta = delta;
                AssemblyPlan.From = from;
                AssemblyPlan.To = to;
            }

            AssemblyPlan.Stitch = StitchСhckBox.Checked;
            AssemblyPlan.ChekStitchPlan = ChekStitchPlanСhckBox.Checked;
            AssemblyPlan.StitchingDirectory = StitchingDirectoryTxtBox.Text;

            AssemblyPlan.SaveRezult = SaveResultChckBox.Checked;
            AssemblyPlan.SpeedCounting = SpeedCountingСhckBox.Checked;
            AssemblyPlan.ShowRezult = OpenResultChckBox.Checked;

            double millimetersInPixel = 0, timePerFrame = 0;
            double.TryParse(MillimetersInPixelTxtBox.Text, out millimetersInPixel);
            double.TryParse(TimePerFrameTxtBox.Text, out timePerFrame);
            AssemblyPlan.MillimetersInPixel = millimetersInPixel;
            AssemblyPlan.TimePerFrame = timePerFrame;
        }
        private void AutoChckBox_CheckedChanged(object sender, EventArgs e) => FixStitchingDirectoryTxtBox();
        private void AutoChckBoxInvok()
        {
            if (AutoChckBox.Checked) FixingImgDirectoryTxtBox.Text = AssemblyPlan.WorkingDirectory + "AutoOut";
            else FixingImgDirectoryTxtBox.Text = AssemblyPlan.FixingImgDirectory;
        }
        private void FixImgChckBox_CheckedChanged(object sender, EventArgs e)
        {
            AssemblyPlan.FixImg = FixImgChckBox.Checked;
            EnableFixImgPanel(FixImgChckBox.Checked);
            FixStitchingDirectoryTxtBox();
        }
        private void EnableFixImgPanel(bool Enabled)
        {
            ChekFixedImgsChckBox.Enabled = Enabled;
            AutoChckBox.Enabled = Enabled;
            FixingImgDirectoryTxtBox.Enabled = Enabled;
            ImgFixingPlanTxtBox.Enabled = Enabled;
            OpenImgFixingPlanBtn.Enabled = Enabled;
            label2.Enabled = Enabled;
            label3.Enabled = Enabled;
        }

        private void FixStitchingDirectoryTxtBox()
        {
            if (string.IsNullOrEmpty(WorkingDirectoryTxtBox.Text)) return;
            if (FixImgChckBox.Checked && AutoChckBox.Checked)
            {
                FixingImgDirectoryTxtBox.Text = WorkingDirectoryTxtBox.Text + "AutoOut";
                StitchingDirectoryTxtBox.Text = FixingImgDirectoryTxtBox.Text;
            }
            //else if (FixImgChckBox.Checked)
            //{
            //    FixingImgDirectoryTxtBox.Text = string.Empty;
            //    StitchingDirectoryTxtBox.Text = string.Empty;
            //}
            else StitchingDirectoryTxtBox.Text = WorkingDirectoryTxtBox.Text;
        }

        private void WorkingDirectoryTxtBox_TextChanged(object sender, EventArgs e) => FixStitchingDirectoryTxtBox();
        private void SaveToBtn_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "Assembling file plan (*.asp)|*.asp|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                UpdateAssemblyPlan();
                if (fileEdit.SaveJson(saveFileDialog1.FileName, AssemblyPlan)) InfoLabel.Text = "File saved: " + saveFileDialog1.FileName;
                else InfoLabel.Text = "Saving Err: " + saveFileDialog1.FileName+"!!!";
            }
        }
        private void LoadBtn_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                var dir = fileEdit.GetDefoltDirectory();
                //openFileDialog.InitialDirectory = "D:\\Work\\C#\\ImageStitching\\bin\\Debug\\net8.0-windows";
                openFileDialog.Filter = "Assembling file plan (*.asp)|*.asp|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    LoadAssemblyPlan(openFileDialog.FileName);
                    if(AssemblyPlan!=null) LoadSettings();
                    //AssemblyPlan assemblyPlan;
                    //fileEdit.LoadeJson(openFileDialog.FileName, out assemblyPlan);
                    //if (assemblyPlan != null)
                    //{
                    //    AssemblyPlan = assemblyPlan;
                    //    LoadSettings();
                    //}
                }
            }
        }
        private AssemblyPlan LoadAssemblyPlan(string file)
        {
            AssemblyPlan assemblyPlan;
            fileEdit.LoadeJson(file, out assemblyPlan);
            if (assemblyPlan != null)AssemblyPlan = assemblyPlan;
            return assemblyPlan;
        }
        private void OpenImgFixingPlanBtn_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                FileEdit fileEdit = new FileEdit();
                openFileDialog.InitialDirectory = fileEdit.GetDefoltDirectory();
                openFileDialog.Filter = "fixing img plan (*.oip)|*.oip|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK) ImgFixingPlanTxtBox.Text = openFileDialog.FileName;
            }
        }
        private void LoadParametrsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (DefaultParametersCheckBox.Checked)
            {
                PeriodTxtBox.Text = "1";
                PeriodTxtBox.Enabled = false;
                DeltaTxtBox.Text = "0";
                DeltaTxtBox.Enabled = false;
                FromTxtBox.Text = "0";
                FromTxtBox.Enabled = false;
                ToTxtBox.Text = "100";
                ToTxtBox.Enabled = false;
                PeriodTxtBox.Enabled = false;
                DeltaTxtBox.Enabled = false;
                FromTxtBox.Enabled = false;
                ToTxtBox.Enabled = false;
                label5.Visible = true;
                label5.Enabled = false;
                label6.Visible = true;
                label6.Enabled = false;
                label7.Enabled = false;
                label8.Enabled = false;
                FrLb.Enabled = false;
                ToLb.Enabled = false;
            }
            else
            {
                PeriodTxtBox.Enabled = true;
                DeltaTxtBox.Enabled = true;
                FromTxtBox.Enabled = true;
                ToTxtBox.Enabled = true;
                label5.Enabled = true;
                label6.Enabled = true;
                label7.Enabled = true;
                label8.Enabled = true;
                FrLb.Enabled = true;
                ToLb.Enabled = true;
            }
        }

        private void PersentInvok()
        {
            if (DefaultParametersCheckBox.Checked) return;
            PersentOnOff = !PersentOnOff;
            if (PersentOnOff)
            {
                label5.Visible = true;
                label6.Visible = true;
            }
            else
            {
                label5.Visible = false;
                label6.Visible = false;
            }
        }

        private void StitchСhckBox_CheckedChanged(object sender, EventArgs e) => CheckFKPoins();
        private void SpeedCountingСhckBox_CheckedChanged(object sender, EventArgs e) => CheckFKPoins();

        private void CheckFKPoins()
        {
            if (SpeedCountingСhckBox.Checked || StitchСhckBox.Checked)
            {
                FindKeyPointsСhckBox.Checked = true;
                FindKeyPointsСhckBox.Enabled = false;
            }
            else FindKeyPointsСhckBox.Enabled = true;
        }
        private void MillimetersInPixelTxtBox_TextChanged(object sender, EventArgs e)
        {
            int SelectionStart = MillimetersInPixelTxtBox.SelectionStart;
            string checkString = CheckText(MillimetersInPixelTxtBox.Text);
            if (MillimetersInPixelTxtBox.Text != checkString) MillimetersInPixelTxtBox.Text = checkString;
            MillimetersInPixelTxtBox.SelectionStart = SelectionStart;
        }

        private void TimePerFrameTxtBox_TextChanged(object sender, EventArgs e)
        {
            int SelectionStart = TimePerFrameTxtBox.SelectionStart;
            string checkString = CheckText(TimePerFrameTxtBox.Text);
            if (TimePerFrameTxtBox.Text != checkString) TimePerFrameTxtBox.Text = checkString;
            TimePerFrameTxtBox.SelectionStart = SelectionStart;
        }

        private string CheckText(string checkString)
        {
            if (checkString.IndexOf(',') != -1) checkString = checkString.Replace('.', ',');
            checkString = RemoveNonNumbers(checkString);
            if (string.IsNullOrEmpty(checkString)) checkString = "1";
            return checkString;
        }
        private string RemoveNonNumbers(string txt)
        {
            List<char> charList = new List<char>();
            foreach (char c in txt) if (Char.IsDigit(c) || c == ',') charList.Add(c);
            return new string(charList.ToArray());
        }

        //private async void StartBtn_Click(object sender, EventArgs e) => await StartAssembling();

        //private async void Test_Click(object sender, EventArgs e)
        //{
        //    await StartAssembling();
        //}
        public async Task<bool> StartAssembling(Bitmap[] bitmapArray)
        {
            if (bitmapArray.Length == 0) return SetErr("Err StartAssembling.bitmapArray = 0!!!");
            Assembling assembling = new Assembling(AssemblyPlan, bitmapArray, _context);
            if (await assembling.StartAssembling()) return true; // Запуск сборки изображения
            else return SetErr(assembling.ErrText);
        }
        //private async Task<bool> StartAssembling()
        //{
        //    if (AssemblyPlan == null)AssemblyPlan = new AssemblyPlan();
        //    UpdateAssemblyPlan(); // Подготовка плана сборки
        //    InfoLabel.Text = "Start Assembling";
            
        //    Assembling assembling;
        //    if (BitMapChckBox.Checked) // Вариант сборки с помощью массива битмапов
        //    {
        //        string ImgFixingPlan = string.IsNullOrEmpty(ImgFixingPlanTxtBox.Text) ? string.Empty : ImgFixingPlanTxtBox.Text; // Файл с параментрами корректировки изображений
        //        string WorkingDirectory = string.IsNullOrEmpty(WorkingDirectoryTxtBox.Text) ? string.Empty : WorkingDirectoryTxtBox.Text; // Папка изображений для испраления
        //                                                                                                                                  //string WorkingDirectory = "E:\All\Side1\Left"; // Папка изображений для испраления
        //        if (!fileEdit.ChkDir(WorkingDirectory)) return false;
        //        //Для имитации загружаем файлы из папки и создаем массив битмапов
        //        FileInfo[] fileList = fileEdit.SearchFiles(WorkingDirectory);
        //        if (fileList.Length == 0) return false;
        //        Bitmap[] dataArray = fileList.Select(x => { return new Bitmap(x.FullName); }).ToArray();
        //        assembling = new Assembling(AssemblyPlan, dataArray, _context);
        //        assembling.SaveImgFixingRezultToFile = SavingImgWBitmapChckBox.Checked;
        //    } // Вариант сборки через файлы или ссылки на них
        //    else assembling = new Assembling(AssemblyPlan, _context);
            
        //    assembling.RTBAddInfo += rtbText_UpDateInfo;
        //    if (await assembling.StartAssembling())// Запуск сборки изображения
        //    {
        //        InfoLabel.Text = "Assembling is finished!";
        //        if (OpenResultChckBox.Checked)
        //        {
        //            string SavedFileName = assembling.GetSavedFileName();
        //            if (!string.IsNullOrEmpty(SavedFileName)) fileEdit.OpenFileDir(SavedFileName);
        //        }
        //        return true;
        //    }
        //    else
        //    {
        //        InfoLabel.Text = assembling.ErrText;
        //        return false;
        //    }
        //}
        private void SaveResultChckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (!SaveResultChckBox.Checked) OpenResultChckBox.Checked = false;
        }
        private void OpenResultChckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (OpenResultChckBox.Checked) SaveResultChckBox.Checked = true;
        }
        private void SavingImgWBitmapChckBox_CheckedChanged(object sender, EventArgs e)
        {
            FixingImgDirectoryTxtBox.Enabled = SavingImgWBitmapChckBox.Checked;
            label2.Enabled = SavingImgWBitmapChckBox.Checked;
        }
        private void CheckBitMap()
        {
            if(!BitMapChckBox.Checked) SavingImgWBitmapChckBox.Checked = BitMapChckBox.Checked;
            SavingImgWBitmapChckBox.Enabled = BitMapChckBox.Checked;
            PeriodTxtBox.Enabled = !BitMapChckBox.Checked;
            FromTxtBox.Enabled = !BitMapChckBox.Checked;
            ToTxtBox.Enabled = !BitMapChckBox.Checked;
           
            FixingImgDirectoryTxtBox.Enabled = !BitMapChckBox.Checked;
            label2.Enabled = !BitMapChckBox.Checked;
            label5.Enabled = !BitMapChckBox.Checked;
            label6.Enabled = !BitMapChckBox.Checked;
            label7.Enabled = !BitMapChckBox.Checked;

            FrLb.Enabled = !BitMapChckBox.Checked;
            ToLb.Enabled = !BitMapChckBox.Checked;
            DefaultParametersCheckBox.Enabled = !BitMapChckBox.Checked;
            ChekStitchPlanСhckBox.Checked = !BitMapChckBox.Checked;
            ChekStitchPlanСhckBox.Enabled = !BitMapChckBox.Checked;

            DeltaTxtBox.Enabled = true;
            label8.Enabled = true;
        }
        private void BitMapChckBox_CheckedChanged(object sender, EventArgs e) => CheckBitMap();
        private void EditingStitchingPlan_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (PlanIsUpDate) UpdateAssemblyPlan();
        }
        private void ExitBtn_Click(object sender, EventArgs e)
        {
            PlanIsUpDate = false;
            Close();
        }
        private void label5_Click(object sender, EventArgs e) => PersentInvok();
        private void label6_Click(object sender, EventArgs e) => PersentInvok();
        private void EditingStitchingPlan_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.X > 305 && e.X < 333 && e.Y > 173 && e.Y < 233)
                PersentInvok();
        }

    }
}
