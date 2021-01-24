using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace Lightning.Core.Configs
{
    public class YamlConfigurationProvider : FileConfigurationProvider
    {
        public YamlConfigurationProvider(YamlConfigurationSource source) : base(source) { }

        public override void Load(Stream stream)
        {
            var parser = new YamlConfigurationFileParser();
            try
            {
                Data = parser.Parse(stream);
            }
            catch(YamlDotNet.Core.YamlException ex)
            {
                throw new FormatException("Yaml parse error", ex);
            }
        }
    }
}