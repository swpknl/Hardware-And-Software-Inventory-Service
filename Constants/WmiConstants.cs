namespace Constants
{
    /// <summary>
    /// The WMI constants.
    /// </summary>
    public static class WmiConstants
    {
        public const string WmiNamespace = "root\\CIMV2";

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
        public const string ChassisType = "ChassisType";
        #endregion

        #region Battery
        public const string StatusInfo = "StatusInfo";
        #endregion

        #region OperatingSystem
        public const string Caption = "Caption";
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

        public const string Voltage = "Voltage";

        public const string AddressWidth = "AddressWidth";

        public const string Device = "Device";

        public const string L2CacheSize = "L2CacheSize";

        public const string L3CacheSize = "L3CacheSize";

        public const string NumberOfEnabledCore = "NumberOfEnabledCore";

        public const string CurrentClockSpeed = "CurrentClockSpeed";

        public const string VirtualizationFirmwareEnabled = "VirtualizationFirmwareEnabled";
        #endregion
    }
}
