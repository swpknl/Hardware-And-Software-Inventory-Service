namespace PopulateWMIInfo.Rules
{
    using System.Collections.Generic;
    using System.Management;

    using Constants;

    using Entities;

    using PopulateWMIInfo.Contracts;

    /// <summary>
    /// WMI Info for the baseboard.
    /// </summary>
    internal class Baseboard : IWmiInfo
    {
        private ManagementObjectSearcher searcher;

        private List<BaseboardInfo> baseboardInfoList;

        /// <summary>
        /// Initializes a new instance of the <see cref="Baseboard"/> class.
        /// </summary>
        public Baseboard()
        {
            this.baseboardInfoList = new List<BaseboardInfo>();
            this.searcher = new ManagementObjectSearcher(
                WmiConstants.WmiNamespace,
                string.Format(
                    "SELECT {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8} FROM Win32_BaseBoard",
                    WmiConstants.Product,
                    WmiConstants.Manufacturer,
                    WmiConstants.HotSwappable,
                    WmiConstants.HostingBoard,
                    WmiConstants.Removable,
                    WmiConstants.Replaceable,
                    WmiConstants.RequiresDaughterBoard,
                    WmiConstants.Version,
                    WmiConstants.SerialNumber));
        }

        /// <summary>
        /// The get WMI info method.
        /// </summary>
        public void GetWMIInfo()
        {
            foreach (var queryObj in this.searcher.Get())
            {
                var baseboardInfo = new BaseboardInfo
                                        {
                                            Product = queryObj[WmiConstants.Product],
                                            Manufacturer = queryObj[WmiConstants.Manufacturer],
                                            HotSwappable = queryObj[WmiConstants.HotSwappable],
                                            HostingBoard = queryObj[WmiConstants.HostingBoard],
                                            Removable = queryObj[WmiConstants.Removable],
                                            Replaceable = queryObj[WmiConstants.Replaceable],
                                            RequiresDaughterBoard = queryObj[WmiConstants.RequiresDaughterBoard],
                                            Version = queryObj[WmiConstants.Version],
                                            SerialNumber = queryObj[WmiConstants.SerialNumber]
                                        };
                this.baseboardInfoList.Add(baseboardInfo);
            }
        }

        /// <summary>
        /// The report WMI info method.
        /// </summary>
        public void ReportWMIInfo()
        {

        }
    }
}
