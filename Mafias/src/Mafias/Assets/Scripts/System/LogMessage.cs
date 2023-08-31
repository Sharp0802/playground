using System;
using UnityEngine;

namespace Mafias.System
{
    public class LogMessage
    {
        public LogMessage(DateTime timestamp, LogType logLevel, string message)
        {
            Timestamp = timestamp;
            LogLevel = logLevel;
            Message = message;
        }

        public DateTime Timestamp { get; }
        public LogType LogLevel { get; }
        public string Message { get; }
    }
}