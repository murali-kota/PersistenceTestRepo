using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Language_Translate
{
    public interface ILanguageTranslator
    {
        
        public Task<string> TranslateTextRequest(string from, string to, string inputText);
       
    }

}