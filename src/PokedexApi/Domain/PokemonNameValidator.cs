using FluentValidation;
using System.Text.RegularExpressions;

namespace PokedexApi.Domain
{
    public class PokemonNameValidator : AbstractValidator<string>
    {
        public PokemonNameValidator()
        {
            RuleFor(name => name)
                .NotEmpty().WithMessage("Pokemon name cannot be empty.")
                .Must(BeARealPokemon).WithMessage("Invalid Pokemon name.");
        }

        private bool BeARealPokemon(string name)
        {
            return Regex.IsMatch(name, "^[a-zA-Z]+$");
        }
    }
}
