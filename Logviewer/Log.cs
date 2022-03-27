using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


namespace Logviewer
{
    public class Log
    {
        public string Message { get; }

        public LogType Type { get; }

        public TimeSpan Timestamp { get; }

        /// <summary>
        /// Create a log object
        /// </summary>
        /// <param name="message"></param>
        /// <param name="type">Nature of the message (log, warning, exception, etc)</param>
        /// <param name="timestamp">Time at which the message was logged (current time if unspecified)</param>
        /// <param name="stacktrace">Stacktrace if the logtype is an exception</param>
        public Log(string message, LogType type = LogType.Log, TimeSpan? timestamp = null, string stacktrace = "")
        {
            Type = type;
            if (Type == LogType.Exception && !string.IsNullOrWhiteSpace(stacktrace))
                Message = $"{message}\nstacktrace: {stacktrace}";
            else
                Message = message;

            if (timestamp.HasValue)
                Timestamp = timestamp.Value;
            else
                Timestamp = DateTime.Now.TimeOfDay;
        }

        public bool Contains(string subset) => Message.Contains(subset);

        public override string ToString()
        {
            if (Type == LogType.Log) // most likely
                return $"[{Timestamp:hh\\:mm\\:ss\\.fff}] {Message}";
            else if (Type == LogType.Warning)
                return $"<color=yellow>[{Timestamp:hh\\:mm\\:ss\\.fff}] {Message}</color>";
            else if (Type == LogType.Exception || Type == LogType.Error)
                return $"<color=red>[{Timestamp:hh\\:mm\\:ss\\.fff}] {Message}</color>";
            else
                return $"[{Timestamp:hh\\:mm\\:ss\\.fff}] {Message}";
        }

        public bool MatchFilter(string filter)
        {
            if (string.IsNullOrEmpty(filter))
                return true;

            // todo: regex
            return Contains(filter);
        }
    }
}
