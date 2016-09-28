namespace PopulateRegistryInformation.Impl
{
    using System.Collections.Generic;

    using Logger.Contracts;

    using PopulateRegistryInformation.Contracts;
    using PopulateRegistryInformation.RegistryKeys;

    using ReportToRestEndpoint.Contracts;

    /// <summary>
    /// The populate registry info.
    /// </summary>
    public class PopulateRegistryInfoFacade : IPopulateRegistryInfoFacade
    {
        private List<IPopulateRegistryInfo> registryKeysList;

        private ILogger logger;

        private IVisitor visitor;

        /// <summary>
        /// Initializes a new instance of the <see cref="PopulateRegistryInfoFacade"/> class.
        /// </summary>
        /// <param name="logger">
        /// The logger.
        /// </param>
        /// <param name="visitor">
        /// The visitor.
        /// </param>
        public PopulateRegistryInfoFacade(ILogger logger, IVisitor visitor)
        {
            this.logger = logger;
            this.registryKeysList = new List<IPopulateRegistryInfo>
                                        {
                                            new Wow6432NodeRegistryInfo(this.logger),
                                            new MicrosoftRegistryInfo(this.logger)
                                        };
            this.visitor = visitor;
        }

        /// <summary>
        /// The populate registry information.
        /// </summary>
        public void PopulateRegistryInformation()
        {
            foreach (var registryInfo in this.registryKeysList)
            {
                registryInfo.PopulateRegistryInfo();
                registryInfo.ReportRegistryInfo(this.visitor);
            }
        }
    }
}
