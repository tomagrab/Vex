// IUserService.cs
using System.Security.Claims;
using Vex.Models;

namespace Vex.Services
{
    public interface IUserService
    {
        Task<ClaimsPrincipal?> GetUserAsync();
        Task<UserModel?> GetUserModelAsync();
    }
}