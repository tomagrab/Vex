using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Vex.Services
{
    public class Auth0Service : IAuth0Service
    {
        private readonly HttpClient _httpClient;
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _domain;

        public Auth0Service(HttpClient httpClient, string clientId, string clientSecret, string domain)
        {
            _httpClient = httpClient;
            _clientId = clientId;
            _clientSecret = clientSecret;
            _domain = domain;
        }

        public async Task<string> GetAuth0TokenAsync()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"https://{_domain}/oauth/token");

            var content = new StringContent($"{{\"client_id\":\"{_clientId}\",\"client_secret\":\"{_clientSecret}\",\"audience\":\"https://{_domain}/api/v2/\",\"grant_type\":\"client_credentials\"}}", Encoding.UTF8, "application/json");

            request.Content = content;

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonSerializer.Deserialize<Auth0TokenResponse>(responseBody);

            if (tokenResponse == null)
            {
                throw new Exception("Failed to deserialize token response.");
            }

            return tokenResponse.AccessToken;
        }

        public async Task<string> GetUsersAsync()
        {
            var token = await GetAuth0TokenAsync();

            var request = new HttpRequestMessage(HttpMethod.Get, $"https://{_domain}/api/v2/users");

            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("Authorization", $"Bearer {token}");

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetUserByIdAsync(string userId)
        {
            var token = await GetAuth0TokenAsync();

            var request = new HttpRequestMessage(HttpMethod.Get, $"https://{_domain}/api/v2/users/{userId}");

            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("Authorization", $"Bearer {token}");

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetUserInfoAsync()
        {
            var token = await GetAuth0TokenAsync();

            var request = new HttpRequestMessage(HttpMethod.Get, $"https://{_domain}/userinfo");

            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("Authorization", $"Bearer {token}");

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        private class Auth0TokenResponse
        {
            [JsonPropertyName("access_token")]
            public required string AccessToken { get; set; }

            [JsonPropertyName("token_type")]
            public required string TokenType { get; set; }
        }
    }
}