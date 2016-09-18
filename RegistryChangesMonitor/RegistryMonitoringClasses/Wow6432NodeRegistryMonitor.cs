namespace RegistryChangesMonitor.Rules
{
    using Constants;

    using RegistryChangesMonitor.Contracts;

    /// <summary>
    /// The WOW6432Node registry monitor.
    /// </summary>
    internal class Wow6432NodeRegistryMonitor : RegistryMonitorAbstractBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Wow6432NodeRegistryMonitor"/> class.
        /// </summary>
        public Wow6432NodeRegistryMonitor() : base(RegistryKeyConstants.HKEY_LOCAL_MACHINE, RegistryKeyConstants.Wow6432Node)
        {
            
        }
    }
}
