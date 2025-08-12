using ImgAssemblingLibOpenCV.AditionalForms;
using System;
using System.Windows.Forms;

namespace StartTestProject
{
    internal static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new HidenForm());
            //Application.Run(new DebugingForm());
            //Application.Run(new TestingForm());
            //Application.Run(new AutoTest());
        }
    }
}
