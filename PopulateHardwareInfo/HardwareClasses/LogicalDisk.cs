namespace PopulateWMIInfo.Rules
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Management;

    using Constants;

    using Entities;

    using Helpers;

    using Newtonsoft.Json.Linq;

    using PopulateWMIInfo.Contracts;

    using ReportToRestEndpoint.Contracts;

    public class LogicalDisk : IWmiInfo
    {
        private ManagementObjectSearcher searcher;

        private List<LogicalDrivesInfo> logicalDrivesInfoList;

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
            this.logicalDrivesInfoList = this.GetLogicalDrivesInfos();
        }

        private List<LogicalDrivesInfo> GetLogicalDrivesInfos()
        {
            var tempList = new List<LogicalDrivesInfo>();
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
                tempList.Add(logicalDrivesInfo);
            }

            return tempList;
        }

        /// <summary>
        /// The report WMI info.
        /// </summary>
        /// <param name="visitor">
        /// The visitor.
        /// </param>
        public void ReportWMIInfo(IVisitor visitor)
        {
            
        }

        /// <summary>
        /// The check for hardware changes.
        /// </summary>
        public void CheckForHardwareChanges(IVisitor visitor)
        {
            var changedHardwareList = new List<LogicalDrivesInfo>();
            var tempList = this.GetLogicalDrivesInfos();
            changedHardwareList = tempList.GetDifference(this.logicalDrivesInfoList);
        }

        private string ConvertLogicalDrivesInfoToJson(List<LogicalDrivesInfo> logicalDrivesInfoList)
        {
            var data = new JObject(
                new JProperty(
                    "resources",
                    new JArray(
                        from logicalDrivesInfo in logicalDrivesInfoList
                        select 
                        new JObject(
                            new JProperty()))))
        }
    }
}
