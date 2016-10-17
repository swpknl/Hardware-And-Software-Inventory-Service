namespace FilesWatcher.Impl
{
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Security.Permissions;

    using Constants;

    using Entities;

    using FilesWatcher.Contracts;

    using Helpers;

    using Logger.Contracts;

    using Newtonsoft.Json.Linq;

    using ReportToRestEndpoint.Contracts;

    /// <summary>
    /// The file system watcher.
    /// </summary>
    public class FilesSystemWatcher : IFilesWatcher
    {
        private const string SoftwareTableName = "software";

        private readonly IVisitor visitor;

        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="FilesSystemWatcher"/> class.
        /// </summary>
        /// <param name="visitor">
        /// The visitor.
        /// </param>
        /// <param name="logger">
        /// The logger.
        /// </param>
        public FilesSystemWatcher(IVisitor visitor, ILogger logger)
        {
            this.visitor = visitor;
            this.logger = logger;
        }

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
                    watcher.Changed += this.OnChanged;
                    watcher.Created += this.OnChanged;
                    watcher.Deleted += this.OnChanged;
                    watcher.Renamed += this.OnRenamed;
                    
                    this.logger.LogInfo(
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
        private void OnChanged(object source, FileSystemEventArgs e)
        {
            // Specify what is done when a file is changed, created, or deleted.
            if (!ConfigurationKeys.DirectoriesToExclude.Any(directory => e.FullPath.Contains(directory)))
            {
                this.logger.LogInfo("File system change for the file: " + e.FullPath + ". Action occured: " + e.ChangeType);
                var file = new FileInfo(e.FullPath);
                var fileSystemInfo = new FileSystemInformation
                {
                    FileName = file.Name,
                    FilePath = file.FullName,
                    Extension = file.Extension
                };
                int id;
                fileSystemInfo.PopulateMetaData();
                var data = this.ConvertFileSystemInfoToJson(fileSystemInfo);
                this.visitor.Visit(SoftwareTableName, data, out id);
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
        private void OnRenamed(object source, RenamedEventArgs e)
        {
            // Specify what is done when a file is renamed.
            if (!ConfigurationKeys.DirectoriesToExclude.Any(directory => e.FullPath.Contains(directory)))
            {
                this.logger.LogInfo("File rename occured from " + e.OldFullPath + " to " + e.FullPath);
                var file = new FileInfo(e.FullPath);
                var fileSystemInfo = new FileSystemInformation
                {
                    FileName = file.Name,
                    FilePath = file.FullName,
                    Extension = file.Extension
                };
                fileSystemInfo.PopulateMetaData();
                int id;
                var data = this.ConvertFileSystemInfoToJson(fileSystemInfo);
                this.visitor.Visit(SoftwareTableName, data, out id);
            }
        }

        /// <summary>
        /// The convert file system info to json.
        /// </summary>
        /// <param name="fileSystemInfo">
        /// The file system info.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private string ConvertFileSystemInfoToJson(FileSystemInformation fileSystemInfo)
        {
            var data =
                new JObject(
                    new JProperty(
                        "resource",
                        new JArray(
                                new JObject(
                                new JProperty("executable_path", fileSystemInfo.FilePath),
                                new JProperty("md5", fileSystemInfo.Md5),
                                new JProperty("product_desc", fileSystemInfo.Description),
                                new JProperty("product_name", fileSystemInfo.ProductName),
                                new JProperty("file_version", fileSystemInfo.FileVersion),
                                new JProperty("manufacturer", fileSystemInfo.Manufacturer),
                                new JProperty("executable_file", fileSystemInfo.FileName),
                                new JProperty("trademark", fileSystemInfo.TradeMark),
                                new JProperty("executable_extention", fileSystemInfo.Extension),
                                new JProperty("size", fileSystemInfo.FileSize == string.Empty ? "0" : fileSystemInfo.FileSize),
                                new JProperty("os_type", 1),
                                new JProperty("is_registry", "FALSE")))));
            return data.ToString();
        }
    }
}
