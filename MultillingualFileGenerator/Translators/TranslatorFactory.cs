using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultillingualFileGenerator.Translators;
public class TranslatorFactory
{
    public ITranslator GetTranslator(string targetLanguage)
    {
        var openAIKey = Environment.GetEnvironmentVariable("OpenAIKey");

        if (!string.IsNullOrEmpty(openAIKey))
            return new ChatGptTranslator(targetLanguage, openAIKey);

        return null;
    }
}
