
using System.Collections.Generic;
using System.Linq;

namespace ImgAssemblingLib.Models
{
    public class PointStatistiq
    {
        public int SamePoints { get; set; } = 0;
        public int T0005 { get; set; } = 0;
        public int T001 { get; set; } = 0;
        public int T005 { get; set; } = 0;
        public int T01 { get; set; } = 0;
        public int T02 { get; set; } = 0;
        public int Total { get; set; } = 0;
        public int F01 { get; set; } = 0;
        public int F02 { get; set; } = 0;
        public int F05 { get; set; } = 0;
        public PointStatistiq(List<Vector> PointList)
        {
            Total = PointList.Count;
            T0005 = PointList.Where(x => x.CoDirection < 0.005).Count();
            T001 = PointList.Where(x => x.CoDirection < 0.01).Count();
            T005 = PointList.Where(x => x.CoDirection < 0.05).Count();
            T01 = PointList.Where(x => x.CoDirection < 0.1).Count();
            T02 = PointList.Where(x => x.CoDirection < 0.2).Count();
            F01 = PointList.Where(x => x.CoDirection > 0.1).Count();
            F02 = PointList.Where(x => x.CoDirection > 0.2).Count();
            F05 = PointList.Where(x => x.CoDirection > 0.5).Count();
            SamePoints = PointList.Where(x => x.isSamePoint).Count();
        }
    }
}
