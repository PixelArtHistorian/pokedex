using Ardalis.Result;
using Microsoft.Extensions.Options;
using PokedexApi.Configuration;
using PokedexApi.Domain.Interfaces;
using PokedexApi.Domain.Models;
using PokedexApi.Infrastructure.Client;
using PokedexApi.Infrastructure.Response;

namespace PokedexApi.Domain
{
    public class PokemonTranslationService : IPokemonTranslationService
    {
        IPokemonInformationService _pokemonInformationService;
        ITranslationClient _translatorClient;
        TranslationServiceOptions _translationServiceOptions;
        ILogger _logger;
        public PokemonTranslationService(
            IPokemonInformationService pokemonInformationService,
            ITranslationClient translatorClient,
            IOptions<TranslationServiceOptions> translationServiceOptions,
            ILogger<PokemonTranslationService> logger)
        {
            _pokemonInformationService = pokemonInformationService;
            _translatorClient = translatorClient;
            _translationServiceOptions = translationServiceOptions.Value;
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
                var endpoint = PickTranslationEndpoint(pokemonInformation.Value);
                var response = await _translatorClient.TranslateTextAsync(endpoint, pokemonInformation.Value.Description);
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogDebug("Could not translate description {@pokemonInformation.Value}", pokemonInformation.Value);
                    return pokemonInformation;
                }

                var translationResponse = await response.Content.ReadFromJsonAsync<TranslationResponse>();
                if (translationResponse is null)
                {
                    _logger.LogDebug("Could not parse translation {@response}", response);
                    return pokemonInformation;
                }
                var translatedInformation = pokemonInformation.Value with { Description = translationResponse.Contents.Translated };

                return Result.Success(translatedInformation);
            }
            catch (Exception ex)
            {
                _logger.LogError("Unhandled exception {@ex}", ex);
                return pokemonInformation;
            }
        }

        private string PickTranslationEndpoint(PokemonInformation pokemonInformation)
        {
            if (pokemonInformation.Habitat == "cave" || pokemonInformation.IsLegendary)
            {
                return _translationServiceOptions.YodishEndpoint;
            }
            return _translationServiceOptions.ShakespeareanEndpoint;
        }
    }
}
