using PokedexApi.Domain.Interfaces;
using PokedexApi.Domain.Models;
using PokedexApi.Infrastructure.DTO;
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
                Name = source.name,
                Description = GetPokemonEnglishDescription(source.flavor_text_entries),
                Habitat = GetPokemonHabitat(source.habitat),
                IsLegendary = source.is_legendary,
            };
        }

        private string GetPokemonHabitat(Habitat habitat)
        {
            if (habitat == null) 
            { 
                return placeholderHabitat;
            }
            if(habitat.name == null)
            {
                return placeholderHabitat;
            }
            return habitat.name;
        }

        private string GetPokemonEnglishDescription(Flavor_Text_Entries[] entries)
        {
            var description = 
                entries
                .FirstOrDefault(e => e.language.name.Equals("en"))?.flavor_text ??
                placeholderDescription;

            return Regex.Replace(description, @"\s+", " ", RegexOptions.Compiled);
        }
    }

}
