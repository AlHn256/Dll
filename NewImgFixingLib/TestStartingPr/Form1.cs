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


        // ������ ������� �������� ����������� � ����� �� ���������� �� ����� ��������� ������ ��������
        private void FixingImgsUsingDataArrayBtn_Click(object sender, EventArgs e)
        {
            var stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
            TimeSpan ts = stopwatch.Elapsed;

            string ImgFixingPlan = "14.fip"; // ���� � ������������ ������������� �����������
            string WorkingDirectory = "D:\\Work\\Exampels\\14(3)"; // ����� ����������� ��� ����������
            //string WorkingDirectory = "E:\All\Side1\Left"; // ����� ����������� ��� ����������
            if (!fileEdit.ChkDir(WorkingDirectory)) return;

            //��� �������� ��������� ����� �� ����� � ������� ������ ��������
            FileInfo[] fileList = fileEdit.SearchFiles(WorkingDirectory);
            if (fileList.Length == 0) return;
            Bitmap[] dataArray = fileList.Select(x => { return new Bitmap(x.FullName); }).ToArray();

            ImgFixingForm imgFixingForm = new ImgFixingForm(ImgFixingPlan, false);
            var respArray = imgFixingForm.FixImgArray(dataArray);

            ts = stopwatch.Elapsed;
            string text = String.Format("{0:00}:{1:00}:{2:00}", ts.Minutes, ts.Seconds, ts.Milliseconds / 10);

            // ��� �������� ����� �������� ���� ���� �� ��������� �������
            // respArray[6].Save("test2060.jpg");
        }


        // ������ ������� �������� ����������� � ����� �� ���������� �� ����� ��������� ������ ��������
        private void FixingImgsUsingDataArrayBtn_Click2(object sender, EventArgs e)
        {
            var stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
            TimeSpan ts = stopwatch.Elapsed;

            string ImgFixingPlan = "14.fip"; // ���� � ������������ ������������� �����������
            string WorkingDirectory = "D:\\Work\\Exampels\\14(3)"; // ����� ����������� ��� ����������
            if (!fileEdit.ChkDir(WorkingDirectory)) return;

            FileInfo[] fileList = fileEdit.SearchFiles(WorkingDirectory);
            MagickImage[] dataArray = (from file in fileList select new MagickImage(file.FullName)).ToArray();
            ImgFixingForm imgFixingForm = new ImgFixingForm(ImgFixingPlan, false);
            var respArray = imgFixingForm.FixImgArray(dataArray);

            ts = stopwatch.Elapsed;
            string text = String.Format("{0:00}:{1:00}:{2:00}", ts.Minutes, ts.Seconds, ts.Milliseconds / 10);

            // ��� �������� ����� �������� ���� ���� �� ��������� �������
            //if (respArray.Length > 0) File.WriteAllBytes("D:\\Work\\Exampels\\rezult.jpg", respArray[0].ToByteArray());
        }
        private void ImgFixingFormBtn_Click(object sender, EventArgs e)
        {
            ShowImgFixingForm();
        }

        // ������ ����� ��� �������� ����� � ����������� ��������� �����������
        private void ShowImgFixingForm()
        {
            ImgFixingForm imgFixingForm = new ImgFixingForm("E:\\All\\Side1\\Left");
            imgFixingForm.ShowDialog();
        }

        // ������ ������� �������� ����������� � ����� �� ���������� �� �����
        private void HidenFixingExampelBtn_Click(object sender, EventArgs e)
        {
            string ImgFixingPlan = "14.fip"; // ���� � ������������ ������������� �����������
            string WorkingDirectory = "D:\\Work\\Exampels\\14(3)"; // ����� ����������� ��� ����������
            string outputDir = "D:\\Work\\Exampels\\14(3)AutoOut";// �������������� �����
            //string ImgFixingPlan = "Left.fip";// ���� � ������������ ������������� �����������
            //string WorkingDirectory = "D:\\Work\\Exampels\\Left";// ������ ����������� ��� ����������
            //string outputDir = "D:\\Work\\Exampels\\LeftAutoOut";// �������������� �����
            ImgFixingForm distortionTest = new ImgFixingForm(ImgFixingPlan, WorkingDirectory, false);
            if (string.IsNullOrEmpty(ImgFixingPlan)) ImgFixingPlan = distortionTest.GetImgFixingPlan();
            distortionTest.FixImges(outputDir);
        }


        // ���� ����� ��� ���� ������ ������������ �����������
        private async void HidenFixingExampel2Btn_Click(object sender, EventArgs e) => await StartFixing();
        public async Task<bool> StartFixing()
        {
            string ImgFixingPlan = "Left.fip";// ���� � ������������ ������������� �����������
            string WorkingDirectory = "D:\\Work\\Exampels\\Left";// ������ ����������� ��� ����������
            string outputDir = "D:\\Work\\Exampels\\LeftAutoOut";// �������������� �����
            //string ImgFixingPlan = "14.fip"; // ���� � ������������ ������������� �����������
            //string WorkingDirectory = "D:\\Work\\Exampels\\14(3)"; // ����� ����������� ��� ����������
            //string outputDir = "D:\\Work\\Exampels\\14(3)AutoOut";// �������������� �����
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
