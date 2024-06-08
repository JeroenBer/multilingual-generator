using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace MultillingualFileGenerator.Config;

public class ConfigWriter : IConfigWriter
{
    JsonSerializerOptions _serializerOptions = new JsonSerializerOptions()
    {
        WriteIndented = true,
        Converters =
    {
        new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
    }
    };

    public void WriteConfig(string filePath, MultilingualConfig config)
    {
        using (var fs = new FileStream(filePath, FileMode.CreateNew, FileAccess.Write))
        {
            JsonSerializer.Serialize(fs, config, _serializerOptions);
        }
    }
}
