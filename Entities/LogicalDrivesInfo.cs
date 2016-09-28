namespace Entities
{
    /// <summary>
    /// The logical drives info.
    /// </summary>
    public class LogicalDrivesInfo
    {
        public object Name { get; set; }

        public object Description { get; set; }

        public object FileSystem { get; set; }

        public object Size { get; set; }

        public object ProviderName { get; set; }

        public object SupportsFileCompression { get; set; }

        public object SupportsDiskQuotas { get; set; }

        public object FreeSpace { get; set; }

        public object Compressed { get; set; }

        public object VolumeSerialNumber { get; set; }

        public object VolumeName { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((LogicalDrivesInfo)obj);
        }

        protected bool Equals(LogicalDrivesInfo other)
        {
            return Equals(this.Name, other.Name) && Equals(this.Description, other.Description) && Equals(this.FileSystem, other.FileSystem) && Equals(this.Size, other.Size) && Equals(this.ProviderName, other.ProviderName) && Equals(this.SupportsFileCompression, other.SupportsFileCompression) && Equals(this.SupportsDiskQuotas, other.SupportsDiskQuotas) && Equals(this.FreeSpace, other.FreeSpace) && Equals(this.Compressed, other.Compressed) && Equals(this.VolumeSerialNumber, other.VolumeSerialNumber) && Equals(this.VolumeName, other.VolumeName);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (this.Name != null ? this.Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.Description != null ? this.Description.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.FileSystem != null ? this.FileSystem.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.Size != null ? this.Size.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.ProviderName != null ? this.ProviderName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.SupportsFileCompression != null ? this.SupportsFileCompression.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.SupportsDiskQuotas != null ? this.SupportsDiskQuotas.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.FreeSpace != null ? this.FreeSpace.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.Compressed != null ? this.Compressed.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.VolumeSerialNumber != null ? this.VolumeSerialNumber.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.VolumeName != null ? this.VolumeName.GetHashCode() : 0);
                return hashCode;
            }
        }
    }

}
