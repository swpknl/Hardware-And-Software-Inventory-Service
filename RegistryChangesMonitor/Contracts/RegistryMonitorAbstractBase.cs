namespace RegistryChangesMonitor.Contracts
{
    using System.Diagnostics;

    using RegistryChangesMonitor.Impl;

    /// <summary>
    /// The registry monitor abstract base.
    /// </summary>
    internal abstract class RegistryMonitorAbstractBase : IRegistryMonitor
    {
        private RegistryKeyChange regKeyMonitor;        

        private RegistryTreeChange regTreeMonitor;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistryMonitorAbstractBase"/> class.
        /// </summary>
        /// <param name="hive">
        /// The hive.
        /// </param>
        /// <param name="keyName">
        /// The key Name.
        /// </param>
        protected RegistryMonitorAbstractBase(string hive, string keyName)
        {
            this.regKeyMonitor = new RegistryKeyChange(hive, keyName);
            this.regKeyMonitor.RegistryKeyChanged += this.RegMonitor_RegistryKeyChanged;
            this.regTreeMonitor = new RegistryTreeChange(hive, keyName);
            this.regTreeMonitor.RegistryTreeChanged += this.RegTreeMonitor_RegistryTreeChanged;
        }

        /// <summary>
        /// The begin registry monitoring.
        /// </summary>
        public void BeginRegistryMonitoring()
        {
            this.regKeyMonitor.Start();
            this.regTreeMonitor.Start();
        }

        /// <summary>
        /// The stop registry monitoring.
        /// </summary>
        public void StopRegistryMonitoring()
        {
            this.regKeyMonitor.Stop();
            this.regTreeMonitor.Stop();
        }

        public void ReportRegistryChanges()
        {
            // TODO: Report the changes
        }

        /// <summary>
        /// The registry tree changed event.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The event.
        /// </param>
        private void RegTreeMonitor_RegistryTreeChanged(object sender, RegistryTreeChangedEventArgs e)
        {
            var message = string.Format(
                "Registry key changed for hive: {0} and Rootpath: {1}",
                e.RegistryTreeChangeData.Hive,
                e.RegistryTreeChangeData.RootPath);
            Trace.WriteLine(message);
        }

        /// <summary>
        /// The registry key changed event.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The event.
        /// </param>
        private void RegMonitor_RegistryKeyChanged(object sender, RegistryKeyChangedEventArgs e)
        {
            var message = string.Format(
                "Registry key changed for hive: {0} and key: {1}",
                e.RegistryKeyChangeData.Hive,
                e.RegistryKeyChangeData.KeyPath);
            Trace.WriteLine(message);
        }
    }
}
