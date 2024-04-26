namespace PokedexApi.Infrastructure.Client
{
    public interface IPokemonSpeciesClient
    {
        Task<HttpResponseMessage> GetPokemonSpeciesInformationAsync(string pokemonName);
    }
}