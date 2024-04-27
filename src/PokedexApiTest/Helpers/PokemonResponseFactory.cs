using AutoFixture;
using PokedexApi.Infrastructure.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokedexApiTest.Helpers
{
    public static class PokemonResponseFactory
    {
        public static PokemonResponse CreatePokemonResponse()
        {
            var fixture = new Fixture();
            return fixture.Create<PokemonResponse>();
        }
        public static PokemonResponse CreatePokemonResponse(
            string pokemonName,
            string description,
            string habitatName,
            bool isLegendary,
            string language)
        {

            var fixture = new Fixture();
            Language lang = fixture.Build<Language>()
                .With(l => l.name, language)
                .Create();

            Flavor_Text_Entries entry = fixture.Build<Flavor_Text_Entries>()
                .With(f => f.flavor_text, description)
                .With(f => f.language, lang)
                .Create();

            var entries = new Flavor_Text_Entries[] { entry };

            PokemonResponse response = fixture.Build<PokemonResponse>()
                .With(p => p.name, pokemonName)
                .With(p => p.flavor_text_entries, entries)
                .With(p => p.habitat, new Habitat { name = habitatName })
                .With(p => p.is_legendary, isLegendary)
                .Create();
            return response;
        }
    }
}
