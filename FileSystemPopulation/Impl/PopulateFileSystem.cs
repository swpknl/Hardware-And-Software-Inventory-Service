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

    using Newtonsoft.Json.Linq;

    using ReportToRestEndpoint.Contracts;

    /// <summary>
    /// Class to scan the file system.
    /// </summary>
    public class PopulateFileSystem : IPopulateFileSystem
    {
        private const string SoftwareTableName = "software";

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
            var data = this.ConvertFilesInfoToJson();
            int id;
            visitor.Visit(SoftwareTableName, data, out id);
        }

        /// <summary>
        /// The convert files info to json.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private string ConvertFilesInfoToJson()
        {
            var data =
                new JObject(
                    new JProperty(
                        "resource",
                        new JArray(
                            from fileSystemInfo in this.fileSystemInformationList
                            select
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
                                new JProperty("os_type", 1), // Windows has ID 1
                                new JProperty("is_registry", "FALSE")))));
            return data.ToString();
        }
    }
}
