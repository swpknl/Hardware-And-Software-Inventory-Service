namespace PopulateRegistryInformation.Contracts
{
    using System;
    using System.Collections.Generic;

    using Constants;

    using Entities;

    using Logger.Contracts;

    using Microsoft.Win32;

    using ReportToRestEndpoint.Contracts;

    /// <summary>
    /// The populate registry info abstract base.
    /// </summary>
    internal abstract class PopulateRegistryInfoAbstractBase : IPopulateRegistryInfo
    {
        protected List<RegistryInfo> registryInfoList;

        private string registryKey;

        private ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="PopulateRegistryInfoAbstractBase"/> class.
        /// </summary>
        /// <param name="registryKey">
        /// The registry key.
        /// </param>
        /// <param name="logger">
        /// The logger.
        /// </param>
        protected PopulateRegistryInfoAbstractBase(string registryKey, ILogger logger)
        {
            this.logger = logger;
            this.registryInfoList = new List<RegistryInfo>();
            this.registryKey = registryKey;
        }

        /// <summary>
        /// The populate registry info.
        /// </summary>
        public void PopulateRegistryInfo()
        {
            try
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(registryKey))
                {
                    foreach (var subKeyName in key.GetSubKeyNames())
                    {
                        var tempRegistryKey = key.OpenSubKey(subKeyName);
                        var registryInfo = new RegistryInfo
                                                {
                                                    Publisher = tempRegistryKey.GetValue(RegistryPropertiesConstants.Publisher, null),
                                                    DisplayName = tempRegistryKey.GetValue(RegistryPropertiesConstants.DisplayName, null),
                                                    DisplayVersion = tempRegistryKey.GetValue(RegistryPropertiesConstants.DisplayVersion, null),
                                                    InstallDate = tempRegistryKey.GetValue(RegistryPropertiesConstants.InstallDate, null),
                                                    InstallLocation = tempRegistryKey.GetValue(RegistryPropertiesConstants.InstallLocation, null),
                                                    DisplayIcon = tempRegistryKey.GetValue(RegistryPropertiesConstants.DisplayIcon, null),
                                                    EstimatedSize = tempRegistryKey.GetValue(RegistryPropertiesConstants.EstimatedSize, null),
                                                    URLInfoAbout = tempRegistryKey.GetValue(RegistryPropertiesConstants.URLInfoAbout, null),
                                                    URLUpdateInfo = tempRegistryKey.GetValue(RegistryPropertiesConstants.URLUpdateInfo, null),
                                                    HelpLink = tempRegistryKey.GetValue(RegistryPropertiesConstants.HelpLink,null)
                                                };
                        this.registryInfoList.Add(registryInfo);

                    }
                }
            }
            catch (Exception ex)
            {
                this.logger.LogException("An exception occurred while reading the registry values", ex);
            }
        }


        public void ReportRegistryInfo(IVisitor visitor)
        {
            
        }
        
    }
}
