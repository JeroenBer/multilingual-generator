using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenAI;
using OpenAI.Chat;

namespace MultillingualFileGenerator.Translators;
internal class ChatGptTranslator : ITranslator
{
    private readonly OpenAIClient _openAIClient;
    private readonly string _targetLanguage;

    public ChatGptTranslator(string targetLanguage, string openApiKey)
    {
        _openAIClient = new OpenAIClient(openApiKey);
        _targetLanguage = targetLanguage;
    }

    public async Task<string> Translate(string text)
    {
        if (string.IsNullOrEmpty(text))
            return text;

        // Get Models
        //var modelClient = _openAIClient.GetOpenAIModelClient();
        //var models = modelClient.GetModels(CancellationToken.None);

        //foreach(var model in models.Value)
        //{
        //    Console.WriteLine(model.Id);
        //}

        var chatModel = "gpt-4.1-mini";
        // var chatModel = "gpt-3.5-turbo";

        var chatClient = _openAIClient.GetChatClient(chatModel);
       
        var chatMessages = new ChatMessage[]
        {
            new SystemChatMessage($"You are a translation assistant. You need to translate text for a mobile app. Translate the text into the language and locale specified by '{_targetLanguage}'. Reply only with the translation. Please use polite terms and be brief. {0} {1} etc are replacement fields that should be kept in the output. Also newlines and \\n\\r should be kept in the anser"),
            new UserChatMessage(text)
        };

        try
        {
            var response = await chatClient.CompleteChatAsync(chatMessages);

            var translation = response.Value.Content.FirstOrDefault()?.Text;

            return translation;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERROR translating {_targetLanguage}: {text} - {ex.Message}", ex);
            return null;
        }
    }
}
