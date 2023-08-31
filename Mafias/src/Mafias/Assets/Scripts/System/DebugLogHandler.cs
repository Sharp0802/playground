using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Mafias.System
{
    public sealed class DebugLogHandler : ILogHandler
    {
        private static readonly object Locker = new();
        private static readonly Queue<LogMessage> MessageQueue = new();

        static DebugLogHandler()
        {
            Debug.unityLogger.logHandler = new DebugLogHandler();
        }

        public void LogFormat(LogType logType, Object context, string format, params object[] args)
        {
            lock (Locker)
            {
                MessageQueue.Enqueue(new LogMessage(DateTime.Now, logType, string.Format(format, args)));
            }
        }

        public void LogException(Exception exception, Object context)
        {
            lock (Locker)
            {
                MessageQueue.Enqueue(new LogMessage(DateTime.Now, LogType.Exception, exception.ToString()));
            }
        }

        public static LogMessage[] FlushMessages()
        {
            lock (Locker)
            {
                var arr = MessageQueue.ToArray();
                MessageQueue.Clear();
                return arr;
            }
        }
    }
}