using System.Text.Json.Serialization;

namespace Vex.Models
{
    public class Auth0UserModel
    {
        [JsonPropertyName("user_id")]
        public string? UserId { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("email")]
        public string? Email { get; set; }

        [JsonPropertyName("picture")]
        public string? Picture { get; set; }

        [JsonPropertyName("http://schemas.microsoft.com/ws/2008/06/identity/claims/role")]
        public List<string>? Role { get; set; }

        [JsonPropertyName("identities")]
        public List<IdentityModel>? Identities { get; set; }
    }
}