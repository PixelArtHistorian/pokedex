using System.Net;
using System.Text.Json;

namespace PokedexApiTest.Helpers
{
    public static class HttpResponseFactory
    {
        public static HttpResponseMessage CreateMockResponse(HttpStatusCode httpStatusCode, object payload)
        {
            var pokemonResponse = PokemonResponseFactory.CreatePokemonResponse();
            var httpResponseMessage = new HttpResponseMessage(httpStatusCode)
            {
                Content = new StringContent(JsonSerializer.Serialize(payload)),
            };
            return httpResponseMessage;
        }
    }
}
