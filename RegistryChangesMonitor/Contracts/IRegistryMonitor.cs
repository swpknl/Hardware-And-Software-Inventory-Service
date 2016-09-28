namespace RegistryChangesMonitor.Contracts
{
    using ReportToRestEndpoint.Contracts;

    /// <summary>
    /// The RegistryMonitor interface.
    /// </summary>
    public interface IRegistryMonitor
    {
        /// <summary>
        /// The begin registry monitoring.
        /// </summary>
        void BeginRegistryMonitoring();

        /// <summary>
        /// The stop registry monitoring.
        /// </summary>
        void StopRegistryMonitoring();

        /// <summary>
        /// The report registry changes.
        /// </summary>
        /// <param name="data">
        /// The data.
        /// </param>
        void ReportRegistryChanges(string data);
    }
}
