using FluentAssertions;
using PokedexApi.Domain;
using System.ComponentModel;

namespace PokedexApiTest
{
    [Trait("Unit","Validator")]
    public class PokemonNameValidatorTest
    {
        public PokemonNameValidator Sut { get; set; }
        public PokemonNameValidatorTest()
        {
            Sut = new();
        }

        [Theory]
        [InlineData("pikachu")]
        [InlineData("charmender")]
        [InlineData("bulbasaur")]
        public void Validate_ValidName_ReturnsTrue(string name)
        {
            var result = Sut.Validate(name);
            result.IsValid.Should().BeTrue();
        }

        [Theory]
        [InlineData("")]
        [InlineData("Pikachu")]
        [InlineData("Charmender")]
        [InlineData("Bulbasaur")]
        [InlineData("Pikachu1")]
        [InlineData("Charmender2")]
        [InlineData("Bulbasaur3")]
        [InlineData("pikachu1")]
        [InlineData("charmender2")]
        [InlineData("bulbasaur3")]
        public void Validate_InvalidName_Returnsfalse(string name)
        {
            var result = Sut.Validate(name);
            result.IsValid.Should().BeFalse();
        }

        [Theory]
        [InlineData("", "Pokemon name cannot be empty.")]
        [InlineData(" ", "Pokemon name cannot be empty.")]
        public void Validate_EmptyName_ReturnsFalse_WithAppropriateMessage(string name, string errorMessage) 
        {
            var result = Sut.Validate(name);
            result.IsValid.Should().BeFalse();
            result.Errors.Should()
                .HaveCount(1)
                .And
                .OnlyContain(m => m.ErrorMessage == errorMessage);
        }

        [Fact]
        public void Validate_Null_ThrowsArgumentNullException()
        {
            string nullString = null!;
            Action action = () => { Sut.Validate(nullString!); };

            action.Should().Throw<ArgumentNullException>();
        }
    }
}