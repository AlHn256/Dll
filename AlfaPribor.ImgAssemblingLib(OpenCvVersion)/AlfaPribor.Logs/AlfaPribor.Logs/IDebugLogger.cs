using System;
using System.Collections.Generic;
using System.Text;

namespace AlfaPribor.Logs
{
    /// <summary>��������� ������� ������� ����������� ���������� ���������</summary>
    public interface IDebugLogger
    {
        /// <summary>���������� ���������� ��������� � ������</summary>
        /// <param name="message">������ �� ������ � ���������� ���������</param>
        /// <remarks>����� ������ ������������� ����� ����������, ����������� � ���</remarks>
        void DebugPrint(string message);

        /// <summary>
        /// ���������� ���������� ��������� � ������
        /// </summary>
        /// <param name="message">��������� ��� ������</param>
        /// <param name="printTimeMetric">������� �� ����� ������� ��� ������ ���������</param>
        void DebugPrint(string message,bool printTimeMetric);

        /// <summary>������� ���������� ���������� ���������, ���������� � ������</summary>
        bool MarkMessages
        {
            get;
        }

        /// <summary>������ �������������� ����/�������, �������� ����������� ���������, ���������� � ������</summary>
        string MarkerFormat
        {
            get;
        }

        /// <summary>���������� ���������, � ������� ����� ������������ ��������� � ������ �����������</summary>
        Encoding Encoding
        {
            get;
        }
       
    }

    /// <summary>��� ���������</summary>
    public enum MessageType
    {
        /// <summary>��� ��������� �� ���������</summary>
        Unknown = 0,

        /// <summary>�������������� ���������</summary>
        Information,

        /// <summary>���������, ��������� ������� ��������; ��������������</summary>
        Warning,

        /// <summary>��������� �� ������</summary>
        Error
    }

    /// <summary>��������� ������� ������� ���������� ���������, ������������� �� �����</summary>
    public interface ITypedDebugLogger : IDebugLogger
    {
        /// <summary>���������� ���������� ��������� � ������</summary>
        /// <param name="message">������ �� ������ � ���������� ���������</param>
        /// <param name="type">��� ���������</param>
        /// <remarks>����� ������ ������������� ����� ����������, ����������� � ���</remarks>
        void DebugPrint(string message, MessageType type);

        /// <summary>
        /// ���������� ���������� ��������� � ������
        /// </summary>
        /// <param name="message">������������ ���������</param>
        /// <param name="type">��� ���������</param>
        /// <param name="printTimeMetric">�������� �� ����� �������</param>
        void DebugPrint(string message, MessageType type, bool printTimeMetric);
    }
}
