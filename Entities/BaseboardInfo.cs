namespace Entities
{
    /// <summary>
    /// The baseboard info.
    /// </summary>
    public class BaseboardInfo
    {
        public object Product { get; set; }

        public object Manufacturer { get; set; }

        public object HotSwappable { get; set; }

        public object HostingBoard { get; set; }

        public object Removable { get; set; }

        public object Replaceable { get; set; }

        public object RequiresDaughterBoard { get; set; }

        public object Version { get; set; }

        public object SerialNumber { get; set; }

        public object TotalCpuSockets { get; set; }

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
            var other = (BaseboardInfo)obj;
            return Equals(this.Product, other.Product) && Equals(this.Manufacturer, other.Manufacturer) && Equals(this.HotSwappable, other.HotSwappable) 
                && Equals(this.HostingBoard, other.HostingBoard) && Equals(this.Removable, other.Removable) && Equals(this.Replaceable, other.Replaceable) 
                && Equals(this.RequiresDaughterBoard, other.RequiresDaughterBoard) && Equals(this.Version, other.Version) 
                && Equals(this.SerialNumber, other.SerialNumber);
        }

        /// <summary>
        /// The get hash code.
        /// </summary>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (this.Product != null ? this.Product.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.Manufacturer != null ? this.Manufacturer.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.HotSwappable != null ? this.HotSwappable.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.HostingBoard != null ? this.HostingBoard.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.Removable != null ? this.Removable.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.Replaceable != null ? this.Replaceable.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.RequiresDaughterBoard != null ? this.RequiresDaughterBoard.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.Version != null ? this.Version.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.SerialNumber != null ? this.SerialNumber.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}
