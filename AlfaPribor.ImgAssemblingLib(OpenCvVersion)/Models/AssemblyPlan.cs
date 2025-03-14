namespace ImgAssemblingLibOpenCV.Models
{
    /// <summary>
    /// Класс для создания планов сборки
    /// </summary>
    public class AssemblyPlan  
    {
        /// <summary>
        /// Работаем не через файлы, а через массивы Bitmapов
        /// </summary>
        public bool BitMap { get; set; } = true;
        /// <summary>
        /// Принудительное сохранение исправленных кадров в фалы при работе с Bitmapами
        /// </summary>
        public bool SaveImgFixingRezultToFile { get; set; } = false;
        public string WorkingDirectory { get; set; } = string.Empty;
        //public bool FileNameCheck { get; set; } = false; // Проверка и исправление имен файлов
        //public string FileNameCheckRezult { get; set; } = "Не выполнено!";
        public bool FileNameFixing { get; set; } = false;
        /// <summary>
        /// Удаление дублирующихся изображений
        /// </summary>
        public bool DelFileCopy { get; set; } = true;
        
        /// <summary>
        /// Исправление избражений
        /// </summary>
        public bool FixImg { get; set; } = false; 
        /// <summary>
        /// Папка для исправленных кадров
        /// </summary>
        public string FixingImgDirectory { get; set; } = string.Empty;
        public bool ChekFixImg { get; set; } = true;

        /// <summary>
        /// Файл для загрузки параметров поумолчанию
        /// </summary>
        public string ImgFixingPlan { get; set; } = "imgFixingSettings.oip";
        /// <summary>
        /// Поиск ключевых точек
        /// </summary>
        public bool FindKeyPoints { get; set; } = true;
        /// <summary>
        /// Проверка на наличие существующего плана сборки
        /// </summary>
        public bool ChekStitchPlan { get; set; } = true;
        /// <summary>
        /// Устанавливает значение параметров Period, From, To, Percent, Delta по умолчанию
        /// </summary>
        public bool DefaultParameters { get; set; } = false;
        /// <summary>
        /// Папка из которой запускается итоговая сборка изображения
        /// </summary>
        public string StitchingDirectory { get; set; } = string.Empty;
        /// <summary>
        /// Период с которым берутся кадры
        /// </summary>
        public int Period { get; set; } = 1; 
        /// <summary>
        /// Номер кадра с которого начинается сборка
        /// </summary>
        public int From { get; set; } = 0; 
        /// <summary>
        /// Номер кадра на котором зканчивается сборка
        /// </summary>
        public int To { get; set; } = 100;
        /// <summary>
        /// Определяет в чем работают параметры From и To в роцентах или номерах кадров
        /// </summary>
        public bool Percent { get; set; } = true; 
        /// <summary>
        /// Смещение линии сборки от центра картинки
        /// </summary>
        public int Delta { get; set; } = 0; 
        /// <summary>
        /// Поиск точек в заданном участке
        /// </summary>
        public bool SelectSearchArea { get; set; } = false;
        public float MinHeight { get; set; } = 0;
        public float MaxHeight { get; set; } = 0;
        public float MinWight { get; set; } = 0;
        public float MaxWight { get; set; } = 0;
        /// <summary>
        /// Подсчет скорости
        /// </summary>
        public bool SpeedCounting { get; set; } = false; 
        public double Speed { get; set; } = -1;
        /// <summary>
        /// Количество мм в одном пикселе
        /// </summary>
        public double MillimetersInPixel { get; set; } = 5.5; 
        /// <summary>
        /// Милисекунд в одном кадре
        /// </summary>
        public double TimePerFrame { get; set; } = 40;
        /// <summary>
        /// Склейка изображений
        /// </summary>
        public bool Stitch { get; set; } = true; 
        /// <summary>
        /// Автосохранение результирующего изображения
        /// </summary>
        public bool SaveRezult { get; set; } = true;
        public const string defaultAssemblingFile = "NewAssemblyPlan.asp";
        /// <summary>
        /// Показать сохраненный файл ( работает только при включенном SaveRezults) 
        /// </summary>
        public bool ShowRezult { get; set; } = false;
        /// <summary>
        /// Включение/отключение дополнительный фильтр ключевых точек
        /// </summary>
        public bool AdditionalFilter { get; set; } = false;
        public void ResetRezults()
        {
            Speed = -1;
            //FileNameCheckRezult = "Не выполнено!";
            //FileNameFixingRezult = "Не выполнено!";
            //DelFileCopyRezult = "Не выполнено!";
            //ChekFixImgRezult = "Не выполнено!";
            //FixImgRezult = "Не выполнено!";
            //ChekStitchPlanRezult = "Не выполнено!";
            //FindKeyPointsRezult = "Не выполнено!";
            //SpeedCountingRezults = "Не выполнено!";
            //StitchRezult = "Не выполнено!";
            //RezultOfSavingRezults = "Не выполнено!";
        }
    }
}