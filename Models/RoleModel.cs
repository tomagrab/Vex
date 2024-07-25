using System.Text.Json.Serialization;

namespace Vex.Models
{
    public class RoleModel
    {
        [JsonPropertyName("id")]
        public required string Id { get; set; }

        [JsonPropertyName("name")]
        public required string Name { get; set; }
    }
}