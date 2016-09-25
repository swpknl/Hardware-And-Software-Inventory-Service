namespace Constants
{
    using System;

    /// <summary>
    /// The current system time.
    /// </summary>
    public class CurrentSystemTime
    {
        /// <summary>
        /// Gets the current system time.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public string GetCurrentSystemTime()
        {
            return DateTime.Now.ToString("HH:mm:ss tt").Substring(0, 8);
        }
    }
}
