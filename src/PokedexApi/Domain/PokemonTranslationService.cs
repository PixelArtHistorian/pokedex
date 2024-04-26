using Ardalis.Result;
using PokedexApi.Domain.Interfaces;
using PokedexApi.Domain.Models;

namespace PokedexApi.Domain
{
    public class PokemonTranslationService: IPokemonInformationService
    {
        IPokemonInformationService _pokemonInformationService { get; set; }
        public PokemonTranslationService() 
        { 
            
        }


        Task<Result<PokemonInformation>> IPokemonInformationService.GetPokemonInformationAsync(string pokemonName)
        {
            throw new NotImplementedException();
        }
    }
}
