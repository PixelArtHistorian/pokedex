using Ardalis.Result;
using PokedexApi.Domain.Interfaces;
using PokedexApi.Domain.Models;
using PokedexApi.Infrastructure.Client;
using PokedexApi.Infrastructure.DTO;

namespace PokedexApi.Domain
{
    public class PokemonTranslationService: IPokemonTranslationService
    {
        IPokemonInformationService _pokemonInformationService;
        ITranslationClient _translatorClient;
        ILogger _logger;
        public PokemonTranslationService(
            IPokemonInformationService pokemonInformationService,  
            ITranslationClient translatorClient, 
            ILogger<PokemonTranslationService> logger)
        {
            _pokemonInformationService = pokemonInformationService;
            _translatorClient = translatorClient;
            _logger = logger;
        }

        public async Task<Result<PokemonInformation>> GetPokemonInformationTranslationAsync(string pokemonName)
        {
            var pokemonInformation = await _pokemonInformationService.GetPokemonInformationAsync(pokemonName);
            if (!pokemonInformation.IsSuccess)
            {
                return pokemonInformation;
            }

            try
            {
                var response = await _translatorClient.TranslateTextAsync(pokemonInformation.Value.Description);
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogDebug("Could not translate description {@pokemonInformation.Value}", pokemonInformation.Value);
                    return pokemonInformation;
                }
            
                var translationResponse = await response.Content.ReadFromJsonAsync<TranslationResponse>();
                if(translationResponse is null)
                {
                    _logger.LogDebug("Could not parse translation {@response}", response);
                    return pokemonInformation;
                }
                var translatedInformation = pokemonInformation.Value with { Description = translationResponse.contents.translated };     
            
                return Result.Success(translatedInformation);
            }
            catch (Exception ex)
            {
                _logger.LogError("Unhandled exception {@ex}", ex);
                return pokemonInformation;
            }
        }
    }
}
