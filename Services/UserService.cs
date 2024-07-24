using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using Vex.Models;

namespace Vex.Services
{
    public class UserService : IUserService
    {
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private ClaimsPrincipal? _cachedUser;
        private UserModel? _cachedUserModel;

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

        public async Task<UserModel?> GetUserModelAsync()
        {
            if (_cachedUserModel == null)
            {
                var user = await GetUserAsync();
                if (user != null && user.Identity != null && user.Identity.IsAuthenticated)
                {
                    _cachedUserModel = new UserModel
                    {
                        Id = user.FindFirst("user_id")?.Value,
                        Name = user.Identity.Name,
                        Role = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value,
                        Picture = user.FindFirst("picture")?.Value,
                        Email = user.FindFirst(ClaimTypes.Email)?.Value ?? user?.FindFirst("emailClaim")?.Value
                    };
                }
            }
            return _cachedUserModel;
        }
    }
}