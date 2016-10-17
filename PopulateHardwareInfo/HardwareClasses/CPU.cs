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

    /// <summary>
    /// The CPU WMI info.
    /// </summary>
    public class CPU : IWmiInfo
    {
        private const string CpuTableName = "cpu";

        private const string ClientCpuTableName = "x_client_cpu";

        private ManagementObjectSearcher searcher;

        private List<CPUInfo> cpuInfoList;

        private int id;

        /// <summary>
        /// Initializes a new instance of the <see cref="CPU"/> class.
        /// </summary>
        public CPU()
        {
            this.cpuInfoList = new List<CPUInfo>();
            this.searcher = new ManagementObjectSearcher(
                WmiConstants.WmiRootNamespace,
                string.Format(
                    "SELECT {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}, {13} FROM Win32_Processor",
                    WmiConstants.Name,
                    WmiConstants.Manufacturer,
                    WmiConstants.Description,
                    WmiConstants.NumberOfCores,
                    WmiConstants.NumberOfLogicalProcessors,
                    WmiConstants.ProcessorId,
                    WmiConstants.SocketDesignation,
                    WmiConstants.MaxClockSpeed,
                    WmiConstants.CurrentVoltage,
                    WmiConstants.AddressWidth,
                    WmiConstants.DeviceID,
                    WmiConstants.L2CacheSize,
                    WmiConstants.L3CacheSize,
                    WmiConstants.CurrentClockSpeed));
        }

        /// <summary>
        /// The get WMI info.
        /// </summary>
        public void GetWMIInfo()
        {
            this.cpuInfoList = this.GetValue();
        }

        /// <summary>
        /// The report WMI info.
        /// </summary>
        public void ReportWMIInfo(IVisitor visitor)
        {
            string data = this.ConvertCpuInfoToJson(this.cpuInfoList);
            visitor.Visit(CpuTableName, data, out this.id);
            data = this.ConvertClientCpuInfoToJson(this.cpuInfoList);
            int temp;
            visitor.Visit(ClientCpuTableName, data, out temp);
        }

        /// <summary>
        /// The check for hardware changes.
        /// </summary>
        public void CheckForHardwareChanges(IVisitor visitor)
        {
            var changedHardwareList = new List<CPUInfo>();
            var tempList = this.GetValue();
            changedHardwareList = tempList.GetDifference(this.cpuInfoList);
            if (changedHardwareList.Any())
            {
                var data = this.ConvertCpuInfoToJson(changedHardwareList);
                visitor.Visit(CpuTableName, data, out this.id);
                data = this.ConvertClientCpuInfoToJson(changedHardwareList);
                int temp;
                visitor.Visit(ClientCpuTableName, data, out temp);
            }
        }

        private List<CPUInfo> GetValue()
        {
            var tempList = new List<CPUInfo>();

            // TODO: Model, Threadcount, NumberOfEnabledCores, SerialNumber and VirtualizationFirmwareEnabled(not present before Windows 8) 
            // info are not present before Windows 10
            try
            {
                foreach (var queryObject in this.searcher.Get())
                {
                    var cpuInfo = new CPUInfo
                    {
                        Name = queryObject[WmiConstants.Name],
                        Manufacturer = queryObject[WmiConstants.Manufacturer],
                        Description = queryObject[WmiConstants.Description],
                        NumberOfCores = queryObject[WmiConstants.NumberOfCores],
                        NumberOfLogicalProcessors = queryObject[WmiConstants.NumberOfLogicalProcessors],
                        ProcessorId = queryObject[WmiConstants.ProcessorId],
                        SocketDesignation = queryObject[WmiConstants.SocketDesignation],
                        MaxClockSpeed = queryObject[WmiConstants.MaxClockSpeed],
                        Voltage = queryObject[WmiConstants.CurrentVoltage],
                        AddressWidth = queryObject[WmiConstants.AddressWidth],
                        Device = queryObject[WmiConstants.DeviceID],
                        L2CacheSize = queryObject[WmiConstants.L2CacheSize],
                        L3CacheSize = queryObject[WmiConstants.L3CacheSize],
                        CurrentClockSpeed = queryObject[WmiConstants.CurrentClockSpeed]
                    };
                    tempList.Add(cpuInfo);
                }
            }
            catch (Exception)
            {
                throw;
            }
            
            return tempList;
        }

        /// <summary>
        /// The convert cpu info to json.
        /// </summary>
        /// <param name="cpuInfoList">
        /// The cpu info list.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private string ConvertCpuInfoToJson(List<CPUInfo> cpuInfoList)
        {
            var data = new JObject(
                new JProperty(
                    "resource",
                    new JArray(
                        from cpuInfo in cpuInfoList
                        select
                            new JObject(
                            new JProperty("name", cpuInfo.Name),
                            new JProperty("model", cpuInfo.Model),

                            // new JProperty("cpu_group_id", cpuInfo.Description), // Commenting as description cannot be parsed to int
                            new JProperty("thread_count", cpuInfo.ThreadCount),
                            new JProperty("number_of_cores", cpuInfo.NumberOfCores),
                            new JProperty("number_of_logical_processors", cpuInfo.NumberOfLogicalProcessors),
                            new JProperty("processor_identifier", cpuInfo.ProcessorId),
                            new JProperty("socket", cpuInfo.SocketDesignation),
                            new JProperty("max_clock_speed_mhz", cpuInfo.MaxClockSpeed),
                            new JProperty("voltage", cpuInfo.Voltage),
                            new JProperty("architecture", cpuInfo.AddressWidth),
                            new JProperty("device", cpuInfo.Device),
                            new JProperty("l2_cache_size", cpuInfo.L2CacheSize),
                            new JProperty("l3_cache_size", cpuInfo.L3CacheSize),
                            new JProperty("number_of_enabled_cores", cpuInfo.NumberOfEnabledCore)))));
            return data.ToString();
        }

        /// <summary>
        /// The convert client cpu info to json.
        /// </summary>
        /// <param name="cpuInfoList">
        /// The cpu info list.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private string ConvertClientCpuInfoToJson(List<CPUInfo> cpuInfoList)
        {
            var data = new JObject(
                new JProperty(
                    "resource",
                    new JArray(
                        from cpuInfo in cpuInfoList
                        select
                            new JObject(
                            new JProperty("clock_speed_mhz", cpuInfo.CurrentClockSpeed),
                            new JProperty("serial_number", cpuInfo.SerialNumber),
                            new JProperty(
                            "is_virtualization_firmware_enabled", GenericExtensions.GetBooleanValue(cpuInfo.VirtualizationFirmwareEnabled)),
                            new JProperty("client_id", ClientId.Id),
                            new JProperty("cpu_id", this.id)))));
            return data.ToString();
        }
    }
}
