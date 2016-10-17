namespace Helpers
{
    using System.Collections.Generic;
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

        /// <summary>
        /// The get difference method for difference between two lists.
        /// </summary>
        /// <param name="firstList">
        /// The first list.
        /// </param>
        /// <param name="secondList">
        /// The second list.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public static List<T> GetDifference<T>(this List<T> firstList, List<T> secondList)
        {
            return firstList.Where(x => secondList.Any(y => x.Equals(y))).ToList();
        }

        /// <summary>
        /// The get boolean value.
        /// </summary>
        /// <param name="input">
        /// The input.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string GetBooleanValue(object input)
        {
            if (input == null)
            {
                return null;
            }
            else
            {
                return (bool)input ? "TRUE" : "FALSE";
            }
        }
    }
}
