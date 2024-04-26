﻿using Ardalis.Result;
using FluentValidation;
using PokedexApi.Domain.Interfaces;
using PokedexApi.Domain.Models;
using PokedexApi.Infrastructure.Client;
using PokedexApi.Infrastructure.DTO;

namespace PokedexApi.Domain
{
    public class PokemonInformationService : IPokemonInformationService
    {
        private IPokemonSpeciesClient _pokemonClient;
        private IValidator<string> _validator;
        private IMapper<PokemonResponse, PokemonInformation> _mapper;
        private ILogger _logger;

        public PokemonInformationService(
            IPokemonSpeciesClient pokemonSpeciesClient,
            IValidator<string> validator,
            IMapper<PokemonResponse, PokemonInformation> mapper,
            ILogger<PokemonInformationService> logger)
        {
            _pokemonClient = pokemonSpeciesClient;
            _mapper = mapper;
            _validator = validator;
            _logger = logger;
        }

        public async Task<Result<PokemonInformation>> GetPokemonInformationAsync(string pokemonName)
        {
            if (pokemonName is null)
            {
                throw new ArgumentNullException(nameof(pokemonName));
            }

            var validationResult = await _validator.ValidateAsync(pokemonName);
            if (!validationResult.IsValid)
            {
                return Result.Invalid(validationResult.Errors
                    .Select(x => new ValidationError(x.ErrorMessage))
                    .ToArray());
            }

            try
            {
                var response = await _pokemonClient.GetPokemonSpeciesInformationAsync(pokemonName);

                if (!response.IsSuccessStatusCode)
                {
                    return Result.NotFound();
                }

                var pokemonResponse = await response.Content.ReadFromJsonAsync<PokemonResponse>();
                var pokemonInformation = _mapper.Map(pokemonResponse!);

                return Result.Success(pokemonInformation);
            }
            catch (Exception ex)
            {
                _logger.LogError("Unhandled exception {@ex}", ex);
                return Result.Error();
            }
        }
    }
}
