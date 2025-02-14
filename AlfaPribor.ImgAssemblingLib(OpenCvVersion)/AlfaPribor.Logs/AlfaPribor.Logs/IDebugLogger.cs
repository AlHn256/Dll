using System;
using System.Collections.Generic;
using System.Text;

namespace AlfaPribor.Logs
{
    /// <summary>Интерфейс ведения журнала регистрации отладочных сообщений</summary>
    public interface IDebugLogger
    {
        /// <summary>Записывает отладочное сообщение в журнал</summary>
        /// <param name="message">Ссылка на строку с отладочным сообщеним</param>
        /// <remarks>Метод должен перехватывать любые исключения, возникающие в нем</remarks>
        void DebugPrint(string message);

        /// <summary>
        /// Записывает отладочное сообщение в журнал
        /// </summary>
        /// <param name="message">Сообщение для вывода</param>
        /// <param name="printTimeMetric">Ставить ли метку времени при выводе сообщений</param>
        void DebugPrint(string message,bool printTimeMetric);

        /// <summary>Признак выполнения маркировки сообщений, помещаемых в журнал</summary>
        bool MarkMessages
        {
            get;
        }

        /// <summary>Строка форматирования даты/времени, которыми маркируются сообщения, помещаемые в журнал</summary>
        string MarkerFormat
        {
            get;
        }

        /// <summary>Определяет кодировку, в которой будут записываться сообщения в журнал регистрации</summary>
        Encoding Encoding
        {
            get;
        }
       
    }

    /// <summary>Тип сообщения</summary>
    public enum MessageType
    {
        /// <summary>Тип сообщения не определен</summary>
        Unknown = 0,

        /// <summary>Информационное сообщение</summary>
        Information,

        /// <summary>Сообщение, требующее особого внимания; предупреждение</summary>
        Warning,

        /// <summary>Сообщение об ошибке</summary>
        Error
    }

    /// <summary>Интерфейс ведения журнала отладочных сообщений, различающихся по типам</summary>
    public interface ITypedDebugLogger : IDebugLogger
    {
        /// <summary>Записывает отладочное сообщение в журнал</summary>
        /// <param name="message">Ссылка на строку с отладочным сообщеним</param>
        /// <param name="type">Тип сообщения</param>
        /// <remarks>Метод должен перехватывать любые исключения, возникающие в нем</remarks>
        void DebugPrint(string message, MessageType type);

        /// <summary>
        /// Записывает отладочное сообщение в журнал
        /// </summary>
        /// <param name="message">Записываемое сообщение</param>
        /// <param name="type">Тип сообщения</param>
        /// <param name="printTimeMetric">выводить ли метку времени</param>
        void DebugPrint(string message, MessageType type, bool printTimeMetric);
    }
}
