namespace HardwareAndSoftwareInventoryService
{
    using System.ServiceProcess;

    using Autofac;

    using FilesWatcher.Contracts;
    using FilesWatcher.Impl;

    using FileSystemPopulation.Contracts;
    using FileSystemPopulation.Impl;

    using Logger.Contracts;
    using Logger.Impl;

    /// <summary>
    /// The main program.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// Gets or sets the container.
        /// </summary>
        public static IContainer Container { get; set; }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static void Main()
        {
            ServiceBase[] ServicesToRun;
            RegisterContainerTypes();
            ServicesToRun = new ServiceBase[]
            {
                Container.Resolve<HardwareAndSoftwareInventoryService>()
            };
            ServiceBase.Run(ServicesToRun);
        }

        /// <summary>
        /// Method to register container types.
        /// </summary>
        private static void RegisterContainerTypes()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<HardwareAndSoftwareInventoryService>().AsSelf();
            builder.RegisterType<ScanFileSystem>().As<IPopulateFileSystem>() // Populate the files and their metadata from the filesystem before the service starts
                .OnActivated(activationEvent => activationEvent.Instance.PopulateFiles()); 
            builder.RegisterType<WindowsEventLogger>().As<ILogger>();
            builder.RegisterType<FilesSystemWatcher>().As<IFilesWatcher>();

            Container = builder.Build();
        }
    }
}