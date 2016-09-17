namespace Logger.Impl
{
    using System;

    using Logger.Contracts;

    /// <summary>
    /// The windows event logger class.
    /// </summary>
    public class WindowsEventLogger : ILogger
    {
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
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Method to log info.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        public void LogInfo(string value)
        {
            //throw new NotImplementedException();
        }
    }
}
