namespace PopulateWMIInfo.Rules
{
    using System.Collections.Generic;
    using System.Management;

    using Constants;

    using Entities;

    using PopulateWMIInfo.Contracts;

    /// <summary>
    /// The port connector information.
    /// </summary>
    public class PortConnector : IWmiInfo
    {
        private ManagementObjectSearcher searcher;

        private List<PortConnectorInfo> portConnectorInfoList;

        /// <summary>
        /// Initializes a new instance of the <see cref="PortConnector"/> class.
        /// </summary>
        public PortConnector()
        {
            this.portConnectorInfoList = new List<PortConnectorInfo>();
            this.searcher = new ManagementObjectSearcher(
                WmiConstants.WmiRootNamespace,
                string.Format(
                    "SELECT {0}, {1}, {2} FROM Win32_PortConnector",
                    WmiConstants.Tag,
                    WmiConstants.InternalReferenceDesignator,
                    WmiConstants.ExternalReferenceDesignator));
        }

        /// <summary>
        /// The get WMI info.
        /// </summary>
        public void GetWMIInfo()
        {
            foreach (var queryObject in this.searcher.Get())
            {
                var portConnectorInfo = new PortConnectorInfo
                                            {
                                                Tag = queryObject[WmiConstants.Tag],
                                                InternalReferenceDesignator = queryObject[WmiConstants.InternalReferenceDesignator],
                                                ExternalReferenceDesignator = queryObject[WmiConstants.ExternalReferenceDesignator]
                                            };
                this.portConnectorInfoList.Add(portConnectorInfo);
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
