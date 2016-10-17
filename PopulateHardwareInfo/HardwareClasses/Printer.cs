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
    /// The WMI information for printer.
    /// </summary>
    public class Printer : IWmiInfo
    {
        private const string PrinterTableName = "printer";

        private const string ClientPrinterTableName = "client_printer";

        private ManagementObjectSearcher searcher;

        private List<PrinterInfo> printerInfoList;

        private int id;

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
            this.printerInfoList = this.GetValue();
        }

        /// <summary>
        /// The report WMI info.
        /// </summary>
        /// <param name="visitor">
        /// The visitor.
        /// </param>
        public void ReportWMIInfo(IVisitor visitor)
        {
            var data = this.ConvertPrinterInfoToJson(this.printerInfoList);
            visitor.Visit(PrinterTableName, data, out id);
            data = this.ConvertClientPrinterInfoToJson(this.printerInfoList);
            int temp;
            visitor.Visit(ClientPrinterTableName, data, out temp);
        }

        /// <summary>
        /// The check for hardware changes.
        /// </summary>
        /// <param name="visitor">
        /// The visitor.
        /// </param>
        public void CheckForHardwareChanges(IVisitor visitor)
        {
            var changedHardwareList = new List<PrinterInfo>();
            var tempList = this.GetValue();
            changedHardwareList = tempList.GetDifference(this.printerInfoList);
            if (changedHardwareList.Any())
            {
                var data = this.ConvertPrinterInfoToJson(changedHardwareList);
                visitor.Visit(PrinterTableName, data, out id);
                data = this.ConvertClientPrinterInfoToJson(changedHardwareList);
                int temp;
                visitor.Visit(ClientPrinterTableName, data, out temp);
            }
        }

        /// <summary>
        /// The get value.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        private List<PrinterInfo> GetValue()
        {
            var tempList = new List<PrinterInfo>();

            // TODO: Manufacturer info is not available
            try
            {
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
                    tempList.Add(printerInfo);
                }
            }
            catch (Exception)
            {
                throw;
            }
            
            return tempList;
        }

        /// <summary>
        /// The convert printer info to json.
        /// </summary>
        /// <param name="printerInfoList">
        /// The printer info list.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private string ConvertPrinterInfoToJson(List<PrinterInfo> printerInfoList)
        {
            var data = new JObject(
                new JProperty(
                    "resource",
                    new JArray(
                        from printerInfo in printerInfoList
                        select
                            new JObject(
                            new JProperty("name", printerInfo.Name),
                            new JProperty("manufacturer", printerInfo.Manufacturer),
                            new JProperty("driver_name", printerInfo.DriverName),
                            new JProperty("location", printerInfo.Location),
                            new JProperty("server_name", printerInfo.ServerName),
                            new JProperty("vertical_resolution", printerInfo.VerticalResolution),
                            new JProperty("horizontal_resolution", printerInfo.HorizontalResolution),
                            new JProperty("is_hidden", GenericExtensions.GetBooleanValue(printerInfo.Hidden))))));
            return data.ToString();
        }

        /// <summary>
        /// The convert client printer info to json.
        /// </summary>
        /// <param name="printerInfoList">
        /// The printer info list.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private string ConvertClientPrinterInfoToJson(List<PrinterInfo> printerInfoList)
        {
            var data = new JObject(
                new JProperty(
                    "resource",
                    new JArray(
                        from printerInfo in printerInfoList
                        select
                            new JObject(
                            new JProperty("port", printerInfo.PortName),
                            new JProperty("status_id", printerInfo.Status),
                            new JProperty("is_shared", GenericExtensions.GetBooleanValue((bool)printerInfo.Shared)),
                            new JProperty("is_default", GenericExtensions.GetBooleanValue((bool)printerInfo.Default)),
                            new JProperty("work_offline", GenericExtensions.GetBooleanValue((bool)printerInfo.WorkOffline)),
                            new JProperty("client_id", ClientId.Id),
                            new JProperty("printer_id", this.id)))));
            return data.ToString();
        }
    }
}