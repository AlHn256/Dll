using OpenCvSharp;
using System.Collections.Generic;
using System.Drawing;
using WinFormsApp1.Enum;

namespace ImgAssemblingLib.Models
{
    public class FinalResult
    {
        public double Speed { get; set; } = 0;
        public Mat MatRezult { get; set; }
        public Bitmap BitRezult { get; set; }
        public string AssemblyReport { get; set; }
        public bool IsErr { get; set; } = false;
        public string ErrText { get; set; } = string.Empty;
        public EnumErrCode ErrCode { get; set; }
        public List<string> ErrList { get; set; } = new List<string>();

        public FinalResult()
        { 
        }
    }
}
