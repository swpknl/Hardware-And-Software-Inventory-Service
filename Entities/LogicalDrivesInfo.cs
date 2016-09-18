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
    }

}
