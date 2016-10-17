namespace Entities
{
    using System;

    /// <summary>
    /// The bios info.
    /// </summary>
    public class BIOSInfo
    {
        public object Manufacturer { get; set; }

        public object Version { get; set; }

        public object SystemBIOSMajorVersion { get; set; }

        public object SystemBIOSMinorVersion { get; set; }

        public object SMBIOSBIOSVersion { get; set; }

        public object SMBIOSMajorVersion { get; set; }

        public object SMBIOSMinorVersion { get; set; }

        public DateTime? ReleaseDate { get; set; }

        public object SerialNumber { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((BIOSInfo)obj);
        }

        protected bool Equals(BIOSInfo other)
        {
            return Equals(this.Manufacturer, other.Manufacturer) && Equals(this.Version, other.Version) && Equals(this.SystemBIOSMajorVersion, other.SystemBIOSMajorVersion) && Equals(this.SystemBIOSMinorVersion, other.SystemBIOSMinorVersion) && Equals(this.SMBIOSBIOSVersion, other.SMBIOSBIOSVersion) && Equals(this.SMBIOSMajorVersion, other.SMBIOSMajorVersion) && Equals(this.SMBIOSMinorVersion, other.SMBIOSMinorVersion) && Equals(this.ReleaseDate, other.ReleaseDate) && Equals(this.SerialNumber, other.SerialNumber);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (this.Manufacturer != null ? this.Manufacturer.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.Version != null ? this.Version.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.SystemBIOSMajorVersion != null ? this.SystemBIOSMajorVersion.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.SystemBIOSMinorVersion != null ? this.SystemBIOSMinorVersion.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.SMBIOSBIOSVersion != null ? this.SMBIOSBIOSVersion.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.SMBIOSMajorVersion != null ? this.SMBIOSMajorVersion.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.SMBIOSMinorVersion != null ? this.SMBIOSMinorVersion.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.ReleaseDate != null ? this.ReleaseDate.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.SerialNumber != null ? this.SerialNumber.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}
