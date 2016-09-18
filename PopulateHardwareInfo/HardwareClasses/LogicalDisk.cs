namespace PopulateWMIInfo.Rules
{
    using System.Collections.Generic;
    using System.Management;

    using Constants;

    using Entities;

    using PopulateWMIInfo.Contracts;

    public class LogicalDisk : IWmiInfo
    {
        private ManagementObjectSearcher searcher;

        private readonly List<LogicalDrivesInfo> logicalDrivesInfoList;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicalDisk"/> class.
        /// </summary>
        public LogicalDisk()
        {
            this.logicalDrivesInfoList = new List<LogicalDrivesInfo>();
            this.searcher = new ManagementObjectSearcher(
                WmiConstants.WmiRootNamespace,
                string.Format(
                    "SELECT {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10} FROM Win32_LogicalDisk",
                    WmiConstants.Name,
                    WmiConstants.Description,
                    WmiConstants.FileSystem,
                    WmiConstants.Size,
                    WmiConstants.ProviderName,
                    WmiConstants.SupportsFileBasedCompression,
                    WmiConstants.SupportsDiskQuotas,
                    WmiConstants.FreeSpace,
                    WmiConstants.Compressed,
                    WmiConstants.VolumeSerialNumber,
                    WmiConstants.VolumeName));
        }

        /// <summary>
        /// The get WMI info.
        /// </summary>
        public void GetWMIInfo()
        {
            foreach (var queryObject in this.searcher.Get())
            {
                var logicalDrivesInfo = new LogicalDrivesInfo
                                            {
                                                Name = queryObject[WmiConstants.Name],
                                                Description = queryObject[WmiConstants.Description],
                                                FileSystem = queryObject[WmiConstants.FileSystem],
                                                Size = queryObject[WmiConstants.Size],
                                                ProviderName = queryObject[WmiConstants.ProviderName],
                                                SupportsFileCompression =
                                                    queryObject[WmiConstants.SupportsFileBasedCompression],
                                                SupportsDiskQuotas =
                                                    queryObject[WmiConstants.SupportsDiskQuotas],
                                                FreeSpace = queryObject[WmiConstants.FreeSpace],
                                                Compressed = queryObject[WmiConstants.Compressed],
                                                VolumeSerialNumber =
                                                    queryObject[WmiConstants.VolumeSerialNumber],
                                                VolumeName = queryObject[WmiConstants.VolumeName]
                                            };
                this.logicalDrivesInfoList.Add(logicalDrivesInfo);
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
