namespace ImgAssemblingLibOpenCV.Models
{
    /// <summary>
    /// Класс параметров настройки исправления кадров
    /// </summary>
    public class ImgFixingSettings
    {
        public bool CropBeforChkBox { get; set; } = true; // Обезка кадра перед обработкой
        public int XBefor { get; set; } = 0; // Обезка кадра слева
        public int DXBefor { get; set; } = 0; // Кол-во пикселей которые нужно взять от левой стороны
        public int YBefor { get; set; } = 0; // Обезка кадра сверху
        public int DYBefor { get; set; } = 0; // Кол-во пикселей которые нужно взять от верхушки

        public double Diminish { get; set; } = 1; //Уменьшение размеров кадра перед обработкой
        public double Zoom { get; set; } // Увеличение
        public int Rotation90 { get; set; } // Поворот на угол кратный 90
        public bool BlackWhiteMode { get; set; } = false; // Вкл\откл чернобелого режима
        public bool Distortion { get; set; } = true; // Исправление дисторсии
        public DistorSettings DistorSettings { get; set; } // Настройки дисторсии
        public bool CropAfterChkBox { get; set; } = true; // Обезка кадра после обработки
        public int XAfter { get; set; } = 0; // Обезка кадра слева
        public int DXAfter { get; set; } = 0; // Кол-во пикселей которые нужно взять от левой стороны
        public int YAfter { get; set; } = 0; // Обезка кадра сверху
        public int DYAfter { get; set; } = 0; // Кол-во пикселей которые нужно взять от верхушки
        public bool AutoReload { get; set; } = false; // Перезагрузка кадра после измененя одного из параметров
        public bool ShowGrid { get; set; } = false; // Добавление сетки для упрощения корекции
        public ImgFixingSettings() { 
            DistorSettings = new DistorSettings();
        }
    }
}