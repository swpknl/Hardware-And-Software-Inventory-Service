namespace PopulateRegistryInformation.Impl
{
    using System.Collections.Generic;

    using Constants;

    using Entities;

    using Logger.Contracts;

    using PopulateRegistryInformation.Contracts;

    /// <summary>
    /// The WOW6432Node registry info.
    /// </summary>
    internal class Wow6432NodeRegistryInfo : PopulateRegistryInfoAbstractBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Wow6432NodeRegistryInfo"/> class.
        /// </summary>
        /// <param name="logger">
        /// The logger.
        /// </param>
        public Wow6432NodeRegistryInfo(ILogger logger) : base(RegistryKeyConstants.Wow6432Node, logger)
        {
            
        }
    }
}
