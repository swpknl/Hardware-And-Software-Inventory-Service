namespace Entities
{
    /// <summary>
    /// The diskdrive info.
    /// </summary>
    public class DiskdriveInfo
    {
        public object Name { get; set; }

        public object Model { get; set; }

        public object Manufacturer { get; set; }

        public object InterfaceType { get; set; }

        public object Size { get; set; }

        public object MediaType { get; set; }

        public object DiskDrive { get; set; }

        public object FirmwareRevisions { get; set; }

        public object Partitions { get; set; }

        public object SerialNumber { get; set; }

        /// <summary>
        /// The equals.
        /// </summary>
        /// <param name="other">
        /// The other.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public override bool Equals(object obj)
        {
            var other = (DiskdriveInfo)obj;
            return Equals(this.Name, other.Name) && Equals(this.Model, other.Model) && Equals(this.Manufacturer, other.Manufacturer) && 
                Equals(this.InterfaceType, other.InterfaceType) && Equals(this.Size, other.Size) && Equals(this.MediaType, other.MediaType) && 
                Equals(this.DiskDrive, other.DiskDrive) && Equals(this.FirmwareRevisions, other.FirmwareRevisions) && Equals(this.Partitions, other.Partitions) 
                && Equals(this.SerialNumber, other.SerialNumber);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (this.Name != null ? this.Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.Model != null ? this.Model.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.Manufacturer != null ? this.Manufacturer.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.InterfaceType != null ? this.InterfaceType.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.Size != null ? this.Size.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.MediaType != null ? this.MediaType.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.DiskDrive != null ? this.DiskDrive.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.FirmwareRevisions != null ? this.FirmwareRevisions.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.Partitions != null ? this.Partitions.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.SerialNumber != null ? this.SerialNumber.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}
