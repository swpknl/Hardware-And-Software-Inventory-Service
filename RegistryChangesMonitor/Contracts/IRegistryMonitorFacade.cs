namespace RegistryChangesMonitor.Contracts
{
    /// <summary>
    /// The RegistryMonitorClient interface.
    /// </summary>
    public interface IRegistryMonitorFacade
    {
        /// <summary>
        /// The monitor registry.
        /// </summary>
        void BeginMonitoringRegistry();

        /// <summary>
        /// The stop monitoring registry.
        /// </summary>
        void StopMonitoringRegistry();
    }
}
