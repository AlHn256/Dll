using WinFormsLib;

namespace StartingForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void StartBtn_Click(object sender, EventArgs e)
        {
            TestForm testForm = new TestForm();
            //testForm.S
            NewForm newForm = new NewForm();
            newForm.ShowDialog();
        }
    }
}
