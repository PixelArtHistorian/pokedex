using Ardalis.Result;
using PokedexApi.Domain.Models;

namespace PokedexApi.Domain.Interfaces
{
    public interface IPokemonTranslationService
    {
        Task<Result<PokemonInformation>> GetPokemonInformationTranslationAsync(string pokemonName);
    }
}
