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

    public class DiskDrive : IWmiInfo
    {
        private const string DiskDriveTableName = "disk_drive";

        private const string DiskDriveClientTableName = "x_client_disk_drive";

        private ManagementObjectSearcher searcher;

        private List<DiskdriveInfo> diskDriveInfoList;

        private int id;

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

        /// <summary>
        /// The get wmi info.
        /// </summary>
        public void GetWMIInfo()
        {
            this.diskDriveInfoList = this.GetValue();
        }

        /// <summary>
        /// The report wmi info.
        /// </summary>
        /// <param name="visitor">
        /// The visitor.
        /// </param>
        public void ReportWMIInfo(IVisitor visitor)
        {
            var data = this.ConvertDiskDriveInfoToJson(this.diskDriveInfoList);
            visitor.Visit(DiskDriveTableName, data, out this.id);
            data = this.ConvertClientDiskDriveInfoToJson(this.diskDriveInfoList);
            int temp;
            visitor.Visit(DiskDriveClientTableName, data, out temp);
        }

        /// <summary>
        /// The check for hardware changes.
        /// </summary>
        public void CheckForHardwareChanges(IVisitor visitor)
        {
            var tempList = new List<DiskdriveInfo>();
            var changedHardwareList = new List<DiskdriveInfo>();
            tempList = this.GetValue();
            changedHardwareList = tempList.GetDifference(this.diskDriveInfoList);
            if (changedHardwareList.Any())
            {
                var data = this.ConvertDiskDriveInfoToJson(changedHardwareList);
                visitor.Visit(DiskDriveTableName, data, out id);
                data = this.ConvertClientDiskDriveInfoToJson(changedHardwareList);
                int temp;
                visitor.Visit(DiskDriveClientTableName, data, out temp);
            }
        }

        /// <summary>
        /// The get value.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        private List<DiskdriveInfo> GetValue()
        {
            var tempList = new List<DiskdriveInfo>();

            // TODO: Disk drive and Manufacturer are not available
            try
            {
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

                    tempList.Add(diskDriveInfo);
                }
            }
            catch (Exception)
            {
                throw;
            }

            return tempList;
        }

        /// <summary>
        /// The convert disk drive info to json.
        /// </summary>
        /// <param name="diskDriveInfoList">
        /// The disk drive info list.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private string ConvertDiskDriveInfoToJson(List<DiskdriveInfo> diskDriveInfoList)
        {
            var data = new JObject(
                new JProperty(
                    "resource",
                    new JArray(
                        from diskDriveInfo in diskDriveInfoList
                        select
                            new JObject(
                            new JProperty("name", diskDriveInfo.Name),
                            new JProperty("model", diskDriveInfo.Model),
                            new JProperty("manufacturer", diskDriveInfo.Manufacturer),
                            new JProperty("interface_type", diskDriveInfo.InterfaceType),
                            new JProperty("size", diskDriveInfo.Size),
                            new JProperty("media_type", diskDriveInfo.MediaType)))));
            return data.ToString();
        }

        /// <summary>
        /// The convert client disk drive info to json.
        /// </summary>
        /// <param name="diskDriveInfoList">
        /// The disk drive info list.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private string ConvertClientDiskDriveInfoToJson(List<DiskdriveInfo> diskDriveInfoList)
        {
            var data = new JObject(
                new JProperty(
                    "resource",
                    new JArray(
                        from diskDriveInfo in diskDriveInfoList
                        select
                            new JObject(
                            new JProperty("firmware_revision", diskDriveInfo.FirmwareRevisions),
                            new JProperty("partitions", diskDriveInfo.Partitions),
                            new JProperty("serial_number", diskDriveInfo.SerialNumber),
                            new JProperty("client_id", ClientId.Id),
                            new JProperty("disk_drive_id", this.id)))));
            return data.ToString();
        }
    }
}
