namespace RegistryChangesMonitor.Rules
{
    using Constants;

    using Logger.Contracts;

    using RegistryChangesMonitor.Contracts;

    using ReportToRestEndpoint.Contracts;

    /// <summary>
    /// The WOW6432Node registry monitor.
    /// </summary>
    internal class Wow6432NodeRegistryMonitor : RegistryMonitorAbstractBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Wow6432NodeRegistryMonitor"/> class.
        /// </summary>
        public Wow6432NodeRegistryMonitor(ILogger logger, IVisitor visitor) 
            : base(RegistryKeyConstants.HKEY_LOCAL_MACHINE, RegistryKeyConstants.Wow6432Node, logger, visitor)
        {
            
        }
    }
}
