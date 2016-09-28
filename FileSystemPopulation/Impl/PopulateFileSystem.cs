namespace FileSystemPopulation.Impl
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Constants;

    using Contracts;

    using Entities;

    using Helpers;

    using Logger.Contracts;

    using ReportToRestEndpoint.Contracts;

    /// <summary>
    /// Class to scan the file system.
    /// </summary>
    public class PopulateFileSystem : IPopulateFileSystem
    {
        private readonly ILogger logger;

        private readonly List<FileSystemInformation> fileSystemInformationList;

        /// <summary>
        /// Initializes a new instance of the <see cref="PopulateFileSystem"/> class.
        /// </summary>
        /// <param name="logger">
        /// The logger.
        /// </param>
        public PopulateFileSystem(ILogger logger)
        {
            this.logger = logger;
            this.fileSystemInformationList = new List<FileSystemInformation>();
        }

        /// <summary>
        /// Method to populate files from the file system, based on the extension specified in the configuration file.
        /// </summary>
        public void PopulateFiles()
        {
            foreach (var hardDiskDrive in HardDiskDrivesInfo.HardDiskDrives)
            {
                var directories =
                    new DirectoryInfo(hardDiskDrive.Name).GetDirectories()
                        .Select(directory => directory.FullName)
                        .Except(ConfigurationKeys.DirectoriesToExclude) // Filter the directories to be excluded
                        .Select(directory => new DirectoryInfo(directory)).ToList();
                foreach (var directory in directories)
                {
                    try
                    {
                        var files =
                            ConfigurationKeys.FileTypesToMonitor.SelectMany(
                                searchPattern => directory.EnumerateFiles(searchPattern, SearchOption.AllDirectories));
                        foreach (var file in files)
                        {
                            if (ConfigurationKeys.DirectoriesToExclude.Any(
                                    dir => file.Directory.ToString().Contains(dir)))
                            {
                                continue;
                            }
                            var fileSystemInfo = new FileSystemInformation
                                                     {
                                                         FileName = file.Name,
                                                         FilePath = file.FullName,
                                                         Extension = file.Extension
                                                     };
                            fileSystemInfo.PopulateMetaData();
                            this.fileSystemInformationList.Add(fileSystemInfo);
                        }
                    }
                    catch (UnauthorizedAccessException ex)
                    {
                        this.logger.LogException("Unable to get files for the directory: " + directory, ex);
                    }
                    catch (Exception ex)
                    {
                        this.logger.LogException("An exception occurred", ex);
                    }
                }
            }
        }

        /// <summary>
        /// The report files.
        /// </summary>
        /// <param name="visitor">
        /// The visitor.
        /// </param>
        public void ReportFilesInfo(IVisitor visitor)
        {
            
        }
    }
}
