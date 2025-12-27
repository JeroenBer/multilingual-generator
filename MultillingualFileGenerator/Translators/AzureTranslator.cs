using System.Text;
using System.Text.Json;

namespace MultillingualFileGenerator.Translators;

public class AzureTranslator: ITranslator
{
    private readonly string _endpoint;
    private readonly string _key;
    private readonly string _region;
    private readonly string _sourceLangauge;
    private readonly string _targetLanguage;
    private readonly HttpClient _http = new();

    public AzureTranslator(string sourceLangague, string targetLanguage, string key, string region, string endpoint = "https://api.cognitive.microsofttranslator.com")
    {
        _key = key;
        _region = region;
        _endpoint = endpoint;
        _sourceLangauge = sourceLangague;
        _targetLanguage = targetLanguage;
    }

    public async Task<string> Translate(string text)
    {
        var route = $"/translate?api-version=3.0";
        if(!string.IsNullOrEmpty(_sourceLangauge))
            route += $"&from={_sourceLangauge}";
        route += $"&to={_targetLanguage}";

        _http.DefaultRequestHeaders.Clear();
        _http.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _key);
        _http.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Region", _region);

        var body = new object[] { new { Text = text } };
        var requestBody = JsonSerializer.Serialize(body);

        using var content = new StringContent(requestBody, Encoding.UTF8, "application/json");
        using var response = await _http.PostAsync(_endpoint + route, content);

        var json = await response.Content.ReadAsStringAsync();

        using var doc = JsonDocument.Parse(json);
        return doc.RootElement[0]
                  .GetProperty("translations")[0]
                  .GetProperty("text")
                  .GetString()!;
    }
}
