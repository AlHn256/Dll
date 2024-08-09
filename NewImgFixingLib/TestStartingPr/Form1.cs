using NewImgFixingLib;
using NewImgFixingLib.Models;
using ImageMagick;
namespace TestStartingPr
{
    public partial class Form1 : Form
    {
        private SynchronizationContext _context;
        private FileEdit fileEdit = new FileEdit(new string[] { "*.jpeg", "*.jpg", "*.png", "*.bmp" });
        public Form1()
        {
            InitializeComponent();
            if (SynchronizationContext.Current != null) _context = SynchronizationContext.Current;
            else _context = new SynchronizationContext();
            ShowImgFixingForm();
        }


        // Пример запуска корекции изображений в папке по настройкам из файла используя массив битмапов
        private void FixingImgsUsingDataArrayBtn_Click(object sender, EventArgs e)
        {
            var stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
            TimeSpan ts = stopwatch.Elapsed;

            string ImgFixingPlan = "14.fip"; // Файл с параментрами корректировки изображений
            string WorkingDirectory = "D:\\Work\\Exampels\\14(3)"; // Папка изображений для испраления
            //string WorkingDirectory = "E:\All\Side1\Left"; // Папка изображений для испраления
            if (!fileEdit.ChkDir(WorkingDirectory)) return;

            //Для имитации загружаем файлы из папки и создаем массив битмапов
            FileInfo[] fileList = fileEdit.SearchFiles(WorkingDirectory);
            if (fileList.Length == 0) return;
            Bitmap[] dataArray = fileList.Select(x => { return new Bitmap(x.FullName); }).ToArray();

            ImgFixingForm imgFixingForm = new ImgFixingForm(ImgFixingPlan, false);
            var respArray = imgFixingForm.FixImgArray(dataArray);

            ts = stopwatch.Elapsed;
            string text = String.Format("{0:00}:{1:00}:{2:00}", ts.Minutes, ts.Seconds, ts.Milliseconds / 10);

            // Для проверки можно записать один файл из итогового массива
            // respArray[6].Save("test2060.jpg");
        }


        // Пример запуска корекции изображений в папке по настройкам из файла используя массив картинок
        private void FixingImgsUsingDataArrayBtn_Click2(object sender, EventArgs e)
        {
            var stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
            TimeSpan ts = stopwatch.Elapsed;

            string ImgFixingPlan = "14.fip"; // Файл с параментрами корректировки изображений
            string WorkingDirectory = "D:\\Work\\Exampels\\14(3)"; // Папка изображений для испраления
            if (!fileEdit.ChkDir(WorkingDirectory)) return;

            FileInfo[] fileList = fileEdit.SearchFiles(WorkingDirectory);
            MagickImage[] dataArray = (from file in fileList select new MagickImage(file.FullName)).ToArray();
            ImgFixingForm imgFixingForm = new ImgFixingForm(ImgFixingPlan, false);
            var respArray = imgFixingForm.FixImgArray(dataArray);

            ts = stopwatch.Elapsed;
            string text = String.Format("{0:00}:{1:00}:{2:00}", ts.Minutes, ts.Seconds, ts.Milliseconds / 10);

            // Для проверки можно записать один файл из итогового массива
            //if (respArray.Length > 0) File.WriteAllBytes("D:\\Work\\Exampels\\rezult.jpg", respArray[0].ToByteArray());
        }
        private void ImgFixingFormBtn_Click(object sender, EventArgs e)
        {
            ShowImgFixingForm();
        }

        // Запуск формы для создания файла с параметрами коррекции изображений
        private void ShowImgFixingForm()
        {
            ImgFixingForm imgFixingForm = new ImgFixingForm("E:\\All\\Side1\\Left");
            imgFixingForm.ShowDialog();
        }

        // Пример запуска корекции изображений в папке по настройкам из файла
        private void HidenFixingExampelBtn_Click(object sender, EventArgs e)
        {
            string ImgFixingPlan = "14.fip"; // Файл с параментрами корректировки изображений
            string WorkingDirectory = "D:\\Work\\Exampels\\14(3)"; // Папка изображений для испраления
            string outputDir = "D:\\Work\\Exampels\\14(3)AutoOut";// Результирующая папка
            //string ImgFixingPlan = "Left.fip";// Файл с параментрами корректировки изображений
            //string WorkingDirectory = "D:\\Work\\Exampels\\Left";// Парака изображений для испраления
            //string outputDir = "D:\\Work\\Exampels\\LeftAutoOut";// Результирующая папка
            ImgFixingForm distortionTest = new ImgFixingForm(ImgFixingPlan, WorkingDirectory, false);
            if (string.IsNullOrEmpty(ImgFixingPlan)) ImgFixingPlan = distortionTest.GetImgFixingPlan();
            distortionTest.FixImges(outputDir);
        }


        // Тоже самое что выше только отображением результатов
        private async void HidenFixingExampel2Btn_Click(object sender, EventArgs e) => await StartFixing();
        public async Task<bool> StartFixing()
        {
            string ImgFixingPlan = "Left.fip";// Файл с параментрами корректировки изображений
            string WorkingDirectory = "D:\\Work\\Exampels\\Left";// Парака изображений для испраления
            string outputDir = "D:\\Work\\Exampels\\LeftAutoOut";// Результирующая папка
            //string ImgFixingPlan = "14.fip"; // Файл с параментрами корректировки изображений
            //string WorkingDirectory = "D:\\Work\\Exampels\\14(3)"; // Папка изображений для испраления
            //string outputDir = "D:\\Work\\Exampels\\14(3)AutoOut";// Результирующая папка
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
