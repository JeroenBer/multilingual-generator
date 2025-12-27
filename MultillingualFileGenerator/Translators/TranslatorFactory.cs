using System.IO.Abstractions;
using System.Text.Json;

namespace MultillingualFileGenerator.Translators;
public class TranslatorFactory
{
    private readonly IFileSystem _fileSystem;

    // Default constructor for production use
    public TranslatorFactory() : this(new FileSystem()) { }

    // Constructor for dependency injection (e.g., testing)
    public TranslatorFactory(IFileSystem fileSystem)
    {
        _fileSystem = fileSystem;
    }

    public ITranslator GetTranslator(string sourceLanguage, string targetLanguage)
    {
        var openAIKey = GetOpenAIKey();
        var azureTranslatorKey = Environment.GetEnvironmentVariable("AzureTranslatorKey");
        var azureRegion = Environment.GetEnvironmentVariable("AzureRegion");

        if (!string.IsNullOrEmpty(openAIKey))
            return new ChatGptTranslator(targetLanguage, openAIKey);

        if (!string.IsNullOrEmpty(azureTranslatorKey) && !string.IsNullOrEmpty(azureRegion))
            return new AzureTranslator(sourceLanguage, targetLanguage, azureTranslatorKey, azureRegion);

        return null;
    }

    private string GetOpenAIKey()
    {
        const string configFileName = "application.json";
        var configFilePath = _fileSystem.Path.Combine(_fileSystem.Directory.GetCurrentDirectory(), configFileName);

        if (_fileSystem.File.Exists(configFilePath))
        {
            try
            {
                var configContent = _fileSystem.File.ReadAllText(configFilePath);
                var config = JsonSerializer.Deserialize<Dictionary<string, string>>(configContent);

                if (config != null && config.TryGetValue("OpenAIKey", out var openAIKey))
                {
                    return openAIKey;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading {configFileName}: {ex.Message}");
            }
        }

        return Environment.GetEnvironmentVariable("OpenAIKey");
    }

}
