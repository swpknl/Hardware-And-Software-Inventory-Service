namespace PopulateWMIInfo.HardwareClasses
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Management;

    using Constants;

    using Entities;

    using Helpers;

    using Newtonsoft.Json.Linq;

    using PopulateWMIInfo.Contracts;

    using ReportToRestEndpoint.Contracts;

    public class OperatingSystem : IWmiInfo
    {
        private const string OperatingSystemTableName = "os";

        private const string OperatingSystemClientTableName = "x_client_os";

        private ManagementObjectSearcher seacher;

        private List<OperatingSystemInfo> operatingSystemInfoList;

        private ManagementObjectSearcher softwareLicensingServiceSearcher;

        private string oa3XOriginalProductKey;

        private int id;

        /// <summary>
        /// Initializes a new instance of the <see cref="OperatingSystem"/> class.
        /// </summary>
        public OperatingSystem()
        {
            this.operatingSystemInfoList = new List<OperatingSystemInfo>();
            this.seacher = new ManagementObjectSearcher(
                WmiConstants.WmiRootNamespace,
                string.Format(
                    "SELECT {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12} FROM Win32_OperatingSystem",
                    WmiConstants.OSType,
                    WmiConstants.Caption,
                    WmiConstants.Manufacturer,
                    WmiConstants.Version,
                    WmiConstants.CSDVersion,
                    WmiConstants.SerialNumber,
                    WmiConstants.OSArchitecture,
                    WmiConstants.OperatingSystemSKU,
                    WmiConstants.Locale,
                    WmiConstants.CountryCode,
                    WmiConstants.Organization,
                    WmiConstants.SystemDirectory,
                    WmiConstants.OSLanguage));
            this.softwareLicensingServiceSearcher = new ManagementObjectSearcher(
                WmiConstants.WmiMS409Namespace,
                "SELECT * FROM SoftwareLicensingService");
        }

        /// <summary>
        /// The get WMI info.
        /// </summary>
        public void GetWMIInfo()
        {
            this.operatingSystemInfoList = this.GetValue();
            this.oa3XOriginalProductKey = this.GetSoftwareLicensingServiceValue();
        }

        /// <summary>
        /// The report WMI info.
        /// </summary>
        /// <param name="visitor">
        /// The visitor.
        /// </param>
        public void ReportWMIInfo(IVisitor visitor)
        {
            var data = this.ConvertOperatingSystemInfoToJson(this.operatingSystemInfoList);
            visitor.Visit(OperatingSystemTableName, data, out id);
            data = this.ConvertClientOperatingSystemInfoToJson(
                this.operatingSystemInfoList,
                this.oa3XOriginalProductKey);
            int temp;
            visitor.Visit(OperatingSystemClientTableName, data, out temp);
        }

        /// <summary>
        /// The check for hardware changes.
        /// </summary>
        /// <param name="visitor">
        /// The visitor.
        /// </param>
        public void CheckForHardwareChanges(IVisitor visitor)
        {
            var changedHardwareList = new List<OperatingSystemInfo>();
            var tempList = this.GetValue();
            changedHardwareList = tempList.GetDifference(this.operatingSystemInfoList);
            var tempValue = this.GetSoftwareLicensingServiceValue();
            var changedHardwareValue = tempValue.Equals(
                this.oa3XOriginalProductKey,
                System.StringComparison.OrdinalIgnoreCase)
                                           ? string.Empty
                                           : tempValue;
            int temp;
            if (changedHardwareList.Any() && changedHardwareValue != string.Empty)
            {
                var data = this.ConvertOperatingSystemInfoToJson(changedHardwareList);
                visitor.Visit(OperatingSystemTableName, data, out id);
                data = this.ConvertClientOperatingSystemInfoToJson(changedHardwareList, changedHardwareValue);
                visitor.Visit(OperatingSystemClientTableName, data, out temp);
            }
            else if (changedHardwareList.Any() && changedHardwareValue == string.Empty)
            {
                var data = this.ConvertOperatingSystemInfoToJson(changedHardwareList);
                visitor.Visit(OperatingSystemTableName, data, out id);
                data = this.ConvertClientOperatingSystemInfoToJson(changedHardwareList, string.Empty);
                visitor.Visit(OperatingSystemClientTableName, data, out temp);
            }
            else if (changedHardwareList.Any() == false && changedHardwareValue != string.Empty)
            {
                var data = this.ConvertOperatingSystemInfoToJson(new List<OperatingSystemInfo>());
                visitor.Visit(OperatingSystemTableName, data, out id);
                data = this.ConvertClientOperatingSystemInfoToJson(
                    new List<OperatingSystemInfo>(),
                    changedHardwareValue);
                visitor.Visit(OperatingSystemClientTableName, data, out temp);
            }
        }

        /// <summary>
        /// The get value.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        private List<OperatingSystemInfo> GetValue()
        {
            var tempList = new List<OperatingSystemInfo>();
            try
            {
                foreach (var queryObject in this.seacher.Get())
                {
                    var operatingSystemInfo = new OperatingSystemInfo
                    {
                        OSType = queryObject[WmiConstants.OSType],
                        Caption = queryObject[WmiConstants.Caption],
                        Manufacturer = queryObject[WmiConstants.Manufacturer],
                        Version = queryObject[WmiConstants.Version],
                        CSDVersion = queryObject[WmiConstants.CSDVersion],
                        SerialNumber = queryObject[WmiConstants.SerialNumber],
                        OSArchitecture = queryObject[WmiConstants.OSArchitecture],
                        OperatingSystemSKU = queryObject[WmiConstants.OperatingSystemSKU],
                        Locale = queryObject[WmiConstants.Locale],
                        CountryCode = queryObject[WmiConstants.CountryCode],
                        OSLanguage = queryObject[WmiConstants.OSLanguage],
                        SystemDirectory = queryObject[WmiConstants.SystemDirectory],
                        Organization = queryObject[WmiConstants.Organization]
                    };
                    tempList.Add(operatingSystemInfo);
                }
            }
            catch (Exception)
            {
                throw;
            }
            
            return tempList;
        }

        /// <summary>
        /// The get software licensing service value.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private string GetSoftwareLicensingServiceValue()
        {
            var tempValue = string.Empty;
            foreach (var queryObject in this.softwareLicensingServiceSearcher.Get())
            {
                tempValue = queryObject[WmiConstants.OA3xOriginalProductKey].ToString();
            }

            return tempValue;
        }

        /// <summary>
        /// The convert operating system info to json.
        /// </summary>
        /// <param name="operatingSystemInfoList">
        /// The operating system info list.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private string ConvertOperatingSystemInfoToJson(List<OperatingSystemInfo> operatingSystemInfoList)
        {
            var data =
                new JObject(
                    new JProperty(
                        "resource",
                        new JArray(
                            from operatingSystemInfo in operatingSystemInfoList
                            select
                                new JObject(
                                //new JProperty("os_type_id", operatingSystemInfo.OSType),
                                new JProperty("manufacturer_id", operatingSystemInfo.Manufacturer),
                                new JProperty("version", operatingSystemInfo.Version),
                                new JProperty("version_info_id", operatingSystemInfo.CSDVersion),
                                new JProperty("serial_number", operatingSystemInfo.SerialNumber),
                                new JProperty("os_architecture", operatingSystemInfo.OSArchitecture),
                                new JProperty("os_sku", operatingSystemInfo.OperatingSystemSKU)))));
            return data.ToString();
        }

        /// <summary>
        /// The convert client operating system info to json.
        /// </summary>
        /// <param name="operatingSystemInfoList">
        /// The operating system info list.
        /// </param>
        /// <param name="oa3xOriginalProductKey">
        /// The oa 3 x original product key.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private string ConvertClientOperatingSystemInfoToJson(List<OperatingSystemInfo> operatingSystemInfoList, string oa3xOriginalProductKey = null)
        {
            var data =
                new JObject(
                    new JProperty(
                        "resource",
                        new JArray(
                            from operatingSystemInfo in operatingSystemInfoList
                            select
                                new JObject(
                                new JProperty("locale", operatingSystemInfo.Locale),
                                new JProperty("country", operatingSystemInfo.CountryCode),
                                new JProperty("system_ui_language", operatingSystemInfo.OSLanguage),
                                new JProperty("company_id", operatingSystemInfo.Organization),
                                new JProperty("system_directory_id", operatingSystemInfo.SystemDirectory),
                                new JProperty("cd_key", oa3xOriginalProductKey),
                                new JProperty("client_id", ClientId.Id),
                                new JProperty("os_id", this.id)))));
            return data.ToString();
        }
    }
}
