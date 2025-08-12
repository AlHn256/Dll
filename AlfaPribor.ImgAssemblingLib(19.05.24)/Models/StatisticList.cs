namespace ImgAssemblingLibOpenCV.Models
{
    public class StatisticList
    {
        public int Fr { get; set; }
        public int To { get; set; }
        public double Sp { get; set; }
        public StatisticList(int fr, int to, double sp)
        {
            Fr = fr;
            To = to;
            Sp = sp;
        }
    }
}