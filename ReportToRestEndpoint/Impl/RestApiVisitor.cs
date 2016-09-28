namespace ReportToRestEndpoint.Impl
{
    using System;
    using System.IO;
    using System.Net;

    using Entities;

    using Logger.Contracts;

    using ReportToRestEndpoint.Contracts;

    /// <summary>
    /// The rest api visitor.
    /// </summary>
    public class RestApiVisitor : IVisitor
    {
        private string databaseUrl = @"http://sams.northeurope.cloudapp.azure.com/api/v2/samnet/_table/{0}";

        private string authenticationUrl = @"http://sams.northeurope.cloudapp.azure.com/api/v2/user/session";

        private ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="RestApiVisitor"/> class.
        /// </summary>
        /// <param name="logger">
        /// The logger.
        /// </param>
        public RestApiVisitor(ILogger logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// The visit.
        /// </summary>
        /// <param name="tableName">
        /// The table name.
        /// </param>
        /// <param name="data">
        /// The data.
        /// </param>
        public void Visit(string tableName, string data)
        {
            Authenticate();
            var url = string.Format(this.databaseUrl, tableName);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "text/plain;charset=utf-8";
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            var bytes = encoding.GetBytes(data);
            request.ContentLength = bytes.Length;
            using (Stream requestStream = request.GetRequestStream())
            {
                // Send the data.
                requestStream.Write(bytes, 0, bytes.Length);
            }

            try
            {
                WebResponse webResponse = request.GetResponse();
                Stream webStream = webResponse.GetResponseStream();
                StreamReader responseReader = new StreamReader(webStream);
                string response = responseReader.ReadToEnd();
                this.logger.LogInfo("Call made successfully for table " + tableName);
                responseReader.Close();
            }
            catch (Exception ex)
            {
                this.logger.LogException("An exception occured while making the REST API call for the table "+tableName, ex);
            }
        }

        /// <summary>
        /// The authenticate.
        /// </summary>
        private void Authenticate()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(this.authenticationUrl);
            request.Method = "POST";
            request.ContentType = "text/plain;charset=utf-8";
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            var data = "{ \"email\": \"\"" + ConfigurationKeys.DbUserName + "\"\", \"password\": \"\"" + ConfigurationKeys.DbPassword + "\"\", \"remember_me\": \"false\" }";
            var bytes = encoding.GetBytes(data);
            request.ContentLength = bytes.Length;
            using (Stream requestStream = request.GetRequestStream())
            {
                // Send the data.
                requestStream.Write(bytes, 0, bytes.Length);
            }

            try
            {
                WebResponse webResponse = request.GetResponse();
                Stream webStream = webResponse.GetResponseStream();
                StreamReader responseReader = new StreamReader(webStream);
                string response = responseReader.ReadToEnd();
                this.logger.LogInfo("Authenticated successfully for user " + ConfigurationKeys.DbUserName);
                responseReader.Close();
            }
            catch (Exception ex)
            {
                this.logger.LogException("An exception occured while making the REST API call for the user " + ConfigurationKeys.DbUserName, ex);
            }
        }
    }
}
