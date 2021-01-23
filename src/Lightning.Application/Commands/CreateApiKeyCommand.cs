using System.ComponentModel.DataAnnotations;

namespace Lightning.Application.Commands
{
    /// <summary>
    /// A command for creating new api key.
    /// </summary>
    public class CreateApiKeyCommand
    {
        /// <summary>
        /// client id
        /// </summary>
        /// <value>string</value>
        [Required]
        public string Client { get; set; }
    }
}