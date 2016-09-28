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

    using PopulateRegistryInformation.Contracts;
    using PopulateRegistryInformation.Impl;

    using PopulateWMIInfo.Contracts;
    using PopulateWMIInfo.Impl;

    using RegistryChangesMonitor.Contracts;
    using RegistryChangesMonitor.Impl;

    using ReportToRestEndpoint.Contracts;
    using ReportToRestEndpoint.Impl;

    /// <summary>
    /// The main program.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// Gets or sets the container.
        /// </summary>
        private static IContainer Container { get; set; }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static void Main()
        {
            RegisterContainerTypes();
            var servicesToRun = new ServiceBase[]
                                              {
                                                  Container.Resolve<HardwareAndSoftwareInventoryService>()
                                              };
            ServiceBase.Run(servicesToRun);
        }

        /// <summary>
        /// Method to register container types.
        /// </summary>
        private static void RegisterContainerTypes()
        {
            System.Diagnostics.Debugger.Launch();
            var builder = new ContainerBuilder();
            
            builder.RegisterType<PopulateFileSystem>().As<IPopulateFileSystem>();
            builder.RegisterType<TraceLogger>().As<ILogger>().InstancePerDependency();
            builder.RegisterType<FilesSystemWatcher>().As<IFilesWatcher>();
            builder.RegisterType<RestApiVisitor>().As<IVisitor>().InstancePerDependency();
            builder.Register(
                componentContext => new HardwareAndSoftwareInventoryService(Container, componentContext.Resolve<ILogger>()));
            builder.Register(
                componentContext =>
                new PopulateWmiInformationFacade(
                    componentContext.Resolve<IVisitor>(),
                    componentContext.Resolve<ILogger>())).As<IPopulateWMIInfoFacade>();
            builder.Register(
                componentContext =>
                new PopulateRegistryInfoFacade(
                    componentContext.Resolve<ILogger>(),
                    componentContext.Resolve<IVisitor>())).As<IPopulateRegistryInfoFacade>();
            builder.Register(
                componentContext =>
                new RegistryMonitorFacade(componentContext.Resolve<ILogger>(), componentContext.Resolve<IVisitor>()))
                .As<IRegistryMonitorFacade>();

            Container = builder.Build();
        }
    }
}