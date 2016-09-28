namespace Logger.Impl
{
    using System;
    using System.Diagnostics;

    using Logger.Contracts;

    /// <summary>
    /// The windows event logger class.
    /// </summary>
    public class TraceLogger : ILogger
    {
        private const string ExceptionMessage = "An exception occurred. Message: {0}. Exception: {1}";

        /// <summary>
        /// Method to log exception.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="exception">
        /// The exception.
        /// </param>
        public void LogException(string value, Exception exception)
        {
            var message = string.Format(ExceptionMessage, value, exception.ToString());
            Trace.WriteLine(message);
        }

        /// <summary>
        /// Method to log info.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        public void LogInfo(string value)
        {
            Trace.WriteLine(value);
        }
    }
}
