namespace FilesWatcher.Impl
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Security.Permissions;
    using System.Text;
    using System.Threading.Tasks;

    using Constants;

    using Entities;

    using FilesWatcher.Contracts;

    /// <summary>
    /// The file system watcher.
    /// </summary>
    public class FilesSystemWatcher : IFilesWatcher
    {
        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public void BeginMonitoringFiles()
        {
            foreach (var hardDiskDrive in HardDiskDrivesInfo.HardDiskDrives) // Create a file system watcher for every HDD in the system
            {
                foreach (var fileType in ConfigurationKeys.FileTypesToMonitor)
                {
                    FileSystemWatcher watcher = new FileSystemWatcher();
                    watcher.Path = hardDiskDrive.Name;
                    watcher.IncludeSubdirectories = true;
                    watcher.EnableRaisingEvents = true;
                    watcher.Filter = fileType;
                    watcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.DirectoryName;

                    // Add event handlers.
                    watcher.Changed += OnChanged;
                    watcher.Created += OnChanged;
                    watcher.Deleted += OnChanged;
                    watcher.Renamed += OnRenamed;
                }
            }
        }

        // Define the event handlers.
        private static void OnChanged(object source, FileSystemEventArgs e)
        {
            // Specify what is done when a file is changed, created, or deleted.
            if (!ConfigurationKeys.DirectoriesToExclude.Any(directory => e.FullPath.Contains(directory)))
            {
                // TODO: Handle the file changed event
            }
        }

        private static void OnRenamed(object source, RenamedEventArgs e)
        {
            // Specify what is done when a file is renamed.
            if (!ConfigurationKeys.DirectoriesToExclude.Any(directory => e.FullPath.Contains(directory)))
            {
                // TODO: Handle the file changed event
            }
        }
    }
}
