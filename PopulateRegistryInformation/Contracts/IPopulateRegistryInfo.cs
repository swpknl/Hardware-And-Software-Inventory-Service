namespace PopulateRegistryInformation.Contracts
{
    using ReportToRestEndpoint.Contracts;

    /// <summary>
    /// The PopulateRegistryInfo interface.
    /// </summary>
    public interface IPopulateRegistryInfo
    {
        /// <summary>
        /// The populate registry info.
        /// </summary>
        void PopulateRegistryInfo();

        /// <summary>
        /// The report registry info.
        /// </summary>
        /// <param name="visitor">
        /// The visitor.
        /// </param>
        void ReportRegistryInfo(IVisitor visitor);

    }
}
