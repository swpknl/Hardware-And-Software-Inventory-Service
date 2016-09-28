namespace Entities
{
    /// <summary>
    /// The operating system info.
    /// </summary>
    public class OperatingSystemInfo
    {
        public object OSType { get; set; }

        public object Caption { get; set; }

        public object Manufacturer { get; set; }

        public object Version { get; set; }

        public object CSDVersion { get; set; }

        public object SerialNumber { get; set; }

        public object OSArchitecture { get; set; }

        public object OperatingSystemSKU { get; set; }

        public object Locale { get; set; }

        public object CountryCode { get; set; }

        public object OSLanguage { get; set; }

        public object Organization { get; set; }

        public object SystemDirectory { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((OperatingSystemInfo)obj);
        }

        protected bool Equals(OperatingSystemInfo other)
        {
            return Equals(this.OSType, other.OSType) && Equals(this.Caption, other.Caption) && Equals(this.Manufacturer, other.Manufacturer) && Equals(this.Version, other.Version) && Equals(this.CSDVersion, other.CSDVersion) && Equals(this.SerialNumber, other.SerialNumber) && Equals(this.OSArchitecture, other.OSArchitecture) && Equals(this.OperatingSystemSKU, other.OperatingSystemSKU) && Equals(this.Locale, other.Locale) && Equals(this.CountryCode, other.CountryCode) && Equals(this.OSLanguage, other.OSLanguage) && Equals(this.Organization, other.Organization) && Equals(this.SystemDirectory, other.SystemDirectory);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (this.OSType != null ? this.OSType.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.Caption != null ? this.Caption.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.Manufacturer != null ? this.Manufacturer.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.Version != null ? this.Version.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.CSDVersion != null ? this.CSDVersion.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.SerialNumber != null ? this.SerialNumber.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.OSArchitecture != null ? this.OSArchitecture.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.OperatingSystemSKU != null ? this.OperatingSystemSKU.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.Locale != null ? this.Locale.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.CountryCode != null ? this.CountryCode.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.OSLanguage != null ? this.OSLanguage.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.Organization != null ? this.Organization.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.SystemDirectory != null ? this.SystemDirectory.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}
