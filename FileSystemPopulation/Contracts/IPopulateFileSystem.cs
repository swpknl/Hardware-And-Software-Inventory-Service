namespace FileSystemPopulation.Contracts
{
    using ReportToRestEndpoint.Contracts;

    /// <summary>
    /// The PopulateFileSystem interface.
    /// </summary>
    public interface IPopulateFileSystem
    {
        /// <summary>
        /// Contract to populate files.
        /// </summary>
        void PopulateFiles();

        /// <summary>
        /// The report files info.
        /// </summary>
        void ReportFilesInfo(IVisitor visitor);
    }
}
