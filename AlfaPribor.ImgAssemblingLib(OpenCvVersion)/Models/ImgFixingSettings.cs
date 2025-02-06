using ImgAssemblingLibOpenCV.Models;

namespace ImgFixingLibOpenCvVersion.Models
{
    public class ImgFixingSettings
    {
        public string Dir {  get; set; } = string.Empty;
        public string File { get; set; } = string.Empty;

        public double Zoom { get; set; }
        public int Rotation90 { get; set; }
        public bool BlackWhiteMode { get; set; } = false;
        public bool Distortion { get; set; }
        public DistorSettings DistorSettings { get; set; }
        public bool CropAfterChkBox { get; set; }
        public int XAfter { get; set; }
        public int DXAfter { get; set; }
        public int YAfter { get; set; }
        public int DYAfter { get; set; }
    }
}