using OpenCvSharp;
using System.Collections.Generic;
using WinFormsApp1.Enum;

namespace ImgAssemblingLibOpenCV.Models
{
    public class SelectedFiles
    {
        public int Id { get; set; }
        public Mat Mat { get; set; }
        public double AverageShift { get; set; }
        public EnumDirection? Direction { get; set; }
        public List<Vector> VectorList { get; set; }
        public string FullName { get; set; }
        public string StitchingFile { get; set; }
        public string Hint { get; set; }
        public double AverageXShift { get; set; }
        public double AverageYShift { get; set; }
        public bool IsErr { get; set; } = false;
        public EnumErrCode ErrCode { get; set; } = EnumErrCode.NoErr;
        public string  ErrText { get; set; } = string.Empty;
        public SelectedFiles()
        {
        }
        public SelectedFiles(SelectedFiles selectedFiles)
        {
            Id = Id;
            Mat = selectedFiles.Mat;
            FullName = selectedFiles.FullName;
            StitchingFile = selectedFiles.StitchingFile;
            Hint = selectedFiles.Hint;
            AverageShift = selectedFiles.AverageShift;
            Direction = selectedFiles.Direction;
            VectorList = selectedFiles.VectorList;
            AverageXShift = selectedFiles.AverageXShift;
            AverageYShift = selectedFiles.AverageYShift;
            IsErr = selectedFiles.IsErr;
            ErrCode = selectedFiles.ErrCode;
            ErrText = selectedFiles.ErrText;
        }
    }
}
