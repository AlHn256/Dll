using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlfaPribor.Logs
{
    /// <summary>
    /// Позволяет вести журнал регистрации отладочных сообщений, размер которого имеет регулируемое значение.
    /// Сообщения различаются по типу.
    /// </summary>
    /// <remarks>
    /// !!! Все свойства и методы класса являются потокобезопасными !!!
    /// </remarks>
    public class TypedRotateFileLog : RotateFileLogger, ITypedDebugLogger
    {
        /// <summary>Конструктор класса</summary>
        /// <param name="parts_count">Количество частей (файлов), на которые будет делиться журнал регистрации</param>
        /// <param name="part_size">Максимальная длина в байтах каждого файла (части) журнала регистрации</param>
        /// <param name="marker_format">Строка форматирования даты/времени, которыми должны маркироваться сообщения, помещаемые в журнал</param>
        public TypedRotateFileLog(long parts_count, long part_size, string marker_format):
            base(parts_count, part_size, marker_format) { }

        /// <summary>Конструктор класса</summary>
        /// <param name="parts_count">Количество частей (файлов), на которые будет делиться журнал регистрации</param>
        /// <param name="part_size">Максимальная длина в байтах каждого файла (части) журнала регистрации</param>
        /// <param name="encoding">Определяет кодировку, в которой будет записываться сообщение в журнал регистрации</param>
        public TypedRotateFileLog(long parts_count, long part_size, Encoding encoding)
            : base(parts_count, part_size, encoding) { }

        /// <summary>Конструктор класса</summary>
        /// <param name="parts_count">Количество частей (файлов), на которые будет делиться журнал регистрации</param>
        /// <param name="part_size">Максимальная длина в байтах каждого файла (части) журнала регистрации</param>
        /// <param name="marker_format">Строка форматирования даты/времени, которыми должны маркироваться сообщения, помещаемые в журнал</param>
        /// <param name="encoding">Определяет кодировку, в которой будет записываться сообщение в журнал регистрации</param>
        public TypedRotateFileLog(long parts_count, long part_size, string marker_format, Encoding encoding) :
            base(parts_count, part_size, marker_format, encoding) { }

        /// <summary>Конструктор класса</summary>
        /// <param name="parts_count">Количество частей (файлов), на которые будет делиться журнал регистрации</param>
        /// <param name="part_size">Максимальная длина в байтах каждого файла (части) журнала регистрации</param>
        public TypedRotateFileLog(long parts_count, long part_size) :
            base(parts_count, part_size) { }

        #region Члены ITypedDebugLogger

#pragma warning disable CS0419 // Неоднозначная ссылка в атрибуте cref: "AlfaPribor.Logs.ITypedDebugLogger.DebugPrint". Предполагается "ITypedDebugLogger.DebugPrint(string, MessageType)", но может также соответствовать другим перегрузкам, включая "ITypedDebugLogger.DebugPrint(string, MessageType, bool)".
        /// <summary cref="AlfaPribor.Logs.ITypedDebugLogger.DebugPrint">Записывает отладочное сообщение в журнал</summary>
        /// <param name="message">Ссылка на строку с отладочным сообщеним</param>
        /// <param name="type">Тип сообщения</param>
        public void DebugPrint(string message, MessageType type)
#pragma warning restore CS0419 // Неоднозначная ссылка в атрибуте cref: "AlfaPribor.Logs.ITypedDebugLogger.DebugPrint". Предполагается "ITypedDebugLogger.DebugPrint(string, MessageType)", но может также соответствовать другим перегрузкам, включая "ITypedDebugLogger.DebugPrint(string, MessageType, bool)".
        {
            DebugPrint(message, type, true);
        }

        #endregion

        #region Члены ITypedDebugLogger


#pragma warning disable CS1591 // Отсутствует комментарий XML для публично видимого типа или члена "TypedRotateFileLog.DebugPrint(string, MessageType, bool)"
        public void DebugPrint(string message, MessageType type, bool printTimeMetric)
#pragma warning restore CS1591 // Отсутствует комментарий XML для публично видимого типа или члена "TypedRotateFileLog.DebugPrint(string, MessageType, bool)"
        {
            string typedMessage;
            switch (type)
            {
                case MessageType.Information:
                    typedMessage = "[Сообщение] " + message;
                    break;
                case MessageType.Warning:
                    typedMessage = "[Предупреждение] " + message;
                    break;
                case MessageType.Error:
                    typedMessage = "[Ошибка] " + message;
                    break;
                default:
                    typedMessage = message;
                    break;
            }
            base.DebugPrint(typedMessage,printTimeMetric);
        }

        #endregion
    }
}
