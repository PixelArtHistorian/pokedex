using System.Text.Json.Serialization;

namespace PokedexApi.Infrastructure.Response
{
    public class TranslationResponse
    {
        [JsonPropertyName("contents")]
        public Contents Contents { get; set; } = new();
    }

    public class Contents
    {
        [JsonPropertyName("translated")]
        public string Translated { get; set; } = string.Empty;
    }
}
