namespace PopulateWMIInfo.Rules
{
    using System.Collections.Generic;
    using System.Management;

    using Constants;

    using Entities;

    using PopulateWMIInfo.Contracts;

    /// <summary>
    /// The partition WMI information.
    /// </summary>
    public class Partition : IWmiInfo
    {
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
                this.partitionInfoList.Add(partitionInfo);
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
