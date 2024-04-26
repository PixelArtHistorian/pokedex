using PokedexApi.Domain.Models;

namespace PokedexApi.Domain.Interfaces
{
    public interface IPokemonInformationService
    {
        Task<IResult> GetPokemonInformationAsync(string pokemonName);
    }
}
