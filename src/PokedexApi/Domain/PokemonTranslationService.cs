using Ardalis.Result;
using PokedexApi.Domain.Interfaces;
using PokedexApi.Domain.Models;

namespace PokedexApi.Domain
{
    public class PokemonTranslationService: IPokemonInformationService
    {
        IPokemonInformationService _pokemonInformationService { get; set; }
        ILogger _logger { get; set; }
        public PokemonTranslationService(IPokemonInformationService pokemonInformationService, ILogger logger)
        {
            _pokemonInformationService = pokemonInformationService;
            _logger = logger;
        }


        Task<Result<PokemonInformation>> IPokemonInformationService.GetPokemonInformationAsync(string pokemonName)
        {
            throw new NotImplementedException();
        }
    }
}
