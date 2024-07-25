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
        Task<List<UserModel>> GetUsersAsync();
        Task<List<UserModel>> GetUsersByRoleAsync(string roleId);
        Task<List<RoleModel>> GetRolesAsync();
        Task<string> GetUserByIdAsync(string userId);
        Task<string> GetUserInfoAsync();
    }
}