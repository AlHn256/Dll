namespace ImgAssemblingLibOpenCV.Models
{
    public class ImgFixingSettings
    {
        public string Dir {  get; set; } = string.Empty;
        public string File { get; set; } = string.Empty;

        public double Zoom { get; set; } // Увеличение
        public int Rotation90 { get; set; } // Поворот на угол кратный 90
        public bool BlackWhiteMode { get; set; } = false;
        public bool Distortion { get; set; } = true; // Исправление дисторсии
        public DistorSettings DistorSettings { get; set; } // Настройки дисторсии
        public bool CropAfterChkBox { get; set; } = true; // Обезка кадра
        public int XAfter { get; set; }
        public int DXAfter { get; set; }
        public int YAfter { get; set; }
        public int DYAfter { get; set; }
        public bool AutoReload { get; set; } = false; // Перезагрузка кадра после измененя одного из параметров
        public bool ShowGrid { get; set; } = false; // Добавление сетки для упрощения корекции
        public ImgFixingSettings() { 
            DistorSettings = new DistorSettings();
        }
    }
}