using ImgAssemblingLib.AditionalForms;
using ImgAssemblingLib.Models;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace StartTestProject
{
    public partial class TestingForm : Form
    {
        
        public TestingForm()
        {
            InitializeComponent();
            KeyPreview = true;

            //ShowMainForm();
            ShowEditingStitchingForm();
            //ShowImgFixingForm();
        }

        private void ShowMainForm()
        {
            Form1 imgFixingForm = new Form1();
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

        private string text = string.Empty;

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Left)
            {
                MessageBox.Show("You pressed Left arrow key");
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void TestingForm_KeyDown(object sender, KeyEventArgs e)
        {
            Button buttonToClick = null;

            text += e.KeyCode;
            string checkTxt = text.Length - 4 > 0 ? text.Skip(text.Length-4).Take(4).ToString() : string.Empty;
            var gdfg = text.Skip(text.Length - 4).Take(4);
            if (checkTxt == "test" || checkTxt == "TEST") ShowImgFixingForm();

            if (e.Control && e.KeyCode == Keys.S)
            {
                //RTB labelResult.Text = "Ctrl + S pressed";
                e.SuppressKeyPress = true; // Prevents the event from being passed to the control with focus
            }

            switch (e.KeyCode)
            {
                case Keys.Q:
                    //buttonToClick = button1;
                    break;
                case Keys.W:
                   // buttonToClick = button2;
                    break;
                case Keys.E:
                    //buttonToClick = button3;
                    break;
                case Keys.A:
                    //buttonToClick = button4;
                    break;
                case Keys.S:
                    //buttonToClick = button5;
                    break;
                case Keys.D:
                    //buttonToClick = button6;
                    break;
                case Keys.Z:
                    //buttonToClick = button7;
                    break;
                case Keys.X:
                    //buttonToClick = button8;
                    break; 
            }

            if (buttonToClick != null)
            {
                //HighlightButton(buttonToClick);
                buttonToClick.PerformClick();
            }

        }

        private void MainFormBtn_Click(object sender, EventArgs e)
        {
            ShowMainForm();
        }

        private void EditingStitchingPlanBtn_Click(object sender, EventArgs e)
        {
            ShowEditingStitchingForm();
        }

        private void ImgFixingFormBtn_Click(object sender, EventArgs e)
        {
            ShowImgFixingForm();
        }

        private FileEdit fileEdit = new FileEdit(new string[] { "*.jpeg", "*.jpg", "*.png", "*.bmp" });
        private void FixingImgsUsingDataArrayBtn_Click(object sender, EventArgs e)
        {
            //var stopwatch = new System.Diagnostics.Stopwatch();
            //stopwatch.Start();
            //TimeSpan ts = stopwatch.Elapsed;

            string ImgFixingPlan = "14.fip"; // Файл с параментрами корректировки изображений
            string WorkingDirectory = "D:\\Work\\Exampels\\14(3)"; // Папка изображений для испраления
            //string WorkingDirectory = "E:\All\Side1\Left"; // Папка изображений для испраления
            if (!fileEdit.ChkDir(WorkingDirectory)) return;

            //Для имитации загружаем файлы из папки и создаем массив битмапов
            FileInfo[] fileList = fileEdit.SearchFiles(WorkingDirectory);
            if (fileList.Length == 0) return;
            Bitmap[] dataArray = fileList.Select(x => { return new Bitmap(x.FullName); }).ToArray();

            ImgFixingForm imgFixingForm = new ImgFixingForm(ImgFixingPlan, false);
            var respBitmapArray = imgFixingForm.FixImgArray(dataArray);


            // Для проверки можно записать один файл из итогового массива
            // respArray[22].Save("test2022.jpg");

            //ts = stopwatch.Elapsed;
            //string text = String.Format("{0:00}:{1:00}:{2:00}", ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
        }
    }
}
