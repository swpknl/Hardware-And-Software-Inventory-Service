namespace RegistryChangesMonitor.Rules
{
    using Constants;

    using RegistryChangesMonitor.Contracts;

    /// <summary>
    /// The Microsoft node registry monitor.
    /// </summary>
    internal class MicrosoftNodeRegistryMonitor : RegistryMonitorAbstractBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MicrosoftNodeRegistryMonitor"/> class.
        /// </summary>
        public MicrosoftNodeRegistryMonitor() : base(RegistryKeyConstants.HKEY_LOCAL_MACHINE, RegistryKeyConstants.MicrosoftNode)
        {
            
        }
    }
}
