namespace Entities
{
    /// <summary>
    /// The partition info.
    /// </summary>
    public class PartitionInfo
    {
        public object Name { get; set; }

        public object Description { get; set; }

        public object DiskIndex { get; set; }

        public object Bootable { get; set; }

        public object BootPartition { get; set; }

        public object Size { get; set; }

        public object StartingOffset { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((PartitionInfo)obj);
        }

        protected bool Equals(PartitionInfo other)
        {
            return Equals(this.Name, other.Name) && Equals(this.Description, other.Description) && Equals(this.DiskIndex, other.DiskIndex) && Equals(this.Bootable, other.Bootable) && Equals(this.BootPartition, other.BootPartition) && Equals(this.Size, other.Size) && Equals(this.StartingOffset, other.StartingOffset);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (this.Name != null ? this.Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.Description != null ? this.Description.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.DiskIndex != null ? this.DiskIndex.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.Bootable != null ? this.Bootable.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.BootPartition != null ? this.BootPartition.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.Size != null ? this.Size.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.StartingOffset != null ? this.StartingOffset.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}
