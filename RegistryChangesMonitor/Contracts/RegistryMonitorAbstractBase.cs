namespace RegistryChangesMonitor.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    using Constants;

    using Entities;

    using Helpers;

    using Logger.Contracts;

    using Microsoft.Win32;

    using Newtonsoft.Json.Linq;

    using RegistryChangesMonitor.Impl;

    using ReportToRestEndpoint.Contracts;

    /// <summary>
    /// The registry monitor abstract base.
    /// </summary>
    internal abstract class RegistryMonitorAbstractBase : IRegistryMonitor
    {
        private const string TableName = "software";

        private RegistryKeyChange regKeyMonitor;        

        private RegistryTreeChange regTreeMonitor;

        private ILogger logger;

        private IVisitor visitor;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistryMonitorAbstractBase"/> class.
        /// </summary>
        /// <param name="hive">
        /// The hive.
        /// </param>
        /// <param name="keyName">
        /// The key Name.
        /// </param>
        protected RegistryMonitorAbstractBase(string hive, string keyName, ILogger logger, IVisitor visitor)
        {
            this.regKeyMonitor = new RegistryKeyChange(hive, keyName);
            this.regKeyMonitor.RegistryKeyChanged += this.RegMonitor_RegistryKeyChanged;
            this.regTreeMonitor = new RegistryTreeChange(hive, keyName);
            this.regTreeMonitor.RegistryTreeChanged += this.RegTreeMonitor_RegistryTreeChanged;
            this.logger = logger;
            this.visitor = visitor;
        }

        /// <summary>
        /// The begin registry monitoring.
        /// </summary>
        public void BeginRegistryMonitoring()
        {
            this.regKeyMonitor.Start();
            this.regTreeMonitor.Start();
        }

        /// <summary>
        /// The stop registry monitoring.
        /// </summary>
        public void StopRegistryMonitoring()
        {
            this.regKeyMonitor.Stop();
            this.regTreeMonitor.Stop();
        }

        /// <summary>
        /// The report registry changes.
        /// </summary>
        /// <param name="visitor">
        /// The visitor.
        /// </param>
        /// <param name="data">
        /// The data.
        /// </param>
        public void ReportRegistryChanges(string data)
        {
            int temp;
            this.visitor.Visit(TableName, data, out temp);
        }

        /// <summary>
        /// The registry tree changed event.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The event.
        /// </param>
        private void RegTreeMonitor_RegistryTreeChanged(object sender, RegistryTreeChangedEventArgs e)
        {
            var message = string.Format(
                "Registry key changed for hive: {0} and Rootpath: {1}",
                e.RegistryTreeChangeData.Hive,
                e.RegistryTreeChangeData.RootPath);
            this.logger.LogInfo(message);
            try
            {
                var registryInfoList = new List<RegistryInfo>();
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(e.RegistryTreeChangeData.RootPath))
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
                            HelpLink = tempRegistryKey.GetValue(RegistryPropertiesConstants.HelpLink, null).ToString().RemoveSpecialCharacters()
                        };
                        registryInfoList.Add(registryInfo);
                    }
                }
                var data = this.ConvertRegistryInfoToJson(registryInfoList);
                this.ReportRegistryChanges(data);
            }
            catch (Exception ex)
            {
                this.logger.LogException(
                    "An exception occurred while reading the registry for " + e.RegistryTreeChangeData.RootPath,
                    ex);
            }
        }

        /// <summary>
        /// The registry key changed event.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The event.
        /// </param>
        private void RegMonitor_RegistryKeyChanged(object sender, RegistryKeyChangedEventArgs e)
        {
            var message = string.Format(
                "Registry key changed for hive: {0} and key: {1}",
                e.RegistryKeyChangeData.Hive,
                e.RegistryKeyChangeData.KeyPath);
            this.logger.LogInfo(message);
            try
            {
                var registryInfoList = new List<RegistryInfo>();
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(e.RegistryKeyChangeData.KeyPath))
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
                        registryInfoList.Add(registryInfo);
                    }
                }
                var data = this.ConvertRegistryInfoToJson(registryInfoList);
                this.ReportRegistryChanges(data);
            }
            catch (Exception ex)
            {
                this.logger.LogException(
                    "An exception occurred while reading the registry value for " + e.RegistryKeyChangeData.KeyPath,
                    ex);
            }
        }

        /// <summary>
        /// The convert registry info to json.
        /// </summary>
        /// <param name="registryInfoList">
        /// The registry info list.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private string ConvertRegistryInfoToJson(List<RegistryInfo> registryInfoList)
        {
            var data = new JObject(
                new JProperty(
                    "resource",
                    new JArray(
                        from registryInfo in registryInfoList
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