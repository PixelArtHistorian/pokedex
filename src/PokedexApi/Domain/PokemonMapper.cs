using PokedexApi.Domain.Interfaces;
using PokedexApi.Domain.Models;
using PokedexApi.Infrastructure.Response;
using System.Text.RegularExpressions;

namespace PokedexApi.Domain
{
    public class PokemonMapper : IMapper<PokemonResponse, PokemonInformation>
    {
        private readonly string placeholderDescription = "No english description available";
        private readonly string placeholderHabitat = "This pokemon has no known habitat";
        public PokemonInformation Map(PokemonResponse source)
        {
            return new PokemonInformation
            {
                Name = source.Name,
                Description = GetPokemonEnglishDescription(source.FlavorTextEntries),
                Habitat = GetPokemonHabitat(source.Habitat!),
                IsLegendary = source.IsLegendary,
            };
        }

        private string GetPokemonHabitat(Habitat habitat)
        {
            if (habitat == null || habitat.Name == null) 
            { 
                return placeholderHabitat;
            }
            return habitat.Name;
        }

        private string GetPokemonEnglishDescription(IEnumerable<FlavorTextEntries> entries)
        {
            var description = 
                entries
                .FirstOrDefault(e => e.Language.Name.Equals("en"))?.FlavorText ??
                placeholderDescription;

            return Regex.Replace(description, @"\s+", " ", RegexOptions.Compiled);
        }
    }

}
