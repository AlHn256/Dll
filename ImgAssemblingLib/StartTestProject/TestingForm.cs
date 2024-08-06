using ImgAssemblingLib.AditionalForms;
using ImgAssemblingLib.Models;
using System;
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
            //ShowEditingStitchingForm();
            ShowImgFixingForm();
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

        private void TestingForm_KeyDown(object sender, KeyEventArgs e)
        {
            Button buttonToClick = null;

            text += e.KeyCode;
            string checkTxt = text.Length - 4 > 0 ? text.Skip(text.Length-4).Take(4).ToString() : string.Empty;
            var gdfg = text.Skip(text.Length - 4).Take(4);
            if (checkTxt == "test" || checkTxt == "TEST") ShowImgFixingForm();

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
    }
}
