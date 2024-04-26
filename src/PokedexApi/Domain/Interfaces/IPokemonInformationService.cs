using Ardalis.Result;
using PokedexApi.Domain.Models;

namespace PokedexApi.Domain.Interfaces
{
    public interface IPokemonInformationService
    {
        Task<Result<PokemonInformation>> GetPokemonInformationAsync(string pokemonName);
    }
}
