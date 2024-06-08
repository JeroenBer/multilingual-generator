using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MultillingualFileGenerator.Config
{
    public class ConfigReader : IConfigReader
    {
        JsonSerializerOptions _serializerOptions = new JsonSerializerOptions()
        {
            WriteIndented = true,
            Converters =
            {
                new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
            }
        };

        public MultilingualConfig ReadConfig(string configFilePath)
        {
            var fileContent = File.ReadAllText(configFilePath);
            return JsonSerializer.Deserialize<MultilingualConfig>(fileContent, _serializerOptions);
        }
    }
}
