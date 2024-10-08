﻿using WinFormsApp1.Enum;

namespace ImgAssemblingLibOpenCV.Models
{
    public class VectorInfo
    {
        public double AverageXShift { get; set; }
        public double AverageYShift { get; set; }
        public EnumDirection? Direction { get; set; }
    }
}
