using OpenCvSharp;
using System.Collections.Generic;
using System.Drawing;
using WinFormsApp1.Enum;

namespace ImgAssemblingLibOpenCV.Models
{
    /// <summary> Класс для описания результатов сборки </summary>
    public class FinalResult
    {
        public double Speed { get; set; } = 0;
        public Mat MatRezult { get; set; }
        public Bitmap BitRezult { get; set; }
        public string AssemblyReport { get; set; } = string.Empty;
        public string RezulFileLink { get; set; } = string.Empty;


        public string FileNameFixingRezult { get; set; } = "Не выполнено!";
        public string DelFileCopyRezult { get; set; } = "Не выполнено!";

        public string ChekFixImgRezult { get; set; } = "Не выполнено!";

        public string FixImgRezult { get; set; } = "Не выполнено!";

        public string ChekStitchPlanRezult { get; set; } = "Не выполнено!";
        public string FindKeyPointsRezult { get; set; } = "Не выполнено!";

        public string SpeedCountingRezults { get; set; } = "Не выполнено!";
        public string StitchRezult { get; set; } = "Не выполнено!";
        public string RezultOfSavingRezults { get; set; } = "Не выполнено!";
        public bool IsErr { get; set; } = false;
        public bool IsCriticalErr { get; set; } = false;
        public string ErrText { get; set; } = string.Empty;
        public EnumErrCode ErrCode { get; set; }
        public List<string> ErrList { get; set; } = new List<string>();

        public bool SetErr(string err)
        {
            IsErr = true;
            ErrText = err;
            ErrList.Add(err);
            return false;
        }
        public bool SetCriticalErr(string err)
        {
            IsErr = true;
            IsCriticalErr = true;
            ErrText = err;
            ErrList.Add(err);
            return false;
        }
        public void Clear()
        {
            Speed = 0;
            MatRezult = null;
            BitRezult = null;
            AssemblyReport = string.Empty;
            IsErr = false;
            ErrText = string.Empty;
            ErrList.Clear();
        }

        public Color GetColor()
        {
            if (IsCriticalErr) return Color.Red;
            else if (IsErr && BitRezult == null) return Color.Red;
            else if (IsErr && BitRezult.Width == 0 && BitRezult.Height == 0) return Color.Red;
            else if (IsErr && BitRezult.Width > 0 && BitRezult.Height > 0) return Color.Yellow;
            else return Color.Green;
        }
    }
}