namespace Helpers
{
    using System.Linq;

    /// <summary>
    /// Class for generic extensions.
    /// </summary>
    public static class GenericExtensions
    {
        /// <summary>
        /// Extension method replicating the IN SQL clause.
        /// </summary>
        /// <param name="value">
        /// The value to check.
        /// </param>
        /// <param name="list">
        /// The list in which to check.
        /// </param>
        /// <typeparam name="T">
        /// The type of the value
        /// </typeparam>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool In<T>(this T value, params T[] list)
        {
            return list.Contains(value);
        }
    }
}
