using ImgAssemblingLibOpenCV.Models;
using System.Collections.Generic;
using System.Drawing;

namespace ImgAssemblingLib.Models
{
    public class AutoTestPlan
    {
        public int Id { get; set; } = 0;
        public string AssemblingFile { get; set; }
        public string ImgFixingFile { get; set; }
        public bool FileTestPassed { get; set; } = false;
        public Bitmap FileRezult { get; set; }
        public bool ImgTestPassed { get; set; } = false;
        public Bitmap ImgRezult { get; set; }
        public int ISpeedRezult { get; set; }
        public int FSpeedRezult { get; set; }
        public AutoTestPlan(int id, string assemblingFile)
        {
            Id = id;
            AssemblingFile = assemblingFile;
            ImgFinalResult = new FinalResult();
            FileFinalResult = new FinalResult();
        }

        public AutoTestPlan()
        {
            ImgFinalResult = new FinalResult();
            FileFinalResult = new FinalResult();
        }

        public FinalResult FileFinalResult { get; set; }
        public FinalResult ImgFinalResult { get; set; }
        public bool IsErr { get; set; } = false;
        public bool IsCriticalErr { get; set; } = false;
        public string ErrText { get; set; } = string.Empty;
        public List<string> ErrList { get; set; } = new List<string>();
        public bool SetErr(string err)
        {
            IsErr = true;
            ErrText = err;
            return false;
        }
        public bool SetCriticalErr(string err)
        {
            IsErr = true;
            IsCriticalErr = true;
            ErrText = err;
            return false;
        }

        public void AddFileFinalResult(FinalResult finalResult)
        {
            FileFinalResult.AssemblyReport = finalResult.AssemblyReport;
            FileFinalResult.BitRezult = finalResult.BitRezult;
            FileFinalResult.Speed = finalResult.Speed;
            FileFinalResult.RezulFileLink = finalResult.RezulFileLink;
            FileFinalResult.IsErr = finalResult.IsErr;
            FileFinalResult.IsCriticalErr = finalResult.IsCriticalErr;
            FileFinalResult.ErrText = finalResult.ErrText;
            FileFinalResult.ErrCode = finalResult.ErrCode;

            if (finalResult.ErrList == null) return;
            if (finalResult.ErrList.Count == 0) return;
            if (ImgFinalResult.ErrList == null) ImgFinalResult.ErrList = new List<string>();
            else ImgFinalResult.ErrList.Clear();
            foreach (var elem in finalResult.ErrList)
            {
                ImgFinalResult.ErrList.Add(elem);
            }
        }
        public void AddImgFinalResult(FinalResult finalResult)
        {
            ImgFinalResult.AssemblyReport = finalResult.AssemblyReport;
            ImgFinalResult.BitRezult = finalResult.BitRezult;
            ImgFinalResult.Speed = finalResult.Speed;
            ImgFinalResult.RezulFileLink = finalResult.RezulFileLink;
            ImgFinalResult.IsErr = finalResult.IsErr;
            ImgFinalResult.IsCriticalErr = finalResult.IsCriticalErr;
            ImgFinalResult.ErrText = finalResult.ErrText;
            ImgFinalResult.ErrCode = finalResult.ErrCode;

            if (finalResult.ErrList == null) return;
            if (finalResult.ErrList.Count == 0) return;
            if (ImgFinalResult.ErrList == null) ImgFinalResult.ErrList = new List<string>();
            else ImgFinalResult.ErrList.Clear();
            foreach (var elem in finalResult.ErrList)
            {
                ImgFinalResult.ErrList.Add(elem);
            }
        }
    }
}
