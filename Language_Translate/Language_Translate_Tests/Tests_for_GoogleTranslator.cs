using Language_Translate;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Language_Translate_Tests
{
    public class Tests_for_GoogleTranslator
    {
        private ILogger<GoogleLanguageTranslator> Logger;
        private IConfigurationRoot Configuration;
        [SetUp]
        public void Setup()
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();
            using var loggerFactory = Microsoft.Extensions.Logging.LoggerFactory.Create(builder =>
            {
                builder
                    .AddFilter("Microsoft", LogLevel.Warning)
                    .AddFilter("System", LogLevel.Warning)
                    .AddFilter("NonHostConsoleApp.Program", LogLevel.Debug)
                    .AddConsole();
            });
            Logger = loggerFactory.CreateLogger<GoogleLanguageTranslator>();
        }

        [Test]
        public void Test_for_sample_case()
        {
            ILanguageTranslator languageTranslator = new GoogleLanguageTranslator(Configuration,Logger);
            string translatedText = languageTranslator.TranslateTextRequest("en", "fr", "test words").Result;
            Assert.IsNotNull(translatedText);
            Assert.IsNotEmpty(translatedText);
            Assert.That(translatedText, Is.EqualTo("mots d'essai"));
            Assert.Pass();
        }
        [Test]
        public void Test_for_first_sentence_given()
        {
            ILanguageTranslator languageTranslator = new GoogleLanguageTranslator(Configuration,Logger);
            string translatedText = languageTranslator.TranslateTextRequest("en", "fr", @"Farnborough International Airshow, biennial global aerospace, defence and space trade event 
                                                                                            which showcases the latest commercial and military aircraft.Manufacturers such as Airbus and Boeing
                                                                                            are expected to display their products and announce new orders* 2020 event was held virtually after
                                                                                            the physical show was cancelled due to the coronavirus(COVID-19) pandemic").Result;
            Assert.IsNotNull(translatedText);
            Assert.IsNotEmpty(translatedText);
            Assert.That(translatedText, Is.EqualTo("Farnborough International Airshow, �v�nement mondial biennal sur le commerce de l�a�rospatiale, de la d�fense et de l�espace \r\nqui pr�sente les derniers avions commerciaux et militaires. Des constructeurs comme Airbus et Boeing \r\ndevraient exposer leurs produits et annoncer les nouvelles commandes * L��v�nement 2020 a eu lieu virtuellement apr�s \r\nle spectacle physique a �t� annul� en raison de la pand�mie de coronavirus (COVID-19)"));
            Assert.Pass();
        }
        [Test]
        public void Test_for_second_sentence_given()
        {
            ILanguageTranslator languageTranslator = new GoogleLanguageTranslator(Configuration,Logger);
            string translatedText = languageTranslator.TranslateTextRequest("en", "fr", @"Labour market statistics: integrated national release, including the latest data for employment, 
                                                                                        economic activity, economic inactivity, unemployment, claimant count, average earnings, productivity, 
                                                                                        unit wage costs, vacancies & labour disputes").Result;
            Assert.IsNotNull(translatedText);
            Assert.IsNotEmpty(translatedText);
            Assert.That(translatedText, Is.EqualTo(@"Statistiques du march� du travail : diffusion nationale int�gr�e, y compris les donn�es les plus r�centes sur l�emploi, l�activit� �conomique, l�inactivit� �conomique, le ch�mage, le nombre de prestataires, les gains moyens, la productivit�, les co�ts salariaux unitaires, les postes vacants et les conflits du travail"));
            Assert.Pass();
        }
        [Test]
        public void Test_for_third_sentence_given()
        {
            ILanguageTranslator languageTranslator = new GoogleLanguageTranslator(Configuration,Logger);
            string translatedText = languageTranslator.TranslateTextRequest("en", "fr", @"City of London Corporation's Financial and Professional Services dinner. Chancellor Rishi Sunak 
                                                                                            and Bank of England Governor Andrew Bailey make their annual Mansion House speeches at the event 
                                                                                            hosted by the Lord Mayor of the City of London Vincent Keaveny").Result;
            Assert.IsNotNull(translatedText);
            Assert.IsNotEmpty(translatedText);
            Assert.That(translatedText, Is.EqualTo("D�ner sur les services financiers et professionnels de la City of London Corporation. Le chancelier Rishi Sunak et le gouverneur de la Banque d�Angleterre Andrew Bailey prononcent leurs discours annuels � Mansion House lors de l��v�nement organis� par le maire de la ville de Londres, Vincent Keaveny."));
            Assert.Pass();
        }       
    }
}