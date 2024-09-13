using System;
using System.Collections.Generic;
using System.Linq;
using WinFormsApp1.Enum;

namespace ImgAssemblingLibOpenCV.Models
{
    public class PointFiltr
    {
        public List<Vector> PointList { get; set; }
        public EnumDirection Direction { get; set; }
        //private bool IsPanaram { get; set; }
        public bool IsErr { get; set; } = false;
        public string ErrText { get; set; } = string.Empty;
        public EnumErrCode ErrCode { get; set; }=EnumErrCode.NoErr;
        bool SetErr(string text, EnumErrCode errCode = EnumErrCode.NoErr)
        {
            IsErr = true;
            ErrCode = errCode;
            ErrText = text;
            return false;
        }

        public PointFiltr(List<Vector> pointList)
        {
            PointList = pointList;
        }

        public class QualifyingTable
        {
            public int Quantyty { get; set; }
            public double From { get; set; }
            public double To { get; set; }
            public List<Vector> VectorsList { get; set; }
            public QualifyingTable(double from, double to)
            {
                Quantyty = 0;
                From = from;
                To = to;
                VectorsList = new List<Vector>();
            }
        }

        // отсев лишних точек, определение направления склейки изображения
        public List<Vector> PointScreening()
        {
            if (PointList == null || PointList.Count == 0) return new List<Vector>();
            Dictionary<EnumDirection, int> EnumList = new Dictionary<EnumDirection, int>();
            foreach (EnumDirection value in EnumDirection.GetValues(typeof(EnumDirection))) EnumList.Add(value, 0);
            foreach (var point in PointList)EnumList[point.Direction]++;
            EnumDirection direction = EnumList.Where(x => x.Value == EnumList.Max(y => y.Value)).Select(z => z.Key).FirstOrDefault();

            int ver = EnumList[direction] * 100 / PointList.Count;// Вероятность определения направления движения
            
            PointList = PointList.Where(x=>x.Direction == direction).ToList();

            Double DeltaMin = PointList.Min(x => x.Delta), DeltaMax = PointList.Max(x => x.Delta);
            List<QualifyingTable> qualifyingList = new List<QualifyingTable>();
            double from = 0, to = 0;
            int SepNumb = PointList.Count > 20 ? 10 : 5; 
            for (int i = 0; i < 6; i++)
            {
                to = DeltaMin + i * (DeltaMax - DeltaMin) / 5;
                if (i != 0) qualifyingList.Add(new QualifyingTable(from - 1, to + 1));
                from = to;
            }

            foreach (var point in PointList)
            {
                foreach (var qualify in qualifyingList)
                {
                    if (point.Delta > qualify.From && point.Delta < qualify.To)
                    {
                        qualify.Quantyty++;
                        qualify.VectorsList.Add(point);
                    }
                }
            }

            var rezult = qualifyingList.Where(x => x.Quantyty == qualifyingList.Max(y => y.Quantyty)).Select(z => z.VectorsList).FirstOrDefault();

            if (rezult == null)
            {
                return new List<Vector>();
            }

            rezult = rezult.Where(x => x.Direction == direction ).OrderBy(x => x.CoDirection).ToList();

            if (rezult.Count() > 4) // Если точек много то запускаем дополнительную фильтрацию по сонаправленности 
            {
                var rezult001 = rezult.Where(x => Math.Abs(x.CoDirection) < 0.001).ToList();
                var rezult002 = rezult.Where(x => Math.Abs(x.CoDirection) < 0.002).ToList();
                var rezult005 = rezult.Where(x => Math.Abs(x.CoDirection) < 0.005).ToList();
                var rezult01 = rezult.Where(x => Math.Abs(x.CoDirection) < 0.01).ToList();
                var rezult05 = rezult.Where(x => Math.Abs(x.CoDirection) < 0.05).ToList();
                var rezult1 = rezult.Where(x => Math.Abs(x.CoDirection) < 0.1).ToList();
                var rezult5 = rezult.Where(x => Math.Abs(x.CoDirection) < 0.5).ToList();
                var identity = rezult.OrderBy(x => x.Identity).ToList();

                if (rezult001.Count > 4) rezult = rezult001;
                else if (rezult002.Count > 4) rezult = rezult002;
                else if (rezult005.Count > 4) rezult = rezult005;
                else if (rezult01.Count > 4) rezult = rezult01;
                else if (rezult05.Count > 4) rezult = rezult05;
                else if (rezult1.Count > 4) rezult = rezult1;
                else if (rezult5.Count > 4) rezult = rezult5;

                if (rezult05.Count == 0)
                {

                }
            }

            return rezult;
        }

        // Фильтрация точек
        public List<Vector> PointFiltering()
        {
            //var gdfsd = (from x in PointList where x.Direction == EnumDirection.Down select new { x.Direction, x.Delta, x.CoDirection, x.Identity }).OrderBy(y => y.Identity).ToList();
            //return PointList.OrderBy(x => x.Identity).Take(5).ToList();
            return PointList.OrderBy(x => x.Identity).ToList();

            //PointStatistiq pointStatistiq = new PointStatistiq(PointList);
            //if(pointStatistiq.T0005 >= 5) PointList = PointList.Where(x => x.CoDirection < 0.005).OrderBy(x => x.CoDirection).ToList();
            //else if (pointStatistiq.T001 >= 10) PointList = PointList.Where(x => x.CoDirection < 0.01).OrderBy(x => x.CoDirection).ToList();
            //else if (pointStatistiq.T005 >= 20) PointList = PointList.Where(x => x.CoDirection < 0.05).OrderBy(x => x.CoDirection).ToList();
            //else if (pointStatistiq.T02 < 5 || pointStatistiq.F01 > 15 || pointStatistiq.F05 > 2 || PointList.Count < 3)
            //{
            //    if (pointStatistiq.T02 < 5) SetErr("Err Filter1.не достаточно подходящих ключевых точек F02 !!!", EnumErrCode.NotEnoughKeyPointsF02);
            //    else if (pointStatistiq.F01 > 15) SetErr("Err Filter1.превышено количество точек F01 !!!", EnumErrCode.F01PointsExceed);
            //    else if (pointStatistiq.F05 > 2) SetErr("Err Filter1.превышено количество точек F05 !!!", EnumErrCode.F05PointsExceed);
            //    else if (PointList.Count < 3) SetErr("Err Filter1.не достаточно подходящих ключевых точек !!!", EnumErrCode.NotEnoughKeyPoints);
            //    PointList = new List<Vector>();
            //}
            //else PointList = PointList.Where(x => x.CoDirection < 0.05).OrderBy(x => x.CoDirection).ToList();

            //return PointList;
        }
    }
}
