using System.Collections.Generic;
using System.Linq;

namespace ImgAssemblingLibOpenCV.Models
{
    public class MNK
    {
        /// <summary>
        /// Класс для поиска линий по методу наименьших квдратов
        /// </summary>
        class MNKLine
        {
            public double X { get; set; }
            public double Y { get; set; }
            public double SQX { get; set; }
            public double XY { get; set; }
        }

        private List<MNKLine> MNKTable = new List<MNKLine>();
        public double A { get; set; }
        public double B { get; set; }

        public MNK(double[] x, double[] y)
        {
            FindAB(x, y);
        }
        public MNK(double[] y)
        {
            double i = 0;
            var x = y.Select(t => { return i++; }).ToArray();

            FindAB(x, y);
        }
        public MNK(List<double> y)
        {
            double i = 0;
            var x = y.Select(t => { return i++; }).ToArray();

            FindAB(x, y.ToArray());
        }
        private bool FindAB(double[] x, double[] y)
        {
            for (int i = 0; i < x.Length; i++)
                MNKTable.Add(new MNKLine() { X = x[i], Y = y[i], XY = x[i] * y[i], SQX = x[i] * x[i] });

            double Sxy = MNKTable.Sum(z => z.XY);
            double Ssqx = MNKTable.Sum(z => z.SQX);
            double Sx = MNKTable.Sum(z => z.X);
            double Sy = MNKTable.Sum(z => z.Y);
            A = (MNKTable.Count * Sxy - Sx * Sy) / (MNKTable.Count * Ssqx - Sx * Sx);
            B = (Sy - A * Sx) / MNKTable.Count;
            return true;
        }
    }
}
