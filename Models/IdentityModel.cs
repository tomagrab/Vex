using System.Text.Json.Serialization;

namespace Vex.Models
{
    public class IdentityModel
    {
        [JsonPropertyName("user_id")]
        public string? UserId { get; set; }
    }
}