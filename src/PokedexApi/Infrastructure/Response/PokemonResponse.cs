using System.Text.Json.Serialization;

namespace PokedexApi.Infrastructure.Response
{

    public class PokemonResponse
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        [JsonPropertyName("flavor_text_entries")]
        public IEnumerable<FlavorTextEntries> FlavorTextEntries { get; set; } = Array.Empty<FlavorTextEntries>();
        [JsonPropertyName("habitat")]
        public Habitat? Habitat { get; set; }
        [JsonPropertyName("is_legendary")]
        public bool IsLegendary { get; set; }
    }

    public class Habitat
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
    }

    public class FlavorTextEntries
    {
        [JsonPropertyName("flavor_text")]
        public string FlavorText { get; set; } = string.Empty;
        [JsonPropertyName("language")]
        public Language Language { get; set; } = new Language();
    }

    public class Language
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
    }
}
