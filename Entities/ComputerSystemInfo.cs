namespace Entities
{
    /// <summary>
    /// The computer system info.
    /// </summary>
    public class ComputerSystemInfo
    {
        public object Name { get; set; }

        public object Status { get; set; }

        public object IsVirtual { get; set; }

        public object Hypervisor { get; set; }

        public object IsServer { get; set; }

        public object IsPortable { get; set; }

        public object ComputerId { get; set; }

        public object PrimaryOwnerName { get; set; }

        public object SystemSKUNumber { get; set; }

        public object SystemType { get; set; }

        public object ThermalState { get; set; }

        public object PartOfDomain { get; set; }

        public object Domain { get; set; }

        public object Workgroup { get; set; }

        public object CurrentTimeZone { get; set; }

        public object Manufacturer { get; set; }

        public object Model { get; set; }
    }
}
