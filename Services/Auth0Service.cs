using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using Vex.Models;

namespace Vex.Services
{
    public class Auth0Service : IAuth0Service
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private ClaimsPrincipal? _cachedUser;
        private UserModel? _cachedUserModel;

        private HttpContext _httpContext => new HttpContextAccessor().HttpContext ?? throw new Exception("HttpContext is not available.");
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _domain;

        public Auth0Service(HttpClient httpClient, AuthenticationStateProvider authenticationStateProvider, string clientId, string clientSecret, string domain)
        {
            _httpClient = httpClient;
            _authenticationStateProvider = authenticationStateProvider;
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
            var tokenResponse = JsonSerializer.Deserialize<Auth0TokenResponseModel>(responseBody);

            if (tokenResponse == null)
            {
                throw new Exception("Failed to deserialize token response.");
            }

            return tokenResponse.AccessToken;
        }

        public async Task<string> GetAuth0AccessTokenAsync()
        {
            if (_httpContext == null)
            {
                throw new Exception("HttpContext is not available.");
            }

            var accessToken = await _httpContext.GetTokenAsync("access_token");
            if (string.IsNullOrEmpty(accessToken))
            {
                throw new Exception("Access token not found.");
            }

            return accessToken;
        }

        public async Task<ClaimsPrincipal?> GetUserAsync()
        {
            if (_cachedUser == null)
            {
                var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
                _cachedUser = authState.User;
            }
            return _cachedUser;
        }

        public async Task<UserModel?> GetUserModelAsync()
        {
            if (_cachedUserModel == null)
            {
                var user = await GetUserAsync();

                if (user != null && user.Identity != null && user.Identity.IsAuthenticated)
                {
                    _cachedUserModel = new UserModel
                    {
                        Id = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value,
                        Name = user.Identity.Name,
                        Role = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value,
                        Picture = user.FindFirst("picture")?.Value,
                        Email = user.FindFirst(ClaimTypes.Email)?.Value ?? user?.FindFirst("emailClaim")?.Value
                    };
                }
            }
            return _cachedUserModel;
        }

        public async Task<List<RoleModel>> GetRolesAsync()
        {
            var token = await GetAuth0TokenAsync();

            var request = new HttpRequestMessage(HttpMethod.Get, $"https://{_domain}/api/v2/roles");

            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("Authorization", $"Bearer {token}");

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();

            var roles = JsonSerializer.Deserialize<List<RoleModel>>(responseBody);

            if (roles == null)
            {
                throw new Exception("Failed to deserialize roles response.");
            }

            return roles;
        }

        public async Task<List<UserModel>> GetUsersByRoleAsync(string roleId)
        {
            var token = await GetAuth0TokenAsync();

            var request = new HttpRequestMessage(HttpMethod.Get, $"https://{_domain}/api/v2/roles/{roleId}/users");

            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("Authorization", $"Bearer {token}");

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();

            var users = JsonSerializer.Deserialize<List<Auth0UserModel>>(responseBody);

            if (users == null)
            {
                throw new Exception("Failed to deserialize users response.");
            }

            return users.Select(u => new UserModel
            {
                Id = u.UserId,
                Name = u.Name,
                Picture = u.Picture,
                Email = u.Email
            }).ToList();
        }

        public async Task<List<UserModel>> GetUsersAsync()
        {
            var roles = await GetRolesAsync();

            var allUsers = new List<UserModel>();

            foreach (var role in roles)
            {
                var usersInRole = await GetUsersByRoleAsync(role.Id);
                foreach (var user in usersInRole)
                {
                    user.Role = role.Name;
                }
                allUsers.AddRange(usersInRole);
            }

            return allUsers;
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
            var accessToken = await GetAuth0AccessTokenAsync();

            var request = new HttpRequestMessage(HttpMethod.Get, $"https://{_domain}/userinfo");

            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("Authorization", $"Bearer {accessToken}");

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
    }
}