namespace Constants
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// The hard disk drive information.
    /// </summary>
    public static class HardDiskDrivesInfo
    {
        /// <summary>
        /// Initializes static members of the <see cref="HardDiskDrivesInfo"/> class.
        /// </summary>
        static HardDiskDrivesInfo()
        {
            HardDiskDrives = DriveInfo.GetDrives().
                Where(diskDrive => diskDrive.DriveType == DriveType.Fixed)
                .ToList();
        }

        /// <summary>
        /// Gets the hard disk drives on the current system
        /// </summary>
        public static List<DriveInfo> HardDiskDrives { get; private set; }
    }
}
