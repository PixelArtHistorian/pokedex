using PokedexApi.Domain.Interfaces;
using PokedexApi.Domain.Models;
using PokedexApi.Infrastructure;
using System.Text;
using System.Text.Json;

namespace PokedexApi.Domain
{
    public class PokemonInformationService : IPokemonInformationService
    {
        private IHttpClientFactory _httpClientFactory;
        private IMapper<PokemonResponse, PokemonInformation> _mapper;

        public PokemonInformationService(IHttpClientFactory httpClientFactory, IMapper<PokemonResponse, PokemonInformation> mapper)
        {
            _httpClientFactory = httpClientFactory;
            _mapper = mapper;
        }

        public async Task<PokemonInformation> GetPokemonInformationAsync(string pokemonName)
        {
            var client = _httpClientFactory.CreateClient();
            StringBuilder builder = new StringBuilder();
            var url = builder.
                Append("https://pokeapi.co/api/v2/")
                .Append("pokemon-species/")
                .Append(pokemonName)
                .ToString();

            var response = await client.GetAsync(url);

            PokemonResponse pokemonResponse = JsonSerializer.Deserialize<PokemonResponse>(await response.Content.ReadAsStringAsync());

            return _mapper.Map(pokemonResponse);
        }
    }
}
