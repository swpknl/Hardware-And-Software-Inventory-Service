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
                        var registryInfo = new RegistryInfo
                                                {
                                                    Publisher = tempRegistryKey.GetValue(RegistryPropertiesConstants.Publisher, null).ToString().RemoveSpecialCharacters(),
                                                    DisplayName = tempRegistryKey.GetValue(RegistryPropertiesConstants.DisplayName, null).ToString().RemoveSpecialCharacters(),
                                                    DisplayVersion = tempRegistryKey.GetValue(RegistryPropertiesConstants.DisplayVersion, null).ToString().RemoveSpecialCharacters(),
                                                    InstallDate = tempRegistryKey.GetValue(RegistryPropertiesConstants.InstallDate, null).ToString().RemoveSpecialCharacters(),
                                                    InstallLocation = tempRegistryKey.GetValue(RegistryPropertiesConstants.InstallLocation, null).ToString().RemoveSpecialCharacters(),
                                                    DisplayIcon = tempRegistryKey.GetValue(RegistryPropertiesConstants.DisplayIcon, null).ToString().RemoveSpecialCharacters(),
                                                    EstimatedSize = tempRegistryKey.GetValue(RegistryPropertiesConstants.EstimatedSize, null).ToString().RemoveSpecialCharacters(),
                                                    URLInfoAbout = tempRegistryKey.GetValue(RegistryPropertiesConstants.URLInfoAbout, null).ToString().RemoveSpecialCharacters(),
                                                    URLUpdateInfo = tempRegistryKey.GetValue(RegistryPropertiesConstants.URLUpdateInfo, null).ToString().RemoveSpecialCharacters(),
                                                    HelpLink = tempRegistryKey.GetValue(RegistryPropertiesConstants.HelpLink,null).ToString().RemoveSpecialCharacters()
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

        /// <summary>
        /// The report registry info.
        /// </summary>
        /// <param name="visitor">
        /// The visitor.
        /// </param>
        public void ReportRegistryInfo(IVisitor visitor)
        {
            var data = this.ConvertRegistryInfoToJson();
            visitor.Visit(TableName, data);
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
                    "resources",
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
