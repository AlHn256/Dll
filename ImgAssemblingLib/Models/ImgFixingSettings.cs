using ImageMagick;

namespace ImgAssemblingLib.Models
{
    public class ImgFixingSettings
    {
        public string Dir {  get; set; } = string.Empty;
        public string File { get; set; } = string.Empty;
        public bool CropBeforeChkBox { get; set; }
        public int XBefore { get; set; }
        public int YBefore { get; set; }
        public int HeightBefore { get; set; }
        public int WidthBefore { get; set; }

        public bool Rotation { get; set; }
        public decimal RotationAngle { get; set; }

        public bool Distortion { get; set; }
        public DistortMethod DistortMethod { get; set; }
        public decimal A { get; set; }
        public decimal B { get; set; }
        public decimal C { get; set; }
        public decimal D { get; set; }

        public bool CropAfterChkBox { get; set; }
        public int XAfter { get; set; }
        public int YAfter { get; set; }
        public int HeightAfter { get; set; }
        public int WidthAfter { get; set; }
    }
}
