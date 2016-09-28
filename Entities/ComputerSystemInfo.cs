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
            return Equals((ComputerSystemInfo)obj);
        }

        protected bool Equals(ComputerSystemInfo other)
        {
            return Equals(this.Name, other.Name) && Equals(this.Status, other.Status) && Equals(this.IsVirtual, other.IsVirtual) && Equals(this.Hypervisor, other.Hypervisor) && Equals(this.IsServer, other.IsServer) && Equals(this.IsPortable, other.IsPortable) && Equals(this.ComputerId, other.ComputerId) && Equals(this.PrimaryOwnerName, other.PrimaryOwnerName) && Equals(this.SystemSKUNumber, other.SystemSKUNumber) && Equals(this.SystemType, other.SystemType) && Equals(this.ThermalState, other.ThermalState) && Equals(this.PartOfDomain, other.PartOfDomain) && Equals(this.Domain, other.Domain) && Equals(this.Workgroup, other.Workgroup) && Equals(this.CurrentTimeZone, other.CurrentTimeZone) && Equals(this.Manufacturer, other.Manufacturer) && Equals(this.Model, other.Model);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (this.Name != null ? this.Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.Status != null ? this.Status.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.IsVirtual != null ? this.IsVirtual.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.Hypervisor != null ? this.Hypervisor.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.IsServer != null ? this.IsServer.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.IsPortable != null ? this.IsPortable.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.ComputerId != null ? this.ComputerId.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.PrimaryOwnerName != null ? this.PrimaryOwnerName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.SystemSKUNumber != null ? this.SystemSKUNumber.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.SystemType != null ? this.SystemType.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.ThermalState != null ? this.ThermalState.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.PartOfDomain != null ? this.PartOfDomain.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.Domain != null ? this.Domain.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.Workgroup != null ? this.Workgroup.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.CurrentTimeZone != null ? this.CurrentTimeZone.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.Manufacturer != null ? this.Manufacturer.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.Model != null ? this.Model.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}
