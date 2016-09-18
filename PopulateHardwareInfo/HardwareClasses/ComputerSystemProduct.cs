namespace PopulateWMIInfo.Rules
{
    using System.Collections.Generic;
    using System.Management;

    using Constants;

    using Entities;

    using PopulateWMIInfo.Contracts;

    /// <summary>
    /// The computer system product WMI info.
    /// </summary>
    public class ComputerSystemProduct : IWmiInfo
    {
        private ManagementObjectSearcher searcher;

        private List<ComputerSystemProductInfo> computerSystemProductInfoList;

        /// <summary>
        /// Initializes a new instance of the <see cref="ComputerSystemProduct"/> class.
        /// </summary>
        public ComputerSystemProduct()
        {
            this.searcher = new ManagementObjectSearcher(
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
            foreach (var queryObject in this.searcher.Get())
            {
                var computerSystemProductInfo = new ComputerSystemProductInfo
                                                    {
                                                        IdentifyingNumber = queryObject[WmiConstants.IdentifyingNumber],
                                                        UUID = queryObject[WmiConstants.UUID]
                                                    };
                this.computerSystemProductInfoList.Add(computerSystemProductInfo);
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
