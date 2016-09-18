namespace PopulateWMIInfo.Rules
{
    using System.Collections.Generic;
    using System.Management;

    using Constants;

    using Entities;

    using PopulateWMIInfo.Contracts;

    /// <summary>
    /// The WMI information for printer.
    /// </summary>
    public class Printer : IWmiInfo
    {
        private ManagementObjectSearcher searcher;

        private List<PrinterInfo> printerInfoList;

        /// <summary>
        /// Initializes a new instance of the <see cref="Printer"/> class.
        /// </summary>
        public Printer()
        {
            this.printerInfoList = new List<PrinterInfo>();
            this.searcher = new ManagementObjectSearcher(
                WmiConstants.WmiRootNamespace,
                string.Format(
                    "SELECT {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11} FROM Win32_Printer",
                    WmiConstants.Name,
                    WmiConstants.DriverName,
                    WmiConstants.Location,
                    WmiConstants.ServerName,
                    WmiConstants.VerticalResolution,
                    WmiConstants.HorizontalResolution,
                    WmiConstants.Hidden,
                    WmiConstants.PortName,
                    WmiConstants.Status,
                    WmiConstants.Shared,
                    WmiConstants.Default,
                    WmiConstants.WorkOffline));
        }

        /// <summary>
        /// The get WMI info.
        /// </summary>
        public void GetWMIInfo()
        {
            // TODO: Manufacturer info is not available
            foreach (var queryObject in this.searcher.Get())
            {
                var printerInfo = new PrinterInfo
                                      {
                                          Name = queryObject[WmiConstants.Name],
                                          DriverName = queryObject[WmiConstants.DriverName],
                                          Location = queryObject[WmiConstants.Location],
                                          ServerName = queryObject[WmiConstants.ServerName],
                                          VerticalResolution = queryObject[WmiConstants.VerticalResolution],
                                          HorizontalResolution = queryObject[WmiConstants.HorizontalResolution],
                                          Hidden = queryObject[WmiConstants.Hidden],
                                          PortName = queryObject[WmiConstants.PortName],
                                          Status = queryObject[WmiConstants.Status],
                                          Shared = queryObject[WmiConstants.Shared],
                                          Default = queryObject[WmiConstants.Default],
                                          WorkOffline = queryObject[WmiConstants.WorkOffline]
                                      };
                this.printerInfoList.Add(printerInfo);
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
