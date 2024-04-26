using static System.Net.Mime.MediaTypeNames;

namespace PokedexApi.Infrastructure.Client
{
    public class YodaTranslationClient : ITranslationClient
    {
        private HttpClient _client;
        private readonly string _baseUri = "https://api.funtranslations.com/translate/yoda.json";
        public YodaTranslationClient(HttpClient httpClient)
        {
            _client = httpClient;
            _client.BaseAddress = new Uri(_baseUri);
        }
        public Task<HttpResponseMessage> TranslateTextAsync(string textToTranslate)
        {
            var requestContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("text", textToTranslate)
            });
            return _client.PostAsync(_baseUri, requestContent);
        }
    }
}
