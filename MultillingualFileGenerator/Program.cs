using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MultillingualFileGenerator.Config;
using MultillingualFileGenerator.Sources;
using MultillingualFileGenerator.Targets;
using MultillingualFileGenerator.Translators;
using MultillingualFileGenerator.Xliff;

namespace MultillingualFileGenerator
{
    internal class Program
    {
        static async Task<int> Main(string[] args)
        {
            HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

            builder.Services.AddTransient<Main>();
            builder.Services.AddSingleton<IConfigReader, ConfigReader>();
            builder.Services.AddSingleton<IConfigWriter, ConfigWriter>();
            builder.Services.AddSingleton<SourceReaderFactory>();
            builder.Services.AddSingleton<SourceReaderService>();
            builder.Services.AddSingleton<XliffReader>();
            builder.Services.AddSingleton<XliffWriter>();
            builder.Services.AddSingleton<TargetService>();
            builder.Services.AddSingleton<TargetWriterFactory>();
            builder.Services.AddSingleton<SampleConfig>();
            builder.Services.AddSingleton<TranslatorFactory>();
            builder.Services.AddSingleton<ITranslator, ChatGptTranslator>();

            using IHost host = builder.Build();

            var main = host.Services.GetRequiredService<Main>();

            try
            {
                return await main.Run(args);
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: " + ex.ToString());
                return 1;
            }
        }
    }
}
