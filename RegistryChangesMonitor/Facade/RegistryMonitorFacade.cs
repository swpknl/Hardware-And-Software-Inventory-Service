namespace RegistryChangesMonitor.Impl
{
    using System.Collections.Generic;

    using RegistryChangesMonitor.Contracts;
    using RegistryChangesMonitor.Rules;

    /// <summary>
    /// The registry monitor client.
    /// </summary>
    public class RegistryMonitorFacade : IRegistryMonitorFacade
    {
        private List<IRegistryMonitor> registryNodesToBeMonitoredList;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistryMonitorFacade"/> class.
        /// </summary>
        public RegistryMonitorFacade()
        {
            this.registryNodesToBeMonitoredList = new List<IRegistryMonitor>
                                                      {
                                                          new Wow6432NodeRegistryMonitor(),
                                                          new MicrosoftNodeRegistryMonitor()
                                                      };
        }

        /// <summary>
        /// The begin monitoring registry.
        /// </summary>
        public void BeginMonitoringRegistry()
        {
            foreach (var registryMonitor in this.registryNodesToBeMonitoredList)
            {
                registryMonitor.BeginRegistryMonitoring();
            }    
        }

        /// <summary>
        /// The stop monitoring registry.
        /// </summary>
        public void StopMonitoringRegistry()
        {
            foreach (var registryMonitor in this.registryNodesToBeMonitoredList)
            {
                registryMonitor.StopRegistryMonitoring();
            }
        }
    }
}
