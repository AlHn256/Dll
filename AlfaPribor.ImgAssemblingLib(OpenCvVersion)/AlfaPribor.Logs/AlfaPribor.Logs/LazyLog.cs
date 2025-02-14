using System;
using System.IO;

namespace AlfaPribor.Logs
{
    /// <summary>
    /// Ленивый логгер. Создаёт файл только в момент записи в него. В противном случае просто не активен.
    /// </summary>
    public class LazyLog
    {
        string _filename;
        string _ext;
        int _maxlen = 1024 * 1024;
        int _daylen = 0;
        /// <summary>Ограничение максимального размера логов</summary>
        int _max_files_size = 0;
        /// <summary>Для потокобезопасности</summary>
        private object _logCheckObject = new object();
        /// <summary>Объект лога</summary>
        private Log _innerLog;
        /// <summary>Путь к каталогу лога </summary>
        private string _directoryPath;
        /// <summary>Флаг инициализации директории</summary>
        private bool _isInitDirectory;

        /// <summary>Конструктор класса </summary>
        /// <param name="file"></param>
        /// <param name="maxlength"></param>
        public LazyLog(string file, int maxlength, string directoryPath = null)
        {
            _filename = file;
            _ext = Path.GetExtension(file);
            _maxlen = maxlength;
            _daylen = 0;
            _directoryPath = directoryPath;
            _isInitDirectory=false;
        }
        /// <summary>Конструктор класса </summary>
        /// <param name="file"></param>
        /// <param name="max_length"></param>
        /// <param name="days_length"></param>
        public LazyLog(string file, int max_length, int days_length, string directoryPath = null) : this(file, max_length, directoryPath)
        {
            _daylen = days_length;
        }

        /// <summary>Конструктор класса</summary>
        /// <param name="file"></param>
        /// <param name="max_length"></param>
        /// <param name="days_length"></param>
        /// <param name="max_size"></param>
        public LazyLog(string file, int max_length, int days_length, int max_size, string directoryPath = null) : this(file, max_length, days_length, directoryPath)
        {
            _max_files_size = max_size;
        }

        /// <summary>Проверка на наличие файла лога </summary>
        private void CheckLogObject()
        {

            lock (_logCheckObject)
            {
                try
                {
                    if (!_isInitDirectory && !string.IsNullOrEmpty(_directoryPath))
                    {
                        try
                        {
                            if (!Directory.Exists(_directoryPath))
                            {
                                DirectoryInfo dirInfo = Directory.CreateDirectory(_directoryPath);
                                _isInitDirectory = dirInfo.Exists;
                            }
                            else { _isInitDirectory = true; }
                        }
                        catch { }
                    }
                    if (_innerLog == null)
                        _innerLog = new Log(_filename, _maxlen, _daylen, _max_files_size);
                }
                catch { }
            }
        }

        /// <summary>Запись в файл</summary>
        /// <param name="data"></param>
        public void Write(string data)
        {
            try
            {
                CheckLogObject();
                _innerLog.Write(data);
            }
            catch { }
        }

        /// <summary>Запись в файл лога</summary>
        /// <param name="data"></param>
        /// <param name="dt"></param>
        public void Write(string data, ref DateTime dt)
        {
                CheckLogObject();
                _innerLog.Write(data, ref dt);
         }

        /// <summary>Определение размера директорий</summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static long DirSize(DirectoryInfo d)
#pragma warning restore CS1591 // Отсутствует комментарий XML для публично видимого типа или члена "Log.DirSize(DirectoryInfo)"
        {
            long size = 0;
            // Add file sizes.
            FileInfo[] fis = d.GetFiles("");
            foreach (FileInfo fi in fis)
            {
                size += fi.Length;
            }
            // Add subdirectory sizes.
            DirectoryInfo[] dis = d.GetDirectories();
            foreach (DirectoryInfo di in dis)
            {
                size += DirSize(di);
            }
            return size;
        }
    }
}
