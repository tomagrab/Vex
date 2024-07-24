using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace Vex.Services
{
    public class UserService : IUserService
    {
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private ClaimsPrincipal? _cachedUser;

        public UserService(AuthenticationStateProvider authenticationStateProvider)
        {
            _authenticationStateProvider = authenticationStateProvider;
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

        public async Task<string?> GetUserNameAsync()
        {
            var user = await GetUserAsync();
            return user?.Identity?.Name;
        }

        public async Task<string?> GetUserPictureAsync()
        {
            var user = await GetUserAsync();
            return user?.FindFirst("picture")?.Value;
        }
    }
}