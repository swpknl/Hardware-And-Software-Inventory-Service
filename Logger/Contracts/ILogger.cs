namespace Logger.Contracts
{
    using System;

    /// <summary>
    /// The Logger interface.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Method to log. 
        /// </summary>
        /// <param name="value">
        /// The value to be logged.
        /// </param>
        /// <param name="exception">
        /// Exception object
        /// </param>
        void LogException(string value, Exception exception);

        /// <summary>
        /// Method to log information.
        /// </summary>
        /// <param name="value">
        /// The value to be logged.
        /// </param>
        void LogInfo(string value);
    }
}
