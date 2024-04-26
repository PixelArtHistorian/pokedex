using PokedexApi.Domain.Interfaces;

namespace PokedexApi.Domain
{
    public class PokemonTranslationService: IPokemonInformationService
    {
        IPokemonInformationService _pokemonInformationService { get; set; }
        public PokemonTranslationService() 
        { 
            
        }

        public Task<IResult> GetPokemonInformationAsync(string pokemonName)
        {
            throw new NotImplementedException();
        }
    }
}
