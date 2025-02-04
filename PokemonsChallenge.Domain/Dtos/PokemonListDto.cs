using System.Text.Json.Serialization;

namespace PokemonsChallenge.Domain.Dtos
{
    public class PokemonListDto
    {
        [JsonPropertyName("results")]
        public List<PokemonResultDto> Results { get; set; } = new();
    }

    public class PokemonResultDto
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }
    }
}
