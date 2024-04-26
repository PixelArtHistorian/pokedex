namespace PokedexApi.Infrastructure.Client
{
    public class PokemonSpeciesClient : IPokemonSpeciesClient
    {
        private HttpClient _client;
        private readonly string _baseUri = "https://pokeapi.co/api/v2/";
        public PokemonSpeciesClient(HttpClient httpClient)
        {
            _client = httpClient;
            _client.BaseAddress = new Uri(_baseUri);
        }
        public async Task<HttpResponseMessage> GetPokemonSpeciesInformationAsync(string pokemonName)
        {
            return await _client.GetAsync($"pokemon-species/{pokemonName}");
        }
    }
}
