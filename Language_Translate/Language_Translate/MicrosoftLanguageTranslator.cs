using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;
using Microsoft.Extensions.Hosting;

namespace Language_Translate
{
    public class MicrosoftLanguageTranslator:ILanguageTranslator
    {
        private readonly ILogger Logger;
        private readonly IConfigurationRoot Configuration;
        public MicrosoftLanguageTranslator(IConfigurationRoot _configuration, ILogger<MicrosoftLanguageTranslator> _logger)
        {
            Configuration=_configuration;
            Logger = _logger;
        }
        //public MicrosoftLanguageTranslator()
        //{
        //    Configuration = new ConfigurationBuilder()
        //        .SetBasePath(Directory.GetCurrentDirectory())
        //        .AddJsonFile("appsettings.json")
        //        .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
        //        .AddEnvironmentVariables()
        //        .Build();
        //    using var loggerFactory = LoggerFactory.Create(builder =>
        //    {
        //        builder
        //            .AddFilter("Microsoft", LogLevel.Warning)
        //            .AddFilter("System", LogLevel.Warning)
        //            .AddFilter("NonHostConsoleApp.Program", LogLevel.Debug)
        //            .AddConsole();
        //    });
        //    Logger = loggerFactory.CreateLogger<MicrosoftLanguageTranslator>();

        //}
        public async Task<string> TranslateTextRequest(string from, string to, string inputText)
        {
            string resourceKey = Configuration["TranslatorKey"];
            string endpoint = Configuration["TextTranslatorAPI"];
            string region = Configuration["region"];


            object[] body = new object[] { new { Text = inputText } };
            var requestBody = JsonConvert.SerializeObject(body);

            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage())
            {
                // Build the request.
                request.Method = HttpMethod.Post;
                request.RequestUri = new Uri(string.Format(endpoint, from, to));
                request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                request.Headers.Add("Ocp-Apim-Subscription-Key", resourceKey);
                request.Headers.Add("Ocp-Apim-Subscription-Region", region);

                // Send the request and get response.
                HttpResponseMessage response = await client.SendAsync(request).ConfigureAwait(false);
                // Read response as a string.
                string result = await response.Content.ReadAsStringAsync();
                TranslationResult[] translationResults = JsonConvert.DeserializeObject<TranslationResult[]>(result);

                return translationResults == null ? null : translationResults[0].Translations[0].Text;

            }
        }
    }
}
