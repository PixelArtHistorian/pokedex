using PokedexApi.Domain.Models;

namespace PokedexApi.Domain.Interfaces
{
    public interface IPokemonInformationService
    {
        Task<PokemonInformation> GetPokemonInformationAsync(string pokemonName);
    }
}
