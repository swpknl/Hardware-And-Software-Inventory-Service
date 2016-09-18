namespace PopulateWMIInfo.Rules
{
    using System.Collections.Generic;
    using System.Management;

    using Constants;

    using Entities;

    using PopulateWMIInfo.Contracts;

    /// <summary>
    /// The CPU WMI info.
    /// </summary>
    public class CPU : IWmiInfo
    {
        private ManagementObjectSearcher searcher;

        private List<CPUInfo> cpuInfoList;
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
            // TODO: Model, Threadcount, NumberOfEnabledCores, SerialNumber and VirtualizationFirmwareEnabled(not present before Windows 8) 
            // info are not present before Windows 10
            foreach (var queryObject in this.searcher.Get())
            {
                var cpuInfo = new CPUInfo
                                  {
                                      Name = queryObject[WmiConstants.Name],
                                      Manufacturer = queryObject[WmiConstants.Manufacturer],
                                      Description = queryObject[WmiConstants.Description],
                                      NumberOfCores = queryObject[WmiConstants.NumberOfCores],
                                      NumberOfLogicalProcessors =
                                          queryObject[WmiConstants.NumberOfLogicalProcessors],
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
                this.cpuInfoList.Add(cpuInfo);
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
