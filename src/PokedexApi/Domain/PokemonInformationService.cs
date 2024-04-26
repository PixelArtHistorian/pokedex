using FluentValidation;
using PokedexApi.Domain.Interfaces;
using PokedexApi.Domain.Models;
using PokedexApi.Infrastructure.DTO;
using System.Text;
using System.Text.Json;
using static System.Net.WebRequestMethods;

namespace PokedexApi.Domain
{
    public class PokemonInformationService : IPokemonInformationService
    {
        private IHttpClientFactory _httpClientFactory;
        private IValidator<string> _validator;
        private IMapper<PokemonResponse, PokemonInformation> _mapper;
        private ILogger _logger;

        private readonly string _baseUri = "https://pokeapi.co/api/v2/";

        public PokemonInformationService(
            IHttpClientFactory httpClientFactory,
            IValidator<string> validator,
            IMapper<PokemonResponse, PokemonInformation> mapper,
            ILogger<PokemonInformationService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _mapper = mapper;
            _validator = validator;
            _logger = logger;
        }

        public async Task<IResult> GetPokemonInformationAsync(string pokemonName)
        {
            if (pokemonName is null)
            {
                throw new ArgumentNullException(nameof(pokemonName));
            }

            _logger.LogDebug("Processing equest {@pokemonName}", pokemonName);

            var validationResult = _validator.Validate(pokemonName);
            if (!validationResult.IsValid)
            {
                _logger.LogDebug("Pokemon name is not valid {@pokemonName}", pokemonName);
                return Results.ValidationProblem(validationResult.ToDictionary());
            }

            try
            {
                var client = CreatePokemonClient();

                var response = await client.GetAsync($"pokemon-species/{pokemonName}");

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogDebug("Could not find pokemon information {@pokemonName}", pokemonName);
                    return Results.NotFound("Not Found");
                }

                var pokemonResponse = await response.Content.ReadFromJsonAsync<PokemonResponse>();

                var pokemonInformation = _mapper.Map(pokemonResponse!);

                _logger.LogDebug("Found pokemon information {@pokemonInformation}", pokemonInformation);
                return Results.Ok(pokemonInformation);
            }
            catch (Exception ex)
            {
                _logger.LogError("Unhandled exception {@ex}", ex);
                return Results.Problem();
            }
        }

        private HttpClient CreatePokemonClient()
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_baseUri);
            return client;
        }
        
    }
}
