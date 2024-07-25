using System.Security.Claims;
using Vex.Models;

namespace Vex.Services
{
    public interface IAuth0Service
    {
        Task<string> GetAuth0TokenAsync();
        Task<string> GetAuth0AccessTokenAsync();
        Task<ClaimsPrincipal?> GetUserAsync();
        Task<UserModel?> GetUserModelAsync();
        Task<string> GetUsersAsync();
        Task<string> GetUserByIdAsync(string userId);
        Task<string> GetUserInfoAsync();
    }
}