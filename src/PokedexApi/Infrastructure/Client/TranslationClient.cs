using Microsoft.Extensions.Options;
using PokedexApi.Configuration;

namespace PokedexApi.Infrastructure.Client
{
    public class TranslationClient : ITranslationClient
    {
        private HttpClient _client;
        public TranslationClient(HttpClient httpClient, IOptions<TranslationClientOptions> options)
        {
            _client = httpClient;
            _client.BaseAddress = new Uri(options.Value.BaseUri);
        }
        public Task<HttpResponseMessage> TranslateTextAsync(string translationEndpoint, string textToTranslate)
        {
            var requestContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("text", textToTranslate)
            });
            return _client.PostAsync($"{translationEndpoint}", requestContent);
        }
    }
}
