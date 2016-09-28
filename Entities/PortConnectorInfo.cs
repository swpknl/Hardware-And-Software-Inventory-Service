namespace Entities
{
    /// <summary>
    /// The port connector info.
    /// </summary>
    public class PortConnectorInfo
    {
        public object Tag { get; set; }

        public object InternalReferenceDesignator { get; set; }

        public object ExternalReferenceDesignator { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((PortConnectorInfo)obj);
        }

        protected bool Equals(PortConnectorInfo other)
        {
            return Equals(this.Tag, other.Tag) && Equals(this.InternalReferenceDesignator, other.InternalReferenceDesignator) && Equals(this.ExternalReferenceDesignator, other.ExternalReferenceDesignator);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (this.Tag != null ? this.Tag.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.InternalReferenceDesignator != null ? this.InternalReferenceDesignator.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.ExternalReferenceDesignator != null ? this.ExternalReferenceDesignator.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}
