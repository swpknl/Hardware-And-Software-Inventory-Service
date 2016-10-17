namespace ReportToRestEndpoint.Contracts
{
    /// <summary>
    /// The Visitor interface.
    /// </summary>
    public interface IVisitor
    {
        /// <summary>
        /// Method to make REST API call.
        /// </summary>
        /// <param name="tableName">
        /// The table Name.
        /// </param>
        /// <param name="data">
        /// The data.
        /// </param>
        void Visit(string tableName, string data, out int id);
    }
}
