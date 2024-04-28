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

            var expectedResult = Result.Success(pokemonInfo with { Description = translation.Contents.Translated });
            
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
            result.Value.Description.Should().Be(translation.Contents.Translated);
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

            var expectedResult = Result.Success(pokemonInfo with { Description = translation.Contents.Translated });

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
            result.Value.Description.Should().Be(translation.Contents.Translated);
            result.Value.Habitat.Should().Be(pokemonInfo.Habitat);
            result.Value.IsLegendary.Should().Be(pokemonInfo.IsLegendary);
            MockTranslationClient.Verify(
                v => v.TranslateTextAsync(endpoint, pokemonInfo.Description),
                Times.Exactly(1));
        }

        [Theory]
        [MemberData(nameof(FailureTestData))]
        public async Task GetPokemonInformationTranslationAsync_ReturnsFailure_WhenPokemonInformationServiceFails(Result<PokemonInformation> failure)
        {
            //Arrange
            string pokemonName = "bulbasaur";
            MockInfoService
                .Setup(s => s.GetPokemonInformationAsync(It.IsAny<string>()))
                .ReturnsAsync(failure);
            //Act
            var res = await Sut.GetPokemonInformationTranslationAsync(pokemonName);
            //Assert
            res.IsSuccess.Should().Be(failure.IsSuccess);
            res.Status.Should().Be(failure.Status);
            res.Value.Should().Be(failure.Value);
        }

        public static IEnumerable<object[]> FailureTestData()
        {

            yield return new object[] { Result<PokemonInformation>.Invalid(new ValidationError[]
            {
                new ValidationError("Error1"),
                new ValidationError("Error2"),
            }) };
            yield return new object[] { Result<PokemonInformation>.NotFound() };
            yield return new object[] { Result<PokemonInformation>.Error() };
        }

        [Fact]
        public async Task GetPokemonInformationTranslationAsync_ReturnsUntranslatedInfo_WhenTranslationFails()
        {
            //Arrange
            var expectedResult = Fixture.Create<PokemonInformation>();
            var httpResponse = HttpResponseFactory
                .CreateMockResponse(
                    System.Net.HttpStatusCode.NotFound,
                    string.Empty);

            MockInfoService
                .Setup(s => s.GetPokemonInformationAsync(It.IsAny<string>()))
                .ReturnsAsync(Result.Success(expectedResult));
            MockTranslationClient
                .Setup(s => s.TranslateTextAsync(It.IsAny<string>(), expectedResult.Description))
                .ReturnsAsync(httpResponse);

            //Act
            var result = await Sut.GetPokemonInformationTranslationAsync(expectedResult.Name);
            //Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Name.Should().Be(expectedResult.Name);
            result.Value.Description.Should().Be(expectedResult.Description);
            result.Value.Habitat.Should().Be(expectedResult.Habitat);
            result.Value.IsLegendary.Should().Be(expectedResult.IsLegendary);
        }

        [Fact]
        public async Task GetPokemonInformationTranslationAsync_ReturnsUntranslatedInfo_WhenTranslationClientThrowsException()
        {
            //Arrange
            var expectedResult = Fixture.Create<PokemonInformation>();

            MockInfoService
                .Setup(s => s.GetPokemonInformationAsync(It.IsAny<string>()))
                .ReturnsAsync(Result.Success(expectedResult));
            MockTranslationClient
                .Setup(s => s.TranslateTextAsync(It.IsAny<string>(), expectedResult.Description))
                .ThrowsAsync(new Exception());

            //Act
            var result = await Sut.GetPokemonInformationTranslationAsync(expectedResult.Name);
            //Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Name.Should().Be(expectedResult.Name);
            result.Value.Description.Should().Be(expectedResult.Description);
            result.Value.Habitat.Should().Be(expectedResult.Habitat);
            result.Value.IsLegendary.Should().Be(expectedResult.IsLegendary);
        }

        [Fact]
        public async Task GetPokemonInformationTranslationAsync_ReturnsUntranslatedInfo_WhenTranslationParsingFails()
        {
            //Arrange
            var expectedResult = Fixture.Create<PokemonInformation>();
            var httpResponse = HttpResponseFactory
                .CreateMockResponse(
                    System.Net.HttpStatusCode.OK,
                    string.Empty);

            MockInfoService
                .Setup(s => s.GetPokemonInformationAsync(It.IsAny<string>()))
                .ReturnsAsync(Result.Success(expectedResult));
            MockTranslationClient
                .Setup(s => s.TranslateTextAsync(It.IsAny<string>(), expectedResult.Description))
                .ReturnsAsync(httpResponse);

            //Act
            var result = await Sut.GetPokemonInformationTranslationAsync(expectedResult.Name);
            //Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Name.Should().Be(expectedResult.Name);
            result.Value.Description.Should().Be(expectedResult.Description);
            result.Value.Habitat.Should().Be(expectedResult.Habitat);
            result.Value.IsLegendary.Should().Be(expectedResult.IsLegendary);
        }
    }
}
