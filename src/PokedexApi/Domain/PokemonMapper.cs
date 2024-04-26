using PokedexApi.Domain.Interfaces;
using PokedexApi.Domain.Models;
using PokedexApi.Infrastructure.DTO;
using System.Text.RegularExpressions;

namespace PokedexApi.Domain
{
    public class PokemonMapper : IMapper<PokemonResponse, PokemonInformation>
    {
        private readonly string placeholderDescription = "No english description available";
        public PokemonInformation Map(PokemonResponse source)
        {
            return new PokemonInformation
            {
                Name = source.name,
                Description = GetPokemonEnglishDescription(source.flavor_text_entries),
                Habitat = source.habitat.name,
                IsLegendary = source.is_legendary,
            };
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
