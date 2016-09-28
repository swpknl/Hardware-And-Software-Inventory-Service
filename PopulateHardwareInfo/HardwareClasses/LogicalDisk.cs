namespace PopulateWMIInfo.Rules
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

    public class LogicalDisk : IWmiInfo
    {
        private const string LogicalDrivesInfoTableName = "logical_disk";

        private const string LogicalDrivesInfoClientTableName = "x_client_table_name";

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
            this.logicalDrivesInfoList = this.GetLogicalDrivesInfo();
        }

        /// <summary>
        /// The report WMI info.
        /// </summary>
        /// <param name="visitor">
        /// The visitor.
        /// </param>
        public void ReportWMIInfo(IVisitor visitor)
        {
            var data = this.ConvertLogicalDrivesInfoToJson(this.logicalDrivesInfoList);
            visitor.Visit(LogicalDrivesInfoTableName, data);
            data = this.ConvertClientLogicalDrivesInfoToJson(this.logicalDrivesInfoList);
            visitor.Visit(LogicalDrivesInfoClientTableName, data);
        }

        /// <summary>
        /// The check for hardware changes.
        /// </summary>
        /// <param name="visitor">
        /// The visitor.
        /// </param>
        public void CheckForHardwareChanges(IVisitor visitor)
        {
            var changedHardwareList = new List<LogicalDrivesInfo>();
            var tempList = this.GetLogicalDrivesInfo();
            changedHardwareList = tempList.GetDifference(this.logicalDrivesInfoList);
            if (changedHardwareList.Any())
            {
                var data = this.ConvertLogicalDrivesInfoToJson(changedHardwareList);
                visitor.Visit(LogicalDrivesInfoTableName, data);
                data = this.ConvertClientLogicalDrivesInfoToJson(changedHardwareList);
                visitor.Visit(LogicalDrivesInfoClientTableName, data);
            }
        }

        /// <summary>
        /// The get logical drives info.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        private List<LogicalDrivesInfo> GetLogicalDrivesInfo()
        {
            var tempList = new List<LogicalDrivesInfo>();
            try
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
                    tempList.Add(logicalDrivesInfo);
                }
            }
            catch (Exception)
            {
                throw;
            }

            return tempList;
        }

        /// <summary>
        /// The convert logical drives info to json.
        /// </summary>
        /// <param name="logicalDrivesInfoList">
        /// The logical drives info list.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private string ConvertLogicalDrivesInfoToJson(List<LogicalDrivesInfo> logicalDrivesInfoList)
        {
            var data = new JObject(
                new JProperty(
                    "resources",
                    new JArray(
                        from logicalDrivesInfo in logicalDrivesInfoList
                        select
                            new JObject(
                            new JProperty("name", logicalDrivesInfo.Name),
                            new JProperty("file_system", logicalDrivesInfo.FileSystem),
                            new JProperty("total_size", logicalDrivesInfo.Size),
                            new JProperty("provider_name", logicalDrivesInfo.ProviderName),
                            new JProperty("supports_file_compression", GenericExtensions.GetBooleanValue((bool)logicalDrivesInfo.SupportsFileCompression)),
                            new JProperty("support_disk_quotas", GenericExtensions.GetBooleanValue((bool)logicalDrivesInfo.SupportsDiskQuotas))))));
            return data.ToString();
        }

        /// <summary>
        /// The convert client logical drives info to json.
        /// </summary>
        /// <param name="logicalDrivesInfoList">
        /// The logical drives info list.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private string ConvertClientLogicalDrivesInfoToJson(List<LogicalDrivesInfo> logicalDrivesInfoList)
        {
            var data = new JObject(
                new JProperty(
                    "resources",
                    new JArray(
                        from logicalDrivesInfo in logicalDrivesInfoList
                        select
                            new JObject(
                            new JProperty("free_space", logicalDrivesInfo.FreeSpace),
                            new JProperty(
                            "is_compressed",
                            GenericExtensions.GetBooleanValue((bool)logicalDrivesInfo.Compressed)),
                            new JProperty("volume_serial_number", logicalDrivesInfo.VolumeSerialNumber),
                            new JProperty("volume_name", logicalDrivesInfo.VolumeName)))));
            return data.ToString();
        }
    }
}