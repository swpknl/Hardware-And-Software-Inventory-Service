namespace Entities
{
    /// <summary>
    /// The cpu info.
    /// </summary>
    public class CPUInfo
    {
        public object Name { get; set; }

        public object Manufacturer { get; set; }

        public object Model { get; set; }

        public object Description { get; set; }

        public object ThreadCount { get; set; }

        public object NumberOfCores { get; set; }

        public object NumberOfLogicalProcessors { get; set; }

        public object ProcessorId { get; set; }

        public object SocketDesignation { get; set; }

        public object MaxClockSpeed { get; set; }

        public object Voltage { get; set; }

        public object AddressWidth { get; set; }

        public object Device { get; set; }

        public object L2CacheSize { get; set; }

        public object L3CacheSize { get; set; }

        public object NumberOfEnabledCore { get; set; }

        public object CurrentClockSpeed { get; set; }

        public object SerialNumber { get; set; }

        public object VirtualizationFirmwareEnabled { get; set; }

        /// <summary>
        /// The equals.
        /// </summary>
        /// <param name="obj">
        /// The obj.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((CPUInfo)obj);
        }

        protected bool Equals(CPUInfo other)
        {
            return Equals(this.Name, other.Name) && Equals(this.Manufacturer, other.Manufacturer) && Equals(this.Model, other.Model) && Equals(this.Description, other.Description) && Equals(this.ThreadCount, other.ThreadCount) && Equals(this.NumberOfCores, other.NumberOfCores) && Equals(this.NumberOfLogicalProcessors, other.NumberOfLogicalProcessors) && Equals(this.ProcessorId, other.ProcessorId) && Equals(this.SocketDesignation, other.SocketDesignation) && Equals(this.MaxClockSpeed, other.MaxClockSpeed) && Equals(this.Voltage, other.Voltage) && Equals(this.AddressWidth, other.AddressWidth) && Equals(this.Device, other.Device) && Equals(this.L2CacheSize, other.L2CacheSize) && Equals(this.L3CacheSize, other.L3CacheSize) && Equals(this.NumberOfEnabledCore, other.NumberOfEnabledCore) && Equals(this.CurrentClockSpeed, other.CurrentClockSpeed) && Equals(this.SerialNumber, other.SerialNumber) && Equals(this.VirtualizationFirmwareEnabled, other.VirtualizationFirmwareEnabled);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (this.Name != null ? this.Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.Manufacturer != null ? this.Manufacturer.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.Model != null ? this.Model.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.Description != null ? this.Description.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.ThreadCount != null ? this.ThreadCount.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.NumberOfCores != null ? this.NumberOfCores.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.NumberOfLogicalProcessors != null ? this.NumberOfLogicalProcessors.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.ProcessorId != null ? this.ProcessorId.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.SocketDesignation != null ? this.SocketDesignation.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.MaxClockSpeed != null ? this.MaxClockSpeed.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.Voltage != null ? this.Voltage.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.AddressWidth != null ? this.AddressWidth.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.Device != null ? this.Device.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.L2CacheSize != null ? this.L2CacheSize.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.L3CacheSize != null ? this.L3CacheSize.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.NumberOfEnabledCore != null ? this.NumberOfEnabledCore.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.CurrentClockSpeed != null ? this.CurrentClockSpeed.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.SerialNumber != null ? this.SerialNumber.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.VirtualizationFirmwareEnabled != null ? this.VirtualizationFirmwareEnabled.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}
