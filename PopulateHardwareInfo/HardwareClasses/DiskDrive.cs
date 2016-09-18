namespace PopulateWMIInfo.Rules
{
    using System.Collections.Generic;
    using System.Management;

    using Constants;

    using Entities;

    using PopulateWMIInfo.Contracts;

    public class DiskDrive : IWmiInfo
    {
        private ManagementObjectSearcher searcher;

        private List<DiskdriveInfo> diskDriveInfoList;

        /// <summary>
        /// Initializes a new instance of the <see cref="DiskDrive"/> class.
        /// </summary>
        public DiskDrive()
        {
            this.diskDriveInfoList = new List<DiskdriveInfo>();
            this.searcher = new ManagementObjectSearcher(
                WmiConstants.WmiRootNamespace,
                string.Format(
                    "SELECT {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7} FROM Win32_DiskDrive",
                    WmiConstants.Name,
                    WmiConstants.Model,
                    WmiConstants.InterfaceType,
                    WmiConstants.Size,
                    WmiConstants.MediaType,
                    WmiConstants.FirmwareRevision,
                    WmiConstants.Partitions,
                    WmiConstants.SerialNumber));
        }

        public void GetWMIInfo()
        {
            // TODO: Disk drive and Manufacturer are not available
            foreach (var queryObject in this.searcher.Get())
            {
                var diskDriveInfo = new DiskdriveInfo
                                        {
                                            Name = queryObject[WmiConstants.Name],
                                            Model = queryObject[WmiConstants.Model],
                                            InterfaceType = queryObject[WmiConstants.InterfaceType],
                                            Size = queryObject[WmiConstants.Size],
                                            MediaType = queryObject[WmiConstants.MediaType],
                                            FirmwareRevisions = queryObject[WmiConstants.FirmwareRevision],
                                            Partitions = queryObject[WmiConstants.Partitions],
                                            SerialNumber = queryObject[WmiConstants.SerialNumber]
                                        };
                this.diskDriveInfoList.Add(diskDriveInfo);
            }
        }

        public void ReportWMIInfo()
        {
            
        }
    }
}
