using System;
using System.Text;
using System.IO;
using System.Threading;

namespace AlfaPribor.Logs
{
    /// <summary>Позволяет вести журнал регистрации отладочных сообщений, размер которого имеет регулируемое значение</summary>
    /// <remarks>
    /// Принцип организации журнала регистрации:
    /// =======================================
    /// считаем, что журнал регистрации будет состоять из PartsCount частей, каждая часть
    /// которого имеет размер не более PartSize байт. Физически журнал будет представлять собой
    /// группу файлов с именами:
    /// FileName - главный файл журнала, содержащий наиболее актуальную (по времени занесения в журнал) информацию;
    /// FileName + "." + N - оставшиеся части журнала, где N меньше PartsCount. Чем больше число N
    /// в имене файла, тем более старую информацию он содержит.
    /// Т.о. общий размер журнала регистрации будет меньше или равен PartsCount * PartSize байтам.
    /// ____________________
    /// Чтобы осуществлялась запись в журнал регистрации, главный файл журнал регистрации отладочных сообщений
    /// должен существовать.
    /// 
    /// !!! Все свойства и методы класса являются потокобезопасными !!!
    /// 
    /// </remarks>
    public class RotateFileLogger: IDebugLogger
    {
        #region Fields

        /// <summary>Имя файла, представляющего журнал регистрации</summary>
        private string _FileName;

        /// <summary>Расширение файла журнала регистрации</summary>
        private string _FileExtension;

        /// <summary>Признак маркировки сообщений, помещаемых в журнал регистрации</summary>
        private bool _MarkMessages;

        /// <summary>Строка форматирования даты/времени, которыми маркируются сообщения, помещаемые в журнал</summary>
        private string _MarkerFormat;

        /// <summary>Максимальный размер каждой части журнала регистрации</summary>
        private long _PartSize;

        /// <summary>Количество частей журнала регистации</summary>
        private long _PartsCount;

        /// <summary>Определяет кодировку, в которой будет записываться сообщение в журнал регистрации</summary>
        private Encoding _Encoding;

        /// <summary>Признак автоматического создания файла журнала в случае его отсутствия</summary>
        private volatile bool _AutoCreate;

        private ReaderWriterLockSlim _Lock;

        #endregion

        #region Methods

        /// <summary>Конструктор класса</summary>
        /// <param name="parts_count">Количество частей (файлов), на которые будет делиться журнал регистрации</param>
        /// <param name="part_size">Максимальная длина в байтах каждого файла (части) журнала регистрации</param>
        /// <param name="marker_format">Строка форматирования даты/времени, которыми должны маркироваться сообщения, помещаемые в журнал</param>
        public RotateFileLogger(long parts_count, long part_size, string marker_format):
            this(parts_count, part_size)
        {
            _MarkMessages = true;
            _MarkerFormat = marker_format;
        }

        /// <summary>Конструктор класса</summary>
        /// <param name="parts_count">Количество частей (файлов), на которые будет делиться журнал регистрации</param>
        /// <param name="part_size">Максимальная длина в байтах каждого файла (части) журнала регистрации</param>
        /// <param name="encoding">Определяет кодировку, в которой будет записываться сообщение в журнал регистрации</param>
        public RotateFileLogger(long parts_count, long part_size, Encoding encoding)
            : this(parts_count, part_size)
        {
            _Encoding = encoding;
        }

        /// <summary>Конструктор класса</summary>
        /// <param name="parts_count">Количество частей (файлов), на которые будет делиться журнал регистрации</param>
        /// <param name="part_size">Максимальная длина в байтах каждого файла (части) журнала регистрации</param>
        /// <param name="marker_format">Строка форматирования даты/времени, которыми должны маркироваться сообщения, помещаемые в журнал</param>
        /// <param name="encoding">Определяет кодировку, в которой будет записываться сообщение в журнал регистрации</param>
        public RotateFileLogger(long parts_count, long part_size, string marker_format, Encoding encoding) :
            this(parts_count, part_size)
        {
            _MarkMessages = true;
            _MarkerFormat = marker_format;
            _Encoding = encoding;
        }

