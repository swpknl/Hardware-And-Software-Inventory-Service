namespace Entities
{
    using System;

    /// <summary>
    /// The file system info.
    /// </summary>
    public class FileSystemInformation
    {
        public string FilePath { get; set; }

        public string FileName { get; set; }

        public string Md5 { get; set; }

        public string Description { get; set; }

        public string ProductName { get; set; }

        public string FileVersion { get; set; }

        public string Manufacturer { get; set; }

        public string TradeMark { get; set; }

        public string Extension { get; set; }

        public string FileSize { get; set; }

        /// <summary>
        /// The equals.
        /// </summary>
        /// <param name="obj">
        /// The obj.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((FileSystemInformation)obj);
        }

        /// <summary>
        /// The get hash code.
        /// </summary>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (this.FilePath != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(this.FilePath) : 0);
                hashCode = (hashCode * 397) ^ (this.FileName != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(this.FileName) : 0);
                hashCode = (hashCode * 397) ^ (this.Md5 != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(this.Md5) : 0);
                hashCode = (hashCode * 397) ^ (this.Description != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(this.Description) : 0);
                hashCode = (hashCode * 397) ^ (this.ProductName != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(this.ProductName) : 0);
                hashCode = (hashCode * 397) ^ (this.FileVersion != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(this.FileVersion) : 0);
                hashCode = (hashCode * 397) ^ (this.Manufacturer != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(this.Manufacturer) : 0);
                hashCode = (hashCode * 397) ^ (this.TradeMark != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(this.TradeMark) : 0);
                hashCode = (hashCode * 397) ^ (this.Extension != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(this.Extension) : 0);
                hashCode = (hashCode * 397) ^ (this.FileSize != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(this.FileSize) : 0);
                return hashCode;
            }
        }

        /// <summary>
        /// The equals.
        /// </summary>
        /// <param name="other">
        /// The other.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        protected bool Equals(FileSystemInformation other)
        {
            return string.Equals(this.FilePath, other.FilePath, StringComparison.OrdinalIgnoreCase) && string.Equals(this.FileName, other.FileName, StringComparison.OrdinalIgnoreCase) && string.Equals(this.Md5, other.Md5, StringComparison.OrdinalIgnoreCase) && string.Equals(this.Description, other.Description, StringComparison.OrdinalIgnoreCase) && string.Equals(this.ProductName, other.ProductName, StringComparison.OrdinalIgnoreCase) && string.Equals(this.FileVersion, other.FileVersion, StringComparison.OrdinalIgnoreCase) && string.Equals(this.Manufacturer, other.Manufacturer, StringComparison.OrdinalIgnoreCase) && string.Equals(this.TradeMark, other.TradeMark, StringComparison.OrdinalIgnoreCase) && string.Equals(this.Extension, other.Extension, StringComparison.OrdinalIgnoreCase) && string.Equals(this.FileSize, other.FileSize, StringComparison.OrdinalIgnoreCase);
        }
    }
}
