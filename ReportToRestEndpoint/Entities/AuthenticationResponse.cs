namespace ReportToRestEndpoint.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    internal class AuthenticationResponse
    {
        public string session_token { get; set; }
        public string session_id { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public string is_sys_admin { get; set; }
        public string last_login_date { get; set; }
        public string host { get; set; }
    }
}
