using System;
using System.Collections.Generic;
using System.IO;

namespace ImgAssemblingLibOpenCV.Models
{
    public class AssemblyPlan : ICloneable
    {
        public bool BitMap { get; set; } = false; // Работаем не через файлы, а через массивы Bitmapов
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
        public string ImgFixingPlan { get; set; } = "imgFixingSettings.fip";
        public string FixImgRezult { get; set; } = "Не выполнено!";

        public bool FindKeyPoints { get; set; } = true; // Поиск ключевых точек
        public bool ChekStitchPlan { get; set; } = true;
        public bool DefaultParameters { get; set; } = false;
        public string StitchingDirectory { get; set; } = string.Empty;
        public int Period { get; set; } = 1;
        public bool Percent { get; set; } = true;
        public int From { get; set; } = 0;
        public int To { get; set; } = 100;
        public int Delta { get; set; } = 20;
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

        public bool IsErr {  get; set; } = false;
        public string ErrText { get; set; } = string.Empty;
        public List<string> ErrList { get; set; } = new List<string>();
        public bool CheckPlan()
        {
            if (string.IsNullOrEmpty(WorkingDirectory))SetErr("Err AssemblyPlan.FirstDirectory.IsNullOrEmpty!!!");
            if(!Directory.Exists(WorkingDirectory)) SetErr("Err WorkingDirectory не существует!!!");
            if (Stitch && !Directory.Exists(StitchingDirectory)) SetErr("Err StitchingDirectory не существует!!!");
            if (FixImg && !Directory.Exists(FixingImgDirectory)) SetErr("Err FixingImgDirectory не существует!!!");
            if(Percent)
            {
                if (From < 0 || From > To || From > 100 || To < 0 || To > 100)
                {
                    From = 0;
                    To = 100;
                    SetErr("Err From\\To corrected!!!");
                }
            }
            return false;
        }
        private bool SetErr(string err)
        {
            ErrText = err;
            return false;
        }

        public object Clone() => MemberwiseClone();
    }
}
