using Ardalis.Result;
using PokedexApi.Domain.Models;
using PokedexApi.Infrastructure.Response;
using System.Text.RegularExpressions;

namespace PokedexApiIntegrationTest.Helpers
{
    public static class ResultFactory
    {
        private static string placeholderDescription = "No english description available";
        private static string placeholderHabitat = "This pokemon has no known habitat";
        public static async Task<PokemonInformation> CreatePokemonInformationFromPokemonApiAsync()
        {
            HttpClient client = new HttpClient();
            var random = new Random();
            var response = await client.GetAsync($"https://pokeapi.co/api/v2/pokemon-species/{random.Next(1,1026)}/");
            var pokemonResponse = await response.Content.ReadFromJsonAsync<PokemonResponse>();
            if(pokemonResponse is null)
            {
                throw new NullReferenceException("Could not generate test data");
            }
            return new PokemonInformation
            {
                Name = pokemonResponse.Name,
                Description = GetPokemonEnglishDescription(pokemonResponse.FlavorTextEntries),
                Habitat = GetPokemonHabitat(pokemonResponse.Habitat!),
                IsLegendary = pokemonResponse.IsLegendary,
            };

        }
        private static string GetPokemonHabitat(Habitat habitat)
        {
            if (habitat == null || habitat.Name == null)
            {
                return placeholderHabitat;
            }
            return habitat.Name;
        }

        private static string GetPokemonEnglishDescription(IEnumerable<FlavorTextEntries> entries)
        {
            var description =
                entries
                .FirstOrDefault(e => e.Language.Name.Equals("en"))?.FlavorText ??
                placeholderDescription;

            return Regex.Replace(description, @"\s+", " ", RegexOptions.Compiled);
        }
    }
}
