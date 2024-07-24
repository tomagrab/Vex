// IUserService.cs
using System.Security.Claims;

namespace Vex.Services
{
    public interface IUserService
    {
        Task<ClaimsPrincipal?> GetUserAsync();
        Task<string?> GetUserNameAsync();
        Task<string?> GetUserPictureAsync();
    }
}