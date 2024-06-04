namespace WinFormsLib
{
    public partial class TestForm : UserControl
    {
        public TestForm()
        {
            InitializeComponent();
        }

        private void ExitBtn_Click(object sender, EventArgs e)
        {
            //Close();
        }
        private void StartBtn_Click(object sender, EventArgs e)
        {
            NewForm newForm = new NewForm();
            newForm.ShowDialog();
        }
    }
}
