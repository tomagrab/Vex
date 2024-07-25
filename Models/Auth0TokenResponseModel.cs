using System.Text.Json.Serialization;

namespace Vex.Models
{
    public class Auth0TokenResponseModel
    {
        [JsonPropertyName("access_token")]
        public required string AccessToken { get; set; }

        [JsonPropertyName("token_type")]
        public required string TokenType { get; set; }
    }
}