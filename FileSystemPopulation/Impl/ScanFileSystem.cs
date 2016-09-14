namespace FileSystemPopulation.Impl
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    using Constants;

    using Contracts;

    using Logger.Contracts;
    using System.Linq;
    using System.Threading.Tasks;

    using Helpers;
    using Entities;

    /// <summary>
    /// Class to scan the file system.
    /// </summary>
    public class ScanFileSystem : IPopulateFileSystem
    {
        private readonly ILogger logger;

        private readonly List<FileSystemInformation> fileSystemInformationList;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScanFileSystem"/> class.
        /// </summary>
        /// <param name="logger">
        /// The logger.
        /// </param>
        public ScanFileSystem(ILogger logger)
        {
            this.logger = logger;
            fileSystemInformationList = new List<FileSystemInformation>();
        }

        /// <summary>
        /// Method to populate files from the file system, based on the extension specified in the configuration file.
        /// </summary>
        public async void PopulateFiles()
        {
            foreach (var hardDiskDrive in HardDiskDrivesInfo.HardDiskDrives)
            {
                var directories =
                    new DirectoryInfo(hardDiskDrive.Name).GetDirectories()
                        .Select(directory => directory.FullName)
                        .Except(ConfigurationKeys.DirectoriesToExclude) // Filter the directories to be excluded
                        .Select(directory => new DirectoryInfo(directory)).ToList();
                await Task.Run(
                    () =>
                        {
                            foreach (var directory in directories)
                            {
                                try
                                {
                                    var files =
                                        ConfigurationKeys.FileTypesToMonitor.AsParallel()
                                            .SelectMany(
                                                searchPattern =>
                                                directory.EnumerateFiles(searchPattern, SearchOption.AllDirectories));
                                    foreach (var file in files)
                                    {
                                        var fileSystemInfo = new FileSystemInformation
                                                                 {
                                                                     FileName = file.Name,
                                                                     FilePath = file.FullName,
                                                                     Extension = file.Extension
                                                                 };
                                        fileSystemInfo.PopulateMetaData();
                                        fileSystemInformationList.Add(fileSystemInfo);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    this.logger.LogException("Unable to get files for the directory: " + directory, ex);
                                    continue;
                                }
                            }
                        });
            }
        }
    }
}
