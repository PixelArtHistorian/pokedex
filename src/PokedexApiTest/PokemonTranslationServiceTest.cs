using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using PokedexApi.Configuration;
using PokedexApi.Domain;
using PokedexApi.Domain.Interfaces;
using PokedexApi.Infrastructure.Client;

namespace PokedexApiTest
{
    [Trait("Category", "PokemonTranslationService")]
    public class PokemonTranslationServiceTest
    {
        Mock<IPokemonInformationService> MockInfoService { get; set; }
        Mock<ITranslationClient> MockTranslationClient { get; set; }
        IOptions<TranslationServiceOptions> TranslationServiceOptions { get; set; }
        Mock<ILogger<PokemonTranslationService>> MockLogger {get;set;}
        PokemonTranslationService Sut { get; set;}
        public PokemonTranslationServiceTest()
        {
            MockInfoService = new ();
            MockTranslationClient = new();
            var options = new TranslationServiceOptions
            {
                YodishEndpoint = "yoda",
                ShakespeareanEndpoint = "shakespeare"
            };
            TranslationServiceOptions = Options.Create<TranslationServiceOptions>(options);

            MockLogger = new ();
            Sut = new(
                MockInfoService.Object, 
                MockTranslationClient.Object, 
                TranslationServiceOptions, 
                MockLogger.Object);
        }

        //[Fact]
        //public async Task GetPokemonInformationTranslationAsync_GetsATranslation()
        //{
        //    MockInfoService.
        //}
    }
}
