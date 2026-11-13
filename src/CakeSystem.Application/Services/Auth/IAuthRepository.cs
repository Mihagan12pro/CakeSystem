using CakeSystem.Application.DTOs.Auth;
using CakeSystem.Domain;

namespace CakeSystem.Application.Services.Auth
{
    public interface IAuthRepository
    {
        Task<User> GetUserAsync(
            LoginUserDto userDto, 
            CancellationToken token);

        Task<bool> AddUserAsync(
            User user, 
            CancellationToken token);

        Task PutOrAddUserAsync(
            User user,
            CancellationToken token);
    }
}
