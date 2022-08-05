
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;

namespace Language_Translate
{
    public class LanguageTranslator
    {
        public static ILoggerFactory LoggerFactory;
        public static IConfigurationRoot Configuration;
        public static void Main(string[] args)
        {
            LanguageTranslator languageTranslator = new LanguageTranslator();
            string textToTranslate = string.Empty;
            while(textToTranslate.ToLower() != "exit")
            {
                textToTranslate= Console.ReadLine();
                languageTranslator.TranslateTextRequest("en", "to", textToTranslate);
                
            }//while

        }
        public LanguageTranslator()
        {
            var builder = new ConfigurationBuilder()
                                            .SetBasePath(Directory.GetCurrentDirectory())
                                            .AddJsonFile("appsettings.json");

            Configuration = builder.Build();

        }
        public async Task<string> TranslateTextRequest(string from, string to,  string inputText)
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
                
                return translationResults == null?null: translationResults[0].Translations[0].Text;
                
            }
        }
    }
}
