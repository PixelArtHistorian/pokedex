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
                .With(l => l.Name, language)
                .Create();

            FlavorTextEntries entry = fixture.Build<FlavorTextEntries>()
                .With(f => f.FlavorText, description)
                .With(f => f.Language, lang)
                .Create();

            var entries = new FlavorTextEntries[] { entry };

            PokemonResponse response = fixture.Build<PokemonResponse>()
                .With(p => p.Name, pokemonName)
                .With(p => p.FlavorTextEntries, entries)
                .With(p => p.Habitat, new Habitat { Name = habitatName })
                .With(p => p.IsLegendary, isLegendary)
                .Create();
            return response;
        }
    }
}
