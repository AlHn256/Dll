namespace ImgFixingLibOpenCvVersion.Models
{
    public class DistorSettings
    {
        public double A { get; set; } = -0.13;
        public double B { get; set; } = 0.39;
        public double C { get; set; } = 0.08;
        public double D { get; set; } = 0;
        public double E { get; set; } = 0;
        public double Sm11 { get; set; } = 1261; 
        public double Sm12 { get; set; } = 0.0; 
        public double Sm13 { get; set; }= 9.4;
        public double Sm21 { get; set; } = 0.5; 
        public double Sm22 { get; set; } = 1217;
        public double Sm23 { get; set; } = 5.9;
        public double Sm31 { get; set; } = 0.0; 
        public double Sm32 { get; set; }= 0.0;
        public double Sm33 { get; set; } = 1.0;
    }
}
