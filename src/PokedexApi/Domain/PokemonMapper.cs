using PokedexApi.Domain.Interfaces;
using PokedexApi.Domain.Models;
using PokedexApi.Infrastructure;
using System.Text.RegularExpressions;

namespace PokedexApi.Domain
{
    public class PokemonMapper : IMapper<PokemonResponse, PokemonInformation>
    {
        public PokemonInformation Map(PokemonResponse source)
        {
            return new PokemonInformation
            {
                Name = source.name,
                Description = source.flavor_text_entries
                    .First(e => e.language.name.Equals("en")).flavor_text
                    .Replace("\n", " ")
                    .Replace("\f", " "),
                Habitat = source.habitat.name,
                IsLegendary = source.is_legendary,
            };
        }
    }

}
