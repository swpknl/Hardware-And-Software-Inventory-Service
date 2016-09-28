namespace PopulateWMIInfo.Impl
{
    using System;
    using System.Collections.Generic;

    using Logger.Contracts;

    using PopulateWMIInfo.Contracts;
    using PopulateWMIInfo.Rules;

    using ReportToRestEndpoint.Contracts;

    using OperatingSystem = PopulateWMIInfo.Rules.OperatingSystem;

    /// <summary>
    /// Class for populating hardware information.
    /// </summary>
    public class PopulateWmiInformationFacade : IPopulateWMIInfoFacade
    {
        private readonly List<IWmiInfo> wmiInfoList;

        private readonly IVisitor visitor;

        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="PopulateWmiInformationFacade"/> class.
        /// </summary>
        /// <param name="visitor">
        /// The visitor.
        /// </param>
        public PopulateWmiInformationFacade(IVisitor visitor, ILogger logger)
        {
            this.wmiInfoList = new List<IWmiInfo>()
                                 {
                                     new Baseboard(),
                                     new BIOS(),
                                     new ComputerSystem(),
                                     new CPU(),
                                     new DiskDrive(),
                                     new LogicalDisk(),
                                     new OperatingSystem(),
                                     new Partition(),
                                     new PortConnector(),
                                     new Printer(),
                                 };
            this.visitor = visitor;
            this.logger = logger;
        }

        /// <summary>
        /// The populate hardware info method, which uses a rule based engine for each hardware type.
        /// </summary>
        public void PopulateWMIInfo()
        {
            foreach (var wmiClass in this.wmiInfoList)
            {
                try
                {
                    wmiClass.GetWMIInfo();
                    wmiClass.ReportWMIInfo(this.visitor);
                }
                catch (Exception ex)
                {
                    this.logger.LogException(
                        "An exception occurred while getting the hardware information for " + wmiClass.ToString(),
                        ex);
                }
            }
        }

        /// <summary>
        /// The check for hardware changes.
        /// </summary>
        public void CheckForHardwareChanges()
        {
            foreach (var wmiInfo in this.wmiInfoList)
            {
                try
                {
                    wmiInfo.CheckForHardwareChanges(this.visitor);
                }
                catch (Exception ex)
                {
                    this.logger.LogException(
                        "An exception occurred while reporting the hardware information for " + wmiInfo.ToString(),
                        ex);
                }
            }
        }
    }
}
