namespace Constants
{
    /// <summary>
    /// The WMI constants.
    /// </summary>
    public static class WmiConstants
    {
        public const string WmiRootNamespace = "root\\CIMV2";

        public const string WmiMS409Namespace = "\\\\.\\ROOT\\CIMV2\\ms_409";

        public const string SerialNumber = "SerialNumber";

        public const string Manufacturer = "Manufacturer";

        public const string Version = "Version";

        #region Baseboard
        public const string Product = "Product";

        public const string HotSwappable = "HotSwappable";
        
        public const string HostingBoard = "HostingBoard";
        
        public const string Removable = "Removable";
        
        public const string Replaceable = "Replaceable";
        
        public const string RequiresDaughterBoard = "RequiresDaughterBoard";
        #endregion

        #region BIOS
        public const string SystemBIOSMajorVersion = "SystemBIOSMajorVersion";

        public const string SystemBIOSMinorVersion = "SystemBIOSMinorVersion";

        public const string SMBIOSBIOSVersion = "SMBIOSBIOSVersion";

        public const string SMBIOSMajorVersion = "SMBIOSMajorVersion";

        public const string SMBIOSMinorVersion = "SMBIOSMinorVersion";

        public const string ReleaseDate = "ReleaseDate";
        #endregion

        #region ComputerSystem
        public const string Name = "Name";

        public const string Status = "Status";

        public const string PrimaryOwnerName = "PrimaryOwnerName";

        public const string SystemSKUNumber = "SystemSKUNumber";

        public const string SystemType = "SystemType";

        public const string ThermalState = "ThermalState";

        public const string PartOfDomain = "PartOfDomain";

        public const string Domain = "Domain";

        public const string Workgroup = "Workgroup";

        public const string CurrentTimeZone = "CurrentTimeZone";

        public const string Model = "Model";
        #endregion

        #region SystemEnclosure
        public const string ChassisTypes = "ChassisTypes";
        #endregion

        #region Battery
        public const string StatusInfo = "StatusInfo";
        #endregion

        #region OperatingSystem
        public const string Caption = "Caption";

        public const string OSType = "OSType";

        public const string CSDVersion = "CSDVersion";

        public const string OSArchitecture = "OSArchitecture";

        public const string OperatingSystemSKU = "OperatingSystemSKU";

        public const string Locale = "Locale";

        public const string CountryCode = "CountryCode";

        public const string OSLanguage = "OSLanguage";

        public const string Organization = "Organization";

        public const string SystemDirectory = "SystemDirectory";
        #endregion

        #region ComputerSystemProduct
        public const string IdentifyingNumber = "IdentifyingNumber";

        public const string UUID = "UUID";
        #endregion

        #region CPU
        public const string Description = "Description";

        public const string ThreadCount = "ThreadCount";

        public const string NumberOfCores = "NumberOfCores";

        public const string NumberOfLogicalProcessors = "NumberOfLogicalProcessors";

        public const string ProcessorId = "ProcessorId";

        public const string SocketDesignation = "SocketDesignation";

        public const string MaxClockSpeed = "MaxClockSpeed";

        public const string CurrentVoltage = "CurrentVoltage";

        public const string AddressWidth = "AddressWidth";

        public const string DeviceID = "DeviceID";

        public const string L2CacheSize = "L2CacheSize";

        public const string L3CacheSize = "L3CacheSize";

        public const string NumberOfEnabledCore = "NumberOfEnabledCore";

        public const string CurrentClockSpeed = "CurrentClockSpeed";

        public const string VirtualizationFirmwareEnabled = "VirtualizationFirmwareEnabled";
        #endregion

        #region DiskDrive
        public const string InterfaceType = "InterfaceType";

        public const string Size = "Size";

        public const string MediaType = "MediaType";

        public const string Diskdrive = "Diskdrive";

        public const string FirmwareRevision = "FirmwareRevision";

        public const string Partitions = "Partitions";
        #endregion

        #region LogicalDrives
        public const string FileSystem = "FileSystem";

        public const string ProviderName = "Size";

        public const string SupportsFileBasedCompression = "SupportsFileBasedCompression";

        public const string SupportsDiskQuotas = "SupportsDiskQuotas";

        public const string FreeSpace = "FreeSpace";

        public const string Compressed = "Compressed";

        public const string VolumeSerialNumber = "VolumeSerialNumber";

        public const string VolumeName = "VolumeName";
        #endregion

        #region SoftwareLicensingService
        public const string OA3xOriginalProductKey = "OA3xOriginalProductKey";
        #endregion

        #region Partition
        public const string DiskIndex = "DiskIndex";

        public const string Bootable = "Bootable";

        public const string BootPartition = "BootPartition";
        
        public const string StartingOffset = "StartingOffset";
        #endregion

        #region PortConnector
        public const string Tag = "Tag";

        public const string InternalReferenceDesignator = "InternalReferenceDesignator";

        public const string ExternalReferenceDesignator = "ExternalReferenceDesignator";
        #endregion

        #region Printer
        public const string DriverName = "DriverName";

        public const string Location = "Location";

        public const string ServerName = "ServerName";

        public const string VerticalResolution = "VerticalResolution";

        public const string HorizontalResolution = "HorizontalResolution";

        public const string Hidden = "Hidden";

        public const string PortName = "PortName";

        public const string Shared = "Shared";

        public const string Default = "Default";

        public const string WorkOffline = "WorkOffline";
        #endregion
    }
}
