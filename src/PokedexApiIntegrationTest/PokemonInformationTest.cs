using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using PokedexApi.Domain.Models;
using PokedexApiIntegrationTest.Helpers;
using System.Net;

namespace PokedexApiIntegrationTest
{
    [Trait("Integration", "Pokemon")]
    public class PokemonInformationTest
    {
        [Fact]
        public async Task Pokemon_ReturnsPokemonInformation()
        {
            //Arrange
            await using var application = new WebApplicationFactory<Program>();
            using var client = application.CreateClient();
            var expectedResult = await ResultFactory.GetPokemonInformationFromPokemonApiAsync();
            //Act
            var result = await client.GetAsync($"/pokemon/{expectedResult.Name}");
            var content = await result.Content.ReadFromJsonAsync<PokemonInformation>();
            //Assert
            result.StatusCode.Should().Be(HttpStatusCode.OK);
            content.Should().NotBeNull();
            content?.Name.Should().Be(expectedResult.Name);
            content?.Description.Should().Be(expectedResult.Description);
            content?.Habitat.Should().Be(expectedResult.Habitat);
            content?.IsLegendary.Should().Be(expectedResult.IsLegendary);
        }
        [Theory]
        [InlineData("ABC")]
        [InlineData("abc123")]
        [InlineData("ABC123")]
        public async Task Pokemon_ReturnsBadRequest_WhenPokemonNameIsInvalid(string pokemonName)
        {
            //Arrange
            await using var application = new WebApplicationFactory<Program>();
            using var client = application.CreateClient();
            //Act
            var result = await client.GetAsync($"/pokemon/{pokemonName}");
            //Assert
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        [Theory]
        [InlineData("fakename")]
        public async Task Pokemon_ReturnsNotFound_WhenPokemonNameIsNotReal(string pokemonName)
        {
            //Arrange
            await using var application = new WebApplicationFactory<Program>();
            using var client = application.CreateClient();
            //Act
            var result = await client.GetAsync($"/pokemon/{pokemonName}");
            //Assert
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}