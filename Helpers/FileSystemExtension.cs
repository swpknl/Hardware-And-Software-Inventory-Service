namespace Helpers
{
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using System.Text.RegularExpressions;

    using Constants;

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
            using (var shellObject = ShellObject.FromParsingName(fileSystemInfo.FilePath))
            {
                var description = shellObject.Properties.GetProperty(SystemProperties.System.FileDescription).ValueAsObject;
                fileSystemInfo.Description = description == null ? string.Empty : description.ToString().RemoveSpecialCharacters().Trim();
                var productName = shellObject.Properties.GetProperty(SystemProperties.System.Software.ProductName).ValueAsObject;
                fileSystemInfo.ProductName = productName == null ? string.Empty : productName.ToString().RemoveSpecialCharacters().Trim();
                var fileVersion = shellObject.Properties.GetProperty(SystemProperties.System.FileVersion).ValueAsObject;
                fileSystemInfo.FileVersion = fileVersion == null ? string.Empty : fileVersion.ToString().RemoveSpecialCharacters().Trim();
                var copyright = shellObject.Properties.GetProperty(SystemProperties.System.Copyright).ValueAsObject;
                fileSystemInfo.Manufacturer = copyright == null ? string.Empty : copyright.ToString().RemoveSpecialCharacters().Trim();
                var tradeMark = shellObject.Properties.GetProperty(SystemProperties.System.Trademarks).ValueAsObject;
                fileSystemInfo.TradeMark = tradeMark == null ? string.Empty : tradeMark.ToString().RemoveSpecialCharacters().Trim();
                var fileSize = shellObject.Properties.GetProperty(SystemProperties.System.TotalFileSize).ValueAsObject;
                fileSystemInfo.FileSize = fileSize == null ? string.Empty : fileSize.ToString().RemoveSpecialCharacters().Trim();
            }
            fileSystemInfo.Md5 = GenerateFileHash(fileSystemInfo.FileName);
        }

        private static string GenerateFileHash(string input)
        {
            // step 1, calculate MD5 hash from input
            MD5 md5 = MD5.Create();

            var inputBytes = Encoding.ASCII.GetBytes(input);

            var hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }

            return sb.ToString();
        }

        /// <summary>
        /// Removes special characters.
        /// </summary>
        /// <param name="input">
        /// The input string.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string RemoveSpecialCharacters(this string input)
        {
            var filteredOutput = input.Select(
                character =>
                    {
                        if (character.ToString().In(
                            Symbols.ACircumflex, 
                            Symbols.Comma, 
                            Symbols.Copyright,
                            Symbols.DoubleLowQuotationMark, 
                            Symbols.LeftDoubleQuotationMark, 
                            Symbols.QuotationMark,
                            Symbols.Registered, 
                            Symbols.TradeMark))
                        {
                            // Replace all of the above symbols by an empty string
                            return string.Empty;
                        }
                        else
                        {
                            // Else, return the string as is
                            return character.ToString();
                        }

                    });

            // Create a single string from the above IEnumerable<string>
            var filteredTempOutput = string.Join(string.Empty, filteredOutput);

            // Remove the date range from this filtered output
            var outputWithoutDates = Regex.Replace(filteredTempOutput, @"(\b\d{4}-\d{4}\b)|(\b\d{4} - \d{4}\b)", string.Empty);

            // Remove the following: (c), (C), (tm), (TM), Copyright
            var tempOutput = Regex.Replace(outputWithoutDates, @"(\(tm\)|\(TM\)|\(c\)|\(C\)|Copyright)", string.Empty);

            // Remove the double spaces 
            var output = Regex.Replace(tempOutput, @"  ", " ");
            return output;
        }
    }
}
