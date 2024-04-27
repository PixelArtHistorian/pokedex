using AutoFixture;
using PokedexApi.Infrastructure.Response;

namespace PokedexApiTest.Helpers
{
    public static class TranslationResponseFactory
    {
        public static TranslationResponse CreateTranslationResponse()
        {
            var fixture = new Fixture();
            return fixture.Create<TranslationResponse>();
        }
    }
}