        /// <summary>Конструктор класса</summary>
        /// <param name="parts_count">Количество частей (файлов), на которые будет делиться журнал регистрации</param>
        /// <param name="part_size">Максимальная длина в байтах каждого файла (части) журнала регистрации</param>
        public RotateFileLogger(long parts_count, long part_size)
        {
            _FileName = string.Empty;
            _FileExtension = string.Empty;
            _MarkMessages = false;
            _MarkerFormat = "yyyy/MM/dd HH:mm:ss:fff ";
            _Encoding = Encoding.Default;
            _PartSize = part_size;
            _PartsCount = parts_count;
            _AutoCreate = true;
            _Lock = new ReaderWriterLockSlim();
        }

        /// <summary>Ротация файлов журнала регистрации</summary>
        private void RotateLog()
        {
            try
            {
                // Проверяем длину наиболее актуального файла журнала регистрации
                using (FileStream fs = File.OpenRead(_FileName + _FileExtension))
                {
                    long file_size = fs.Seek(0, SeekOrigin.End);
                    fs.Close();
                    // Если длина этой части журнала маньше заданной максимальной длины -
                    // ничего не предпринимаем
                    if (file_size < _PartSize) return;
                }
                long last_ext = _PartsCount - 1;
                // Удаляем самую старую часть журнала регистрации
                // (с наиболее ранними записями сообщений из имеющихся в журнале)
                File.Delete(_FileName + "." + last_ext.ToString() + _FileExtension);
                // Осуществляем ротацию оставшихся частей журнала регистрации
                for (long old_ext = last_ext - 1; old_ext > 0; --old_ext)
                {
                    long new_ext = old_ext + 1;
                    try
                    {
                        string sourceFile = _FileName + "." + old_ext.ToString() + _FileExtension;
                        if (File.Exists(sourceFile))
                        {
                            File.Move(
                                sourceFile,
                                _FileName + "." + new_ext.ToString() + _FileExtension);
                        }
                    }
                    catch { }
                }
                try
                {
                    File.Move(_FileName + _FileExtension, _FileName + ".1" + _FileExtension);
                }
                catch { }
                // Создаем пустой файл для продолжения журнала регистрации
                using (FileStream fs = File.Create(_FileName + _FileExtension)) { }
            }
            catch { }
        }

        #endregion

        #region Properties

        /// <summary>Имя файла, представляющего журнал регистрации</summary>
        /// <remarks>Имя файла может содержать аблолютный или относительный путь к нему</remarks>
        public string FileName
        {
            get { return _FileName + _FileExtension; }
            set 
            {
                _Lock.EnterWriteLock();
                try
                {
                    string path = string.Empty;
                    string name = Path.GetFileNameWithoutExtension(value);
                    if (!string.IsNullOrEmpty(name))
                    {
                        int index = value.IndexOf(name);
                        path = value.Substring(0, index);
                    }
                    _FileName = path + name;
                    _FileExtension = Path.GetExtension(value);
                }
                finally
                {
                    _Lock.ExitWriteLock();
                }
            }
        }

        /// <summary>Признак автоматического создания файла журнала сообщений в случае его отсутствия.
        /// Если равено FALSE - запись в файл будет осуществлятся только при его наличии.
        /// </summary>
        public bool AutoCreate
        {
            get { return _AutoCreate; }
            set { _AutoCreate = value;  }
        }

        /// <summary>Количество частей журнала регистации</summary>
        /// <remarks>
        /// Журнал регистрации разбивается на несколько частей для того, чтобы было легче
        /// удалять устаревшие сообщения из журнала, обеспечивая т.о. постоянное хранение
        /// только актуальной информации.
        /// </remarks>
        public long PartsCount
        {
            get { return _PartsCount; }
        }

        /// <summary>Максимальный размер каждой части журнала регистрации (байт)</summary>
        public long PartSize
        {
            get { return _PartSize; }
        }

        /// <summary>Определяет кодировку, в которой будет записываться сообщение в журнал регистрации
        /// <para>По умолчанию принимает значение текущей кодировки, определенной в операционной системе</para>
        /// </summary>
        public Encoding Encoding
        {
            get { return _Encoding; }
        }

