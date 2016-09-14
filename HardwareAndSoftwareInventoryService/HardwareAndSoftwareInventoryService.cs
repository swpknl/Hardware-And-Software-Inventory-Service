namespace HardwareAndSoftwareInventoryService
{
    using System;
    using System.Configuration;
    using System.ServiceProcess;
    using System.Threading.Tasks;

    using Autofac;

    using Constants;

    using Entities;

    using FilesWatcher.Contracts;

    using FileSystemPopulation.Contracts;
    using FileSystemPopulation.Impl;

    using Logger.Contracts;

    /// <summary>
    /// The hardware and software inventory service.
    /// </summary>
    public partial class HardwareAndSoftwareInventoryService : ServiceBase
    {
        private readonly IContainer container;

        private readonly ILogger logger;

        /// <summary>
        /// Initializes static members of the <see cref="HardwareAndSoftwareInventoryService"/> class
        /// </summary>
        static HardwareAndSoftwareInventoryService()
        {
            ConfigurationKeys.DbUserName = ConfigurationManager.AppSettings[ConfigurationKeysConstants.DbUsername];
            ConfigurationKeys.DbPassword = ConfigurationManager.AppSettings[ConfigurationKeysConstants.DbPassword];
            ConfigurationKeys.DirectoriesToExclude =
                ConfigurationManager.AppSettings[ConfigurationKeysConstants.DirectoryToExclude].Split(
                    Delimiters.ConfigKeyDelimiter,
                    StringSplitOptions.RemoveEmptyEntries);
            ConfigurationKeys.FileTypesToMonitor =
                ConfigurationManager.AppSettings[ConfigurationKeysConstants.FileTypesToMonitor].Split(
                    Delimiters.ConfigKeyDelimiter,
                    StringSplitOptions.RemoveEmptyEntries);
            ConfigurationKeys.ReportingInterval =
                Convert.ToDouble(ConfigurationManager.AppSettings[ConfigurationKeysConstants.ReportingInterval]);
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
        }

        /// <summary>
        /// The on start method, that triggers the service operation and performs actions when the service is started.
        /// </summary>
        /// <param name="args">
        /// The arguments.
        /// </param>
        protected override void OnStart(string[] args)
        {
            this.container.Resolve<IFilesWatcher>().BeginMonitoringFiles();
        }

        /// <summary>
        /// The on stop method that contains the actions to be performed when the service is stopped.
        /// </summary>
        protected override void OnStop()
        {
        }
    }
}