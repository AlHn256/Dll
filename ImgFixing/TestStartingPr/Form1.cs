using ImgFixingLib;

namespace TestStartingPr
{
    public partial class Form1 : Form
    {
        private SynchronizationContext _context;
        public Form1()
        {
            InitializeComponent();
            if (SynchronizationContext.Current != null) _context = SynchronizationContext.Current;
            else _context = new SynchronizationContext();
        }
        private void ImgFixingFormBtn_Click(object sender, EventArgs e)
        {
            ShowImgFixingForm();
        }

        // Запуск формы для создания файла с параметрами коррекции изображений
        private void ShowImgFixingForm()
        {
            ImgFixingForm imgFixingForm = new ImgFixingForm("D:\\Work\\Exampels\\Left");
            imgFixingForm.ShowDialog();
        }

        // Пример запуска исправления всех изображений в папке по настройкам из файла
        private void HidenFixingExampelBtn_Click(object sender, EventArgs e)
        {
            string ImgFixingPlan = "3179.fip";// Файл с параментрами корректировки изображений
            string WorkingDirectory = "E:\\ImageArchive\\3179_4_0";// Парака изображений для испраления
            string outputDir = "E:\\ImageArchive\\3179_4_0AutoOut";// Результирующая папка
            ImgFixingForm distortionTest = new ImgFixingForm(ImgFixingPlan, WorkingDirectory, false);
            if (string.IsNullOrEmpty(ImgFixingPlan)) ImgFixingPlan = distortionTest.GetImgFixingPlan();
            distortionTest.FixImges(outputDir);
        }

        // Тоже самое что выше только отображением результатов
        private async void HidenFixingExampel2Btn_Click(object sender, EventArgs e)
        {
            await StartFixing();
        }
        public async Task<bool> StartFixing()
        {
            string ImgFixingPlan = "3179.fip";// Файл с параментрами корректировки изображений
            string WorkingDirectory = "E:\\ImageArchive\\3179_2_0";// Парака изображений для испраления
            string outputDir = "E:\\ImageArchive\\3179_2_0AutoOut";// Результирующая папка
            ImgFixingForm distortionTest = new ImgFixingForm(ImgFixingPlan, WorkingDirectory, false);
            distortionTest.ProcessChanged += worker_ProcessChang;
            distortionTest.TextChanged += worker_TextChang;
            if (string.IsNullOrEmpty(ImgFixingPlan)) ImgFixingPlan = distortionTest.GetImgFixingPlan();
            bool checkFixinImg = false;
            await Task.Run(() => { checkFixinImg = distortionTest.FixImges(_context, outputDir); });
            if (checkFixinImg) label1.Text = "Task Finished";
            else label1.Text = "Task Failed!";
            return checkFixinImg;
        }

        private void worker_ProcessChang(int progress) => progressBar1.Value = progress;
        private void worker_TextChang(string text) => label1.Text = text;
        private void ExitBtn_Click(object sender, EventArgs e)=>Close();
    }
}
