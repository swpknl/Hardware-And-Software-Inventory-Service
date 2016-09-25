namespace HardwareAndSoftwareInventoryService
{
    using System;
    using System.Configuration;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.ServiceProcess;
    using System.Text;
    using System.Threading.Tasks;

    using Autofac;

    using Constants;

    using Entities;

    using FilesWatcher.Contracts;

    using FileSystemPopulation.Contracts;

    using Logger.Contracts;

    using PopulateRegistryInformation.Contracts;
    using PopulateRegistryInformation.Impl;

    using PopulateWMIInfo.Contracts;

    using RegistryChangesMonitor.Contracts;

    /// <summary>
    /// The hardware and software inventory service.
    /// </summary>
    public partial class HardwareAndSoftwareInventoryService : ServiceBase
    {
        private readonly IContainer container;

        private readonly ILogger logger;
        private FileSystemWatcher fileSystemWatcher;
        private IntPtr deviceNotifyHandle;
        private IntPtr deviceEventHandle;
        private IntPtr directoryHandle;
        private Win32.ServiceControlHandlerEx callback;

        /// <summary>
        /// Initializes static members of the <see cref="HardwareAndSoftwareInventoryService"/> class
        /// </summary>
        static HardwareAndSoftwareInventoryService()
        {
            // TODO: Add timespan for reporting and create a configuration manager
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
            base.OnStart(args);

            // TODO: Remove this when pushing in production
            System.Diagnostics.Debugger.Launch();
            RegisterDeviceNotification();

            fileSystemWatcher = new FileSystemWatcher();
            fileSystemWatcher.Created += new FileSystemEventHandler(fileSystemWatcher_Created);
            fileSystemWatcher.Deleted += new FileSystemEventHandler(fileSystemWatcher_Deleted);
            fileSystemWatcher.Changed += new FileSystemEventHandler(fileSystemWatcher_Changed);
            fileSystemWatcher.Renamed += new RenamedEventHandler(fileSystemWatcher_Renamed);
            Task.Factory.StartNew(() => this.container.Resolve<IPopulateFileSystem>().PopulateFiles())
                .ContinueWith((antecedent) => this.container.Resolve<IFilesWatcher>().BeginMonitoringFiles());
            Task.Factory.StartNew(() => this.container.Resolve<IPopulateWMIInfoFacade>().PopulateWMIInfo());
            Task.Factory.StartNew(() => this.container.Resolve<IPopulateRegistryInfoFacade>().PopulateRegistryInformation())
                .ContinueWith((antecedent) => this.container.Resolve<IRegistryMonitorFacade>().BeginMonitoringRegistry());
        }

        /// <summary>
        /// The on stop method that contains the actions to be performed when the service is stopped.
        /// </summary>
        protected override void OnStop()
        {
            this.container.Resolve<IRegistryMonitorFacade>().StopMonitoringRegistry();
            this.container.Dispose();
        }

        private int ServiceControlHandler(int control, int eventType, IntPtr eventData, IntPtr context)
        {
            if (control == Win32.SERVICE_CONTROL_STOP || control == Win32.SERVICE_CONTROL_SHUTDOWN)
            {
                UnregisterHandles();
                Win32.UnregisterDeviceNotification(deviceEventHandle);

                base.Stop();
            }
            else if (control == Win32.SERVICE_CONTROL_DEVICEEVENT)
            {
                switch (eventType)
                {
                    case Win32.DBT_DEVICEARRIVAL:
                        Win32.DEV_BROADCAST_HDR hdr;
                        hdr = (Win32.DEV_BROADCAST_HDR)
                            Marshal.PtrToStructure(eventData, typeof(Win32.DEV_BROADCAST_HDR));

                        if (hdr.dbcc_devicetype == Win32.DBT_DEVTYP_DEVICEINTERFACE)
                        {
                            Win32.DEV_BROADCAST_DEVICEINTERFACE deviceInterface;
                            deviceInterface = (Win32.DEV_BROADCAST_DEVICEINTERFACE)
                                Marshal.PtrToStructure(eventData, typeof(Win32.DEV_BROADCAST_DEVICEINTERFACE));
                            string name = new string(deviceInterface.dbcc_name);
                            name = name.Substring(0, name.IndexOf('\0')) + "\\";

                            StringBuilder stringBuilder = new StringBuilder();
                            Win32.GetVolumeNameForVolumeMountPoint(name, stringBuilder, 100);

                            uint stringReturnLength = 0;
                            string driveLetter = "";

                            Win32.GetVolumePathNamesForVolumeNameW(stringBuilder.ToString(), driveLetter, (uint)driveLetter.Length, ref stringReturnLength);
                            if (stringReturnLength == 0)
                            {
                                // TODO handle error
                            }

                            driveLetter = new string(new char[stringReturnLength]);

                            if (!Win32.GetVolumePathNamesForVolumeNameW(stringBuilder.ToString(), driveLetter, stringReturnLength, ref stringReturnLength))
                            {
                                // TODO handle error
                            }

                            RegisterForHandle(driveLetter[0]);

                            fileSystemWatcher.Path = driveLetter[0] + ":\\";
                            fileSystemWatcher.EnableRaisingEvents = true;
                        }
                        break;
                    case Win32.DBT_DEVICEQUERYREMOVE:
                        UnregisterHandles();
                        fileSystemWatcher.EnableRaisingEvents = false;
                        break;
                }
            }

            return 0;
        }

        private void UnregisterHandles()
        {
            if (directoryHandle != IntPtr.Zero)
            {
                Win32.CloseHandle(directoryHandle);
                directoryHandle = IntPtr.Zero;
            }
            if (deviceNotifyHandle != IntPtr.Zero)
            {
                Win32.UnregisterDeviceNotification(deviceNotifyHandle);
                deviceNotifyHandle = IntPtr.Zero;
            }
        }

        private void RegisterForHandle(char c)
        {
            Win32.DEV_BROADCAST_HANDLE deviceHandle = new Win32.DEV_BROADCAST_HANDLE();
            int size = Marshal.SizeOf(deviceHandle);
            deviceHandle.dbch_size = size;
            deviceHandle.dbch_devicetype = Win32.DBT_DEVTYP_HANDLE;
            directoryHandle = CreateFileHandle(c + ":\\");
            deviceHandle.dbch_handle = directoryHandle;
            IntPtr buffer = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(deviceHandle, buffer, true);
            deviceNotifyHandle = Win32.RegisterDeviceNotification(this.ServiceHandle, buffer, Win32.DEVICE_NOTIFY_SERVICE_HANDLE);
            if (deviceNotifyHandle == IntPtr.Zero)
            {
                // TODO handle error
            }
        }

        private void RegisterDeviceNotification()
        {
            callback = new Win32.ServiceControlHandlerEx(ServiceControlHandler);
            Win32.RegisterServiceCtrlHandlerEx(this.ServiceName, callback, IntPtr.Zero);

            if (this.ServiceHandle == IntPtr.Zero)
            {
                // TODO handle error
            }

            Win32.DEV_BROADCAST_DEVICEINTERFACE deviceInterface = new Win32.DEV_BROADCAST_DEVICEINTERFACE();
            int size = Marshal.SizeOf(deviceInterface);
            deviceInterface.dbcc_size = size;
            deviceInterface.dbcc_devicetype = Win32.DBT_DEVTYP_DEVICEINTERFACE;
            IntPtr buffer = default(IntPtr);
            buffer = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(deviceInterface, buffer, true);
            deviceEventHandle = Win32.RegisterDeviceNotification(this.ServiceHandle, buffer, Win32.DEVICE_NOTIFY_SERVICE_HANDLE | Win32.DEVICE_NOTIFY_ALL_INTERFACE_CLASSES);
            if (deviceEventHandle == IntPtr.Zero)
            {
                // TODO handle error
            }
        }

        public static IntPtr CreateFileHandle(string driveLetter)
        {
            // open the existing file for reading          
            IntPtr handle = Win32.CreateFile(
                  driveLetter,
                  Win32.GENERIC_READ,
                  Win32.FILE_SHARE_READ | Win32.FILE_SHARE_WRITE,
                  0,
                  Win32.OPEN_EXISTING,
                  Win32.FILE_FLAG_BACKUP_SEMANTICS | Win32.FILE_ATTRIBUTE_NORMAL,
                  0);

            if (handle == Win32.INVALID_HANDLE_VALUE)
            {
                return IntPtr.Zero;
            }
            else
            {
                return handle;
            }
        }

        void fileSystemWatcher_Created(object sender, System.IO.FileSystemEventArgs e)
        {
            // TODO handle event
        }

        void fileSystemWatcher_Renamed(object sender, System.IO.RenamedEventArgs e)
        {
            // TODO handle event
        }

        void fileSystemWatcher_Changed(object sender, System.IO.FileSystemEventArgs e)
        {
            // TODO handle event
        }

        void fileSystemWatcher_Deleted(object sender, System.IO.FileSystemEventArgs e)
        {
            // TODO handle event
        }
    }
}