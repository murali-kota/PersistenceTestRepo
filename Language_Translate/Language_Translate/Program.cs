
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Language_Translate
{
    public static class LanguageTranslator
    {
        public static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();
            using IHost host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) => 
                {
                    services.AddTransient<ILanguageTranslator, MicrosoftLanguageTranslator>();
                    services.AddTransient<ILanguageTranslator, GoogleLanguageTranslator>();
                })
                .ConfigureLogging((_, logging) => {
                    logging.ClearProviders();
                    logging.AddSimpleConsole(options => options.IncludeScopes = true);
                    logging.AddEventLog();
                })
                .Build();            
            string textToTranslate = string.Empty;
            ILanguageTranslator languageTranslator = null;
            if (args.Length > 0 && args[1].ToLower() == "microsoft")
                languageTranslator = ActivatorUtilities.GetServiceOrCreateInstance<MicrosoftLanguageTranslator>(host.Services);
            else if (args.Length > 0 && args[1].ToLower() == "google")
                languageTranslator = ActivatorUtilities.GetServiceOrCreateInstance<GoogleLanguageTranslator>(host.Services);
            else
                languageTranslator = ActivatorUtilities.GetServiceOrCreateInstance<GoogleLanguageTranslator>(host.Services);
            while (textToTranslate.ToLower() != "exit")
            {
                textToTranslate = Console.ReadLine();
                var translatedText = languageTranslator.TranslateTextRequest("en", "fr", textToTranslate).Result;
                Console.WriteLine(translatedText);

            }//while
            host.RunAsync();
        }
        static void BuildConfig(IConfigurationBuilder builder)
        {
            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")??"Production"}.json",optional:true)
                .AddEnvironmentVariables()
                .Build();
        }
       
        
    }
}
