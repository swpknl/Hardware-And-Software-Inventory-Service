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
                WmiConstants.WmiRootNamespace,
                    "SELECT * FROM Win32_BIOS");
        }

        /// <summary>
        /// The get WMI info.
        /// </summary>
        public void GetWMIInfo()
        {
            // TODO: SystemBIOSMajorVersion and SystemBiosMinorVersion are not present before Windows 10
            foreach (var queryObj in this.searcher.Get())
            {
                var biosInfo = new BIOSInfo
                {
                    Manufacturer = queryObj[WmiConstants.Manufacturer],
                    Version = queryObj[WmiConstants.Version],
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
