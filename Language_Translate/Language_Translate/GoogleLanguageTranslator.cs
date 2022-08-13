using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;
using Microsoft.Extensions.Hosting;
using Google.Cloud.Translation.V2;

namespace Language_Translate
{
    public class GoogleLanguageTranslator:ILanguageTranslator
    {
        private readonly ILogger Logger;
        private readonly IConfigurationRoot Configuration;
        private readonly TranslationClient client;
        public GoogleLanguageTranslator(IConfigurationRoot _configuration, ILogger<GoogleLanguageTranslator> _logger)
        {
            Configuration = _configuration;
            Logger = _logger;
            client = TranslationClient.Create();
        }
        public async Task<string> TranslateTextRequest(string from, string to, string inputText)
        {
            throw new Exception("Not proceeding further because it costs $45 per hour");
            var result = client.TranslateText(inputText, to, from);
            return result.TranslatedText;            
        }

    }
}
