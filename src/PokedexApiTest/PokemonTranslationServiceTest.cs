using Ardalis.Result;
using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using PokedexApi.Configuration;
using PokedexApi.Domain;
using PokedexApi.Domain.Interfaces;
using PokedexApi.Domain.Models;
using PokedexApi.Infrastructure.Client;
using PokedexApiTest.Helpers;

namespace PokedexApiTest
{
    [Trait("Category", "PokemonTranslationService")]
    public class PokemonTranslationServiceTest
    {
        Fixture Fixture { get; set; }
        Mock<IPokemonInformationService> MockInfoService { get; set; }
        Mock<ITranslationClient> MockTranslationClient { get; set; }
        IOptions<TranslationServiceOptions> TranslationServiceOptions { get; set; }
        Mock<ILogger<PokemonTranslationService>> MockLogger {get;set;}
        PokemonTranslationService Sut { get; set;}
        public PokemonTranslationServiceTest()
        {
            Fixture = new();
            MockInfoService = new();
            MockTranslationClient = new();
            var options = new TranslationServiceOptions
            {
                YodishEndpoint = "yoda",
                ShakespeareanEndpoint = "shakespeare"
            };
            TranslationServiceOptions = Options.Create<TranslationServiceOptions>(options);

            MockLogger = new();
            Sut = new(
                MockInfoService.Object, 
                MockTranslationClient.Object, 
                TranslationServiceOptions, 
                MockLogger.Object);
        }

        [Fact]
        public async Task GetPokemonInformationTranslationAsync_GetsATranslation()
        {
            //Arrange
            var pokemonInfo = Fixture.Create<PokemonInformation>();
            var translation = TranslationResponseFactory.CreateTranslationResponse();
            var httpResponse = HttpResponseFactory
                .CreateMockResponse(
                System.Net.HttpStatusCode.OK,
                translation);

            var expectedResult = Result.Success(pokemonInfo with { Description = translation.contents.translated });
            
            MockInfoService
                .Setup(s => s.GetPokemonInformationAsync(It.IsAny<string>()))
                .ReturnsAsync(Result.Success(pokemonInfo));
            MockTranslationClient
                .Setup(s => s.TranslateTextAsync(It.IsAny<string>(), pokemonInfo.Description))
                .ReturnsAsync(httpResponse);

            //Act
            var result = await Sut.GetPokemonInformationTranslationAsync(pokemonInfo.Name);
            //Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Name.Should().Be(pokemonInfo.Name);
            result.Value.Description.Should().Be(translation.contents.translated);
            result.Value.Habitat.Should().Be(pokemonInfo.Habitat);
            result.Value.IsLegendary.Should().Be(pokemonInfo.IsLegendary);
        }

        [Theory]
        [InlineData("cave", true, "yoda")]
        [InlineData("cave", false, "yoda")]
        [InlineData("not-cave", true, "yoda")]
        [InlineData("not-cave", false, "shakespeare")]
        public async Task GetPokemonInformationTranslationAsync_GetsCorrectTranslation(string habitat, bool isLegend, string endpoint)
        {
            //Arrange
            var pokemonInfo = Fixture
                .Build<PokemonInformation>()
                .With(x => x.Habitat, habitat)
                .With(x => x.IsLegendary, isLegend)
                .Create();
            var translation = TranslationResponseFactory.CreateTranslationResponse();
            var httpResponse = HttpResponseFactory
                .CreateMockResponse(
                System.Net.HttpStatusCode.OK,
                translation);

            var expectedResult = Result.Success(pokemonInfo with { Description = translation.contents.translated });

            MockInfoService
                .Setup(s => s.GetPokemonInformationAsync(It.IsAny<string>()))
                .ReturnsAsync(Result.Success(pokemonInfo));
            MockTranslationClient
                .Setup(s => s.TranslateTextAsync(endpoint, pokemonInfo.Description))
                .ReturnsAsync(httpResponse);

            //Act
            var result = await Sut.GetPokemonInformationTranslationAsync(pokemonInfo.Name);
            //Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Name.Should().Be(pokemonInfo.Name);
            result.Value.Description.Should().Be(translation.contents.translated);
            result.Value.Habitat.Should().Be(pokemonInfo.Habitat);
            result.Value.IsLegendary.Should().Be(pokemonInfo.IsLegendary);
            MockTranslationClient.Verify(
                v => v.TranslateTextAsync(endpoint, pokemonInfo.Description),
                Times.Exactly(1));
        }
    }
}
