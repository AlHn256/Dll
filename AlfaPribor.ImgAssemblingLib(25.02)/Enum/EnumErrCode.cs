namespace WinFormsApp1.Enum
{
    public enum EnumErrCode
    {
        NoErr,
        Err,
        FileNotFound,
        ZeroFile, // Нулевой файл или блок битмапа
        ParametersDontMatch, // Не совпадают параметры между блоками
        Copy, // Копии кадров
        PointsNotFound, // Не найдены подходящие точки
        ShiftThreshold, // Смещение не превысило порог погрешности
        WrongDirection, // 
        F01PointsExceed, // Превышено количество точек F01
        F05PointsExceed, // Превышено количество точек F05
        NotEnoughKeyPoints, // Недостаточно подходящих ключевых точек
        NotEnoughKeyPointsF02 // Недостаточно подходящих ключевых точек  F02
    }
}