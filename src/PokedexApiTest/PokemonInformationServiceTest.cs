using AutoFixture;
using FluentAssertions;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using PokedexApi.Domain;
using PokedexApi.Domain.Interfaces;
using PokedexApi.Domain.Models;
using PokedexApi.Infrastructure.Client;
using PokedexApi.Infrastructure.Response;
using PokedexApiTest.Helpers;
using System.Net;
using System.Net.Mail;
using System.Text.Json;

namespace PokedexApiTest
{
    [Trait("Category", "PokemonInformationService")]
    public class PokemonInformationServiceTest
    {
        Fixture Fixture { get; set; }
        Mock<IPokemonSpeciesClient> MockClient { get; set; }
        InlineValidator<string> MockValidator { get; set; }
        Mock<IMapper<PokemonResponse, PokemonInformation>> MockMapper { get; set; }
        Mock<ILogger<PokemonInformationService>> MockLogger { get; set; }
        PokemonInformationService Sut { get; set; }
        public PokemonInformationServiceTest()
        {
            Fixture = new Fixture();
            MockClient = new();
            MockValidator = new();
            MockMapper = new();
            MockLogger = new();
            Sut = new PokemonInformationService(
                    MockClient.Object,
                    MockValidator,
                    MockMapper.Object,
                    MockLogger.Object
                );
        }

        [Fact]
        public async Task GetPokemonInformationAsync_ReturnsOk()
        {
            //Arrange
            var pokemonName = "squirtle";
            HttpResponseMessage httpResponseMessage = HttpResponseFactory
                .CreateMockResponse(
                    HttpStatusCode.OK, 
                    PokemonResponseFactory.CreatePokemonResponse());

            var pokemonInformation = Fixture.Create<PokemonInformation>();

            MockClient
                .Setup(s => s.GetPokemonSpeciesInformationAsync(pokemonName))
                .ReturnsAsync(httpResponseMessage);
            MockValidator
                .RuleFor(x => x)
                .Must(x => true);
            MockMapper
                .Setup(s => s.Map(It.IsAny<PokemonResponse>()))
                .Returns(pokemonInformation);

            //Act
            var result = await Sut.GetPokemonInformationAsync(pokemonName);
            //Assert
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task GetPokemonInformationAsync_ThrowsArgumentNullException_WhenPokemonNameIsNull()
        {
            //Arrange
            string pokemonName = null!;
            //Act
            var action = async () =>
            {
                await Sut.GetPokemonInformationAsync(pokemonName);
            };
            //Assert
            await action.Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task GetPokemonInformationAsync_ReturnsValidationProblem_WhenValidationFails()
        {
            //Arrange
            string pokemonName = "NotValid123";

            MockValidator
                .RuleFor(x => x)
                .Must(x => false);

            //Act
            var result = await Sut.GetPokemonInformationAsync(pokemonName);
            //Assert
            result.IsSuccess.Should().BeFalse();
            result.Status.Should().Be(Ardalis.Result.ResultStatus.Invalid);
        }

        [Fact]
        public async Task GetPokemonInformationAsync_ReturnsNotFound_WhenNoInformationIsFound()
        {
            //Arrange
            var pokemonName = "squirtle";
            HttpResponseMessage httpResponseMessage = HttpResponseFactory
                .CreateMockResponse(
                    HttpStatusCode.NotFound,
                    PokemonResponseFactory.CreatePokemonResponse());


            var pokemonInformation = Fixture.Create<PokemonInformation>();

            MockClient
                .Setup(s => s.GetPokemonSpeciesInformationAsync(pokemonName))
                .ReturnsAsync(httpResponseMessage);
            MockValidator
                .RuleFor(x => x)
                .Must(x => true);
            //Act
            var result = await Sut.GetPokemonInformationAsync(pokemonName);
            //Assert
            result.IsSuccess.Should().BeFalse();
            result.Status.Should().Be(Ardalis.Result.ResultStatus.NotFound);
        }

        [Fact]
        public async Task GetPokemonInformationAsync_ReturnsProblem_WhenDeserializationFails()
        {
            //Arrange
            var pokemonName = "squirtle";
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK);

            var pokemonInformation = Fixture.Create<PokemonInformation>();

            MockClient
                .Setup(s => s.GetPokemonSpeciesInformationAsync(pokemonName))
                .ReturnsAsync(httpResponseMessage);
            MockValidator
                .RuleFor(x => x)
                .Must(x => true);
            //Act
            var result = await Sut.GetPokemonInformationAsync(pokemonName);
            //Assert
            result.IsSuccess.Should().BeFalse();
            result.Status.Should().Be(Ardalis.Result.ResultStatus.Error);
        }
    }
}
