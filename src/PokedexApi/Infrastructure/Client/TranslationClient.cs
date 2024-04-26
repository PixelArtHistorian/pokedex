namespace PokedexApi.Infrastructure.Client
{
    public class TranslationClient : ITranslationClient
    {
        private HttpClient _client;
        private readonly string _baseUri = "https://api.funtranslations.com/translate/";
        public TranslationClient(HttpClient httpClient)
        {
            _client = httpClient;
            _client.BaseAddress = new Uri(_baseUri);
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
