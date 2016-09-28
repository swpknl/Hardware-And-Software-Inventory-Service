namespace RegistryChangesMonitor.Rules
{
    using Constants;

    using Logger.Contracts;

    using RegistryChangesMonitor.Contracts;

    using ReportToRestEndpoint.Contracts;

    /// <summary>
    /// The Microsoft node registry monitor.
    /// </summary>
    internal class MicrosoftNodeRegistryMonitor : RegistryMonitorAbstractBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MicrosoftNodeRegistryMonitor"/> class.
        /// </summary>
        public MicrosoftNodeRegistryMonitor(ILogger logger, IVisitor visitor) :
            base(RegistryKeyConstants.HKEY_LOCAL_MACHINE, RegistryKeyConstants.MicrosoftNode, logger, visitor)
        {
            
        }
    }
}
