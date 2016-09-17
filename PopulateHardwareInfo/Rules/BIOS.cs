namespace PopulateWMIInfo.Rules
{
    using System.Collections.Generic;
    using System.Management;

    using Constants;

    using Entities;

    using PopulateWMIInfo.Contracts;

    /// <summary>
    /// The WMI info for BIOS.
    /// </summary>
    public class BIOS : IWmiInfo
    {
        private ManagementObjectSearcher searcher;

        private List<BIOSInfo> biosInfoList;

        /// <summary>
        /// Initializes a new instance of the <see cref="BIOS"/> class.
        /// </summary>
        public BIOS()
        {
            this.biosInfoList = new List<BIOSInfo>();
            this.searcher = new ManagementObjectSearcher(
                WmiConstants.WmiNamespace,
                string.Format(
                    "SELECT {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8} FROM Win32_BIOS",
                    WmiConstants.Manufacturer,
                    WmiConstants.Version,
                    WmiConstants.SystemBIOSMajorVersion,
                    WmiConstants.SystemBIOSMinorVersion,
                    WmiConstants.SMBIOSBIOSVersion,
                    WmiConstants.SMBIOSMajorVersion,
                    WmiConstants.SMBIOSMinorVersion,
                    WmiConstants.ReleaseDate,
                    WmiConstants.SerialNumber));
        }

        /// <summary>
        /// The get WMI info.
        /// </summary>
        public void GetWMIInfo()
        {
            foreach (var queryObj in this.searcher.Get())
            {
                var biosInfo = new BIOSInfo
                {
                    Manufacturer = queryObj[WmiConstants.Manufacturer],
                    Version = queryObj[WmiConstants.Version],
                    SystemBIOSMajorVersion = queryObj[WmiConstants.SystemBIOSMajorVersion],
                    SystemBIOSMinorVersion = queryObj[WmiConstants.SystemBIOSMinorVersion],
                    SMBIOSBIOSVersion = queryObj[WmiConstants.SMBIOSBIOSVersion],
                    SMBIOSMajorVersion = queryObj[WmiConstants.SMBIOSMajorVersion],
                    SMBIOSMinorVersion = queryObj[WmiConstants.SMBIOSMinorVersion],
                    ReleaseDate = queryObj[WmiConstants.ReleaseDate],
                    SerialNumber = queryObj[WmiConstants.SerialNumber]
                };

                this.biosInfoList.Add(biosInfo);
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
