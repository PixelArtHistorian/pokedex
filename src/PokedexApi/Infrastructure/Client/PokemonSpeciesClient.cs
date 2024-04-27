using Microsoft.Extensions.Options;
using PokedexApi.Configuration;

namespace PokedexApi.Infrastructure.Client
{
    public class PokemonSpeciesClient : IPokemonSpeciesClient
    {
        private HttpClient _client;
        private PokemonSpeciesClientOptions _options;
        public PokemonSpeciesClient(HttpClient httpClient, IOptions<PokemonSpeciesClientOptions> options)
        {
            _client = httpClient;
            _options = options.Value;
            _client.BaseAddress = new Uri(_options.BaseUri);
        }
        public async Task<HttpResponseMessage> GetPokemonSpeciesInformationAsync(string pokemonName)
        {
            return await _client.GetAsync($"{_options.PokemonSpeciesEndpoint}/{pokemonName}");
        }
    }
}
