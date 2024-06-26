﻿namespace PokedexApi.Domain.Models
{
    public record PokemonInformation
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Habitat { get; set; } = string.Empty;
        public bool IsLegendary { get; set; }
    }
}
