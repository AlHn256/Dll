﻿using System;
using WinFormsApp1.Enum;

namespace ImgAssemblingLibOpenCV.Models
{
    public class Vector
    {
        #region Fields
        public const int PixelError = 5; // Погрешность (в пикселях) в пределах которой точки считаются идентичными
        public int MatchesId { get; set; }
        public double Xfr { get; set; }
        public double Yfr { get; set; }
        public double Xto { get; set; }
        public double Yto { get; set; }
        public double dX { get; set; }
        public double dY { get; set; }
        public double Delta { get; set; }
        public EnumDirection Direction { get; set; }
        public double CoDirection { get; set; }
        public double Identity { get; set; }
        public bool isSamePoint { get; set; }
        #endregion
        public Vector(int matchesId, double xfr, double yfr, double xto, double yto)
        {
            MatchesId = matchesId;
            Xfr = xfr;
            Yfr = yfr;
            Xto = xto;
            Yto = yto;
            dX = Xto - Xfr;
            dY = Yto - Yfr;

            if (Math.Abs(dX) > Math.Abs(dY)) Direction = dX > 0 ? EnumDirection.Right : Direction = EnumDirection.Left;
            else Direction = dY > 0 ? EnumDirection.Down : Direction = EnumDirection.Up;

            if (Math.Sqrt(dY * dY + dX * dX) < PixelError) isSamePoint = true;
            if (Math.Abs(dX) > Math.Abs(dY))
            {
                CoDirection = dY / dX;
                Delta = dX;
            }
            else
            {
                CoDirection = dX / dY;
                Delta= dY;
            }

            Identity = Delta * CoDirection;
        }
    }
}