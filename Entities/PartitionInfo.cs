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
    }
}
