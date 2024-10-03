using System;
using System.Collections.Generic;
using System.IO;

namespace ImgAssemblingLibOpenCV.Models
{
    public class AssemblyPlan  
    {
        public bool BitMap { get; set; } = false; // Работаем не через файлы, а через массивы Bitmapов
        public bool SaveImgFixingRezultToFile { get; set; } = false; // Принудительное сохранение исправленных кадров в фалы при работе с Bitmapами
        public string WorkingDirectory { get; set; } = string.Empty;
        public bool FileNameCheck { get; set; } = false; // Проверка и исправление имен файлов
        public string FileNameCheckRezult { get; set; } = "Не выполнено!";

        public bool FileNameFixing { get; set; } = false;
        public string FileNameFixingRezult { get; set; } = "Не выполнено!";

        public bool DelFileCopy { get; set; } = true; // Удаление дублирующихся изображений
        public string DelFileCopyRezult { get; set; } = "Не выполнено!";

        public bool FixImg { get; set; } = true; // Исправление избражений
        public string FixingImgDirectory { get; set; } = string.Empty;
        public bool ChekFixImg { get; set; } = true;
        public string ChekFixImgRezult { get; set; } = "Не выполнено!";
        public string ImgFixingPlan { get; set; } = "imgFixingSettings.oip";
        public string FixImgRezult { get; set; } = "Не выполнено!";

        public bool FindKeyPoints { get; set; } = true; // Поиск ключевых точек
        public bool ChekStitchPlan { get; set; } = true; // Проверка на наличие существующего плана сборки
        public bool DefaultParameters { get; set; } = false;
        public string StitchingDirectory { get; set; } = string.Empty; // Папка из которой запускается итоговая сборка изображения
        public int Period { get; set; } = 1; 
        public int From { get; set; } = 0; // Период от и 
        public int To { get; set; } = 100; // до
        public bool Percent { get; set; } = true; // в процентах или номерах файлов
        public int Delta { get; set; } = 20; // смещение линии сборки от центра картинки
        public bool SelectSearchArea { get; set; } = false;
        public float MinHeight { get; set; } = 0;
        public float MaxHeight { get; set; } = 0;
        public float MinWight { get; set; } = 0;
        public float MaxWight { get; set; } = 0;
        public string ChekStitchPlanRezult { get; set; } = "Не выполнено!";
        public string FindKeyPointsRezult { get; set; } = "Не выполнено!";

        public bool SpeedCounting { get; set; } = false; // Подсчет скорости
        public string SpeedCountingRezults { get; set; } = "Не выполнено!";
        public double Speed { get; set; } = -1;
        public double MillimetersInPixel { get; set; } = 5.5; // Количество мм в одном пикселе
        public double TimePerFrame { get; set; } = 40; // Милисекунд в одном кадре

        public bool Stitch { get; set; } = true;  // Склейка изображений
        public string StitchRezult { get; set; } = "Не выполнено!";

        public bool SaveRezults { get; set; } = false; // Автосохранение результирующего изображения
        public string RezultOfSavingRezults { get; set; } = "Не выполнено!";
        public const string defaultAssemblingFile = "NewAssemblyPlan.asp";
        public bool ShowAssemblingFile { get; set; } = false; // Показать сохраненный файл ( работает только при включенном SaveRezults) 

        public bool AdditionalFilter { get; set; } = false; // Включение/отключение дополнительный фильтр ключевых точек
        public void ResetRezults()
        {
            FileNameCheckRezult = "Не выполнено!";
            FileNameFixingRezult = "Не выполнено!";
            DelFileCopyRezult = "Не выполнено!";
            ChekFixImgRezult = "Не выполнено!";
            FixImgRezult = "Не выполнено!";
            ChekStitchPlanRezult = "Не выполнено!";
            FindKeyPointsRezult = "Не выполнено!";
            SpeedCountingRezults = "Не выполнено!";
            Speed = -1;
            StitchRezult = "Не выполнено!";
            RezultOfSavingRezults = "Не выполнено!";
        }
    }
}
