namespace PopulateWMIInfo.Rules
{
    using System.Collections.Generic;
    using System.Management;

    using Constants;

    using Entities;

    using PopulateWMIInfo.Contracts;

    public class OperatingSystem : IWmiInfo
    {
        private ManagementObjectSearcher seacher;

        private readonly List<OperatingSystemInfo> operatingSystemInfoList;

        /// <summary>
        /// Initializes a new instance of the <see cref="OperatingSystem"/> class.
        /// </summary>
        public OperatingSystem()
        {
            this.operatingSystemInfoList = new List<OperatingSystemInfo>();
            this.seacher = new ManagementObjectSearcher(
                WmiConstants.WmiRootNamespace,
                string.Format(
                    "SELECT {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11} FROM Win32_OperatingSystem",
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
                    WmiConstants.SystemDirectory));
        }

        /// <summary>
        /// The get WMI info.
        /// </summary>
        public void GetWMIInfo()
        {
            foreach (var queryObject in this.seacher.Get())
            {
                var operatingSystemInfo = new OperatingSystemInfo
                                              {
                                                  OSType = queryObject[WmiConstants.OSType],
                                                  Caption = queryObject[WmiConstants.Caption],
                                                  Manufacturer =
                                                      queryObject[WmiConstants.Manufacturer],
                                                  Version = queryObject[WmiConstants.Version],
                                                  CSDVersion = queryObject[WmiConstants.CSDVersion],
                                                  SerialNumber =
                                                      queryObject[WmiConstants.SerialNumber],
                                                  OSArchitecture =
                                                      queryObject[WmiConstants.OSArchitecture],
                                                  OperatingSystemSKU =
                                                      queryObject[WmiConstants.OperatingSystemSKU],
                                                  Locale = queryObject[WmiConstants.Locale],
                                                  CountryCode = queryObject[WmiConstants.CountryCode],
                                                  OSLanguage = queryObject[WmiConstants.Organization],
                                                  SystemDirectory =
                                                      queryObject[WmiConstants.SystemDirectory],
                                                  Organization =
                                                      queryObject[WmiConstants.Organization]
                                              };
                this.operatingSystemInfoList.Add(operatingSystemInfo);
            }
        }

        /// <summary>
        /// The report WMI info.
        /// </summary>
        public void ReportWMIInfo()
        {
            
        }
    }
}
