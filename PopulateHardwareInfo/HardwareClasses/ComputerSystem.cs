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
    /// The computer system WMI info.
    /// </summary>
    public class ComputerSystem : IWmiInfo
    {
        private const string ComputerSystemTableName = "client";

        private readonly ManagementObjectSearcher computerSystemSearcher;

        private ManagementObjectSearcher computerSystemProductSearcher;

        private readonly ManagementObjectSearcher systemEnclosureSearcher;

        private readonly ManagementObjectSearcher batterySearcher;

        private readonly ManagementObjectSearcher operatingSystemSearcher;

        private List<ComputerSystemInfo> computerSystemInfoList;

        private List<ComputerSystemProductInfo> computerSystemProductInfoList;

        private Dictionary<string, int> ComputerSystemStatus = new Dictionary<string, int>
                                                                   {
                                                                       { "OK", 1 },
                                                                       { "Error", 2 },
                                                                       { "Degraded", 3 },
                                                                       { "Unknown", 4 },
                                                                       { "Pred Fail", 5 },
                                                                       { "Starting", 6 },
                                                                       { "Stopping", 7 },
                                                                       { "Service", 8 },
                                                                       { "Stressed", 9 },
                                                                       { "NonRecover", 10 },
                                                                       { "No Contact", 11 },
                                                                       { "Lost Comm", 12 }
                                                                   };

        /// <summary>
        /// Initializes a new instance of the <see cref="ComputerSystem"/> class.
        /// </summary>
        public ComputerSystem()
        {
            this.computerSystemInfoList = new List<ComputerSystemInfo>();
            this.computerSystemSearcher = new ManagementObjectSearcher(
                WmiConstants.WmiRootNamespace,
                string.Format(
                    "SELECT {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10} FROM Win32_ComputerSystem",
                    WmiConstants.Name,
                    WmiConstants.Status,
                    WmiConstants.PrimaryOwnerName,
                    WmiConstants.SystemType,
                    WmiConstants.ThermalState,
                    WmiConstants.PartOfDomain,
                    WmiConstants.Domain,
                    WmiConstants.Workgroup,
                    WmiConstants.CurrentTimeZone,
                    WmiConstants.Manufacturer,
                    WmiConstants.Model));
            this.systemEnclosureSearcher = new ManagementObjectSearcher(
                WmiConstants.WmiRootNamespace,
                string.Format("SELECT {0} FROM Win32_SystemEnclosure", WmiConstants.ChassisTypes));
            this.batterySearcher = new ManagementObjectSearcher(
                WmiConstants.WmiRootNamespace,
                string.Format("SELECT {0} FROM Win32_Battery", WmiConstants.StatusInfo));
            this.operatingSystemSearcher = new ManagementObjectSearcher(
                WmiConstants.WmiRootNamespace,
                string.Format("SELECT {0} FROM Win32_OperatingSystem", WmiConstants.Caption));
            this.computerSystemProductSearcher = new ManagementObjectSearcher(
                WmiConstants.WmiRootNamespace,
                string.Format(
                    "SELECT {0}, {1} FROM Win32_ComputerSystemProduct",
                    WmiConstants.IdentifyingNumber,
                    WmiConstants.UUID));
            this.computerSystemProductInfoList = new List<ComputerSystemProductInfo>();
        }

        /// <summary>
        /// The get WMI info.
        /// </summary>
        public void GetWMIInfo()
        {
            this.computerSystemInfoList = this.GetComputerSystemValue();
            this.computerSystemProductInfoList = this.GetComputerSystemProductValue();
        }

        /// <summary>
        /// The report WMI info.
        /// </summary>
        /// <param name="visitor">
        /// The visitor.
        /// </param>
        public void ReportWMIInfo(IVisitor visitor)
        {
            var data = this.ConvertComputerSystemToJson(this.computerSystemInfoList, this.computerSystemProductInfoList);
            int id;
            visitor.Visit(ComputerSystemTableName, data, out id);
            ClientId.Id = id;
        }

        /// <summary>
        /// The convert computer system to json.
        /// </summary>
        /// <param name="computerSystemInfoList">
        /// The computer system info list.
        /// </param>
        /// <param name="computerSystemProductInfoList">
        /// The computer system product info list.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private string ConvertComputerSystemToJson(List<ComputerSystemInfo> computerSystemInfoList, List<ComputerSystemProductInfo> computerSystemProductInfoList)
        {
            var data =
                new JObject(
                    new JProperty(
                        "resource",
                        new JArray(
                            from computerSystemInfo in computerSystemInfoList
                            select
                                new JObject(
                                new JProperty("organization", string.Empty),
                                new JProperty("status_id", computerSystemInfo.Status),
                                new JProperty(
                                "is_virtual",
                                GenericExtensions.GetBooleanValue(computerSystemInfo.IsVirtual)),
                                new JProperty("hypervisor_name", computerSystemInfo.Hypervisor),
                                new JProperty(
                                "is_server",
                                GenericExtensions.GetBooleanValue(computerSystemInfo.IsServer)),
                                new JProperty(
                                "is_portable",
                                GenericExtensions.GetBooleanValue(computerSystemInfo.IsPortable)),
                                new JProperty("computer_id", computerSystemInfo.ComputerId),
                                new JProperty("owner_name", computerSystemInfo.PrimaryOwnerName),
                                new JProperty("sku", computerSystemInfo.SystemSKUNumber),
                                new JProperty("system_type_id", computerSystemInfo.SystemType),
                                new JProperty("thermal_state", computerSystemInfo.ThermalState),
                                new JProperty(
                                "is_part_of_domain",
                                GenericExtensions.GetBooleanValue(computerSystemInfo.PartOfDomain)),
                                // new JProperty("domain_id", computerSystemInfo.Domain), // Domain ID is string and cannot be converted to int
                                // new JProperty("workgroup_id", computerSystemInfo.Workgroup), // Workgroup ID is string and cannot be parsed to int
                                new JProperty("time_zone_id", computerSystemInfo.CurrentTimeZone),
                                new JProperty("manufacturer", computerSystemInfo.Manufacturer),
                                new JProperty("computer_model", computerSystemInfo.Model),
                                from computerSystemProduct in computerSystemProductInfoList
                                select
                                new JProperty("identifying_number", computerSystemProduct.IdentifyingNumber),
                                from computerSystemProduct in computerSystemProductInfoList
                                select new JProperty("uuid", computerSystemProduct.UUID)))));
            return data.ToString();
        }

        /// <summary>
        /// The check for hardware changes.
        /// </summary>
        /// <param name="visitor">
        /// The visitor.
        /// </param>
        public void CheckForHardwareChanges(IVisitor visitor)
        {
            var changedComputerSystemList = new List<ComputerSystemInfo>();
            var tempComputerSystemInfoList = this.GetComputerSystemValue();
            changedComputerSystemList = tempComputerSystemInfoList.GetDifference(this.computerSystemInfoList);
            var changedComputerSystemProductList = new List<ComputerSystemProductInfo>();
            var tempComputerSystemProductList = this.GetComputerSystemProductValue();
            changedComputerSystemProductList =
                tempComputerSystemProductList.GetDifference(this.computerSystemProductInfoList);
            int temp;
            if (changedComputerSystemList.Any() && changedComputerSystemProductList.Any())
            {
                var data = this.ConvertComputerSystemToJson(changedComputerSystemList, changedComputerSystemProductList);
                visitor.Visit(ComputerSystemTableName, data, out temp);
            }
            else if (changedComputerSystemList.Any() && !changedComputerSystemProductList.Any())
            {
                var data = this.ConvertComputerSystemToJson(
                    changedComputerSystemList,
                    new List<ComputerSystemProductInfo>());
                visitor.Visit(ComputerSystemTableName, data, out temp);
            }
            else if (!changedComputerSystemList.Any() && changedComputerSystemProductList.Any())
            {
                var data = this.ConvertComputerSystemToJson(
                    new List<ComputerSystemInfo>(),
                    changedComputerSystemProductList);
                visitor.Visit(ComputerSystemTableName, data, out temp);
            }
        }

        /// <summary>
        /// Gets hypervisor info.
        /// </summary>
        /// <param name="manufacturer">
        /// The manufacturer.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private string GetHypervisorInfo(string manufacturer)
        {
            var hyperVisorName = "Hypervisor name: {0}";
            if (manufacturer.ToLower().Contains("vmware"))
            {
                return string.Format(hyperVisorName, HypervisorCompanies.VMWareEsxi);
            }
            else if (manufacturer.ToLower().Contains("hyper-v"))
            {
                return string.Format(hyperVisorName, HypervisorCompanies.MicrosoftHyperV);
            }
            else if (manufacturer.ToLower().Contains("xen"))
            {
                return string.Format(hyperVisorName, HypervisorCompanies.CitrixXenserver);
            }
            else if (manufacturer.ToLower().Contains("innotek"))
            {
                return string.Format(hyperVisorName, HypervisorCompanies.VirtualBox);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Check if system is virtual machine.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <param name="manufacturer">
        /// The manufacturer.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private bool CheckIfSystemIsVirtualMachine(string model, string manufacturer)
        {
            return (model.ToLower().Contains("virtual") || model.ToLower().Contains("vm"))
                   && (manufacturer.ToLower().Contains("vmware") || manufacturer.ToLower().Contains("hyper-v"));
        }

        /// <summary>
        /// Check if system is server.
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private bool CheckIfSystemIsServer()
        {
            var caption = string.Empty;
            foreach (var queryObject in this.operatingSystemSearcher.Get())
            {
                caption = queryObject[WmiConstants.Caption].ToString();
            }

            return caption.ToLower().Contains("server");
        }

        /// <summary>
        /// Check if system is laptop.
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private bool CheckIfSystemIsLaptop()
        {
            uint chassisType = 0, statusInfo = 0;
            foreach (var queryObject in this.systemEnclosureSearcher.Get())
            {
                var chassisTypes = (ushort[])queryObject.GetPropertyValue(WmiConstants.ChassisTypes);
                chassisType = chassisTypes[0];

            }

            foreach (var queryObject in this.batterySearcher.Get())
            {
                statusInfo = (uint)queryObject[WmiConstants.StatusInfo];
            }

            return (chassisType == 9 || chassisType == 10 || chassisType == 14) && statusInfo != 5;
        }

        /// <summary>
        /// The get value.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        private List<ComputerSystemInfo> GetComputerSystemValue()
        {
            var tempList = new List<ComputerSystemInfo>();

            // TODO: SystemSKUNumber is not supported before Windows 10
            try
            {
                foreach (var queryObject in this.computerSystemSearcher.Get())
                {
                    var computerSystemInfo = new ComputerSystemInfo
                    {
                        Name = queryObject[WmiConstants.Name],
                        Status = this.ComputerSystemStatus[queryObject[WmiConstants.Status].ToString()],
                        PrimaryOwnerName = queryObject[WmiConstants.PrimaryOwnerName],
                        SystemType = queryObject[WmiConstants.SystemType],
                        ThermalState = queryObject[WmiConstants.ThermalState],
                        PartOfDomain = queryObject[WmiConstants.PartOfDomain],
                        Domain = queryObject[WmiConstants.Domain],
                        Workgroup = queryObject[WmiConstants.Workgroup],
                        CurrentTimeZone = queryObject[WmiConstants.CurrentTimeZone],
                        Manufacturer = queryObject[WmiConstants.Manufacturer],
                        Model = queryObject[WmiConstants.Model]
                    };
                    computerSystemInfo.IsPortable = this.CheckIfSystemIsLaptop();
                    computerSystemInfo.IsServer = this.CheckIfSystemIsServer();
                    computerSystemInfo.IsVirtual = this.CheckIfSystemIsVirtualMachine(
                        computerSystemInfo.Model.ToString(),
                        computerSystemInfo.Manufacturer.ToString());
                    computerSystemInfo.Hypervisor = this.GetHypervisorInfo(computerSystemInfo.Manufacturer.ToString());
                    tempList.Add(computerSystemInfo);
                }

                return tempList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private List<ComputerSystemProductInfo> GetComputerSystemProductValue()
        {
            var tempList = new List<ComputerSystemProductInfo>();
            try
            {
                foreach (var queryObject in this.computerSystemProductSearcher.Get())
                {
                    var computerSystemProductInfo = new ComputerSystemProductInfo
                    {
                        IdentifyingNumber = queryObject[WmiConstants.IdentifyingNumber],
                        UUID = queryObject[WmiConstants.UUID]
                    };
                    tempList.Add(computerSystemProductInfo);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return tempList;
        }
    }
}