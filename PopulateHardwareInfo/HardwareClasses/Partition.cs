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

    /// <summary>
    /// The partition WMI information.
    /// </summary>
    public class Partition : IWmiInfo
    {
        private const string PartitionTableName = "partition";

        private const string ClientPartitionTableName = "x_client_partition";

        private ManagementObjectSearcher searcher;

        private List<PartitionInfo> partitionInfoList;

        /// <summary>
        /// Initializes a new instance of the <see cref="Partition"/> class.
        /// </summary>
        public Partition()
        {
            this.partitionInfoList = new List<PartitionInfo>();
            this.searcher = new ManagementObjectSearcher(
                WmiConstants.WmiRootNamespace,
                string.Format(
                    "SELECT {0}, {1}, {2}, {3}, {4}, {5}, {6} FROM Win32_DiskPartition",
                    WmiConstants.Name,
                    WmiConstants.Description,
                    WmiConstants.DiskIndex,
                    WmiConstants.Bootable,
                    WmiConstants.BootPartition,
                    WmiConstants.Size,
                    WmiConstants.StartingOffset));
        }

        /// <summary>
        /// The get WMI info.
        /// </summary>
        public void GetWMIInfo()
        {
            this.partitionInfoList = this.GetValue();
        }

        /// <summary>
        /// The report WMI info.
        /// </summary>
        /// <param name="visitor">
        /// The visitor.
        /// </param>
        public void ReportWMIInfo(IVisitor visitor)
        {
            var data = this.ConvertPartitionInfoToJson(this.partitionInfoList);
            visitor.Visit(PartitionTableName, data);
            data = this.ConvertClientPartitionInfoToJson(this.partitionInfoList);
            visitor.Visit(ClientPartitionTableName, data);
        }

        /// <summary>
        /// The check for hardware changes.
        /// </summary>
        /// <param name="visitor">
        /// The visitor.
        /// </param>
        public void CheckForHardwareChanges(IVisitor visitor)
        {
            var changedHardwareList = new List<PartitionInfo>();
            var tempList = this.GetValue();
            changedHardwareList = tempList.GetDifference(this.partitionInfoList);
            if (changedHardwareList.Any())
            {
                var data = this.ConvertClientPartitionInfoToJson(changedHardwareList);
                visitor.Visit(PartitionTableName, data);
                data = this.ConvertClientPartitionInfoToJson(changedHardwareList);
                visitor.Visit(ClientPartitionTableName, data);
            }
        }

        /// <summary>
        /// The get value.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        private List<PartitionInfo> GetValue()
        {
            var tempList = new List<PartitionInfo>();
            try
            {
                foreach (var queryObject in this.searcher.Get())
                {
                    var partitionInfo = new PartitionInfo
                    {
                        Name = queryObject[WmiConstants.Name],
                        Description = queryObject[WmiConstants.Description],
                        DiskIndex = queryObject[WmiConstants.DiskIndex],
                        Bootable = queryObject[WmiConstants.Bootable],
                        BootPartition = queryObject[WmiConstants.BootPartition],
                        Size = queryObject[WmiConstants.Size],
                        StartingOffset = queryObject[WmiConstants.StartingOffset]
                    };
                    tempList.Add(partitionInfo);
                }
            }
            catch (Exception)
            {
                throw;
            }
            
            return tempList;
        }

        /// <summary>
        /// The convert partition info to json.
        /// </summary>
        /// <param name="partitionInfoList">
        /// The partition info list.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private string ConvertPartitionInfoToJson(List<PartitionInfo> partitionInfoList)
        {
            var data = new JObject(
                new JProperty(
                    "resources",
                    new JArray(
                        from partitionInfo in partitionInfoList
                        select
                            new JObject(
                            new JProperty("name", partitionInfo.Name),
                            new JProperty("description", partitionInfo.Description),
                            new JProperty("disk_index", partitionInfo.DiskIndex),
                            new JProperty(
                            "is_bootable",
                            GenericExtensions.GetBooleanValue((bool)partitionInfo.Bootable),
                            new JProperty(
                            "is_boot_partition",
                            GenericExtensions.GetBooleanValue((bool)partitionInfo.BootPartition)))))));
            return data.ToString();
        }

        /// <summary>
        /// The convert client partition info to json.
        /// </summary>
        /// <param name="partitionInfoList">
        /// The partition info list.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private string ConvertClientPartitionInfoToJson(List<PartitionInfo> partitionInfoList)
        {
            var data = new JObject(
                new JProperty(
                    "resources",
                    new JArray(
                        from partitionInfo in partitionInfoList
                        select
                            new JObject(
                            new JProperty("size", partitionInfo.Size),
                            new JProperty("starting_offset", partitionInfo.StartingOffset)))));
            return data.ToString();
        }
    }
}
