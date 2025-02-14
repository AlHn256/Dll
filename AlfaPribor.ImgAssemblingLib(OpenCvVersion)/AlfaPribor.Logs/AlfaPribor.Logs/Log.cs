using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace AlfaPribor.Logs
{

    /// <summary>Класс ведения лога с ротацией файлов</summary>
    public class Log
    {

        string filename;
        string ext;
        int maxlen = 1024 * 1024;
        int daylen = 0;
        /// <summary>Ограничение максимального размера логов</summary>
        int max_files_size = 0;

        /// <summary>Конструктор лога с ротацией 10 файлов</summary>
        /// <param name="file">Имя лог файла</param>
        /// <param name="maxlength">Максимальный размер файла</param>
        public Log(string file, int maxlength)
        {
            filename = file;
            ext = Path.GetExtension(file);
            maxlen = maxlength;
            daylen = 0;
            if (!File.Exists(file))
            {
                //Создание директории если ее нет
                if (!Directory.Exists(Path.GetDirectoryName(file)))  
                    Directory.CreateDirectory(Path.GetDirectoryName(file));
                //Создание файла
                FileStream fs = File.Create(file);
                fs.Close();
            }
        }

        /// <summary>Конструктор лога с ротацией по длительности хранения файлов</summary>
        /// <param name="file">Имя лог файла</param>
        /// <param name="max_length">Максимальный размер файла</param>
        /// <param name="days_length">Длительность хранения в днях</param>
        public Log(string file, int max_length, int days_length)
        {
            filename = file;
            ext = Path.GetExtension(file);
            maxlen = max_length;
            daylen = days_length;
            if (!File.Exists(file))
            {
                //Создание директории если ее нет
                string path = Path.GetDirectoryName(file);
                if (path != "" && !Directory.Exists(path)) Directory.CreateDirectory(Path.GetDirectoryName(file));
                //Создание файла
                FileStream fs = File.Create(file);
                fs.Close();
            }
        }

        /// <summary>Конструктор лога с ротацией по длительности хранения файлов</summary>
        /// <param name="file">Имя лог файла</param>
        /// <param name="max_length">Максимальный размер файла</param>
        /// <param name="days_length">Длительность хранения в днях</param>
        /// <param name="max_size">Максимальный размер</param>
        public Log(string file, int max_length, int days_length, int max_size)
        {
            filename = file;
            ext = Path.GetExtension(file);
            maxlen = max_length;
            daylen = days_length;
            max_files_size = max_size;
            if (!File.Exists(file))
            {
                //Создание директории если ее нет
                if (!Directory.Exists(Path.GetDirectoryName(file)))
                    Directory.CreateDirectory(Path.GetDirectoryName(file));
                //Создание файла
                FileStream fs = File.Create(file);
                fs.Close();
            }
        }

        /// <summary>Сохранение строки в лог файл</summary>
        /// <param name="data"></param>
        public void Write(string data)
        {
    	    try 
            {
                Metka:
                if (File.Exists(filename))
                {
                    RotateLog(filename);//Проверка размера файла и ротация
                    using (StreamWriter sw = File.AppendText(filename))
                    {
                        string tmp = "";
                        //Метка времени
                        if (data != "")
                        {
                            DateTime time_now = DateTime.Now;
                            tmp = time_now.ToString("u");
                            tmp = tmp.Remove(tmp.Length - 1, 1);
                            tmp += ":" + time_now.Millisecond.ToString("000");
                            tmp += ": " + data;
                        }
                        else tmp = "\r\n";
                        try { sw.WriteLine(tmp); }
                        finally { sw.Close(); }
                    }
	            }
                else 
                { 
                    FileInfo file = new FileInfo(filename);
                    file.Create();
                    goto Metka;
                }
            }
            catch (Exception) { }
        }
        
        /// <summary>Сохранение строки в лог файл</summary>
        /// <param name="data"></param>
        /// <param name="dt">Возвращаемое время записи события</param>
        public void Write(string data, ref DateTime dt)
        {
            try
            {
                if (File.Exists(filename))
                {
                    RotateLog(filename);//Проверка размера файла и ротация
                    using (StreamWriter sw = File.AppendText(filename))
                    {
                        string tmp = "";
                        //Метка времени
                        if (data != "")
                        {
                            dt = DateTime.Now;
                            tmp = dt.ToString("u");
                            tmp = tmp.Remove(tmp.Length - 1, 1);
                            tmp += ":" + dt.Millisecond.ToString("000");
                            tmp += ": " + data;
                        }
                        else tmp = "\r\n";
                        try { sw.WriteLine(tmp); }
                        finally { sw.Close(); }
                    }
                }
            }
            catch (Exception) { }
        }

        /// <summary>Ротация лог-файлов</summary>
        /// <param name="file_name">Имя файла</param>
        void RotateLog(string file_name)
        {
            try
            {
                //Проверка размера файла

                using (FileStream fs = File.OpenRead(file_name))
                {
                    long file_size = fs.Seek(0, SeekOrigin.End);
                    fs.Close();
                    //Размер текущего файла не превышен - выход
                    if (file_size < maxlen) return;
                } 

                //Каталог лога
                string dir = Path.GetDirectoryName(file_name);
                //Имя без расширения
                string name = Path.GetFileNameWithoutExtension(file_name);

                #region Обычный вариант 10 файлов логов
                if (daylen == 0)
                {
                    //Удаление самого старого файла с окончанием 9
                    try 
                    {
                        File.Delete(dir + Path.DirectorySeparatorChar + name + ".9" + ext); 
                    } 
                    catch (Exception) { }                    
                    //Сдвиг названий файлов на 1-цу
                    for (int i = 0; i < 8; i++)
                    {
                        try
                        {
                            File.Move(dir + Path.DirectorySeparatorChar + name + "." + (8 - i).ToString() + ext,
                                      dir + Path.DirectorySeparatorChar + name + "." + (9 - i).ToString() + ext);
                        }
                        catch (Exception) { }
                    }
                }
                #endregion

                #region Вариант смещения по длительности хранения
                else
                {
                    //Число файлов указанного типа
                    string[] files = Directory.GetFiles(dir + Path.DirectorySeparatorChar, name + ".*" + ext);
                    int cnt = files.Length;
                    for (int i = cnt; i > 0; i--)
                    {
                        try
                        {
                            //Получить атрибуты
                            DateTime creation_time = File.GetCreationTime(dir + Path.DirectorySeparatorChar + name + "." + (i).ToString() + ext);
                            DateTime write_time = File.GetLastWriteTime(dir + Path.DirectorySeparatorChar + name + "." + (i).ToString() + ext);

                            //Переименование суффиксов файлов
                            File.Move(dir + Path.DirectorySeparatorChar + name + "." + (i).ToString() + ext,
                                      dir + Path.DirectorySeparatorChar + name + "." + (i + 1).ToString() + ext);

                            //Установить атрибуты
                            File.SetCreationTime(dir + Path.DirectorySeparatorChar + name + "." + (i + 1).ToString() + ext, creation_time);
                            File.SetLastWriteTime(dir + Path.DirectorySeparatorChar + name + "." + (i + 1).ToString() + ext, write_time);
                        }
                        catch (Exception) { }
                    }
                    //Удаление файлов выходящих за границы времени хранения
                    string[] all_files = Directory.GetFiles(dir + Path.DirectorySeparatorChar, name + ".*" + ext);
                    for (int i = 0; i < all_files.Length; i++)
                    {
                        try
                        {
                            //Дата создания или дата изменения ранее указанной даты
                            if (File.GetCreationTime(all_files[i]).AddDays(daylen) < DateTime.Now ||
                                File.GetLastWriteTime(all_files[i]).AddDays(daylen) < DateTime.Now)
                                File.Delete(all_files[i]);
                        }
                        catch { }
                    }

                    //Удаление файлов, выходящих за общий размер хранения - доделать !!!
                    //if (max_files_size > 0)
                    //{
                    //    long size = 0;
                    //    DirectoryInfo d = new DirectoryInfo(dir + Path.DirectorySeparatorChar);
                    //    FileInfo[] fis = d.GetFiles();
                    //    foreach (FileInfo fi in fis) size += fi.Length;
                    //}

                }
                #endregion

                //Присвоение текущему лог-файлу суффикса 1
                try 
                {
                    //Получить атрибуты
                    DateTime write_time = File.GetLastWriteTime(file_name);
                    DateTime create_time = File.GetCreationTime(file_name);

                    string new_file = dir + Path.DirectorySeparatorChar + name + ".1" + ext;
                    File.Move(file_name, new_file);

                    //Установить атрибуты
                    File.SetLastWriteTime(new_file, write_time);
                    File.SetCreationTime(new_file, create_time);
                }
                catch (Exception) { }

                //Создание нового файла
                using (FileStream fs = File.Create(file_name)) { fs.Close(); }

                //Установить атрибуты
                File.SetLastWriteTime(file_name, DateTime.Now);
                File.SetCreationTime(file_name, DateTime.Now);
            }
            catch (Exception) { }
        }

        #pragma warning disable CS1591 // Отсутствует комментарий XML для публично видимого типа или члена "Log.DirSize(DirectoryInfo)"
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
