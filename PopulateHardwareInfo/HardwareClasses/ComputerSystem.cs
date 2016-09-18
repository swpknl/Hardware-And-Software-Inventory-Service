namespace PopulateWMIInfo.Rules
{
    using System.Collections.Generic;
    using System.Management;

    using Constants;

    using Entities;

    using PopulateWMIInfo.Contracts;

    /// <summary>
    /// The computer system WMI info.
    /// </summary>
    public class ComputerSystem : IWmiInfo
    {
        private readonly ManagementObjectSearcher computerSystemSearcher;

        private readonly ManagementObjectSearcher systemEnclosureSearcher;

        private readonly ManagementObjectSearcher batterySearcher;

        private readonly ManagementObjectSearcher operatingSystemSearcher;

        private readonly List<ComputerSystemInfo> computerSystemInfoList;

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
        }

        /// <summary>
        /// The get WMI info.
        /// </summary>
        public void GetWMIInfo()
        {
            // TODO: SystemSKUNumber is not supported before Windows 10
            foreach (var queryObject in this.computerSystemSearcher.Get())
            {
                var computerSystemInfo = new ComputerSystemInfo
                {
                    Name = queryObject[WmiConstants.Name],
                    Status = queryObject[WmiConstants.Status],
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
                this.computerSystemInfoList.Add(computerSystemInfo);
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
        /// The report WMI info.
        /// </summary>
        public void ReportWMIInfo()
        {
            
        }
    }
}
