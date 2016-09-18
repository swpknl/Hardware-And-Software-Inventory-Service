namespace PopulateRegistryInformation.Impl
{
    using System.Collections.Generic;

    using Logger.Contracts;

    using PopulateRegistryInformation.Contracts;
    using PopulateRegistryInformation.RegistryKeys;

    /// <summary>
    /// The populate registry info.
    /// </summary>
    public class PopulateRegistryInfoFacade : IPopulateRegistryInfoFacade
    {
        private List<IPopulateRegistryInfo> registryKeysList;

        private ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="PopulateRegistryInfoFacade"/> class.
        /// </summary>
        public PopulateRegistryInfoFacade(ILogger logger)
        {
            this.logger = logger;
            this.registryKeysList = new List<IPopulateRegistryInfo>
                                        {
                                            new Wow6432NodeRegistryInfo(this.logger),
                                            new MicrosoftRegistryInfo(this.logger)
                                        };
        }

        /// <summary>
        /// The populate registry information.
        /// </summary>
        public void PopulateRegistryInformation()
        {
            foreach (var registryInfo in this.registryKeysList)
            {
                registryInfo.PopulateRegistryInfo();
                registryInfo.ReportRegistryInfo();
            }
        }
    }
}
