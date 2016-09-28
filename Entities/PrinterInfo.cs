namespace Entities
{
    /// <summary>
    /// The printer info.
    /// </summary>
    public class PrinterInfo
    {
        public object Name { get; set; }

        public object Manufacturer { get; set; }

        public object DriverName { get; set; }

        public object Location { get; set; }

        public object ServerName { get; set; }

        public object VerticalResolution { get; set; }

        public object HorizontalResolution { get; set; }

        public object Hidden { get; set; }

        public object PortName { get; set; }

        public object Status { get; set; }

        public object Shared { get; set; }

        public object Default { get; set; }

        public object WorkOffline { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((PrinterInfo)obj);
        }

        protected bool Equals(PrinterInfo other)
        {
            return Equals(this.Name, other.Name) && Equals(this.Manufacturer, other.Manufacturer) && Equals(this.DriverName, other.DriverName) && Equals(this.Location, other.Location) && Equals(this.ServerName, other.ServerName) && Equals(this.VerticalResolution, other.VerticalResolution) && Equals(this.HorizontalResolution, other.HorizontalResolution) && Equals(this.Hidden, other.Hidden) && Equals(this.PortName, other.PortName) && Equals(this.Status, other.Status) && Equals(this.Shared, other.Shared) && Equals(this.Default, other.Default) && Equals(this.WorkOffline, other.WorkOffline);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (this.Name != null ? this.Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.Manufacturer != null ? this.Manufacturer.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.DriverName != null ? this.DriverName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.Location != null ? this.Location.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.ServerName != null ? this.ServerName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.VerticalResolution != null ? this.VerticalResolution.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.HorizontalResolution != null ? this.HorizontalResolution.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.Hidden != null ? this.Hidden.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.PortName != null ? this.PortName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.Status != null ? this.Status.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.Shared != null ? this.Shared.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.Default != null ? this.Default.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.WorkOffline != null ? this.WorkOffline.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}
