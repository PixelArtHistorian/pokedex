namespace PokedexApi.Infrastructure.Client
{
    public interface ITranslationClient
    {
        Task<HttpResponseMessage> TranslateTextAsync(string translationEndpoint, string textToTranslate);
    }
}
