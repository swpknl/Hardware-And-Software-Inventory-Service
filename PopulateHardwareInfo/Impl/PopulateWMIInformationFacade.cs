namespace PopulateWMIInfo.Impl
{
    using System.Collections.Generic;

    using PopulateWMIInfo.Contracts;
    using PopulateWMIInfo.Rules;

    using ReportToRestEndpoint.Contracts;

    /// <summary>
    /// Class for populating hardware information.
    /// </summary>
    public class PopulateWmiInformationFacade : IPopulateWMIInfoFacade
    {
        private List<IWmiInfo> wmiInfoList;

        private IVisitor visitor;

        /// <summary>
        /// Initializes a new instance of the <see cref="PopulateWmiInformationFacade"/> class.
        /// </summary>
        public PopulateWmiInformationFacade(IVisitor visitor)
        {
            this.wmiInfoList = new List<IWmiInfo>()
                                 {
                                     new Baseboard(),
                                     new BIOS(),
                                     new ComputerSystem(),
                                     new ComputerSystemProduct(),
                                     new CPU(),
                                     new DiskDrive(),
                                     new LogicalDisk(),
                                     new OperatingSystem(),
                                     new Partition(),
                                     new PortConnector(),
                                     new Printer(),
                                     new SoftwareLicensingService()
                                 };
            this.visitor = visitor;
        }

        /// <summary>
        /// The populate hardware info method, which uses a rule based engine for each hardware type.
        /// </summary>
        public void PopulateWMIInfo()
        {
            foreach (var wmiClass in this.wmiInfoList)
            {
                wmiClass.GetWMIInfo();
                wmiClass.ReportWMIInfo(this.visitor);
            }
        }
    }
}
