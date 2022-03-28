using System;
using System.Text.RegularExpressions;
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

        /// <summary>
        /// Whether the pattern matches <see cref="Message"/>
        /// </summary>
        /// <param name="pattern">The regex expression that should be matched</param>
        /// <returns></returns>
        public bool MatchesRegex(string pattern) => Regex.IsMatch(Message, pattern);

        public override string ToString()
        {
            if (Type == LogType.Log) // same as default, but log is the most likely type so it goes first
                return $"[{Timestamp:hh\\:mm\\:ss\\.fff}] {Message}";
            else if (Type == LogType.Warning)
                return $"<color=yellow>[{Timestamp:hh\\:mm\\:ss\\.fff}] {Message}</color>";
            else if (Type == LogType.Exception || Type == LogType.Error)
                return $"<color=red>[{Timestamp:hh\\:mm\\:ss\\.fff}] {Message}</color>";
            else
                return $"[{Timestamp:hh\\:mm\\:ss\\.fff}] {Message}";
        }

        /// <summary>
        /// If there is no filter return true, else return whether or not the message is matched
        /// </summary>
        public bool MatchFilter(string filter)
        {
            if (string.IsNullOrEmpty(filter))
                return true;

            return MatchesRegex(filter);
        }
    }
}
