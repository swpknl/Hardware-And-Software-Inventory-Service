namespace PopulateRegistryInformation.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Constants;

    using Entities;

    using Helpers;

    using Logger.Contracts;

    using Microsoft.Win32;

    using Newtonsoft.Json.Linq;

    using ReportToRestEndpoint.Contracts;

    /// <summary>
    /// The populate registry info abstract base.
    /// </summary>
    internal abstract class PopulateRegistryInfoAbstractBase : IPopulateRegistryInfo
    {
        private const string TableName = "software";

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
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(this.registryKey))
                {
                    foreach (var subKeyName in key.GetSubKeyNames())
                    {
                        var tempRegistryKey = key.OpenSubKey(subKeyName);
                        var registryInfo = new RegistryInfo();
                        var publisher = tempRegistryKey.GetValue(RegistryPropertiesConstants.Publisher, null);
                        if (publisher != null)
                        {
                            registryInfo.Publisher = publisher.ToString().RemoveSpecialCharacters();
                        }
                            
                        var displayName = tempRegistryKey.GetValue(RegistryPropertiesConstants.DisplayName, null);
                        if (displayName != null)
                        {
                            registryInfo.DisplayName = displayName.ToString().RemoveSpecialCharacters();
                        }

                        var displayVersion = tempRegistryKey.GetValue(RegistryPropertiesConstants.DisplayVersion, null);
                        if (displayVersion != null)
                        {
                            registryInfo.DisplayVersion = displayVersion.ToString().RemoveSpecialCharacters();
                        }

                        var installDate = tempRegistryKey.GetValue(RegistryPropertiesConstants.InstallDate, null);
                        if (installDate != null)
                        {
                            DateTime temp;
                            if (DateTime.TryParse(installDate.ToString(), out temp))
                            {
                                registryInfo.InstallDate = temp.ToShortDateString();
                            }
                            else
                            {
                                registryInfo.InstallDate = null;
                            }
                        }

                        var installLocation = tempRegistryKey.GetValue(RegistryPropertiesConstants.InstallLocation, null);
                        if (installLocation != null)
                        {
                            registryInfo.InstallLocation = installLocation.ToString().RemoveSpecialCharacters();
                        }

                        var displayIcon = tempRegistryKey.GetValue(RegistryPropertiesConstants.DisplayIcon, null);
                        if (displayIcon != null)
                        {
                            registryInfo.DisplayIcon = displayIcon.ToString().RemoveSpecialCharacters();
                        }

                        var estimatedSize = tempRegistryKey.GetValue(RegistryPropertiesConstants.EstimatedSize, null);
                        if (estimatedSize != null)
                        {
                            registryInfo.EstimatedSize = estimatedSize.ToString().RemoveSpecialCharacters();
                        }

                        var urlInfoAbout = tempRegistryKey.GetValue(RegistryPropertiesConstants.URLInfoAbout, null);
                        if (urlInfoAbout != null)
                        {
                            registryInfo.URLInfoAbout = urlInfoAbout.ToString().RemoveSpecialCharacters();
                        }

                        var urlUpdateInfo = tempRegistryKey.GetValue(RegistryPropertiesConstants.URLUpdateInfo, null);
                        if (urlUpdateInfo != null)
                        {
                            registryInfo.URLUpdateInfo = urlUpdateInfo.ToString().RemoveSpecialCharacters();
                        }

                        var helpLink = tempRegistryKey.GetValue(RegistryPropertiesConstants.HelpLink, null);
                        if (helpLink != null)
                        {
                            registryInfo.HelpLink = helpLink.ToString().RemoveSpecialCharacters();
                        }

                        this.registryInfoList.Add(registryInfo);
                    }
                }
            }
            catch (Exception ex)
            {
                this.logger.LogException("An exception occurred while reading the registry values", ex);
            }
        }

        /// <summary>
        /// The report registry info.
        /// </summary>
        /// <param name="visitor">
        /// The visitor.
        /// </param>
        public void ReportRegistryInfo(IVisitor visitor)
        {
            var data = this.ConvertRegistryInfoToJson();
            int temp;
            visitor.Visit(TableName, data, out temp);
        }

        /// <summary>
        /// The convert registry info to json.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private string ConvertRegistryInfoToJson()
        {
            var data = new JObject(
                new JProperty(
                    "resource",
                    new JArray(
                        from registryInfo in this.registryInfoList
                        select
                            new JObject(
                            new JProperty("manufacturer", registryInfo.Publisher),
                            new JProperty("product_name", registryInfo.DisplayName),
                            new JProperty("product_version", registryInfo.DisplayVersion),
                            new JProperty("install_date", registryInfo.InstallDate),
                            new JProperty("executable_path", registryInfo.InstallLocation),
                            new JProperty("icon_path", registryInfo.DisplayIcon),
                            new JProperty("size", registryInfo.EstimatedSize),
                            new JProperty("url_info_about", registryInfo.URLInfoAbout),
                            new JProperty("url_update_info", registryInfo.URLUpdateInfo),
                            new JProperty("help_link", registryInfo.HelpLink)))));
            return data.ToString();
        }
    }
}
