namespace PopulateRegistryInformation.RegistryKeys
{
    using Constants;

    using Logger.Contracts;

    using PopulateRegistryInformation.Contracts;

    /// <summary>
    /// The Microsoft registry info.
    /// </summary>
    internal class MicrosoftRegistryInfo : PopulateRegistryInfoAbstractBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MicrosoftRegistryInfo"/> class.
        /// </summary>
        public MicrosoftRegistryInfo(ILogger logger) : base(RegistryKeyConstants.MicrosoftNode, logger)
        {
            
        }
    }
}
