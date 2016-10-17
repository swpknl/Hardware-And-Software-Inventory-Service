namespace HardwareAndSoftwareInventoryService
{
    using System;
    using System.ServiceProcess;
    using System.Threading.Tasks;
    using System.Timers;

    using Autofac;

    using Constants;

    using Entities;

    using FilesWatcher.Contracts;

    using FileSystemPopulation.Contracts;

    using Logger.Contracts;

    using PopulateRegistryInformation.Contracts;

    using PopulateWMIInfo.Contracts;

    using RegistryChangesMonitor.Contracts;

    using ReportToRestEndpoint.Contracts;

    /// <summary>
    /// The hardware and software inventory service.
    /// </summary>
    public partial class HardwareAndSoftwareInventoryService : ServiceBase
    {
        private readonly IContainer container;

        private readonly ILogger logger;

        private readonly Timer reportingTimer;

        /// <summary>
        /// Initializes static members of the <see cref="HardwareAndSoftwareInventoryService"/> class
        /// </summary>
        static HardwareAndSoftwareInventoryService()
        {
            // TODO: Remove this when pushing in production
            System.Diagnostics.Debugger.Launch();
            ConfigurationKeys.DbUserName = Helpers.ConfigurationManager.TryGetConfigurationValue(ConfigurationKeysConstants.DbUsername);
            ConfigurationKeys.DbPassword = Helpers.ConfigurationManager.TryGetConfigurationValue(ConfigurationKeysConstants.DbPassword);
            ConfigurationKeys.DirectoriesToExclude =
                Helpers.ConfigurationManager.TryGetConfigurationValue(ConfigurationKeysConstants.DirectoryToExclude).Split(
                    Delimiters.ConfigKeyDelimiter,
                    StringSplitOptions.RemoveEmptyEntries);
            ConfigurationKeys.FileTypesToMonitor =
                Helpers.ConfigurationManager.TryGetConfigurationValue(ConfigurationKeysConstants.FileTypesToMonitor).Split(
                    Delimiters.ConfigKeyDelimiter,
                    StringSplitOptions.RemoveEmptyEntries);
            ConfigurationKeys.BaseTime =
                Helpers.TimeSpanHelper.GetTimeSpan(
                    Helpers.ConfigurationManager.TryGetConfigurationValue(ConfigurationKeysConstants.BaseTime));
            ConfigurationKeys.TimeSpan =
                Helpers.TimeSpanHelper.GetTimeSpan(
                    Helpers.ConfigurationManager.TryGetConfigurationValue(ConfigurationKeysConstants.TimeSpan));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HardwareAndSoftwareInventoryService"/> class.
        /// </summary>
        /// <param name="container">
        /// The container.
        /// </param>
        /// <param name="logger">
        /// The logger.
        /// </param>
        public HardwareAndSoftwareInventoryService(IContainer container, ILogger logger)
        {
            this.InitializeComponent();
            this.container = container;
            this.logger = logger;
            this.reportingTimer = new Timer();
        }

        /// <summary>
        /// The on start method, that triggers the service operation and performs actions when the service is started.
        /// </summary>
        /// <param name="args">
        /// The arguments.
        /// </param>
        protected override void OnStart(string[] args)
        {
            // TODO: Remove this when pushing in production
            System.Diagnostics.Debugger.Launch();

            Task.Factory.StartNew(() => this.container.Resolve<IPopulateFileSystem>().PopulateFiles())
                .ContinueWith((antecedent) => this.container.Resolve<IPopulateFileSystem>().ReportFilesInfo(this.container.Resolve<IVisitor>()))
                .ContinueWith((antecedent) => this.container.Resolve<IFilesWatcher>().BeginMonitoringFiles());
            Task.Factory.StartNew(() => this.container.Resolve<IPopulateWMIInfoFacade>().PopulateWMIInfo());
            Task.Factory.StartNew(() => this.container.Resolve<IPopulateRegistryInfoFacade>().PopulateRegistryInformation())
                .ContinueWith((antecedent) => this.container.Resolve<IRegistryMonitorFacade>().BeginMonitoringRegistry());
            ReportingTime.Time = Helpers.TimeSpanHelper.GetReportingTime(
                ConfigurationKeys.BaseTime,
                ConfigurationKeys.TimeSpan);
            this.reportingTimer.Elapsed += this.ReportingTimer_Elapsed;
            this.reportingTimer.Interval = 30000; // 30 seconds
            this.reportingTimer.Enabled = true;
        }

        /// <summary>
        /// The on stop method to trigger clean up actions.
        /// </summary>
        protected override void OnStop()
        {
            this.container.Resolve<IRegistryMonitorFacade>().StopMonitoringRegistry();
            this.container.Dispose();
            base.OnStop();
        }

        /// <summary>
        /// Event for checking the current time, and if it matches with the reporting time, then report to the database.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void ReportingTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            var currentTime = new CurrentSystemTime().GetCurrentSystemTime();
            if (Helpers.TimeSpanHelper.IsCurrentTimeEqualToReportingTime(currentTime))
            {
                this.container.Resolve<IPopulateWMIInfoFacade>().CheckForHardwareChanges();
            }
        }
    }
}