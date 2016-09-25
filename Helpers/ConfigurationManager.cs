namespace Helpers
{
    using System;

    /// <summary>
    /// The configuration manager.
    /// </summary>
    public static class ConfigurationManager
    {
        /// <summary>
        /// The get configuration value.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string TryGetConfigurationValue(string key)
        {
            try
            {
                var value = System.Configuration.ConfigurationManager.AppSettings[key];
                return value;
            }
            catch (Exception)
            {
                return string.Empty;
            }
            
        }
        
    }
}
