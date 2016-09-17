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
                WmiConstants.WmiNamespace,
                string.Format(
                    "SELECT {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}, {13}, {14}, {15}, {16}, {17}, {18}",
                    WmiConstants.Name,
                    WmiConstants.Manufacturer,
                    WmiConstants.Model,
                    WmiConstants.Description,
                    WmiConstants.ThreadCount,
                    WmiConstants.NumberOfCores,
                    WmiConstants.NumberOfLogicalProcessors,
                    WmiConstants.ProcessorId,
                    WmiConstants.SocketDesignation,
                    WmiConstants.MaxClockSpeed,
                    WmiConstants.Voltage,
                    WmiConstants.AddressWidth,
                    WmiConstants.Device,
                    WmiConstants.L2CacheSize,
                    WmiConstants.L3CacheSize,
                    WmiConstants.NumberOfEnabledCore,
                    WmiConstants.CurrentClockSpeed,
                    WmiConstants.SerialNumber,
                    WmiConstants.VirtualizationFirmwareEnabled));
        }

        /// <summary>
        /// The get WMI info.
        /// </summary>
        public void GetWMIInfo()
        {
            foreach (var queryObject in this.searcher.Get())
            {
                var cpuInfo = new CPUInfo
                                  {
                                      Name = queryObject[WmiConstants.Name],
                                      Manufacturer = queryObject[WmiConstants.Manufacturer],
                                      Model = queryObject[WmiConstants.Model],
                                      Description = queryObject[WmiConstants.Description],
                                      ThreadCount = queryObject[WmiConstants.ThreadCount],
                                      NumberOfCores = queryObject[WmiConstants.NumberOfCores],
                                      NumberOfLogicalProcessors =
                                          queryObject[WmiConstants.NumberOfLogicalProcessors],
                                      ProcessorId = queryObject[WmiConstants.ProcessorId],
                                      SocketDesignation = queryObject[WmiConstants.SocketDesignation],
                                      MaxClockSpeed = queryObject[WmiConstants.MaxClockSpeed],
                                      Voltage = queryObject[WmiConstants.Voltage],
                                      AddressWidth = queryObject[WmiConstants.AddressWidth],
                                      Device = queryObject[WmiConstants.Device],
                                      L2CacheSize = queryObject[WmiConstants.L2CacheSize],
                                      L3CacheSize = queryObject[WmiConstants.L3CacheSize],
                                      NumberOfEnabledCore = queryObject[WmiConstants.NumberOfEnabledCore],
                                      CurrentClockSpeed = queryObject[WmiConstants.CurrentClockSpeed],
                                      SerialNumber = queryObject[WmiConstants.SerialNumber],
                                      VirtualizationFirmwareEnabled =
                                          queryObject[WmiConstants.VirtualizationFirmwareEnabled]
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
