using FluentValidation;
using Microsoft.VisualBasic;
using PokedexApi.Domain.Interfaces;
using PokedexApi.Domain.Models;
using PokedexApi.Infrastructure.DTO;
using System.Text;
using System.Text.Json;

namespace PokedexApi.Domain
{
    public class PokemonInformationService : IPokemonInformationService
    {
        private IHttpClientFactory _httpClientFactory;
        private IValidator<string> _validator;
        private IMapper<PokemonResponse, PokemonInformation> _mapper;
        private ILogger _logger;

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
            _logger.LogDebug("Processing equest {@pokemonName}", pokemonName);

            var validationResult = _validator.Validate(pokemonName);
            if (!validationResult.IsValid)
            {
                _logger.LogDebug("Pokemon name is not valid {@pokemonName}", pokemonName);
                return Results.ValidationProblem(validationResult.ToDictionary());
            }

            try
            {
                var client = _httpClientFactory.CreateClient();
                StringBuilder builder = new StringBuilder();
                var url = builder
                    .Append("https://pokeapi.co/api/v2/")
                    .Append("pokemon-species/")
                    .Append(pokemonName)
                    .ToString();

                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogDebug("Could not find pokemon information {@pokemonName}", pokemonName);
                    return Results.NotFound("Not Found");
                }

                var pokemonResponse = JsonSerializer.Deserialize<PokemonResponse>(await response.Content.ReadAsStringAsync());

                if(pokemonResponse == null)
                {
                    _logger.LogDebug("Could not parse response {@pokemonName}", pokemonName);
                    return Results.UnprocessableEntity();
                }

                var pokemonInformation = _mapper.Map(pokemonResponse);

                _logger.LogDebug("Found pokemon information {@pokemonInformation}", pokemonInformation);
                return Results.Ok(pokemonInformation);
            }
            catch (Exception)
            {
                return Results.Problem();
            }
        }
    }
}
