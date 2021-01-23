

namespace Lightning.Domain.Models
{
    public class ApiKey 
    {
        /// <summary>
        /// client id
        /// </summary>
        /// <value></value>
        public string Client { get; set; }

        /// <summary>
        /// secret key, when one client invokes api, he/she should contain the secret key in headers or query parameters.
        /// </summary>
        /// <value></value>
        public string SecretKey { get; set; }

        /// <summary>
        /// the secret key may access these api urls.
        /// </summary>
        /// <value></value>
        public string[] ApiUrl { get; set; }
    }
}