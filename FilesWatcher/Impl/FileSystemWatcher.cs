namespace FilesWatcher.Impl
{
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Security.Permissions;

    using Constants;

    using Entities;

    using FilesWatcher.Contracts;

    /// <summary>
    /// The file system watcher.
    /// </summary>
    public class FilesSystemWatcher : IFilesWatcher
    {
        /// <summary>
        /// The begin monitoring files.
        /// </summary>
        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public void BeginMonitoringFiles()
        {
            // Create a file system watcher for every HDD in the system
            foreach (var hardDiskDrive in HardDiskDrivesInfo.HardDiskDrives)
            {
                foreach (var fileType in ConfigurationKeys.FileTypesToMonitor)
                {
                    FileSystemWatcher watcher = new FileSystemWatcher();
                    watcher.Path = hardDiskDrive.Name;
                    watcher.IncludeSubdirectories = true;
                    watcher.EnableRaisingEvents = true;
                    watcher.Filter = fileType;
                    watcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.DirectoryName | NotifyFilters.CreationTime
                        | NotifyFilters.LastWrite | NotifyFilters.Size;

                    // Add event handlers.
                    watcher.Changed += OnChanged;
                    watcher.Created += OnChanged;
                    watcher.Deleted += OnChanged;
                    watcher.Renamed += OnRenamed;
                    
                    Trace.WriteLine(
                        "Creating the FileSystemWatcher class for the disk drive: " + hardDiskDrive
                        + " and file extenion: " + fileType);
                }
            }
        }

        /// <summary>
        /// The on changed event handler for any changes in the file system.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <param name="e">
        /// The file.
        /// </param>
        private static void OnChanged(object source, FileSystemEventArgs e)
        {
            // Specify what is done when a file is changed, created, or deleted.
            if (!ConfigurationKeys.DirectoriesToExclude.Any(directory => e.FullPath.Contains(directory)))
            {
                Trace.WriteLine("File system change for the file: " + e.FullPath + ". Action occured: " + e.ChangeType);
                
                // TODO: Handle the file changed event
            }
        }

        /// <summary>
        /// The on renamed event handler for file rename.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <param name="e">
        /// The file.
        /// </param>
        private static void OnRenamed(object source, RenamedEventArgs e)
        {
            // Specify what is done when a file is renamed.
            if (!ConfigurationKeys.DirectoriesToExclude.Any(directory => e.FullPath.Contains(directory)))
            {
                Trace.WriteLine("File rename occured from " + e.OldFullPath + " to " + e.FullPath);
                
                // TODO: Handle the file changed event
            }
        }
    }
}
