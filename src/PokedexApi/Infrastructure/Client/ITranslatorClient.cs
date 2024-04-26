namespace PokedexApi.Infrastructure.Client
{
    public interface ITranslatorClient
    {
        Task<HttpResponseMessage> TranslatetextAsync(string pokemonName);
    }
}
