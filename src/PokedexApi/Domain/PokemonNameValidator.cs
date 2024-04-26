using FluentValidation;
using System.Text.RegularExpressions;

namespace PokedexApi.Domain
{
    public class PokemonNameValidator : AbstractValidator<string>
    {
        public PokemonNameValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(name => name)
                .NotEmpty()
                .WithMessage("Pokemon name cannot be empty.")
                .Must(BeValidPokemonName)
                .WithMessage("Invalid Pokemon name, pokemon names must be lowercase and not include any number or special character.");
        }

        private bool BeValidPokemonName(string name)
        {
            return Regex.IsMatch(name, "^[a-z]+$");
        }
    }
}
