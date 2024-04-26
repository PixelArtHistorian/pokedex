using AutoFixture;
using FluentAssertions;
using PokedexApi.Domain;
using PokedexApi.Infrastructure.DTO;
using PokedexApiTest.Helpers;
using System.Text.RegularExpressions;


namespace PokedexApiTest
{
    [Trait("Category", "Mapper")]
    public class PokemonMapperTest
    {
        Fixture Fixture { get; set; }
        PokemonMapper Sut { get; set; }
        public PokemonMapperTest()
        {
            Sut = new();
            Fixture = new();
        }

        [Fact]
        public void Mapper_Maps_PokemonResponse()
        {
            string pokemonName = "mewtwo";
            string description = "It was created by a scientist after years of horrific gene splicing and DNA engineering experiments.";
            string habitatName = "rare";
            bool isLegendary = true;
            string language = "en";
            //Arrange
            PokemonResponse response = PokemonResponseFactory.CreatePokemonResponse(pokemonName, description, habitatName, isLegendary, language);

            //Act
            var result = Sut.Map(response);
            //Assert
            result.Name.Should().Be(pokemonName);
            result.Description.Should().Be(description);
            result.Habitat.Should().Be(habitatName);
            result.IsLegendary.Should().Be(isLegendary);

        }

        [Fact]
        public void Mapper_Removes_WhiteSpaceCharacters_From_Description()
        {
            string pokemonName = "mewtwo";
            string description = "It was created by\na scientist after\nyears of horrific\fgene splicing and\nDNA engineering\nexperiments.";
            string habitatName = "rare";
            bool isLegendary = true;
            string language = "en";

            string expectedDescription = "It was created by a scientist after years of horrific gene splicing and DNA engineering experiments.";

            //Arrange
            PokemonResponse response = PokemonResponseFactory.CreatePokemonResponse(pokemonName, description, habitatName, isLegendary, language);
            //Act
            var result = Sut.Map(response);
            //Assert
            result.Description.Should().Be(expectedDescription);
        }

        [Theory]
        [InlineData("mewtwo", "Dieses Pokémon ist das Resultat eines jahrelangen\nund skrupellosen Experimentes.", "rare", true, "de")]
        [InlineData("mewtwo", "한 과학자가 몇 년에 걸쳐\n무서운 유전자의 연구를\n계속한 결과 탄생했다.", "rare", true, "ko")]
        public void Mapper_Maps_PokemonResponse_WithPlaceholder_WhenEnglishNotAvailable(
            string pokemonName,
            string description,
            string habitatName,
            bool isLegendary,
            string language)
        {
            //Arrange
            PokemonResponse response = PokemonResponseFactory.CreatePokemonResponse(pokemonName, description, habitatName, isLegendary, language);
            //Act
            var result = Sut.Map(response);
            //Assert
            result.Name.Should().Be(pokemonName);
            result.Description.Should().Be("No english description available");
            result.Habitat.Should().Be(habitatName);
            result.IsLegendary.Should().Be(isLegendary);

        }
    }
}
