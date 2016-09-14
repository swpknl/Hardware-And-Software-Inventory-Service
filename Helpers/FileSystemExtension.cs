namespace Helpers
{
    using Entities;

    using Microsoft.WindowsAPICodePack.Shell;
    using Microsoft.WindowsAPICodePack.Shell.PropertySystem;

    /// <summary>
    /// The file system extension.
    /// </summary>
    public static class FileSystemExtension
    {
        /// <summary>
        /// Gets the meta data for the file.
        /// </summary>
        /// <param name="fileSystemInfo">
        /// The file system info.
        /// </param>
        public static void PopulateMetaData(this FileSystemInformation fileSystemInfo)
        {
            // TODO: Generate unique hash as well as clean the copyright, tm etc symbols
            using (var shellObject = ShellObject.FromParsingName(fileSystemInfo.FilePath))
            {
                var description = shellObject.Properties.GetProperty(SystemProperties.System.FileDescription).ValueAsObject;
                fileSystemInfo.Description = description == null ? string.Empty : description.ToString();
                var productName = shellObject.Properties.GetProperty(SystemProperties.System.Software.ProductName).ValueAsObject;
                fileSystemInfo.ProductName = productName == null ? string.Empty : productName.ToString();
                var fileVersion = shellObject.Properties.GetProperty(SystemProperties.System.FileVersion).ValueAsObject;
                fileSystemInfo.FileVersion = fileVersion == null ? string.Empty : fileVersion.ToString();
                var copyright = shellObject.Properties.GetProperty(SystemProperties.System.Copyright).ValueAsObject;
                fileSystemInfo.Manufacturer = copyright == null ? string.Empty : copyright.ToString();
                var tradeMark = shellObject.Properties.GetProperty(SystemProperties.System.Trademarks).ValueAsObject;
                fileSystemInfo.TradeMark = tradeMark == null ? string.Empty : tradeMark.ToString();
                var fileSize = shellObject.Properties.GetProperty(SystemProperties.System.TotalFileSize).ValueAsObject;
                fileSystemInfo.FileSize = fileSize == null ? string.Empty : fileSize.ToString();
            }
        }
    }
}