        #endregion

        #region IDebugLogger Members

#pragma warning disable CS0419 // Неоднозначная ссылка в атрибуте cref: "AlfaPribor.Logs.IDebugLogger.DebugPrint". Предполагается "IDebugLogger.DebugPrint(string)", но может также соответствовать другим перегрузкам, включая "IDebugLogger.DebugPrint(string, bool)".
        /// <summary cref="AlfaPribor.Logs.IDebugLogger.DebugPrint">Реализация интерфейса IDebugLogger</summary>
        public void DebugPrint(string message)
#pragma warning restore CS0419 // Неоднозначная ссылка в атрибуте cref: "AlfaPribor.Logs.IDebugLogger.DebugPrint". Предполагается "IDebugLogger.DebugPrint(string)", но может также соответствовать другим перегрузкам, включая "IDebugLogger.DebugPrint(string, bool)".
        {
            DebugPrint(message, true);
        }

        /// <summary cref="AlfaPribor.Logs.IDebugLogger.MarkMessages">Реализация интерфейса IDebugLogger
        /// <para>По умолчанию принимает значение TRUE</para>
        /// </summary>
        public bool MarkMessages
        {
            get { return _MarkMessages; }
        }

        /// <summary cref="AlfaPribor.Logs.IDebugLogger.MarkerFormat">Реализация интерфейса IDebugLogger
        /// <para>По умолчанию принимает значение "HH:mm:ss:fff "</para>
        /// </summary>
        public string MarkerFormat
        {
            get { return _MarkerFormat; }
        }

        #endregion

        #region Events

        /// <summary>Событие "Отладочная печать"</summary>
        public event EvOnDebugPrintHandler OnDebugPrint;

        #endregion

        #region Члены IDebugLogger


#pragma warning disable CS1591 // Отсутствует комментарий XML для публично видимого типа или члена "RotateFileLogger.DebugPrint(string, bool)"
        public void DebugPrint(string message, bool printTimeMetric)
#pragma warning restore CS1591 // Отсутствует комментарий XML для публично видимого типа или члена "RotateFileLogger.DebugPrint(string, bool)"
        {
            string text = printTimeMetric==true ? DateTime.Now.ToString(_MarkerFormat) + message : message;
            if (OnDebugPrint != null)
            {
                try
                {
                    OnDebugPrint(this, new EventOnDebugPrintArgs(text));
                }
                catch { }
            }
            try
            {
                _Lock.EnterReadLock();
                try
                {
                    if (File.Exists(_FileName + _FileExtension))
                    {
                        RotateLog();
                        using (StreamWriter sw = new StreamWriter(_FileName + _FileExtension, true, _Encoding))
                        {
                            sw.WriteLine(text);
                        }
                    }
                    else if (_AutoCreate)
                    {
                        using (StreamWriter sw = new StreamWriter(_FileName + _FileExtension, false, _Encoding))
                        {
                            sw.WriteLine(text);
                        }
                    }
                }
                finally
                {
                    _Lock.ExitReadLock();
                }
            }
            catch { }
        }

        #endregion
    }

    /// <summary>Класс с дополнительной информацией о событии "Отладочная печать"</summary>
    public class EventOnDebugPrintArgs : EventArgs
    {
        #region Fields

        /// <summary>Текст сообщения</summary>
        string _Message;

        #endregion

        #region Methods

        /// <summary>Конструктор класса</summary>
        /// <param name="msg">Текст сообщения</param>
        public EventOnDebugPrintArgs(string msg)
        {
            _Message = msg;
        }

        #endregion

        #region Events

        /// <summary>Текст сообщения</summary>
        public string Message
        {
            get { return _Message; }
        }

        #endregion
    }

    /// <summary>Делегат обработчика события "Отладочная печать"</summary>
    /// <param name="sender">Объект, породивший событие</param>
    /// <param name="args">Дополнительные сведения о событии</param>
    public delegate void EvOnDebugPrintHandler(object sender, EventOnDebugPrintArgs args);
}
