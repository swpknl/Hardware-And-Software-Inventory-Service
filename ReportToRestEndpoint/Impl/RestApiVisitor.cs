namespace ReportToRestEndpoint.Impl
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Net;

    using Entities;

    using global::Entities;

    using Logger.Contracts;

    using Newtonsoft.Json;

    using ReportToRestEndpoint.Contracts;

    /// <summary>
    /// The rest api visitor.
    /// </summary>
    public class RestApiVisitor : IVisitor
    {
        private readonly string databaseUrl = @"http://sams.northeurope.cloudapp.azure.com/api/v2/samnet/_table/{0}?api_key={1}&session_token={2}";

        private readonly string authenticationUrlForAdmin = @"http://sams.northeurope.cloudapp.azure.com/api/v2/system/admin/session";

        private readonly string authenticationUrlForUser = @"http://sams.northeurope.cloudapp.azure.com/api/v2/user/session";

        private readonly string apiKey = @"8b5d83d676d95e8da0a14c08cda66b37b89f25faa1deea462bbd53dc839fc618";

        private AuthenticationResponse authenticationReponse;

        private readonly ILogger logger;

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
        public void Visit(string tableName, string data, out int id)
        {
            this.Authenticate();
            var url = string.Format(this.databaseUrl, tableName, apiKey, this.authenticationReponse.session_token);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json";
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
                id = JsonConvert.DeserializeObject<Resources>(response).resource.First().id;
                this.logger.LogInfo("Call made successfully for table " + tableName);
                responseReader.Close();
            }
            catch (Exception ex)
            {
                id = 0;
                this.logger.LogException(
                    "An exception occured while making the REST API call for the table " + tableName,
                    ex);
            }
        }

        /// <summary>
        /// The authenticate.
        /// </summary>
        private void Authenticate()
        {
            if (this.Login(this.authenticationUrlForAdmin) == false)
            {
                this.Login(this.authenticationUrlForUser);
            }
        }

        /// <summary>
        /// The request server.
        /// </summary>
        /// <param name="url">
        /// The url to make the request to.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private bool Login(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json";
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            var data = "{ \"email\": \"" + ConfigurationKeys.DbUserName + "\", \"password\": \""
                       + ConfigurationKeys.DbPassword + "\", \"remember_me\": \"false\" }";
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
                this.authenticationReponse = JsonConvert.DeserializeObject<AuthenticationResponse>(response);
                this.logger.LogInfo("Authenticated successfully for user " + ConfigurationKeys.DbUserName);
                responseReader.Close();
                return true;
            }
            catch (Exception ex)
            {
                this.logger.LogException("An exception occurred while authenticating the user.", ex);
                return false;
            }
        }
    }
}
