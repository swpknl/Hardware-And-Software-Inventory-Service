namespace Entities
{
    /// <summary>
    /// The computer system product info.
    /// </summary>
    public class ComputerSystemProductInfo
    {
        public object IdentifyingNumber { get; set; }

        public object UUID { get; set; }

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
            return Equals((ComputerSystemProductInfo)obj);
        }

        protected bool Equals(ComputerSystemProductInfo other)
        {
            return Equals(this.IdentifyingNumber, other.IdentifyingNumber) && Equals(this.UUID, other.UUID);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((this.IdentifyingNumber != null ? this.IdentifyingNumber.GetHashCode() : 0) * 397) ^ (this.UUID != null ? this.UUID.GetHashCode() : 0);
            }
        }
    }
}
