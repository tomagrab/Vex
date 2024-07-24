namespace Vex.Services
{
    public interface IAuth0Service
    {
        Task<string> GetAuth0TokenAsync();
        Task<string> GetUsersAsync();
        Task<string> GetUserByIdAsync(string userId);
        Task<string> GetUserInfoAsync();
    }
}